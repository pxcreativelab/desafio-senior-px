using System;
using System.Security.Cryptography;
using System.Text;
using HubieTest.Dal.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HubieTest.Business.Security
{
    /// <summary>
    /// Password hashing (SHA-256) and JWT generation/validation (HS256).
    ///
    /// Fidelity note: the production Hubie validates the JWT with
    /// System.IdentityModel.Tokens.Jwt (Microsoft.IdentityModel). Here we use a
    /// small dependency-free HMAC-SHA256 implementation to keep the test
    /// self-contained. The flow (Bearer token in the Authorization header,
    /// sub/login/profile claims) is identical to the real project.
    /// </summary>
    public static class SecurityHelper
    {
        private static string Secret
        {
            get
            {
                // same key used by safety.cs (APPLICATION_API_KEY in Web.config)
                var s = ConfigReader.GetByKey("APPLICATION_API_KEY");
                return string.IsNullOrEmpty(s) ? "hubie-tech-test-default-key" : s;
            }
        }

        /// <summary>SHA-256 of the password as lowercase hex (matches the DB seed).</summary>
        public static string HashPassword(string password)
        {
            if (password == null) password = string.Empty;
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static string CreateToken(long userId, string login, string profile, string name, int validityHours = 8)
        {
            var header = new { alg = "HS256", typ = "JWT" };
            var payload = new
            {
                sub = userId,
                login = login,
                profile = profile,
                name = name,
                exp = ToUnix(DateTime.UtcNow.AddHours(validityHours))
            };

            string h = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header)));
            string p = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload)));
            string signature = Sign(h + "." + p);
            return h + "." + p + "." + signature;
        }

        /// <summary>Validates signature + expiration and returns the claims.</summary>
        public static bool TryValidate(string token, out TokenClaims claims)
        {
            claims = null;
            if (string.IsNullOrEmpty(token)) return false;

            var parts = token.Split('.');
            if (parts.Length != 3) return false;

            string expectedSig = Sign(parts[0] + "." + parts[1]);
            if (!FixedTimeEquals(expectedSig, parts[2])) return false;

            try
            {
                string json = Encoding.UTF8.GetString(Base64UrlDecode(parts[1]));
                JObject o = JObject.Parse(json);

                long exp = o.Value<long>("exp");
                if (ToUnix(DateTime.UtcNow) > exp) return false; // expired

                claims = new TokenClaims
                {
                    UserId = o.Value<long>("sub"),
                    Login = o.Value<string>("login"),
                    Profile = o.Value<string>("profile"),
                    Name = o.Value<string>("name")
                };
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region [ helpers ]

        private static string Sign(string data)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Secret)))
            {
                return Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes(data)));
            }
        }

        private static long ToUnix(DateTime dt)
        {
            return (long)(dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        private static string Base64UrlEncode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        private static byte[] Base64UrlDecode(string input)
        {
            string s = input.Replace('-', '+').Replace('_', '/');
            switch (s.Length % 4)
            {
                case 2: s += "=="; break;
                case 3: s += "="; break;
            }
            return Convert.FromBase64String(s);
        }

        private static bool FixedTimeEquals(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++) diff |= a[i] ^ b[i];
            return diff == 0;
        }

        #endregion
    }

    public class TokenClaims
    {
        public long UserId { get; set; }
        public string Login { get; set; }
        public string Profile { get; set; }
        public string Name { get; set; }
    }
}
