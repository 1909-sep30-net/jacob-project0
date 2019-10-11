using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class Components
    {
        public Components()
        {
            LocationInventories = new HashSet<LocationInventories>();
            ProductComponents = new HashSet<ProductComponents>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Cost { get; set; }

        public virtual ICollection<LocationInventories> LocationInventories { get; set; }
        public virtual ICollection<ProductComponents> ProductComponents { get; set; }
    }
}
