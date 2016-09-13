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
                if (_dict.ContainsKey(request.Url))
                {
                    IController controller = _dict[request.Url];
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
            }
            catch(Exception)
            {
                var response = context.Response;
                response.StatusCode = 500;
                response.ContentType = "application/json";
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
