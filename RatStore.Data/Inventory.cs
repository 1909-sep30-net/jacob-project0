using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public class Inventory
    {
        public int Id { get; set; }
        public Component Component { get; set; }
        public int Quantity { get; set; }
    }
}
