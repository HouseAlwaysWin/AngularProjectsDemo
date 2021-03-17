using System.Collections.Generic;

namespace EcommerceApi.Helpers
{
    public class Pagination<T> where T:class
    {
       public Pagination(int pageIndex,int pageSize,int totalCount,IReadOnlyList<T> data)
       {
          this.PageIndex = pageIndex;
          this.PageSize = pageSize;
          this.TotalCount = totalCount;
          this.Data = data; 
       } 

       public int PageIndex { get; set; }
       public int PageSize { get; set; }
       public int TotalCount { get; set; }
       public IReadOnlyList<T> Data { get; set; }
    }
}