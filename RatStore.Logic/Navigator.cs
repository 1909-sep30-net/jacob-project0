using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;

namespace RatStore.Logic
{
    public class Navigator
    {
        public RatStore CurrentStore { get; set; }

        public Customer CurrentCustomer { get; set; }

        public Dictionary<Product, int> Cart { get; set; }

        public void TryGoToStore(int targetStoreId)
        {
            CurrentStore.TryChangeLocation(targetStoreId);
        }
    }
}
