using MockingJay;
using System.Collections.Generic;

namespace MockingJayAcceptanceCriteria.helpers
{
    internal class HeaderHelper
    {
        public static List<Header> CreateHeaders(params string[] headers)
        {
            var list = new List<Header>();
            foreach(string item in headers)
            {
                string[] header = item.Split(':');
                list.Add(new Header
                {
                    Name = header[0],
                    Value = header[1]
                });
            }
            return list;
        }
    }
}
