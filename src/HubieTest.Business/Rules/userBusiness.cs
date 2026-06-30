using HubieTest.Business.Data;
using HubieTest.Business.Entities;
using HubieTest.Business.Security;
using HubieTest.Dal;

namespace HubieTest.Business
{
    /// <summary>
    /// Authentication rules. Mirrors the Hubie flow (auth generates the JWT and
    /// returns it in the X-User-Token header).
    /// IMPLEMENTED as a reference — no need to change it.
    /// </summary>
    public class userBusiness
    {
        private readonly userDB _db = new userDB();

        /// <param name="login">user login</param>
        /// <param name="password">plain-text password (compared by hash)</param>
        public AuthResult auth(string login, string password)
        {
            var result = new AuthResult();

            APP_USER u = _db.getByLogin(login);
            if (u == null)
            {
                result.STATUS = "USER_NOT_FOUND";
                return result;
            }

            string hash = SecurityHelper.HashPassword(password);
            if (u.USER_PASSWORD != hash)
            {
                result.STATUS = "INVALID_PASSWORD";
                return result;
            }

            result.STATUS = "OK";
            result.TOKEN = SecurityHelper.CreateToken(u.USER_ID, u.USER_LOGIN, u.USER_PROFILE, u.USER_NAME);
            result.USER_ID = u.USER_ID;
            result.USER_NAME = u.USER_NAME;
            result.USER_LOGIN = u.USER_LOGIN;
            result.USER_PROFILE = u.USER_PROFILE;
            result.USER_EMAIL = u.USER_EMAIL;
            return result;
        }
    }
}
