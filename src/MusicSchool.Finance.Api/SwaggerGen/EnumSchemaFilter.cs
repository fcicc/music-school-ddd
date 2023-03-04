using System.Globalization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MusicSchool.Finance.Api.SwaggerGen;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear();

            IEnumerable<string> names = Enum.GetNames(context.Type).Select(ToCamelCase);
            foreach (string name in names)
            {
                schema.Enum.Add(new OpenApiString(name));
            }

            schema.Type = "string";
            schema.Format = "";
        }
    }

    private string ToCamelCase(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return string.Concat(
            char.ToLowerInvariant(value[0]).ToString(CultureInfo.InvariantCulture),
            value.Substring(1)
        );
    }
}
