using System.Linq;
using HubieTest.Dal;

namespace HubieTest.Business.Data
{
    /// <summary>
    /// Data access for APP_USER (repository style, like ticketDB in Hubie:
    /// each operation opens its own DbContext inside a using).
    /// </summary>
    public class userDB
    {
        public APP_USER getByLogin(string login)
        {
            using (var db = new HubieContext())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                return db.APP_USERS
                         .Where(u => u.USER_LOGIN == login && u.USER_ACTIVE)
                         .FirstOrDefault();
            }
        }

        public APP_USER getById(long userId)
        {
            using (var db = new HubieContext())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                return db.APP_USERS
                         .Where(u => u.USER_ID == userId)
                         .FirstOrDefault();
            }
        }
    }
}
