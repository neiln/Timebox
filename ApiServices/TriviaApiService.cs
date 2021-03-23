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
    public class TriviaApiService
    {
        private readonly HttpClient _apiClient;
        private readonly string _baseUrl = "https://opentdb.com";
        private int _categoryId = 11;
        private string _difficulty = "easy";
        
        public TriviaApiService()
        {
            _apiClient = new HttpClient();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<TriviaApiResult> GetTrivia(TriviaOptions options, TriviaDifficulty levels)
        {

            _categoryId = (int) options;
            _difficulty = levels.ToString().ToLower();
            string endpoint = $@"{_baseUrl}/api.php?amount=1&category={_categoryId}&difficulty={_difficulty}&type=multiple";

            using (HttpResponseMessage response = await _apiClient.GetAsync(endpoint))
            {
               
                if (response.IsSuccessStatusCode)
                {
                    Task<TriviaApiResult> triviaModel = response.Content.ReadAsAsync<TriviaApiResult>();
                    //var trivia = triviaModel.Result.Results.FirstOrDefault();

                    //if (trivia != null)
                    //{
                    //    //insert the correct answer in the random spot
                    //    Random rnd = new Random();
                    //    int idx = rnd.Next(0, 3);

                    //    var lst = trivia.Incorrect_Answers.Select(x => x).ToList();
                    //    lst.Insert(idx, trivia.Correct_Answer);
                        
                    //    //add options A-D
                    //    var r = lst.Select((value, index) => $"{(char)(65+index)}. {value}");
                        
                    //    //insert indentation
                    //    string result = r.Select(i => i).Aggregate((i, j) => i + "\r\n   " + j);
                    //    result = $"{trivia.Question}\r\n   {result}";

                    //    //remove quotations and apostrophes
                    //    return result.Replace("&#039;","'").Replace("&quot;","'");


                    //}

                    return triviaModel.Result;
                }


                return null;
            }

        }
    }

    public class TriviaApiResult
    {
        public int ResponseCode { get; set; }
        public TriviaModel [] Results { get; set; }
    }   
    public class TriviaModel
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string Correct_Answer { get; set; }
        public string [] Incorrect_Answers { get; set; }
    }


    public enum TriviaDifficulty { Easy, Medium, Hard}
    public enum TriviaOptions { Movies=11, Computers=18, History=23, Gadgets=30 }
}
