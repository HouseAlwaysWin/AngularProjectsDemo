using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApi.Core.Models.Entities
{
    public class DeliveryMethod
    {
        public int Id  { get; set; }
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string LangCode { get; set; }
    }
}