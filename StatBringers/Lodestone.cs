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
        /// <TODO>
        /// Benchmark:
        /// httpClient.GetStringAsync()
        /// httpClient.GetAsync()
        /// httpClient.SendAsync()
        /// 
        /// Get a list of unique IDs from Lodestone
        /// </TODO>

        public int LastCharacterIdChecked { get { return GetLastCharacterIdChecked(); } }
        public List<int> ValidCharacterIdsList { get { return GetValidCharacterIdsList(); } }
        private readonly HttpClient httpClient;
        private ConcurrentBag<string> ValidCharacterIds { get; set; }

        public Lodestone()
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://eu.finalfantasyxiv.com/lodestone/character/")
            };
            ValidCharacterIds = new ConcurrentBag<string>();
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
            var result = await httpClient.GetAsync(address);
            Console.WriteLine($"{ CharacterId } - { result.StatusCode }");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                ValidCharacterIds.Add(CharacterId.ToString());
            }
        }

        private void WriteLastCharacterIdChecked(int LastCharacterIdChecked)
        {
            File.WriteAllText($"{ Directory.GetCurrentDirectory() }\\LastCharacterIdChecked.txt", LastCharacterIdChecked.ToString());
        }

        private int GetLastCharacterIdChecked()
        {
            var path = $"{ Directory.GetCurrentDirectory() }\\LastCharacterIdChecked.txt";
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
            File.AppendAllLines($"{ Directory.GetCurrentDirectory() }\\ValidCharacterIdsList.txt", ValidCharacterIds);
            ValidCharacterIds.Clear();
        }

        private List<int> GetValidCharacterIdsList()
        {
            var output = new List<int>();
            output = File.ReadAllLines($"{ Directory.GetCurrentDirectory() }\\ValidCharacterIdsList.txt").Select(int.Parse).ToList();
            output.Sort();
            return output;
        }
    }
}
