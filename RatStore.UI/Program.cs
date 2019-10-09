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

            Console.WriteLine("Welcome to the Rat Company! We sell rats and rat accessories.");

            LogInCustomer(ref nav);
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
                    int throwAway;

                    if (!int.TryParse(phoneNumber, out throwAway))
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
                    Console.Write($"Your name is {firstName} {lastName} and your number is {phoneNumber}. Is this correct? (y/n)");
                    if (Console.ReadKey().KeyChar != 'y')
                        continue;


                    nav.CurrentStore.DataStore.AddCustomer(firstName, middleName, lastName, phoneNumber);

                    Console.WriteLine("New customer added.");
                }

                break;
            }
        }

        static void MainMenu(ref Navigator nav)
        {
            
        }
    }
}
