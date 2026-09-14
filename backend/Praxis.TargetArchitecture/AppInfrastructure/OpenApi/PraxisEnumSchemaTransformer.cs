using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Praxis.TargetArchitecture.AppInfrastructure.OpenApi;

public sealed class PraxisEnumSchemaTransformer : IOpenApiSchemaTransformer
{
    private const string XEnumVarnamesKey = "x-enum-varnames";

    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var enumType = Nullable.GetUnderlyingType(context.JsonTypeInfo.Type) ?? context.JsonTypeInfo.Type;

        if (!enumType.IsEnum)
        {
            return Task.CompletedTask;
        }

        var values = Enum.GetValues(enumType).Cast<object>().ToList();

        schema.Enum = values
            .Select(value => (JsonNode)JsonValue.Create(Convert.ToInt64(value)))
            .ToList();

        var varNames = new JsonArray();

        foreach (var value in values)
        {
            varNames.Add(JsonValue.Create(value.ToString()));
        }

        schema.Extensions ??= new Dictionary<string, IOpenApiExtension>();
        schema.Extensions[XEnumVarnamesKey] = new JsonNodeExtension(varNames);

        return Task.CompletedTask;
    }
}
