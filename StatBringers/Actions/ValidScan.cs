using System;
using System.Collections.Generic;
using System.Text;

namespace StatBringers.Actions
{
    class ValidScan : IAction
    {
        public string Description { get; } = "Start a valid Character ID scan (last checked: )";

        public void Do()
        {
            Console.WriteLine();
            Console.WriteLine("Press ESC to stop");
            Console.WriteLine();

            do
            {
                while (!Console.KeyAvailable)
                {
                    //lodestone.Test();
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Console.WriteLine();
        }
    }
}
