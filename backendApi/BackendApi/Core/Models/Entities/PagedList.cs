using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BackendApi.Core.Models.Entities
{
    public class PagedList<T> 
    {
        public PagedList(List<T> data,int totalCount)
        {
           Data = data; 
           TotalCount = totalCount;
        }
        public List<T> Data {get;set;}

        public int TotalCount {get; set;}

    }
}