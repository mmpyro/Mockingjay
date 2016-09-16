using System;
using MockingJayRoutes;
using MockingJayRoutes.helpers;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MockingJay.Controllers
{
    public class GetAllConfigurationController : IController
    {
        private readonly ParserFactory _factory;
        private readonly MockingJayApp _mockingJayApp;
        private readonly PageBuilder<Configuration> _pageBuilder;

        public GetAllConfigurationController(MockingJayApp mockingJayApp, ParserFactory factory, PageBuilder<Configuration> pageBuilder)
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
                var data = _mockingJayApp.GetAllRegisteredConfiguration();
                if (data.Any())
                {
                    Page<Configuration> page = FetchPage(request.Url, data);

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

        private Page<Configuration> FetchPage(string url, IEnumerable<Configuration> data)
        {
            var parser = _factory.CreateQueryStringParser(url);
            int items = parser.TakeIntValue("items", 10);
            int page = parser.TakeIntValue("page", 0);

            var filteredData = data.Skip(page * items).Take(items);
            return _pageBuilder.For(filteredData)
                                .WithMetadata(url, items, page, data.Count())
                                .Build();
        }
    }
}
