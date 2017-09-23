using System;
using Xunit;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text;

namespace XUnitTest
{
    public class XUnitTest
    {
        public object HttpResponse { get; private set; }

        [Fact]
        public void CreateGetTests()
        {
            //construct content to send
            var content = new StringContent("json, Encoding.UTF8, application/json");
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:65081/api/values"),
                Content = content
            };

            //Get the HTML site data
            WebClient client = new WebClient();
            String downloadedString = client.DownloadString("http://localhost:65081/api/values");
            //string response = request.ToString();
            //string expected = "value1";

            //Get the reponse code from HTML site
            HttpWebRequest requestCode = (HttpWebRequest)HttpWebRequest.Create("http://localhost:65081/api/values");
            HttpWebResponse responseCode = (HttpWebResponse)requestCode.GetResponse();

            //Assert
            //Verify the strings
            //Assert.Contains(expected, downloadedString);
            //Check that response is '200' <-> OK 
            Assert.Equal(responseCode.StatusCode, HttpStatusCode.OK);
        }

        [Fact]
        public void CreateSetTests()
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create("http://localhost:65081/api/values");
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            string postData = "Item2";
            string beforeData = "Item1";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);

            //Asset tests...
            //Verify the strings
            WebClient client = new WebClient();
            String downloadedString = client.DownloadString("http://localhost:65081/api/values");
            //Assert.Contains(beforeData+","+postData, downloadedString);
            //Check that response is '200' <-> OK 
            HttpWebRequest requestCode = (HttpWebRequest)HttpWebRequest.Create("http://localhost:65081/api/values");
            HttpWebResponse responseCode = (HttpWebResponse)requestCode.GetResponse();
            Assert.Equal(responseCode.StatusCode, HttpStatusCode.OK);

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
        }

        //Post to todo page a data line
         [Fact]
        public void CreatePostTest()
        {
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create("http://localhost:65081/api/todo");
            // Set the Method property of the request to POST.
            request.Method = "POST";

            request.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"id\":\"2\"," +
                              "\"name\":\"Walk dog\"," +
                              "\"isComplete\":\"true\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
}

