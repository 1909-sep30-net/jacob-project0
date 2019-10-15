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
        #region Properties
        private DbContextOptions<jacobproject0Context> _options;
        private jacobproject0Context _context;

        /*public static readonly ILoggerFactory AppLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(Console);
        }); */
        #endregion

        #region Startup and Shutdown
        public DatabaseStore()
        {
            _options = new DbContextOptionsBuilder<jacobproject0Context>()
                .UseSqlServer(SecretCode.Sauce)
                .EnableSensitiveDataLogging()
                //.UseLoggerFactory(AppLoggerFactory)
                .Options;

            _context = new jacobproject0Context(_options);
        }

        ~DatabaseStore()
        {
            _context.Dispose();
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
        }
        #endregion

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
        public Customer GetCustomerByNameAndPhone(string firstName, string lastName, string phoneNumber)
            => _context.Customer.Select(Mapper.MapCustomer).Where(c => c.FirstName == firstName && c.LastName == lastName && c.PhoneNumber == phoneNumber).FirstOrDefault();
        public Customer GetCustomerById(int id)
            => Mapper.MapCustomer(_context.Customer.Find(id));
        public List<Customer> GetAllCustomers()
        {
            IQueryable<Entities.Customer> customers = _context.Customer
                .AsNoTracking();

            return customers.Select(Mapper.MapCustomer).ToList();
        }
        public void UpdateCustomer(Customer customer)
        {
            Entities.Customer currentCustomer = _context.Customer.Find(customer.CustomerId);
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
        public Location GetLocationById(int id)
            => Mapper.MapLocation(_context.Location
                .Include(l => l.Inventory)
                .ThenInclude(i => i.Component)
                .Include(l => l.Order)
                .First(l => l.LocationId == id));
        public List<Location> GetAllLocations()
        {
            IQueryable<Entities.Location> locations = _context.Location
                .Include(l => l.Inventory)
                .ThenInclude(i => i.Component)
                .Include(l => l.Order)
                .AsNoTracking();

            return locations.Select(Mapper.MapLocation).ToList();
        }
        public void UpdateLocation(Location location)
        {
            Entities.Location currentLocation = _context.Location
                .Include(l => l.Inventory).FirstOrDefault(l => l.LocationId == location.LocationId);

            foreach (Entities.Inventory item in currentLocation.Inventory)
            {
                item.Quantity = location.Inventory.Find(i => i.Component.ComponentId == item.ComponentId).Quantity;
            }
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
        public Product GetProductByName(string name)
            => Mapper.MapProduct(_context.Product
                .Include(p => p.ProductComponent)
                .ThenInclude(pc => pc.Component)
                .First(p => p.Name == name));
        public List<Product> GetAllProducts()
        {
            IQueryable<Entities.Product> products = _context.Product
                .Include(p => p.ProductComponent)
                .ThenInclude(pc => pc.Component)
                .AsNoTracking();

            return products.Select(Mapper.MapProduct).ToList();
        }
        public void UpdateProduct(Product product)
        {
            Entities.Product currentProduct = _context.Product.Find(product.ProductId);
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
        public Component GetComponentName(string name)
            => _context.Component.Select(Mapper.MapComponent).Where(c => c.Name == name).FirstOrDefault();
        public Component GetComponentById(int id)
            => Mapper.MapComponent(_context.Component.AsNoTracking().FirstOrDefault(c => c.ComponentId == id));
        public List<Component> GetAllComponents()
        {
            IQueryable<Entities.Component> component = _context.Component
                .AsNoTracking();

            return component.Select(Mapper.MapComponent).ToList();
        }
        public void UpdateComponent(Component component)
        {
            Entities.Component currentComponent = _context.Component.AsNoTracking().FirstOrDefault(c => c.ComponentId == component.ComponentId);
            Entities.Component newComponent = Mapper.MapComponent(component);

            _context.Entry(currentComponent).CurrentValues.SetValues(newComponent);
        }
        public void RemoveComponent(int id)
        {
            Entities.Component component = _context.Component.FirstOrDefault(c => c.ComponentId == id);
            _context.Component.Remove(component);
        }
        #endregion

        #region Order
        public void AddOrder(Order order)
            => _context.Order.Add(Mapper.MapOrder(order));
        public Order GetOrderById(int id)
            => Mapper.MapOrder(_context.Order
                .Include(o => o.Customer)
                .Include(o => o.Location)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductComponent)
                .ThenInclude(pc => pc.Component)
                .First(o => o.OrderId == id));
        public List<Order> GetOrderHistory(int customerId = 0)
        {
            IQueryable<Entities.Order> orders = _context.Order
                    .AsNoTracking();

            try
            {
                if (customerId == 0)
                {
                    return orders.Include(o => o.Location)
                        .ThenInclude(l => l.Inventory)
                        .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.ProductComponent)
                        .ThenInclude(pc => pc.Component)
                        .Include(o => o.Customer)
                        .Select(Mapper.MapOrder)
                        .ToList();
                }
                else
                {
                    return orders.Include(o => o.Location)
                        .ThenInclude(l => l.Inventory)
                        .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.ProductComponent)
                        .ThenInclude(pc => pc.Component)
                        .Include(o => o.Customer)
                        .Select(Mapper.MapOrder)
                        .Where(o => o.CustomerId == customerId)
                        .ToList();
                }
            }
            catch (Exception e)
            {

            }

            return new List<Order>();
        }

        public void UpdateOrder(Order order)
        {
            Entities.Order currentOrder = _context.Order.Find(order.OrderId);
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
