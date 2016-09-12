using System;
using System.Collections.Specialized;
using System.Net;
using MockingJayRoutes;
using System.IO;

namespace MockingJayServer
{
    internal class HttpRequest : IHttpRequest
    {
        public HttpRequest(HttpListenerRequest request)
        {
            HttpMethod = request.HttpMethod;
            Url = request.Url.AbsoluteUri;
            ContentBody = TakeDataFromBody(request);
            Headers = request.Headers;
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

        public string Url { get; set; }
    }
}