using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public class Location
    {
        public string Address { get; set; }

        public int Id { get; set; }

        public Dictionary<Component, int> Inventory { get; set; }

        public List<Product> AvailableProducts { get; protected set; }

        public List<Order> OrderHistory { get; set; }
    }
}
