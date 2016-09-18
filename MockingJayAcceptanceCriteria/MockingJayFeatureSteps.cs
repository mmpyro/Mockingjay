using MockingJay;
using System;
using System.Net.Http;
using TechTalk.SpecFlow;
using MockingJayAcceptanceCriteria.helpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using TechTalk.SpecFlow.Assist;
using System.Linq;

namespace MockingJayAcceptanceCriteria
{
    [Binding]
    public class MockingJayFeatureSteps
    {
        private readonly HttpClient _client;

        public MockingJayFeatureSteps(HttpClient client)
        {
            _client = client;
        }

        [Given(@"I have http request for register mock response")]
        public void GivenIHaveHttpRequestForRegisterMockResponse()
        {
            Configuration configuration = new Configuration()
            {
                Url = "http://localhost:51111/users",
                Type = "GET",
                Return = new Response
                {
                    StatusCode = 200,
                    ContentType = "text/plain",
                    Content = "Hello World",
                    Headers = HeaderHelper.CreateHeaders("ETag:0000","Access-Control-Allow-Origin:*")
                }
            };

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:51111/mockingJay/register"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(configuration))
            };
            ScenarioContext.Current["request"] = request;
        }

        [Given(@"I have http request for remove all mock response")]
        public void GivenIHaveHttpRequestForRemoveAllMockResponse()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:51111/mockingJay/remove/all"),
                Method = HttpMethod.Delete
            };
            ScenarioContext.Current["request"] = request;
        }

        [Given(@"I have http request for get all mock requests")]
        public void GivenIHaveHttpRequestForGetAllMockRequests()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:51111/mockingJay/requests"),
                Method = HttpMethod.Get
            };
            ScenarioContext.Current["request"] = request;
        }

        [Given(@"I have http request for get all registered configuration")]
        public void GivenIHaveHttpRequestForGetAllRegisteredConfiguration()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:51111/mockingJay/configurations"),
                Method = HttpMethod.Get
            };
            ScenarioContext.Current["request"] = request;
        }

        [Given(@"I have http request for remove signle registered configuration")]
        public void GivenIHaveHttpRequestForRemoveSignleRegisteredConfiguration()
        {
            var content = new Request
            {
                Url = "http://localhost:51111/users",
                Type = "GET"
            };

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:51111/mockingJay/remove"),
                Method = HttpMethod.Put,
                Content = new StringContent(JsonConvert.SerializeObject(content))
            };
            ScenarioContext.Current["request"] = request;
        }

        [Given(@"I have http request for ask about registered message")]
        public void GivenIHaveHttpRequestForAskAboutRegisteredMessage()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:51111/users"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("ETag","0000");
            ScenarioContext.Current["request"] = request;
        }

        [When(@"I send it into mockingjay")]
        public void WhenISendItIntoMockingjay()
        {
            var request = ScenarioContext.Current["request"] as HttpRequestMessage;
            var response = _client.SendAsync(request).Result;
            ScenarioContext.Current["response"] = response;
        }

        [Then(@"the response Status code is (.*)")]
        public void ThenTheResponseStatusCodeIsNoContent(string statusCode)
        {
            if (ScenarioContext.Current.ContainsKey("response"))
            {
                HttpStatusCode expectedStatus = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCode);
                var response = ScenarioContext.Current["response"] as HttpResponseMessage;
                Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
            }
            else
            {
                Assert.Fail("Response was not present in ScenarioContext");
            }
        }

        [Then(@"the register response Status code is (.*) and contains headers")]
        public void ThenRegisterResponse(string statusCode,Table table)
        {
            if(ScenarioContext.Current.ContainsKey("response"))
            {
                HttpStatusCode expectedStatus = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCode);
                var response = ScenarioContext.Current["response"] as HttpResponseMessage;
                var expectedHeaders = table.CreateSet<Header>().ToArray();

                Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
                Assert.IsNotNull(response.Headers.Location);
                Assert.That(response.Headers.Location.AbsoluteUri, Is.EqualTo(expectedHeaders[0].Value));
            }
            else
            {
                Assert.Fail("Response was not present in ScenarioContext");
            }
        }
    }
}
