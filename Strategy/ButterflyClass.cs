using System;
using System.Collections.Generic;
using System.Data;

namespace TOC.Strategy
{
    public class ButterflyClass
    {
        public static DataSet GetButterflySpreadStrategies(FilterConditions filterConditions)
        {
            DataTable filteredDataTable = OCHelper.FilterDataTableRecords(filterConditions);
            DataSet dataSet = new DataSet();
            DataSet dataSetCE = new DataSet();
            DataSet dataSetPE = new DataSet();

            if (filterConditions.ContractType.Equals(enumContractType.CE.ToString()))
            {
                dataSetCE = GetButterflySpreadStrategies(filteredDataTable, enumContractType.CE.ToString(), filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetCE);
            }

            if (filterConditions.ContractType.Equals(enumContractType.PE.ToString()))
            {
                dataSetPE = GetButterflySpreadStrategies(filteredDataTable, enumContractType.PE.ToString(), filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetPE);
            }

            if (filterConditions.ContractType.Equals(Constants.ALL))
            {
                dataSetCE = GetButterflySpreadStrategies(filteredDataTable, enumContractType.CE.ToString(), filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetCE);

                dataSetPE = GetButterflySpreadStrategies(filteredDataTable, enumContractType.PE.ToString(), filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetPE);
            }
            return dataSet;
        }

        public static DataSet GetButterflySpreadStrategies(DataTable filteredDataTable, string contractType, FilterConditions filterConditions)
        {
            DataSet dataSet = new DataSet();
            Records recordsObject = MySession.Current.RecordsObject;

            //Find Lowest and Highest value in the given table column
            int iLowestSP = int.MaxValue;
            int iHighestSP = int.MinValue;
            foreach (DataRow dr in filteredDataTable.Rows)
            {
                int accountLevel = dr.Field<int>(enumStrategyColumns.StrikePrice.ToString());
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

            double iHighterSPIterator = iLowestSP;
            double iMiddleSPIterator = iLowestSP;
            double iLowerSPIterator = iLowestSP;

            int iMiddleLowerRange = 0;
            int iMiddleHigherRange = 0;

            //If "NONE" selected in the ddlb
            if (filterConditions.SPExpiry == 0)
            {
                iMiddleLowerRange = iLowestSP + diffStrikePrice;
                iMiddleHigherRange = iHighestSP;
            }
            else
            {
                iMiddleLowerRange = filterConditions.SPExpiry;
                iMiddleHigherRange = filterConditions.SPExpiry + 1;
            }

            int currentStrikePrice = OCHelper.RoundTo100(recordsObject.underlyingValue);
            int maxDiff = iHighestSP - currentStrikePrice;

            var enumStrategyColumnsCount = Enum.GetNames(typeof(enumStrategyColumns)).Length;

            for (int iMiddle = iMiddleLowerRange; iMiddle < iMiddleHigherRange; iMiddle += diffStrikePrice)
            {
                for (int iLower = iMiddle - diffStrikePrice; iLower >= iLowestSP; iLower -= diffStrikePrice) //(int iLower = iLowestSP; iLower < iMiddle; iLower += diffStrikePrice)
                {
                    for (int iHigher = iHighestSP; iHigher > iMiddle; iHigher -= diffStrikePrice)  //(int iHigher = iMiddle + diffStrikePrice; iHigher <= iHighestSP; iHigher += diffStrikePrice)
                    {
                        iMiddleSPIterator = iMiddle;
                        iHighterSPIterator = iHigher;
                        iLowerSPIterator = iLower;

                        if (iLowerSPIterator >= iHighterSPIterator ||
                            iHighterSPIterator <= iMiddleSPIterator ||
                            iLowerSPIterator >= iMiddleSPIterator ||
                            iHighterSPIterator > iHighestSP ||
                            iLowerSPIterator < iLowestSP)
                        {
                            continue;
                        }

                        string selectQuery = string.Empty;

                        if (filterConditions.ExpiryDate != null && !filterConditions.ExpiryDate.ToUpper().Equals("ALL"))
                        {
                            selectQuery = "(" + enumStrategyColumns.ContractType + " = '" + contractType + "')" +
                            " AND ((" + enumStrategyColumns.StrikePrice + " = " + (iLowerSPIterator) + " AND " + enumStrategyColumns.TransactionType + " = 'Buy') OR (" + enumStrategyColumns.StrikePrice + " = " + iMiddleSPIterator +
                            " AND " + enumStrategyColumns.TransactionType + " = 'Sell') OR (" + enumStrategyColumns.StrikePrice + " = " + (iHighterSPIterator) +
                            " AND " + enumStrategyColumns.TransactionType + " = 'Buy')) AND (" + enumStrategyColumns.ExpiryDate + " = #" + filterConditions.ExpiryDate + "#)";
                        }
                        else
                        {
                            selectQuery = "(" + enumStrategyColumns.ContractType + "= '" + contractType + "')" +
                            " AND ((" + enumStrategyColumns.StrikePrice + " = " + (iLowerSPIterator) + " AND " + enumStrategyColumns.TransactionType + " = 'Buy') OR (" + enumStrategyColumns.StrikePrice + " = " + iMiddleSPIterator +
                            " AND " + enumStrategyColumns.TransactionType + " = 'Sell') OR (" + enumStrategyColumns.StrikePrice + " = " + (iHighterSPIterator) +
                            " AND " + enumStrategyColumns.TransactionType + " = 'Buy'))";
                        }

                        DataRow[] drs = filteredDataTable.Select(selectQuery, enumStrategyColumns.StrikePrice + " ASC");

                        DataTable dt = filteredDataTable.Clone();

                        //Create new datatable with name
                        DataTable dtResult = new DataTable("dt" + filterConditions.ExpiryDate + contractType + iLowerSPIterator + iMiddleSPIterator + iHighterSPIterator);
                        OCHelper.AddColumnsToOutputGrid(dtResult);

                        foreach (DataRow dr in drs)
                        {
                            //This is alternate code for Calculation code in AddRecordsToDataTable function in OCHelper
                            for (int iCellCount = enumStrategyColumnsCount; iCellCount < dr.ItemArray.Length; iCellCount++)
                            {
                                dr[iCellCount] =
                                    FO.CalcExpVal(dr[enumStrategyColumns.ContractType.ToString()].ToString(),
                                    dr[enumStrategyColumns.TransactionType.ToString()].ToString(),
                                    Convert.ToDouble(dr[enumStrategyColumns.StrikePrice.ToString()]),
                                    Convert.ToDouble(dr[enumStrategyColumns.Premium.ToString()]),
                                    Convert.ToDouble(dr.Table.Columns[iCellCount].ColumnName));
                            }

                            dt.ImportRow(dr);
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            double sum = 0;
                            var strikePrice = dt.Rows[i][enumStrategyColumns.StrikePrice.ToString()].ToString();

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                var rowStrikePrice = dt.Rows[j][enumStrategyColumns.StrikePrice.ToString()].ToString();

                                if (rowStrikePrice == iMiddleSPIterator.ToString())
                                {
                                    sum += Convert.ToDouble(dt.Rows[j][strikePrice]) * 2;
                                }
                                else if (rowStrikePrice == iLowerSPIterator.ToString() || rowStrikePrice == iHighterSPIterator.ToString())
                                {
                                    sum += Convert.ToDouble(dt.Rows[j][strikePrice]);
                                }
                            }

                            int quantity = Convert.ToInt32(dt.Rows[i][enumStrategyColumns.LotSize.ToString()]);
                            int newquantity = quantity;
                            if (strikePrice.Equals(iMiddleSPIterator.ToString()))
                                newquantity = quantity * 2;

                            dtResult.Rows.Add(new string[] {
                            dt.Rows[i][enumStrategyColumns.TradingSymbol.ToString()].ToString(),
                            dt.Rows[i][enumStrategyColumns.ContractType.ToString()].ToString(),
                            dt.Rows[i][enumStrategyColumns.TransactionType.ToString()].ToString(),
                            dt.Rows[i][enumStrategyColumns.StrikePrice.ToString()].ToString(), newquantity.ToString(),
                            dt.Rows[i][enumStrategyColumns.Premium.ToString()].ToString(),
                            Math.Round(quantity * sum, 2).ToString()});
                        }

                        if (dtResult.Rows.Count == 3)
                        {
                            dataSet.Tables.Add(dtResult);
                        }
                    }
                }
            }
            return dataSet;
        }
    }
}