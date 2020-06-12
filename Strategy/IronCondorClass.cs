using System;
using System.Collections.Generic;
using System.Data;

namespace TOC.Strategy
{
    public class IronCondorClass
    {
        public static DataSet GetIronCondors(FilterConditions filterConditions)
        {
            DataTable filteredDataTable = OCHelper.AddRecordsToDataTable(filterConditions);
            DataSet dataSet = new DataSet();
            DataSet dataSetCE = new DataSet();
            DataSet dataSetPE = new DataSet();

            if (filterConditions.ContractType.Equals(enumContractType.CE.ToString()))
            {
                dataSetCE = CalculateIronCondors(filteredDataTable, filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetCE);
            }

            if (filterConditions.ContractType.Equals(enumContractType.PE.ToString()))
            {
                dataSetPE = CalculateIronCondors(filteredDataTable, filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetPE);
            }

            if (filterConditions.ContractType.Equals("ALL"))
            {
                dataSetCE = CalculateIronCondors(filteredDataTable, filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetCE);

                //dataSetPE = CalculateIronCondors(filteredDataTable, enumContractType.PE.ToString(), filterConditions);
                //dataSet = OCHelper.MergeDataSets(dataSet, dataSetPE);
            }
            return dataSet;
        }

        public static DataSet CalculateIronCondors(DataTable filteredDataTable, FilterConditions filterConditions)
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
            else
            {
                diffStrikePrice = filterConditions.SPDifference;
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

            int ATMStrikePrice = 0;
            if (OCHelper.RoundTo100(recordsObject.underlyingValue) < recordsObject.underlyingValue)
                ATMStrikePrice = OCHelper.RoundTo100(recordsObject.underlyingValue) + 100;
            else
                ATMStrikePrice = OCHelper.RoundTo100(recordsObject.underlyingValue);

            //int maxDiff = iHighestSP - ATMStrikePrice;

            if (filterConditions.SPLowerRange == 0)
            {
                filterConditions.SPLowerRange = ATMStrikePrice - diffStrikePrice;
            }

            if (filterConditions.SPHigherRange == 0)
            {
                filterConditions.SPHigherRange = ATMStrikePrice;
            }

            //for (int iMiddle = iMiddleLowerRange; iMiddle < iMiddleHigherRange; iMiddle = iMiddle + diffStrikePrice)
            //{
            for (int iHighest = filterConditions.SPHigherRange + diffStrikePrice; iHighest <= iHighestSP; iHighest += diffStrikePrice)
            {
                for (int iHigher = filterConditions.SPHigherRange; iHigher < iHighest; iHigher += diffStrikePrice)
                {
                    for (int iLower = filterConditions.SPLowerRange; iLower > iLowestSP + diffStrikePrice; iLower -= diffStrikePrice)//iLowestSP + diffStrikePrice
                    {
                        for (int iLowerst = iLower - diffStrikePrice; iLowerst >= iLowestSP; iLowerst -= diffStrikePrice)//iLowestSP
                        {
                            //iMiddleSPIterator = iMiddle;
                            //iHighterSPIterator = iHigher;
                            //iLowerSPIterator = iLower;

                            if (iLowerst >= iLower || iLower >= iHigher || iHigher >= iHighest)
                            {
                                continue;
                            }

                            string selectQuery = string.Empty;

                            if (!filterConditions.ExpiryDate.Equals(string.Empty))
                            {
                                selectQuery =
                                   "(Contract = 'PE' AND StrikePrice = " + (iLowerst) + " AND TransactionType = 'Buy') OR " +
                                   "(Contract = 'PE' AND StrikePrice = " + (iLower) + " AND TransactionType = 'Sell') OR " +
                                   "(Contract = 'CE' AND StrikePrice = " + (iHigher) + " AND TransactionType = 'Sell') OR " +
                                   "(Contract = 'CE' AND StrikePrice = " + (iHighest) + " AND TransactionType = 'Buy') " +
                                   "AND (ExpiryDate = #" + filterConditions.ExpiryDate + "#)";
                            }
                            else
                            {
                                selectQuery =
                                "(Contract = 'PE' AND StrikePrice = " + (iLowerst) + " AND TransactionType = 'Buy') OR " +
                                "(Contract = 'PE' AND StrikePrice = " + (iLower) + " AND TransactionType = 'Sell') OR " +
                                "(Contract = 'CE' AND StrikePrice = " + (iHigher) + " AND TransactionType = 'Sell') OR " +
                                "(Contract = 'CE' AND StrikePrice = " + (iHighest) + " AND TransactionType = 'Buy')";
                            }



                            DataRow[] drs = filteredDataTable.Select(selectQuery, "StrikePrice ASC");

                            DataTable dt = filteredDataTable.Clone();

                            //Create new datatable with name
                            DataTable dtResult = new DataTable("dt" + filterConditions.ExpiryDate + iLowerst + iLower + iHigher + iHighest);
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
                                    //if (rowStrikePrice == iLower.ToString() || rowStrikePrice == iHigher.ToString())
                                    //{
                                    sum += Convert.ToDouble(dt.Rows[j][strikePrice]);
                                    //}
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

                            if (dtResult.Rows.Count == 4)
                            {
                                //dtResult.DefaultView.Sort = "StrikePrice ASC";
                                dataSet.Tables.Add(dtResult);
                            }
                        }
                    }
                }
            }
            return dataSet;
        }
    }
}