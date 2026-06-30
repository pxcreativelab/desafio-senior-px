using System.Web;
using HubieTest.Business;
using HubieTest.Web.ashx;
using Newtonsoft.Json;

namespace HubieTest.Web.process
{
    /// <summary>
    /// Categories handler. REFERENCE EXAMPLE of a complete slice:
    /// AngularJS -> categories.ashx -> categoryBusiness -> categoryDB (EF) -> JSON.
    /// Use this file as a template to implement ticket.ashx.
    ///
    /// POST process/categories.ashx   (requires Authorization: Bearer &lt;jwt&gt;)
    ///   method = "list"
    /// </summary>
    public class categories : AshxBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequestSafe(context); // validates the token
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.ContentType = "application/json";

            if (HttpStatusReturn == 200)
            {
                strContextResponse = processRequest(strMETHOD, strData);
            }

            context.Response.StatusCode = HttpStatusReturn;
            context.Response.Write(strContextResponse);
        }

        private string processRequest(string method, string data)
        {
            var business = new categoryBusiness();

            switch (method)
            {
                case "list":
                    return JsonConvert.SerializeObject(business.listActive());

                default:
                    HttpStatusReturn = 400;
                    return JsonConvert.SerializeObject(new { error = "Unsupported method: " + method });
            }
        }
    }
}
