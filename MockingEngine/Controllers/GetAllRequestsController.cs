using MockingJayRoutes;
using MockingJayRoutes.helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MockingJay.Controllers
{
    public class GetAllRequestsController : IController
    {
        private readonly ParserFactory _factory;
        private readonly MockingJayApp _mockingJayApp;
        private readonly PageBuilder<Request> _pageBuilder;

        public GetAllRequestsController(MockingJayApp mockingJayApp, ParserFactory factory, PageBuilder<Request> pageBuilder)
        {
            _mockingJayApp = mockingJayApp;
            _factory = factory;
            _pageBuilder = pageBuilder;
        }

        public void Invoke(IHttpContext httpContext)
        {
            var request = httpContext.Request;
            var response = httpContext.Response;
            if (request.HttpMethod == "GET")
            {
                var data = _mockingJayApp.GetAllRegisteredRequest();
                if (data.Any())
                {
                    Page<Request> page = FetchPage(request.Url, data);

                    response.FillContent(JsonConvert.SerializeObject(page), request.ContentEncoding);
                    response.StatusCode = 200;
                }
                else
                {
                    response.StatusCode = 204;
                }
            }
            else
            {
                response.StatusCode = 404;
            }
            response.Close();
        }

        private Page<Request> FetchPage(string url, IEnumerable<Request> data)
        {
            var parser = _factory.CreateQueryStringParser(url);
            int items = parser.TakeIntValue("items",10);
            int page = parser.TakeIntValue("page", 0);
            
            var filteredData = data.Skip(page*items).Take(items);
            return _pageBuilder.For(filteredData)
                                .WithMetadata(url, items, page, data.Count())
                                .Build();
        }
    }
}
