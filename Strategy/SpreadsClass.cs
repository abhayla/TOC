using System;
using System.Collections.Generic;
using System.Data;

namespace TOC.Strategy
{
    public class SpreadsClass
    {
        public static DataSet GetSpreads(FilterConditions filterConditions)
        {
            DataTable filteredDataTable = OCHelper.AddRecordsToDataTable(filterConditions);
            DataSet dataSet = new DataSet();
            DataSet dataSetCE = new DataSet();
            DataSet dataSetPE = new DataSet();

            if (filterConditions.ContractType.Equals(enumContractType.CE.ToString()))
            {
                dataSetCE = CalculateSpreads(filteredDataTable, enumContractType.CE.ToString(), filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetCE);
            }

            if (filterConditions.ContractType.Equals(enumContractType.PE.ToString()))
            {
                dataSetPE = CalculateSpreads(filteredDataTable, enumContractType.PE.ToString(), filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetPE);
            }

            if (filterConditions.ContractType.Equals("ALL"))
            {
                dataSetCE = CalculateSpreads(filteredDataTable, enumContractType.CE.ToString(), filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetCE);

                dataSetPE = CalculateSpreads(filteredDataTable, enumContractType.PE.ToString(), filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetPE);
            }
            return dataSet;
        }

        private static DataSet CalculateSpreads(DataTable filteredDataTable, string contractType, FilterConditions filterConditions)
        {
            DataSet dataSet = new DataSet();
            Records recordsObject = MySession.Current.RecordsObject;

            //Find Lowest and Highest value in the given table column
            int iLowestSP = int.MaxValue;
            int iHighestSP = int.MinValue;
            foreach (DataRow dr in filteredDataTable.Rows)
            {
                int accountLevel = Convert.ToInt32(dr.Field<string>("StrikePrice"));
                iLowestSP = Math.Min(iLowestSP, accountLevel);
                iHighestSP = Math.Max(iHighestSP, accountLevel);
            }

            //Find difference between two strike prices
            List<int> prices = recordsObject.strikePrices;
            int diffStrikePrice = 0;
            if (prices.Count > 2)
            {
                diffStrikePrice = (prices[1] - prices[0]);
            }

            //double iHighterSPIterator = iLowestSP;
            //double iMiddleSPIterator = iLowestSP;
            //double iLowerSPIterator = iLowestSP;

            //int iMiddleLowerRange = 0;
            //int iMiddleHigherRange = 0;

            //If "NONE" selected in the ddlb
            //if (filterConditions.SPExpiry == 0)
            //{
            //    iMiddleLowerRange = iLowestSP + diffStrikePrice;
            //    iMiddleHigherRange = iHighestSP;
            //}
            //else
            //{
            //    iMiddleLowerRange = filterConditions.SPExpiry;
            //    iMiddleHigherRange = filterConditions.SPExpiry + 1;
            //}

            int currentStrikePrice = OCHelper.RoundTo100(recordsObject.underlyingValue);
            int maxDiff = iHighestSP - currentStrikePrice;

            //for (int iMiddle = iMiddleLowerRange; iMiddle < iMiddleHigherRange; iMiddle = iMiddle + diffStrikePrice)
            //{

            for (int iHigher = iHighestSP ; iHigher > iLowestSP; iHigher -= diffStrikePrice)//int iHigher = iLowestSP + diffStrikePrice; iHigher <= iHighestSP; iHigher += +diffStrikePrice
            {
                for (int iLower = iHigher - diffStrikePrice; iLower >= iLowestSP; iLower -= diffStrikePrice) //int iLower = iLowestSP; iLower < iHigher; iLower += diffStrikePrice
                {
                    //iMiddleSPIterator = iMiddle;
                    //iHighterSPIterator = iHigher;
                    //iLowerSPIterator = iLower;

                    if (iLower >= iHigher ||
                        iHigher > iHighestSP ||
                        iLower < iLowestSP)
                    {
                        continue;
                    }

                    string selectQuery = "(Contract = '" + contractType + "')" +
                    " AND ((StrikePrice = " + (iLower) + " AND TransactionType = 'Buy') OR (StrikePrice = " + (iHigher) +
                    " AND TransactionType = 'Sell')) AND (ExpiryDate = #" + filterConditions.ExpiryDate + "#)";

                    DataRow[] drs = filteredDataTable.Select(selectQuery, "StrikePrice ASC");

                    DataTable dt = filteredDataTable.Clone();

                    //Create new datatable with name
                    DataTable dtResult = new DataTable("dt" + contractType + iLower + iHigher);
                    OCHelper.AddColumnsToOutputGrid(dtResult);

                    foreach (DataRow dr in drs)
                    {
                        //This is alternate code for Calculation code in AddRecordsToDataTable function in OCHelper
                        //for (int iCellCount = 9; iCellCount < dr.ItemArray.Length; iCellCount++)
                        //{
                        //    dr[iCellCount] =
                        //        FO.CalcExpVal(dr["Contract"].ToString(), 
                        //        dr["TransactionType"].ToString(), 
                        //        Convert.ToDouble(dr["StrikePrice"]), 
                        //        Convert.ToDouble(dr["Premium"]), 
                        //        Convert.ToDouble(dr.Table.Columns[iCellCount].ColumnName));
                        //}

                        dt.ImportRow(dr);
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        double sum = 0;
                        var strikePrice = dt.Rows[i]["StrikePrice"].ToString();
                        //int quantity = 0;

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            var rowStrikePrice = dt.Rows[j]["StrikePrice"].ToString();

                            //if (rowStrikePrice == iMiddleSPIterator.ToString())
                            //{
                            //    sum += Convert.ToDouble(dt.Rows[j][strikePrice]) * 2;
                            //}
                            if (rowStrikePrice == iLower.ToString() || rowStrikePrice == iHigher.ToString())
                            {
                                sum += Convert.ToDouble(dt.Rows[j][strikePrice]);
                            }
                        }

                        int quantity = Convert.ToInt32(dt.Rows[i]["LotSize"]);
                        int newquantity = quantity;
                        //if (strikePrice.Equals(iMiddleSPIterator.ToString()))
                        //    newquantity = quantity * 2;

                        dtResult.Rows.Add(new string[] {
                            dt.Rows[i]["TradingSymbol"].ToString(),
                            dt.Rows[i]["Contract"].ToString(),
                            dt.Rows[i]["TransactionType"].ToString(),
                            dt.Rows[i]["StrikePrice"].ToString(),
                            newquantity.ToString(),
                            dt.Rows[i]["Premium"].ToString(),
                            Math.Round(quantity*sum,2).ToString()});
                    }

                    if (dtResult.Rows.Count >= 2)
                    {
                        dataSet.Tables.Add(dtResult);
                    }
                }
            }
            //}
            return dataSet;
        }
    }
}