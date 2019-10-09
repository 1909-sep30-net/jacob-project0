using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public class Location
    {
        public IDataStore DataStore { get; protected set; }

        public string Address { get; set; }

        public int Id { get; set; }

        public Dictionary<Component, int> Inventory { get; set; }

        public List<Product> AvailableProducts { get; protected set; }

        public List<Order> OrderHistory { get; set; }

        public void TryChangeLocation(int targetStoreId)
        {
            Location temp = DataStore.TryGetLocationById(targetStoreId);

            Address = temp.Address;
            Id = temp.Id;
            Inventory = temp.Inventory;
            AvailableProducts = temp.AvailableProducts;
            OrderHistory = temp.OrderHistory;
        }

        #region Inventory Management
        virtual public void UpdateAvailableProducts()
        {
            throw new Exception("No data for Location class!");
        }
        virtual public bool CanFulfillRecipeQty(Recipe recipe, int quantity)
        {
            foreach (Component comp in recipe.Ingredients.Keys)
            {
                if (recipe.Ingredients[comp] * quantity > Inventory[comp])
                    return false;
            }

            return true;
        }
        virtual public void TryFulfillRecipeQty(Recipe recipe, int quantity)
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
                    OrderId = DataStore.GetNextOrderId()
                };

                return o;
            }
        }
        public void TrySubmitOrder(Order order)
        {
            if (!ValidateOrder(order))
                throw new Exception("Invalid order");

            DataStore.AddOrder(order);
        }
        #endregion

        #region Validation
        virtual protected bool ValidateLocation(Location location)
        {
            return true;
        }
        virtual protected bool ValidateCustomer(Customer customer)
        {
            return true;
        }
        virtual protected bool ValidateProductRequest(Dictionary<Product, int> products)
        {
            return true;
        }
        virtual protected bool ValidateOrder(Order order)
        {
            return true;
        }
        #endregion
    }
}
