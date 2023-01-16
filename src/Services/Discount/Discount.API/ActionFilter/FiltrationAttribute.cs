using Microsoft.AspNetCore.Mvc.Filters;

namespace Discount.API.ActionFilter
{
    public class FiltrationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
