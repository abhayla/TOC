using System;
using System.Collections.Generic;
using System.Data;

namespace TOC.Strategy
{
    public class IronCondorClass
    {
        public static DataSet GetIronCondors(FilterConditions filterConditions)
        {
            DataTable filteredDataTable = OCHelper.FilterDataTableRecords(filterConditions);

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

            if (filterConditions.ContractType.Equals(Constants.ALL))
            {
                dataSetCE = CalculateIronCondors(filteredDataTable, filterConditions);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetCE);
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
                int accountLevel = dr.Field<int>(enumStrategyColumns.StrikePrice.ToString());
                iLowestSP = Math.Min(iLowestSP, accountLevel);
                iHighestSP = Math.Max(iHighestSP, accountLevel);
            }

            //Find difference between two strike prices
            List<int> prices = recordsObject.strikePrices;
            int diffStrikePrice = 0;
            if (prices.Count > 2)
            {
                //diffStrikePrice = (prices[1] - prices[0]);
                diffStrikePrice = filterConditions.SPDifference;
            }
            else
            {
                diffStrikePrice = filterConditions.SPDifference;
            }

            int ATMStrikePrice = 0;
            if (OCHelper.RoundTo100(recordsObject.underlyingValue) < recordsObject.underlyingValue)
                ATMStrikePrice = OCHelper.RoundTo100(recordsObject.underlyingValue) + 100;
            else
                ATMStrikePrice = OCHelper.RoundTo100(recordsObject.underlyingValue);

            if (filterConditions.SPLowerRange == 0)
            {
                filterConditions.SPLowerRange = ATMStrikePrice - diffStrikePrice;
            }

            if (filterConditions.SPHigherRange == 0)
            {
                filterConditions.SPHigherRange = ATMStrikePrice;
            }

            var enumStrategyColumnsCount = Enum.GetNames(typeof(enumStrategyColumns)).Length;

            for (int iHighest = filterConditions.SPHigherRange + diffStrikePrice; iHighest <= iHighestSP; iHighest += diffStrikePrice)
            {
                for (int iHigher = filterConditions.SPHigherRange; iHigher < iHighest; iHigher += diffStrikePrice)
                {
                    for (int iLower = filterConditions.SPLowerRange; iLower > iLowestSP + diffStrikePrice; iLower -= diffStrikePrice)//iLowestSP + diffStrikePrice
                    {
                        for (int iLowest = iLower - diffStrikePrice; iLowest >= iLowestSP; iLowest -= diffStrikePrice)//iLowestSP
                        {
                            if (iLowest >= iLower || iLower >= iHigher || iHigher >= iHighest)
                            {
                                continue;
                            }

                            string selectQuery = string.Empty;

                            if (filterConditions.ExpiryDate != null && !filterConditions.ExpiryDate.ToUpper().Equals(Constants.ALL))
                            {
                                selectQuery =
                                "(" + enumStrategyColumns.ContractType + " = 'PE' AND " + enumStrategyColumns.StrikePrice + " = " + (iLowest) + " AND " + enumStrategyColumns.TransactionType + " = 'Buy') OR " +
                                "(" + enumStrategyColumns.ContractType + " = 'PE' AND " + enumStrategyColumns.StrikePrice + " = " + (iLower) + " AND " + enumStrategyColumns.TransactionType + " = 'Sell') OR " +
                                "(" + enumStrategyColumns.ContractType + " = 'CE' AND " + enumStrategyColumns.StrikePrice + " = " + (iHigher) + " AND " + enumStrategyColumns.TransactionType + " = 'Sell') OR " +
                                "(" + enumStrategyColumns.ContractType + " = 'CE' AND " + enumStrategyColumns.StrikePrice + " = " + (iHighest) + " AND " + enumStrategyColumns.TransactionType + " = 'Buy') " +
                                "AND (" + enumStrategyColumns.ExpiryDate + " = #" + filterConditions.ExpiryDate + "#)";
                            }
                            else
                            {
                                selectQuery =
                                "(" + enumStrategyColumns.ContractType + " = 'PE' AND " + enumStrategyColumns.StrikePrice + " = " + (iLowest) + " AND " + enumStrategyColumns.TransactionType + " = 'Buy') OR " +
                                "(" + enumStrategyColumns.ContractType + " = 'PE' AND " + enumStrategyColumns.StrikePrice + " = " + (iLower) + " AND " + enumStrategyColumns.TransactionType + " = 'Sell') OR " +
                                "(" + enumStrategyColumns.ContractType + " = 'CE' AND " + enumStrategyColumns.StrikePrice + " = " + (iHigher) + " AND " + enumStrategyColumns.TransactionType + " = 'Sell') OR " +
                                "(" + enumStrategyColumns.ContractType + " = 'CE' AND " + enumStrategyColumns.StrikePrice + " = " + (iHighest) + " AND " + enumStrategyColumns.TransactionType + " = 'Buy') ";

                                //"(ContractType = 'PE' AND StrikePrice = " + (iLowest) + " AND TransactionType = 'Buy') OR " +
                                //"(ContractType = 'PE' AND StrikePrice = " + (iLower) + " AND TransactionType = 'Sell') OR " +
                                //"(ContractType = 'CE' AND StrikePrice = " + (iHigher) + " AND TransactionType = 'Sell') OR " +
                                //"(ContractType = 'CE' AND StrikePrice = " + (iHighest) + " AND TransactionType = 'Buy')";
                            }

                            DataRow[] drs = filteredDataTable.Select(selectQuery, enumStrategyColumns.StrikePrice + " ASC");

                            if (drs.Length == 4)
                            {
                                DataTable dt = filteredDataTable.Clone();

                                //Create new datatable with name
                                DataTable dtResult = new DataTable("dt" + filterConditions.ExpiryDate + iLowest + iLower + iHigher + iHighest);
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
                                    //int quantity = 0;

                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        var rowStrikePrice = dt.Rows[j][enumStrategyColumns.StrikePrice.ToString()].ToString();

                                        sum += Convert.ToDouble(dt.Rows[j][strikePrice]);
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

                                if (dtResult.Rows.Count == 4)
                                {
                                    dataSet.Tables.Add(dtResult);
                                }
                            }
                        }
                    }
                }
            }
            return dataSet;
        }
    }
}