using System.Collections.Generic;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Data.QuerySpecs
{
    public class GetOrderByEmailListSpec:BaseQuerySpec<Order>
    {
        public GetOrderByEmailListSpec(GetOrderParam param)
            :base(o => o.BuyerEmail == param.Email)
        {
           AddInclude(o => o.OrderItems);
           AddInclude(o => o.OrderAddress);
           AddInclude(o => o.DeliveryMethod);
           ApplyPaging(param.PageSize * (param.PageIndex -1),param.PageSize);

           switch(param.Sort){
                    case "dateAsc":
                        AddOrderBy(p => p.CreatedDate);
                        break;
                    case "dateDesc":
                        AddOrderByDescending(p => p.CreatedDate);
                        break;
                    case "priceAsc":
                        AddOrderBy(p => p.TotalPrice);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.TotalPrice);
                        break;
                    default:
                        AddOrderByDescending(n => n.CreatedDate);
                        break;
                }
        } 
    }
}