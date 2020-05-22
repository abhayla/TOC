using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace TOC.Strategy
{
    public class Butterfly
    {
        public static DataTable GetBestButterflySpreadStrategy(string ocType, int iPercentageRange, string expiryDate, string contractType)
        {
            DataTable dataTable = new DataTable();
            Records recordsObject = MySession.Current.RecordsObject;
            DataSet dataSet = GetButterflySpreadStrategies(ocType, iPercentageRange, expiryDate, contractType);

            return dataTable;
        }

        public static DataSet GetButterflySpreadStrategies(string ocType, int iPercentageRange, string expiryDates, string contractType)
        {
            DataTable filteredDataTable = OCHelper.AddRecordsToDataTable(ocType, iPercentageRange);
            DataSet dataSet = new DataSet();
            DataSet dataSetCE = new DataSet();
            DataSet dataSetPE = new DataSet();

            if (contractType.Equals(enumContractType.CE.ToString()))
            {
                dataSetCE = GetButterflySpreadStrategies(filteredDataTable, expiryDates, enumContractType.CE.ToString());
                //dataSet.Merge(dataSetCE);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetCE);
            }

            if (contractType.Equals(enumContractType.PE.ToString()))
            {
                dataSetPE = GetButterflySpreadStrategies(filteredDataTable, expiryDates, enumContractType.PE.ToString());
                //dataSet.Merge(dataSetPE);
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetPE);
            }

            if (contractType.Equals("ALL"))
            {
                dataSetCE = GetButterflySpreadStrategies(filteredDataTable, expiryDates, enumContractType.CE.ToString());
                //dataSet = dataSetCE.Copy();
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetCE);

                dataSetPE = GetButterflySpreadStrategies(filteredDataTable, expiryDates, enumContractType.PE.ToString());
                //dataSet = dataSetPE.Copy();
                dataSet = OCHelper.MergeDataSets(dataSet, dataSetPE);
            }
            return dataSet;
        }

        private static DataSet GetButterflySpreadStrategies(DataTable filteredDataTable, string expiryDates, string contractType)
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

            double iHighterSPIterator = iLowestSP;
            double iMiddleSPIterator = iLowestSP;
            double iLowerSPIterator = iLowestSP;

            int currentStrikePrice = OCHelper.RoundTo100(recordsObject.underlyingValue);
            int maxDiff = iHighestSP - currentStrikePrice;

            for (int iMiddle = iLowestSP + diffStrikePrice; iMiddle < iHighestSP; iMiddle = iMiddle + diffStrikePrice)
            {
                for (int iLower = iLowestSP; iLower < iMiddle; iLower = iLower + diffStrikePrice)
                {
                    for (int iHigher = iMiddle + diffStrikePrice; iHigher <= iHighestSP; iHigher = iHigher + diffStrikePrice)
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

                        string selectQuery = "(Contract = '" + contractType + "')" +
                        " AND ((StrikePrice = " + (iLowerSPIterator) + " AND TransactionType = 'Buy') OR (StrikePrice = " + iMiddleSPIterator +
                        " AND TransactionType = 'Sell') OR (StrikePrice = " + (iHighterSPIterator) +
                        " AND TransactionType = 'Buy')) AND (ExpiryDate = #" + expiryDates + "#)";

                        DataRow[] drs = filteredDataTable.Select(selectQuery, "StrikePrice ASC");

                        DataTable dt = filteredDataTable.Clone();

                        //Create new datatable with name
                        DataTable dtResult = new DataTable("dt" + contractType + iLowerSPIterator + iMiddleSPIterator + iHighterSPIterator);
                        OCHelper.AddColumnsToOutputGrid(dtResult);

                        foreach (DataRow d in drs)
                        {
                            dt.ImportRow(d);
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            double sum = 0;
                            var strikePrice = dt.Rows[i]["StrikePrice"].ToString();
                            //int quantity = 0;

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                var rowStrikePrice = dt.Rows[j]["StrikePrice"].ToString();

                                if (rowStrikePrice == iMiddleSPIterator.ToString())
                                {
                                    sum += Convert.ToDouble(dt.Rows[j][strikePrice]) * 2;
                                }
                                else if (rowStrikePrice == iLowerSPIterator.ToString() || rowStrikePrice == iHighterSPIterator.ToString())
                                {
                                    sum += Convert.ToDouble(dt.Rows[j][strikePrice]);
                                }
                            }

                            int quantity = Convert.ToInt32(dt.Rows[i]["LotSize"]);
                            int newquantity = quantity;
                            if (strikePrice.Equals(iMiddleSPIterator.ToString()))
                                newquantity = quantity * 2;

                            //string tradingSymbol = OCHelper.FormatTradingSymbol(dt.Rows[i]);

                            dtResult.Rows.Add(new string[] {
                            dt.Rows[i]["TradingSymbol"].ToString(),
                            dt.Rows[i]["Contract"].ToString(),
                            dt.Rows[i]["TransactionType"].ToString(),
                            dt.Rows[i]["StrikePrice"].ToString(),
                            newquantity.ToString(),
                            dt.Rows[i]["Premium"].ToString(),
                            Math.Round(quantity*sum,2).ToString()});
                        }

                        if (dtResult.Rows.Count >= 3)
                        {
                            dataSet.Tables.Add(dtResult);
                        }
                    }
                }
            }
            return dataSet;
        }

        private static DataSet GetButterflySpreadStrategiesOld(DataTable filteredDataTable, string expiryDates, string contractType)
        {
            DataSet dataSet = new DataSet();
            Records recordsObject = MySession.Current.RecordsObject;

            //Find Lowest and Highest value in the given table column
            int iLowerSP = int.MaxValue;
            int iHighterSP = int.MinValue;
            foreach (DataRow dr in filteredDataTable.Rows)
            {
                int accountLevel = Convert.ToInt32(dr.Field<string>("StrikePrice"));
                iLowerSP = Math.Min(iLowerSP, accountLevel);
                iHighterSP = Math.Max(iHighterSP, accountLevel);
            }

            //Find difference between two strike prices
            List<int> prices = recordsObject.strikePrices;
            int diffStrikePrice = 0;
            if (prices.Count > 2)
            {
                diffStrikePrice = (prices[1] - prices[0]);
            }

            double iHighterSPIterator = iLowerSP;
            double iMiddleSPIterator = iLowerSP;
            double iLowerSPIterator = iLowerSP;

            for (int iCount = 2; iCount < filteredDataTable.Rows.Count; iCount++)
            {
                for (int jCount = iCount - 1; jCount < iCount; jCount++)
                {
                    for (int kCount = jCount - 1; kCount < jCount; kCount++)
                    {
                        iHighterSPIterator = iLowerSP + (iCount * diffStrikePrice);
                        iMiddleSPIterator = iLowerSP + (jCount * diffStrikePrice);
                        iLowerSPIterator = iLowerSP + (kCount * diffStrikePrice);

                        if (iLowerSPIterator >= iHighterSPIterator ||
                            iHighterSPIterator <= iMiddleSPIterator ||
                            iLowerSPIterator >= iMiddleSPIterator)
                        {
                            continue;
                        }

                        string selectQuery = "(Contract = '" + contractType + "')" +
                        " AND ((StrikePrice = " + (iLowerSPIterator) + " AND TransactionType = 'Buy') OR (StrikePrice = " + iMiddleSPIterator +
                        " AND TransactionType = 'Sell') OR (StrikePrice = " + (iHighterSPIterator) +
                        " AND TransactionType = 'Buy')) AND (ExpiryDate = #" + expiryDates + "#)";

                        DataRow[] drs = filteredDataTable.Select(selectQuery, "StrikePrice ASC");

                        DataTable dt = filteredDataTable.Clone();

                        //Create new datatable with name
                        DataTable dtResult = new DataTable("dt" + contractType + iLowerSPIterator + iMiddleSPIterator + iHighterSPIterator);
                        dtResult.Columns.Add("Stock");
                        dtResult.Columns.Add("Contract");
                        dtResult.Columns.Add("TransactionType");
                        dtResult.Columns.Add("StrikePrice");
                        dtResult.Columns.Add("Quantity");
                        dtResult.Columns.Add("Premium");
                        dtResult.Columns.Add("ProfitLoss");

                        //DataTable dtResult = dt.Clone();

                        foreach (DataRow d in drs)
                        {
                            dt.ImportRow(d);
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            double sum = 0;
                            var strikePrice = dt.Rows[i]["StrikePrice"].ToString();
                            //int quantity = 0;

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                var rowStrikePrice = dt.Rows[j]["StrikePrice"].ToString();

                                if (rowStrikePrice == iMiddleSPIterator.ToString())
                                {
                                    sum += Convert.ToDouble(dt.Rows[j][strikePrice]) * 2;
                                }
                                else if (rowStrikePrice == iLowerSPIterator.ToString() || rowStrikePrice == iHighterSPIterator.ToString())
                                {
                                    sum += Convert.ToDouble(dt.Rows[j][strikePrice]);
                                }
                            }

                            int quantity = Convert.ToInt32(dt.Rows[i]["LotSize"]);
                            int newquantity = quantity;
                            if (strikePrice.Equals(iMiddleSPIterator.ToString()))
                                newquantity = quantity * 2;

                            dtResult.Rows.Add(new string[] {
                            dt.Rows[i]["Stock"].ToString(),
                            dt.Rows[i]["Contract"].ToString(),
                            dt.Rows[i]["TransactionType"].ToString(),
                            dt.Rows[i]["StrikePrice"].ToString(),
                            newquantity.ToString(),
                            dt.Rows[i]["Premium"].ToString(),
                            Math.Round(quantity*sum,2).ToString()});
                        }

                        if (dtResult.Rows.Count >= 3)
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