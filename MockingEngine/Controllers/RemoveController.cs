using MockingJayRoutes;
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
            if(httpContext.Request.HttpMethod == "DELETE")
            {
                Request request = JsonConvert.DeserializeObject<Request>(httpContext.Request.ContentBody);
                _mockingJayApp.DeleteRequest(request);
            }
        }
    }
}