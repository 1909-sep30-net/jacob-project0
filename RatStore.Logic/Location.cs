using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Logic
{
    class Location
    {
        public string Address { get; set; }

        public Dictionary<Component, int> Inventory { get; set; }

        public List<Product> AvailableProducts { get; }

        public int Id { get; set; }
    }
}
