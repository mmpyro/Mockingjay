using MockingJay;
using MockingJayRoutes;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockingjaySpecyfication
{
    [TestFixture]
    public class ReportSpecification
    {
        private NameValueCollection headers;

        public ReportSpecification()
        {
            headers = new NameValueCollection();
            headers.Add("Host", "localhost:51111");
            headers.Add("User-Agent", "Mozilla");
        }

        [Test]
        public void ShouldCreateReportBasedOnRequest()
        {
            //Given
            var request = Substitute.For<IHttpRequest>();
            request.Url.Returns(t => "http://localhost:51111/users");
            request.HttpMethod.Returns(t => "GET");
            request.Headers.Returns(t => headers);
            ReportGenerator printer = new ReportGenerator();
            //When
            string report = printer.CreateReport(request);
            string[] lines = report.Split('\n');
            //Then
            Assert.That(lines.Length, Is.EqualTo(5));
            Console.WriteLine(report);
        }
    }
}
