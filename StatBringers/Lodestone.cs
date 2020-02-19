using System;
using System.Collections.Generic;
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

        private readonly HttpClient httpClient;

        public Lodestone()
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://eu.finalfantasyxiv.com/lodestone/character/")
            };
        }

        private async Task<string> GetCharacterInfo(int CharacterId, string page)
        {
            string address = $"{ CharacterId }/{ page }";
            var content = await httpClient.GetStringAsync(address);
            return content;
        }

        private async Task<HttpResponseMessage> GetHeaders(int CharacterId, string page)
        {
            string address = $"{ CharacterId }/{ page }";
            var result = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, address));
            //// OK (200) for existing character, NotFound (404) for missing character 
            //var result = await client.GetAsync(address);
            //var status = result.StatusCode;
            return result;
        }
    }
}
