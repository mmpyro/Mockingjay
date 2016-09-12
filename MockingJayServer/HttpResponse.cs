using System;
using System.Net;
using MockingJayRoutes;

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
        }

        public string ContentType { get; set; }

        public int StatusCode { get; set; }

        public void Close()
        {
            _response.Close();
        }
    }
}