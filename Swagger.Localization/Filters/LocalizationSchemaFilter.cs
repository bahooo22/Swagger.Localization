using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swagger.Localization.Filters
{
    public class LocalizationSchemaFilter(IStringLocalizerFactory localizerFactory) : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var localizer = localizerFactory.Create("SwaggerModels", context.Type.Name);

            // Локализация описания модели
            schema.Description = localizer["Description"]?.Value ?? schema.Description;

            // Локализация свойств модели
            foreach (var property in schema.Properties)
            {
                var propertyKey = $"{property.Key}";
                property.Value.Description = localizer[propertyKey]?.Value ?? property.Value.Description;
            }
        }
    }
}
