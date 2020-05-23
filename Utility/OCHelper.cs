using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

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

        public static DataTable AddRecordsToDataTable(string ocType, int iPercentageRange)
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

            Records recordsObject = GetOC(ocType);

            int iUpperStrikePriceRange = RoundTo100(recordsObject.underlyingValue + (DefaultSP(ocType) * iPercentageRange / 100));
            int iLowerStrikePriceRange = RoundTo100(recordsObject.underlyingValue - (DefaultSP(ocType) * iPercentageRange / 100));

            List<int> strikePrices = MySession.Current.RecordsObject.strikePrices;
            List<int> filteredStrikePrices = new List<int>();

            foreach (var item in strikePrices)
            {
                if (item <= iUpperStrikePriceRange && item >= iLowerStrikePriceRange)
                {
                    filteredStrikePrices.Add(item);
                    dt.Columns.Add(item.ToString());
                }
            }

            DataRow datarow;
            foreach (var row in recordsObject.data)
            {
                string formattedDateForTradingSymbol = TradingSymbol_DateFormatter(row.expiryDate);
                if (row.CE != null &&
                    (row.CE.strikePrice <= iUpperStrikePriceRange && row.CE.strikePrice >= iLowerStrikePriceRange) &&
                    (row.CE.strikePrice % 100 == 0)
                    && row.CE.lastPrice > 0)
                {
                    //Add CE Buy row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.CE.underlying;
                    datarow["Identifier"] = row.CE.identifier;
                    datarow["Contract"] = enumContractType.CE.ToString();
                    datarow["TransactionType"] = enumTransactionType.BUY.ToString();
                    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
                    datarow["LotSize"] = GetLotSize(ocType);
                    datarow["Premium"] = row.CE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.CE.expiryDate;
                    datarow["TradingSymbol"] = string.Concat(row.CE.underlying, formattedDateForTradingSymbol, row.CE.strikePrice.ToString(), enumContractType.CE.ToString());

                    foreach (var item in filteredStrikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = FO.CallBuy(row.CE.strikePrice, row.CE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);

                    //Add CE Sell row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.CE.underlying;
                    datarow["Identifier"] = row.CE.identifier;
                    datarow["Contract"] = enumContractType.CE.ToString();
                    datarow["TransactionType"] = enumTransactionType.SELL.ToString();
                    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
                    datarow["LotSize"] = GetLotSize(ocType);
                    datarow["Premium"] = row.CE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.CE.expiryDate;
                    datarow["TradingSymbol"] = string.Concat(row.CE.underlying, formattedDateForTradingSymbol, row.CE.strikePrice.ToString(), enumContractType.CE.ToString());

                    foreach (var item in filteredStrikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = FO.CallSell(row.CE.strikePrice, row.CE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);
                }

                if (row.PE != null &&
                    (row.PE.strikePrice <= iUpperStrikePriceRange && row.PE.strikePrice >= iLowerStrikePriceRange) &&
                    (row.PE.strikePrice % 100 == 0) &&
                    row.PE.lastPrice > 0)
                {
                    //Add PE Buy row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.PE.underlying;
                    datarow["Identifier"] = row.PE.identifier;
                    datarow["Contract"] = enumContractType.PE.ToString();
                    datarow["TransactionType"] = enumTransactionType.BUY.ToString();
                    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
                    datarow["LotSize"] = GetLotSize(ocType);
                    datarow["Premium"] = row.PE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.PE.expiryDate;
                    datarow["TradingSymbol"] = string.Concat(row.PE.underlying, formattedDateForTradingSymbol, row.PE.strikePrice.ToString(), enumContractType.PE.ToString());

                    foreach (var item in filteredStrikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = FO.PutBuy(row.PE.strikePrice, row.PE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);

                    //Add PE Sell row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.PE.underlying;
                    datarow["Identifier"] = row.PE.identifier;
                    datarow["Contract"] = enumContractType.PE.ToString();
                    datarow["TransactionType"] = enumTransactionType.SELL.ToString();
                    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
                    datarow["LotSize"] = GetLotSize(ocType);
                    datarow["Premium"] = row.PE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.PE.expiryDate;
                    datarow["TradingSymbol"] = string.Concat(row.PE.underlying, formattedDateForTradingSymbol, row.PE.strikePrice.ToString(), enumContractType.PE.ToString());

                    foreach (var item in filteredStrikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = FO.PutSell(row.PE.strikePrice, row.PE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);
                }
            }
            return dt;
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
                    if (jObject != null && jObject.Count > 0)
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
                        if (jObject != null)
                        {
                            recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                            MySession.Current.RecordsNifty = recordsObject;
                        }
                    }
                }
                MySession.Current.RecordsObject = MySession.Current.RecordsNifty;
            }

            if (ocType.Equals(enumOptionChainType.BANKNIFTY.ToString()))
            {
                if (MySession.Current.RecordsBankNifty == null)
                {
                    JObject jObject = DownloadJSONDataFromURL(ocType);
                    if (jObject != null)
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
                        if (jObject != null)
                        {
                            recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                            MySession.Current.RecordsBankNifty = recordsObject;
                        }
                    }
                }
                MySession.Current.RecordsObject = MySession.Current.RecordsBankNifty;
            }
            return recordsObject;
        }

        private static bool FetchOCAgain(Records recordsObject)
        {
            TimeSpan timeAddGap = new TimeSpan(0, 2, 0);
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
            dtResult.Columns.Add("Strike Price");
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

        public void FillAllStrikePrice(string ocType, System.Web.UI.WebControls.DropDownList ddlSP)
        {
            List<int> expiryDates = OCHelper.GetOCSPList(ocType);
            foreach (int item in expiryDates)
            {
                ddlSP.Items.Add(item.ToString());
            }
        }

        public void FillAllExpiryDates(string ocType, System.Web.UI.WebControls.DropDownList ddlExpDt)
        {
            List<string> expiryDates = OCHelper.GetOCExpList(ocType);
            foreach (string item in expiryDates)
            {
                ddlExpDt.Items.Add(item);
            }
        }
    }
}