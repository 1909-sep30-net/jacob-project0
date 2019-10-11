using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class OrderDetails
    {
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }
    }
}
