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
            var response = httpContext.Response;
            if (request.HttpMethod == "DELETE")
            {
                _mockingJayApp.DeleteAll();
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