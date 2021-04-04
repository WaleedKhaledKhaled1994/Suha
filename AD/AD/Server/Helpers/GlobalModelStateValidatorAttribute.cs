using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Helpers
{
    public class GlobalModelStateValidatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var route = context.HttpContext.Request.Path.Value;
            if (!context.ModelState.IsValid)
            {
                if (route.Contains("/api/"))
                {
                    var message = string.Empty;
                    foreach (var modelState in context.ModelState.Values)
                    {
                        var firstError = modelState.Errors.FirstOrDefault();
                        if (firstError != null)
                            message = firstError.ErrorMessage;
                        else
                            message = "Validation Error";
                        context.Result = new OkObjectResult(HttpStatusCode.BadRequest);
                    }
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
