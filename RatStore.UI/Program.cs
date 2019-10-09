using System;

namespace RatStore.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            int currentStoreId = 1;
            string name = "Jacob";

            Console.WriteLine("Welcome to the Rat Company!");
            Console.WriteLine($"You are now logged in as {name}.");
            Console.WriteLine($"You are currently in store {currentStoreId}");
            Console.WriteLine("");

            Console.WriteLine($"Currently, we have the following products in stock: ");
            PrintAvailableProducts(currentStoreId);
            PrintMenu(currentStoreId);

            string s = Console.ReadLine();
        }

        static void PrintAvailableProducts(int storeId)
        {
            Console.WriteLine("1 - BigRat x 6");
            Console.WriteLine("2 - FatRat x 1");
            Console.WriteLine("3 - RatFood x 7");
            Console.WriteLine("4 - BigRatCage x 1");
            Console.WriteLine("");
        }

        static void PrintMenu(int storeId)
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1: submit and order to this location");
            Console.WriteLine("2: change locations");
            Console.WriteLine("3: log out");
        }
    }
}
