using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public interface IDataStore
    {
        void Initialize();

        void Save();
        void Cleanup();

        #region Customer
        void AddCustomer(Customer customer);
        void AddCustomer(string firstName, string middleName, string lastName, string phoneNumber);
        Customer TryGetCustomerByNameAndPhone(string firstName, string lastName, string phoneNumber);
        Customer TryGetCustomerById(int id);
        List<Customer> GetAllCustomers();
        void UpdateCustomer(Customer customer);
        void RemoveCustomer(int id);
        #endregion

        #region Location
        void AddLocation(Location location);
        Location TryGetLocationById(int id);
        List<Location> GetAllLocations();
        void UpdateLocation(Location location);
        void RemoveLocation(int id);
        #endregion

        #region Product
        void AddProduct(Product product);
        Product TryGetProductByProductName(string name);
        List<Product> GetAllProducts();
        void UpdateProduct(Product product);
        void RemoveProduct(int id);
        #endregion

        #region Component
        void AddComponent(Component component);
        Component TryGetComponentByName(string name);
        Component GetComponentById(int id);
        List<Component> GetAllComponents();
        void UpdateComponent(Component component);
        void RemoveComponent(int id);
        #endregion

        #region Order
        void AddOrder(Order order);
        Order TryGetOrderById(int id);
        List<Order> GetOrderHistory(int customerId = -1);
        void UpdateOrder(Order order);
        void RemoveOrder(int id);
        #endregion
    }
}
