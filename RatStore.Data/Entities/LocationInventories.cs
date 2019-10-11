﻿using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class LocationInventories
    {
        public int? LocationId { get; set; }
        public int? ComponentId { get; set; }
        public int? Quantity { get; set; }

        public virtual Components Component { get; set; }
        public virtual Locations Location { get; set; }
    }
}