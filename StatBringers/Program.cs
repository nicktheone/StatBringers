using StatBringers.Actions;
using System;
using System.Collections.Generic;

namespace StatBringers
{
    class Program
    {
        private static List<IAction> MenuItems { get; set; }
        public Lodestone Lodestone { get; set; } = new Lodestone();

        static void Main(string[] args)
        {
            PopulateMenu();
            MainMenu();

            Console.ReadLine();
        }

        private static void MainMenu()
        {
            ShowMenuOptions();

            int input;
            // Loops until valid input is given
            do
            {
                Console.WriteLine($"Choose an option between 1 and { MenuItems.Count }");
            } while (!int.TryParse(Console.ReadLine(), out input) || input < 1 || input > MenuItems.Count);

            MenuItems[input - 1].Do();
        }

        // List containing all the menu item to be displayed
        // Used to populate a list in the start up process
        private static void PopulateMenu()
        {
            var actions = new List<IAction>
            {
                new ValidScan(),
                new RecheckScan(),
                new ValidList(),
                new RecheckList(),
                new Exit()
            };

            MenuItems = actions;
        }

        // Prints out every item contained in the menu list
        private static void ShowMenuOptions()
        {
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {MenuItems[i].Description}");
            }

            Console.WriteLine();
        }
    }
}
