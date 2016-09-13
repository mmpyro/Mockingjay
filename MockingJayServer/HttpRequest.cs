using System;
using System.Collections.Specialized;
using System.Net;
using MockingJayRoutes;
using System.IO;
using System.Text;

namespace MockingJayServer
{
    internal class HttpRequest : IHttpRequest
    {
        private readonly HttpListenerRequest _request;

        public HttpRequest(HttpListenerRequest request)
        {
            _request = request;
            HttpMethod = request.HttpMethod;
            Url = request.Url.AbsoluteUri;
            ContentBody = TakeDataFromBody(request);
            Headers = request.Headers;
            ContentEncoding = request.ContentEncoding;
        }

        private string TakeDataFromBody(HttpListenerRequest request)
        {
            string res = "";
            using (var sr = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                res = sr.ReadToEnd();
            }
            return res;
        }

        public string ContentBody { get; set; }

        public NameValueCollection Headers { get; set; }

        public string HttpMethod { get; set; }

        public string Url { get; set;}

        public Encoding ContentEncoding { get; set; }
    }
}