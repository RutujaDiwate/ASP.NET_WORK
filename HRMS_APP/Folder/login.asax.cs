using System;
using System.Web;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Configuration;

namespace HRMS_APP.Folder
{
    public class Login : IHttpHandler, IRequiresSessionState
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["\"Driver={SQL Server};Server=103.190.54.22\\\\SQLEXPRESS,1633;Database=hrms_app;Uid=ecohrms;Pwd=EcoHrms@123;"].ConnectionString;

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod == "POST")
            {
                string userid = context.Request["userid"];
                string password = context.Request["password"];
                string registrationKey = context.Request["RegistrationKey"];

                // Your login logic here
                if (IsValidRegistrationKey(userid, registrationKey))
                {
                    if (IsValidUser(userid, password))
                    {
                        context.Session["isLoggedIn"] = true;
                        context.Session["userid"] = userid;

                        context.Response.ContentType = "text/plain";
                        context.Response.Write("Login successful");
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.Write("Invalid Credentials!");
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Write("Invalid Registration Key or User ID!");
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }

        private bool IsValidRegistrationKey(string userid, string registrationKey)
        {
            // Implement your logic to validate the registration key
            // Query your database to check if the registrationKey is valid for the given userid
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT status FROM RegistrationKeys WHERE userid = @userid AND RegistrationKey = @registrationKey";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@registrationKey", registrationKey);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string status = reader["status"].ToString().Trim();
                            return status == "R" || status == "A";
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        private bool IsValidUser(string userid, string password)
        {
            // Implement your logic to validate the user's credentials
            // Query your database to check if the userid and password match
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT password FROM userdata WHERE userid = @userid";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashedPassword = reader["password"].ToString();

                            // Compare the hashed password with the input password
                            using (SHA256 sha256 = SHA256.Create())
                            {
                                byte[] hashedInputBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                                string hashedInputPassword = BitConverter.ToString(hashedInputBytes).Replace("-", "").ToLower();

                                return hashedInputPassword == hashedPassword;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
