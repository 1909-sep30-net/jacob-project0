using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;

namespace RatStore.UI
{
    static class RatStoreConsoleExtensions
    {
        public static void PrintAvailableProducts(this Location location)
        {
            location.UpdateAvailableProducts();

            for (int i = 0; i < location.AvailableProducts.Count; ++i)
            {
                Console.WriteLine($"{i}: {location.AvailableProducts[i]}");
            }
        }

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

        public static void PrintAllRecipes(this Location location)
        {
            Console.WriteLine($"The following recipes are available at store {location.Id}:");
            foreach (Recipe recipe in location.DataStore.GetAllRecipes())
            {
                if (location.CanFulfillRecipeQty(recipe, 1))
                {
                    Console.WriteLine($"Recipe for {recipe.EndProductName}:");
                    foreach (Component component in recipe.Ingredients.Keys)
                    {
                        Console.WriteLine($"    {component.Name} x {recipe.Ingredients[component]}");
                    }
                }
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
    }
}
