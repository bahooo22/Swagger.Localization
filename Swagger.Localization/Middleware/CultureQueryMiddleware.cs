using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Microsoft.AspNetCore.Http;

namespace Swagger.Localization.Middleware
{
    public class CultureQueryMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureQueryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var lang = context.Request.Query["lang"].ToString();

            if (string.IsNullOrEmpty(lang))
            {
                lang = context.Request.Headers["Accept-Language"]
                    .ToString()
                    .Split(',').FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(lang))
            {
                try
                {
                    var culture = new CultureInfo(lang);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }
                catch (CultureNotFoundException)
                {
                    // fallback silently
                }
            }

            await _next(context);
        }
    }
}
