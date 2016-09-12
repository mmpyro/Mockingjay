using MockingJay;
using MockingjaySpecyfication.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MockingjaySpecyfication
{
    [TestFixture]
    public class StructureSpecification
    {
        [Test]
        public void RequestShouldEquals()
        {
            //Given
            var requestBuilder = new RequestBuilder();
            string url = "/users";
            Request req1 = requestBuilder.WithUrl(url).Get()
                                         .WithHeader("Etag", "0000")
                                         .WithHeader("Cache-Control", "public, max-age=3600");

            Request req2 = requestBuilder.WithUrl(url).Get()
                                         .WithHeader("Cache-Control", "public, max-age=3600")
                                         .WithHeader("Etag", "0000");
            //When
            bool areEqual = req1.Equals(req2);
            //Then
            Assert.That(areEqual, Is.True);
        }

        [Test]
        public void RequestShouldNotEquals()
        {
            //Given
            var requestBuilder = new RequestBuilder();
            string url = "/users";
            Request req1 = requestBuilder.WithUrl(url).Get().WithHeader("Etag", "0000");
            Request req2 = requestBuilder.WithUrl(url).Get().WithHeader("Etag", "0001");
            //When
            bool areEqual = req1.Equals(req2);
            //Then
            Assert.That(areEqual, Is.False);
        }

        [Test]
        public void HeaderShouldBeEqual()
        {
            //Given
            var header1 = new Header
            {
                Name = "Etag",
                Value = "0000"
            };

            var header2 = new Header
            {
                Name = "Etag",
                Value = "0000"
            };
            //When
            bool areEqual = header1.Equals(header2);
            //Then
            Assert.That(areEqual, Is.True);
            Assert.AreEqual(header1.GetHashCode(), header2.GetHashCode());
        }

        [Test]
        public void HeaderShouldNotBeEqual()
        {
            //Given
            var header1 = new Header
            {
                Name = "Etag",
                Value = "0000"
            };

            var header2 = new Header
            {
                Name = "Etag",
                Value = "0001"
            };
            //When
            bool areEqual = header1.Equals(header2);
            //Then
            Assert.That(areEqual, Is.False);
            Assert.AreNotEqual(header1.GetHashCode(), header2.GetHashCode());
        }

        [Test]
        public void ShouldDeSerializeConfigurationFromJSON()
        {
            //Given
            string json = @"{'url': '/users', 'type': 'GET', 'headers': [{'name': 'Etag', 'value': '0000'}],
                            'return': {'content': 'hello world', 'statusCode': 200,'headers': [{'name': 'Etag', 'value': '0000'}] }}";
            List<Header> headers = new List<Header>();
            headers.Add(new Header { Name = "Etag", Value = "0000" });
            //When
            Configuration conf = JsonConvert.DeserializeObject<Configuration>(json);
            var @return = conf.Return;
            //Then
            Assert.That(conf.Url, Is.EqualTo("/users"));
            Assert.That(conf.Type, Is.EqualTo(HttpMethodType.GET));
            CollectionAssert.AreEquivalent(conf.Headers, headers);
            Assert.That(@return.Content, Is.EqualTo("hello world"));
            Assert.That(@return.StatusCode, Is.EqualTo(200));
            CollectionAssert.AreEquivalent(@return.Headers, headers);
        }
    }
}
