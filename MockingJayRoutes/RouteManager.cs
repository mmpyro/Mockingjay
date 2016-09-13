using System;
using System.Collections.Generic;

namespace MockingJayRoutes
{
    public class RouteManager
    {
        private readonly Dictionary<string, IController> _dict = new Dictionary<string, IController>();

        public void Resolve(IHttpContext context)
        {
            try
            {
                IHttpRequest request = context.Request;
                string url = request.Url;
                if (request.Url.IndexOf("?") > 0)
                    url = request.Url.Substring(0, url.IndexOf("?"));

                if (_dict.ContainsKey(url))
                {
                    IController controller = _dict[url];
                    controller.Invoke(context);
                }
                else
                {
                    IController controller = _dict["*"];
                    controller.Invoke(context);
                }
            }
            catch(KeyNotFoundException)
            {
                var response = context.Response;
                response.StatusCode = 404;
                response.ContentType = "application/json";
                response.Close();
            }
            catch(Exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = 500;
                response.Close();
            }
        }

        public void Add(string url, IController controller)
        {
            if (!_dict.ContainsKey(url))
            {
                _dict.Add(url, controller);
            }
        }
    }
}
