using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RatStore.Data.Entities;
using Microsoft.Extensions.Logging;

namespace RatStore.Data
{
    public class DatabaseStore : IDataStore
    {
        private DbContextOptions<jacobproject0Context> _options;
        private jacobproject0Context _context;

        /*public static readonly ILoggerFactory AppLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(Console);
        }); */

        public DatabaseStore()
        {
            _options = new DbContextOptionsBuilder<jacobproject0Context>()
                .UseSqlServer(SecretCode.Sauce)
                //.UseLoggerFactory(AppLoggerFactory)
                .Options;

            jacobproject0Context _context = new jacobproject0Context(_options);
        }

        public void Initialize()
        {
            
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Cleanup()
        {
            _context.SaveChanges();
            _context.Dispose();
        }

        #region Customer
        public void AddCustomer(Customer customer)
        {
            Entities.Customer newCustomer = Mapper.MapCustomer(customer);
            _context.Customer.Add(newCustomer);
        }
        public void AddCustomer(string firstName, string middleName, string lastName, string phoneNumber)
        {
            Customer customer = new Customer
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                PhoneNumber = phoneNumber
            };

            AddCustomer(customer);
        }
        public Customer TryGetCustomerByNameAndPhone(string firstName, string lastName, string phoneNumber)
            => _context.Customer.Select(Mapper.MapCustomer).Where(c => c.FirstName == firstName && c.LastName == lastName && c.PhoneNumber == phoneNumber).FirstOrDefault();
        public Customer TryGetCustomerById(int id)
            => Mapper.MapCustomer(_context.Customer.Find(id));
        public List<Customer> GetAllCustomers()
        {
            IQueryable<Entities.Customer> customers = _context.Customer
                .AsNoTracking();

            return customers.Select(Mapper.MapCustomer).ToList();
        }
        public void UpdateCustomer(Customer customer)
        {
            Entities.Customer currentCustomer = _context.Customer.Find(customer.Id);
            Entities.Customer newCustomer = Mapper.MapCustomer(customer);

            _context.Entry(currentCustomer).CurrentValues.SetValues(newCustomer);
        }
        public void RemoveCustomer(int id)
        {
            Entities.Customer customer = _context.Customer.Find(id);
            _context.Customer.Remove(customer);
        }
        #endregion

        #region Location
        public void AddLocation(Location location)
        {
            Entities.Location newLocation = Mapper.MapLocation(location);
            _context.Location.Add(newLocation);
        }
        public Location TryGetLocationById(int id)
            => Mapper.MapLocation(_context.Location.Find(id));
        public List<Location> GetAllLocations()
        {
            IQueryable<Entities.Location> locations = _context.Location
                .AsNoTracking();

            return locations.Select(Mapper.MapLocation).ToList();
        }
        public void UpdateLocation(Location location)
        {
            Entities.Location currentLocation = _context.Location.Find(location.Id);
            Entities.Location newLocation = Mapper.MapLocation(location);

            _context.Entry(currentLocation).CurrentValues.SetValues(newLocation);
        }
        public void RemoveLocation(int id)
        {
            Entities.Location location = _context.Location.Find(id);
            _context.Location.Remove(location);
        }
        #endregion

        #region Product
        public void AddProduct(Product product)
        {
            Entities.Product newProduct = Mapper.MapProduct(product);
            _context.Product.Add(newProduct);
        }
        public Product TryGetProductByProductName(string name)
            => _context.Product.Select(Mapper.MapProduct).Where(p => p.Name == name).FirstOrDefault();
        public List<Product> GetAllProducts()
        {
            IQueryable<Entities.Product> customers = _context.Product
                .AsNoTracking();

            return customers.Select(Mapper.MapProduct).ToList();
        }
        public void UpdateProduct(Product product)
        {
            Entities.Product currentProduct = _context.Product.Find(product.Id);
            Entities.Product newProduct = Mapper.MapProduct(product);

            _context.Entry(currentProduct).CurrentValues.SetValues(newProduct);
        }
        public void RemoveProduct(int id)
        {
            Entities.Product product = _context.Product.Find(id);
            _context.Product.Remove(product);
        }
        #endregion

        #region Component
        public void AddComponent(Component component)
        {
            Entities.Component newComponent = Mapper.MapComponent(component);
            _context.Component.Add(newComponent);
        }
        public Component TryGetComponentByName(string name)
            => _context.Component.Select(Mapper.MapComponent).Where(c => c.Name == name).FirstOrDefault();
        public Component GetComponentById(int id)
            => Mapper.MapComponent(_context.Component.Find(id));
        public List<Component> GetAllComponents()
        {
            IQueryable<Entities.Component> component = _context.Component
                .AsNoTracking();

            return component.Select(Mapper.MapComponent).ToList();
        }
        public void UpdateComponent(Component component)
        {
            Entities.Component currentComponent = _context.Component.Find(component.Id);
            Entities.Component newComponent = Mapper.MapComponent(component);

            _context.Entry(currentComponent).CurrentValues.SetValues(newComponent);
        }
        public void RemoveComponent(int id)
        {
            Entities.Component component = _context.Component.Find(id);
            _context.Component.Remove(component);
        }
        #endregion

        #region Order
        public void AddOrder(Order order)
        {
            Entities.Order newOrder = Mapper.MapOrder(order);
            _context.Order.Add(newOrder);
        }
        public Order TryGetOrderById(int id)
            => Mapper.MapOrder(_context.Order.Find(id));
        public List<Order> GetOrderHistory(int customerId = -1)
        {
            IQueryable<Entities.Order> orders = _context.Order
                    .AsNoTracking();

            if (customerId == -1)
            {
                return orders.Select(Mapper.MapOrder).ToList();
            }
            else
            {
                return orders.Select(Mapper.MapOrder).Where(o => o.CustomerId == customerId).ToList();
            }
        }
        public void UpdateOrder(Order order)
        {
            Entities.Order currentOrder = _context.Order.Find(order.Id);
            Entities.Order newOrder = Mapper.MapOrder(order);

            _context.Entry(currentOrder).CurrentValues.SetValues(newOrder);
        }
        public void RemoveOrder(int id)
        {
            Entities.Order customer = _context.Order.Find(id);
            _context.Order.Remove(customer);
        }
        #endregion
    }
}
