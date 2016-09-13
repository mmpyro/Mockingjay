using System.Collections.Generic;

namespace MockingJayRoutes
{
    public class Page<T>
    {
        public PageMetaData Metadata { get; set; }
        public IEnumerable<T> Data { get; set; }
    }

    public class PageMetaData
    {
        public int TotalPages { get; set; }
        public int Items { get; set; }
        public string Next { get; set; }
        public string Self { get; set; }
    }
}
