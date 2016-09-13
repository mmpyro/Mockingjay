using System;
using System.Collections.Generic;

namespace MockingJayRoutes.helpers
{
    public class QueryStringParser
    {
        private string _url;
        private Dictionary<string, string> _dict = new Dictionary<string, string>();

        public QueryStringParser(string url)
        {
            _url = url;
            Parse();
        }

        public int TakeIntValue(string value, int @default = 0)
        {
            if (_dict.ContainsKey(value))
            {
                int result;
                if(int.TryParse(_dict[value], out result))
                {
                    return result;
                }
            }
            return @default;
        }

        private void Parse()
        {
            Uri uri = new Uri(_url);
            string[] query = uri.Query.Replace("?", "").Split('&');
            foreach (var item in query)
            {
                string[] values = item.Split('=');
                if(values.Length > 1)
                    _dict.Add(values[0], values[1]);
            }
        }
    }
}
