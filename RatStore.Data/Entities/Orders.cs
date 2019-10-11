using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class Orders
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? LocationId { get; set; }
        public int? CustomerId { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Locations Location { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
