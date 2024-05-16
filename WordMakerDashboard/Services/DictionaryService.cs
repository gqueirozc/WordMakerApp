using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace WordMakerDashboard.Services
{
    public class DictionaryService
    {
        public static Dictionary<string, string> LoadDictionary(string languageCode)
        {
            var filePath = "C:\\Users\\guilh\\Downloads\\dictionary.json";

            try
            {
                if (File.Exists(filePath))
                {
                    string jsonContent = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
                }
                else
                {
                    Console.WriteLine($"Dictionary file '{languageCode}.json' not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dictionary: {ex.Message}");
                return null;
            }
        }
    }
}
