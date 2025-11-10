using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Swagger.Localization.Middleware
{
    public class SwaggerLocalizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IList<CultureInfo> _supportedCultures;

        public SwaggerLocalizationMiddleware(RequestDelegate next, IList<CultureInfo> supportedCultures)
        {
            _next = next;
            _supportedCultures = supportedCultures;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Приоритет: query string -> header -> культура по умолчанию
            var cultureQuery = context.Request.Query["lang"].FirstOrDefault();
            var cultureHeader = context.Request.Headers["Accept-Language"].FirstOrDefault();

            var cultureName = cultureQuery ?? cultureHeader ?? CultureInfo.CurrentCulture.Name;

            var culture = _supportedCultures.FirstOrDefault(c => c.Name.Equals(cultureName, StringComparison.OrdinalIgnoreCase))
                          ?? _supportedCultures.FirstOrDefault()
                          ?? CultureInfo.CurrentCulture;

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            await _next(context);
        }
    }
}