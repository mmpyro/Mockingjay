using NUnit.Framework;
using MockingJayRoutes;
using System.Linq;
using MockingJayRoutes.helpers;

namespace MockingjaySpecyfication
{
    [TestFixture]
    public class PageSpecification
    {
        [Test]
        public void ShouldCreateBasicPageginationStructure()
        {
            //Given
            var data = Enumerable.Range(0, 20);
            int items = 10;
            int currentPage = 0;
            //When
            Page<int> page = new PageBuilder<int>()
                                .For(data)
                                .WithMetadata($"http://localhost:8008/api/requests?items={items}&page={currentPage}",
                                                    items, currentPage, data.Count())
                                .Build();
            //Then
            Assert.That(page.Metadata.Items, Is.EqualTo(10));
            Assert.That(page.Metadata.TotalPages, Is.EqualTo(2));
        }

        [TestCase(10,0)]
        [TestCase(20, 1)]
        [TestCase(30, 2)]
        public void ShouldTakeValuesFromQueryString(int numberOfItems, int pageNumber)
        {
            //Given
            string url = $"http://localhost:8008/api/requests?items={numberOfItems}&page={pageNumber}";
            QueryStringParser parser = new QueryStringParser(url);
            int items, page;
            //When
            items = parser.TakeIntValue("items");
            page = parser.TakeIntValue("page");
            //Then
            Assert.That(items, Is.EqualTo(numberOfItems));
            Assert.That(page, Is.EqualTo(pageNumber));
        }

        [Test]
        public void ShouldReturnDefaultValues()
        {
            //Given
            string url = $"http://localhost:8008/api/requests";
            QueryStringParser parser = new QueryStringParser(url);
            int items, page;
            //When
            items = parser.TakeIntValue("items",10);
            page = parser.TakeIntValue("page",0);
            //Then
            Assert.That(items, Is.EqualTo(10));
            Assert.That(page, Is.EqualTo(0));
        }
    }
}
