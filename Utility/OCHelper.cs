using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Web.UI.WebControls;

namespace TOC
{
    public class OCHelper
    {
        private const string url = "https://www.nseindia.com/api/option-chain-indices?symbol=";

        public static DataSet MergeDataSets(DataSet mergedDataSet, DataSet fromDataSet)
        {
            foreach (DataTable dt in fromDataSet.Tables)
            {
                mergedDataSet.Tables.Add(dt.Copy());
            }
            return mergedDataSet;
        }

        public static int GetLotSize(string ocType)
        {
            int lotsSize = 0;
            if (ocType.Equals(enumOptionChainType.NIFTY.ToString()))
                lotsSize = Convert.ToInt32(enumLotSize.Nifty);
            if (ocType.Equals(enumOptionChainType.BANKNIFTY.ToString()))
                lotsSize = Convert.ToInt32(enumLotSize.BankNifty);

            return lotsSize;
        }

        public static DataTable FilterDataTableRecords(FilterConditions filterConditions)
        {
            DataTable allRecordsDt = AddAllRecordsToDataTable(filterConditions.OcType);

            string strFilter = GetFilterString(filterConditions);

            DataRow[] drs = allRecordsDt.Select(strFilter);
            DataTable filteredDataTable = allRecordsDt.Clone();
            foreach (DataRow dr in drs)
            {
                filteredDataTable.ImportRow(dr);
            }

            return filteredDataTable;
        }

        public static DataTable FilterOptionChainDataTable(FilterConditions filterConditions)
        {
            DataTable allRecordsDt = OCHelper.toDataTable(MySession.Current.RecordsObject);

            //DataTable dt = new DataTable();
            //dt.Columns.Add("Stock");
            //dt.Columns.Add("Identifier");
            //dt.Columns.Add("TradingSymbol");
            //dt.Columns.Add("Contract");
            //dt.Columns.Add("TransactionType");
            //dt.Columns.Add("StrikePrice");
            //dt.Columns.Add("LotSize");
            //dt.Columns.Add("Premium");
            //dt.Columns.Add("ExpiryDate");

            //Records recordsObject = GetOC(filterConditions.OcType);

            string strFilter = GetFilterString(filterConditions);

            DataRow[] drs = allRecordsDt.Select(strFilter, "StrikePrice ASC");
            DataTable filteredDataTable = allRecordsDt.Clone();
            foreach (DataRow dr in drs)
            {
                filteredDataTable.ImportRow(dr);
            }

            return filteredDataTable;

            //List<int> strikePrices = MySession.Current.RecordsObject.strikePrices;
            //List<int> filteredStrikePrices = new List<int>();

            //foreach (var item in MySession.Current.RecordsObject.strikePrices)
            //{
            //    if (item <= iUpperStrikePriceRange && item >= iLowerStrikePriceRange)
            //    {
            //        filteredStrikePrices.Add(item);
            //        dt.Columns.Add(item.ToString());
            //    }
            //}

            //DataRow datarow;
            //foreach (DataRow datarow in allRecordsDt.Rows)
            //{
            //string formattedDateForTradingSymbol = TradingSymbol_DateFormatter(datarow["ExpiryDate"].ToString());
            //int strikePrice = Convert.ToInt32(datarow["StrikePrice"]);

            //if (strikePrice <= iUpperStrikePriceRange && strikePrice <= iLowerStrikePriceRange && strikePrice % 100 == 0)
            //{

            //}


            //if (!filterConditions.ExpiryDate.Equals(string.Empty))
            //{
            //    selectQuery = "(Contract = '" + contractType + "')" +
            //    " AND ((StrikePrice = " + (iLowerSPIterator) + " AND TransactionType = 'Buy') OR (StrikePrice = " + iMiddleSPIterator +
            //    " AND TransactionType = 'Sell') OR (StrikePrice = " + (iHighterSPIterator) +
            //    " AND TransactionType = 'Buy')) AND (ExpiryDate = #" + filterConditions.ExpiryDate + "#)";
            //}
            //else
            //{
            //    selectQuery = "(Contract = '" + contractType + "')" +
            //    " AND ((StrikePrice = " + (iLowerSPIterator) + " AND TransactionType = 'Buy') OR (StrikePrice = " + iMiddleSPIterator +
            //    " AND TransactionType = 'Sell') OR (StrikePrice = " + (iHighterSPIterator) +
            //    " AND TransactionType = 'Buy'))";
            //}

            //if (row.CE != null &&
            //    (row.CE.strikePrice <= iUpperStrikePriceRange &&
            //    row.CE.strikePrice >= iLowerStrikePriceRange) &&
            //    (row.CE.strikePrice % 100 == 0) &&
            //    row.CE.lastPrice > 0 &&
            //    //row.expiryDate.Equals(filterConditions.ExpiryDate) &&
            //    (filterConditions.ContractType.Equals(enumContractType.CE.ToString()) ||
            //    filterConditions.ContractType.Equals("ALL")))
            //{
            //    //Add CE Buy row
            //    datarow = dt.NewRow();
            //    datarow["Stock"] = row.CE.underlying;
            //    datarow["Identifier"] = row.CE.identifier;
            //    datarow["Contract"] = enumContractType.CE.ToString();
            //    datarow["TransactionType"] = enumTransactionType.BUY.ToString();
            //    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
            //    datarow["LotSize"] = GetLotSize(filterConditions.OcType);
            //    datarow["Premium"] = row.CE.lastPrice.ToString();
            //    datarow["ExpiryDate"] = row.CE.expiryDate;
            //    datarow["TradingSymbol"] = string.Concat(row.CE.underlying, formattedDateForTradingSymbol, row.CE.strikePrice.ToString(), enumContractType.CE.ToString());

            //    foreach (var item in filteredStrikePrices)
            //    {
            //        datarow[dt.Columns[item.ToString()].ColumnName] = FO.CallBuy(row.CE.strikePrice, row.CE.lastPrice, Convert.ToDouble(item));
            //    }
            //    dt.Rows.Add(datarow);

            //    //Add CE Sell row
            //    datarow = dt.NewRow();
            //    datarow["Stock"] = row.CE.underlying;
            //    datarow["Identifier"] = row.CE.identifier;
            //    datarow["Contract"] = enumContractType.CE.ToString();
            //    datarow["TransactionType"] = enumTransactionType.SELL.ToString();
            //    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
            //    datarow["LotSize"] = GetLotSize(filterConditions.OcType);
            //    datarow["Premium"] = row.CE.lastPrice.ToString();
            //    datarow["ExpiryDate"] = row.CE.expiryDate;
            //    datarow["TradingSymbol"] = string.Concat(row.CE.underlying, formattedDateForTradingSymbol, row.CE.strikePrice.ToString(), enumContractType.CE.ToString());

            //    foreach (var item in filteredStrikePrices)
            //    {
            //        datarow[dt.Columns[item.ToString()].ColumnName] = FO.CallSell(row.CE.strikePrice, row.CE.lastPrice, Convert.ToDouble(item));
            //    }
            //    dt.Rows.Add(datarow);
            //}

            //if (row.PE != null &&
            //    (row.PE.strikePrice <= iUpperStrikePriceRange && row.PE.strikePrice >= iLowerStrikePriceRange) &&
            //    (row.PE.strikePrice % 100 == 0) &&
            //    row.PE.lastPrice > 0 &&
            //    //row.expiryDate.Equals(filterConditions.ExpiryDate) &&
            //    (filterConditions.ContractType.Equals(enumContractType.PE.ToString()) ||
            //    filterConditions.ContractType.Equals("ALL")))
            //{
            //    //Add PE Buy row
            //    datarow = dt.NewRow();
            //    datarow["Stock"] = row.PE.underlying;
            //    datarow["Identifier"] = row.PE.identifier;
            //    datarow["Contract"] = enumContractType.PE.ToString();
            //    datarow["TransactionType"] = enumTransactionType.BUY.ToString();
            //    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
            //    datarow["LotSize"] = GetLotSize(filterConditions.OcType);
            //    datarow["Premium"] = row.PE.lastPrice.ToString();
            //    datarow["ExpiryDate"] = row.PE.expiryDate;
            //    datarow["TradingSymbol"] = string.Concat(row.PE.underlying, formattedDateForTradingSymbol, row.PE.strikePrice.ToString(), enumContractType.PE.ToString());

            //    foreach (var item in filteredStrikePrices)
            //    {
            //        datarow[dt.Columns[item.ToString()].ColumnName] = FO.PutBuy(row.PE.strikePrice, row.PE.lastPrice, Convert.ToDouble(item));
            //    }
            //    dt.Rows.Add(datarow);

            //    //Add PE Sell row
            //    datarow = dt.NewRow();
            //    datarow["Stock"] = row.PE.underlying;
            //    datarow["Identifier"] = row.PE.identifier;
            //    datarow["Contract"] = enumContractType.PE.ToString();
            //    datarow["TransactionType"] = enumTransactionType.SELL.ToString();
            //    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
            //    datarow["LotSize"] = GetLotSize(filterConditions.OcType);
            //    datarow["Premium"] = row.PE.lastPrice.ToString();
            //    datarow["ExpiryDate"] = row.PE.expiryDate;
            //    datarow["TradingSymbol"] = string.Concat(row.PE.underlying, formattedDateForTradingSymbol, row.PE.strikePrice.ToString(), enumContractType.PE.ToString());

            //    foreach (var item in filteredStrikePrices)
            //    {
            //        datarow[dt.Columns[item.ToString()].ColumnName] = FO.PutSell(row.PE.strikePrice, row.PE.lastPrice, Convert.ToDouble(item));
            //    }
            //    dt.Rows.Add(datarow);
            //}
            //}
            //return dt;
        }

        public static DataTable AddAllRecordsToDataTable(string OcType)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Stock");
            dt.Columns.Add("Identifier");
            dt.Columns.Add("TradingSymbol");
            dt.Columns.Add("Contract");
            dt.Columns.Add("TransactionType");
            dt.Columns.Add("StrikePrice");
            dt.Columns.Add("LotSize");
            dt.Columns.Add("Premium");
            dt.Columns.Add("ExpiryDate");
            dt.Columns.Add("WeeksToExpiry");
            dt.Columns.Add("IntrinsicValue");

            Records recordsObject = GetOC(OcType);

            GlobalFilterConditions globalFilter = new GlobalFilterConditions();
            if (MySession.Current.GlobalFilterConditions == null)
            {
                globalFilter = new GlobalFilterConditions();
                MySession.Current.GlobalFilterConditions = globalFilter;
            }

            int hide50MultimpleStrikePrice = 0;
            if (globalFilter.Hide50MulSP)
                hide50MultimpleStrikePrice = 100;

            //int iUpperStrikePriceRange = 0;
            //int iLowerStrikePriceRange = 0;

            //if (filterConditions.PercentageRange > 0)
            //{
            //    iUpperStrikePriceRange = RoundTo100(recordsObject.underlyingValue + (DefaultSP(filterConditions.OcType) * filterConditions.PercentageRange / 100));
            //    iLowerStrikePriceRange = RoundTo100(recordsObject.underlyingValue - (DefaultSP(filterConditions.OcType) * filterConditions.PercentageRange / 100));
            //}
            //else
            //{
            //    iUpperStrikePriceRange = filterConditions.SPHigherRange;
            //    iLowerStrikePriceRange = filterConditions.SPLowerRange;
            //}

            //List<int> strikePrices = MySession.Current.RecordsObject.strikePrices;
            //List<int> filteredStrikePrices = new List<int>();

            foreach (var item in MySession.Current.RecordsObject.strikePrices)
            {
                //if (item <= iUpperStrikePriceRange && item >= iLowerStrikePriceRange)
                //{
                //filteredStrikePrices.Add(item);
                dt.Columns.Add(item.ToString());
                //}
            }

            DataRow datarow;
            foreach (var row in recordsObject.data)
            {
                string formattedDateForTradingSymbol = TradingSymbol_DateFormatter(row.expiryDate);
                int weeksToExpiry = DateTime.Parse(row.expiryDate).Subtract(DateTime.Now).Days / 7;

                if (row.CE != null &&
                    (row.CE.strikePrice % hide50MultimpleStrikePrice == 0) &&
                    row.CE.lastPrice > 0
                    )
                {
                    //Add CE Buy row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.CE.underlying;
                    datarow["Identifier"] = row.CE.identifier;
                    datarow["Contract"] = enumContractType.CE.ToString();
                    datarow["TransactionType"] = enumTransactionType.BUY.ToString();
                    datarow["StrikePrice"] = row.CE.strikePrice;
                    datarow["LotSize"] = GetLotSize(OcType);
                    datarow["Premium"] = row.CE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.CE.expiryDate;
                    datarow["TradingSymbol"] = string.Concat(row.CE.underlying, formattedDateForTradingSymbol, row.CE.strikePrice.ToString(), enumContractType.CE.ToString());
                    datarow["WeeksToExpiry"] = weeksToExpiry;
                    datarow["IntrinsicValue"] = row.CE.lastPrice - Convert.ToDouble(row.CE.strikePrice);

                    dt.Rows.Add(datarow);

                    //Add CE Sell row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.CE.underlying;
                    datarow["Identifier"] = row.CE.identifier;
                    datarow["Contract"] = enumContractType.CE.ToString();
                    datarow["TransactionType"] = enumTransactionType.SELL.ToString();
                    datarow["StrikePrice"] = row.CE.strikePrice;
                    datarow["LotSize"] = GetLotSize(OcType);
                    datarow["Premium"] = row.CE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.CE.expiryDate;
                    datarow["TradingSymbol"] = string.Concat(row.CE.underlying, formattedDateForTradingSymbol, row.CE.strikePrice.ToString(), enumContractType.CE.ToString());
                    datarow["WeeksToExpiry"] = weeksToExpiry;

                    dt.Rows.Add(datarow);
                }

                if (row.PE != null &&
                    (row.PE.strikePrice % hide50MultimpleStrikePrice == 0) &&
                    row.PE.lastPrice > 0
                    )
                {
                    //Add PE Buy row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.PE.underlying;
                    datarow["Identifier"] = row.PE.identifier;
                    datarow["Contract"] = enumContractType.PE.ToString();
                    datarow["TransactionType"] = enumTransactionType.BUY.ToString();
                    datarow["StrikePrice"] = row.PE.strikePrice;
                    datarow["LotSize"] = GetLotSize(OcType);
                    datarow["Premium"] = row.PE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.PE.expiryDate;
                    datarow["TradingSymbol"] = string.Concat(row.PE.underlying, formattedDateForTradingSymbol, row.PE.strikePrice.ToString(), enumContractType.PE.ToString());
                    datarow["WeeksToExpiry"] = weeksToExpiry;

                    dt.Rows.Add(datarow);

                    //Add PE Sell row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.PE.underlying;
                    datarow["Identifier"] = row.PE.identifier;
                    datarow["Contract"] = enumContractType.PE.ToString();
                    datarow["TransactionType"] = enumTransactionType.SELL.ToString();
                    datarow["StrikePrice"] = row.PE.strikePrice;
                    datarow["LotSize"] = GetLotSize(OcType);
                    datarow["Premium"] = row.PE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.PE.expiryDate;
                    datarow["TradingSymbol"] = string.Concat(row.PE.underlying, formattedDateForTradingSymbol, row.PE.strikePrice.ToString(), enumContractType.PE.ToString());
                    datarow["WeeksToExpiry"] = weeksToExpiry;

                    dt.Rows.Add(datarow);
                }
            }
            return dt;
        }

        private static int GetWeeksToExpiry(string expiryDate)
        {
            //DateTime expDate = DateTime.Parse(expiryDate);
            int dateDiff = DateTime.Now.Subtract(DateTime.Parse(expiryDate)).Days;
            //int dateDiff = ts.Days;
            return dateDiff;
        }

        private static string TradingSymbol_DateFormatter(string expiryDate)
        {
            bool isMonthEndExpiry = true;
            string formattedDate = string.Empty;
            DateTime dateTime = DateTime.Parse(expiryDate);

            foreach (var item in MySession.Current.RecordsObject.expiryDates)
            {
                if (DateTime.Parse(item) == dateTime)
                {
                    foreach (var newitem in MySession.Current.RecordsObject.expiryDates)
                    {
                        if (DateTime.Parse(newitem).Date > dateTime.Date &&
                            DateTime.Parse(newitem).Month == dateTime.Date.Month &&
                            DateTime.Parse(newitem).Year == dateTime.Date.Year)
                        {
                            isMonthEndExpiry = false;
                        }
                    }
                }
            }

            if (isMonthEndExpiry)
                formattedDate = DateTime.Parse(expiryDate).ToString("yyMMM").ToUpper();
            else
                formattedDate = DateTime.Parse(expiryDate).ToString("yyMdd").ToUpper();


            return formattedDate;
        }

        public static Records GetOC(string ocType)
        {
            Records recordsObject = new Records();

            if (MySession.Current.RecordsObject != null)
            {
                if (MySession.Current.RecordsObject.index == null)
                    MySession.Current.RecordsNifty = MySession.Current.RecordsObject;
                else if (MySession.Current.RecordsObject.index.indexSymbol.Equals("NIFTY BANK"))
                    MySession.Current.RecordsBankNifty = MySession.Current.RecordsObject;

            }

            if (ocType.Equals(enumOptionChainType.NIFTY.ToString()))
            {
                if (MySession.Current.RecordsNifty == null)
                {
                    JObject jObject = DownloadJSONDataFromURL(ocType);
                    if (jObject != null && !jObject.ToString().Equals("{}"))
                    {
                        recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                        MySession.Current.RecordsNifty = recordsObject;
                    }
                }
                else
                {
                    recordsObject = MySession.Current.RecordsNifty;
                    if (FetchOCAgain(recordsObject))
                    {
                        JObject jObject = DownloadJSONDataFromURL(ocType);
                        if (jObject != null && !jObject.ToString().Equals("{}"))
                        {
                            recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                            MySession.Current.RecordsNifty = recordsObject;
                        }
                    }
                }

                if (MySession.Current.RecordsNifty != null)
                    MySession.Current.RecordsObject = MySession.Current.RecordsNifty;
            }

            if (ocType.Equals(enumOptionChainType.BANKNIFTY.ToString()))
            {
                if (MySession.Current.RecordsBankNifty == null)
                {
                    JObject jObject = DownloadJSONDataFromURL(ocType);
                    if (jObject != null && !jObject.ToString().Equals("{}"))
                    {
                        recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                        MySession.Current.RecordsBankNifty = recordsObject;
                    }
                }
                else
                {
                    recordsObject = MySession.Current.RecordsBankNifty;
                    if (FetchOCAgain(recordsObject))
                    {
                        JObject jObject = DownloadJSONDataFromURL(ocType);
                        if (jObject != null && !jObject.ToString().Equals("{}"))
                        {
                            recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                            MySession.Current.RecordsBankNifty = recordsObject;
                        }
                    }
                }

                if (MySession.Current.RecordsBankNifty != null)
                    MySession.Current.RecordsObject = MySession.Current.RecordsBankNifty;
            }
            return recordsObject;
        }

        private static bool FetchOCAgain(Records recordsObject)
        {
            TimeSpan timeAddGap = new TimeSpan(0, 3, 0);
            TimeSpan timeMktOpenTiming = new TimeSpan(9, 15, 0);
            TimeSpan timeMktCloseTiming = new TimeSpan(15, 30, 0);
            bool flag = false;
            DateTime timeLastFetchedTime = DateTime.ParseExact(recordsObject.timestamp, "dd-MMM-yyyy HH:mm:ss", null);

            //Few new OC only if current time is between market open and close time and 
            //current time is more than 5 mins from last fetched time and
            //Current day is a weekday
            if (DateTime.Now.TimeOfDay > timeLastFetchedTime.TimeOfDay.Add(timeAddGap) &&
                DateTime.Now.TimeOfDay > timeMktOpenTiming &&
                DateTime.Now.TimeOfDay < timeMktCloseTiming &&
                (!DateTime.Now.DayOfWeek.ToString().Equals(DayOfWeek.Saturday) &&
                !DateTime.Now.DayOfWeek.ToString().Equals(DayOfWeek.Sunday)))
            {
                flag = true;
            }
            return flag;
        }

        public static Records.Datum.CE1 GetCE(string ocType, string expiryDate, int strikePrice)
        {
            Records.Datum.CE1 result = null;
            Records recordsObject = GetOC(ocType);

            foreach (Records.Datum datum in recordsObject.data)
            {
                if (datum.expiryDate.Equals(expiryDate) && datum.strikePrice.Equals(strikePrice))
                {
                    if (datum.CE != null)
                        result = datum.CE;
                }
            }
            return result;
        }

        public static Records.Datum.PE1 GetPE(string ocType, string expiryDate, int strikePrice)
        {
            Records.Datum.PE1 result = null;
            Records recordsObject = GetOC(ocType);

            foreach (Records.Datum datum in recordsObject.data)
            {
                if (datum.expiryDate.Equals(expiryDate) && datum.strikePrice.Equals(strikePrice))
                {
                    if (datum.PE != null)
                        result = datum.PE;
                }
            }
            return result;
        }

        public static string FormatTradingSymbol(DataRow dataRow)
        {
            string formattedString = string.Empty;

            return formattedString;
        }
        public static void AddColumnsToOutputGrid(DataTable dtResult)
        {
            dtResult.Columns.Add("Trading Symbol");
            dtResult.Columns.Add("CE/PE");
            dtResult.Columns.Add("Buy/Sell");
            dtResult.Columns.Add("StrikePrice");
            dtResult.Columns.Add("Qty");
            dtResult.Columns.Add("Premium");
            dtResult.Columns.Add("Profit/Loss");
        }

        public static int RoundTo100(double value)
        {
            int result = (int)Math.Round(value / 100);
            if (value > 0 && result == 0)
            {
                result = 1;
            }
            return (int)result * 100;
        }

        public static int DefaultSP(string ocType)
        {
            Records recordsObject = GetOC(ocType);
            double cp = recordsObject.underlyingValue;

            int result = (int)Math.Round(cp / 100);
            if (cp > 0 && result == 0)
            {
                result = 1;
            }
            return (int)result * 100;
        }

        public static string DefaultExpDate(string ocType)
        {
            Records recordsObject = GetOC(ocType);
            List<string> expDates = recordsObject.expiryDates;
            string result = string.Empty;
            if (expDates.Count > 0)
                result = expDates[0];

            return result;
        }

        public static List<string> GetOCExpList(string ocType)
        {
            Records recordsObject = GetOC(ocType);

            List<string> expiryDates = new List<string> { };

            if (recordsObject.expiryDates != null)
                expiryDates = recordsObject.expiryDates;

            return expiryDates;
        }

        public static List<int> GetOCSPList(string ocType)
        {
            Records recordsObject = GetOC(ocType);

            List<int> strikePrices = new List<int> { };

            if (recordsObject.expiryDates != null)
                strikePrices = recordsObject.strikePrices;

            return strikePrices;
        }

        private static JObject DownloadJSONDataFromURL(string ocType)
        {
            string webResourceURL = url + ocType;
            JObject jObject = new JObject();
            try
            {
                using (var webClient = new WebClient())
                {
                    // Set headers to download the data
                    webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                    webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                    //webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");

                    // Download the data
                    string stockWatchJSONString = webClient.DownloadString(webResourceURL);

                    //DataTable dt = toDataTable(stockWatchJSONString);

                    // Serialise it into a JObject
                    jObject = JObject.Parse(stockWatchJSONString);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return jObject;
        }

        public static void FillAllStrikePrice(string ocType, DropDownList ddlSP, bool IsAddAll)
        {
            if (IsAddAll)
                ddlSP.Items.Add("All");

            List<int> expiryDates = OCHelper.GetOCSPList(ocType);
            foreach (int item in expiryDates)
            {
                ddlSP.Items.Add(item.ToString());
            }
        }

        public static void FillAllExpiryDates(string ocType, DropDownList ddlExp, bool IsAddAll)
        {
            if (IsAddAll)
                ddlExp.Items.Add("All");

            List<string> expiryDates = OCHelper.GetOCExpList(ocType);
            foreach (string item in expiryDates)
            {
                ddlExp.Items.Add(item);
            }
        }

        private static DataTable toDataTable(Filtered filteredObject)
        {
            var result = new DataTable();

            result = addDataTableColumns(result);

            foreach (var row in filteredObject.data)
            {
                var datarow = result.NewRow();
                if (row.CE != null)
                {
                    datarow["StrikePrice"] = row.CE.strikePrice;
                    datarow["ExpiryDate"] = row.CE.expiryDate.ToString();
                    datarow["CE" + "underlying"] = row.CE.underlying.ToString();
                    datarow["CE" + "identifier"] = row.CE.identifier.ToString();
                    datarow["CE" + "openInterest"] = row.CE.openInterest.ToString();
                    datarow["CE" + "changeinOpenInterest"] = row.CE.changeinOpenInterest.ToString();
                    datarow["CE" + "pchangeinOpenInterest"] = row.CE.pchangeinOpenInterest.ToString();
                    datarow["CE" + "totalTradedVolume"] = row.CE.totalTradedVolume.ToString();
                    datarow["CE" + "impliedVolatility"] = row.CE.impliedVolatility.ToString();
                    datarow["CE" + "lastPrice"] = row.CE.lastPrice.ToString();
                    datarow["CE" + "change"] = Math.Round(row.CE.change, 2).ToString();
                    datarow["CE" + "pChange"] = row.CE.pChange.ToString();
                    datarow["CE" + "totalBuyQuantity"] = row.CE.totalBuyQuantity.ToString();
                    datarow["CE" + "totalSellQuantity"] = row.CE.totalSellQuantity.ToString();
                    datarow["CE" + "bidQty"] = row.CE.bidQty.ToString();
                    datarow["CE" + "bidprice"] = row.CE.bidprice.ToString();
                    datarow["CE" + "askQty"] = row.CE.askQty.ToString();
                    datarow["CE" + "askPrice"] = row.CE.askPrice.ToString();
                    datarow["CE" + "underlyingValue"] = row.CE.underlyingValue.ToString();
                }

                if (row.PE != null)
                {
                    datarow["StrikePrice"] = row.PE.strikePrice;
                    datarow["ExpiryDate"] = row.PE.expiryDate.ToString();
                    datarow["PE" + "underlying"] = row.PE.underlying.ToString();
                    datarow["PE" + "identifier"] = row.PE.identifier.ToString();
                    datarow["PE" + "openInterest"] = row.PE.openInterest.ToString();
                    datarow["PE" + "changeinOpenInterest"] = row.PE.changeinOpenInterest.ToString();
                    datarow["PE" + "pchangeinOpenInterest"] = row.PE.pchangeinOpenInterest.ToString();
                    datarow["PE" + "totalTradedVolume"] = row.PE.totalTradedVolume.ToString();
                    datarow["PE" + "impliedVolatility"] = row.PE.impliedVolatility.ToString();
                    datarow["PE" + "lastPrice"] = row.PE.lastPrice.ToString();
                    datarow["PE" + "change"] = Math.Round(row.PE.change, 2).ToString();
                    datarow["PE" + "pChange"] = row.PE.pChange.ToString();
                    datarow["PE" + "totalBuyQuantity"] = row.PE.totalBuyQuantity.ToString();
                    datarow["PE" + "totalSellQuantity"] = row.PE.totalSellQuantity.ToString();
                    datarow["PE" + "bidQty"] = row.PE.bidQty.ToString();
                    datarow["PE" + "bidprice"] = row.PE.bidprice.ToString();
                    datarow["PE" + "askQty"] = row.PE.askQty.ToString();
                    datarow["PE" + "askPrice"] = row.PE.askPrice.ToString();
                    datarow["PE" + "underlyingValue"] = row.PE.underlyingValue.ToString();
                }

                result.Rows.Add(datarow);
            }

            return result;
        }

        public static string GetFilterString(FilterConditions filterConditions)
        {
            int iUpperStrikePriceRange = 0;
            int iLowerStrikePriceRange = 0;

            if (filterConditions.PercentageRange > 0)
            {
                iUpperStrikePriceRange = RoundTo100(MySession.Current.RecordsObject.underlyingValue + (DefaultSP(filterConditions.OcType) * filterConditions.PercentageRange / 100));
                iLowerStrikePriceRange = RoundTo100(MySession.Current.RecordsObject.underlyingValue - (DefaultSP(filterConditions.OcType) * filterConditions.PercentageRange / 100));
            }
            else
            {
                iUpperStrikePriceRange = filterConditions.SPHigherRange;
                iLowerStrikePriceRange = filterConditions.SPLowerRange;
            }

            string strFilter = string.Empty;
            List<string> filters = new List<string>();

            if (filterConditions.ExpiryDate != null && !filterConditions.ExpiryDate.ToUpper().Equals("ALL"))
                filters.Add(" (ExpiryDate = #" + filterConditions.ExpiryDate + "#) ");

            if (filterConditions.ShowSPBetweenLowHigh)
            {
                filters.Add(" ((StrikePrice >= " + iLowerStrikePriceRange + ") AND (StrikePrice <= " + iUpperStrikePriceRange + ")) ");
            }
            else
            {
                filters.Add(" ((StrikePrice <= " + iLowerStrikePriceRange + ") OR (StrikePrice >= " + iUpperStrikePriceRange + ")) ");
            }

            if (filterConditions.ContractType != null && !filterConditions.ContractType.ToUpper().Equals("ALL"))
                filters.Add(" (Contract = '" + filterConditions.ContractType + "') ");

            if (filterConditions.TransactionType != null && !filterConditions.TransactionType.ToUpper().Equals("ALL"))
                filters.Add(" (TransactionType = '" + filterConditions.TransactionType + "') ");

            for (int index = 0; index < filters.Count; index++)
            {
                if (index < filters.Count - 1)
                {
                    strFilter += filters[index] + " AND ";
                }
                else
                {
                    strFilter += filters[index];
                }
            }

            return strFilter;
        }

        public static void GetGlobalFilterConditions()
        {
            GlobalFilterConditions globalFilterConditions = new GlobalFilterConditions();

            //These values should actually be fetched from the database.
            globalFilterConditions.Hide50MulSP = true;
            globalFilterConditions.Hide1SDSP = true;
            globalFilterConditions.Hide2SDSP = true;

            MySession.Current.GlobalFilterConditions = globalFilterConditions;
        }

        public static DataTable toDataTable(Records recordsObject)
        {
            var result = new DataTable();

            result = addDataTableColumns(result);

            foreach (var row in recordsObject.data)
            {
                //string formattedDateForTradingSymbol = TradingSymbol_DateFormatter(row.expiryDate);
                var datarow = result.NewRow();
                if (row.CE != null && row.CE.strikePrice % 100 == 0 && row.CE.lastPrice > 0)
                {
                    datarow["StrikePrice"] = row.CE.strikePrice;
                    datarow["ExpiryDate"] = row.CE.expiryDate.ToString();
                    datarow["CE" + "underlying"] = row.CE.underlying.ToString();
                    datarow["CE" + "identifier"] = row.CE.identifier.ToString();
                    datarow["CE" + "openInterest"] = row.CE.openInterest.ToString();
                    datarow["CE" + "changeinOpenInterest"] = row.CE.changeinOpenInterest.ToString();
                    datarow["CE" + "pchangeinOpenInterest"] = row.CE.pchangeinOpenInterest.ToString();
                    datarow["CE" + "totalTradedVolume"] = row.CE.totalTradedVolume.ToString();
                    datarow["CE" + "impliedVolatility"] = row.CE.impliedVolatility.ToString();
                    datarow["CE" + "lastPrice"] = row.CE.lastPrice.ToString();
                    datarow["CE" + "change"] = Math.Round(row.CE.change, 2).ToString();
                    datarow["CE" + "pChange"] = row.CE.pChange.ToString();
                    datarow["CE" + "totalBuyQuantity"] = row.CE.totalBuyQuantity.ToString();
                    datarow["CE" + "totalSellQuantity"] = row.CE.totalSellQuantity.ToString();
                    datarow["CE" + "bidQty"] = row.CE.bidQty.ToString();
                    datarow["CE" + "bidprice"] = row.CE.bidprice.ToString();
                    datarow["CE" + "askQty"] = row.CE.askQty.ToString();
                    datarow["CE" + "askPrice"] = row.CE.askPrice.ToString();
                    datarow["CE" + "underlyingValue"] = row.CE.underlyingValue.ToString();
                    datarow["Contract"] = enumContractType.CE.ToString();
                    datarow["CETradingSymbol"] = string.Concat(row.CE.underlying, TradingSymbol_DateFormatter(row.expiryDate), row.CE.strikePrice.ToString(), enumContractType.CE.ToString());
                }

                if (row.PE != null && row.PE.strikePrice % 100 == 0 && row.PE.lastPrice > 0)
                {
                    datarow["StrikePrice"] = row.PE.strikePrice;
                    datarow["ExpiryDate"] = row.PE.expiryDate.ToString();
                    datarow["PE" + "underlying"] = row.PE.underlying.ToString();
                    datarow["PE" + "identifier"] = row.PE.identifier.ToString();
                    datarow["PE" + "openInterest"] = row.PE.openInterest.ToString();
                    datarow["PE" + "changeinOpenInterest"] = row.PE.changeinOpenInterest.ToString();
                    datarow["PE" + "pchangeinOpenInterest"] = row.PE.pchangeinOpenInterest.ToString();
                    datarow["PE" + "totalTradedVolume"] = row.PE.totalTradedVolume.ToString();
                    datarow["PE" + "impliedVolatility"] = row.PE.impliedVolatility.ToString();
                    datarow["PE" + "lastPrice"] = row.PE.lastPrice.ToString();
                    datarow["PE" + "change"] = Math.Round(row.PE.change, 2).ToString();
                    datarow["PE" + "pChange"] = row.PE.pChange.ToString();
                    datarow["PE" + "totalBuyQuantity"] = row.PE.totalBuyQuantity.ToString();
                    datarow["PE" + "totalSellQuantity"] = row.PE.totalSellQuantity.ToString();
                    datarow["PE" + "bidQty"] = row.PE.bidQty.ToString();
                    datarow["PE" + "bidprice"] = row.PE.bidprice.ToString();
                    datarow["PE" + "askQty"] = row.PE.askQty.ToString();
                    datarow["PE" + "askPrice"] = row.PE.askPrice.ToString();
                    datarow["PE" + "underlyingValue"] = row.PE.underlyingValue.ToString();
                    datarow["Contract"] = enumContractType.PE.ToString();
                    datarow["PETradingSymbol"] = string.Concat(row.PE.underlying, TradingSymbol_DateFormatter(row.expiryDate), row.PE.strikePrice.ToString(), enumContractType.PE.ToString());
                }
                if ((row.PE != null && row.PE.strikePrice % 100 == 0 && row.PE.lastPrice > 0) ||
                    row.CE != null && row.CE.strikePrice % 100 == 0 && row.CE.lastPrice > 0)
                {
                    result.Rows.Add(datarow);
                }
            }

            return result;
        }

        private static DataTable addDataTableColumns(DataTable result)
        {
            Records.Datum.CE1 ce = new Records.Datum.CE1();

            result.Columns.Add("CE" + "expiryDate");
            result.Columns.Add("CE" + "underlying");
            result.Columns.Add("CE" + "identifier");
            result.Columns.Add("CE" + "pchangeinOpenInterest");
            result.Columns.Add("CE" + "impliedVolatility");
            result.Columns.Add("CE" + "totalBuyQuantity");
            result.Columns.Add("CE" + "totalSellQuantity");
            result.Columns.Add("CE" + "underlyingValue");
            result.Columns.Add("CE" + "pChange");
            result.Columns.Add("Contract");
            result.Columns.Add("CE" + "openInterest");
            result.Columns.Add("CE" + "changeinOpenInterest");
            result.Columns.Add("CE" + "totalTradedVolume");
            result.Columns.Add("CE" + "lastPrice");
            result.Columns.Add("CE" + "change");
            result.Columns.Add("CE" + "bidQty");
            result.Columns.Add("CE" + "bidprice");
            result.Columns.Add("CE" + "askPrice");
            result.Columns.Add("CE" + "askQty");
            result.Columns.Add("StrikePrice");
            result.Columns.Add("CETradingSymbol");

            Records.Datum.PE1 pe = new Records.Datum.PE1();
            //result.Columns.Add("strikePrice");
            result.Columns.Add("PE" + "bidQty");
            result.Columns.Add("PE" + "bidprice");
            result.Columns.Add("PE" + "askPrice");
            result.Columns.Add("PE" + "askQty");
            result.Columns.Add("PE" + "change");
            result.Columns.Add("PE" + "lastPrice");
            result.Columns.Add("PE" + "totalTradedVolume");
            result.Columns.Add("PE" + "changeinOpenInterest");
            result.Columns.Add("PE" + "pchangeinOpenInterest");

            result.Columns.Add("PE" + "pChange");
            result.Columns.Add("ExpiryDate");
            result.Columns.Add("PE" + "underlying");
            result.Columns.Add("PE" + "identifier");
            result.Columns.Add("PE" + "openInterest");
            result.Columns.Add("PE" + "impliedVolatility");
            result.Columns.Add("PE" + "totalBuyQuantity");
            result.Columns.Add("PE" + "totalSellQuantity");
            result.Columns.Add("PE" + "underlyingValue");
            result.Columns.Add("PETradingSymbol");

            return result;
        }

        public static int GetColumnIndexByName(GridViewRow row, string SearchColumnName)
        {
            int columnIndex = 0;
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.ContainingField is BoundField)
                {
                    if (((BoundField)cell.ContainingField).DataField.Equals(SearchColumnName))
                    {
                        break;
                    }
                }
                columnIndex++;
            }
            return columnIndex;
        }
    }
}