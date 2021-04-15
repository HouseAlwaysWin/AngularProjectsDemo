// using EcommerceApi.Core.Models.Entities;

// namespace EcommerceApi.Core.Data.QuerySpecs
// {
//     public class GetOrderWithItemsSpec:BaseQuerySpec<Order>
//     {
//        public GetOrderWithItemsSpec(string email):base(o => o.BuyerEmail == email)
//        {
//            AddInclude(o => o.OrderItems);
//            AddInclude(o => o.DeliveryMethod);
//            AddOrderByDescending(o => o.OrderDate);
//        } 

//        public GetOrderWithItemsSpec(int id,string email):base(o => o.BuyerEmail == email && o.Id == id)
//        {
//            AddInclude(o => o.OrderItems);
//            AddInclude(o => o.DeliveryMethod);
//        } 
//     }
// }