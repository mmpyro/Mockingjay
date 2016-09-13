namespace MockingJayRoutes.helpers
{
    public class ParserFactory
    {
        public QueryStringParser CreateQueryStringParser(string url)
        {
            return new QueryStringParser(url);
        }
    }
}
