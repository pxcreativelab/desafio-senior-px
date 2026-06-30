using System.Collections.Generic;
using System.Linq;
using HubieTest.Dal;

namespace HubieTest.Business.Data
{
    /// <summary>
    /// Data access for CATEGORY.
    /// >>> This class is the REFERENCE EXAMPLE of a complete slice
    ///     (ashx -> business -> DB -> EF -> JSON). Use it as a template
    ///     to implement ticketDB.
    /// </summary>
    public class categoryDB
    {
        public List<CATEGORY> listActive()
        {
            using (var db = new HubieContext())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                return db.CATEGORIES
                         .Where(c => c.CATEGORY_ACTIVE)
                         .OrderBy(c => c.CATEGORY_NAME)
                         .ToList();
            }
        }
    }
}
