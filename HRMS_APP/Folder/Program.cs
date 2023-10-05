using System;
using System.Net.Http;
using System.Text;


namespace HRMS_APP.Folder
{
    internal class Program
    {
        internal class Json
        {
            static void Main(string[] args)
            {
                var postData = new PostData
                {
                    userid = "ram",
                    email = "rutuja.22110140@gmail.com",
                    password = "Rutuja123",
                    city = "Satara",
                    status = "Active"
                };
                string connectionString = "103.190.54.22,1633\\SQLEXPRESS;Database=hrms_app;User Id=ecohrms;Password=EcoHrms@123;";

                var client = new HttpClient();
                client.BaseAddress = new System.Uri(connectionString);

                var json = Json.Serializer(postData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.PostAsync("posts", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(responseContent);

                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }
            private static string Serializer(PostData postData)
            {
                throw new NotImplementedException();
            }
        }

    }
}