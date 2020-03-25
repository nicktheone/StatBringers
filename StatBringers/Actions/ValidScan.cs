using System;

namespace StatBringers.Actions
{
    class ValidScan : IAction
    {
        public string Description { get; } = $"Start a valid Character ID scan (last checked: {Program.Lodestone.LastCharacterIdChecked})";

        public void Do()
        {
            Console.WriteLine();
            Console.WriteLine("Valid Character ID scan");
            Console.WriteLine("Press ESC to stop");
            Console.WriteLine();

            //do
            //{
            //    while (!Console.KeyAvailable)
            //    {
            //        Program.Lodestone.TestAsync();
            //    }
            //} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Program.Lodestone.Test();

            Console.WriteLine();
            Console.ReadLine();

            //Returns to the menu
            Program.MainMenu();
        }
    }
}
