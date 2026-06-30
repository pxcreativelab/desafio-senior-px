using System.Net;
using System.Web;
using HubieTest.Business;
using HubieTest.Business.Entities;
using HubieTest.Web.ashx;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HubieTest.Web.auth
{
    /// <summary>
    /// Authentication handler (mirrors auth/starter.ashx in Hubie).
    /// PUBLIC endpoint (does not validate an incoming token).
    ///
    /// POST auth/starter.ashx
    ///   method = "authlogin"
    ///   data   = {"login":"requester","password":"123456"}
    ///
    /// Success -> 200, header "X-User-Token: &lt;jwt&gt;" and a JSON body with the user.
    /// Failure -> 401, header "X-User-ErrorMessage".
    ///
    /// IMPLEMENTED as a reference.
    /// </summary>
    public class starter : AshxBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequestSafe(context, checkIsSafe: false);
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.ContentType = "application/json";

            if (strMETHOD == "authlogin")
            {
                strContextResponse = authLogin(context, strData);
            }

            context.Response.StatusCode = HttpStatusReturn;
            context.Response.Write(strContextResponse);
        }

        private string authLogin(HttpContext context, string data)
        {
            string login = null, password = null;
            if (!string.IsNullOrEmpty(data))
            {
                JObject o = JObject.Parse(data);
                login = (string)o["login"];
                password = (string)o["password"];
            }

            var ub = new userBusiness();
            AuthResult auth = ub.auth(login, password);

            if (auth.STATUS != "OK")
            {
                HttpStatusReturn = (int)HttpStatusCode.Unauthorized;
                context.Response.Headers.Add("X-User-ErrorMessage", auth.STATUS);
                return JsonConvert.SerializeObject(new { status = auth.STATUS });
            }

            // the token goes in the header (Hubie pattern); the body carries the user data
            context.Response.Headers.Add("X-User-Token", auth.TOKEN);

            // do not return the TOKEN in the body to avoid duplication / accidental logging
            auth.TOKEN = null;
            return JsonConvert.SerializeObject(auth);
        }
    }
}
