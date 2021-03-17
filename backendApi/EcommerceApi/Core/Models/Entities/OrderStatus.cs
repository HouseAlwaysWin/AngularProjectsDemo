using System.Runtime.Serialization;

namespace EcommerceApi.Core.Models.Entities
{
    public enum OrderStatus
    {
        [EnumMember(Value="Pending")]
        Pending,
        [EnumMember(Value="Payment Recived")]
        PaymentReceived,
        [EnumMember(Value="Payment Failed")]
        PaymentFailed
    }
}