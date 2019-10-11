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

        public double Subtotal
        {
            get
            {
                double sum = 0;
                foreach (Product product in Cart.Keys)
                {
                    sum += product.Cost * Cart[product];
                }

                return sum;
            }
        }

        public void TryGoToStore(int targetStoreId)
        {
            CurrentStore.TryChangeLocation(targetStoreId);
        }

        public void AddProductToCart(int productId, int quantity)
        {
            List<Product> availableProducts = CurrentStore.GetAvailableProducts();
            if (productId > availableProducts.Count || productId < 0)
                throw new Exception("Invalid product id");

            Product product = availableProducts[productId];

            if (!Cart.ContainsKey(product) || Cart[product] == 0)
                Cart.Add(product, quantity);
            else Cart[product] += quantity;

            if (!CurrentStore.CanFulfillProductQty(product, quantity))
            {
                Cart[product] -= quantity;
                throw new Exception($"Inventory cannot fulfill quantity: {product.Name} x {quantity}");
            }
        }

        public void ClearCart()
        {
            Cart.Clear();
        }

        public void SubmitCart()
        {
            Order order = CurrentStore.TryBuildOrder(CurrentCustomer, Cart);
            CurrentStore.TrySubmitOrder(order);
            ClearCart();
        }
    }
}
