using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StatBringers
{
    class Lodestone
    {
        public int LastCharacterIdChecked { get { return GetLastCharacterIdChecked(); } }
        public List<int> ValidCharacterIdsList { get { return GetValidCharacterIdsList(); } }
        public List<int> CharactersToRecheckIdsList { get { return GetCharactersToRecheckIdsList(); } }
        private readonly HttpClient httpClient;
        private ConcurrentBag<int> ValidCharactersChecked { get; set; }
        private ConcurrentBag<int> CharactersToRecheck { get; set; }

        public Lodestone()
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://eu.finalfantasyxiv.com/lodestone/character/"),
                Timeout = TimeSpan.FromSeconds(10)
            };
            ValidCharactersChecked = new ConcurrentBag<int>();
            CharactersToRecheck = new ConcurrentBag<int>();
        }

        public void Test()
        {
            var tasks = new ConcurrentBag<Task>();
            var lastId = GetLastCharacterIdChecked();
            Parallel.For(lastId + 1, lastId + 31, i =>
            {
                tasks.Add(CheckIfCharacterExistsAsync(i));
            });
            Task.WaitAll(tasks.ToArray());
            
            lastId += 30;
            Console.WriteLine("STEP");

            WriteLastCharacterIdChecked(lastId);
            WriteValidCharacterIdsList();
            if (!CharactersToRecheck.IsEmpty)
            {
                WriteCharactersToRecheckList();
            }
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
                Console.WriteLine($"Failed ID: { CharacterId }");
            }
        }

        #region I/O

        private void WriteLastCharacterIdChecked(int LastCharacterIdChecked)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "LastCharacterIdChecked.txt");
            File.WriteAllText(path, LastCharacterIdChecked.ToString());
            //File.WriteAllText($"{ Directory.GetCurrentDirectory() }\\LastCharacterIdChecked.txt", LastCharacterIdChecked.ToString());
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
            //File.AppendAllLines($"{ Directory.GetCurrentDirectory() }\\ValidCharacterIdsList.txt", list.Select(x => x.ToString()));
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
