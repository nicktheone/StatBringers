using System;
using System.Collections.Generic;
using System.Text;

namespace StatBringers.Actions
{
    class ValidList : IAction
    {
        public string Description { get; } = "Get the list of valid Character IDs";

        public void Do()
        {
            Console.WriteLine();
            Console.WriteLine("Valid Character IDs:");
            Console.WriteLine();

            //foreach (var item in lodestone.ValidCharacterIdsList)
            //{
            //    Console.WriteLine(item);
            //}

            Console.WriteLine();
        }
    }
}
