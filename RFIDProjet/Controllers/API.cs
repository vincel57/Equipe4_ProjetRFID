/*
 * using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using APIRFID.Model;
using Newtonsoft.Json;

namespace RFIDProjet.ControllersAPI
{
    public sealed class API
    {
        private static readonly HttpClient client = new HttpClient();

        private API()
        {
            client.BaseAddress = new Uri("https://localhost:7188");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private static readonly object padlock = new object();
        private static API instance = null;

        public static API Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new API();
                    }
                    return instance;
                }
            }
        }

        public async Task<UserE> GetUserE(int id)
        {
            UserE usere = null;
            HttpResponseMessage response = client.GetAsync("api/userEs/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                usere = JsonConvert.DeserializeObject<UserE>(resp);
            }
            return usere;
        }

        public async Task<UserE> GetUserE(string idString)
        {
            int id;
            if (int.TryParse(idString, out id))
                return await GetUserE(id);
            return null;
        }

        public async Task<UserE> GetUserE(int loginE, string passwordE)
        {
            UserE usere = null;
            HttpResponseMessage response = client.GetAsync("api/userEs/" + loginE + "/" + passwordE).Result;
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                usere = JsonConvert.DeserializeObject<UserE>(resp);
            }
            return usere;
        }
    }
    
}
*/