using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RatStore.Data
{
    public static class Mapper
    {
        #region Component
        public static Data.Component MapComponent(Entities.Component component)
        {
            return new Data.Component
            {
                Id = component.Id,
                Name = component.Name,
                Cost = component.Cost ?? throw new ArgumentException("Argument cannot be null", nameof(component))
            };
        }
        public static Entities.Component MapComponent(Data.Component component)
        {
            return new Entities.Component
            {
                Id = component.Id,
                Name = component.Name,
                Cost = component.Cost
            };
        }
        #endregion

        #region ProductComponent
        public static Data.ProductComponent MapProductComponent(Entities.ProductComponent productComponent)
        {
            return new Data.ProductComponent
            {
                Component = MapComponent(productComponent.Component),
                Quantity = productComponent.Quantity ?? throw new ArgumentException("Argument cannot be null", nameof(productComponent))
            };
        }
        public static Entities.ProductComponent MapProductComponent(Data.ProductComponent productComponent)
        {
            return new Entities.ProductComponent
            {
                Component = MapComponent(productComponent.Component),
                Quantity = productComponent.Quantity 
            };
        }
        #endregion

        #region Product
        public static Data.Product MapProduct(Entities.Product product)
        {
            return new Data.Product
            {
                Id = product.Id,
                Name = product.Name,
                Ingredients = product.ProductComponent.Select(MapProductComponent).ToList()
            };
        }
        public static Entities.Product MapProduct(Data.Product product)
        {
            return new Entities.Product
            {
                Id = product.Id,
                Name = product.Name,
                ProductComponent = product.Ingredients.Select(MapProductComponent).ToList()
            };
        }
        #endregion

        #region Customer
        public static Data.Customer MapCustomer(Entities.Customer customer)
        {
            return new Customer
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber
            };
        }
        public static Entities.Customer MapCustomer(Data.Customer customer)
        {
            return new Entities.Customer
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber
            };
        }
        #endregion

        #region Inventory
        public static Data.Inventory MapInventory(Entities.Inventory inventoryItem)
        {
            return new Data.Inventory
            {
                Component = MapComponent(inventoryItem.Component),
                Quantity = inventoryItem.Quantity ?? throw new ArgumentException("Argument cannot be null", nameof(inventoryItem))
            };
        }
        public static Entities.Inventory MapInventory(Data.Inventory inventoryItem)
        {
            return new Entities.Inventory
            {
                Component = MapComponent(inventoryItem.Component),
                Quantity = inventoryItem.Quantity
            };
        }
        #endregion

        #region Location
        public static Data.Location MapLocation(Entities.Location location)
        {
            return new Data.Location
            {
                Id = location.Id,
                Address = location.Address,
                Inventory = location.Inventory.Select(MapInventory).ToList()
            };
        }
        public static Entities.Location MapLocation(Data.Location location)
        {
            return new Entities.Location
            {
                Id = location.Id,
                Address = location.Address,
                Inventory = location.Inventory.Select(MapInventory).ToList()
            };
        }
        #endregion

        #region OrderDetails
        public static Data.OrderDetails MapOrderDetails(Entities.OrderDetails orderDetails)
        {
            return new Data.OrderDetails
            {
                Product = MapProduct(orderDetails.Product),
                Quantity = orderDetails.Quantity ?? throw new ArgumentException("Argument cannot be null", nameof(orderDetails))
            };
        }
        public static Entities.OrderDetails MapOrderDetails(Data.OrderDetails orderDetails)
        {
            return new Entities.OrderDetails
            {
                Product = MapProduct(orderDetails.Product),
                Quantity = orderDetails.Quantity
            };
        }
        #endregion

        #region Order
        public static Data.Order MapOrder(Entities.Order order)
        {
            return new Data.Order
            {
                Id = order.Id,
                CustomerId = order.CustomerId ?? throw new ArgumentException("Argument cannot be null", nameof(order)),
                LocationId = order.LocationId ?? throw new ArgumentException("Argument cannot be null", nameof(order)),
                OrderDetails = order.OrderDetails.Select(MapOrderDetails).ToList(),
                OrderDate = order.OrderDate ?? throw new ArgumentException("Argument cannot be null", nameof(order))
            };
        }
        public static Entities.Order MapOrder(Data.Order order)
        {
            return new Entities.Order
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                LocationId = order.LocationId,
                OrderDetails = order.OrderDetails.Select(MapOrderDetails).ToList(),
                OrderDate = order.OrderDate
            };
        }
        #endregion
    }
}
