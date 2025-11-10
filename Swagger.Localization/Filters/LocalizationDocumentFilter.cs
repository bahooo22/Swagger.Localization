using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;
using Swagger.Localization.Resources;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swagger.Localization.Filters;

public class LocalizationDocumentFilter(IStringLocalizer<SwaggerDescriptions> localizer) : IDocumentFilter
{
    private readonly IStringLocalizer _localizer = localizer;

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var path in swaggerDoc.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                // Локализация описания операции
                var descKey = $"{operation.Value.OperationId}_Description";
                operation.Value.Description = _localizer[descKey] ?? operation.Value.Description;

                // Локализация summary (дополнительно)
                var summaryKey = $"{operation.Value.OperationId}_Summary";
                operation.Value.Summary = _localizer[summaryKey] ?? operation.Value.Summary;

                // Локализация тегов
                var tagKey = $"{operation.Value.OperationId}_Tag";
                var localizedTag = _localizer[tagKey];
                if (!string.IsNullOrEmpty(localizedTag))
                {
                    operation.Value.Tags = new List<OpenApiTag>
                    {
                        new OpenApiTag { Name = localizedTag }
                    };
                }
            }
        }
    }
}