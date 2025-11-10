using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swagger.Localization.Filters;
using Swagger.Localization.Middleware;


namespace Swagger.Localization.Extensions;

public static class SwaggerLocalizationExtensions
{
    public static IServiceCollection AddLocalizedSwagger(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.DocumentFilter<LocalizationDocumentFilter>();
            c.SchemaFilter<LocalizationSchemaFilter>();
            c.ParameterFilter<LocalizationParameterFilter>();
        });

        return services;
    }

    public static IApplicationBuilder UseCultureQuery(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CultureQueryMiddleware>();
    }

    public static void UseLocalizedSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwaggerUI(c =>
        {
            c.IndexStream = () => Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("Swagger.Localization.SwaggerUI.index.html");
        });
    }
}