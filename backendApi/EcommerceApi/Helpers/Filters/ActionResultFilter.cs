using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceApi.Helpers.Filters
{
    public class ActionResultFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }

    public class AsyncActionResultFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
        }
    }
}