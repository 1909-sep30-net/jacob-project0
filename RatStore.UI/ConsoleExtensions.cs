using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RatStore.Data;
using RatStore.Logic;

namespace RatStore.UI
{
    static class ConsoleExtensions
    {
        #region RatStore
        public static void PrintStoreInformation(this Location location)
        {
            Console.WriteLine($"You are currently at store {location.Id}.");
            Console.WriteLine($"This store has {location.AvailableProducts.Count} different products you can choose from:");
            PrintAvailableProducts(location);
        }

        public static void PrintCustomerInformation(this Location location, int customerId)
        {
            try
            {
                Customer customer = location.DataStore.TryGetCustomerById(customerId);
                Console.WriteLine($"Records for customer {customer.Id}:");

                string middle = (customer.MiddleName != null && customer.MiddleName != "") ? customer.MiddleName + " " : "";
                Console.WriteLine($"Name: {customer.FirstName} {middle}{customer.LastName}"); // Weird spacing on purpose
                Console.WriteLine($"Phone Number: {customer.PhoneNumber}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void PrintInventory(this Location location) //(of components)
        {
            Console.WriteLine($"The following ingredients and stocks are available at store {location.Id}:");
            foreach (Inventory inventoryItem in location.Inventory)
            {
                Console.WriteLine($"   {inventoryItem.Component.Name} x {inventoryItem.Quantity}");
            }
        }

        public static void PrintAvailableProducts(this Location location)
        {
            Console.WriteLine($"The following products are available at store {location.Id}:");
            List<Product> availableProducts = location.GetAvailableProducts();
            for (int i = 0; i < availableProducts.Count; ++i)
            {
                Console.WriteLine($"{i}: {availableProducts[i].Name} --");
                foreach (ProductComponent ingredient in availableProducts[i].Ingredients)
                {
                    Console.WriteLine($"    {ingredient.Component.Name} x {ingredient.Quantity}");
                }
            }
        }

        public static void PrintAllLocations(this Location thisLocation)
        {
            Console.WriteLine($"The following locations exist within the company: ");
            List<Location> allLocations = thisLocation.DataStore.GetAllLocations();
            for (int i = 0; i < allLocations.Count; ++i)
            {
                if (allLocations[i].Id == thisLocation.Id)
                    Console.WriteLine($"{i}: location {allLocations[i].Id} at {allLocations[i].Address} (this store)");
                else 
                    Console.WriteLine($"{i}: location {allLocations[i].Id} at {allLocations[i].Address}");
            }
        }

        public static void PrintOrderAtId(this Location location, int id)
        {
            Order order = location.DataStore.TryGetOrderById(id);
            Console.WriteLine($"{order.Id}: {order.OrderDetails.Count} products ordered on {order.OrderDate.ToShortDateString()} for {order.Const}");
        }

        public static void PrintCustomerOrderHistory(this Location location, int customerId)
        {
            List<Order> customerOrderHistory = location.DataStore.GetOrderHistory(customerId).ToList();

            if (customerOrderHistory.Count == 0)
                Console.WriteLine("Order history is empty for this customer.");
            else
                foreach (Order order in customerOrderHistory)
                {
                    Console.WriteLine($"{order.Id}: {order.OrderDetails.Count} products ordered on {order.OrderDate.ToShortDateString()} for {order.Const}");
                }
        }

        public static void PrintLocationOrderHistory(this Location thisLocation)
        {
            if (thisLocation.OrderHistory.Count == 0)
                Console.WriteLine("Order history is empty for this location.");
            else
                foreach (Order order in thisLocation.OrderHistory)
                {
                    Console.WriteLine($"{order.Id}: {order.OrderDetails.Count} products ordered on {order.OrderDate.ToShortDateString()} for {order.Const}");
                }
        }
        #endregion

        #region Navigator
        public static void PrintCart(this Navigator navigator)
        {
            Console.WriteLine($"Total: ${navigator.Subtotal.ToString("C0")}");
        }
        #endregion
    }
}
