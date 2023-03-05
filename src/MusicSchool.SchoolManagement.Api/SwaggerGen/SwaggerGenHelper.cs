using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MusicSchool.SchoolManagement.Domain.ValueObjects;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MusicSchool.SchoolManagement.Api.SwaggerGen;

public static class SwaggerGenHelper
{
    public static void Setup(SwaggerGenOptions options)
    {
        options.SchemaFilter<EnumSchemaFilter>();

        options.MapType<BrlAmount>(() => new OpenApiSchema
        {
            Type = "number",
        });

        options.MapType<DateMonthOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Example = new OpenApiString(DateMonthOnly.Current.ToString()),
        });

        options.UseOneOfForPolymorphism();
    }
}
