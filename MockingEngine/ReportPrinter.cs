using System.Text;
using MockingJayRoutes;

namespace MockingJay
{
    public class ReportGenerator
    {
        public string CreateReport(IHttpRequest request)
        {
            StringBuilder sb = new StringBuilder();
            sb
                .AppendLine($"Received: {request.Url}, Method: {request.HttpMethod}")
                .AppendLine("Headers:");
            foreach(string key in request.Headers)
            {
                sb.AppendLine($"\t{key}: {request.Headers[key]}");
            }
            return sb.ToString();
        }
    }
}
