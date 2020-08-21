using System.Data;

namespace TOC.Strategy
{
    public class StrategyBuilderClass
    {
        public static DataTable AddSBColumns()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(enumSBColumns.SelectFlg.ToString(), typeof(bool));
            dataTable.Columns.Add(enumSBColumns.ExpiryDate.ToString(), typeof(string));
            dataTable.Columns.Add(enumSBColumns.ContractType.ToString(), typeof(string));
            dataTable.Columns.Add(enumSBColumns.TransactionType.ToString(), typeof(string));
            dataTable.Columns.Add(enumSBColumns.StrikePrice.ToString(), typeof(int));
            dataTable.Columns.Add(enumSBColumns.Lots.ToString(), typeof(int));
            dataTable.Columns.Add(enumSBColumns.DataSource.ToString(), typeof(string));
            dataTable.Columns.Add(enumSBColumns.CMP.ToString(), typeof(double));
            dataTable.Columns.Add(enumSBColumns.StrategyName.ToString(), typeof(string));
            dataTable.Columns.Add(enumSBColumns.Id.ToString(), typeof(string));
            return dataTable;
        }

        public static DataRow AddNewRows()
        {
            DataTable dataTable = AddSBColumns();
            DataRow dataRow = dataTable.NewRow();
            return dataRow;
        }

        public static void AddSBRows(DataTable dataSBTable)
        {
            //DataTable dataTableFromFile = FileClass.ReadCsvFile(Constants.MYFILES_FOLDER_PATH + Constants.SB_FILE_NAME);
            DataTable dataTableFromFile = DatabaseClass.ReadStrategyBuilderData();
            bool IsAddRow = true;

            foreach (DataRow dataRowInput in dataSBTable.Rows)
            {
                IsAddRow = true;
                foreach (DataRow dataRowFromFile in dataTableFromFile.Rows)
                {
                    if (dataRowFromFile[enumSBColumns.ExpiryDate.ToString()].ToString().Equals(dataRowInput[enumSBColumns.ExpiryDate.ToString()].ToString()) &&
                      dataRowFromFile[enumSBColumns.ContractType.ToString()].ToString().Equals(dataRowInput[enumSBColumns.ContractType.ToString()].ToString()) &&
                      dataRowFromFile[enumSBColumns.TransactionType.ToString()].ToString().Equals(dataRowInput[enumSBColumns.TransactionType.ToString()].ToString()) &&
                      dataRowFromFile[enumSBColumns.CMP.ToString()].ToString().Equals(dataRowInput[enumSBColumns.CMP.ToString()].ToString()) &&
                      dataRowFromFile[enumSBColumns.StrikePrice.ToString()].ToString().Equals(dataRowInput[enumSBColumns.StrikePrice.ToString()].ToString()))
                    {
                        IsAddRow = false;
                    }
                }

                if (IsAddRow)
                {
                    DataRow newDataRow = dataTableFromFile.NewRow();
                    newDataRow[enumSBColumns.SelectFlg.ToString()] = false;
                    newDataRow[enumSBColumns.ExpiryDate.ToString()] = dataRowInput[enumSBColumns.ExpiryDate.ToString()].ToString();
                    newDataRow[enumSBColumns.ContractType.ToString()] = dataRowInput[enumSBColumns.ContractType.ToString()].ToString();
                    newDataRow[enumSBColumns.TransactionType.ToString()] = dataRowInput[enumSBColumns.TransactionType.ToString()].ToString();
                    newDataRow[enumSBColumns.StrikePrice.ToString()] = dataRowInput[enumSBColumns.StrikePrice.ToString()];
                    newDataRow[enumSBColumns.Lots.ToString()] = dataRowInput[enumSBColumns.Lots.ToString()];
                    newDataRow[enumSBColumns.DataSource.ToString()] = dataRowInput[enumSBColumns.DataSource.ToString()];

                    if (dataRowInput[enumSBColumns.DataSource.ToString()].Equals(enumDataSource.Positions.ToString()))
                    {
                        newDataRow[enumSBColumns.CMP.ToString()] = dataRowInput[enumSBColumns.CMP.ToString()];
                        newDataRow[enumSBColumns.StrategyName.ToString()] = "Positions";
                    }
                    else if (dataRowInput[enumSBColumns.DataSource.ToString()].Equals(enumDataSource.NiftyWatchlist.ToString()))
                    {
                        newDataRow[enumSBColumns.CMP.ToString()] = dataRowInput[enumSBColumns.CMP.ToString()];
                        newDataRow[enumSBColumns.StrategyName.ToString()] = "NiftyWatchlist";
                    }
                    else
                    {
                        newDataRow[enumSBColumns.CMP.ToString()] = 0;
                        newDataRow[enumSBColumns.StrategyName.ToString()] = "Default";
                    }

                    newDataRow[enumSBColumns.Id.ToString()] = dataRowInput[enumSBColumns.Id.ToString()];

                    dataTableFromFile.Rows.Add(newDataRow);
                }
            }
            //FileClass.WriteDataTable(dataTableFromFile, Constants.MYFILES_FOLDER_PATH + Constants.SB_FILE_NAME);
            DatabaseClass.SaveStrategyBuilder(dataTableFromFile);
        }
    }

    enum enumSBColumns
    {
        SelectFlg,
        ExpiryDate,
        ContractType,
        TransactionType,
        StrikePrice,
        Lots,
        DataSource,
        CMP,
        StrategyName,
        UserId,
        Id
    }
}