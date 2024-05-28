using Microsoft.AspNetCore.Mvc;
using WordMakerDashboard.Models;
using WordMakerDashboard.Services;
using WordMakerAPI.Services;


namespace WordMakerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordMakerController : ControllerBase
    {
        [HttpGet("GetWords")]
        public async Task<List<WordInfo>> GetWords(int wordMaxLength, int numberOfEntries, string language)
        {
            var filteredWords = new List<WordInfo>();
            while (filteredWords.Count < numberOfEntries)
            {
                var selectedWord = DatabaseService.GetRandomWord(wordMaxLength, language);
                filteredWords.Add(selectedWord);

                var selectedWordLetters = DatabaseService.SeparateLetters(selectedWord.Word);
                var allWordsWithLength = DatabaseService.GetAllWordsByLength(wordMaxLength, language);

                var additionalWords = FilterWords(allWordsWithLength, selectedWordLetters, numberOfEntries - filteredWords.Count, selectedWord);
                filteredWords.AddRange(additionalWords);

                foreach (var word in filteredWords)
                {
                    var wordToUpdate = await FillWordInfoService.TryUpdateWordInfo(word, language);
                    if (wordToUpdate.Definition != null && wordToUpdate.Definition != null)
                    {
                        DatabaseService.WordExists(wordToUpdate.Word, language, out var wordId);
                        
                        var updateDictionary = new Dictionary<string, object>
                        {
                            { "WordId", wordId },
                            { "Word", wordToUpdate.Word },
                            { "WordDefinition", wordToUpdate.Definition ?? "NULL" },
                            { "WordExample", wordToUpdate.Example ?? "NULL" },
                            { "LanguageName", language },
                        };

                        string updateQuery = @"UPDATE tblWords
                                               SET
                                                   Word = @Word,
                                                   WordDefinition = @WordDefinition,
                                                   WordExample = @WordExample
                                               WHERE WordId = @WordId
                                               AND LanguageId = (SELECT LanguageId FROM tbLanguages WHERE LanguageName = @LanguageName);";

                        DatabaseService.UpdateDatabaseEntry(updateQuery, updateDictionary);
                    }
                    else
                    {
                        var additionalWord = FilterWords(allWordsWithLength, selectedWordLetters, 1, wordToUpdate);
                        filteredWords.Remove(word);
                        filteredWords.AddRange(additionalWord);
                        continue;
                    }
                }
            }
            
            return filteredWords.ToList();
        }

        private static List<WordInfo> FilterWords(List<WordInfo> wordInfoList, List<string> letterList, int amountToGet, WordInfo selectedWord)
        {
            var filteredList = wordInfoList
                .Where(wordInfo => !wordInfo.Word.Equals(selectedWord.Word, StringComparison.OrdinalIgnoreCase) &&
                                   DatabaseService.SeparateLetters(wordInfo.Word)
                                   .All(letter => letterList.Contains(letter.ToLower())))
                .Take(amountToGet)
                .ToList();

            return filteredList;
        }
    }
}