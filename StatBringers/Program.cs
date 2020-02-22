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
            //var lodestone = new Lodestone();

            var showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }

            //Console.WriteLine("Press ESC to stop");
            //do
            //{
            //    while (!Console.KeyAvailable)
            //    {
            //        lodestone.Test();
            //    }
            //} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            //Console.WriteLine("\nValid Character IDs");
            //foreach (var item in lodestone.ValidIds.OrderBy(x => x))
            //{
            //    Console.WriteLine(item);
            //}

            //Console.ReadLine();
        }

        private static bool MainMenu()
        {
            var lodestone = new Lodestone();

            Console.WriteLine("Choose an option:");
            Console.WriteLine($"1) Start a valid Character ID scan (last checked: { lodestone.LastCharacterIdChecked })");
            Console.WriteLine("2) Get the list of valid Character IDs");
            Console.WriteLine("3) Exit");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine();
                    Console.WriteLine("Press ESC to stop");
                    Console.WriteLine();

                    do
                    {
                        while (!Console.KeyAvailable)
                        {
                            lodestone.Test();
                        }
                    } while (Console.ReadKey(true).Key != ConsoleKey.Escape);                   

                    return true;

                case "2":
                    Console.WriteLine();
                    Console.WriteLine("Valid Character IDs:");
                    Console.WriteLine();

                    foreach (var item in lodestone.ValidCharacterIdsList)
                    {
                        Console.WriteLine(item);
                    }

                    return true;

                case "3":
                    Console.WriteLine();
                    return false;

                default:
                    Console.WriteLine();
                    Console.WriteLine("No valid option selected");
                    Console.WriteLine();

                    return true;
            }
        }
    }
}
