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
            if (ocType.Equals(enumOCType.NIFTY.ToString()))
                lotsSize = Convert.ToInt32(enumLotSize.Nifty);
            if (ocType.Equals(enumOCType.BANKNIFTY.ToString()))
                lotsSize = Convert.ToInt32(enumLotSize.BankNifty);

            return lotsSize;
        }

        public static DataTable FilterDataTableRecords(FilterConditions filterConditions)
        {
            DataTable allRecordsDt = AddAllRecordsToDataTable(filterConditions.OCType);

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

            string strFilter = GetFilterString(filterConditions);

            DataRow[] drs = allRecordsDt.Select(strFilter, "StrikePrice ASC");
            DataTable filteredDataTable = allRecordsDt.Clone();
            foreach (DataRow dr in drs)
            {
                filteredDataTable.ImportRow(dr);
            }

            return filteredDataTable;
        }

        public static DataTable AddAllRecordsToDataTable(string OcType)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(enumStrategyColumns.Stock.ToString(), typeof(string));
            dt.Columns.Add(enumStrategyColumns.Identifier.ToString(), typeof(string));
            dt.Columns.Add(enumStrategyColumns.TradingSymbol.ToString(), typeof(string));
            dt.Columns.Add(enumStrategyColumns.ContractType.ToString(), typeof(string));
            dt.Columns.Add(enumStrategyColumns.TransactionType.ToString(), typeof(string));
            dt.Columns.Add(enumStrategyColumns.StrikePrice.ToString(), typeof(int));
            dt.Columns.Add(enumStrategyColumns.LotSize.ToString(), typeof(int));
            dt.Columns.Add(enumStrategyColumns.Premium.ToString(), typeof(double));
            dt.Columns.Add(enumStrategyColumns.ExpiryDate.ToString(), typeof(string));
            dt.Columns.Add(enumStrategyColumns.WeeksToExpiry.ToString(), typeof(int));
            dt.Columns.Add(enumStrategyColumns.IntrinsicValue.ToString(), typeof(double));

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

            foreach (var item in MySession.Current.RecordsObject.strikePrices)
            {
                dt.Columns.Add(item.ToString());
            }

            DataRow datarow;
            foreach (var row in recordsObject.data)
            {
                string formattedDateForTradingSymbol = TradingSymbol_DateFormatter(row.expiryDate);
                int weeksToExpiry = DateTime.Parse(row.expiryDate).Subtract(DateTime.Now).Days / 7;

                if (row.CE != null && (row.CE.strikePrice % hide50MultimpleStrikePrice == 0) && row.CE.lastPrice > 0)
                {
                    //Add CE Buy row
                    datarow = dt.NewRow();
                    datarow[enumStrategyColumns.Stock.ToString()] = row.CE.underlying;
                    datarow[enumStrategyColumns.Identifier.ToString()] = row.CE.identifier;
                    datarow[enumStrategyColumns.ContractType.ToString()] = enumContractType.CE;
                    datarow[enumStrategyColumns.TransactionType.ToString()] = enumTransactionType.BUY;
                    datarow[enumStrategyColumns.StrikePrice.ToString()] = row.CE.strikePrice;
                    datarow[enumStrategyColumns.LotSize.ToString()] = GetLotSize(OcType);
                    datarow[enumStrategyColumns.Premium.ToString()] = row.CE.lastPrice;
                    datarow[enumStrategyColumns.ExpiryDate.ToString()] = row.CE.expiryDate;
                    datarow[enumStrategyColumns.TradingSymbol.ToString()] = string.Concat(row.CE.underlying, formattedDateForTradingSymbol, row.CE.strikePrice.ToString(), enumContractType.CE.ToString());
                    datarow[enumStrategyColumns.WeeksToExpiry.ToString()] = weeksToExpiry;
                    datarow[enumStrategyColumns.IntrinsicValue.ToString()] = row.CE.lastPrice - Convert.ToDouble(row.CE.strikePrice);

                    dt.Rows.Add(datarow);

                    //Add CE Sell row
                    datarow = dt.NewRow();
                    datarow[enumStrategyColumns.Stock.ToString()] = row.CE.underlying;
                    datarow[enumStrategyColumns.Identifier.ToString()] = row.CE.identifier;
                    datarow[enumStrategyColumns.ContractType.ToString()] = enumContractType.CE;
                    datarow[enumStrategyColumns.TransactionType.ToString()] = enumTransactionType.SELL;
                    datarow[enumStrategyColumns.StrikePrice.ToString()] = row.CE.strikePrice;
                    datarow[enumStrategyColumns.LotSize.ToString()] = GetLotSize(OcType);
                    datarow[enumStrategyColumns.Premium.ToString()] = row.CE.lastPrice;
                    datarow[enumStrategyColumns.ExpiryDate.ToString()] = row.CE.expiryDate;
                    datarow[enumStrategyColumns.TradingSymbol.ToString()] = string.Concat(row.CE.underlying, formattedDateForTradingSymbol, row.CE.strikePrice.ToString(), enumContractType.CE.ToString());
                    datarow[enumStrategyColumns.WeeksToExpiry.ToString()] = weeksToExpiry;

                    dt.Rows.Add(datarow);
                }

                if (row.PE != null && (row.PE.strikePrice % hide50MultimpleStrikePrice == 0) && row.PE.lastPrice > 0)
                {
                    //Add PE Buy row
                    datarow = dt.NewRow();
                    datarow[enumStrategyColumns.Stock.ToString()] = row.PE.underlying;
                    datarow[enumStrategyColumns.Identifier.ToString()] = row.PE.identifier;
                    datarow[enumStrategyColumns.ContractType.ToString()] = enumContractType.PE;
                    datarow[enumStrategyColumns.TransactionType.ToString()] = enumTransactionType.BUY;
                    datarow[enumStrategyColumns.StrikePrice.ToString()] = row.PE.strikePrice;
                    datarow[enumStrategyColumns.LotSize.ToString()] = GetLotSize(OcType);
                    datarow[enumStrategyColumns.Premium.ToString()] = row.PE.lastPrice;
                    datarow[enumStrategyColumns.ExpiryDate.ToString()] = row.PE.expiryDate;
                    datarow[enumStrategyColumns.TradingSymbol.ToString()] = string.Concat(row.PE.underlying, formattedDateForTradingSymbol, row.PE.strikePrice.ToString(), enumContractType.PE.ToString());
                    datarow[enumStrategyColumns.WeeksToExpiry.ToString()] = weeksToExpiry;

                    dt.Rows.Add(datarow);

                    //Add PE Sell row
                    datarow = dt.NewRow();
                    datarow[enumStrategyColumns.Stock.ToString()] = row.PE.underlying;
                    datarow[enumStrategyColumns.Identifier.ToString()] = row.PE.identifier;
                    datarow[enumStrategyColumns.ContractType.ToString()] = enumContractType.PE;
                    datarow[enumStrategyColumns.TransactionType.ToString()] = enumTransactionType.SELL;
                    datarow[enumStrategyColumns.StrikePrice.ToString()] = row.PE.strikePrice;
                    datarow[enumStrategyColumns.LotSize.ToString()] = GetLotSize(OcType);
                    datarow[enumStrategyColumns.Premium.ToString()] = row.PE.lastPrice;
                    datarow[enumStrategyColumns.ExpiryDate.ToString()] = row.PE.expiryDate;
                    datarow[enumStrategyColumns.TradingSymbol.ToString()] = string.Concat(row.PE.underlying, formattedDateForTradingSymbol, row.PE.strikePrice.ToString(), enumContractType.PE.ToString());
                    datarow[enumStrategyColumns.WeeksToExpiry.ToString()] = weeksToExpiry;

                    dt.Rows.Add(datarow);
                }
            }
            return dt;
        }

        private static int GeDaysToExpiry(string expiryDate)
        {
            int dateDiff = DateTime.Now.Subtract(DateTime.Parse(expiryDate)).Days;
            return dateDiff;
        }

        public static string TradingSymbol_DateFormatter(string expiryDate)
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

            if (ocType.Equals(enumOCType.NIFTY.ToString()))
            {
                if (MySession.Current.RecordsNifty == null)
                {
                    //JObject jObject = DownloadJSONDataFromURL(ocType);
                    //if (jObject == null || jObject.ToString().Equals("{}"))
                    //{
                    //    jObject = GetJObjectFromDB(ocType);
                    //}
                    //recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                    //MySession.Current.RecordsNifty = recordsObject;

                    recordsObject = UpdateRecordsObject(ocType);
                }
                else
                {
                    recordsObject = MySession.Current.RecordsNifty;
                    if (FetchOCAgain(recordsObject))
                    {
                        //JObject jObject = DownloadJSONDataFromURL(ocType);
                        //if (jObject != null && !jObject.ToString().Equals("{}"))
                        //{
                        //    recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                        //    MySession.Current.RecordsNifty = recordsObject;
                        //}
                        //JObject jObject = DownloadJSONDataFromURL(ocType);
                        //if (jObject == null || jObject.ToString().Equals("{}"))
                        //{
                        //    jObject = GetJObjectFromDB(ocType);
                        //}
                        //recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                        //MySession.Current.RecordsNifty = recordsObject;

                        recordsObject = UpdateRecordsObject(ocType);
                    }
                }

                if (MySession.Current.RecordsNifty != null)
                    MySession.Current.RecordsObject = MySession.Current.RecordsNifty;
            }

            if (ocType.Equals(enumOCType.BANKNIFTY.ToString()))
            {
                if (MySession.Current.RecordsBankNifty == null)
                {
                    //JObject jObject = DownloadJSONDataFromURL(ocType);
                    //if (jObject != null && !jObject.ToString().Equals("{}"))
                    //{
                    //    recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                    //    MySession.Current.RecordsBankNifty = recordsObject;
                    //}

                    recordsObject = UpdateRecordsObject(ocType);
                }
                else
                {
                    recordsObject = MySession.Current.RecordsBankNifty;
                    if (FetchOCAgain(recordsObject))
                    {
                        //JObject jObject = DownloadJSONDataFromURL(ocType);
                        //if (jObject != null && !jObject.ToString().Equals("{}"))
                        //{
                        //    recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                        //    MySession.Current.RecordsBankNifty = recordsObject;
                        //}

                        recordsObject = UpdateRecordsObject(ocType);
                    }
                }

                if (MySession.Current.RecordsBankNifty != null)
                    MySession.Current.RecordsObject = MySession.Current.RecordsBankNifty;
            }
            return recordsObject;
        }

        private static Records UpdateRecordsObject(string ocType)
        {
            Records recordsObject = new Records();
            JObject jObject = DownloadJSONDataFromURL(ocType);
            if (jObject == null || jObject.ToString().Equals("{}"))
            {
                DataTable dataTable = DatabaseClass.ReadOptionsChain(ocType);
                jObject = (JObject)dataTable.Rows[0][1];
            }
            else
            {
                DatabaseClass.UpdateOptionsChainAsync(ocType, jObject["records"].ToString());
            }
            recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
            
            if (ocType.Equals(enumOCType.NIFTY.ToString()))
            {
                MySession.Current.RecordsNifty = recordsObject;
            }
            if (ocType.Equals(enumOCType.BANKNIFTY.ToString()))
            {
                MySession.Current.RecordsBankNifty = recordsObject;
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
            dtResult.Columns.Add(enumOutputGridColumns.TradingSymbol.ToString(), typeof(string));
            dtResult.Columns.Add(enumOutputGridColumns.CEPE.ToString(), typeof(string));
            dtResult.Columns.Add(enumOutputGridColumns.BuySell.ToString(), typeof(string));
            dtResult.Columns.Add(enumOutputGridColumns.StrikePrice.ToString(), typeof(int));
            dtResult.Columns.Add(enumOutputGridColumns.Quantity.ToString(), typeof(int));
            dtResult.Columns.Add(enumOutputGridColumns.Premium.ToString(), typeof(double));
            dtResult.Columns.Add(enumOutputGridColumns.ProfitLoss.ToString(), typeof(double));
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
                ddlSP.Items.Add(Constants.ALL);

            List<int> expiryDates = OCHelper.GetOCSPList(ocType);
            foreach (int item in expiryDates)
            {
                ddlSP.Items.Add(item.ToString());
            }
        }

        public static void FillAllExpiryDates(string ocType, DropDownList ddlExp, bool IsAddAll)
        {
            if (IsAddAll)
                ddlExp.Items.Add(Constants.ALL);

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
                    datarow[enumOCColumns.StrikePrice.ToString()] = row.CE.strikePrice;
                    datarow[enumOCColumns.ExpiryDate.ToString()] = row.CE.expiryDate;
                    datarow[enumOCColumns.CEunderlying.ToString()] = row.CE.underlying;
                    datarow[enumOCColumns.CEidentifier.ToString()] = row.CE.identifier;
                    datarow[enumOCColumns.CEopenInterest.ToString()] = row.CE.openInterest;
                    datarow[enumOCColumns.CEchangeinOpenInterest.ToString()] = row.CE.changeinOpenInterest;
                    datarow[enumOCColumns.CEpchangeinOpenInterest.ToString()] = row.CE.pchangeinOpenInterest;
                    datarow[enumOCColumns.CEtotalTradedVolume.ToString()] = row.CE.totalTradedVolume;
                    datarow[enumOCColumns.CEimpliedVolatility.ToString()] = row.CE.impliedVolatility;
                    datarow[enumOCColumns.CElastPrice.ToString()] = row.CE.lastPrice;
                    datarow[enumOCColumns.CEchange.ToString()] = Math.Round(row.CE.change, 2);
                    datarow[enumOCColumns.CEpChange.ToString()] = row.CE.pChange;
                    datarow[enumOCColumns.CEtotalBuyQuantity.ToString()] = row.CE.totalBuyQuantity;
                    datarow[enumOCColumns.CEtotalSellQuantity.ToString()] = row.CE.totalSellQuantity;
                    datarow[enumOCColumns.CEbidQty.ToString()] = row.CE.bidQty;
                    datarow[enumOCColumns.CEbidprice.ToString()] = row.CE.bidprice;
                    datarow[enumOCColumns.CEaskQty.ToString()] = row.CE.askQty;
                    datarow[enumOCColumns.CEaskPrice.ToString()] = row.CE.askPrice;
                    datarow[enumOCColumns.CEunderlyingValue.ToString()] = row.CE.underlyingValue;

                    datarow[enumOCColumns.DaysToExpiry.ToString()] = DateTime.Parse(row.expiryDate).Subtract(DateTime.Now).Days;
                    datarow[enumOCColumns.SD.ToString()] = 0.0;
                    datarow[enumOCColumns.CEDelta.ToString()] = 0.0;

                }

                if (row.PE != null)
                {
                    datarow[enumOCColumns.StrikePrice.ToString()] = row.PE.strikePrice;
                    datarow[enumOCColumns.ExpiryDate.ToString()] = row.PE.expiryDate;
                    datarow[enumOCColumns.PEunderlying.ToString()] = row.PE.underlying;
                    datarow[enumOCColumns.PEidentifier.ToString()] = row.PE.identifier;
                    datarow[enumOCColumns.PEopenInterest.ToString()] = row.PE.openInterest;
                    datarow[enumOCColumns.PEchangeinOpenInterest.ToString()] = row.PE.changeinOpenInterest;
                    datarow[enumOCColumns.PEpchangeinOpenInterest.ToString()] = row.PE.pchangeinOpenInterest;
                    datarow[enumOCColumns.PEtotalTradedVolume.ToString()] = row.PE.totalTradedVolume;
                    datarow[enumOCColumns.PEimpliedVolatility.ToString()] = row.PE.impliedVolatility;
                    datarow[enumOCColumns.PElastPrice.ToString()] = row.PE.lastPrice;
                    datarow[enumOCColumns.PEchange.ToString()] = Math.Round(row.PE.change, 2);
                    datarow[enumOCColumns.PEpChange.ToString()] = row.PE.pChange;
                    datarow[enumOCColumns.PEtotalBuyQuantity.ToString()] = row.PE.totalBuyQuantity;
                    datarow[enumOCColumns.PEtotalSellQuantity.ToString()] = row.PE.totalSellQuantity;
                    datarow[enumOCColumns.PEbidQty.ToString()] = row.PE.bidQty;
                    datarow[enumOCColumns.PEbidprice.ToString()] = row.PE.bidprice;
                    datarow[enumOCColumns.PEaskQty.ToString()] = row.PE.askQty;
                    datarow[enumOCColumns.PEaskPrice.ToString()] = row.PE.askPrice;
                    datarow[enumOCColumns.PEunderlyingValue.ToString()] = row.PE.underlyingValue;

                    datarow[enumOCColumns.DaysToExpiry.ToString()] = DateTime.Parse(row.expiryDate).Subtract(DateTime.Now).Days;
                    datarow[enumOCColumns.SD.ToString()] = 0.0;
                    datarow[enumOCColumns.CEDelta.ToString()] = 0.0;
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
                iUpperStrikePriceRange = OCHelper.RoundTo100(MySession.Current.RecordsObject.underlyingValue + (OCHelper.DefaultSP(filterConditions.OCType) * filterConditions.PercentageRange / 100));
                iLowerStrikePriceRange = OCHelper.RoundTo100(MySession.Current.RecordsObject.underlyingValue - (OCHelper.DefaultSP(filterConditions.OCType) * filterConditions.PercentageRange / 100));
            }
            else
            {
                iUpperStrikePriceRange = filterConditions.SPHigherRange;
                iLowerStrikePriceRange = filterConditions.SPLowerRange;
            }

            string strFilter = string.Empty;
            List<string> filters = new List<string>();

            if (filterConditions.ExpiryDate != null && !filterConditions.ExpiryDate.ToUpper().Equals(Constants.ALL))
                filters.Add(" (" + enumStrategyColumns.ExpiryDate + " = #" + filterConditions.ExpiryDate + "#) ");

            if (filterConditions.ShowSPBetweenLowHigh)
            {
                filters.Add(" ((" + enumStrategyColumns.StrikePrice + " >= " + iLowerStrikePriceRange + ") AND (" + enumStrategyColumns.StrikePrice + " <= " + iUpperStrikePriceRange + ")) ");
            }
            else
            {
                filters.Add(" ((" + enumStrategyColumns.StrikePrice + " <= " + iLowerStrikePriceRange + ") OR (" + enumStrategyColumns.StrikePrice + " >= " + iUpperStrikePriceRange + ")) ");
            }

            if (filterConditions.ContractType != null && !filterConditions.ContractType.ToUpper().Equals(Constants.ALL))
                filters.Add(" (" + enumStrategyColumns.ContractType + " = '" + filterConditions.ContractType + "') ");

            if (filterConditions.TransactionType != null && !filterConditions.TransactionType.ToUpper().Equals(Constants.ALL))
                filters.Add(" (" + enumStrategyColumns.TransactionType + " = '" + filterConditions.TransactionType + "') ");

            //if (filterConditions.ExtrinsicValuePer != null && !filterConditions.ExtrinsicValuePer.ToUpper().Equals("ALL"))
            //    filters.Add(" (" + enumStrategyColumns.ExtrinsicValuePer + " >= '" + filterConditions.ExtrinsicValuePer + "') ");

            //if (filterConditions.LTP != null && !filterConditions.LTP.ToUpper().Equals("ALL"))
            //    filters.Add(" ((" + enumStrategyColumns.CElastPrice + " >= '" + filterConditions.LTP + "') OR (" + enumStrategyColumns.PElastPrice + " >= '" + filterConditions.LTP + "')) ");

            //if (filterConditions.ImpliedVolatility != null && !filterConditions.ImpliedVolatility.ToUpper().Equals("ALL"))
            //    filters.Add(" ((" + enumStrategyColumns.CEimpliedVolatility + " >= '" + filterConditions.ImpliedVolatility + "') OR (" + enumStrategyColumns.PEimpliedVolatility + " >= '" + filterConditions.ImpliedVolatility + "')) ");

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

        //public static string GetFilterString(FilterConditions filterConditions)
        //{
        //    int iUpperStrikePriceRange = 0;
        //    int iLowerStrikePriceRange = 0;

        //    if (filterConditions.PercentageRange > 0)
        //    {
        //        iUpperStrikePriceRange = RoundTo100(MySession.Current.RecordsObject.underlyingValue + (DefaultSP(filterConditions.OCType) * filterConditions.PercentageRange / 100));
        //        iLowerStrikePriceRange = RoundTo100(MySession.Current.RecordsObject.underlyingValue - (DefaultSP(filterConditions.OCType) * filterConditions.PercentageRange / 100));
        //    }
        //    else
        //    {
        //        iUpperStrikePriceRange = filterConditions.SPHigherRange;
        //        iLowerStrikePriceRange = filterConditions.SPLowerRange;
        //    }

        //    string strFilter = string.Empty;
        //    List<string> filters = new List<string>();

        //    if (filterConditions.ExpiryDate != null && !filterConditions.ExpiryDate.ToUpper().Equals("ALL"))
        //        filters.Add(" (ExpiryDate = #" + filterConditions.ExpiryDate + "#) ");

        //    if (filterConditions.ShowSPBetweenLowHigh)
        //    {
        //        filters.Add(" ((StrikePrice >= " + iLowerStrikePriceRange + ") AND (StrikePrice <= " + iUpperStrikePriceRange + ")) ");
        //    }
        //    else
        //    {
        //        filters.Add(" ((StrikePrice <= " + iLowerStrikePriceRange + ") OR (StrikePrice >= " + iUpperStrikePriceRange + ")) ");
        //    }

        //    if (filterConditions.ContractType != null && !filterConditions.ContractType.ToUpper().Equals("ALL"))
        //        filters.Add(" (" + enumOCColumns.ContractType + " = '" + filterConditions.ContractType + "') ");

        //    if (filterConditions.TransactionType != null && !filterConditions.TransactionType.ToUpper().Equals("ALL"))
        //        filters.Add(" (TransactionType = '" + filterConditions.TransactionType + "') ");

        //    if (filterConditions.ExtrinsicValuePer != null && !filterConditions.ExtrinsicValuePer.ToUpper().Equals("ALL"))
        //        filters.Add(" (" + enumOCColumns.ExtrinsicValuePer + " >= '" + filterConditions.ExtrinsicValuePer + "') ");

        //    if (filterConditions.LTP != null && !filterConditions.LTP.ToUpper().Equals("ALL"))
        //        filters.Add(" ((" + enumOCColumns.CElastPrice + " >= '" + filterConditions.LTP + "') OR (" + enumOCColumns.PElastPrice + " >= '" + filterConditions.LTP + "')) ");

        //    if (filterConditions.ImpliedVolatility != null && !filterConditions.ImpliedVolatility.ToUpper().Equals("ALL"))
        //        filters.Add(" ((" + enumOCColumns.CEimpliedVolatility + " >= '" + filterConditions.ImpliedVolatility + "') OR (" + enumOCColumns.PEimpliedVolatility + " >= '" + filterConditions.ImpliedVolatility + "')) ");

        //    for (int index = 0; index < filters.Count; index++)
        //    {
        //        if (index < filters.Count - 1)
        //        {
        //            strFilter += filters[index] + " AND ";
        //        }
        //        else
        //        {
        //            strFilter += filters[index];
        //        }
        //    }

        //    return strFilter;
        //}

        public static void GetGlobalFilterConditions()
        {
            GlobalFilterConditions globalFilterConditions = new GlobalFilterConditions();

            //These values should actually be fetched from the database.
            globalFilterConditions.Hide50MulSP = true;
            globalFilterConditions.Hide1SDSP = false;
            globalFilterConditions.Hide2SDSP = false;

            MySession.Current.GlobalFilterConditions = globalFilterConditions;
        }

        public static DataTable toDataTable(Records recordsObject)
        {
            var result = new DataTable();

            result = addDataTableColumns(result);
            double extrinsicValue = 0;

            foreach (var row in recordsObject.data)
            {
                //string formattedDateForTradingSymbol = TradingSymbol_DateFormatter(row.expiryDate);
                var datarow = result.NewRow();
                if (row.CE != null && row.CE.strikePrice % 100 == 0 && row.CE.lastPrice > 0)
                {
                    datarow[enumOCColumns.StrikePrice.ToString()] = row.CE.strikePrice;
                    datarow[enumOCColumns.ExpiryDate.ToString()] = row.CE.expiryDate;
                    datarow[enumOCColumns.CEunderlying.ToString()] = row.CE.underlying;
                    datarow[enumOCColumns.CEidentifier.ToString()] = row.CE.identifier;
                    datarow[enumOCColumns.CEopenInterest.ToString()] = row.CE.openInterest;
                    datarow[enumOCColumns.CEchangeinOpenInterest.ToString()] = row.CE.changeinOpenInterest;
                    datarow[enumOCColumns.CEpchangeinOpenInterest.ToString()] = row.CE.pchangeinOpenInterest;
                    datarow[enumOCColumns.CEtotalTradedVolume.ToString()] = row.CE.totalTradedVolume;
                    datarow[enumOCColumns.CEimpliedVolatility.ToString()] = row.CE.impliedVolatility;
                    datarow[enumOCColumns.CElastPrice.ToString()] = row.CE.lastPrice;
                    datarow[enumOCColumns.CEchange.ToString()] = Math.Round(row.CE.change, 2);
                    datarow[enumOCColumns.CEpChange.ToString()] = row.CE.pChange;
                    datarow[enumOCColumns.CEtotalBuyQuantity.ToString()] = row.CE.totalBuyQuantity;
                    datarow[enumOCColumns.CEtotalSellQuantity.ToString()] = row.CE.totalSellQuantity;
                    datarow[enumOCColumns.CEbidQty.ToString()] = row.CE.bidQty;
                    datarow[enumOCColumns.CEbidprice.ToString()] = row.CE.bidprice;
                    datarow[enumOCColumns.CEaskQty.ToString()] = row.CE.askQty;
                    datarow[enumOCColumns.CEaskPrice.ToString()] = row.CE.askPrice;
                    datarow[enumOCColumns.CEunderlyingValue.ToString()] = row.CE.underlyingValue;
                    datarow[enumOCColumns.ContractType.ToString()] = enumContractType.CE;
                    datarow[enumOCColumns.CETradingSymbol.ToString()] = string.Concat(row.CE.underlying, TradingSymbol_DateFormatter(row.expiryDate), row.CE.strikePrice.ToString(), enumContractType.CE.ToString());

                    int daysToExpiry = DateTime.Parse(row.expiryDate).Subtract(DateTime.Now).Days + 1;
                    datarow[enumOCColumns.DaysToExpiry.ToString()] = daysToExpiry;
                    datarow[enumOCColumns.SD.ToString()] = 0.0;
                    datarow[enumOCColumns.CEDelta.ToString()] = 0.0;

                    if (row.CE.strikePrice < row.CE.underlyingValue) //ITM - In the money
                        extrinsicValue = Math.Abs(row.CE.lastPrice + row.CE.strikePrice - row.CE.underlyingValue);
                    else
                        extrinsicValue = row.CE.lastPrice;

                    datarow[enumOCColumns.CEExtrinsicValue.ToString()] = Math.Round(extrinsicValue, 2);
                    datarow[enumOCColumns.CEExtrinsicValuePer.ToString()] = Math.Round((Math.Abs(extrinsicValue) * 100 / row.CE.underlyingValue), 2);
                    datarow[enumOCColumns.CEExtrinsicValuePerDay.ToString()] = Math.Round(extrinsicValue / daysToExpiry, 2);
                }

                if (row.PE != null && row.PE.strikePrice % 100 == 0 && row.PE.lastPrice > 0)
                {
                    datarow[enumOCColumns.StrikePrice.ToString()] = row.PE.strikePrice;
                    datarow[enumOCColumns.ExpiryDate.ToString()] = row.PE.expiryDate;
                    datarow[enumOCColumns.PEunderlying.ToString()] = row.PE.underlying;
                    datarow[enumOCColumns.PEidentifier.ToString()] = row.PE.identifier;
                    datarow[enumOCColumns.PEopenInterest.ToString()] = row.PE.openInterest;
                    datarow[enumOCColumns.PEchangeinOpenInterest.ToString()] = row.PE.changeinOpenInterest;
                    datarow[enumOCColumns.PEpchangeinOpenInterest.ToString()] = row.PE.pchangeinOpenInterest;
                    datarow[enumOCColumns.PEtotalTradedVolume.ToString()] = row.PE.totalTradedVolume;
                    datarow[enumOCColumns.PEimpliedVolatility.ToString()] = row.PE.impliedVolatility;
                    datarow[enumOCColumns.PElastPrice.ToString()] = row.PE.lastPrice;
                    datarow[enumOCColumns.PEchange.ToString()] = Math.Round(row.PE.change, 2);
                    datarow[enumOCColumns.PEpChange.ToString()] = row.PE.pChange;
                    datarow[enumOCColumns.PEtotalBuyQuantity.ToString()] = row.PE.totalBuyQuantity;
                    datarow[enumOCColumns.PEtotalSellQuantity.ToString()] = row.PE.totalSellQuantity;
                    datarow[enumOCColumns.PEbidQty.ToString()] = row.PE.bidQty;
                    datarow[enumOCColumns.PEbidprice.ToString()] = row.PE.bidprice;
                    datarow[enumOCColumns.PEaskQty.ToString()] = row.PE.askQty;
                    datarow[enumOCColumns.PEaskPrice.ToString()] = row.PE.askPrice;
                    datarow[enumOCColumns.PEunderlyingValue.ToString()] = row.PE.underlyingValue;
                    datarow[enumOCColumns.ContractType.ToString()] = enumContractType.PE;
                    datarow[enumOCColumns.PETradingSymbol.ToString()] = string.Concat(row.PE.underlying, TradingSymbol_DateFormatter(row.expiryDate), row.PE.strikePrice.ToString(), enumContractType.PE.ToString());

                    int daysToExpiry = DateTime.Parse(row.expiryDate).Subtract(DateTime.Now).Days + 1;
                    datarow[enumOCColumns.DaysToExpiry.ToString()] = daysToExpiry;
                    datarow[enumOCColumns.SD.ToString()] = 0.0;
                    datarow[enumOCColumns.PEDelta.ToString()] = 0.0;

                    if (row.PE.strikePrice > row.PE.underlyingValue) //ITM - In the money
                        extrinsicValue = Math.Abs(row.PE.lastPrice - (row.PE.strikePrice - row.PE.underlyingValue));
                    else
                        extrinsicValue = row.PE.lastPrice;

                    datarow[enumOCColumns.PEExtrinsicValue.ToString()] = Math.Round(extrinsicValue, 2);
                    datarow[enumOCColumns.PEExtrinsicValuePer.ToString()] = Math.Round((Math.Abs(extrinsicValue) * 100 / row.PE.underlyingValue), 2);
                    datarow[enumOCColumns.PEExtrinsicValuePerDay.ToString()] = Math.Round(extrinsicValue / daysToExpiry, 2);
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
            //Records.Datum.CE1 ce = new Records.Datum.CE1();
            //result.Columns.Add(enumOCColumns.CEexpiryDate.ToString(), typeof(string));
            result.Columns.Add(enumOCColumns.CEunderlying.ToString(), typeof(string));
            result.Columns.Add(enumOCColumns.CEidentifier.ToString(), typeof(string));
            result.Columns.Add(enumOCColumns.CEpchangeinOpenInterest.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEimpliedVolatility.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEtotalBuyQuantity.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.CEtotalSellQuantity.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.CEunderlyingValue.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEpChange.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.ContractType.ToString(), typeof(string));
            result.Columns.Add(enumOCColumns.CEopenInterest.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEchangeinOpenInterest.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEtotalTradedVolume.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.CElastPrice.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEchange.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEbidQty.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.CEbidprice.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEaskPrice.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEaskQty.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.StrikePrice.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.CETradingSymbol.ToString(), typeof(string));
            result.Columns.Add(enumOCColumns.PEbidQty.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.PEbidprice.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEaskPrice.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEaskQty.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.PEchange.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PElastPrice.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEtotalTradedVolume.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.PEchangeinOpenInterest.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEpchangeinOpenInterest.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEpChange.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.ExpiryDate.ToString(), typeof(string));
            result.Columns.Add(enumOCColumns.PEunderlying.ToString(), typeof(string));
            result.Columns.Add(enumOCColumns.PEidentifier.ToString(), typeof(string));
            result.Columns.Add(enumOCColumns.PEopenInterest.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEimpliedVolatility.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEtotalBuyQuantity.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.PEtotalSellQuantity.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.PEunderlyingValue.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PETradingSymbol.ToString(), typeof(string));
            result.Columns.Add(enumOCColumns.DaysToExpiry.ToString(), typeof(int));
            result.Columns.Add(enumOCColumns.CEExtrinsicValue.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEExtrinsicValuePer.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEExtrinsicValuePerDay.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEExtrinsicValue.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEExtrinsicValuePer.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEExtrinsicValuePerDay.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.SD.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.CEDelta.ToString(), typeof(double));
            result.Columns.Add(enumOCColumns.PEDelta.ToString(), typeof(double));

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

    enum enumOCColumns
    {
        //CEexpiryDate,
        CEunderlying,
        CEidentifier,
        CEpchangeinOpenInterest,
        CEimpliedVolatility,
        CEtotalBuyQuantity,
        CEtotalSellQuantity,
        CEunderlyingValue,
        CEpChange,
        ContractType,
        CEopenInterest,
        CEchangeinOpenInterest,
        CEtotalTradedVolume,
        CElastPrice,
        CEchange,
        CEbidQty,
        CEbidprice,
        CEaskPrice,
        CEaskQty,
        StrikePrice,
        CETradingSymbol,
        PEbidQty,
        PEbidprice,
        PEaskPrice,
        PEaskQty,
        PEchange,
        PElastPrice,
        PEtotalTradedVolume,
        PEchangeinOpenInterest,
        PEpchangeinOpenInterest,
        PEpChange,
        ExpiryDate,
        PEunderlying,
        PEidentifier,
        PEopenInterest,
        PEimpliedVolatility,
        PEtotalBuyQuantity,
        PEtotalSellQuantity,
        PEunderlyingValue,
        PETradingSymbol,
        DaysToExpiry,
        CEExtrinsicValue,
        CEExtrinsicValuePer,
        CEExtrinsicValuePerDay,
        PEExtrinsicValue,
        PEExtrinsicValuePer,
        PEExtrinsicValuePerDay,
        SD,
        CEDelta,
        PEDelta
    }
}