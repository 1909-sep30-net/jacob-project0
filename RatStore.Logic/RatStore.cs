using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;

namespace RatStore.Logic
{
    class RatStore : Location
    {
        static TextStore _textStore = new TextStore();

        public RatStore()
        {
            //TODO: text mode vs sql mode
        }

        #region Inventory Management
        void UpdateAvailableProducts()
        {
            List<Product> availableProducts = new List<Product>();
            foreach(Recipe recipe in _textStore.GetAllRecipes())
            {
                if (CanFulfillRecipeQty(recipe, 1))
                    availableProducts.Add(new Product(recipe));
            }

            AvailableProducts = availableProducts;
        }
        bool CanFulfillRecipeQty(Recipe recipe, int quantity)
        {
            foreach (Component comp in recipe.Ingredients.Keys)
            {
                if (recipe.Ingredients[comp] * quantity > Inventory[comp])
                    return false;
            }

            return true;
        }
        void TryFulfillRecipeQty(Recipe recipe, int quantity)
        {
            foreach (Component comp in recipe.Ingredients.Keys)
            {
                if (recipe.Ingredients[comp] * quantity <= Inventory[comp])
                {
                    Inventory[comp] -= recipe.Ingredients[comp] * quantity;
                }
                else throw new Exception($"Not enough ingredients for recipe: {recipe.EndProductName}");
            }
        }
        #endregion

        #region Order Manipulation
        public Order TryBuildOrder(Location location, Customer customer, Dictionary<Product, int> products)
        {
            if (!ValidateLocation(location))
                throw new Exception("Order build failed: invalid location");
            else if (!ValidateCustomer(customer))
                throw new Exception("Order build failed: invalid customer");
            else if (!ValidateProductRequest(products))
                throw new Exception("Order build failed: invalid products dictionary");
            else
            {
                Order o = new Order()
                {
                    CustomerId = customer.Id,
                    OriginStoreId = location.Id,
                    OrderProducts = products,
                    OrderId = _textStore.GetNextOrderId()
                };

                return o;
            }
        }
        public void TrySubmitOrder(Order order)
        {
            if (!ValidateOrder(order))
                throw new Exception("Invalid order");

            _textStore.AddOrder(order);
        }
        #endregion

        #region Validation
        bool ValidateLocation(Location location)
        {
            if (location.Id == 0
                || location.Address == "")
                return false;

            return true;
        }
        bool ValidateCustomer(Customer customer)
        {
            if (customer.FirstName == ""
                || customer.LastName == ""
                || customer.Id == 0)
                return false;

            return true;
        }
        bool ValidateProductRequest(Dictionary<Product, int> products)
        {
            foreach (Product p in products.Keys)
            {
                if (products[p] > 100
                    || products[p] < 1)
                    return false;
            }

            return true;
        }
        bool ValidateOrder(Order order)
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
