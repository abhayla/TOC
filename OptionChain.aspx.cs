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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

namespace TOC
{
    public partial class OptionChain : System.Web.UI.Page
    {
        private const string NSEIndiaWebsiteURL = "https://www1.nseindia.com";
        private const string mainurl = NSEIndiaWebsiteURL + "/live_market/dynaContent/live_watch/stock_watch/niftyStockWatch.json";
        //string mainurl = "https://docs.microsoft.com";
        //string mainurl = "https://www.nseindia.com/api/option-chain-indices?symbol=BANKNIFTY";
        //string mainurl = "https://www1.nseindia.com/marketinfo/sym_map/symbolMapping.jsp?symbol=NIFTY&instrument=OPTIDX&date=-&segmentLink=17";
        //string mainurl = "https://www1.nseindia.com/live_market/dynaContent/live_watch/option_chain/optionKeys.jsp?symbolCode=-10006&symbol=NIFTY&symbol=NIFTY&instrument=-&date=-&segmentLink=17&symbolCount=2&segmentLink=17";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DownloadJSONDataFromURL(mainurl);
            }
            catch (Exception ex)
            {
                // You might want to handle some specific errors : Just pass on up for now...
                // Remove this catch if you don't want to handle errors here.
                throw;
            }
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

                // Serialise it into a JObject
                JObject jObject = JObject.Parse(stockWatchJSONString);

                return jObject;

            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Text = "Refreshing...";
            JObject equitiesStockWatchDataJObject = null;
            equitiesStockWatchDataJObject = DownloadJSONDataFromURL(mainurl);
            gvData.DataSource = equitiesStockWatchDataJObject;
            gvData.DataBind();
        }
    }
}