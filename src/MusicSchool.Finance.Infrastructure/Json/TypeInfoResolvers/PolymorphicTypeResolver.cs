using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Infrastructure.Json.TypeInfoResolvers;

public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Type == typeof(Transaction))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new(typeof(ExtraPayment), "ExtraPayment"),
                    new(typeof(InvoicePayment), "InvoicePayment"),
                },
            };
        }
        else if (jsonTypeInfo.Type == typeof(ITransactionService.CreateTransactionRequest))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new(typeof(ITransactionService.CreateExtraPaymentRequest), "ExtraPayment"),
                    new(typeof(ITransactionService.CreateInvoicePaymentRequest), "InvoicePayment"),
                },
            };
        }

        return jsonTypeInfo;
    }
}
