using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace HRMS_APP.Folder
{
    public class Register : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod == "POST")
            {
                var userid = context.Request["userid"];
                var email = context.Request["email"];
                var password = context.Request["password"];

                // Generate a new registration key
                var newRegistrationKey = GenerateRandomKey(10);

                // Hash the user's password
                using (var hmac = new HMACSHA256())
                {
                    var hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    // Continue with registration after hashing the password

                    // Create and configure an SMTP client (for sending email)
                    using (var client = new SmtpClient("smtp.gmail.com"))
                    {
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("rutuja.22110140@viit.ac.in", "zalpwsjzrxksvslk");
                        client.EnableSsl = true;

                        // Define the email message
                        var mailMessage = new MailMessage("rutuja.22110140@viit.ac.in", email)
                        {
                            Subject = "Registration Key",
                            Body = $"Your registration key is: {newRegistrationKey}"
                        };

                        // Send the email
                        client.Send(mailMessage);

                        // Store the new registration key and hashed password in the database
                        // Your database insertion logic here
                        // ...

                        // Send the registration key as a JSON response
                        context.Response.ContentType = "application/json";
                        context.Response.Write($"{{ \"registrationKey\": \"{newRegistrationKey}\" }}");
                    }
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
        }

        // Function to generate a random key of specified length
        private static string GenerateRandomKey(int length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
