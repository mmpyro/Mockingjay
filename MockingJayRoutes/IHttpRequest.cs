using System.Collections.Specialized;

namespace MockingJayRoutes
{
    public interface IHttpRequest
    {
        string ContentBody { get; set; }
        NameValueCollection Headers { get; set; }
        string HttpMethod { get; set; }
        string Url { get; }
    }
}