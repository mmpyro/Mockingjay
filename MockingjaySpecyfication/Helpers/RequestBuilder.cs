using System;
using MockingJay;
using NClone;

namespace MockingjaySpecyfication.Helpers
{
    public class RequestBuilder
    {
        private readonly Request _req;

        public RequestBuilder()
        {
            _req = new Request
            {
                Url = "/users",
                Type = HttpMethodType.GET.ToString()
            };
        }

        public RequestBuilder WithUrl(string url)
        {
            _req.Url = url;
            return this;
        }

        public RequestBuilder Get()
        {
            _req.Type = HttpMethodType.GET.ToString();
            return this;
        }

        public RequestBuilder Post()
        {
            _req.Type = HttpMethodType.POST.ToString();
            return this;
        }

        public Request Build()
        {
            var request = Clone.ObjectGraph(_req);
            Reset();
            return request;
        }

        protected void Reset()
        {
            _req.Url = "/users";
            _req.Type = HttpMethodType.GET.ToString();
            _req.Headers.Clear();
        }

        public static implicit operator Request(RequestBuilder builder)
        {
            return builder.Build();
        }

        public RequestBuilder WithHeader(string key, string value)
        {
            _req.Headers.Add(new Header
            {
                Name = key,
                Value = value
            });
            return this;
        }
    }
}
