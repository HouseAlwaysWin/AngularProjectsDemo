using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceApi.Helpers.Filters
{
    public class ApiResponseAttribute:ActionFilterAttribute
    {
     public override void OnActionExecuted(ActionExecutedContext 
                                          context)
    {
        var result = context.Result;
        // Do something with Result.
        if (context.Canceled == true)
        {
            // Action execution was short-circuited by another filter.
        }

        if(context.Exception != null)
        {
            // Exception thrown by action or action filter.
            // Set to null to handle the exception.
            context.Exception = null;
        }
        base.OnActionExecuted(context);
    } 
    }
}