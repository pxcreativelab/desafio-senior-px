using System.Linq;
using System.Net;
using System.Web;

namespace HubieTest.Web.ashx
{
    /// <summary>
    /// Base class for every .ashx handler (mirrors AshxBase.cs in Hubie).
    ///
    /// Responsibilities:
    ///  - enable CORS;
    ///  - read the POST fields: "method" (which operation) and "data" (JSON payload);
    ///  - validate the JWT and expose the logged-in user (UserId/Profile/Name) to
    ///    the concrete handlers.
    ///
    /// Concrete handlers override ProcessRequest, call ProcessRequestSafe and
    /// dispatch by <see cref="strMETHOD"/>.
    /// </summary>
    public class AshxBase : IHttpHandler
    {
        public virtual bool IsReusable => false;

        protected string strContextResponse = null;
        protected string strMETHOD = null;
        protected string strData = null;

        protected long UserId = 0;
        protected string UserLogin = null;
        protected string UserProfile = null;
        protected string UserName = null;
        protected string CurrentToken = null;

        protected int HttpStatusReturn { get; set; } = 200;

        public virtual void ProcessRequest(HttpContext context)
        {
            // overridden by concrete handlers
        }

        /// <param name="checkIsSafe">false for public endpoints (e.g. login).</param>
        public virtual void ProcessRequestSafe(HttpContext context, bool checkIsSafe = true)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization, X-User-System, X-User-Module");
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            // let the frontend read these headers from JS (e.g. the token on login)
            context.Response.Headers.Add("Access-Control-Expose-Headers", "X-User-Token, X-User-ErrorMessage");

            // method / data may come in the form (POST) or the query string
            if (context.Request.Form.AllKeys.Contains("method"))
                strMETHOD = context.Request.Form["method"];
            else if (context.Request.QueryString["method"] != null)
                strMETHOD = context.Request.QueryString["method"];

            if (context.Request.Form.AllKeys.Contains("data"))
                strData = context.Request.Form["data"];
            else if (context.Request.QueryString["data"] != null)
                strData = context.Request.QueryString["data"];

            if (checkIsSafe)
            {
                var safe = new safety();
                bool isSafe = safe.validate(context);

                if (!isSafe)
                {
                    HttpStatusReturn = (int)HttpStatusCode.Unauthorized;
                    context.Response.StatusCode = HttpStatusReturn;
                    context.Response.Headers.Add("X-User-ErrorMessage", safe.tokenMessage);
                    context.Response.Write(string.Empty);
                    context.Response.End();
                    return;
                }

                UserId = safe.currentUserId;
                UserLogin = safe.currentUserLogin;
                UserProfile = safe.currentUserProfile;
                UserName = safe.currentUserName;
                CurrentToken = safe.currentToken;
            }
        }
    }
}
