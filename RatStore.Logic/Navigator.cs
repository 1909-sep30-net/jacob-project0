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

        /// <summary>
        /// Changes the CurrentStore to the Location with the given Id.
        /// </summary>
        /// <param name="targetStoreId"></param>
        public void GoToStore(int targetStoreId)
        {
            CurrentStore.ChangeLocation(targetStoreId);
        }

        /// <summary>
        /// Verifies that the CurrentStore can fulfill the product request and then adds it to the Cart.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        public void AddProductToCart(int productId, int quantity)
        {
            List<Product> availableProducts = CurrentStore.GetAvailableProducts();
            if (productId > availableProducts.Count || productId < 0)
                throw new Exception("Invalid product id");

            Product product = availableProducts[productId];
            OrderDetails cartItem;

            if (!Cart.Exists(item => item.Product.ProductId == product.ProductId))
            {
                cartItem = new OrderDetails 
                { 
                    Product = product, 
                    Quantity = quantity 
                };

                Cart.Add(cartItem);
            }
            else
            {
                cartItem = Cart.Find(item => item.Product.ProductId == product.ProductId);
                cartItem.Quantity += quantity;
            }

            if (!CurrentStore.CanFulfillProductQty(cartItem))
            {
                cartItem.Quantity -= quantity;
                if (cartItem.Quantity == 0)
                    Cart.Remove(cartItem);

                throw new Exception($"Inventory cannot fulfill quantity: {product.Name} x {quantity}");
            }
        }

        /// <summary>
        /// Removes all items from the Cart.
        /// </summary>
        public void ClearCart()
        {
            Cart.Clear();
        }

        /// <summary>
        /// Passes the Cart - a list of OrderDetails - to the CurrentStore for submission.
        /// </summary>
        public void SubmitCart()
        {
            CurrentStore.SubmitOrder(CurrentCustomer, Cart);
            ClearCart();
        }
    }
}
