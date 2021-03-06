﻿using MockingJayRoutes;
using Newtonsoft.Json;
using System;

namespace MockingJay.Controllers
{
    public class RegisterController : IController
    {
        private MockingJayApp _mockingJayApp;

        public RegisterController(MockingJayApp mockingJayApp)
        {
            _mockingJayApp = mockingJayApp;
        }

        public void Invoke(IHttpContext httpContext)
        {
            var request = httpContext.Request;
            var response = httpContext.Response;
            if (request.HttpMethod == "POST")
            {
                Configuration conf = JsonConvert.DeserializeObject<Configuration>(request.ContentBody);
                _mockingJayApp.RegisterMessageIfValid(conf);
                response.StatusCode = 201;
                response.Headers.Add("Location", conf.Url);
            }
            else
            {
                response.StatusCode = 404;
            }
            response.Close();
        }
    }
}
