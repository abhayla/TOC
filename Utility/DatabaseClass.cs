using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using TOC.Strategy;

namespace TOC
{
    public class DatabaseClass
    {
        public static DataTable ReadData(string filePath)
        {
            DataTable dataTable = new DataTable();
            string[] paths = filePath.Split('\\');

            if (paths[paths.Length - 1].Equals(Constants.SB_FILE_NAME))
            {
                dataTable = StrategyBuilderClass.AddSBColumns();
            }
            if (paths[paths.Length - 1].Equals(Constants.PT_FILE_NAME))
            {
                dataTable = ReadPositionsData();
            }
            if (paths[paths.Length - 1].Equals(Constants.BO_FILE_NAME))
            {
                dataTable = BasketOrderClass.AddBOColumns();
            }

            return dataTable;
        }

        public static DataTable ReadOptionsChain(string ocType)
        {
            DataTable dataTable = new DataTable();
            MySqlConnection sqlConnection = new MySqlConnection(Constants.CONNECTION_STRING);
            sqlConnection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("Select IndexType from OptionsChain WHERE OCType = '" + ocType + "'", sqlConnection);
            mySqlDataAdapter.Fill(dataTable);
            sqlConnection.Close();
            return dataTable;
        }

        public static void UpdateOptionsChain(string ocType, string indexType)
        {
            StringBuilder sbSqlQuery = new StringBuilder();
            List<string> sqlQueryList = new List<string>();

            using (MySqlConnection sqlConnection = new MySqlConnection(Constants.CONNECTION_STRING))
            {
                sqlQueryList.Add(string.Format("UPDATE OptionsChain SET IndexType = '{0}' WHERE OCType = '{1}'", indexType, ocType));

                sbSqlQuery.Append(string.Join(";", sqlQueryList));
                sbSqlQuery.Append(";");
                sqlConnection.Open();

                using (MySqlCommand myCmd = new MySqlCommand(sbSqlQuery.ToString(), sqlConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }
            }
        }

        public static DataTable ReadPositionsData()
        {
            DataTable dataTable = new DataTable();
            DataTable resultTable = new DataTable();

            MySqlConnection sqlConnection = new MySqlConnection(Constants.CONNECTION_STRING);
            sqlConnection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("Select * from Positions", sqlConnection);
            mySqlDataAdapter.Fill(dataTable);
            sqlConnection.Close();
            resultTable = PositionsClass.ConvertToRegularPTTable(dataTable);
            return resultTable;
        }

        public static void SavePositions(DataTable sourceTable)
        {
            DataTable dataTable = new DataTable();
            MySqlConnection sqlConnection = new MySqlConnection(Constants.CONNECTION_STRING);
            sqlConnection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("Select * from Positions", sqlConnection);
            mySqlDataAdapter.Fill(dataTable);
            sqlConnection.Close();

            if (dataTable.Rows.Count > sourceTable.Rows.Count)
            {
                var idsNotInB = dataTable.AsEnumerable().Select(r => r.Field<string>(enumPTColumns.Id.ToString())).Except(sourceTable.AsEnumerable().Select(r => r.Field<string>(enumPTColumns.Id.ToString())));
                DataTable toDeleteDataTable = (from row in dataTable.AsEnumerable() join id in idsNotInB on row.Field<string>(enumPTColumns.Id.ToString()) equals id select row).CopyToDataTable();

                if (toDeleteDataTable.Rows.Count > 0)
                    DeletePositions(toDeleteDataTable);
            }

            if (sourceTable.Rows.Count > 0)
                UpsertPositions(sourceTable);
        }

        public static void DeletePositions(DataTable sourceTable)
        {
            StringBuilder sbSqlQuery = new StringBuilder();
            List<string> sqlQueryList = new List<string>();

            using (MySqlConnection sqlConnection = new MySqlConnection(Constants.CONNECTION_STRING))
            {
                foreach (DataRow dataRow in sourceTable.Rows)
                {
                    sqlQueryList.Add(string.Format("DELETE FROM Positions WHERE Id = '{0}'",
                    dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Id.ToString())]));
                }

                sbSqlQuery.Append(string.Join(";", sqlQueryList));
                sbSqlQuery.Append(";");
                sqlConnection.Open();

                using (MySqlCommand myCmd = new MySqlCommand(sbSqlQuery.ToString(), sqlConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpsertPositions(DataTable sourceTable)
        {
            StringBuilder sbSqlQuery = new StringBuilder();
            List<string> sqlQueryList = new List<string>();

            using (MySqlConnection sqlConnection = new MySqlConnection(Constants.CONNECTION_STRING))
            {
                foreach (DataRow dataRow in sourceTable.Rows)
                {
                    sqlQueryList.Add(string.Format("INSERT into Positions ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}) Values ({17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33}) on duplicate key update {34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49}",
                    enumPTColumns.Id.ToString(),
                    enumPTColumns.SelectFlg.ToString(),
                    enumPTColumns.OCType.ToString(),
                    enumPTColumns.ExpiryDate.ToString(),
                    enumPTColumns.ContractType.ToString(),
                    enumPTColumns.TransactionType.ToString(),
                    enumPTColumns.StrikePrice.ToString(),
                    enumPTColumns.Lots.ToString(),
                    enumPTColumns.Recommendation.ToString(),
                    enumPTColumns.Strategy.ToString(),
                    enumPTColumns.Profile.ToString(),
                    enumPTColumns.Position.ToString(),
                    enumPTColumns.UserId.ToString(),
                    enumPTColumns.EntryPrice.ToString(),
                    enumPTColumns.ExitPrice.ToString(),
                    enumPTColumns.EntryDate.ToString(),
                    enumPTColumns.ExitDate.ToString(),

                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Id.ToString())] + "'",
                    dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.SelectFlg.ToString())],
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.OCType.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ExpiryDate.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ContractType.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.TransactionType.ToString())] + "'",
                    dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.StrikePrice.ToString())],
                    dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Lots.ToString())],
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Recommendation.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Strategy.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Profile.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Position.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.UserId.ToString())] + "'",
                    dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.EntryPrice.ToString())],
                    dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ExitPrice.ToString())],
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.EntryDate.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ExitDate.ToString())] + "'",

                    enumPTColumns.SelectFlg.ToString() + " = " + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.SelectFlg.ToString())],
                    enumPTColumns.OCType.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.OCType.ToString())] + "'",
                    enumPTColumns.ExpiryDate.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ExpiryDate.ToString())] + "'",
                    enumPTColumns.ContractType.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ContractType.ToString())] + "'",
                    enumPTColumns.TransactionType.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.TransactionType.ToString())] + "'",
                    enumPTColumns.StrikePrice.ToString() + " = " + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.StrikePrice.ToString())],
                    enumPTColumns.Lots.ToString() + " = " + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Lots.ToString())],
                    enumPTColumns.Recommendation.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Recommendation.ToString())] + "'",
                    enumPTColumns.Strategy.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Strategy.ToString())] + "'",
                    enumPTColumns.Profile.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Profile.ToString())] + "'",
                    enumPTColumns.Position.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Position.ToString())] + "'",
                    enumPTColumns.UserId.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.UserId.ToString())] + "'",
                    enumPTColumns.EntryPrice.ToString() + " = " + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.EntryPrice.ToString())],
                    enumPTColumns.ExitPrice.ToString() + " = " + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ExitPrice.ToString())],
                    enumPTColumns.EntryDate.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.EntryDate.ToString())] + "'",
                    enumPTColumns.ExitDate.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ExitDate.ToString())] + "'"));
                }

                sbSqlQuery.Append(string.Join(";", sqlQueryList));
                sbSqlQuery.Append(";");
                sqlConnection.Open();

                using (MySqlCommand myCmd = new MySqlCommand(sbSqlQuery.ToString(), sqlConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }
            }
        }

        public static DataTable ReadStrategyBuilderData()
        {
            DataTable dataTable = new DataTable();
            //DataTable resultTable = new DataTable();

            MySqlConnection sqlConnection = new MySqlConnection(Constants.CONNECTION_STRING);
            sqlConnection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("Select * from StrategyBuilder", sqlConnection);
            mySqlDataAdapter.Fill(dataTable);
            sqlConnection.Close();
            //resultTable = PositionsClass.ConvertToRegularPTTable(dataTable);
            return dataTable;
        }

        public static void SaveStrategyBuilder(DataTable sourceTable)
        {
            DataTable dataTable = new DataTable();
            MySqlConnection sqlConnection = new MySqlConnection(Constants.CONNECTION_STRING);
            sqlConnection.Open();
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter("Select * from StrategyBuilder", sqlConnection);
            mySqlDataAdapter.Fill(dataTable);
            sqlConnection.Close();

            if (dataTable.Rows.Count > sourceTable.Rows.Count)
            {
                var idsNotInB = dataTable.AsEnumerable().Select(r => r.Field<string>(enumPTColumns.Id.ToString())).Except(sourceTable.AsEnumerable().Select(r => r.Field<string>(enumPTColumns.Id.ToString())));
                DataTable toDeleteDataTable = (from row in dataTable.AsEnumerable() join id in idsNotInB on row.Field<string>(enumPTColumns.Id.ToString()) equals id select row).CopyToDataTable();

                if (toDeleteDataTable.Rows.Count > 0)
                    DeleteStrategyBuilder(toDeleteDataTable);
            }

            if (sourceTable.Rows.Count > 0)
                UpsertStrategyBuilder(sourceTable);
        }

        public static void DeleteStrategyBuilder(DataTable sourceTable)
        {
            StringBuilder sbSqlQuery = new StringBuilder();
            List<string> sqlQueryList = new List<string>();

            using (MySqlConnection sqlConnection = new MySqlConnection(Constants.CONNECTION_STRING))
            {
                foreach (DataRow dataRow in sourceTable.Rows)
                {
                    sqlQueryList.Add(string.Format("DELETE FROM StrategyBuilder WHERE Id = '{0}'",
                    dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.Id.ToString())]));
                }

                sbSqlQuery.Append(string.Join(";", sqlQueryList));
                sbSqlQuery.Append(";");
                sqlConnection.Open();

                using (MySqlCommand myCmd = new MySqlCommand(sbSqlQuery.ToString(), sqlConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpsertStrategyBuilder(DataTable sourceTable)
        {
            StringBuilder sbSqlQuery = new StringBuilder();
            List<string> sqlQueryList = new List<string>();

            using (MySqlConnection sqlConnection = new MySqlConnection(Constants.CONNECTION_STRING))
            {
                foreach (DataRow dataRow in sourceTable.Rows)
                {
                    sqlQueryList.Add(string.Format("INSERT into StrategyBuilder ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) Values ({11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21}) on duplicate key update {22},{23},{24},{25},{26},{27},{28},{29},{30},{31}",
                    enumSBColumns.Id.ToString(),
                    enumSBColumns.SelectFlg.ToString(),
                    enumSBColumns.ExpiryDate.ToString(),
                    enumSBColumns.ContractType.ToString(),
                    enumSBColumns.TransactionType.ToString(),
                    enumSBColumns.StrikePrice.ToString(),
                    enumSBColumns.Lots.ToString(),
                    enumSBColumns.DataSource.ToString(),
                    enumSBColumns.CMP.ToString(),
                    enumSBColumns.StrategyName.ToString(),
                    enumSBColumns.UserId.ToString(),

                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.Id.ToString())] + "'",
                    dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.SelectFlg.ToString())],
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.ExpiryDate.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.TransactionType.ToString())] + "'",
                    dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())],
                    dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.Lots.ToString())],
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.DataSource.ToString())] + "'",
                    dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())],
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.StrategyName.ToString())] + "'",
                    "'" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.UserId.ToString())] + "'",

                    enumSBColumns.SelectFlg.ToString() + " = " + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.SelectFlg.ToString())],
                    enumSBColumns.ExpiryDate.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.ExpiryDate.ToString())] + "'",
                    enumSBColumns.ContractType.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())] + "'",
                    enumSBColumns.TransactionType.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.TransactionType.ToString())] + "'",
                    enumSBColumns.StrikePrice.ToString() + " = " + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())],
                    enumSBColumns.Lots.ToString() + " = " + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.Lots.ToString())],
                    enumSBColumns.DataSource.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.DataSource.ToString())] + "'",
                    enumSBColumns.CMP.ToString() + " = " + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())],
                    enumSBColumns.StrategyName.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.StrategyName.ToString())] + "'",
                    enumSBColumns.UserId.ToString() + " = '" + dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.UserId.ToString())] + "'"));
                }

                sbSqlQuery.Append(string.Join(";", sqlQueryList));
                sbSqlQuery.Append(";");
                sqlConnection.Open();

                using (MySqlCommand myCmd = new MySqlCommand(sbSqlQuery.ToString(), sqlConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }
            }
        }
    }
}