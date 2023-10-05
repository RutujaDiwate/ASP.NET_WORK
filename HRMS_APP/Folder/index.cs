using System;
using System.Web;
using System.Web.SessionState;

namespace HRMS_APP.Folder
{
    public class Index : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // Your code for handling the index page request goes here
            // You can customize this code to display any content you want on the index page

            context.Response.ContentType = "text/html";
            context.Response.Write("<html><head><title>HRMS App</title></head><body><h1>Welcome to HRMS App</h1></body></html>");
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
