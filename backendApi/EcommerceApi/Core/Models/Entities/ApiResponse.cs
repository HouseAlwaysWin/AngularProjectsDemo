using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace EcommerceApi.Core.Entities
{

    public class ApiPagingResponse<T>:ApiResponse{
        public ApiPagingResponse(
            bool isSuccessed, int pageIndex, int pageSize,
            int totalCount,T data, string message = null)
            : base(isSuccessed,message)
        {
           this.Data = data; 
           this.PageIndex = pageIndex;
           this.PageSize = pageSize;
           this.TotalCount = totalCount;
        }

        public ApiPagingResponse(int pageIndex, int pageSize,
            int totalCount,T data, string message = null)
            : base(true,message)
        {
           this.Data = data; 
           this.PageIndex = pageIndex;
           this.PageSize = pageSize;
           this.TotalCount = totalCount;
        }

        public T Data {get;set;}
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;

    }

    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(bool isSuccessed,T data, string message = null) 
            : base(isSuccessed,message)
        { 
            Data = data;
        }

        public ApiResponse(T data, string message = null)
            :base(false,message)
        {
            Data = data;  
        }
        public T Data {get;set;}
        
    }

    public class ApiResponse
    {
        public ApiResponse(bool isSuccessed=true,string message=null)
        {
            this.IsSuccessed = isSuccessed;
            this.Message = message;
        }

        public ApiResponse(string message=null)
        {
            this.IsSuccessed = true;
            this.Message = message;
        }

        

        // public int StatusCode { get; set; }
        public bool IsSuccessed { get; set; }
        public string Message { get; set; }


        // private string GetMessageForStatusCode(int statusCode){
        //     switch(statusCode){
        //         case StatusCodes.Status200OK:
        //             return "Successed";
        //         case StatusCodes.Status400BadRequest:
        //             return "Bad Request";
        //         case StatusCodes.Status401Unauthorized:
        //             return "UnAuthorized";
        //         case StatusCodes.Status404NotFound:
        //             return "NotFound";
        //         case StatusCodes.Status500InternalServerError:
        //             return "InternalServerError";
        //         default:
        //             return "Bad Request";
        //     }
        // }


        
    }
}