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

            //DataStore = new TextStore();
            DataStore = new DatabaseStore();
            DataStore.Initialize();

            try
            {
                TryChangeLocation(1);
            }
            catch
            {
                // Add test location if none available
                Address = "123 Test St, Everett, WA 98203";
                Id = 0;
                DataStore.AddLocation(this);
                DataStore.Save();

                TryChangeLocation(1);
            }
        }

        public override List<Product> GetAvailableProducts()
        {
            List<Product> availableProducts = new List<Product>();
            List<Product> allProducts = DataStore.GetAllProducts();
            for (int i = 0; i < allProducts.Count; ++i)
            {
                OrderDetails tempDetail = new OrderDetails
                {
                    Product = allProducts[i],
                    Quantity = 1
                };

                if (CanFulfillProductQty(tempDetail))
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
                || customer.Id != -1)
                return false;

            return true;
        }
        protected override bool ValidateProductRequest(List<OrderDetails> orderDetails)
        {
            foreach (OrderDetails detail in orderDetails)
            {
                if (detail.Quantity > 100
                    || detail.Quantity < 1)
                    return false;
            }

            return true;
        }
        protected override bool ValidateOrder(Order order)
        {
            if (order.CustomerId == -1
                || order.Id != -1
                || order.OrderDetails.Count == 0
                || order.LocationId == 0)
                return false;

            return true;
        }
        #endregion
    }
}
