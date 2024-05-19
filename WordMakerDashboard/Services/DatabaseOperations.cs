using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Npgsql;
using WordMakerDashboard.Models;

namespace WordMakerDashboard.Database
{
    /// <summary>
    /// Contains all operations related to the POSTGRE ElephantSQL database used to storage all data.
    /// </summary>
    public class DatabaseOperations
    {
        public static string connectionString = "Host=isabelle.db.elephantsql.com;Username=zrmfzafr;Password=2F0bg6YYX51w_NRMFvO8Lhw3PrMBpW0s;Database=zrmfzafr";

        /// <summary>
        /// Sends a select all query to the database.
        /// </summary>
        /// <param name="tableName">Name of the table to be selected</param>
        /// <param name="strSelect">[optional] - Full query string for the select. Will be used instead of the standard.</param>
        /// <returns>Returns a DataTable will all the information from the query.</returns>
        public DataTable SelectAllFromDatabase(string tableName, string strSelect = "")
        {
            if (strSelect == "") strSelect = $"SELECT * FROM {tableName};";
            var dataTable = new DataTable();
            using (NpgsqlConnection oConnection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand oCommand = new NpgsqlCommand(strSelect, oConnection))
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
        public void DeleteFromDatabaseTable(string tableName, string idFieldName, int deletedId)
        {
            var strDELETE = $"DELETE FROM {tableName} WHERE {idFieldName} = @{idFieldName}";
            using (NpgsqlConnection oConnection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand oCommand = new NpgsqlCommand(strDELETE, oConnection))
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
        public void UpdateDatabaseEntry(string updateQuery, Dictionary<string, string> updateValues)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection))
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
        public void ExecuteQuery(string query)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand updateCommand = new NpgsqlCommand(query, connection))
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
        public bool LanguageExists(string languageName)
        {
            bool exists = false;
            string query = $"SELECT COUNT(*) FROM tbLanguages WHERE LanguageName = '{languageName}'";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
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
        public bool WordExists(string word, string languageName, out int wordId)
        {
            wordId = -1;
            bool exists = false;
            string query = $"SELECT COUNT(*) " +
                           $"FROM tblWords w " +
                           $"INNER JOIN tbLanguages l ON w.LanguageId = l.LanguageId " +
                           $"WHERE w.Word = '{word}' AND l.LanguageName = '{languageName}'";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
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

                    using (NpgsqlCommand command = new NpgsqlCommand(queryWordId, connection))
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
        public void PopulateDatabaseWithNewDictionary(Dictionary<string, Word> wordDictionary, string languageName, BackgroundWorker bgWorker)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var wordsToInsertOrUpdate = wordDictionary.ToList();

                wordsToInsertOrUpdate.Sort((x, y) => string.Compare(x.Key, y.Key, StringComparison.Ordinal));
                var entriesToInsert = wordsToInsertOrUpdate.Where(entry => !ContainsInvalidCharacters(entry.Key));

                var currentIteration = 0;
                foreach (var entry in entriesToInsert)
                {
                    var word = entry.Key;
                    var wordData = entry.Value;

                    bool wordExists = WordExists(word, languageName, out var wordId);

                    var newData = new Dictionary<string, string>
                    {
                        { "Word", word },
                        { "WordDefinition", wordData.Definition },
                        { "WordExample", wordData.Example },
                        { "LanguageName", languageName },
                        { "WordId", wordId.ToString()}
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
        public void UpdateWord(Dictionary<string, string> updatedDictionary)
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
                MessageBox.Show("An error occured while trying to alter the data: " + ex.Message);
                return;
            }
        }

        /// <summary>
        /// Insertas a word into the dictionary on the database.
        /// </summary>
        /// <param name="updatedDictionary">Dictionary of strings that contain the placeholder and value for the insert to happen.</param>
        public void InsertWord(Dictionary<string, string> updatedDictionary)
        {
            var query = $@"INSERT INTO tblWords (Word, LanguageId, WordDefinition, WordExample)
                                VALUES (@Word, (SELECT LanguageId FROM tbLanguages WHERE LanguageName = @LanguageName), @WordDefinition, @WordExample)";
            try
            {
                UpdateDatabaseEntry(query, updatedDictionary);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while trying to alter the data: " + ex.Message);
                return;
            }
        }

        private static bool ContainsInvalidCharacters(string word)
        {
            return word.Contains('-') || word.Contains(' ') || word.Contains('.') || ContainsDigit(word);
        }

        private static bool ContainsDigit(string word)
        {
            return word.Any(char.IsDigit) || word.Length < 3;
        }

        private static void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}