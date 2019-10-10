using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;

namespace RatStore.UI
{
    static class RatStoreConsoleExtensions
    {
        //public static void PrintAvailableProducts(this Location location)
        //{
        //    location.UpdateAvailableProducts();

        //    for (int i = 0; i < location.AvailableProducts.Count; ++i)
        //    {
        //        Console.WriteLine($"{i}: {location.AvailableProducts[i]}");
        //    }
        //}

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
            foreach (Component component in location.Inventory.Keys)
            {
                Console.WriteLine($"   {component.Name} x {location.Inventory[component]}");
            }
        }

        public static void PrintAvailableProducts(this Location location)
        {
            Console.WriteLine($"The following products are available at store {location.Id}:");
            List<Recipe> allRecipes = location.DataStore.GetAllRecipes();
            for (int i = 0; i < allRecipes.Count; ++i)
            {
                if (location.CanFulfillRecipeQty(allRecipes[i], 1))
                {
                    Console.WriteLine($"{i}: {allRecipes[i].EndProductName} --");
                    foreach (Component component in allRecipes[i].Ingredients.Keys)
                    {
                        Console.WriteLine($"    {component.Name} x {allRecipes[i].Ingredients[component]}");
                    }
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

        public static void PrintLocationOrderHistory(this Location thisLocation)
        {
            foreach (Order order in thisLocation.OrderHistory)
            {
                Console.WriteLine($"{order.OrderId}: {order.OrderProducts.Count} products ordered on {order.OrderTimestamp.ToShortDateString()} for {order.Const}");
            }
        }
    }
}
