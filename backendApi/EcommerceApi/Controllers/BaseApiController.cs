using EcommerceApi.Core.Entities;
using EcommerceApi.Helpers.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route ("api/[controller]")]
    public class BaseApiController:ControllerBase
    {
        public  ActionResult BaseApiOk(string message=null)  {
            return Ok(new ApiResponse(true,message));
        }

        public ActionResult BaseApiOk<T>(T data,string message=null)  {
            return Ok(new ApiResponse<T>(true,data,message));
        }

        public ActionResult BaseApiOk<T>(T data,int pageIndex,int pageSize,int totalCount,string message=null)  {
            return Ok(new ApiPagingResponse<T>(true,pageIndex,pageSize,totalCount,data,message));
        }

       public ActionResult BaseApiNotFound(string message=null)  {
            return NotFound(new ApiResponse(false,message));
        }

        public ActionResult BaseApiNotFound<T>(T data,string message=null) {
            return NotFound(new ApiResponse<T>(false,data,message));
        }

        public ActionResult BaseApiBadRequest(string message=null)  {
            return BadRequest(new ApiResponse(false,message));
        }

        public ActionResult BaseApiBadRequest<T>(T data,string message=null)  {
            return BadRequest(new ApiResponse<T>(false,data,message));
        }

         public ActionResult BaseApiUnauthorized(string message=null)  {
            return Unauthorized(new ApiResponse(false,message));
        }

        public ActionResult BaseApiUnauthorized<T>(T data,string message=null)  {
            return Unauthorized(new ApiResponse<T>(false,data,message));
        }
    }
}