using MockingJay;
using Ploeh.AutoFixture;
using NUnit.Framework;
using System;
using MockingjaySpecyfication.Helpers;
using MockingJay.Validation;
using System.Collections.Generic;
using System.Linq;

namespace MockingjaySpecyfication
{
    [TestFixture]
    public class MockingJaySpecification
    {
        private Fixture _any = new Fixture();

        [Test]
        public void ShouldRegisterBasicRequest()
        {
            //Given
            MockEngine mock = new MockEngine();
            string url = "/users?page=1&items=10";
            string response = _any.Create<string>();
            var requestBuilder = new RequestBuilder();
            //When
            mock.Register(new ConfigBuilder()
                                .WithUrl(url)
                                .WithResponseContent(response));
            //Then
            var result = mock.Resolve(requestBuilder
                                        .WithUrl(url)
                                        .Get());
            Assert.That(result.Content, Is.EqualTo(response));
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }
        
        [Test]
        public void ShouldRegisterConfigurationWithTheSameTypeAndUriWhenHeadersAreDifferent()
        {
            //Given
            MockEngine mock = new MockEngine();
            string url = "/users?page=1&items=10";
            string etag = _any.Create<string>();
            string response1 = _any.Create<string>();
            string response2 = _any.Create<string>();
            var requestBuilder = new RequestBuilder();
            //When
            mock.Register(new ConfigBuilder()
                    .WithUrl(url)
                    .WithRequestHeader("Cache-Control","public, max-age=3600")
                    .WithResponseContent(response1));

            mock.Register(new ConfigBuilder()
                    .WithUrl(url)
                    .WithRequestHeader("Etag", etag)
                    .WithResponseContent(response2));

            var cacheResponse = mock.Resolve(requestBuilder
                            .WithUrl(url)
                            .WithHeader("Cache-Control", "public, max-age=3600")
                            .Get());

            var etagResponse = mock.Resolve(requestBuilder
                .WithUrl(url)
                .WithHeader("Etag", etag)
                .Get());
            //Then
            Assert.That(cacheResponse.Content, Is.EqualTo(response1));
            Assert.That(etagResponse.Content, Is.EqualTo(response2));
        }
        
        [Test]
        public void ShouldThrowExceptionWhenUrlWasRegisteredBefore()
        {
            //Given
            MockEngine mock = new MockEngine();
            string url = _any.Create<string>();
            var configBuilder = new ConfigBuilder();
            //When 
            mock.Register(configBuilder.WithUrl(url));

            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                mock.Register(configBuilder.WithUrl(url));
            });
            //Then
            Assert.That(ex.Message, Is.EqualTo("This request was register before."));
        }
        
        [Test]
        public void ShouldThrowExceptionWhenUrlWasNotFound()
        {
            //Given
            MockEngine mock = new MockEngine();
            string url = _any.Create<string>();
            var configBuilder = new ConfigBuilder();
            var requestBuilder = new RequestBuilder();
            //When
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var res = mock.Resolve(requestBuilder.WithUrl(url));
            });
            //Then
            Assert.That(ex.Message, Is.EqualTo("This request was not register before."));
        }

        [Test]
        public void ShouldPerformWholeFlow()
        {
            //Given
            MockingJayApp mockingJay = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            var configBuilder = new ConfigBuilder();
            var requestBuilder = new RequestBuilder();
            string url = "/users";
            //When
            mockingJay.RegisterMessageIfValid(configBuilder
                                                .Post()
                                                .WithUrl(url)
                                                .WithResponseCode(201)
                                                .WithResponseContent("Success")
                                                .WithResponseHeader("Cache-Control","public,max-age=3600"));

            Response response = mockingJay.Resolve(requestBuilder
                                                        .WithUrl(url)
                                                        .Post());
            //Then
            Assert.That(response.StatusCode, Is.EqualTo(201));
            Assert.That(response.Content, Is.EqualTo("Success"));
            Assert.That(response.Headers.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldExceptionWhenConfigurationIsNotValid()
        {
            //Given
            MockingJayApp mockingJay = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            var configBuilder = new ConfigBuilder();
            string url = "'http://localhost:8080/users";
            //When
            var ex = Assert.Throws<ArgumentException>(() =>  mockingJay.RegisterMessageIfValid(configBuilder
                                    .WithUrl(url)
                                    .WithResponseCode(201)
                                    .WithResponseContent("Success")));
            //Then
            Assert.That(ex.Message, Is.EqualTo($"This: {url} is not valid url."));
        }

        [Test]
        public void ShouldReturnAllConfigurationObjects()
        {
            //Given
            MockingJayApp mockingJay = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            var configBuilder = new ConfigBuilder();
            string url = "/users";
            //When
            mockingJay.RegisterMessageIfValid(configBuilder
                                    .Post()
                                    .WithUrl(url)
                                    .WithResponseCode(201)
                                    .WithResponseContent("Success")
                                    .WithRequestHeader("Cache-Control", "public,max-age=3600"));

            mockingJay.RegisterMessageIfValid(configBuilder
                                    .Post()
                                    .WithUrl(url)
                                    .WithResponseCode(201)
                                    .WithResponseContent("Success")
                                    .WithRequestHeader("ETag", _any.Create<string>()));

            IEnumerable<Configuration> requests = mockingJay.GetAllRegisteredConfiguration();
            //Then
            Assert.That(requests.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ShouldReturnAllRequestsObjects()
        {
            //Given
            MockingJayApp mockingJay = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            var configBuilder = new ConfigBuilder();
            string url = "/users";
            //When
            mockingJay.RegisterMessageIfValid(configBuilder
                                    .Post()
                                    .WithUrl(url)
                                    .WithResponseCode(201)
                                    .WithResponseContent("Success")
                                    .WithRequestHeader("Cache-Control", "public,max-age=3600"));

            mockingJay.RegisterMessageIfValid(configBuilder
                                    .Post()
                                    .WithUrl(url)
                                    .WithResponseCode(201)
                                    .WithResponseContent("Success")
                                    .WithRequestHeader("ETag", _any.Create<string>()));

            IEnumerable<Request> requests = mockingJay.GetAllRegisteredRequest();
            //Then
            Assert.That(requests.Count(), Is.EqualTo(2));
        }

        
        [Test]
        public void ShouldRemoveSpecificConfiguration()
        {
            //Given
            MockingJayApp mockingJay = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            var configBuilder = new ConfigBuilder();
            string url = "/users";
            //When
            mockingJay.RegisterMessageIfValid(configBuilder
                                    .Post()
                                    .WithUrl(url)
                                    .WithResponseCode(201)
                                    .WithResponseContent("Success")
                                    .WithRequestHeader("Cache-Control", "public,max-age=3600"));

            var before = mockingJay.GetAllRegisteredRequest().ToArray();
            mockingJay.DeleteRequest(before[0]);
            var after = mockingJay.GetAllRegisteredRequest().ToArray();
            //Then
            Assert.That(before.Length, Is.EqualTo(1));
            Assert.That(after.Length, Is.EqualTo(0));
        }
        
        [Test]
        public void ShouldThrowsExceptionWhenTryRemoveUnregisteredRequest()
        {
            //Given
            MockingJayApp mockingJay = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            var requestBuilder = new RequestBuilder();
            //When
            var ex = Assert.Throws<ArgumentException>(() => mockingJay.DeleteRequest(requestBuilder
                                                                    .WithUrl("/users").Get()));
            //Then
            Assert.That(ex.Message, Is.EqualTo("This request wasn't register before. Cannot remove unregistered request."));
        }

        [Test]
        public void ShouldRemoveAllRequests()
        {
            //Given
            MockingJayApp mockingJay = new MockingJayApp(new MockEngine(), CreateComplexValidator());
            var configBuilder = new ConfigBuilder();
            //When
            mockingJay.RegisterMessageIfValid(configBuilder
                        .Post()
                        .WithUrl(_any.Create<string>())
                        .WithResponseCode(201)
                        .WithResponseContent("Success"));

            mockingJay.RegisterMessageIfValid(configBuilder
                        .Post()
                        .WithUrl(_any.Create<string>())
                        .WithResponseCode(201)
                        .WithResponseContent("Success"));

            var before = mockingJay.GetAllRegisteredConfiguration().ToArray();
            mockingJay.DeleteAll();
            var after = mockingJay.GetAllRegisteredConfiguration().ToArray();
            //Then
            Assert.That(before.Count(), Is.EqualTo(2));
            Assert.That(after.Count(), Is.EqualTo(0));
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
