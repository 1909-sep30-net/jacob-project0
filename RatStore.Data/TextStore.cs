using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace RatStore.Data
{
    public class TextStore //: IDataStore
    {
        #region Properties
        string _path, _customersFile, _locationsFile, _productsFile, _componentsFile, _ordersFile;
        public List<Customer> Customers { get; private set; }
        public List<Location> Locations { get; private set; }
        public List<Product> Products { get; private set; }
        public List<Component> Components { get; private set; }
        public List<Order> Orders { get; private set; }
        #endregion

        #region Startup and Shutdown
        public void Initialize()
        {
            _path = "C:\\Users\\Jacob Davis\\Revature\\jacob-project0\\";

            _customersFile = _path + "Customers.json";
            _locationsFile = _path + "Locations.json";
            _productsFile = _path + "Recipes.json";
            _componentsFile = _path + "Components.json";
            _ordersFile = _path + "Orders.json";

            Customers = new List<Customer>();
            Locations = new List<Location>();
            Products = new List<Product>();
            Components = new List<Component>();
            Orders = new List<Order>();

            LoadStores();
        }

        public void Save()
        {
            // Do nothing
        }

        public void Cleanup()
        {
            SaveStores();
        }
        #endregion

        #region Storage
        public void LoadStores()
        {
            try
            {
                if (File.Exists(_customersFile))
                    Customers = JsonConvert.DeserializeObject< List<Customer>>(File.ReadAllText(_customersFile));

                if (File.Exists(_locationsFile))
                    Locations = JsonConvert.DeserializeObject<List<Location>>(File.ReadAllText(_locationsFile));

                if (File.Exists(_productsFile))
                    Products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(_productsFile));

                if (File.Exists(_componentsFile))
                    Components = JsonConvert.DeserializeObject<List<Component>>(File.ReadAllText(_componentsFile));

                if (File.Exists(_ordersFile))
                    Orders = JsonConvert.DeserializeObject<List<Order>>(File.ReadAllText(_ordersFile));
            }
            catch (Exception e)
            {
                // TODO: logging
            }
        }

        public void SaveStores()
        {
            try
            {
                System.IO.File.WriteAllText(_customersFile, JsonConvert.SerializeObject(Customers));
                System.IO.File.WriteAllText(_locationsFile, JsonConvert.SerializeObject(Locations));
                System.IO.File.WriteAllText(_productsFile, JsonConvert.SerializeObject(Products));
                System.IO.File.WriteAllText(_componentsFile, JsonConvert.SerializeObject(Components));
                System.IO.File.WriteAllText(_ordersFile, JsonConvert.SerializeObject(Orders));
            }
            catch (Exception e)
            {
                // TODO: logging
            }
        }
        #endregion

        #region Customer
        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }
        public void AddCustomer(string firstName, string middleName, string lastName, string phoneNumber)
        {
            Customer customer = new Customer()
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                CustomerId = Customers.Count,
                PreferredStoreId = 0
            };

            Customers.Add(customer);
        }
        public Customer TryGetCustomerByNameAndPhone(string firstName, string lastName, string phoneNumber)
        {
            foreach (Customer c in Customers)
            {
                if (c.FirstName == firstName && c.LastName == lastName && c.PhoneNumber == phoneNumber)
                    return c;
            }

            throw new Exception($"No customer named {firstName} {lastName} with phone number {phoneNumber}");
        }
        public Customer TryGetCustomerById(int id)
        {
            foreach (Customer c in Customers)
            {
                if (c.CustomerId == id)
                    return c;
            }

            throw new Exception($"No customer with given id: {id}");
        }
        public List<Customer> GetAllCustomers()
        {
            return new List<Customer>(Customers);
        }
        public int GetNextCustomerId()
        {
            return Customers.Count;
        }
        #endregion

        #region Location
        public void AddLocation(Location location)
        {
            Locations.Add(location);
        }
        public Location TryGetLocationById(int id)
        {
            foreach (Location l in Locations)
            {
                if (l.LocationId == id)
                    return l;
            }

            throw new Exception($"No location found with id: {id}");
        }
        public List<Location> GetAllLocations()
        {
            return new List<Location>(Locations);
        }
        public int GetNextLocationId()
        {
            return Locations.Count;
        }
        #endregion

        #region Product
        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
        public Product TryGetProductByProductName(string name)
        {
            foreach (Product product in Products)
            {
                if (product.Name == name)
                    return product;
            }

            throw new Exception($"No recipe found with name: {name}");
        }
        public List<Product> GetAllProducts()
        {
            return new List<Product>(Products);
        }
        #endregion

        #region Component
        public void AddComponent(Component component)
        {
            Components.Add(component);
        }
        public Component TryGetComponentByName(string name)
        {
            foreach (Component c in Components)
            {
                if (c.Name == name)
                    return c;
            }

            throw new Exception($"No component with name: {name}");
        }
        public List<Component> GetAllComponents()
        {
            return new List<Component>(Components);
        }
        #endregion

        #region Order
        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }
        public Order TryGetOrderById(int id)
        {
            foreach (Order o in Orders)
            {
                if (o.OrderId == id)
                    return o;
            }

            throw new Exception($"No order with id: {id}");
        }
        public List<Order> GetOrderHistory()
        {
            return new List<Order>(Orders);
        }
        public int GetNextOrderId()
        {
            return Orders.Count;
        }
        #endregion
    }
}
