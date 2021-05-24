using System;
using System.Collections.Generic;

namespace BackendApi.Core.Models.Entities
{
    public class ProductBrand:IBaseEntity
    {
        public int Id { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
        public string Name { get; set; }
    }
}