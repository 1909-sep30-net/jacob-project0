using System;
using System.Collections.Generic;
using RatStore.Logic;

namespace RatStore.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Navigator nav = new Navigator();
            nav.CurrentStore = new Logic.RatStore();

            bool loggedIn = false;

            while (true)
            {
                Console.WriteLine("Welcome to the Rat Company! We sell rats and rat accessories.");

                LogInCustomer(ref nav);
                loggedIn = true;

                while (loggedIn)
                {
                    MainMenu(nav);

                    char option = Console.ReadKey().KeyChar;
                    Console.WriteLine("");

                    switch (option)
                    {
                        case '1':
                            Order(ref nav);
                            break;
                        case '2':
                            Information(nav);
                            break;
                        case '3':
                            ChangeLocation(ref nav);
                            break;
                        case '0':
                            Console.WriteLine("Logging out...");
                            loggedIn = false;
                            break;
                        default:
                            Console.WriteLine("Please choose a given number!");
                            break;
                    }
                }

                nav.CurrentStore.DataStore.Cleanup();
            }
        }

        static void LogInCustomer(ref Navigator nav)
        {
            string[] names;
            string firstName, middleName, lastName;
            string phoneNumber;

            while (true)
            {
                while (true)
                {
                    Console.Write("Please enter your full name: ");

                    names = Console.ReadLine().Split(' ');

                    if (names.Length < 2)
                    {
                        Console.WriteLine("Please enter at least your first and last name.");
                        continue;
                    }

                    firstName = names[0];
                    if (names.Length == 2)
                    {
                        middleName = "";
                        lastName = names[1];
                        break;
                    }
                    else if (names[2].ToCharArray()[0] != ' ')
                    {
                        middleName = names[1];
                        lastName = names[2];
                        break;
                    }
                    else Console.WriteLine("Please enter a valid name.");
                }

                while (true)
                {
                    Console.Write("Please enter your phone number in the format 1112223333 or 2223333:");

                    phoneNumber = Console.ReadLine();
                    Console.WriteLine("");

                    Int64 throwAway;

                    if (!Int64.TryParse(phoneNumber, out throwAway))
                    {
                        Console.WriteLine("Please enter numbers only.");
                        continue;
                    }

                    break;
                }

                

                try
                {
                    nav.CurrentCustomer = nav.CurrentStore.DataStore.TryGetCustomerByNameAndPhone(firstName, lastName, phoneNumber);
                    Console.WriteLine($"Welcome back, {nav.CurrentCustomer.FirstName}!");
                }
                catch (Exception e)
                {
                    Console.Write($"Your name is {firstName} {lastName} and your number is {phoneNumber}. Is this correct? (y/n) ");

                    char c = Console.ReadKey().KeyChar;
                    Console.WriteLine("");

                    if (c != 'y')
                        continue;
 
                    nav.CurrentStore.DataStore.AddCustomer(firstName, middleName, lastName, phoneNumber);
                    nav.CurrentStore.DataStore.Save();

                    nav.CurrentCustomer = nav.CurrentStore.DataStore.TryGetCustomerByNameAndPhone(firstName, lastName, phoneNumber);

                    Console.WriteLine("");
                    Console.WriteLine("New customer added.");
                }

                break;
            }
        }

        static void MainMenu(Navigator nav)
        {
            Console.WriteLine($"  Logged in as {nav.CurrentCustomer.FirstName} {nav.CurrentCustomer.LastName}.");
            Console.WriteLine("  Main Menu:");
            Console.WriteLine("  1 - Order from this store");
            Console.WriteLine("  2 - Get information");
            Console.WriteLine("  3 - Change locations");
            Console.WriteLine("  0 - Log out");
        }

        static void Order(ref Navigator nav)
        {
            Console.WriteLine("Let's build your order! Type in the format 1 10 for 10 of product 1.");
            Console.WriteLine("Type 'end' to stop, 'buy' to submit the order, or select from the following: ");

            while (true)
            {
                try
                {
                    nav.CurrentStore.PrintAvailableProducts();
                    string[] input = Console.ReadLine().Split(' ');
                    Console.WriteLine("");

                    if (input.Length >= 1)
                    {
                        if (input[0].ToLower() == "end")
                            break;
                        else if (input[0].ToLower() == "buy")
                        {
                            nav.CurrentStore.SubmitOrder(nav.CurrentCustomer, nav.Cart);
                            nav.CurrentStore.DataStore.Save();

                            Console.WriteLine("Order submitted!");
                            break;
                        }
                        else if (input.Length > 1)
                        {
                            int productId = int.Parse(input[0]);
                            int quantity = int.Parse(input[1]);

                            nav.AddProductToCart(productId, quantity);
                            nav.PrintCart();
                        }
                        else
                            throw new Exception("Invalid input.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid input.");
                }
            }
        }

        static void Information(Navigator nav)
        {
            // order history of current location
            // ingredient inventory of current store
            // order lookup for current customer (by id?)
            // order history for current customer

            Console.WriteLine("  What information would you like to retrieve?");
            Console.WriteLine($"  1 - Inventory of store {nav.CurrentStore.LocationId}");
            Console.WriteLine($"  2 - Look up a past order by id");
            Console.WriteLine($"  3 - Get your order history");
            Console.WriteLine($"  0 - Cancel");

            bool haveOption = false;
            while (!haveOption)
            {
                char c = Console.ReadKey().KeyChar;
                Console.WriteLine("");

                switch (c)
                {
                    case '1':
                        nav.CurrentStore.PrintInventory();
                        haveOption = true;
                        break;

                    case '2':
                        Console.Write("Enter order id: ");
                        string input = Console.ReadLine();
                        Console.WriteLine("");

                        try
                        {
                            int id = int.Parse(input);
                            nav.CurrentStore.PrintOrderAtId(id);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Please enter a valid order id.");
                        }

                        haveOption = true;
                        break;

                    case '3':
                        nav.CurrentStore.PrintCustomerOrderHistory(nav.CurrentCustomer.CustomerId);
                        haveOption = true;
                        break;

                    case '0':
                        Console.WriteLine("Canceling...");
                        haveOption = true;
                        break;

                    default:
                        Console.WriteLine("Please choose a valid option.");
                        break;
                }

                break;
            }
        }

        static void ChangeLocation(ref Navigator nav)
        {
            nav.CurrentStore.PrintAllLocations();

            while (true)
            {
                Console.Write("Enter the ID of the store you'd like to switch to: ");
                string input = Console.ReadLine();
                Console.WriteLine("");

                try
                {
                    int index = int.Parse(input);
                    nav.CurrentStore.TryChangeLocation(index);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid id.");
                }
            }
        }
    }
}
