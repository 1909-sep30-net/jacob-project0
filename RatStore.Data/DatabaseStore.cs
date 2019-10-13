using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RatStore.Data.Entities;
using Microsoft.Extensions.Logging;

namespace RatStore.Data
{
    public class DatabaseStore : IDataStore, IDisposable
    {
        private DbContextOptions<jacobproject0Context> _options;
        private jacobproject0Context _context;

        /*public static readonly ILoggerFactory AppLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(Console);
        }); */

        void Initialize()
        {
            _options = new DbContextOptionsBuilder<jacobproject0Context>()
                .UseSqlServer(SecretCode.Sauce)
                //.UseLoggerFactory(AppLoggerFactory)
                .Options;

            jacobproject0Context Context = new jacobproject0Context(_options);
        }

        void Cleanup()
        {
            
        }

        #region Customer
        void AddCustomer(Customer customer)
        {

        }
        void AddCustomer(string firstName, string middleName, string lastName, string phoneNumber)
        {

        }
        Customer TryGetCustomerByNameAndPhone(string firstName, string lastName, string phoneNumber)
        {

        }
        Customer TryGetCustomerById(int id)
        {

        }
        List<Customer> GetAllCustomers()
        {

        }
        #endregion

        #region Location
        void AddLocation(Location location);
        Location TryGetLocationById(int id);
        List<Location> GetAllLocations();
        int GetNextLocationId();
        #endregion

        #region Product
        void AddRecipe(Product product);
        Product TryGetProductByProductName(string name);
        List<Product> GetAllProducts();
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
