using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public abstract class Location
    {
        public IDataStore DataStore { get; protected set; }

        public string Address { get; set; }

        public int Id { get; set; }

        public Dictionary<Component, int> Inventory { get; set; }

        public List<Product> AvailableProducts { get; protected set; }

        public List<Order> OrderHistory { get; set; }

        public Location()
        {
            Inventory = new Dictionary<Component, int>();
            AvailableProducts = new List<Product>();
            OrderHistory = new List<Order>();
        }

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
        virtual public List<Product> GetAvailableProducts()
        {
            throw new Exception("No data for Location class!");
        }
        virtual public bool CanFulfillProductQty(Product product, int quantity)
        {
            foreach (Component comp in product.Ingredients.Keys)
            {
                if (product.Ingredients[comp] * quantity > Inventory[comp])
                    return false;
            }

            return true;
        }
        public bool CanFulfillOrder(Order order)
        {
            foreach (Product product in order.OrderProducts.Keys)
            {
                if (!CanFulfillProductQty(product, order.OrderProducts[product]))
                    return false;
            }

            return true;
        }
        #endregion

        #region Order Manipulation
        public Order TryBuildOrder(Customer customer, Dictionary<Product, int> products)
        {
            if (!ValidateCustomer(customer))
                throw new Exception("Order build failed: invalid customer");
            else if (!ValidateProductRequest(products))
                throw new Exception("Order build failed: invalid products dictionary");
            else
            {
                Order o = new Order()
                {
                    CustomerId = customer.Id,
                    OriginStoreId = this.Id,
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

            if (!CanFulfillOrder(order))
                throw new Exception("Inventory has insufficient components");

            foreach (Product product in order.OrderProducts.Keys)
            {
                foreach (Component component in product.Ingredients.Keys)
                {
                    Inventory[component] -= order.OrderProducts[product] * product.Ingredients[component];
                }
            }

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
