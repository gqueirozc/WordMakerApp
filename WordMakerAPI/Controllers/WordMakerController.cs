using Microsoft.AspNetCore.Mvc;
using WordMakerDashboard.Models;
using WordMakerDashboard.Services;
using System.Linq;
using System.Text.Json;
using System.Collections;

namespace WordMakerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordMakerController : ControllerBase
    {
        private DatabaseService dbOperations;

        public WordMakerController()
        {
            dbOperations = new DatabaseService();
        }

        [HttpGet("GetWords")]
        public List<WordInfo> GetWords(int wordMaxLength, int numberOfEntries, string language)
        {
            var similarWords = new List<WordInfo>();
            var attempts = 0;

            while (similarWords.Count < numberOfEntries && attempts < numberOfEntries * 10)
            {
                var word = DatabaseService.GetRandomWord(wordMaxLength, language);
                var letters = DatabaseService.GetLetters(word);

                var additionalSimilarWords = DatabaseService.GetRandomWords(wordMaxLength, letters, numberOfEntries - similarWords.Count, language);
                if (additionalSimilarWords.Count < numberOfEntries)
                {
                    attempts++;
                    continue;
                }

                similarWords.AddRange(additionalSimilarWords);
            }

            //foreach (var entry in similarWords)
            //{
            //    var word = entry.Word;
            //    var definition = entry.Definition;
            //    var example = entry.Example;
            //    if (definition == null)
            //    {
            //        var resp = await GetWordInfo("apple");
            //    }
            //}

            return similarWords.Take(numberOfEntries).ToList();
        }

        //private static async Task<WordInfo> GetWordInfo(string word)
        //{
        //    WordInfo result = null;

        //    try
        //    {
        //        var client = new HttpClient();
        //        var definitionUrl = $"https://wordsapiv1.p.rapidapi.com/words/{word}/definitions";
        //        var exampleUrl = $"https://wordsapiv1.p.rapidapi.com/words/{word}/examples";

        //        var definitionRequest = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Get,
        //            RequestUri = new Uri(definitionUrl),
        //            Headers =
        //        {
        //            { "X-RapidAPI-Key", "d9ec980d4dmsh496fb76da9d64bdp1ec87ejsn2c437e356f9b" },
        //            { "X-RapidAPI-Host", "wordsapiv1.p.rapidapi.com" }
        //        }
        //        };

        //        var exampleRequest = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Get,
        //            RequestUri = new Uri(exampleUrl),
        //            Headers =
        //            {
        //                { "X-RapidAPI-Key", "d9ec980d4dmsh496fb76da9d64bdp1ec87ejsn2c437e356f9b" },
        //                { "X-RapidAPI-Host", "wordsapiv1.p.rapidapi.com" }
        //            }
        //        };

        //        var definitionResponse = await client.SendAsync(definitionRequest);
        //        definitionResponse.EnsureSuccessStatusCode();
        //        var definitionBody = await definitionResponse.Content.ReadAsStringAsync();

        //        var exampleResponse = await client.SendAsync(exampleRequest);
        //        exampleResponse.EnsureSuccessStatusCode();
        //        var exampleBody = await exampleResponse.Content.ReadAsStringAsync();

        //        var definitionJson = JsonDocument.Parse(definitionBody);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }

        //    return result;
        //}
    }
}