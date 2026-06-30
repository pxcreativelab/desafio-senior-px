using System;
using System.Web;

namespace HubieTest.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Answers OPTIONS requests (CORS preflight) with 200 so AngularJS can
        /// call the .ashx handlers from another origin.
        /// </summary>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                var r = HttpContext.Current.Response;
                r.Headers.Add("Access-Control-Allow-Origin", "*");
                r.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                r.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization, X-User-System, X-User-Module");
                r.StatusCode = 200;
                r.End();
            }
        }
    }
}
