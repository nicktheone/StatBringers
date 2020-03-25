using System;

namespace StatBringers.Actions
{
    class ValidList : IAction
    {
        public string Description { get; } = $"Get the list of valid Character IDs (total: { Program.Lodestone.ValidCharacterIdsList.Count })";

        public void Do()
        {
            Console.WriteLine();
            Console.WriteLine("Valid Character IDs:");
            Console.WriteLine();

            foreach (var item in Program.Lodestone.ValidCharacterIdsList)
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
