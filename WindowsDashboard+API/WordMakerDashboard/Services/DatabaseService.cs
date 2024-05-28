using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WordMakerDashboard.Models;

namespace WordMakerDashboard.Services
{
    /// <summary>
    /// Contains all operations related to the POSTGRE ElephantSQL database used to storage all data.
    /// </summary>
    public class DatabaseService
    {
        public static string connectionString = "Host=isabelle.db.elephantsql.com;Username=zrmfzafr;Password=2F0bg6YYX51w_NRMFvO8Lhw3PrMBpW0s;Database=zrmfzafr";

        /// <summary>
        /// Sends a select all query to the database.
        /// </summary>
        /// <param name="tableName">Name of the table to be selected</param>
        /// <param name="strSelect">[optional] - Full query string for the select. Will be used instead of the standard.</param>
        /// <returns>Returns a DataTable will all the information from the query.</returns>
        public static DataTable SelectAllFromDatabase(string tableName, string strSelect = "")
        {
            if (strSelect == "") strSelect = $"SELECT * FROM {tableName};";
            var dataTable = new DataTable();
            using (var oConnection = new NpgsqlConnection(connectionString))
            {
                using (var oCommand = new NpgsqlCommand(strSelect, oConnection))
                {
                    var dataAdapter = new NpgsqlDataAdapter(oCommand);
                    oConnection.Open();
                    dataAdapter.Fill(dataTable);
                    oConnection.Close();
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Deletes an entry from the database table.
        /// </summary>
        /// <param name="tableName">Name of the table for the entry to be deleted from.</param>
        /// <param name="idFieldName">Field of reference for the deletion</param>
        /// <param name="deletedId">The identification for the entry that will be deleted</param>
        public static void DeleteFromDatabaseTable(string tableName, string idFieldName, int deletedId)
        {
            var strDELETE = $"DELETE FROM {tableName} WHERE {idFieldName} = @{idFieldName}";
            using (var oConnection = new NpgsqlConnection(connectionString))
            {
                using (var oCommand = new NpgsqlCommand(strDELETE, oConnection))
                {
                    oCommand.Parameters.AddWithValue($"@{idFieldName}", deletedId);
                    oConnection.Open();
                    oCommand.ExecuteNonQuery();
                    oConnection.Close();
                }
            }
        }

        /// <summary>
        /// Updates the database entry.
        /// </summary>
        /// <param name="updateQuery">Query for the update with placeholders</param>
        /// <param name="updateValues">Dictionary containing the key as the placeholder name, and the value as its actual value to be updated.</param>
        public static void UpdateDatabaseEntry(string updateQuery, Dictionary<string, object> updateValues)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var updateCommand = new NpgsqlCommand(updateQuery, connection))
                {
                    foreach (var item in updateValues)
                    {
                        updateCommand.Parameters.AddWithValue($"@{item.Key}", item.Value);
                    }
                    updateCommand.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        /// <summary>
        /// Executes a singular query on database.
        /// </summary>
        /// <param name="query">string of the full query to be executed.</param>
        public static void ExecuteQuery(string query)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var updateCommand = new NpgsqlCommand(query, connection))
                {
                    updateCommand.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        /// <summary>
        /// Validates if a Language is existent on the database.
        /// </summary>
        /// <param name="languageName">Name of the language to be checked.</param>
        /// <returns>true if the language exists, otherwise, false.</returns>
        public static bool LanguageExists(string languageName)
        {
            bool exists = false;
            string query = $"SELECT COUNT(*) FROM tbLanguages WHERE LanguageName = '{languageName}'";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                using (var command = new NpgsqlCommand(query, connection))
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    connection.Close();

                    if (result != null && int.TryParse(result.ToString(), out int count))
                    {
                        exists = (count > 0);
                    }
                }
                connection.Close();
            }

            return exists;
        }

        /// <summary>
        /// Validates if a Word is existent on the database.
        /// </summary>
        /// <param name="word">The word to be checked.</param>
        /// <param name="languageName">Name of the language of the word.</param>
        /// <returns>true if the word exists, otherwise, false.</returns>
        public static bool WordExists(string word, string languageName, out int wordId)
        {
            wordId = -1;
            bool exists = false;
            string query = $"SELECT COUNT(*) " +
                           $"FROM tblWords w " +
                           $"INNER JOIN tbLanguages l ON w.LanguageId = l.LanguageId " +
                           $"WHERE w.Word = '{word}' AND l.LanguageName = '{languageName}'";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int count))
                    {
                        exists = (count > 0);
                    }
                }

                if (exists)
                {
                    string queryWordId = $"SELECT WordId " +
                                         $"FROM tblWords w " +
                                         $"INNER JOIN tbLanguages l ON w.LanguageId = l.LanguageId " +
                                         $"WHERE w.Word = '{word}' AND l.LanguageName = '{languageName}'";

                    using (var command = new NpgsqlCommand(queryWordId, connection))
                    {
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            wordId = Convert.ToInt32(result);
                        }
                    }
                }
                connection.Close();
            }

            return exists;
        }

        /// <summary>
        /// Populates the database with a full dictionary from JSON.
        /// </summary>
        /// <param name="wordDictionary">A deserialized dictionary from a JSON file containing the word's info.</param>
        /// <param name="languageName">The language of the dictionary for the words to be added to.</param>
        /// <param name="bgWorker">An instance of the background worker to update the progress during its process.</param>
        public static void PopulateDatabaseWithNewDictionary(List<WordInfo> wordDictionary, string languageName, BackgroundWorker bgWorker)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var wordsToInsertOrUpdate = wordDictionary.ToList();

                wordsToInsertOrUpdate.Sort((x, y) => string.Compare(x.Word, y.Word, StringComparison.Ordinal));
                var entriesToInsert = wordsToInsertOrUpdate.Where(entry => !ContainsInvalidCharacters(entry.Word));

                var currentIteration = 0;
                foreach (var entry in entriesToInsert)
                {
                    var word = entry.Word;
                    bool wordExists = WordExists(word, languageName, out var wordId);

                    var newData = new Dictionary<string, object>
                    {
                        { "Word", word },
                        { "WordDefinition", entry.Definition },
                        { "WordExample", entry.Example },
                        { "LanguageName", languageName },
                        { "WordId", wordId }
                    };

                    currentIteration += 1;
                    bgWorker.ReportProgress(currentIteration * 100 / entriesToInsert.Count());

                    if (wordExists)
                    {
                        UpdateWord(newData);
                    }
                    else
                    {
                        InsertWord(newData);
                    }
                }
                connection.Close();
            }
        }

        /// <summary>
        /// Updates a word from the dictionary on the database.
        /// </summary>
        /// <param name="updatedDictionary">Dictionary of strings that contain the placeholder and value for the update to happen.</param>
        public static void UpdateWord(Dictionary<string, object> updatedDictionary)
        {
            string updateQuery = $@"UPDATE tblWords
                                        SET Word = @Word,
                                            WordDefinition = @WordDefinition,
                                            WordExample = @WordExample
                                        WHERE WordId = @WordId
                                        AND LanguageId = (SELECT LanguageId FROM tbLanguages WHERE LanguageName = @LanguageName);";
            try
            {
                UpdateDatabaseEntry(updateQuery, updatedDictionary);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// Insertas a word into the dictionary on the database.
        /// </summary>
        /// <param name="updatedDictionary">Dictionary of strings that contain the placeholder and value for the insert to happen.</param>
        public static void InsertWord(Dictionary<string, object> updatedDictionary)
        {
            var query = $@"INSERT INTO tblWords (Word, LanguageId, WordDefinition, WordExample)
                                VALUES (@Word, (SELECT LanguageId FROM tbLanguages WHERE LanguageName = @LanguageName), @WordDefinition, @WordExample)";
            try
            {
                UpdateDatabaseEntry(query, updatedDictionary);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// Gets a random word based on length.
        /// </summary>
        /// <param name="wordLength">The maximum word length.</param>
        /// <param name="language">The language of the word.</param>
        /// <returns>A random word from database</returns>
        public static WordInfo GetRandomWord(int wordLength, string language)
        {
            var randomWord = new WordInfo();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var query = $"SELECT Word, WordDefinition, WordExample FROM tblWords WHERE LanguageId = (SELECT LanguageId FROM tbLanguages WHERE LanguageName = N'{language}') AND LENGTH(Word) = {wordLength} ORDER BY RANDOM() LIMIT 1";
                var command = new NpgsqlCommand(query, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var word = reader.GetString(0);
                    var definition = reader.IsDBNull(1) ? null : reader.GetString(1);
                    var example = reader.IsDBNull(2) ? null : reader.GetString(2);
                    randomWord = new WordInfo { Word = word, Definition = definition, Example = example };
                }
                reader.Close();
            }

            return randomWord;
        }

        /// <summary>
        /// Gets the letters from a word. 
        /// </summary>
        /// <param name="word">The word to be analized.</param>
        /// <returns>A list with all the letters from the word.</returns>
        public static List<string> SeparateLetters(string word)
        {
            List<string> letters = new List<string>();

            foreach (char letter in word)
            {
                letters.Add(letter.ToString());
            }

            return letters;
        }

        /// <summary>
        /// Gets all words according to the max length
        /// </summary>
        /// <param name="wordMaxLength">The max length of the words.</param>
        /// <param name="language">The language of the words.</param>
        /// <returns>A list of wordinfo that contains name, definition and example of words.</returns>
        public static List<WordInfo> GetAllWordsByLength(int wordMaxLength, string language)
        {
            var matchingWords = new List<WordInfo>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var query = $@"SELECT Word, WordDefinition, WordExample 
                          FROM tblWords 
                          WHERE LanguageId = (SELECT LanguageId FROM tbLanguages WHERE LanguageName = '{language}') 
                          AND LENGTH(Word) <= {wordMaxLength}";

                var command = new NpgsqlCommand(query, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var word = reader.GetString(0);
                    var definition = reader.IsDBNull(1) ? null : reader.GetString(1);
                    var example = reader.IsDBNull(2) ? null : reader.GetString(2);

                    matchingWords.Add(new WordInfo { Word = word, Definition = definition, Example = example });
                }
                reader.Close();
            }

            var random = new Random();
            matchingWords = matchingWords.OrderBy(x => random.Next()).ToList();

            return matchingWords.ToList();
        }

        private static bool ContainsInvalidCharacters(string word)
        {
            return word.Contains('-') || word.Contains(' ') || word.Contains('.') || ContainsDigit(word);
        }

        private static bool ContainsDigit(string word)
        {
            return word.Any(char.IsDigit) || word.Length < 3;
        }
    }
}