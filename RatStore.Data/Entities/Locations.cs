using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class Locations
    {
        public Locations()
        {
            LocationInventories = new HashSet<LocationInventories>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string Address { get; set; }

        public virtual ICollection<LocationInventories> LocationInventories { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
