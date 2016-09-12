using MockingJay;
using System.Collections.Generic;
using NClone;

namespace MockingjaySpecyfication.Helpers
{
    internal class ConfigBuilder
    {
        private readonly Configuration _configuration;

        public ConfigBuilder()
        {
            _configuration = new Configuration
            {
                Url = "/users",
                Type = HttpMethodType.GET,
                Return = new Response
                {
                    Content = null,
                    StatusCode = 200,
                    Headers = new List<Header>()
                }
            };
        }

        public ConfigBuilder Get()
        {
            _configuration.Type = HttpMethodType.GET;
            return this;
        }

        public ConfigBuilder Post()
        {
            _configuration.Type = HttpMethodType.POST;
            return this;
        }

        public ConfigBuilder Delete()
        {
            _configuration.Type = HttpMethodType.DELETE;
            return this;
        }

        public ConfigBuilder Put()
        {
            _configuration.Type = HttpMethodType.PUT;
            return this;
        }

        public ConfigBuilder Patch()
        {
            _configuration.Type = HttpMethodType.PATCH;
            return this;
        }

        public ConfigBuilder Head()
        {
            _configuration.Type = HttpMethodType.HEAD;
            return this;
        }

        public ConfigBuilder WithResponseCode(int code)
        {
            _configuration.Return.StatusCode = code;
            return this;
        }

        public ConfigBuilder WithResponseContent(string content)
        {
            _configuration.Return.Content = content;
            return this;
        }

        public ConfigBuilder WithRequestHeader(string key, string value)
        {
            _configuration.Headers.Add(new Header
            {
                Name = key,
                Value = value
            });
            return this;
        }

        public ConfigBuilder WithUrl(string url)
        {
            _configuration.Url = url;
            return this;
        }

        public Configuration Build()
        {
            var response = Clone.ObjectGraph(_configuration);
            Reset();
            return response;
        }

        protected void Reset()
        {
            _configuration.Url = "/users";
            _configuration.Type = HttpMethodType.GET;
            _configuration.Headers.Clear();
            _configuration.Return = new Response
            {
                Content = null,
                StatusCode = 200,
                Headers = new List<Header>()
            };
        }

        public Configuration WithResponseStructure(Response response)
        {
            _configuration.Return = response;
            return this;
        }

        public Configuration WithResponseHeader(string key, string value)
        {
            var response = _configuration.Return;
            response.Headers.Add(new Header
            {
                Name = key,
                Value = value
            });
            return this;
        }

        public static implicit operator Configuration(ConfigBuilder builder)
        {
            return builder.Build();
        }
    }
}