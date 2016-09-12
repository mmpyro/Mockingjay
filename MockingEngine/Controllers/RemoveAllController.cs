using MockingJayRoutes;

namespace MockingJay.Controllers
{
    public class RemoveAllController : IController
    {
        private MockingJayApp _mockingJayApp;

        public RemoveAllController(MockingJayApp mockingJayApp)
        {
            _mockingJayApp = mockingJayApp;
        }

        public void Invoke(IHttpContext httpContext)
        {
            var request = httpContext.Request;
            if (request.HttpMethod == "DELETE")
            {
                _mockingJayApp.DeleteAll();
            }
        }
    }
}