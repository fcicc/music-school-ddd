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
    }

    private class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();

                IEnumerable<string> names = Enum.GetNames(context.Type);
                foreach (string name in names)
                {
                    schema.Enum.Add(new OpenApiString(name));
                }

                schema.Type = "string";
                schema.Format = "";
            }
        }
    }
}
