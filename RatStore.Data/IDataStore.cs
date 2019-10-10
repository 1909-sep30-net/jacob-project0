using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public interface IDataStore
    {
        void Initialize();

        void Cleanup();

        #region Customer
        void AddCustomer(Customer customer);
        void AddCustomer(string firstName, string middleName, string lastName, string phoneNumber);
        Customer TryGetCustomerByNameAndPhone(string firstName, string lastName, string phoneNumber);
        Customer TryGetCustomerById(int id);
        List<Customer> GetAllCustomers();
        int GetNextCustomerId();
        #endregion

        #region Location
        void AddLocation(Location location);
        Location TryGetLocationById(int id);
        List<Location> GetAllLocations();
        int GetNextLocationId();
        #endregion

        #region Recipe
        void AddRecipe(Recipe recipe);
        Recipe TryGetRecipeByProductName(string name);
        List<Recipe> GetAllRecipes();
        #endregion

        #region Component
        void AddComponent(Component component);
        Component TryGetComponentByName(string name);
        List<Component> GetAllComponents();
        #endregion

        #region Order
        void AddOrder(Order order);
        Order TryGetOrderById(int id);
        List<Order> GetOrderHistory();
        int GetNextOrderId();
        #endregion
    }
}
