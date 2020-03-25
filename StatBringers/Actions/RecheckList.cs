using System;

namespace StatBringers.Actions
{
    class RecheckList : IAction 
    {
        public string Description { get; } = $"Get the list of Character IDs to recheck";

        public void Do()
        {
            Console.WriteLine();
            Console.WriteLine("Character IDs to recheck:");
            Console.WriteLine();

            foreach (var item in Program.Lodestone.RecheckCharacterIdsList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.ReadLine();

            //Returns to the menu
            Program.MainMenu();
        }
    }
}
