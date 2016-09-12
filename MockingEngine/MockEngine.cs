using System;
using System.Collections.Generic;
using System.Linq;

namespace MockingJay
{
    public class MockEngine
    {
        private readonly Dictionary<Request, Configuration> _dict = new Dictionary<Request, Configuration>();

        public Response Resolve(Request req)
        {
            if (_dict.ContainsKey(req))
                return _dict[req].Return;
            else
                throw new ArgumentException("This request was not register before.");
        }

        public void Register(Configuration configuration)
        {
            var req = configuration.CreateRequest();
            if (_dict.ContainsKey(req))
                throw new ArgumentException("This request was register before.");
            _dict.Add(req, configuration);
        }

        public IEnumerable<Configuration> GetAllRegisteredConfiguration()
        {
            return _dict.Select(t => t.Value);
        }

        public IEnumerable<Request> GetAllRegisteredRequest()
        {
            return _dict.Select(t => t.Key);
        }

        public void Remove(Request request)
        {
            if (_dict.ContainsKey(request))
                _dict.Remove(request);
            else
                throw new ArgumentException("This request wasn't register before. Cannot remove unregistered request.");
        }

        public void RemoveAll()
        {
            _dict.Clear();
        }
    }
}
