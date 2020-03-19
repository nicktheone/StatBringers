using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

/// <summary>
/// https://softwareengineering.stackexchange.com/questions/405630/how-to-approach-a-large-number-of-multiple-parallel-httpclient-requests/405733#405733
/// </summary>

namespace StatBringers
{
    class Lodestone
    {
        public int LastCharacterIdChecked { get; set; }
        public List<int> ValidCharacterIdsList { get; set; }
        public List<int> CharactersToRecheckIdsList { get ; }
        private readonly HttpClient httpClient;
        private int LastCharacterIdReChecked { get; set; }
        private ConcurrentBag<int> ValidCharactersChecked { get; set; } = new ConcurrentBag<int>();
        private ConcurrentBag<int> CharactersToRecheck { get; set; } = new ConcurrentBag<int>();

        public Lodestone()
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://eu.finalfantasyxiv.com/lodestone/character/"),
                Timeout = TimeSpan.FromSeconds(10)
            };

            LastCharacterIdChecked = GetLastCharacterIdChecked();
            ValidCharacterIdsList = GetValidCharacterIdsList();
            CharactersToRecheckIdsList = GetCharactersToRecheckIdsList();
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
            Console.WriteLine("STEP");

            WriteLastCharacterIdChecked(LastCharacterIdChecked);
            WriteValidCharacterIdsList();
            WriteCharactersToRecheckList();
        }

        public void AnalyzeValidCharacterIdsListAsync()
        {
            var tasks = new ConcurrentBag<Task>();
            // Sets the maximum number of concurrent operations according to the number of IDs to check
            // After checking removes checked 
            if (CharactersToRecheckIdsList.Count < 30)
            {
                Parallel.For(LastCharacterIdReChecked, CharactersToRecheckIdsList.Count, i =>
                {
                    tasks.Add(CheckIfCharacterExistsAsync(CharactersToRecheckIdsList[i]));
                });
                CharactersToRecheckIdsList.RemoveRange(LastCharacterIdReChecked, CharactersToRecheckIdsList.Count);
            }
            else
            {
                Parallel.For(LastCharacterIdReChecked, LastCharacterIdReChecked + 30, i =>
                {
                    tasks.Add(CheckIfCharacterExistsAsync(CharactersToRecheckIdsList[i]));
                });
                CharactersToRecheckIdsList.RemoveRange(LastCharacterIdReChecked, LastCharacterIdReChecked + 30);

            }
            Task.WaitAll(tasks.ToArray());

            LastCharacterIdChecked += 30;

            //Deletes the old list and create a new one
            var path = Path.Combine(Directory.GetCurrentDirectory(), "CharactersToRecheckIdsList.txt");
            File.Delete(path);
            WriteValidCharacterIdsList();
            WriteCombinedCharactersToRecheckList();
        }

        private async Task<string> GetCharacterInfoAsync(int CharacterId, string page)
        {
            string address = $"{ CharacterId }/{ page }";
            var content = httpClient.GetStringAsync(address);
            return await content;
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
                    ValidCharactersChecked.Add(CharacterId);
                }
                else if (result.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    CharactersToRecheck.Add(CharacterId);
                }
            }
            catch (Exception)
            {
                CharactersToRecheck.Add(CharacterId);
                Console.WriteLine($"{ CharacterId } - Failed");
            }
        }

        #region I/O

        private void WriteLastCharacterIdChecked(int LastCharacterIdChecked)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "LastCharacterIdChecked.txt");
            File.WriteAllText(path, LastCharacterIdChecked.ToString());
        }

        private int GetLastCharacterIdChecked()
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

        private void WriteValidCharacterIdsList()
        {
            var list = ValidCharactersChecked.ToList();
            list.Sort();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "ValidCharacterIdsList.txt");
            File.AppendAllLines(path, list.Select(x => x.ToString()));
            ValidCharactersChecked.Clear();
        }

        private List<int> GetValidCharacterIdsList()
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

        private void WriteCharactersToRecheckList()
        {
            var list = CharactersToRecheck.ToList();
            list.Sort();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "CharactersToRecheckIdsList.txt");
            File.AppendAllLines(path, list.Select(x => x.ToString()));
            CharactersToRecheck.Clear();
        }

        private void WriteCombinedCharactersToRecheckList()
        {
            var list = CharactersToRecheck.ToList();
            list.AddRange(CharactersToRecheckIdsList);
            list.Sort();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "CharactersToRecheckIdsList.txt");
            File.AppendAllLines(path, list.Select(x => x.ToString()));
            CharactersToRecheck.Clear();
        }

        private List<int> GetCharactersToRecheckIdsList()
        {
            var output = new List<int>();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "CharactersToRecheckIdsList.txt");
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

        #endregion
    }
}
