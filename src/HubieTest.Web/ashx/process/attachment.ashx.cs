using System.Web;
using HubieTest.Web.ashx;
using Newtonsoft.Json;

namespace HubieTest.Web.process
{
    /// <summary>
    /// Ticket attachment upload. Unlike the other handlers, it receives
    /// multipart/form-data (binary file), so it reads from Request.Files.
    ///
    /// ========================= CANDIDATE AREA =========================
    /// Expected flow (method = "upload"):
    ///   1. validate that a file and a ticketId were sent;
    ///   2. save the file to disk (e.g. ~/uploads/{ticketId}/{guid}_{name});
    ///   3. register the metadata via ticketBusiness.registerAttachment(...);
    ///   4. return the created ATTACHMENT as JSON.
    /// Download/listing can be done by ticket.ashx (listAttachments) + a
    /// static/endpoint route that serves the saved file.
    /// ==================================================================
    /// </summary>
    public class attachment : AshxBase
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequestSafe(context); // validates the JWT
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.ContentType = "application/json";

            if (HttpStatusReturn == 200)
            {
                strContextResponse = processRequest(context);
            }

            context.Response.StatusCode = HttpStatusReturn;
            context.Response.Write(strContextResponse);
        }

        private string processRequest(HttpContext context)
        {
            if (strMETHOD != "upload")
            {
                HttpStatusReturn = 400;
                return JsonConvert.SerializeObject(new { error = "Unsupported method: " + strMETHOD });
            }

            // example of reading the multipart payload (left here as guidance):
            //   HttpPostedFile file = context.Request.Files.Count > 0 ? context.Request.Files[0] : null;
            //   string ticketId = context.Request.Form["ticketId"];

            // TODO:
            //  - validate file/ticketId (and size/extension for extra points)
            //  - save it to disk
            //  - register the metadata (name, type, size, path) via ticketBusiness
            //  - return the ATTACHMENT as JSON

            HttpStatusReturn = 501; // Not Implemented
            return JsonConvert.SerializeObject(new { error = "Attachment upload not implemented yet." });
        }
    }
}
