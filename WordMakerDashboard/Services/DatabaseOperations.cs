using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System;

namespace WordMakerDashboard.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseOperations : DatabaseConnect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="strSelect"></param>
        /// <returns></returns>
        public DataTable SelectAllFromDatabase(string tableName, string strSelect = "")
        {
            if (strSelect == "") strSelect = $"SELECT * FROM {tableName};";
            var dataTable = new DataTable();
            using (SqlConnection oConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand oCommand = new SqlCommand(strSelect, oConnection))
                {
                    var dataAdapter = new SqlDataAdapter(oCommand);

                    oConnection.Open();
                    dataAdapter.Fill(dataTable);
                    oConnection.Close();
                }
            }
            return dataTable;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="idFieldName"></param>
        /// <param name="deletedId"></param>
        public void DeleteFromDatabaseTable(string tableName, string idFieldName, int deletedId)
        {
            var strDELETE = $"DELETE FROM {tableName} WHERE {idFieldName} = @{idFieldName}";
            using (SqlConnection oConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand oCommand = new SqlCommand(strDELETE, oConnection))
                {
                    oCommand.Parameters.AddWithValue($"@{idFieldName}", deletedId);
                    oConnection.Open();
                    oCommand.ExecuteNonQuery();
                    oConnection.Close();
                }
            }
        }

   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateQuery"></param>
        /// <param name="updateValues"></param>
        public void UpdateDatabaseEntry(string updateQuery, Dictionary<string, string> updateValues)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                {
                    foreach(var item in updateValues)
                    {
                        updateCommand.Parameters.AddWithValue($"@{item.Key}", item.Value);
                    }
                    updateCommand.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        public void ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand updateCommand = new SqlCommand(query, connection))
                {
                    updateCommand.ExecuteNonQuery();
                }
                connection.Close();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageName"></param>
        /// <returns></returns>
        public bool LanguageExists(string languageName)
        {
            bool exists = false;
            string query = $"SELECT COUNT(*) FROM tbLanguages WHERE LanguageName = '{languageName}'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    exists = (count > 0);
                }
                connection.Close();
            }

            return exists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="languageName"></param>
        /// <returns></returns>
        public bool WordExists(string word, string languageName, out int wordId)
        {
            wordId = -1;
            bool exists = false;
            string query = $"SELECT COUNT(*) " +
                           $"FROM tblWords w " +
                           $"INNER JOIN tbLanguages l ON w.LanguageId = l.LanguageId " +
                           $"WHERE w.Word = '{word}' AND l.LanguageName = '{languageName}'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int count = (int)command.ExecuteScalar();
                    exists = (count > 0);
                }

                if (exists)
                {
                    string queryWordId = $"SELECT WordId " +
                                         $"FROM tblWords w " +
                                         $"INNER JOIN tbLanguages l ON w.LanguageId = l.LanguageId " +
                                         $"WHERE w.Word = '{word}' AND l.LanguageName = '{languageName}'";

                    using (SqlCommand command = new SqlCommand(queryWordId, connection))
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
    }
}
