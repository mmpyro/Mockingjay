﻿using MockingJayRoutes;
using System;
using System.Collections.Generic;

namespace MockingJay.Controllers
{
    public class FillController : IController
    {
        private MockingJayApp mockingJayApp;

        public FillController(MockingJayApp mockingJayApp)
        {
            this.mockingJayApp = mockingJayApp;
        }

        public void Invoke(IHttpContext httpContext)
        {
            Request request = new Request
            {
                Url = httpContext.Request.Url,
                Type = (HttpMethodType) Enum.Parse(typeof(HttpMethodType), httpContext.Request.HttpMethod)
            };
            FillHeadersIfExists(httpContext,request);
            var responseStructure = mockingJayApp.Resolve(request);
            var response = httpContext.Response;
            response.StatusCode = responseStructure.StatusCode;
            response.Close();
        }

        private void FillHeadersIfExists(IHttpContext httpContext, Request request)
        {
            var headers = httpContext?.Request?.Headers;
            if (headers != null)
            {
                var list = new List<Header>();
                foreach (string key in headers)
                {
                    list.Add(new Header
                    {
                        Name = key,
                        Value = headers[key]
                    });
                }
                request.Headers = list;
            }
        }
    }
}
