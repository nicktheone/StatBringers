using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StatBringers
{
    class Program
    {
        static void Main(string[] args)
        {
            var lodestone = new Lodestone();

            Console.WriteLine("Press ESC to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    lodestone.Test();
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Console.WriteLine("\nValid Character IDs");
            foreach (var item in lodestone.ValidIds.OrderBy(x => x))
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}
