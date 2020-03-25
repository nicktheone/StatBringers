using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// https://softwareengineering.stackexchange.com/questions/405630/how-to-approach-a-large-number-of-multiple-parallel-httpclient-requests/405733#405733
/// </summary>

namespace StatBringers
{
    class Lodestone
    {
        public int LastCharacterIdChecked { get; set; } = GetLastCharacterIdChecked();
        public List<int> ValidCharacterIdsList { get; set; } = GetValidCharacterIdsList();
        public List<int> RecheckCharacterIdsList { get; } = GetRecheckCharacterIdsList();
        private readonly HttpClient httpClient; 
        private int LastCharacterIdReChecked { get; set; }
        private ConcurrentBag<int> ValidCharacterIdsBag { get; set; } = new ConcurrentBag<int>();
        private ConcurrentBag<int> RecheckCharacterIdsBag { get; set; } = new ConcurrentBag<int>();

        public Lodestone()
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://eu.finalfantasyxiv.com/lodestone/character/"),
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public void Test()
        {
            var tasks = new ConcurrentBag<Task>();
            Parallel.For(LastCharacterIdChecked + 1, LastCharacterIdChecked + 31, i =>
            {
                tasks.Add(CheckIfCharacterExistsAsync(i));
            });
            Task.WaitAll(tasks.ToArray());

            LastCharacterIdChecked += 30;

            WriteLastCharacterIdChecked(LastCharacterIdChecked);
            WriteValidCharacterIdsList();
            WriteRecheckCharacterIdsList();

            Console.WriteLine("STEP");
        }

        public void AnalyzeValidCharacterIdsListAsync()
        {
            var tasks = new ConcurrentBag<Task>();
            // Sets the maximum number of concurrent operations according to the number of IDs to check
            // After checking removes checked 
            if (RecheckCharacterIdsList.Count < 30)
            {
                Parallel.For(LastCharacterIdReChecked, RecheckCharacterIdsList.Count, i =>
                {
                    tasks.Add(CheckIfCharacterExistsAsync(RecheckCharacterIdsList[i]));
                });
                RecheckCharacterIdsList.RemoveRange(LastCharacterIdReChecked, RecheckCharacterIdsList.Count);
            }
            else
            {
                Parallel.For(LastCharacterIdReChecked, LastCharacterIdReChecked + 30, i =>
                {
                    tasks.Add(CheckIfCharacterExistsAsync(RecheckCharacterIdsList[i]));
                });
                RecheckCharacterIdsList.RemoveRange(LastCharacterIdReChecked, LastCharacterIdReChecked + 30);

            }
            Task.WaitAll(tasks.ToArray());

            LastCharacterIdChecked += 30;

            //Deletes the old list and create a new one
            var path = Path.Combine(Directory.GetCurrentDirectory(), "CharactersToRecheckIdsList.txt");
            File.Delete(path);
            WriteValidCharacterIdsList();
            WriteCombinedRecheckCharacterIdsList();
        }

        private async Task CheckIfCharacterExistsAsync(int CharacterId)
        {
            string address = $"{ CharacterId }";
            try
            {
                var result = await httpClient.GetAsync(address);

                Console.WriteLine($"{ CharacterId } - { result.StatusCode }");

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    ValidCharacterIdsBag.Add(CharacterId);
                }
                else if (result.StatusCode != HttpStatusCode.NotFound)
                {
                    RecheckCharacterIdsBag.Add(CharacterId);
                }
            }
            catch (Exception)
            {
                RecheckCharacterIdsBag.Add(CharacterId);
                Console.WriteLine($"{ CharacterId } - Failed");
            }
        }

        private async Task<string> GetCharacterInfoAsync(int CharacterId, string page)
        {
            string address = $"{ CharacterId }/{ page }";
            var content = httpClient.GetStringAsync(address);
            return await content;
        }


        #region I/O

        // Gets the last character checked
        // Returns 0 if missing file
        private static int GetLastCharacterIdChecked()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "LastCharacterIdChecked.txt");
            if (File.Exists(path))
            {
                var output = File.ReadAllText(path);
                return int.Parse(output);
            }
            else
            {
                return 0;
            }

        }

        // Gets the list of valid IDs
        // Returns an empty list if missing file
        private static List<int> GetValidCharacterIdsList()
        {
            var output = new List<int>();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "ValidCharacterIdsList.txt");
            if (File.Exists(path))
            {
                output = File.ReadAllLines(path).Select(int.Parse).ToList();
                return output;
            }
            else
            {
                return new List<int>();
            }
        }

        // Gets the list of IDs to recheck
        // Returns an empty list if missing file
        private static List<int> GetRecheckCharacterIdsList()
        {
            var output = new List<int>();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "RecheckCharacterIdsList.txt");
            if (File.Exists(path))
            {
                output = File.ReadAllLines(path).Select(int.Parse).ToList();
                return output;
            }
            else
            {
                return new List<int>();
            }
        }

        // Writes the last character checked
        private void WriteLastCharacterIdChecked(int LastCharacterIdChecked)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "LastCharacterIdChecked.txt");
            File.WriteAllText(path, LastCharacterIdChecked.ToString());
        }

        // Writes the list of valid IDs
        private void WriteValidCharacterIdsList()
        {
            var list = ValidCharacterIdsBag.ToList();
            list.Sort();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "ValidCharacterIdsList.txt");
            File.AppendAllLines(path, list.Select(x => x.ToString()));
            ValidCharacterIdsBag.Clear();
        }

        // Writes the list of IDs to recheck
        private void WriteRecheckCharacterIdsList()
        {
            var list = RecheckCharacterIdsBag.ToList();
            list.Sort();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "RecheckCharacterIdsList.txt");
            File.AppendAllLines(path, list.Select(x => x.ToString()));
            RecheckCharacterIdsBag.Clear();
        }

        private void WriteCombinedRecheckCharacterIdsList()
        {
            var list = RecheckCharacterIdsBag.ToList();
            list.AddRange(RecheckCharacterIdsList);
            list.Sort();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "CharactersToRecheckIdsList.txt");
            File.AppendAllLines(path, list.Select(x => x.ToString()));
            RecheckCharacterIdsBag.Clear();
        }

        #endregion
    }
}
