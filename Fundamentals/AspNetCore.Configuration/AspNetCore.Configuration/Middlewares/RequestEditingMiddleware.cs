﻿namespace AspNetCore.Configuration.Middlewares
{
    public class RequestEditingMiddleware
    {
        private RequestDelegate _next;
        public RequestEditingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Items["Firefox"] = httpContext.Request.Headers["User-Agent"].Any(v => v.Contains("Firefox"));
            await _next(httpContext);
        }
    }
}
