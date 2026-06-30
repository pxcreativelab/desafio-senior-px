using System.Web;
using HubieTest.Business.Security;

namespace HubieTest.Web.ashx
{
    /// <summary>
    /// Token validation (mirrors safety.cs in Hubie): reads the
    /// Authorization: Bearer &lt;jwt&gt; header, validates signature/expiration
    /// and exposes the logged-in user data.
    /// </summary>
    public class safety
    {
        public long currentUserId { get; set; }
        public string currentUserLogin { get; set; }
        public string currentUserProfile { get; set; }
        public string currentUserName { get; set; }
        public string currentToken { get; set; }
        public string tokenMessage { get; set; }

        public bool validate(HttpContext ctx)
        {
            tokenMessage = string.Empty;

            string auth = ctx.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(auth))
            {
                tokenMessage = "INVALID_TOKEN";
                return false;
            }

            currentToken = auth.Replace("Bearer ", "");

            TokenClaims claims;
            if (!SecurityHelper.TryValidate(currentToken, out claims))
            {
                tokenMessage = "INVALID_TOKEN";
                return false;
            }

            currentUserId = claims.UserId;
            currentUserLogin = claims.Login;
            currentUserProfile = claims.Profile;
            currentUserName = claims.Name;
            return true;
        }
    }
}
