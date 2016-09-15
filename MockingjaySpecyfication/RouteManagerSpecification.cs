using MockingJayRoutes;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Text;

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
            controller.When(t => t.Invoke(Arg.Any<IHttpContext>())).Do(t => { throw new ArgumentException(); });
            IHttpContext context = Substitute.For<IHttpContext>();
            context.Request = request;
            RouteManager routes = new RouteManager();
            routes.Add("*", controller);
            //When
            routes.Resolve(context);
            var response = context.Response;
            //Then
            Assert.That(response.StatusCode, Is.EqualTo(404));
            Assert.That(response.ContentType, Is.EqualTo("application/json"));
            response.Received().FillContent(Arg.Any<string>(), Arg.Any<Encoding>());
        }

        [Test]
        public void ShouldReturnInternalServerErrorInResponse()
        {
            //Given
            const string url = "/register";
            var controller = Substitute.For<IController>();
            var request = Substitute.For<IHttpRequest>();
            request.Url.Returns(t => url + "/aa");
            controller.When(t => t.Invoke(Arg.Any<IHttpContext>())).Do(t => { throw new Exception(); });
            IHttpContext context = Substitute.For<IHttpContext>();
            context.Request = request;
            RouteManager routes = new RouteManager();
            routes.Add("*", controller);
            //When
            routes.Resolve(context);
            var response = context.Response;
            //Then
            Assert.That(response.StatusCode, Is.EqualTo(500));
            Assert.That(response.ContentType, Is.EqualTo("application/json"));
            response.Received().FillContent(Arg.Any<string>(), Arg.Any<Encoding>());
        }
    }
}
