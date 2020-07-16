using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Threading;
using System.Text;
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

        //string mainurl = "https://www1.nseindia.com/marketinfo/sym_map/symbolMapping.jsp?symbol=BANKNIFTY&instrument=OPTIDX&date=-&segmentLink=17";
        //string mainurl = "https://www1.nseindia.com/live_market/dynaContent/live_watch/option_chain/optionKeys.jsp?symbolCode=-10006&symbol=NIFTY&symbol=NIFTY&instrument=-&date=-&segmentLink=17&symbolCount=2&segmentLink=17";
        //Records recordsObject = new Records();
        //Filtered filteredObject = new Filtered();
        double currentPrice = 0.0;
        //double higherStrikePrice = 0.0;
        //double lowerStrikePrice = 0.0;
        double currentStrikePrice = 0.0;
        int diffStrikePrice = 0;
        //int iStrategyCount = 2;

        //TimeSpan timeGapForNewOC = new TimeSpan(0, 5, 0);
        //TimeSpan timeLastOCFetched = new TimeSpan(0, 0, 0);

        //DataTable dtOCCalculation = new DataTable();
        int iCalColumnPercentage = 5;
        double iUpperStrikePriceRange = 0.0;
        double iLowerStrikePriceRange = 0.0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    DataTable dtData = GetOCForGivenOption(rblOCType.SelectedValue, ddlExpiryDates.SelectedValue);
                    FillOCGridview(dtData);
                }
            }
            catch (Exception ex)
            {
                // You might want to handle some specific errors : Just pass on up for now...
                // Remove this catch if you don't want to handle errors here.
                throw;
            }
        }

        private DataTable GetOCForGivenOption(string ocType, string expityDate)
        {
            FetchOC(ocType);
            string selectQuery = string.Empty;

            //DataTable dt = toDataTable(filteredObject);
            DataTable dt = OCHelper.toDataTable(MySession.Current.RecordsObject);

            if (!expityDate.Trim().Equals(string.Empty))
            {
                selectQuery = "(ExpiryDate = #" + expityDate + "#)";
            }
            else
            {
                selectQuery = "(ExpiryDate = #" + ddlExpiryDates.SelectedValue + "#)";
            }

            DataRow[] drs = dt.Select(selectQuery, "StrikePrice ASC");

            //make a new "results" datatable via clone to keep structure
            DataTable dtData = dt.Clone();

            //Import the Rows
            foreach (DataRow d in drs)
            {
                dtData.ImportRow(d);
            }

            return dtData;

        }

        private void FetchOC(string ocType)
        {
            MySession.Current.RecordsObject = OCHelper.GetOC(ocType);
            currentPrice = MySession.Current.RecordsObject.underlyingValue;
            FillExpiryDates(ddlExpiryDates);
            UpdateStrikePriceDetails();
        }

        private void FillExpiryDates(DropDownList ddlExpiryDates)
        {
            List<string> expiryDates = MySession.Current.RecordsObject.expiryDates;
            foreach (string item in expiryDates)
            {
                ddlExpiryDates.Items.Add(item);
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Text = "Refreshing...";
            DataTable dtData = GetOCForGivenOption(rblOCType.SelectedValue, ddlExpiryDates.SelectedValue);
            FillOCGridview(dtData);
            btnRefresh.Text = "Refresh Data";
        }

        private void FillOCGridview(DataTable dt)
        {
            dt.Select("", "StrikePrice ASC");
            gvOptionChain.DataSource = dt;
            gvOptionChain.DataBind();
            //gvOptionChain.Columns[dt.Columns["CE" + "bidQty"].Ordinal].Visible = false;
            //gvOptionChain.Columns[dt.Columns["CE" + "bidprice"].Ordinal].Visible = false;
            //gvOptionChain.Columns[dt.Columns["CE" + "askPrice"].Ordinal].Visible = false;
            //gvOptionChain.Columns[dt.Columns["CE" + "askQty"].Ordinal].Visible = false;
            //gvOptionChain.Columns[dt.Columns["PE" + "bidQty"].Ordinal].Visible = false;
            //gvOptionChain.Columns[dt.Columns["PE" + "bidprice"].Ordinal].Visible = false;
            //gvOptionChain.Columns[dt.Columns["PE" + "askPrice"].Ordinal].Visible = false;
            //gvOptionChain.Columns[dt.Columns["PE" + "askQty"].Ordinal].Visible = false;
        }

        private List<int> GetStrikePrices()
        {
            List<int> strikePrices = MySession.Current.RecordsObject.strikePrices;
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
            foreach (var row in MySession.Current.RecordsObject.data)
            {
                if (row.CE != null && (row.CE.strikePrice <= iUpperStrikePriceRange && row.CE.strikePrice >= iLowerStrikePriceRange))
                {
                    //Add CE Buy row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.CE.underlying.ToString();
                    datarow["Contract"] = enumContractType.CE.ToString();
                    datarow["TransactionType"] = enumTransactionType.BUY.ToString();
                    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
                    datarow["LotSize"] = Convert.ToInt32(enumLotSize.BankNifty).ToString();
                    datarow["Premium"] = row.CE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.CE.expiryDate.ToString();

                    foreach (var item in strikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = FO.CallBuy(row.CE.strikePrice, row.CE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);

                    //Add CE Sell row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.CE.underlying.ToString();
                    datarow["Contract"] = enumContractType.CE.ToString();
                    datarow["TransactionType"] = enumTransactionType.SELL.ToString();
                    datarow["StrikePrice"] = row.CE.strikePrice.ToString();
                    datarow["LotSize"] = Convert.ToInt32(enumLotSize.BankNifty).ToString();
                    datarow["Premium"] = row.CE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.CE.expiryDate.ToString();

                    foreach (var item in strikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = FO.CallSell(row.CE.strikePrice, row.CE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);
                }

                if (row.PE != null && (row.PE.strikePrice <= iUpperStrikePriceRange && row.PE.strikePrice >= iLowerStrikePriceRange))
                {
                    //Add PE Buy row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.PE.underlying.ToString();
                    datarow["Contract"] = enumContractType.PE.ToString();
                    datarow["TransactionType"] = enumTransactionType.BUY.ToString();
                    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
                    datarow["LotSize"] = Convert.ToInt32(enumLotSize.BankNifty).ToString();
                    datarow["Premium"] = row.PE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.PE.expiryDate.ToString();

                    foreach (var item in strikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = FO.PutBuy(row.PE.strikePrice, row.PE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);

                    //Add PE Sell row
                    datarow = dt.NewRow();
                    datarow["Stock"] = row.PE.underlying.ToString();
                    datarow["Contract"] = enumContractType.PE.ToString();
                    datarow["TransactionType"] = enumTransactionType.SELL.ToString();
                    datarow["StrikePrice"] = row.PE.strikePrice.ToString();
                    datarow["LotSize"] = Convert.ToInt32(enumLotSize.BankNifty).ToString();
                    datarow["Premium"] = row.PE.lastPrice.ToString();
                    datarow["ExpiryDate"] = row.PE.expiryDate.ToString();


                    foreach (var item in strikePrices)
                    {
                        datarow[dt.Columns[item.ToString()].ColumnName] = FO.PutSell(row.PE.strikePrice, row.PE.lastPrice, Convert.ToDouble(item));
                    }
                    dt.Rows.Add(datarow);
                }
            }
            return dt;
        }

        private void UpdateStrikePriceDetails()
        {
            List<int> prices = MySession.Current.RecordsObject.strikePrices;
            if (prices.Count > 2)
            {
                diffStrikePrice = (prices[1] - prices[0]);
                currentStrikePrice = ((int)Math.Round(currentPrice / diffStrikePrice)) * diffStrikePrice; ;
                //lowerStrikePrice = currentStrikePrice - diffStrikePrice;
                //higherStrikePrice = currentStrikePrice + diffStrikePrice;
                iLowerStrikePriceRange = currentStrikePrice - diffStrikePrice * iCalColumnPercentage;
                iUpperStrikePriceRange = currentStrikePrice + diffStrikePrice * iCalColumnPercentage;
            }
        }

        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void rblOCType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtData = GetOCForGivenOption(rblOCType.SelectedValue, ddlExpiryDates.SelectedValue);
            FillOCGridview(dtData);
        }
    }
}
