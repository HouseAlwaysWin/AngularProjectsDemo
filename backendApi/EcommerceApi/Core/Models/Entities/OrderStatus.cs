using System.Runtime.Serialization;

namespace EcommerceApi.Core.Models.Entities
{
    public enum OrderStatus
    {
        [EnumMember(Value="Pending")]
        Pending,
        [EnumMember(Value="PaymentRecived")]
        PaymentReceived,
        [EnumMember(Value="PaymentFailed")]
        PaymentFailed
    }
}