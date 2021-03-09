using Microsoft.AspNetCore.Http;

namespace EcommerceApi.Core.ErrorHandlers
{

    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(int statusCode=StatusCodes.Status200OK, string message = null,T data = default(T)) 
            : base(statusCode, message)
        { 
            Data = data;
        }
        public T Data {get;set;}
        
    }

    public class ApiResponse
    {

        public ApiResponse(int statusCode=StatusCodes.Status200OK,string message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageForStatusCode(statusCode);
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }


        private string GetMessageForStatusCode(int statusCode){
            switch(statusCode){
                case StatusCodes.Status400BadRequest:
                    return "Bad Request";
                case StatusCodes.Status401Unauthorized:
                    return "UnAuthorized";
                case StatusCodes.Status404NotFound:
                    return "NotFound";
                case StatusCodes.Status500InternalServerError:
                    return "InternalServerError";
                default:
                    return "Bad Request";
            }
        }


        
    }
}