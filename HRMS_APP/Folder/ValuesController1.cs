using System;
using System.IO;
using System.Net;
using System.Web.Http;

namespace HRMS_APP.Folder
{
    public class ValuesController1 : ApiController
    {
        // Define your connection string here
        private string connectionString = "Driver={SQL Server};Server=103.190.54.22\\\\SQLEXPRESS,1633;Database=hrms_app;Uid=ecohrms;Pwd=EcoHrms@123;";

        [HttpGet] // Use this attribute to define HTTP GET method
        public IHttpActionResult GetData()
        {
            try
            {
                // Create a WebRequest for the URL you want to fetch
                string strurltest = "URL_HERE"; // Replace with your URL
                WebRequest requestObjGet = WebRequest.Create(strurltest);
                requestObjGet.Method = "GET";

                // Get the response
                using (HttpWebResponse response = (HttpWebResponse)requestObjGet.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Read the response data
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            string strresulttest = sr.ReadToEnd();
                            return Ok(strresulttest); // Return the response data
                        }
                    }
                    else
                    {
                        return StatusCode(response.StatusCode); // Return the HTTP status code
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // Return a 500 Internal Server Error if an exception occurs
            }
        }
    }
}
