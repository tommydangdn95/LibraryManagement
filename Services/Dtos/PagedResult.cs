using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public PagedResult(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = GetTotalPaging(pageNumber, pageSize, count);
        }

        public int GetTotalPaging(int pageNumber, int pageSize, int totalRecords)
        {
            var totalPages = Convert.ToDouble(totalRecords / pageSize);
            var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            return roundedTotalPages;
        }

        public static PagedResult<T> Empty(int pageNumber = 1, int pageSize = 10)
        {
            return new PagedResult<T>(new List<T>(), 0, pageNumber, pageSize);
        }
    }
}
