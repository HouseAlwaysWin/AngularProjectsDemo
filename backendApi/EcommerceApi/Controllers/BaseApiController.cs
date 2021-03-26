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
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public  OkObjectResult BaseApiOk(string message=null)  {
            return Ok(new ApiResponse(true,message));
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public OkObjectResult BaseApiOk<T>(T data,string message=null)  {
            return Ok(new ApiResponse<T>(true,data,message));
        }



        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public OkObjectResult BaseApiOk<T>(T data,int pageIndex,int pageSize,int totalCount,string message=null)  {
            return Ok(new ApiPagingResponse<T>(true,pageIndex,pageSize,totalCount,data,message));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
       public NotFoundObjectResult BaseApiNotFound(string message=null)  {
            return NotFound(new ApiResponse(false,message));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public NotFoundObjectResult BaseApiNotFound<T>(T data,string message=null) {
            return NotFound(new ApiResponse<T>(false,data,message));
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public BadRequestObjectResult BaseApiBadRequest(string message=null)  {
            return BadRequest(new ApiResponse(false,message));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public BadRequestObjectResult BaseApiBadRequest<T>(T data,string message=null)  {
            return BadRequest(new ApiResponse<T>(false,data,message));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
         public UnauthorizedObjectResult BaseApiUnauthorized(string message=null)  {
            return Unauthorized(new ApiResponse(false,message));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public UnauthorizedObjectResult BaseApiUnauthorized<T>(T data,string message=null)  {
            return Unauthorized(new ApiResponse<T>(false,data,message));
        }
    }
}