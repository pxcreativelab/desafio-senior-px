using System.Collections.Generic;
using HubieTest.Business.Data;
using HubieTest.Dal;

namespace HubieTest.Business
{
    /// <summary>
    /// Category rules. REFERENCE EXAMPLE — already implemented.
    /// </summary>
    public class categoryBusiness
    {
        private readonly categoryDB _db = new categoryDB();

        public List<CATEGORY> listActive()
        {
            return _db.listActive();
        }
    }
}
