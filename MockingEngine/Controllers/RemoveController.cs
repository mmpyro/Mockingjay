﻿using MockingJayRoutes;
using Newtonsoft.Json;

namespace MockingJay.Controllers
{
    public class RemoveController : IController
    {
        private MockingJayApp _mockingJayApp;

        public RemoveController(MockingJayApp mockingJayApp)
        {
            _mockingJayApp = mockingJayApp;
        }

        public void Invoke(IHttpContext httpContext)
        {
            var response = httpContext.Response;
            if(httpContext.Request.HttpMethod == "PUT")
            {
                Request request = JsonConvert.DeserializeObject<Request>(httpContext.Request.ContentBody);
                _mockingJayApp.DeleteRequest(request);
                response.StatusCode = 204;
            }
            else
            {
                response.StatusCode = 404;
            }
            response.Close();
        }
    }
}