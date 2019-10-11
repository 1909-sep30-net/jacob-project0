using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;

namespace RatStore.Logic
{
    public class RatStore : Location
    {
        public RatStore()
        {
            //TODO: text mode vs sql mode

            DataStore = new TextStore();
            DataStore.Initialize();

            try
            {
                TryChangeLocation(0);
            }
            catch
            {
                // Add test location if none available
                Address = "123 Test St, Everett, WA 98203";
                Id = 0;
                DataStore.AddLocation(this);
                TryChangeLocation(0);
            }
        }

        public override List<Product> GetAvailableProducts()
        {
            List<Product> availableProducts = new List<Product>();
            List<Product> allProducts = DataStore.GetAllProducts();
            for (int i = 0; i < allProducts.Count; ++i)
            {
                if (CanFulfillProductQty(allProducts[i], 1))
                {
                    availableProducts.Add(allProducts[i]);
                }
            }

            return availableProducts;
        }

        #region Validation
        protected override bool ValidateLocation(Location location)
        {
            if (location.Id == 0
                || location.Address == "")
                return false;

            return true;
        }
        protected override bool ValidateCustomer(Customer customer)
        {
            if (customer.FirstName == ""
                || customer.LastName == ""
                || customer.Id == 0)
                return false;

            return true;
        }
        protected override bool ValidateProductRequest(Dictionary<Product, int> products)
        {
            foreach (Product p in products.Keys)
            {
                if (products[p] > 100
                    || products[p] < 1)
                    return false;
            }

            return true;
        }
        protected override bool ValidateOrder(Order order)
        {
            if (order.CustomerId == 0
                || order.OrderId == 0
                || order.OrderProducts.Count == 0
                || order.OriginStoreId == 0)
                return false;

            return true;
        }
        #endregion
    }
}
