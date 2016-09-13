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

        public string ContentType
        {
            get
            {
                return _response.ContentType;
            }
            set
            {
                _response.ContentType = value;
            }
        }

        public NameValueCollection Headers
        {
            get
            {
                return _response.Headers;
            }
            set
            {
                _response.Headers.Clear();
                foreach(string key in value)
                {
                    _response.Headers.Add(key, value[key]);
                }
            }
        }

        public int StatusCode
        {
            get
            {
                return _response.StatusCode;
            }
            set
            {
                _response.StatusCode = value;
            }
        }

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