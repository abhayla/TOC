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
        string mainurl = "https://www.nseindia.com/api/option-chain-indices?symbol=BANKNIFTY";
        //string mainurl = "https://www1.nseindia.com/marketinfo/sym_map/symbolMapping.jsp?symbol=BANKNIFTY&instrument=OPTIDX&date=-&segmentLink=17";
        //string mainurl = "https://www1.nseindia.com/live_market/dynaContent/live_watch/option_chain/optionKeys.jsp?symbolCode=-10006&symbol=NIFTY&symbol=NIFTY&instrument=-&date=-&segmentLink=17&symbolCount=2&segmentLink=17";
        Records recordsObject = new Records();
        Filtered filteredObject = new Filtered();
        double currentPrice = 0.0;
        double higherStrikePrice = 0.0;
        double lowerStrikePrice = 0.0;
        double currentStrikePrice = 0.0;
        int diffStrikePrice = 0;
        int iStrategyCount = 1;

        DataTable dtOCCalculation = new DataTable();
        int iCalColumnPercentage = 5;
        double iUpperStrikePriceRange = 0.0;
        double iLowerStrikePriceRange = 0.0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                FetchOC();
                //DataTable dt = toDataTable(filteredObject);
                DataTable dt = toDataTable(recordsObject);

                string selectQuery = "(ExpiryDate = #" + ddlExpiryDates.SelectedValue + "#)";

                DataRow[] drs = dt.Select(selectQuery, "StrikePrice ASC");
                //make a new "results" datatable via clone to keep structure
                DataTable dtData = dt.Clone();
                //Import the Rows
                foreach (DataRow d in drs)
                {
                    dtData.ImportRow(d);
                }

                FillOCGridview(dtData);
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
            FillExpiryDates();
            UpdateStrikePriceDetails();
        }
        private void FillExpiryDates()
        {
            List<string> expiryDates = recordsObject.expiryDates;
            foreach (string item in expiryDates)
            {
                ddlExpiryDates.Items.Add(item);
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
                    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
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
                    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
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
        private static DataTable toDataTable(Records recordsObject)
        {
            var result = new DataTable();

            result = addDataTableColumns(result);

            foreach (var row in recordsObject.data)
            {
                var datarow = result.NewRow();
                if (row.CE != null)
                {
                    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
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
                    datarow["Contract"] = ContractType.CE.ToString();
                }

                if (row.PE != null)
                {
                    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
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
                    datarow["Contract"] = ContractType.PE.ToString();
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

            return result;
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Text = "Refreshing...";
            FetchOC();
            DataTable dt = toDataTable(recordsObject);
            //gvOptionChain.Visible = true;
            FillOCGridview(dt);
            btnRefresh.Text = "Refresh Data";
        }
        private void FillOCGridview(DataTable dt)
        {
            gvOptionChain.DataSource = dt;
            gvOptionChain.DataBind();
        }
        private void FillBFSpread(DataTable dt)
        {
            GridView gridView = new GridView();
            gridView.DataSource = dt;
            gridView.DataBind();
            divButterflySpread.Controls.Add(gridView);
        }
        protected void btnGetButterflySpread_Click(object sender, EventArgs e)
        {
            FetchOC();
            CreateStrategyTable();
        }
        private List<int> GetStrikePrices()
        {
            List<int> strikePrices = recordsObject.strikePrices;
            List<int> filteredStrikePrices = new List<int>();

            foreach (var item in strikePrices)
            {
                if (item <= iUpperStrikePriceRange && item >= iLowerStrikePriceRange)
                {
                    filteredStrikePrices.Add(item);
                }
            }
            return filteredStrikePrices;
        }
        private DataTable AddColumnstoStrategyTable(DataTable dt)
        {
            dt.Columns.Add("Stock");
            dt.Columns.Add("Contract");
            dt.Columns.Add("TransactionType");
            dt.Columns.Add("StrikePrice");
            dt.Columns.Add("LotSize");
            dt.Columns.Add("Premium");
            dt.Columns.Add("ExpiryDate");

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
            foreach (var row in recordsObject.data)
            {
                if (row.CE != null && (row.CE.strikePrice <= iUpperStrikePriceRange && row.CE.strikePrice >= iLowerStrikePriceRange))
                {
                    //Add CE Buy row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.CE.underlying.ToString();
                    datarow["Contract"] = ContractType.CE.ToString();
                    datarow["TransactionType"] = TransactionType.Buy.ToString();
                    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
                    datarow["LotSize"] = LotSize.BankNifty.ToString();
                    datarow["Premium"] = row.CE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.CE.expiryDate.ToString();

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
                    datarow["ExpiryDate"] = row.CE.expiryDate.ToString();

                    foreach (var item in strikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = Utility.FO.CallSell(row.CE.strikePrice, row.CE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);
                }

                if (row.PE != null && (row.PE.strikePrice <= iUpperStrikePriceRange && row.PE.strikePrice >= iLowerStrikePriceRange))
                {
                    //Add PE Buy row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.PE.underlying.ToString();
                    datarow["Contract"] = ContractType.PE.ToString();
                    datarow["TransactionType"] = TransactionType.Buy.ToString();
                    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
                    datarow["LotSize"] = LotSize.BankNifty.ToString();
                    datarow["Premium"] = row.PE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.PE.expiryDate.ToString();

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
                    datarow["ExpiryDate"] = row.PE.expiryDate.ToString();


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
            dtOCCalculation = AddColumnstoStrategyTable(dtOCCalculation);
            dtOCCalculation = AddRowstoStrategyTable(dtOCCalculation);

            DataTable dtButterflySpread = new DataTable();
            dtButterflySpread.Columns.Add("Stock");
            dtButterflySpread.Columns.Add("Contract");
            dtButterflySpread.Columns.Add("TransactionType");
            dtButterflySpread.Columns.Add("StrikePrice");
            dtButterflySpread.Columns.Add("LotSize");
            dtButterflySpread.Columns.Add("Premium");
            dtButterflySpread.Columns.Add("ProfitLoss");

            for (int iCount = 1; iCount <= iStrategyCount; iCount++)
            {
                double iLowerSP = currentStrikePrice - (iCount * diffStrikePrice);
                double iHighterSP = currentStrikePrice + (iCount * diffStrikePrice);

                string selectQuery = "(Contract = 'CE') AND ((StrikePrice = " +
                    (iLowerSP) +
                    " AND TransactionType = 'Buy') OR (StrikePrice = " +
                    currentStrikePrice +
                    " AND TransactionType = 'Sell') OR (StrikePrice = " +
                    (iHighterSP) +
                    " AND TransactionType = 'Buy')) AND (ExpiryDate = #" + ddlExpiryDates.SelectedValue + "#)";

                DataRow[] drs = dtOCCalculation.Select(selectQuery, "StrikePrice ASC");
                //make a new "results" datatable via clone to keep structure
                DataTable dtButterflySpreadCEStrategy = dtOCCalculation.Clone();
                //Import the Rows
                foreach (DataRow d in drs)
                {
                    dtButterflySpreadCEStrategy.ImportRow(d);
                }

                DataTable dtButterflySpreadCEStrategyResult = dtButterflySpread.Clone();
                for (int i = 0; i < dtButterflySpreadCEStrategy.Rows.Count; i++)
                {
                    double sum = 0;
                    var strikePrice = dtButterflySpreadCEStrategy.Rows[i]["StrikePrice"].ToString(); //30400

                    for (int j = 0; j < dtButterflySpreadCEStrategy.Rows.Count; j++)
                    {
                        var rowStrikePrice = dtButterflySpreadCEStrategy.Rows[j]["StrikePrice"].ToString(); //30400
                        //var columnStrikePrice = dtButterflySpreadCEStrategy.Rows[j][strikePrice].ToString(); //-255
                        if (rowStrikePrice == currentStrikePrice.ToString())
                        {
                            sum += Convert.ToDouble(dtButterflySpreadCEStrategy.Rows[j][strikePrice]) * 2;
                        }
                        else if (rowStrikePrice == iLowerSP.ToString() || rowStrikePrice == iHighterSP.ToString())
                        {
                            sum += Convert.ToDouble(dtButterflySpreadCEStrategy.Rows[j][strikePrice]);//-255,
                        }
                    }

                    dtButterflySpreadCEStrategyResult.Rows.Add(new string[] {
                    dtButterflySpreadCEStrategy.Rows[i]["Stock"].ToString(),
                    dtButterflySpreadCEStrategy.Rows[i]["Contract"].ToString(),
                    dtButterflySpreadCEStrategy.Rows[i]["TransactionType"].ToString(),
                    dtButterflySpreadCEStrategy.Rows[i]["StrikePrice"].ToString(),
                    dtButterflySpreadCEStrategy.Rows[i]["LotSize"].ToString(),
                    dtButterflySpreadCEStrategy.Rows[i]["Premium"].ToString(),
                    Math.Round(sum,2).ToString()});
                }
                gvOptionChain.Visible = false;
                FillBFSpread(dtButterflySpreadCEStrategyResult);
            }
        }
        private void UpdateStrikePriceDetails()
        {
            List<int> prices = recordsObject.strikePrices;
            diffStrikePrice = (prices[1] - prices[0]);
            currentStrikePrice = ((int)Math.Round(currentPrice / diffStrikePrice)) * diffStrikePrice; ;
            lowerStrikePrice = currentStrikePrice - diffStrikePrice;
            higherStrikePrice = currentStrikePrice + diffStrikePrice;
            iLowerStrikePriceRange = currentStrikePrice - diffStrikePrice * iCalColumnPercentage;
            iUpperStrikePriceRange = currentStrikePrice + diffStrikePrice * iCalColumnPercentage;
        }
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnShowCalculation_Click(object sender, EventArgs e)
        {
            dtOCCalculation = AddColumnstoStrategyTable(dtOCCalculation);
            dtOCCalculation = AddRowstoStrategyTable(dtOCCalculation);

            string selectQuery = "(ExpiryDate = #" + ddlExpiryDates.SelectedValue + "#)";

            DataRow[] drs = dtOCCalculation.Select(selectQuery, "StrikePrice ASC");
            //make a new "results" datatable via clone to keep structure
            DataTable dtData = dtOCCalculation.Clone();
            //Import the Rows
            foreach (DataRow d in drs)
            {
                dtData.ImportRow(d);
            }

            gvOptionChain.Visible = false;
            FillBFSpread(dtData);
        }
    }
}
