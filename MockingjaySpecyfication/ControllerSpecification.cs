using MockingJay;
using MockingJay.Controllers;
using MockingJay.Validation;
using MockingJayRoutes;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using MockingjaySpecyfication.Helpers;
using System.Collections.Specialized;
using System.Text;
using System;
using MockingJayRoutes.helpers;

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
            var headers = new NameValueCollection();
            const string url = "http://localhost:8888/MockingJay/register";
            var request = Substitute.For<IHttpRequest>();
            var response = Substitute.For<IHttpResponse>();
            response.Headers.Returns(t => headers);
            request.ContentBody.Returns(t => configJson);
            request.HttpMethod.Returns(t => "POST");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new RegisterController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Response = response;
            context.Request = request;
            //When
            routes.Resolve(context);
            var registeredRequests = mockingJayApp.GetAllRegisteredRequest();
            //Then
            Assert.That(registeredRequests.Count(), Is.EqualTo(1));
            Assert.That(response.StatusCode, Is.EqualTo(201));
            Assert.That(response.Headers["Location"], Is.EqualTo("/users"));
        }

        [Test]
        public void ShouldReturnNotFoundResponseWhenRegisterWithNoPostMethod()
        {
            //Given
            const string url = "http://localhost:8888/MockingJay/register";
            var request = Substitute.For<IHttpRequest>();
            var response = Substitute.For<IHttpResponse>();
            request.ContentBody.Returns(t => configJson);
            request.HttpMethod.Returns(t => "PUT");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new RegisterController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Response = response;
            context.Request = request;
            //When
            routes.Resolve(context);
            //Then
            Assert.That(response.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void ShouldRemoveAllRequest()
        {
            //Given
            const string url = "/remove/all";
            var response = Substitute.For<IHttpResponse>();
            var request = Substitute.For<IHttpRequest>();
            request.HttpMethod.Returns(t => "DELETE");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new RemoveAllController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            context.Response = response;
            //When
            mockingJayApp.RegisterMessageIfValid(configBuilder);
            var before = mockingJayApp.GetAllRegisteredRequest().ToList();
            routes.Resolve(context);
            var after = mockingJayApp.GetAllRegisteredRequest().ToList();
            //Then
            Assert.That(before.Count(), Is.EqualTo(1));
            Assert.That(after.Count(), Is.EqualTo(0));
            Assert.That(response.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public void ShouldReturnNotFoundResponseWhenDeleteAllWithNoDeleteMethod()
        {
            //Given
            const string url = "/remove/all";
            var response = Substitute.For<IHttpResponse>();
            var request = Substitute.For<IHttpRequest>();
            request.HttpMethod.Returns(t => "HEAD");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new RemoveAllController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            context.Response = response;
            //When
            routes.Resolve(context);
            //Then
            Assert.That(response.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void ShouldRemoveSpecificRequest()
        {
            //Given
            const string url = "/remove";
            var response = Substitute.For<IHttpResponse>();
            var request = Substitute.For<IHttpRequest>();
            request.ContentBody.Returns(t => reqJson);
            request.HttpMethod.Returns(t => "PUT");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new RemoveController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            context.Response = response;
            //When
            mockingJayApp.RegisterMessageIfValid(configBuilder);
            var before = mockingJayApp.GetAllRegisteredRequest().ToList();
            routes.Resolve(context);
            var after = mockingJayApp.GetAllRegisteredRequest().ToList();
            //Then
            Assert.That(before.Count(), Is.EqualTo(1));
            Assert.That(after.Count(), Is.EqualTo(0));
            Assert.That(response.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public void ShouldReturnNotFoundResponseWhenDeleteWithNoDeleteMethod()
        {
            //Given
            const string url = "/remove";
            var response = Substitute.For<IHttpResponse>();
            var request = Substitute.For<IHttpRequest>();
            request.ContentBody.Returns(t => reqJson);
            request.HttpMethod.Returns(t => "DELETE");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new RemoveController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            context.Response = response;
            //When
            routes.Resolve(context);
            //Then
            Assert.That(response.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void ShouldFillResponse()
        {
            //Given
            const string url = "/users";
            var headers = new NameValueCollection();
            var request = Substitute.For<IHttpRequest>();
            var response = Substitute.For<IHttpResponse>();
            response.Headers.Returns(t => headers);
            response.Body.Returns(t => "hello world");
            request.HttpMethod.Returns(t => "GET");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add(url, new FillController(mockingJayApp));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            context.Response = response;
            mockingJayApp.RegisterMessageIfValid(configBuilder
                                                    .WithResponseHeader("Etag","0000"));
            //When
            routes.Resolve(context);
            //Then
            Received.InOrder(() =>
            {
                response.FillContent(Arg.Any<string>(), Arg.Any<Encoding>());
                response.Close();
            });
            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Body, Is.EqualTo("hello world"));
            Assert.That(response.Headers["Etag"], Is.EqualTo("0000"));
        }

        [Test]
        public void ShouldReturnsAllRegisteredRequests()
        {
            //Given
            const string url = "http://localhost:8080/mockingJay/requests?items=10&page=0";
            var request = Substitute.For<IHttpRequest>();
            var response = Substitute.For<IHttpResponse>();
            request.HttpMethod.Returns(t => "GET");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add("http://localhost:8080/mockingJay/requests", new GetAllRequestsController(mockingJayApp,
                                                                                        new ParserFactory(),
                                                                                        new PageBuilder<Request>()));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            context.Response = response;
            RegisterManyMessages(mockingJayApp, 20);
            //When
            routes.Resolve(context);
            //Then
            Received.InOrder(() =>
            {
                response.FillContent(Arg.Any<string>(), Arg.Any<Encoding>());
                response.Close();
            });
            Assert.That(response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ShouldReturnsAllRegisteredConfigurations()
        {
            //Given
            const string url = "http://localhost:8080/mockingJay/configurations?items=10&page=0";
            var request = Substitute.For<IHttpRequest>();
            var response = Substitute.For<IHttpResponse>();
            request.HttpMethod.Returns(t => "GET");
            request.Url.Returns(t => url);
            var mockingJayApp = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            RouteManager routes = new RouteManager();
            routes.Add("http://localhost:8080/mockingJay/configurations", new GetAllConfigurationController(mockingJayApp,
                                                                                        new ParserFactory(),
                                                                                        new PageBuilder<Configuration>()));
            var context = Substitute.For<IHttpContext>();
            context.Request = request;
            context.Response = response;
            RegisterManyMessages(mockingJayApp, 20);
            //When
            routes.Resolve(context);
            //Then
            Received.InOrder(() =>
            {
                response.FillContent(Arg.Any<string>(), Arg.Any<Encoding>());
                response.Close();
            });
            Assert.That(response.StatusCode, Is.EqualTo(200));
        }

        private void RegisterManyMessages(MockingJayApp mockingJayApp, int number)
        {
            for (int i = 0; i < number; i++) {
                mockingJayApp.RegisterMessageIfValid(configBuilder.WithUrl("aa"+i));
            }
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
