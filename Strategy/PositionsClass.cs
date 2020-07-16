using System;
using System.Data;
using System.Linq;
using System.Web;

namespace TOC.Strategy
{
    public class PositionsClass
    {
        public static DataTable AddPTColumns()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(enumPTColumns.Select.ToString(), typeof(bool));
            dataTable.Columns.Add(enumPTColumns.OCType.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.ExpiryDate.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.ContractType.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.TransactionType.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.StrikePrice.ToString(), typeof(int));
            dataTable.Columns.Add(enumPTColumns.Lots.ToString(), typeof(int));
            dataTable.Columns.Add(enumPTColumns.EntryPrice.ToString(), typeof(double));
            dataTable.Columns.Add(enumPTColumns.ExitPrice.ToString(), typeof(double));
            dataTable.Columns.Add(enumPTColumns.CMP.ToString(), typeof(double));
            dataTable.Columns.Add(enumPTColumns.UnRealisedPL.ToString(), typeof(double));
            dataTable.Columns.Add(enumPTColumns.ChgPer.ToString(), typeof(double));
            dataTable.Columns.Add(enumPTColumns.RealisedPL.ToString(), typeof(double));
            dataTable.Columns.Add(enumPTColumns.MaxProfit.ToString(), typeof(double));
            dataTable.Columns.Add(enumPTColumns.Recommendation.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.Strategy.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.Profile.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.Position.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.Id.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.DaysToExpiry.ToString(), typeof(int));
            dataTable.Columns.Add(enumPTColumns.DaysHeld.ToString(), typeof(int));
            dataTable.Columns.Add(enumPTColumns.EntryDate.ToString(), typeof(string));
            return dataTable;
        }

        public static void AddBlankRows(DataTable dataTable)
        {
            dataTable.Rows.Add(
                    false,                                  // Select,
                    "NIFTY",                                //OCType,
                    OCHelper.DefaultExpDate("NIFTY"),       //ExpiryDate,
                    "CE",                                   //ContractType,
                    "SELL",                                 //TransactionType,
                    OCHelper.DefaultSP("NIFTY"),            //StrikePrice,
                    1,                                      //Lots,
                    0,                                      //EntryPrice,
                    0,                                      //ExitPrice,
                    0,                                      //CMP,
                    0,                                      //UnRealisedPL,
                    0,                                      //ChgPer,
                    0,                                      //RealisedPL,
                    0,                                      //MaxProfit,
                    string.Empty,                           //Recommendation,
                    "Butterfly",                            //Strategy,
                    "Abhay",                                //Profile,
                    "Open",                                 //Position,
                    Guid.NewGuid(),                         //Id,
                    0,                                      //DaysToExpiry,
                    0,                                      //DaysHeld,
                    DateTime.Now.Date                       //EntryDate
                );
        }

        public static void AddPTRows(string oCType, string expiryDate, string contractType, string transactionType, int strikePrice, string strategy, string profile)
        {
            DataTable dataTable = FileClass.ReadCsvFile("C:\\Myfiles\\PositionsTracker.csv");
            bool IsAddRow = true;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow[enumPTColumns.OCType.ToString()].ToString().Equals(oCType) && 
                    dataRow[enumPTColumns.ExpiryDate.ToString()].ToString().Equals(expiryDate) &&
                   dataRow[enumPTColumns.ContractType.ToString()].ToString().Equals(contractType) &&
                   dataRow[enumPTColumns.TransactionType.ToString()].ToString().Equals(transactionType) &&
                   dataRow[enumPTColumns.StrikePrice.ToString()].ToString().Equals(strikePrice.ToString()))
                {
                    IsAddRow = false;
                }
            }

            if (IsAddRow)
            {
                DataRow newDataRow = dataTable.NewRow();
                newDataRow[enumPTColumns.Select.ToString()] = false;
                newDataRow[enumPTColumns.OCType.ToString()] = oCType;
                newDataRow[enumPTColumns.ExpiryDate.ToString()] = expiryDate;
                newDataRow[enumPTColumns.ContractType.ToString()] = contractType;
                newDataRow[enumPTColumns.TransactionType.ToString()] = transactionType;
                newDataRow[enumPTColumns.StrikePrice.ToString()] = strikePrice;
                newDataRow[enumPTColumns.Lots.ToString()] = 1;
                newDataRow[enumPTColumns.EntryPrice.ToString()] = 0;
                newDataRow[enumPTColumns.ExitPrice.ToString()] = 0;
                newDataRow[enumPTColumns.CMP.ToString()] = 0;
                newDataRow[enumPTColumns.UnRealisedPL.ToString()] = 0;
                newDataRow[enumPTColumns.ChgPer.ToString()] = 0;
                newDataRow[enumPTColumns.RealisedPL.ToString()] = 0;
                newDataRow[enumPTColumns.MaxProfit.ToString()] = 0;
                newDataRow[enumPTColumns.Recommendation.ToString()] = string.Empty;
                newDataRow[enumPTColumns.Strategy.ToString()] = strategy;
                newDataRow[enumPTColumns.Profile.ToString()] = profile;
                newDataRow[enumPTColumns.Position.ToString()] = "Open";
                newDataRow[enumPTColumns.Id.ToString()] = Guid.NewGuid();
                newDataRow[enumPTColumns.DaysToExpiry.ToString()] = 0;
                newDataRow[enumPTColumns.DaysHeld.ToString()] = 0;
                newDataRow[enumPTColumns.EntryDate.ToString()] = DateTime.Now.Date;

                dataTable.Rows.Add(newDataRow);

                FileClass.WriteDataTable(dataTable, "C:\\Myfiles\\PositionsTracker.csv");
            }
        }
    }

    enum enumPTColumns
    {
        Select,
        OCType,
        ExpiryDate,
        ContractType,
        TransactionType,
        StrikePrice,
        Lots,
        EntryPrice,
        ExitPrice,
        CMP,
        UnRealisedPL,
        ChgPer,
        RealisedPL,
        MaxProfit,
        Recommendation,
        Strategy,
        Profile,
        Position,
        Id,
        DaysToExpiry,
        DaysHeld,
        EntryDate
    }

}