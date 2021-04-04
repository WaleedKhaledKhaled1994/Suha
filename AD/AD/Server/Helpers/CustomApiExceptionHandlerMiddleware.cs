using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Helpers
{
    public class CustomApiExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomApiExceptionHandlerMiddleware> _logger;

        public CustomApiExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<CustomApiExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            IActionResult result = null;

            code = HttpStatusCode.BadRequest;
            result = new NotFoundObjectResult(ApiResponse.Create(code, null, exception.Message));

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (result == null)
            {
                var message = "somethingWentWrong";
                result = new JsonResult(ApiResponse.Create(code, exception.InnerException?.Message, message));
            }
            _logger.LogError(exception, "exception thrown by the user '{@Name}' at route '{@Route}'", context.User.Identity.Name, context.Request.GetDisplayUrl());
            await result.ExecuteResultAsync(new ActionContext() { HttpContext = context });
        }
    }

    public static class CustomApiExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomApiExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomApiExceptionHandlerMiddleware>();
        }
    }
}
