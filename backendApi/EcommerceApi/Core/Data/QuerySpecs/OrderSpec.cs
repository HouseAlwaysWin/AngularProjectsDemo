using System.Linq;
using EcommerceApi.Core.Models.Dtos;
using EcommerceApi.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Core.Data.QuerySpecs
{
    public static class OrderSpec
    {
       public static IQueryable<Order> GetOrderByEmailListSpec(IQueryable<Order> query,GetOrderParam param){
           query = query.Where(o => o.BuyerEmail == param.Email)
                .Include(o => o.OrderItems)
                .Include(o => o.OrderAddress)
                .Include(o => o.DeliveryMethod);
            
            switch(param.Sort){
                    case "dateAsc":
                        query =query.OrderBy(p => p.CreatedDate);
                        break;
                    case "dateDesc":
                        query = query.OrderByDescending(p => p.CreatedDate);
                        break;
                    case "priceAsc":
                        query = query.OrderBy(p => p.TotalPrice);
                        break;
                    case "priceDesc":
                        query = query.OrderByDescending(p => p.TotalPrice);
                        break;
                    default:
                        query = query.OrderByDescending(n => n.CreatedDate);
                        break;
            }
            return query;
       }

       public static IQueryable<Order> GetOrderWithItemsSpec(IQueryable<Order> query,string email){
            return query.Where(o => o.BuyerEmail == email).Include(o => o.OrderItems)
                         .Include(o => o.DeliveryMethod).OrderByDescending(o => o.OrderDate);
       }

       public static IQueryable<Order> GetOrderWithItemsSpec(IQueryable<Order> query,int id,string email){
            return query.Where(o=> o.BuyerEmail == email && o.Id == id).Include(o => o.OrderItems)
                         .Include(o => o.DeliveryMethod).OrderByDescending(o => o.OrderDate);
       }

       
    }
}