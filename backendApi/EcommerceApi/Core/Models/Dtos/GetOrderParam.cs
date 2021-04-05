using System.Runtime.Serialization;
namespace EcommerceApi.Core.Models.Dtos
{
    public class GetOrderParam:BasePaging
    {
        public string Sort  { get; set; }
        
        public string Email { get; set; }
    }
}