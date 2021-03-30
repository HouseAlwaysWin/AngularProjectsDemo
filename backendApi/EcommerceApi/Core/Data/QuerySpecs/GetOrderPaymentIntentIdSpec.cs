using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.QuerySpecs
{
    public class GetOrderPaymentIntentIdSpec:BaseQuerySpec<Order>
    {
        public GetOrderPaymentIntentIdSpec(string paymentIntentId)
            :base(o => o.PaymentIntentId == paymentIntentId)
        {
            
        }
        
    }
}