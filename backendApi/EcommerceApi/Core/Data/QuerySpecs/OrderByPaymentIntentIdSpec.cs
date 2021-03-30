using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.QuerySpecs
{
    public class OrderByPaymentIntentIdSpec:BaseQuerySpec<Order>
    {
       public OrderByPaymentIntentIdSpec(string paymentIntentId)
        :base(o => o.PaymentIntentId == paymentIntentId)
       {
           
       } 
    }
}