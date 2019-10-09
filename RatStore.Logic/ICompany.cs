using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;

namespace RatStore.Logic
{
    interface ICompany
    {
        Order TryBuildOrder(Location location, Customer customer, Dictionary<Product, int> products);

        void TrySubmitOrder(Order order);
    }
}
