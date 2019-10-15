using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public class Location
    {
        public IDataStore DataStore { get; protected set; }

        public string Address { get; set; }

        public int LocationId { get; set; }

        virtual public List<Inventory> Inventory { get; set; }

        virtual public List<Product> AvailableProducts { get; protected set; }

        virtual public List<Order> OrderHistory { get; set; }

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
            LocationId = temp.LocationId;
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
                Inventory inventoryItem = Inventory.Find(i => i.Component.ComponentId == comp.Component.ComponentId);
                if (comp.Quantity * orderDetails.Quantity > inventoryItem.Quantity)
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
        public void SubmitOrder(Customer customer, List<OrderDetails> orderDetails)
        {
            if (!ValidateCustomer(customer))
                throw new Exception("Order build failed: invalid customer");
            else if (!ValidateProductRequest(orderDetails))
                throw new Exception("Order build failed: invalid products dictionary");

            Order o = new Order()
            {
                CustomerId = customer.CustomerId,
                LocationId = LocationId,
                OrderDetails = orderDetails,
            };

            if (!ValidateOrder(o))
                throw new Exception("Invalid order");

            if (!CanFulfillOrder(o))
                throw new Exception("Inventory has insufficient components");

            foreach (OrderDetails detail in orderDetails)
            {
                foreach (ProductComponent component in detail.Product.Ingredients)
                {
                    Inventory inventoryItem = Inventory.Find(item => item.Component.ComponentId == component.Component.ComponentId);
                    inventoryItem.Quantity -= component.Quantity * detail.Quantity;
                }
            }

            DataStore.AddOrder(o);
            DataStore.Save();
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
