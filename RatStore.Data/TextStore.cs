using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace RatStore.Data
{
    public class TextStore : IDataStore
    {
        string _path, _customersFile, _locationsFile, _recipesFile, _componentsFile, _ordersFile;
        public List<Customer> Customers { get; private set; }
        public List<Location> Locations { get; private set; }
        public List<Recipe> Recipes { get; private set; }
        public List<Component> Components { get; private set; }
        public List<Order> Orders { get; private set; }

        public void InitializeTextStore()
        {
            _path = "";

            _customersFile = _path + "Customers.json";
            _locationsFile = _path + "Locations.json";
            _recipesFile = _path + "Recipes.json";
            _componentsFile = _path + "Components.json";
            _ordersFile = _path + "Orders.json";

            Customers = new List<Customer>();
            Locations = new List<Location>();
            Recipes = new List<Recipe>();
            Components = new List<Component>();
            Orders = new List<Order>();
        }

        public void LoadStores()
        {
            try
            {
                if (File.Exists(_customersFile))
                    Customers = JsonConvert.DeserializeObject< List<Customer>>(File.ReadAllText(_customersFile));

                if (File.Exists(_locationsFile))
                    Locations = JsonConvert.DeserializeObject<List<Location>>(File.ReadAllText(_locationsFile));

                if (File.Exists(_recipesFile))
                    Recipes = JsonConvert.DeserializeObject<List<Recipe>>(File.ReadAllText(_recipesFile));

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
                System.IO.File.WriteAllText(_recipesFile, JsonConvert.SerializeObject(_recipesFile));
                System.IO.File.WriteAllText(_componentsFile, JsonConvert.SerializeObject(Components));
                System.IO.File.WriteAllText(_ordersFile, JsonConvert.SerializeObject(Orders));
            }
            catch (Exception e)
            {
                // TODO: logging
            }
        }

        #region Customer
        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }
        public List<Customer> TryGetCustomerByName(string name)
        {
            /* There is a many-to-one relationship between names and customers,
            so we return a list */
            List<Customer> ret = Customers.Where(
                (Customer c) => 
                { 
                    return (c.FirstName + " " + c.MiddleName + " " + c.LastName).Contains(name); 
                }
            ).ToList();

            return ret;
        }
        public Customer TryGetCustomerById(int id)
        {
            foreach (Customer c in Customers)
            {
                if (c.Id == id)
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
                if (l.Id == id)
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

        #region Recipe
        public void AddRecipe(Recipe recipe)
        {
            Recipes.Add(recipe);
        }
        public Recipe TryGetRecipeByProductName(string name)
        {
            foreach (Recipe r in Recipes)
            {
                if (r.EndProductName == name)
                    return r;
            }

            throw new Exception($"No recipe found with name: {name}");
        }
        public List<Recipe> GetAllRecipes()
        {
            return new List<Recipe>(Recipes);
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
