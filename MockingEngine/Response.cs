using System.Collections.Generic;

namespace MockingJay
{
    public class Response
    {
        public string Content { get; set; }
        public List<Header> Headers { get; set; }
        public int StatusCode { get; set; }
        public string ContentType { get; set; }

        public Response()
        {
            Headers = new List<Header>();
            ContentType = "application/json";
        }
    }

    public class Header
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            var header = obj as Header;
            if(header != null)
            {
                return header.Name.Equals(Name) && header.Value.Equals(Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            string toHash = Name + Value;
            return toHash.GetHashCode();
        }
    }
}