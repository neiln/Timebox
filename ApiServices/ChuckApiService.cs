using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Timebox.Models
{
    public class ChuckApiService
    {
        private readonly HttpClient _apiClient;
        private string _url = "https://api.chucknorris.io/jokes/random?category=dev";
       
        public ChuckApiService()
        {
            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ChuckFactModel> GetChuckFact()
        {
            
            using (HttpResponseMessage response = await _apiClient.GetAsync(_url))
            {
               
                if (response.IsSuccessStatusCode)
                {
                    Task<ChuckFactModel> factModel = response.Content.ReadAsAsync<ChuckFactModel>();

                    return factModel.Result;
                }


                return new ChuckFactModel{ ResponseReason = response.ReasonPhrase};
            }

        }
    }

    public class ChuckFactModel
    {
        public string ResponseReason { get; set; }
        public string IconUrl { get; set; }
        public string Value { get; set; }
    }
}
