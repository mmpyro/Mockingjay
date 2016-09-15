using System;
using System.Collections.Generic;
using System.Text;

namespace MockingJayRoutes
{
    public class RouteManager
    {
        private readonly Dictionary<string, IController> _dict = new Dictionary<string, IController>();

        public void Resolve(IHttpContext context)
        {
            IHttpRequest request = context.Request;
            string url = request.Url;
            try
            {
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
            catch(ArgumentException ex)
            {
                var response = context.Response;
                response.StatusCode = 404;
                response.ContentType = "application/json";
                response.FillContent(Stringify(ex.Message), request.ContentEncoding);
                response.Close();
            }
            catch(Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = 500;
                response.FillContent(Stringify(ex.Message), request.ContentEncoding);
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

        private string Stringify(string str)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"").Append(str).Append("\"");
            return sb.ToString();
        }
    }
}
