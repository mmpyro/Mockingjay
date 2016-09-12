using MockingJayRoutes;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockingJayServer
{
    public class HttpContext : IHttpContext
    {
        public HttpContext(HttpListenerContext context)
        {
            Request = new HttpRequest(context.Request);
            Response = new HttpResponse(context.Response);
        }

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }
    }
}
