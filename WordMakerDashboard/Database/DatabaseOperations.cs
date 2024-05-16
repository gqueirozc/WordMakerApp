using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

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
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable SelectAllFromDictionaryDatabase(string language)
        {
            var selectString = $"SELECT d.Language, w.WordId, w.Word, d.Definition, d.Example FROM tbWords w JOIN tbDefinitions d ON w.WordID = d.WordID WHERE d.Language =  N'{language}'";
            return SelectAllFromDatabase("tbWords", selectString);
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
            }
        }
    }
}
