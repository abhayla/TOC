using System;
using System.Collections.Generic;
using System.Data;

namespace TOC.Strategy
{
    public class SpreadsClass
    {
        public static DataSet GetSpreads(FilterConditions filterConditions)
        {
            DataTable filteredDataTable = OCHelper.FilterDataTableRecords(filterConditions);
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

            int currentStrikePrice = OCHelper.RoundTo100(recordsObject.underlyingValue);
            int maxDiff = iHighestSP - currentStrikePrice;
            var enumStrategyColumnsCount = Enum.GetNames(typeof(enumStrategyColumns)).Length;

            for (int iHigher = iHighestSP; iHigher > iLowestSP; iHigher -= diffStrikePrice)//int iHigher = iLowestSP + diffStrikePrice; iHigher <= iHighestSP; iHigher += +diffStrikePrice
            {
                for (int iLower = iHigher - diffStrikePrice; iLower >= iLowestSP; iLower -= diffStrikePrice) //int iLower = iLowestSP; iLower < iHigher; iLower += diffStrikePrice
                {
                    if (iLower >= iHigher ||
                        iHigher > iHighestSP ||
                        iLower < iLowestSP)
                    {
                        continue;
                    }

                    string selectQuery = string.Empty;

                    if (!filterConditions.ExpiryDate.Equals(string.Empty))
                    {
                        selectQuery = "(" + enumStrategyColumns.ContractType + " = '" + contractType + "')" +
                       " AND ((" + enumStrategyColumns.StrikePrice + " = " + (iLower) + " AND TransactionType = 'Buy') OR (" + enumStrategyColumns.StrikePrice + " = " + (iHigher) +
                       " AND " + enumStrategyColumns.TransactionType + " = 'Sell')) AND (" + enumStrategyColumns.ExpiryDate + " = #" + filterConditions.ExpiryDate + "#)";
                    }
                    else
                    {
                        selectQuery = "(" + enumStrategyColumns.ContractType + " = '" + contractType + "')" +
                       " AND ((" + enumStrategyColumns.StrikePrice + " = " + (iLower) + " AND " + enumStrategyColumns.TransactionType + " = 'Buy') OR (" + enumStrategyColumns.StrikePrice + " = " + (iHigher) +
                       " AND " + enumStrategyColumns.TransactionType + " = 'Sell'))";
                    }

                    DataRow[] drs = filteredDataTable.Select(selectQuery, enumStrategyColumns.StrikePrice + " ASC");

                    DataTable dt = filteredDataTable.Clone();

                    //Create new datatable with name
                    DataTable dtResult = new DataTable("dt" + filterConditions.ExpiryDate + contractType + iLower + iHigher);
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

                            if (rowStrikePrice == iLower.ToString() || rowStrikePrice == iHigher.ToString())
                            {
                                sum += Convert.ToDouble(dt.Rows[j][strikePrice]);
                            }
                        }

                        int quantity = Convert.ToInt32(dt.Rows[i][enumStrategyColumns.LotSize.ToString()]);
                        int newquantity = quantity;

                        dtResult.Rows.Add(new string[] {
                            dt.Rows[i][enumStrategyColumns.TradingSymbol.ToString()].ToString(),
                            dt.Rows[i][enumStrategyColumns.ContractType.ToString()].ToString(),
                            dt.Rows[i][enumStrategyColumns.TransactionType.ToString()].ToString(),
                            dt.Rows[i][enumStrategyColumns.StrikePrice.ToString()].ToString(), newquantity.ToString(),
                            dt.Rows[i][enumStrategyColumns.Premium.ToString()].ToString(),
                            Math.Round(quantity * sum, 2).ToString()});
                    }

                    if (dtResult.Rows.Count == 2)
                    {
                        dataSet.Tables.Add(dtResult);
                    }
                }
            }
            return dataSet;
        }
    }
}