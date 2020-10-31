using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tgs.Common.Util.General
{
    public class Result
    {
        public int Codigo { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string MessageExeption { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Informacion { get; set; }
    }

    public class Result<T>
    {
        public T Data { get; set; }
        public int Codigo { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string MessageExeption { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Informacion { get; set; }
    }

    public class PageFilter
    {
        public int Page { get; set; }
        public int Length { get; set; }
        public string SearchText { get; set; }
        public int Filter { get; set; }
        public string Order { get; set; }
        public string ColumnOrder { get; set; }

    }

    public class PageResult<T>
    {
        public List<T> Data = new List<T>();
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
        public KeyValuePair<int, bool>[] NumbersPages { get; set; }
        public int Itemperpage { get; set; }
        public int? Total { get; set; }
        public int PageCount { get; set; }
    }

    public class PaginationResult
    {
        public int offset { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public KeyValuePair<int, bool>[] numbersPages { get; set; }
        public int itemperpage { get; set; }
        public int total { get; set; }
        public int pageCount { get; set; }
    }

    public static class PageResultUtil
    {
        public static PaginationResult ResultadoPagination(int pageNo, int pageSize, int totalRecordCount)
        {
            PaginationResult result = new PaginationResult();

            var limit = totalRecordCount <= (pageNo * pageSize) ? totalRecordCount : (pageNo * pageSize);
            var pageCount = totalRecordCount > 0 ? (int)Math.Ceiling(totalRecordCount / (double)pageSize) : 0;
            var numberPages = GetNumberPages(pageNo, pageCount);

            result.offset = ((pageNo - 1) * pageSize) + 1;
            result.limit = limit;
            result.page = pageNo;
            result.itemperpage = pageSize;
            result.total = totalRecordCount;
            result.pageCount = pageCount;
            result.numbersPages = numberPages;

            return result;
        }

        private static KeyValuePair<int, bool>[] GetNumberPages(int pageNo, int pageCount)
        {
            var numberPages = new Dictionary<int, bool>();
            var startPage = (int)(Math.Ceiling((decimal)pageNo / 10 - 1) * 10) + 1;
            var endPage = (int)Math.Min(startPage + 9, pageCount);
            for (int i = startPage; i <= endPage; i++)
            {
                numberPages.Add(i, i == pageNo);
            }
            return numberPages.ToArray();
        }

    }

}
