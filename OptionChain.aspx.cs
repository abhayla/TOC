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
using System.Data;


namespace TOC
{
    public partial class OptionChain : System.Web.UI.Page
    {
        private const string NSEIndiaWebsiteURL = "https://www1.nseindia.com";
        //private const string mainurl = NSEIndiaWebsiteURL + "/live_market/dynaContent/live_watch/stock_watch/niftyStockWatch.json";
        //string mainurl = "https://docs.microsoft.com";
        string mainurl = "https://www.nseindia.com/api/option-chain-indices?symbol=BANKNIFTY";
        //string mainurl = "https://www1.nseindia.com/marketinfo/sym_map/symbolMapping.jsp?symbol=NIFTY&instrument=OPTIDX&date=-&segmentLink=17";
        //string mainurl = "https://www1.nseindia.com/live_market/dynaContent/live_watch/option_chain/optionKeys.jsp?symbolCode=-10006&symbol=NIFTY&symbol=NIFTY&instrument=-&date=-&segmentLink=17&symbolCount=2&segmentLink=17";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                JObject jObject = DownloadJSONDataFromURL(mainurl);
                //FillDataTable(toDataTable(jObject));

                Records recordsObject = JsonConvert.DeserializeObject<Records>(jObject["records"].ToString());
                Filtered filteredObject = JsonConvert.DeserializeObject<Filtered>(jObject["filtered"].ToString());
                DataTable dtRecords = toDataTable(recordsObject);
                DataTable dtFiltered = toDataTable(filteredObject);
            }
            catch (Exception ex)
            {
                // You might want to handle some specific errors : Just pass on up for now...
                // Remove this catch if you don't want to handle errors here.
                throw;
            }
        }
        private static DataTable toDataTable(Records recordsObject)
        {


            var result = new DataTable();

            return result;
        }
        private static DataTable toDataTable(Filtered filteredObject)
        {
            var result = new DataTable();
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

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Text = "Refreshing...";
            JObject equitiesStockWatchDataJObject = DownloadJSONDataFromURL(mainurl);
            //JArray equitiesStockWatchDataJObject1 = new JArray(equitiesStockWatchDataJObject["filtered"]["data"]);
            //IDictionary<string, object> dict = equitiesStockWatchDataJObject1.ToDictionary(k => ((JObject)k).Properties().First().Name, v => v.Values().First().Value<object>());
            //var values = JsonConvert.DeserializeObject<OptionChainColumns>(equitiesStockWatchDataJObject1);
            //int count = new System.Linq.SystemCore_EnumerableDebugView<Newtonsoft.Json.Linq.JToken>(new System.Linq.SystemCore_EnumerableDebugView<Newtonsoft.Json.Linq.JToken>(equitiesStockWatchDataJObject1).Items[0]).Items.Count;

            //DataTable tester = (DataTable)JsonConvert.DeserializeObject(equitiesStockWatchDataJObject1.ToString(), (typeof(DataTable)));

        }

        private void FillDataTable(DataTable dt)
        {
            gvData.DataSource = dt;
            gvData.DataBind();
        }


        private static DataTable toDataTable(JObject jObj)
        {
            JArray jArrayCE = new JArray(jObj["filtered"]["data"][0]["CE"]);
            JArray jArrayPE = new JArray(jObj["filtered"]["data"][0]["PE"]);
            //jArrayCE.Add(jArrayPE);

            JArray jArrayRows = new JArray(jObj["filtered"]["data"]);
            var result = new DataTable();

            //Initialize the columns, If you know the row type, replace this   
            foreach (var row in jArrayCE)
            {
                foreach (var jToken in row)
                {
                    var jproperty = jToken as JProperty;
                    if (jproperty == null) continue;
                    if (result.Columns[jproperty.Name] == null)
                        result.Columns.Add("CE"+jproperty.Name, typeof(string));
                }
            }
            foreach (var row in jArrayPE)
            {
                foreach (var jToken in row)
                {
                    var jproperty = jToken as JProperty;
                    if (jproperty == null) continue;
                    if (result.Columns[jproperty.Name] == null)
                        result.Columns.Add("PE" + jproperty.Name, typeof(string));
                }
            }

            foreach (var row in jArrayRows)
            {
                var datarow = result.NewRow();
                foreach (var jToken in row)
                {
                    var jPropertyCE = jToken["CE"] as JProperty;
                    var jPropertyPE = jToken["PE"] as JProperty;
                    if (jPropertyCE != null)
                    {
                        datarow["CE" + jPropertyCE.Name] = jPropertyCE.Value.ToString();
                    }
                    else if (jPropertyPE != null)
                    {
                        datarow["PE" + jPropertyPE.Name] = jPropertyPE.Value.ToString();
                    }
                    else continue;
                }
                result.Rows.Add(datarow);
            }

            return result;
        }
    }
}