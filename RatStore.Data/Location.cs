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

        public List<Inventory> Inventory { get; set; }

        public List<Product> AvailableProducts { get; protected set; }

        public List<Order> OrderHistory { get; set; }

        public Location()
        {
            Inventory = new List<Inventory>();
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
        virtual public bool CanFulfillProductQty(OrderDetails orderDetails)
        {
            foreach (ProductComponent comp in orderDetails.Product.Ingredients)
            {
                if (comp.Quantity * orderDetails.Quantity > Inventory.Find(inventoryItem => inventoryItem.Component.Id == comp.Component.Id).Quantity)
                    return false;
            }

            return true;
        }
        public bool CanFulfillOrder(Order order)
        {
            foreach (OrderDetails orderDetails in order.OrderDetails)
            {
                if (!CanFulfillProductQty(orderDetails))
                    return false;
            }

            return true;
        }
        #endregion

        #region Order Manipulation
        public Order TryBuildOrder(Customer customer, List<OrderDetails> orderDetails)
        {
            if (!ValidateCustomer(customer))
                throw new Exception("Order build failed: invalid customer");
            else if (!ValidateProductRequest(orderDetails))
                throw new Exception("Order build failed: invalid products dictionary");
            else
            {
                Order o = new Order()
                {
                    CustomerId = customer.Id,
                    LocationId = Id,
                    OrderDetails = orderDetails,
                    //Id = DataStore.GetNextOrderId()
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

            foreach (OrderDetails orderDetails in order.OrderDetails)
            {
                foreach (ProductComponent component in orderDetails.Product.Ingredients)
                {
                    Inventory inventoryItem = Inventory.Find(item => item.Component.Id == component.Component.Id);
                    inventoryItem.Quantity -= component.Quantity;
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
        virtual protected bool ValidateProductRequest(List<OrderDetails> products)
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
