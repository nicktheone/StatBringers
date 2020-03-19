using System;

namespace StatBringers.Actions
{
    class RecheckList : IAction 
    {
        public string Description { get; } = "Get the list of Character IDs to recheck";

        public void Do()
        {
            Console.WriteLine();
            Console.WriteLine("Character IDs to recheck:");
            Console.WriteLine();

            //foreach (var item in lodestone.CharactersToRecheckIdsList)
            //{
            //    Console.WriteLine(item);
            //}

            Console.WriteLine();
        }
    }
}
