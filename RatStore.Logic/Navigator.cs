using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;
using System.Linq;

namespace RatStore.Logic
{
    public class Navigator
    {
        public RatStore CurrentStore { get; set; }

        public Customer CurrentCustomer { get; set; }

        public List<OrderDetails> Cart { get; set; }

        public Navigator()
        {
            Cart = new List<OrderDetails>();
        }

        public decimal Subtotal
        {
            get
            {
                decimal sum = 0;
                foreach (OrderDetails cartItem in Cart)
                {
                    sum += cartItem.Product.Cost * cartItem.Quantity;
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

            if (!Cart.Exists(item => item.Product.Id == product.Id))
                Cart.Add(new OrderDetails { Product = product, Quantity = quantity });
            else
            {
                OrderDetails cartItem = Cart.Find(item => item.Product.Id == product.Id);
                cartItem.Quantity += quantity;
            }

            if (!CurrentStore.CanFulfillProductQty(product, quantity))
            {
                OrderDetails cartItem = Cart.Find(item => item.Product.Id == product.Id);
                cartItem.Quantity -= quantity;
                if (cartItem.Quantity == 0)
                    Cart.Remove(cartItem);

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
