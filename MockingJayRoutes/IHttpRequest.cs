using System.Collections.Specialized;
using System.Text;

namespace MockingJayRoutes
{
    public interface IHttpRequest
    {
        string ContentBody { get; set; }
        NameValueCollection Headers { get; set; }
        string HttpMethod { get; set; }
        string Url { get; }
        Encoding ContentEncoding { get; set; }
    }
}