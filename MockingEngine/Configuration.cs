using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockingJay
{
    public class Configuration
    {
        public Response Return { get; set; }
        public string Type { get; set; }
        public List<Header> Headers { get; set; }
        public string Url { get; set; }

        public Configuration()
        {
            Headers = new List<Header>();
        }

        public Request CreateRequest()
        {
            return new Request
            {
                Url = this.Url,
                Type = this.Type,
                Headers = this.Headers
            };
        }
    }

    public class Request
    {
        public string Url { get; set; }
        public string Type { get; set; }
        public List<Header> Headers { get; set; }

        public Request()
        {
            Headers = new List<Header>();
        }

        public override bool Equals(object obj)
        {
            var req = obj as Request;
            if(req != null)
            {
                return Url.Equals(req.Url) && Type.Equals(req.Type)
                        && req.Headers.OrderBy(t => t.Name).SequenceEqual(Headers.OrderBy(t => t.Name));
            }
            return false;
        }

        public override int GetHashCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Url).Append(Type.ToString());
            foreach (var item in Headers)
            {
                sb.Append(item.Name + item.Value);
            }
            return sb.ToString().GetHashCode();
        }
    }
}