using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Data;


namespace TOC
{
    public partial class OptionChain : System.Web.UI.Page
    {
        //private const string NSEIndiaWebsiteURL = "https://www1.nseindia.com";
        //private const string mainurl = NSEIndiaWebsiteURL + "/live_market/dynaContent/live_watch/stock_watch/niftyStockWatch.json";
        //string mainurl = "https://docs.microsoft.com";
        string mainurl = "https://www.nseindia.com/api/option-chain-indices?symbol=BANKNIFTY";
        //string mainurl = "https://www1.nseindia.com/marketinfo/sym_map/symbolMapping.jsp?symbol=NIFTY&instrument=OPTIDX&date=-&segmentLink=17";
        //string mainurl = "https://www1.nseindia.com/live_market/dynaContent/live_watch/option_chain/optionKeys.jsp?symbolCode=-10006&symbol=NIFTY&symbol=NIFTY&instrument=-&date=-&segmentLink=17&symbolCount=2&segmentLink=17";
        Records recordsObject = new Records();
        Filtered filteredObject = new Filtered();
        double currentPrice = 0.0;
        double higherStrikePrice = 0.0;
        double lowerStrikePrice = 0.0;
        double currentStrikePrice = 0.0;
        int diffStrikePrice = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                FetchOC();
                DataTable dt = toDataTable(filteredObject);
                FillDataTable(dt);
            }
            catch (Exception ex)
            {
                // You might want to handle some specific errors : Just pass on up for now...
                // Remove this catch if you don't want to handle errors here.
                throw;
            }
        }
        private void FetchOC()
        {
            JObject jObject = DownloadJSONDataFromURL(mainurl);
            recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
            filteredObject = JsonConvert.DeserializeObject<Filtered>(jObject["filtered"].ToString());
            currentPrice = recordsObject.underlyingValue;

            UpdateStrikePriceDiff();
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
                    datarow["strikePrice"] = row.CE.strikePrice.ToString();
                    //datarow["CE" + "expiryDate"] = row.CE.expiryDate.ToString();
                    //datarow["CE" + "underlying"] = row.CE.underlying.ToString();
                    //datarow["CE" + "identifier"] = row.CE.identifier.ToString();
                    datarow["CE" + "openInterest"] = row.CE.openInterest.ToString();
                    datarow["CE" + "changeinOpenInterest"] = row.CE.changeinOpenInterest.ToString();
                    //datarow["CE" + "pchangeinOpenInterest"] = row.CE.pchangeinOpenInterest.ToString();
                    datarow["CE" + "totalTradedVolume"] = row.CE.totalTradedVolume.ToString();
                    //datarow["CE" + "impliedVolatility"] = row.CE.impliedVolatility.ToString();
                    datarow["CE" + "lastPrice"] = row.CE.lastPrice.ToString();
                    datarow["CE" + "change"] = row.CE.change.ToString();
                    //datarow["CE" + "pChange"] = row.CE.pChange.ToString();
                    //datarow["CE" + "totalBuyQuantity"] = row.CE.totalBuyQuantity.ToString();
                    //datarow["CE" + "totalSellQuantity"] = row.CE.totalSellQuantity.ToString();
                    datarow["CE" + "bidQty"] = row.CE.bidQty.ToString();
                    datarow["CE" + "bidprice"] = row.CE.bidprice.ToString();
                    datarow["CE" + "askQty"] = row.CE.askQty.ToString();
                    datarow["CE" + "askPrice"] = row.CE.askPrice.ToString();
                    //datarow["CE" + "underlyingValue"] = row.CE.underlyingValue.ToString();
                }

                if (row.PE != null)
                {
                    datarow["strikePrice"] = row.PE.strikePrice.ToString();
                    //datarow["PE" + "expiryDate"] = row.PE.expiryDate.ToString();
                    //datarow["PE" + "underlying"] = row.PE.underlying.ToString();
                    //datarow["PE" + "identifier"] = row.PE.identifier.ToString();
                    //datarow["PE" + "openInterest"] = row.PE.openInterest.ToString();
                    datarow["PE" + "changeinOpenInterest"] = row.PE.changeinOpenInterest.ToString();
                    datarow["PE" + "pchangeinOpenInterest"] = row.PE.pchangeinOpenInterest.ToString();
                    datarow["PE" + "totalTradedVolume"] = row.PE.totalTradedVolume.ToString();
                    //datarow["PE" + "impliedVolatility"] = row.PE.impliedVolatility.ToString();
                    datarow["PE" + "lastPrice"] = row.PE.lastPrice.ToString();
                    datarow["PE" + "change"] = row.PE.change.ToString();
                    //datarow["PE" + "pChange"] = row.PE.pChange.ToString();
                    //datarow["PE" + "totalBuyQuantity"] = row.PE.totalBuyQuantity.ToString();
                    //datarow["PE" + "totalSellQuantity"] = row.PE.totalSellQuantity.ToString();
                    datarow["PE" + "bidQty"] = row.PE.bidQty.ToString();
                    datarow["PE" + "bidprice"] = row.PE.bidprice.ToString();
                    datarow["PE" + "askQty"] = row.PE.askQty.ToString();
                    datarow["PE" + "askPrice"] = row.PE.askPrice.ToString();
                    //datarow["PE" + "underlyingValue"] = row.PE.underlyingValue.ToString();
                }

                result.Rows.Add(datarow);
            }

            return result;
        }
        private static DataTable toDataTable(Records recordsObject)
        {
            var result = new DataTable();
            result = addDataTableColumns(result);

            foreach (var row in recordsObject.data)
            {
                var datarow = result.NewRow();
                if (row.CE != null)
                {
                    datarow["CE" + "strikePrice"] = row.CE.strikePrice.ToString();
                    datarow["CE" + "expiryDate"] = row.CE.expiryDate.ToString();
                    datarow["CE" + "underlying"] = row.CE.underlying.ToString();
                    datarow["CE" + "identifier"] = row.CE.identifier.ToString();
                    datarow["CE" + "openInterest"] = row.CE.openInterest.ToString();
                    datarow["CE" + "changeinOpenInterest"] = row.CE.changeinOpenInterest.ToString();
                    datarow["CE" + "pchangeinOpenInterest"] = row.CE.pchangeinOpenInterest.ToString();
                    datarow["CE" + "totalTradedVolume"] = row.CE.totalTradedVolume.ToString();
                    datarow["CE" + "impliedVolatility"] = row.CE.impliedVolatility.ToString();
                    datarow["CE" + "lastPrice"] = row.CE.lastPrice.ToString();
                    datarow["CE" + "change"] = row.CE.change.ToString();
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
                    datarow["PE" + "strikePrice"] = row.PE.strikePrice.ToString();
                    datarow["PE" + "expiryDate"] = row.PE.expiryDate.ToString();
                    datarow["PE" + "underlying"] = row.PE.underlying.ToString();
                    datarow["PE" + "identifier"] = row.PE.identifier.ToString();
                    datarow["PE" + "openInterest"] = row.PE.openInterest.ToString();
                    datarow["PE" + "changeinOpenInterest"] = row.PE.changeinOpenInterest.ToString();
                    datarow["PE" + "pchangeinOpenInterest"] = row.PE.pchangeinOpenInterest.ToString();
                    datarow["PE" + "totalTradedVolume"] = row.PE.totalTradedVolume.ToString();
                    datarow["PE" + "impliedVolatility"] = row.PE.impliedVolatility.ToString();
                    datarow["PE" + "lastPrice"] = row.PE.lastPrice.ToString();
                    datarow["PE" + "change"] = row.PE.change.ToString();
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
        private JObject DownloadJSONDataFromURL(string webResourceURL)
        {
            string stockWatchJSONString = string.Empty;

            using (var webClient = new WebClient())
            {
                // Set headers to download the data
                webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");

                // Download the data
                stockWatchJSONString = webClient.DownloadString(webResourceURL);

                //DataTable dt = toDataTable(stockWatchJSONString);

                // Serialise it into a JObject
                JObject jObject = JObject.Parse(stockWatchJSONString);

                return jObject;
            }
        }
        private static DataTable addDataTableColumns(DataTable result)
        {
            Records.Datum.CE1 ce = new Records.Datum.CE1();

            //result.Columns.Add("CE" + "expiryDate");
            //result.Columns.Add("CE" + "underlying");
            //result.Columns.Add("CE" + "identifier");
            //result.Columns.Add("CE" + "pchangeinOpenInterest");
            //result.Columns.Add("CE" + "impliedVolatility");
            //result.Columns.Add("CE" + "totalBuyQuantity");
            //result.Columns.Add("CE" + "totalSellQuantity");
            //result.Columns.Add("CE" + "underlyingValue");
            //result.Columns.Add("CE" + "pChange");

            result.Columns.Add("CE" + "openInterest");
            result.Columns.Add("CE" + "changeinOpenInterest");
            result.Columns.Add("CE" + "totalTradedVolume");
            result.Columns.Add("CE" + "lastPrice");
            result.Columns.Add("CE" + "change");
            result.Columns.Add("CE" + "bidQty");
            result.Columns.Add("CE" + "bidprice");
            result.Columns.Add("CE" + "askPrice");
            result.Columns.Add("CE" + "askQty");
            result.Columns.Add("strikePrice");

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

            //result.Columns.Add("PE" + "pChange");
            //result.Columns.Add("PE" + "expiryDate");
            //result.Columns.Add("PE" + "underlying");
            //result.Columns.Add("PE" + "identifier");
            //result.Columns.Add("PE" + "openInterest");
            //result.Columns.Add("PE" + "impliedVolatility");
            //result.Columns.Add("PE" + "totalBuyQuantity");
            //result.Columns.Add("PE" + "totalSellQuantity");
            //result.Columns.Add("PE" + "underlyingValue");

            return result;
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Text = "Refreshing...";
            FetchOC();
            DataTable dt = toDataTable(filteredObject);
            FillDataTable(dt);
            btnRefresh.Text = "Refresh Data";
        }
        private void FillDataTable(DataTable dt)
        {
            gvData.DataSource = dt;
            gvData.DataBind();
        }
        protected void btnGetButterflySpread_Click(object sender, EventArgs e)
        {
            FetchOC();
            CreateStrategyTable();
        }
        private List<int> GetStrikePrices()
        {
            return recordsObject.strikePrices;
        }
        private DataTable AddColumnstoStrategyTable(DataTable dt)
        {
            dt.Columns.Add("Stock");
            dt.Columns.Add("Contract");
            dt.Columns.Add("TransactionType");
            dt.Columns.Add("StrikePrice");
            dt.Columns.Add("LotSize");
            dt.Columns.Add("Premium");

            List<int> strikePrices = GetStrikePrices();

            foreach (var item in strikePrices)
            {
                dt.Columns.Add(item.ToString());
            }
            return dt;
        }
        private DataTable AddRowstoStrategyTable(DataTable dt)
        {
            List<int> strikePrices = GetStrikePrices();
            DataRow datarow;
            foreach (var row in filteredObject.data)
            {
                if (row.CE != null)
                {
                    //Add CE Buy row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.CE.underlying.ToString();
                    datarow["Contract"] = ContractType.CE.ToString();
                    datarow["TransactionType"] = TransactionType.Buy.ToString();
                    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
                    datarow["LotSize"] = LotSize.BankNifty.ToString();
                    datarow["Premium"] = row.CE.lastPrice.ToString();

                    foreach (var item in strikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = Utility.FO.CallBuy(row.CE.strikePrice, row.CE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);

                    //Add CE Sell row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.CE.underlying.ToString();
                    datarow["Contract"] = ContractType.CE.ToString();
                    datarow["TransactionType"] = TransactionType.Sell.ToString();
                    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
                    datarow["LotSize"] = LotSize.BankNifty.ToString();
                    datarow["Premium"] = row.CE.lastPrice.ToString();

                    foreach (var item in strikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = Utility.FO.CallSell(row.CE.strikePrice, row.CE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);
                }

                if (row.PE != null)
                {
                    //Add PE Buy row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.PE.underlying.ToString();
                    datarow["Contract"] = ContractType.PE.ToString();
                    datarow["TransactionType"] = TransactionType.Buy.ToString();
                    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
                    datarow["LotSize"] = LotSize.BankNifty.ToString();
                    datarow["Premium"] = row.PE.lastPrice.ToString();

                    foreach (var item in strikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = Utility.FO.PutBuy(row.PE.strikePrice, row.PE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);

                    //Add PE Sell row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.PE.underlying.ToString();
                    datarow["Contract"] = ContractType.PE.ToString();
                    datarow["TransactionType"] = TransactionType.Sell.ToString();
                    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
                    datarow["LotSize"] = LotSize.BankNifty.ToString();
                    datarow["Premium"] = row.PE.lastPrice.ToString();

                    foreach (var item in strikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = Utility.FO.PutSell(row.PE.strikePrice, row.PE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);
                }
            }
            return dt;
        }
        private void CreateStrategyTable()
        {
            DataTable dtStrategy = new DataTable();
            dtStrategy = AddColumnstoStrategyTable(dtStrategy);
            dtStrategy = AddRowstoStrategyTable(dtStrategy);

            //DataTable dtCEStrategy = dtStrategy.Rows.f
            
            DataRow[] drs = dtStrategy.Select("(Contract = 'CE') AND ((StrikePrice = " + lowerStrikePrice + "  AND TransactionType = 'Buy') OR (StrikePrice = " + currentStrikePrice + " AND TransactionType = 'Sell') OR (StrikePrice = " + higherStrikePrice + " AND TransactionType = 'Buy'))");
            //make a new "results" datatable via clone to keep structure
            DataTable dtButterflySpreadCEStrategy = dtStrategy.Clone();
            //Import the Rows
            foreach (DataRow d in drs)
            {
                dtButterflySpreadCEStrategy.ImportRow(d);
            }
            
            DataTable dtButterflySpreadCEStrategyResult = new DataTable();
            dtButterflySpreadCEStrategyResult.Columns.Add("Stock");
            dtButterflySpreadCEStrategyResult.Columns.Add("Contract");
            dtButterflySpreadCEStrategyResult.Columns.Add("TransactionType");
            dtButterflySpreadCEStrategyResult.Columns.Add("StrikePrice");
            dtButterflySpreadCEStrategyResult.Columns.Add("LotSize");
            dtButterflySpreadCEStrategyResult.Columns.Add("Premium");
            dtButterflySpreadCEStrategyResult.Columns.Add("ProfitLoss");

            for (int i = 0; i < dtButterflySpreadCEStrategy.Rows.Count; i++)
            {
                var columnName = dtButterflySpreadCEStrategy.Rows[i]["StrikePrice"].ToString();
                double sum = 0;
                for (int j = 0; j < dtButterflySpreadCEStrategy.Rows.Count; j++)
                {
                    sum += Convert.ToDouble(dtButterflySpreadCEStrategy.Rows[j][columnName]);
                }
                //datarow[dt.Columns[item.ToString()].ColumnName] = Utility.FO.PutSell(row.PE.strikePrice, row.PE.lastPrice, Convert.ToDouble(item));
                //rowTarget["Stock"] = dtButterflySpreadCEStrategy.Rows
                //rowTarget["Stock"] = rowSource["Stock"];
                dtButterflySpreadCEStrategyResult.Rows.Add(new string[] {
                    dtButterflySpreadCEStrategy.Rows[i]["Stock"].ToString(),
                    dtButterflySpreadCEStrategy.Rows[i]["Contract"].ToString(),
                    dtButterflySpreadCEStrategy.Rows[i]["TransactionType"].ToString(),
                    dtButterflySpreadCEStrategy.Rows[i]["StrikePrice"].ToString(),
                    dtButterflySpreadCEStrategy.Rows[i]["LotSize"].ToString(),
                    dtButterflySpreadCEStrategy.Rows[i]["Premium"].ToString(),
                    sum.ToString()});
            }

            FillDataTable(dtButterflySpreadCEStrategyResult);
        }
        private void UpdateStrikePriceDiff()
        {
            List<int> prices = GetStrikePrices();
            diffStrikePrice = (prices[1] - prices[0]);
            currentStrikePrice = ((int)Math.Round(currentPrice / diffStrikePrice)) * diffStrikePrice; ;
            lowerStrikePrice = currentStrikePrice - diffStrikePrice;
            higherStrikePrice = currentStrikePrice + diffStrikePrice;
        }
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}
