using MockingJayRoutes;
using NSubstitute;
using NUnit.Framework;

namespace MockingjaySpecyfication
{
    [TestFixture]
    public class RouteManagerSpecification
    {
        [Test]
        public void ShouldRedirectToProperController()
        {
            //Given
            const string url = "/register";
            var controller = Substitute.For<IController>();
            var request = Substitute.For<IHttpRequest>();
            request.Url.Returns(t => url);
            controller.Invoke(Arg.Any<IHttpContext>());
            IHttpContext context = Substitute.For<IHttpContext>();
            context.Request = request;
            RouteManager routes = new RouteManager();
            routes.Add(url,controller);
            //When
            routes.Resolve(context);
            //Then
            controller.Received().Invoke(Arg.Any<IHttpContext>());
        }

        [Test]
        public void ShouldReturnNotFoundStatusInResponse()
        {
            //Given
            const string url = "/register";
            var controller = Substitute.For<IController>();
            var request = Substitute.For<IHttpRequest>();
            request.Url.Returns(t => url+"/aa");
            controller.Invoke(Arg.Any<IHttpContext>());
            IHttpContext context = Substitute.For<IHttpContext>();
            context.Request = request;
            RouteManager routes = new RouteManager();
            routes.Add(url, controller);
            //When
            routes.Resolve(context);
            var response = context.Response;
            //Then
            Assert.That(response.StatusCode, Is.EqualTo(404));
            Assert.That(response.ContentType, Is.EqualTo("application/json"));
        }
    }
}
