using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
namespace BackendApi.Core.Models.Entities
{
    public class ProductAttributeValue:IBaseEntity
    {
        public int Id { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
       public string Name { get; set; }
       public decimal PriceAdjustment { get; set; }
       public int SeqIndex { get; set; }
       public int ProductAttributeId { get; set; }
       public ProductAttribute ProductAttribute { get; set; }
    }
}