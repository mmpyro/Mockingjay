using System;
using System.Collections.Generic;
using System.Linq;

namespace MockingJayRoutes.helpers
{
    public class PageBuilder<T>
    {
        private Page<T> _page;

        public PageBuilder()
        {
            _page = new Page<T>();
            _page.Metadata = null;
            _page.Data = Enumerable.Empty<T>();
        }

        public PageBuilder<T> For(IEnumerable<T> data)
        {
            _page.Data = data;
            return this;
        }

        public PageBuilder<T> WithMetadata(string url, int items, int currentPage, int allItems)
        {
            Uri uri = new Uri(url);
            int total = ((allItems - 1)/ items)+1;
            _page.Metadata = new PageMetaData
            {
                Items = items,
                TotalPages = total,
                Self = $"{uri.LocalPath}{uri.Query}"
            };
            if (currentPage < (total-1))
            {
                _page.Metadata.Next = $"{uri.LocalPath}?items={items}&page={++currentPage}";
            }
            return this;
        }

        public Page<T> Build()
        {
            return _page;
        }
    }
}
