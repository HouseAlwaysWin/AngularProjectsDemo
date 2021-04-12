using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EcommerceApi.Core.Data.QuerySpecs
{
    public interface IQuerySpec<T>
    {
        Expression<Func<T,bool>> Where {get;}
        List<Expression<Func<T,object>>> Includes {get;}
        List<Expression<Func<T,object>>> ThenIncludes {get;}

        Expression<Func<T,object>> OrderBy {get;}
        Expression<Func<T,object>> OrderByDescending {get;}

        int Take {get;}
        int Skip{get;}
        bool IsPagingEnabled { get;}

    }
}