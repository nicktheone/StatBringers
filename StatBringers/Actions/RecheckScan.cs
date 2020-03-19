using System;
using System.Collections.Generic;
using System.Text;

namespace StatBringers.Actions
{
    class RecheckScan : IAction
    {
        public string Description { get; } = "Recheck the list of IDs (remaining IDs: )";

        public void Do()
        {
            Console.WriteLine();
            Console.WriteLine("Press ESC to stop");
            Console.WriteLine();

            //do
            //{
            //    while (!Console.KeyAvailable)
            //    {
            //        if (lodestone.CharactersToRecheckIdsList.Count > 0)
            //        {
            //            lodestone.AnalyzeValidCharacterIdsListAsync();
            //        }
            //        else
            //        {
            //            Console.WriteLine();
            //            Console.WriteLine("No IDs to recheck");
            //            Console.WriteLine();
            //            return true;
            //        }
            //    }
            //} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Console.WriteLine();
        }
    }
}
