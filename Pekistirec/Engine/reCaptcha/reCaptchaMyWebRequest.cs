using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pekistirec.Engine.reCaptcha
{
    public class reCaptchaMyWebRequest
    {
        private HttpWebRequest request;
        private Stream dataStream;
        private CookieContainer _cookieContainer;
        private string postData;

        public CookieContainer cookieContainer
        {
            get
            {
                if (_cookieContainer == null)
                {
                    _cookieContainer = new CookieContainer();
                }
                return _cookieContainer;
            }
            set
            {
                _cookieContainer = value;
                request.CookieContainer = _cookieContainer;
            }
        }

        private string status;

        public String Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public reCaptchaMyWebRequest(string url)
        {
            // Create a request using a URL that can receive a post.

            request = (HttpWebRequest)WebRequest.Create(url);
        }

        public reCaptchaMyWebRequest(string url, string method)
            : this(url)
        {

            if (method.Equals("GET") || method.Equals("POST"))
            {
                // Set the Method property of the request to POST.
                request.Method = method;
            }
            else
            {
                throw new Exception("Invalid Method Type");
            }
        }

        public reCaptchaMyWebRequest(string url, string method, string data)
            : this(url, method)
        {

            // Create POST data and convert it to a byte array.
            string postData = data;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            dataStream.Close();

        }

        public reCaptchaMyWebRequest(string url, string method, Dictionary<string, string> data)
            : this(url, method)
        {

            StringBuilder tempData = new StringBuilder();
            foreach (var item in data)
            {
                tempData.Append(String.Format(item.Key + "={0}&", HttpUtility.UrlEncode(item.Value)));
            }

            // Create POST data and convert it to a byte array.
            postData = tempData.ToString();
            postData = postData.Substring(0, postData.Length - 1);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            dataStream.Close();
        }

        public string GetResponse()
        {
            request.CookieContainer = cookieContainer;

            // Get the original response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //
            for (int i = 0; i < response.Cookies.Count; i++)
            {
                Cookie http_cookie = response.Cookies[i];
                Cookie cookie = new Cookie(http_cookie.Name, http_cookie.Value, http_cookie.Path);
                cookieContainer.Add(new Uri(request.RequestUri.ToString()), cookie);
            }

            this.Status = ((HttpWebResponse)response).StatusDescription;

            // Get the stream containing all content returned by the requested server.
            dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content fully up to the end.
            string responseFromServer = reader.ReadToEnd();

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        public string Post()
        {

            byte[] postBytes = new ASCIIEncoding().GetBytes(postData.ToString());
            Stream postStream = request.GetRequestStream();
            postStream.Write(postBytes, 0, postBytes.Length);
            postStream.Flush();
            postStream.Close();

            return GetResponse();

        }
    }
}