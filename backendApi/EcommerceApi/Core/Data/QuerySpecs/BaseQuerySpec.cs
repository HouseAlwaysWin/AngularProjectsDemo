// using System;
// using System.Collections.Generic;
// using System.Linq.Expressions;

// namespace EcommerceApi.Core.Data.QuerySpecs
// {
//     public class BaseQuerySpec<T> : IQuerySpec<T>
//     {
//         public BaseQuerySpec()
//         {
//         }

//         public BaseQuerySpec(Expression<Func<T,bool>> where)
//         {
//            Where = where; 
//         }
//         public Expression<Func<T, bool>> Where {get;}

//         public List<Expression<Func<T, object>>> Includes {get;} = new List<Expression<Func<T, object>>>();

//         public List<Expression<Func<T, object>>> ThenIncludes {get;} = new List<Expression<Func<T, object>>>();

//        public Expression<Func<T, object>> OrderBy  {get; private set;}

//         public Expression<Func<T, object>> OrderByDescending  {get;private set;}

//         public int Take  {get; private set;}

//         public int Skip  {get; private set;}
//         public bool IsPagingEnabled  {get; private set;}     

//        protected void AddInclude(Expression<Func<T,object>> includeExpression){
//             Includes.Add(includeExpression);
            
//         }

//         protected void AddOrderBy(Expression<Func<T,object>> orderByExpression){
//             OrderBy = orderByExpression;
//         }

//         protected void ApplyPaging(int skip, int take){
//             Skip = skip;
//             Take = take;
//             IsPagingEnabled = true;
//         }

//         protected void AddOrderByDescending(Expression<Func<T,object>> orderByDescExpression){
//             OrderByDescending = orderByDescExpression;
//         }
//     }
// }