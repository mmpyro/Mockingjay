using MockingJay.Validation;
using System;
using System.Collections.Generic;

namespace MockingJay
{
    public class MockingJayApp
    {
        private readonly MockEngine _mockEngine;
        private readonly IValidator _validator;

        public MockingJayApp(MockEngine mockEngine, IValidator complexValidator)
        {
            _mockEngine = mockEngine;
            _validator = complexValidator;
        }

        public void RegisterMessageIfValid(Configuration configuration)
        {
            if(_validator.IsValidConfiguration(configuration))
            {
                _mockEngine.Register(configuration);
            }
            else
            {
                throw new ArgumentException(_validator.ErrorMessage);
            }
        }

        public Response Resolve(Request req)
        {
            return _mockEngine.Resolve(req);
        }

        public IEnumerable<Configuration> GetAllRegisteredConfiguration()
        {
            return _mockEngine.GetAllRegisteredConfiguration();
        }

        public IEnumerable<Request> GetAllRegisteredRequest()
        {
            return _mockEngine.GetAllRegisteredRequest();
        }

        public void DeleteRequest(Request request)
        {
            _mockEngine.Remove(request);
        }

        public void DeleteAll()
        {
            _mockEngine.RemoveAll();
        }
    }
}