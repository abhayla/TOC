﻿using System;
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
            dataTable.Columns.Add(enumPTColumns.Id.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.SelectFlg.ToString(), typeof(bool));
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
            dataTable.Columns.Add(enumPTColumns.DaysToExpiry.ToString(), typeof(int));
            dataTable.Columns.Add(enumPTColumns.DaysHeld.ToString(), typeof(int));
            dataTable.Columns.Add(enumPTColumns.UserId.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.EntryDate.ToString(), typeof(string));
            dataTable.Columns.Add(enumPTColumns.ExitDate.ToString(), typeof(string));
            return dataTable;
        }

        public static void AddBlankRows(DataTable dataTable)
        {
            dataTable.Rows.Add(
                    Guid.NewGuid(),                                             //Id,
                    false,                                                      // Select,
                    enumOCType.NIFTY.ToString(),                                //OCType,
                    OCHelper.DefaultExpDate(enumOCType.NIFTY.ToString()),       //ExpiryDate,
                    "CE",                                                       //ContractType,
                    "SELL",                                                     //TransactionType,
                    OCHelper.DefaultSP(enumOCType.NIFTY.ToString()),            //StrikePrice,
                    1,                                                          //Lots,
                    0,                                                          //EntryPrice,
                    0,                                                          //ExitPrice,
                    0,                                                          //CMP,
                    0,                                                          //UnRealisedPL,
                    0,                                                          //ChgPer,
                    0,                                                          //RealisedPL,
                    0,                                                          //MaxProfit,
                    string.Empty,                                               //Recommendation,
                    "Butterfly",                                                //Strategy,
                    "Abhay",                                                    //Profile,
                    "Open",                                                     //Position,
                    0,                                                          //DaysToExpiry,
                    0,                                                          //DaysHeld,
                    "DA1707",                                                   //UserId,
                    DateTime.Now.ToString("dd-MMM-yyyy"),                       //EntryDate
                    null                                                        //ExitDate
                );
        }

        public static DataTable ConvertToRegularPTTable(DataTable sourceDataTable)
        {
            DataTable resultTable = PositionsClass.AddPTColumns();
            foreach (DataRow dataRowInput in sourceDataTable.Rows)
            {
                DataRow newDataRow = resultTable.NewRow();
                newDataRow[enumPTColumns.Id.ToString()] = dataRowInput[enumPTColumns.Id.ToString()].ToString();
                newDataRow[enumPTColumns.SelectFlg.ToString()] = false;
                newDataRow[enumPTColumns.OCType.ToString()] = dataRowInput[enumPTColumns.OCType.ToString()].ToString();
                newDataRow[enumPTColumns.ExpiryDate.ToString()] = dataRowInput[enumPTColumns.ExpiryDate.ToString()].ToString();
                newDataRow[enumPTColumns.ContractType.ToString()] = dataRowInput[enumPTColumns.ContractType.ToString()].ToString();
                newDataRow[enumPTColumns.TransactionType.ToString()] = dataRowInput[enumPTColumns.TransactionType.ToString()].ToString();
                newDataRow[enumPTColumns.StrikePrice.ToString()] = dataRowInput[enumPTColumns.StrikePrice.ToString()].ToString();
                newDataRow[enumPTColumns.Lots.ToString()] = dataRowInput[enumPTColumns.Lots.ToString()].ToString();
                newDataRow[enumPTColumns.EntryPrice.ToString()] = dataRowInput[enumPTColumns.EntryPrice.ToString()].ToString();
                newDataRow[enumPTColumns.ExitPrice.ToString()] = dataRowInput[enumPTColumns.ExitPrice.ToString()].ToString();
                newDataRow[enumPTColumns.CMP.ToString()] = 0;
                newDataRow[enumPTColumns.UnRealisedPL.ToString()] = 0;
                newDataRow[enumPTColumns.ChgPer.ToString()] = 0;
                newDataRow[enumPTColumns.RealisedPL.ToString()] = 0;
                newDataRow[enumPTColumns.MaxProfit.ToString()] = 0;
                newDataRow[enumPTColumns.Recommendation.ToString()] = dataRowInput[enumPTColumns.Recommendation.ToString()].ToString();
                newDataRow[enumPTColumns.Strategy.ToString()] = dataRowInput[enumPTColumns.Strategy.ToString()].ToString();
                newDataRow[enumPTColumns.Profile.ToString()] = dataRowInput[enumPTColumns.Profile.ToString()].ToString();
                newDataRow[enumPTColumns.Position.ToString()] = dataRowInput[enumPTColumns.Position.ToString()].ToString();
                newDataRow[enumPTColumns.DaysToExpiry.ToString()] = 0;
                newDataRow[enumPTColumns.DaysHeld.ToString()] = 0;
                newDataRow[enumPTColumns.UserId.ToString()] = dataRowInput[enumPTColumns.UserId.ToString()].ToString();
                newDataRow[enumPTColumns.EntryDate.ToString()] = dataRowInput[enumPTColumns.EntryDate.ToString()].ToString();
                newDataRow[enumPTColumns.ExitDate.ToString()] = dataRowInput[enumPTColumns.ExitDate.ToString()].ToString();
                resultTable.Rows.Add(newDataRow);
            }

            return resultTable;
        }

        public static void AddPTRows(DataTable dataPTTable)
        {
            DataTable dataTable = FileClass.ReadCsvFile(Constants.MYFILES_FOLDER_PATH + Constants.PT_FILE_NAME);
            bool IsAddRow = true;

            foreach (DataRow dataRowInput in dataPTTable.Rows)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (dataRow[enumPTColumns.OCType.ToString()].ToString().Equals(dataRowInput[enumPTColumns.OCType.ToString()].ToString()) &&
                        dataRow[enumPTColumns.ExpiryDate.ToString()].ToString().Equals(dataRowInput[enumPTColumns.ExpiryDate.ToString()].ToString()) &&
                        dataRow[enumPTColumns.ContractType.ToString()].ToString().Equals(dataRowInput[enumPTColumns.ContractType.ToString()].ToString()) &&
                        dataRow[enumPTColumns.TransactionType.ToString()].ToString().Equals(dataRowInput[enumPTColumns.TransactionType.ToString()].ToString()) &&
                        dataRow[enumPTColumns.CMP.ToString()].ToString().Equals(dataRowInput[enumPTColumns.CMP.ToString()].ToString()) &&
                        dataRow[enumPTColumns.StrikePrice.ToString()].ToString().Equals(dataRowInput[enumPTColumns.StrikePrice.ToString()].ToString()))
                    {
                        IsAddRow = false;
                    }
                }

                if (IsAddRow)
                {
                    DataRow newDataRow = dataTable.NewRow();
                    newDataRow[enumPTColumns.SelectFlg.ToString()] = false;
                    newDataRow[enumPTColumns.OCType.ToString()] = dataRowInput[enumPTColumns.OCType.ToString()].ToString();
                    newDataRow[enumPTColumns.ExpiryDate.ToString()] = dataRowInput[enumPTColumns.ExpiryDate.ToString()].ToString();
                    newDataRow[enumPTColumns.ContractType.ToString()] = dataRowInput[enumPTColumns.ContractType.ToString()].ToString();
                    newDataRow[enumPTColumns.TransactionType.ToString()] = dataRowInput[enumPTColumns.TransactionType.ToString()].ToString();
                    newDataRow[enumPTColumns.StrikePrice.ToString()] = dataRowInput[enumPTColumns.StrikePrice.ToString()].ToString();
                    newDataRow[enumPTColumns.Lots.ToString()] = 1;
                    newDataRow[enumPTColumns.EntryPrice.ToString()] = 0;
                    newDataRow[enumPTColumns.ExitPrice.ToString()] = 0;
                    newDataRow[enumPTColumns.CMP.ToString()] = 0;
                    newDataRow[enumPTColumns.UnRealisedPL.ToString()] = 0;
                    newDataRow[enumPTColumns.ChgPer.ToString()] = 0;
                    newDataRow[enumPTColumns.RealisedPL.ToString()] = 0;
                    newDataRow[enumPTColumns.MaxProfit.ToString()] = 0;
                    newDataRow[enumPTColumns.Recommendation.ToString()] = string.Empty;
                    newDataRow[enumPTColumns.Strategy.ToString()] = string.Empty;
                    newDataRow[enumPTColumns.Profile.ToString()] = string.Empty;
                    newDataRow[enumPTColumns.Position.ToString()] = "Open";
                    newDataRow[enumPTColumns.Id.ToString()] = Guid.NewGuid();
                    newDataRow[enumPTColumns.DaysToExpiry.ToString()] = 0;
                    newDataRow[enumPTColumns.DaysHeld.ToString()] = 0;
                    newDataRow[enumPTColumns.EntryDate.ToString()] = DateTime.Now.ToString("dd-MMM-yyyy");
                    newDataRow[enumPTColumns.ExitDate.ToString()] = null;
                    dataTable.Rows.Add(newDataRow);
                }
                FileClass.WriteDataTable(dataTable, Constants.MYFILES_FOLDER_PATH + Constants.PT_FILE_NAME);
            }
        }


        public static void AddPTRows(string oCType, string expiryDate, string contractType, string transactionType, int strikePrice, string strategy, string profile)
        {
            DataTable dataTable = FileClass.ReadCsvFile(Constants.MYFILES_FOLDER_PATH + Constants.PT_FILE_NAME);
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
                newDataRow[enumPTColumns.SelectFlg.ToString()] = false;
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
                newDataRow[enumPTColumns.EntryDate.ToString()] = DateTime.Now.ToString("dd-MMM-yyyy");
                newDataRow[enumPTColumns.ExitDate.ToString()] = null;
                dataTable.Rows.Add(newDataRow);

                FileClass.WriteDataTable(dataTable, Constants.MYFILES_FOLDER_PATH + Constants.PT_FILE_NAME);
            }
        }
    }

    enum enumPTColumns
    {
        SelectFlg,
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
        EntryDate,
        ExitDate,
        UserId
    }

}