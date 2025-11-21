using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HMS.Helper
{
    public class CheckAccess : ActionFilterAttribute, IAuthorizationFilter
    {
        //public void OnAuthorization(AuthorizationFilterContext filterContext)
        //{
        //    if (filterContext.HttpContext.Session.GetString("UserID") == null)
        //    {
        //        filterContext.Result = new RedirectResult("~/Users/Login");
        //    }
        //}
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var endpoint = filterContext.HttpContext.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return; // Skip auth for AllowAnonymous
            }

            if (filterContext.HttpContext.Session.GetString("UserID") == null)
            {
                filterContext.Result = new RedirectResult("~/Users/Login");
            }
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Expires"] = "-1";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            base.OnResultExecuting(context);

        }
    }
}
