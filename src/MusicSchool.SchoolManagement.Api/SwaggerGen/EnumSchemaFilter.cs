using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MusicSchool.SchoolManagement.Api.SwaggerGen;

public class EnumSchemaFilter : ISchemaFilter
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
