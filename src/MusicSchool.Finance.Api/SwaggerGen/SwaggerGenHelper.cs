using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MusicSchool.Finance.Domain.ValueObjects;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MusicSchool.Finance.Api.SwaggerGen;

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
    }
}
