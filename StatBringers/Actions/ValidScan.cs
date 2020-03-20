using System;

namespace StatBringers.Actions
{
    class ValidScan : IAction
    {
        public string Description { get; } = $"Start a valid Character ID scan (last checked: {Program.Lodestone.LastCharacterIdChecked})";

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
