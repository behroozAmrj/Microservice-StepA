using Microsoft.AspNetCore.Mvc.Filters;

namespace Discount.Grpc.ActionFilter
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
