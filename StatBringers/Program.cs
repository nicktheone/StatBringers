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
            var showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }

        private static bool MainMenu()
        {
            var lodestone = new Lodestone();

            Console.WriteLine("Choose an option:");
            Console.WriteLine($"1) Start a valid Character ID scan (last checked: { lodestone.LastCharacterIdChecked })");
            Console.WriteLine("2) Get the list of valid Character IDs");
            Console.WriteLine("3) Get the list of Character IDs to recheck");
            Console.WriteLine($"4) Recheck the list of IDs (remaining IDs: { lodestone.CharactersToRecheckIdsList.Count })");
            Console.WriteLine("5) Exit");

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

                    Console.WriteLine();

                    return true;

                case "2":
                    Console.WriteLine();
                    Console.WriteLine("Valid Character IDs:");
                    Console.WriteLine();

                    foreach (var item in lodestone.ValidCharacterIdsList)
                    {
                        Console.WriteLine(item);
                    }

                    Console.WriteLine();

                    return true;

                case "3":
                    Console.WriteLine();
                    Console.WriteLine("Character IDs to recheck:");
                    Console.WriteLine();

                    foreach (var item in lodestone.CharactersToRecheckIdsList)
                    {
                        Console.WriteLine(item);
                    }

                    Console.WriteLine();

                    return true;

                case "4":
                    Console.WriteLine();
                    Console.WriteLine("Press ESC to stop");
                    Console.WriteLine();

                    do
                    {
                        while (!Console.KeyAvailable)
                        {
                            if (lodestone.CharactersToRecheckIdsList.Count > 0)
                            {
                                lodestone.AnalyzeValidCharacterIdsListAsync();
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("No IDs to recheck");
                                Console.WriteLine();
                                return true;
                            }
                        }
                    } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                    Console.WriteLine();
                    return true;

                case "5":
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
