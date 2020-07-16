using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace TOC.Strategy
{
    public class StrategyBuilderClass
    {
        public static DataTable AddSBColumns()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(enumSBColumns.Select.ToString(), typeof(bool));
            dataTable.Columns.Add(enumSBColumns.ExpiryDate.ToString(), typeof(string));
            dataTable.Columns.Add(enumSBColumns.ContractType.ToString(), typeof(string));
            dataTable.Columns.Add(enumSBColumns.TransactionType.ToString(), typeof(string));
            dataTable.Columns.Add(enumSBColumns.StrikePrice.ToString(), typeof(int));
            dataTable.Columns.Add(enumSBColumns.Lots.ToString(), typeof(int));
            dataTable.Columns.Add(enumSBColumns.CMP.ToString(), typeof(int));
            return dataTable;
        }

        public static DataRow AddNewRows()
        {
            DataTable dataTable = AddSBColumns();
            DataRow dataRow = dataTable.NewRow();
            return dataRow;
        }

        public static void AddBlankRows(DataTable dataTable)
        {
            dataTable.Rows.Add(
                    false,                                  //Select,
                    OCHelper.DefaultExpDate("NIFTY"),       //ExpiryDate,
                    "CE",                                   //ContractType,
                    "SELL",                                 //TransactionType,
                    OCHelper.DefaultSP("NIFTY"),            //StrikePrice,
                    1,                                      //Lots,
                    0                                       //CMP
                );
        }

        public static void AddSBRows(DataTable dataSBTable)
        {
            DataTable dataTableFromFile = FileClass.ReadCsvFile("C:\\Myfiles\\StrategyBuilder.csv");
            bool IsAddRow = true;

            foreach (DataRow dataRowInput in dataSBTable.Rows)
            {
                IsAddRow = true;
                foreach (DataRow dataRowFromFile in dataTableFromFile.Rows)
                {
                    if (dataRowFromFile[enumSBColumns.ExpiryDate.ToString()].ToString().Equals(dataRowInput[enumSBColumns.ExpiryDate.ToString()].ToString()) &&
                      dataRowFromFile[enumSBColumns.ContractType.ToString()].ToString().Equals(dataRowInput[enumSBColumns.ContractType.ToString()].ToString()) &&
                      dataRowFromFile[enumSBColumns.TransactionType.ToString()].ToString().Equals(dataRowInput[enumSBColumns.TransactionType.ToString()].ToString()) &&
                      dataRowFromFile[enumSBColumns.StrikePrice.ToString()].ToString().Equals(dataRowInput[enumSBColumns.StrikePrice.ToString()].ToString()))
                    {
                        IsAddRow = false;
                    }
                }

                if (IsAddRow)
                {
                    DataRow newDataRow = dataTableFromFile.NewRow();
                    newDataRow[enumSBColumns.Select.ToString()] = false;
                    newDataRow[enumSBColumns.ExpiryDate.ToString()] = dataRowInput[enumSBColumns.ExpiryDate.ToString()].ToString();
                    newDataRow[enumSBColumns.ContractType.ToString()] = dataRowInput[enumSBColumns.ContractType.ToString()].ToString();
                    newDataRow[enumSBColumns.TransactionType.ToString()] = dataRowInput[enumSBColumns.TransactionType.ToString()].ToString();
                    newDataRow[enumSBColumns.StrikePrice.ToString()] = dataRowInput[enumSBColumns.StrikePrice.ToString()];
                    newDataRow[enumSBColumns.Lots.ToString()] = dataRowInput[enumSBColumns.Lots.ToString()];
                    newDataRow[enumSBColumns.CMP.ToString()] = 0;

                    dataTableFromFile.Rows.Add(newDataRow);
                }
            }
            FileClass.WriteDataTable(dataTableFromFile, "C:\\Myfiles\\StrategyBuilder.csv");
        }
    }

    enum enumSBColumns
    {
        Select,
        ExpiryDate,
        ContractType,
        TransactionType,
        StrikePrice,
        Lots,
        CMP
    }
}