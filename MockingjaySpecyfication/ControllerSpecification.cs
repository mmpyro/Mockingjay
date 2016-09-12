using MockingJay;
using MockingJay.Controllers;
using MockingJay.Validation;
using MockingJayRoutes;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using MockingjaySpecyfication.Helpers;

namespace MockingjaySpecyfication
{
    [TestFixture]
    public class ControllerSpecification
    {
        private string configJson = @"{'url': '/users', 'type': 'GET', 'headers': [{'name': 'Etag', 'value': '0000'}],
                            'return': {'content': 'hello world', 'statusCode': 200,'headers': [{'name': 'Etag', 'value': '0000'}] }}";

        private string reqJson = @"{'url': '/users', 'type': 'GET'}";

        private ConfigBuilder configBuilder = new ConfigBuilder();

        [Test]
        public void ShouldRegisterNewRequest()
        {
            //Given
            const string url = "/register";
            var request = Substitute.For<IHttpRequest>();
            request.ContentBody.Returns(t => configJson);
            request.HttpMethod.Returns(t => "POST");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new RegisterController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            //When
            routes.Resolve(context);
            var registeredRequests = mockingJayApp.GetAllRegisteredRequest();
            //Then
            Assert.That(registeredRequests.Count(), Is.EqualTo(1));
        }


        [Test]
        public void ShouldRemoveAllRequest()
        {
            //Given
            const string url = "/remove/all";
            var request = Substitute.For<IHttpRequest>();
            request.HttpMethod.Returns(t => "DELETE");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new RemoveAllController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            //When
            mockingJayApp.RegisterMessageIfValid(configBuilder);
            var before = mockingJayApp.GetAllRegisteredRequest().ToList();
            routes.Resolve(context);
            var after = mockingJayApp.GetAllRegisteredRequest().ToList();
            //Then
            Assert.That(before.Count(), Is.EqualTo(1));
            Assert.That(after.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldRemoveSpecificRequest()
        {
            //Given
            const string url = "/remove";
            var request = Substitute.For<IHttpRequest>();
            request.ContentBody.Returns(t => reqJson);
            request.HttpMethod.Returns(t => "DELETE");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new RemoveController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            //When
            mockingJayApp.RegisterMessageIfValid(configBuilder);
            var before = mockingJayApp.GetAllRegisteredRequest().ToList();
            routes.Resolve(context);
            var after = mockingJayApp.GetAllRegisteredRequest().ToList();
            //Then
            Assert.That(before.Count(), Is.EqualTo(1));
            Assert.That(after.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldFillResponse()
        {
            //Given
            const string url = "/users";
            var request = Substitute.For<IHttpRequest>();
            var response = Substitute.For<IHttpResponse>();
            request.HttpMethod.Returns(t => "GET");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new FillController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            context.Response = response;
            mockingJayApp.RegisterMessageIfValid(configBuilder);
            //When
            routes.Resolve(context);
            //Then
            Assert.That(response.StatusCode, Is.EqualTo(200));
        }

        private IValidator CreateComplexValidator()
        {
            return new ComplexValidator(
                                new NullResponseValidator(),
                                new UrlValidator(),
                                new StatusValidator());
        }
    }
}
