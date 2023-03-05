using System.Reflection;
using System.Text.Json.Serialization;
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

        options.UseOneOfForPolymorphism();

        options.SelectDiscriminatorNameUsing(t =>
            t.GetCustomAttribute<JsonPolymorphicAttribute>()?
                .TypeDiscriminatorPropertyName
        );

        options.SelectDiscriminatorValueUsing(t =>
            t.GetCustomAttributes<JsonDerivedTypeAttribute>(true)
                .FirstOrDefault(a => a.DerivedType == t)?
                .TypeDiscriminator as string
        );

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
