using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace TOC.Strategy
{
    public class BasketOrderClass
    {
        public static DataTable AddBOColumns()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(enumBasketOrderColumns.Select.ToString(), typeof(bool));
            dataTable.Columns.Add(enumBasketOrderColumns.ExpiryDate.ToString(), typeof(string));
            dataTable.Columns.Add(enumBasketOrderColumns.ContractType.ToString(), typeof(string));
            dataTable.Columns.Add(enumBasketOrderColumns.TransactionType.ToString(), typeof(string));
            dataTable.Columns.Add(enumBasketOrderColumns.StrikePrice.ToString(), typeof(int));
            dataTable.Columns.Add(enumBasketOrderColumns.OCType.ToString(), typeof(string));
            dataTable.Columns.Add(enumBasketOrderColumns.OrderType.ToString(), typeof(string));
            dataTable.Columns.Add(enumBasketOrderColumns.LimitPrice.ToString(), typeof(double));
            dataTable.Columns.Add(enumBasketOrderColumns.Lots.ToString(), typeof(int));
            dataTable.Columns.Add(enumBasketOrderColumns.CMP.ToString(), typeof(double));
            dataTable.Columns.Add(enumBasketOrderColumns.TradingSymbol.ToString(), typeof(string));
            return dataTable;
        }

        public static void AddBlankRows(DataTable dataTable)
        {
            dataTable.Rows.Add(
                    false,                                                      //Select,
                    OCHelper.DefaultExpDate(enumOCType.NIFTY.ToString()),       //ExpiryDate,
                    enumContractType.CE.ToString(),                             //ContractType,
                    enumTransactionType.SELL.ToString(),                        //TransactionType,
                    OCHelper.DefaultSP(enumOCType.NIFTY.ToString()),            //StrikePrice,
                    enumOCType.NIFTY.ToString(),                                //OC Type,
                    "LIMIT",                                                    //OrderType,
                    0,                                                          //LimitPrice,
                    1,                                                          //Lots,
                    0,                                                          //CMP
                    string.Concat(enumOCType.NIFTY.ToString(), OCHelper.TradingSymbol_DateFormatter(OCHelper.DefaultExpDate(enumOCType.NIFTY.ToString())), OCHelper.DefaultSP(enumOCType.NIFTY.ToString()), enumContractType.CE.ToString())                                                          //TradingSymbol,
                );
        }
    }

    enum enumBasketOrderColumns
    {
        Select,
        ExpiryDate,
        ContractType,
        TransactionType,
        StrikePrice,
        OCType,
        OrderType,
        LimitPrice,
        Lots,
        CMP,
        TradingSymbol
    }
}