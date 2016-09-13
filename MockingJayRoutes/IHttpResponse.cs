using System.Collections.Specialized;
using System.Text;

namespace MockingJayRoutes
{
    public interface IHttpResponse
    {
        string ContentType { get; set; }
        int StatusCode { get; set; }
        NameValueCollection Headers { get; set; }
        string Body { get; }
        void FillContent(string message, Encoding encoding);
        void Close();
    }
}