using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;

namespace WordMakerDashboard.Database
{
    public class DatabaseConnect
    {
        public static string connectionString = "Server=DESKTOP-EJCKEDA\\MSSQLSERVER01;Database=WordMaker;Trusted_Connection=True;\r\n";

        public static void PopulateDatabaseWithNewDictionary(Dictionary<string, string> rawDictionary, BackgroundWorker bgWorker)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var dictionaryListed = rawDictionary.ToList();
                //Sort A-Z
                dictionaryListed.Sort((x, y) => string.Compare(x.Key, y.Key, StringComparison.Ordinal));
            
                var entriesToInsert = dictionaryListed.Where(entry => !ContainsInvalidCharacters(entry.Key));

                var currentIteration = 0;
                foreach (var entry in entriesToInsert)
                {
                    string word = entry.Key;
                    string definition = entry.Value;
                    currentIteration += 1;
                    bgWorker.ReportProgress(currentIteration * 100 / entriesToInsert.Count());

                    InsertWordDefinition(connection, word, definition);
                }

                connection.Close();
            }
        }

        private static void InsertWordDefinition(SqlConnection connection, string word, string definition)
        {
            // Check if the word already exists in tbWords
            string selectWordQuery = "SELECT WordID FROM tbWords WHERE Word = @Word";
            int wordId;

            using (SqlCommand selectCommand = new SqlCommand(selectWordQuery, connection))
            {
                selectCommand.Parameters.AddWithValue("@Word", word);
                object result = selectCommand.ExecuteScalar();
                wordId = (result == null || result == DBNull.Value) ? -1 : Convert.ToInt32(result);
            }

            if (wordId != -1)
            {
                // Word exists, update its definition in tbDefinitions
                string updateDefinitionQuery = "UPDATE tbDefinitions " +
                                               "SET Definition = @Definition " +
                                               "WHERE WordID = @WordID";

                using (SqlCommand updateCommand = new SqlCommand(updateDefinitionQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@WordID", wordId);
                    updateCommand.Parameters.AddWithValue("@Definition", definition);
                    updateCommand.ExecuteNonQuery();
                }
            }
            else
            {
                // Word doesn't exist, insert it into tbWords and tbDefinitions
                string insertQuery = "INSERT INTO tbWords (Word) VALUES (@Word); " +
                                     "DECLARE @WordID INT = SCOPE_IDENTITY(); " +
                                     "INSERT INTO tbDefinitions (WordID, Language, Definition, Example) " +
                                     "VALUES (@WordID, 'English', NULL, NULL)";

                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@Word", word);
                    insertCommand.Parameters.AddWithValue("@Definition", definition);
                    insertCommand.ExecuteNonQuery();
                }
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
