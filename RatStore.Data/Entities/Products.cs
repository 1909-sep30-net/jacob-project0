using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class Products
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
            ProductComponents = new HashSet<ProductComponents>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual ICollection<ProductComponents> ProductComponents { get; set; }
    }
}
