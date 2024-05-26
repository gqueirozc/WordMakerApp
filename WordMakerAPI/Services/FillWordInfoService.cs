using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WordMakerDashboard.Models;

namespace WordMakerAPI.Services
{
    public class FillWordInfoService
    {
        private static readonly HttpClient _client;
        private static readonly string API_KEY = "------";
        static FillWordInfoService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("x-rapidapi-key", API_KEY);
            _client.DefaultRequestHeaders.Add("x-rapidapi-host", "wordsapiv1.p.rapidapi.com");
        }

        /// <summary>
        /// Tries to update the wordinfo if necessary via public APIs.
        /// </summary>
        /// <param name="wordToUpdate">The WordInfo with the word to be updated.</param>
        /// <param name="dictionaryLanguage">The language of the dictionary for the update on database.</param>
        /// <returns>Dictionary with information to update database.</returns>
        public static async Task<WordInfo> TryUpdateWordInfo(WordInfo wordToUpdate, string dictionaryLanguage)
        {
            if (wordToUpdate == null)
            {
                throw new ArgumentNullException(nameof(wordToUpdate));
            }

            var updatedWordInfo = await TryGetWordDefinitionAndExample(wordToUpdate.Word);

            wordToUpdate.SourceUrl = updatedWordInfo.SourceUrl ?? $"https://en.wiktionary.org/wiki/{wordToUpdate.Word}";

            if (updatedWordInfo?.SourceUrl != null) wordToUpdate.SourceUrl = updatedWordInfo.SourceUrl;
            if (string.IsNullOrEmpty(wordToUpdate.Definition) || string.IsNullOrEmpty(wordToUpdate.Example))
            {
                try
                {
                    if (updatedWordInfo != null)
                    {
                        if (string.IsNullOrEmpty(wordToUpdate.Definition))
                        {
                            updatedWordInfo.Definition ??= await GetWordDefinitionAsync(wordToUpdate.Word);
                            wordToUpdate.Definition = updatedWordInfo.Definition;
                        }

                        if (string.IsNullOrEmpty(wordToUpdate.Example))
                        {
                            updatedWordInfo.Example ??= await GetWordExampleAsync(wordToUpdate.Word);
                            wordToUpdate.Example = updatedWordInfo.Example;
                        }
                    }
                    else
                    {             
                        if (string.IsNullOrEmpty(wordToUpdate.Definition))
                            wordToUpdate.Definition = await GetWordDefinitionAsync(wordToUpdate.Word);

                        if (string.IsNullOrEmpty(wordToUpdate.Example))
                            wordToUpdate.Example = await GetWordExampleAsync(wordToUpdate.Word);
                    }
                }
                catch
                {
                    Console.WriteLine("Error fetching API...");
                }
            }

            return wordToUpdate;
        }

        private static async Task<WordInfo> TryGetWordDefinitionAndExample(string wordToFind)
        {
            var updatedWordInfo = new WordInfo();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync($"https://api.dictionaryapi.dev/api/v2/entries/en/{wordToFind}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        dynamic data = JArray.Parse(responseBody);

                        foreach (var meaning in data[0].meanings)
                        {
                            if (meaning.partOfSpeech == "verb")
                            {
                                updatedWordInfo.Definition = meaning.definitions[0].definition;
                                updatedWordInfo.Example = meaning.definitions[0].example;
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(updatedWordInfo.Definition) || string.IsNullOrEmpty(updatedWordInfo.Example))
                        {
                            updatedWordInfo.Definition = data[0].meanings[0].definitions[0].definition;
                            updatedWordInfo.Example = data[0].meanings[0].definitions[0].example;
                        }

                        updatedWordInfo.SourceUrl = data[0].sourceUrls[0];
                    }
                    else
                    {
                        return updatedWordInfo;
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error fetching API...", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching API...", ex);
                }
            }

            return updatedWordInfo;
        }

        private static async Task<string> GetWordDefinitionAsync(string word)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://wordsapiv1.p.rapidapi.com/words/{word}/definitions")
            };

            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                dynamic data = JsonConvert.DeserializeObject(responseBody);
                var definition = "";
                foreach (var definitionObj in data.definitions)
                {
                    definition = definitionObj.definition;
                    break;
                }

                return definition;
            }
        }

        private static async Task<string> GetWordExampleAsync(string word)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://wordsapiv1.p.rapidapi.com/words/{word}/examples")
            };

            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                dynamic data = JsonConvert.DeserializeObject(responseBody);
                var examples = new List<string>();

                foreach (var example in data.examples)
                {
                    examples.Add(example.ToString()); // Convert to string explicitly
                }

                var random = new Random();
                var randomIndex = random.Next(examples.Count);
                return examples[randomIndex];
            }
        }
    }
}

