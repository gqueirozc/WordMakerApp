using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using WordMakerDashboard.Models;

namespace WordMakerDashboard.Services
{
    public class DictionaryService
    {
        /// <summary>
        /// Loads a dictionary from a JSON file and deserializes it.
        /// </summary>
        /// <param name="filePath">The path of the file on the local computer.</param>
        /// <returns>A dictionary of string and Word containing all info from the words.</returns>
        public static List<WordInfo> LoadDictionary(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonContent = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<List<WordInfo>>(jsonContent);
                }
                else
                {
                    Console.WriteLine($"Dictionary JSON file '{filePath}' not found.");
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