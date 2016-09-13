using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using MockingJayRoutes;
using System.IO;

namespace MockingJayServer
{
    internal class HttpResponse : IHttpResponse
    {
        private readonly HttpListenerResponse _response;

        public HttpResponse(HttpListenerResponse response)
        {
            _response = response;
            response.ContentType = "application/json";
            ContentType = response.ContentType;
            StatusCode = response.StatusCode;
            Headers = response.Headers;
        }

        public string Body
        {
            get
            {
                string content = "";
                using (var stream = new StreamReader(_response.OutputStream, _response.ContentEncoding))
                {
                    content = stream.ReadToEnd();
                }
                return content;
            }
        }

        public string ContentType { get; set; }

        public NameValueCollection Headers { get; set; }

        public int StatusCode { get; set; }

        public void Close()
        {
            _response.Close();
        }

        public void FillContent(string message, Encoding encoding)
        {
            using (var stream = new StreamWriter(_response.OutputStream, encoding))
            {
                stream.WriteLine(message);
                stream.Flush();
            }
        }
    }
}