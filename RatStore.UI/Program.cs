using System;
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
                            // Order submenu
                            break;
                        case '2':
                            // Information submenu
                            break;
                        case '3':
                            // Change locations submenu
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

                    break;
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

                firstName = names[0];
                if (names.Length == 2)
                {
                    middleName = "";
                    lastName = names[1];
                }
                else
                {
                    middleName = names[1];
                    lastName = names[2];
                }

                try
                {
                    nav.CurrentCustomer = nav.CurrentStore.DataStore.TryGetCustomerByNameAndPhone(firstName, lastName, phoneNumber);
                    Console.WriteLine($"Welcome back, {nav.CurrentCustomer.FirstName}!");
                }
                catch (Exception e)
                {
                    Console.Write($"Your name is {firstName} {lastName} and your number is {phoneNumber}. Is this correct? (y/n) ");
                    if (Console.ReadKey().KeyChar != 'y')
                        continue;


                    nav.CurrentStore.DataStore.AddCustomer(firstName, middleName, lastName, phoneNumber);
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
    }
}
