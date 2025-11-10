using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;
using Swagger.Localization.Resources;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swagger.Localization.Filters
{
    public class LocalizationParameterFilter(IStringLocalizer<SwaggerDescriptions> localizer) : IParameterFilter
    {
        private readonly IStringLocalizer _localizer = localizer;

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            // Локализация описания параметра
            var paramKey = $"{context.ParameterInfo.Name}_Parameter";
            var localized = _localizer[paramKey];
            if (!string.IsNullOrEmpty(localized))
            {
                parameter.Description = localized;
            }

            // Локализация для параметров в body (схемы)
            if (context.SchemaRepository.Schemas.TryGetValue(context.ParameterInfo.ParameterType.Name, out var schema))
            {
                LocalizeSchema(schema, context.ParameterInfo.ParameterType);
            }
        }

        private void LocalizeSchema(OpenApiSchema schema, Type type)
        {
            // Локализация описания модели
            var modelKey = $"{type.Name}_Description";
            var modelLocalized = _localizer[modelKey];
            if (!string.IsNullOrEmpty(modelLocalized))
            {
                schema.Description = modelLocalized;
            }

            // Локализация свойств модели
            foreach (var property in schema.Properties)
            {
                var propertyKey = $"{type.Name}_{property.Key}_Field";
                var propertyLocalized = _localizer[propertyKey];
                if (!string.IsNullOrEmpty(propertyLocalized))
                {
                    property.Value.Description = propertyLocalized;
                }
            }
        }
    }
}
