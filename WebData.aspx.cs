using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace TOC
{
    public partial class WebData : System.Web.UI.Page
    {
        
        private static string[] json_path = new string[]{
            "/live_market/dynaContent/live_watch/stock_watch/niftyStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/juniorNiftyStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/niftyMidcap50StockWatch.json ",
            "/live_market/dynaContent/live_watch/stock_watch/cnxAutoStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/bankNiftyStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxEnergyStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxFinanceStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxFMCGStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxitStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxMediaStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxMetalStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxPharmaStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxPSUStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxRealtyStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/niftyPvtBankStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxCommoditiesStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxConsumptionStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cpseStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxInfraStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxMNCStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/ni15StockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxPSEStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxServiceStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/nseliquidStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/niftyMidcapLiq15StockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cnxDividendOpptStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/nv20StockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/niftyQuality30StockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/foSecStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/etfStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/goldEtfStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/nifty50EtfStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/iLStockWatch.json",
            "/live_market/dynaContent/live_watch/stock_watch/cbmSecListStockWatch.json"
        };

        private static SortedList<string, SortedSet<Func<LiveData, int>>> callbackData = new SortedList<string, SortedSet<Func<LiveData, int>>>();

        private static SortedList<string, CompanyData> dataSet = new SortedList<string, CompanyData>();

        public static void StartFetch()
        {
            while (true)
            {
                try
                {
                    /*foreach (var s in callbackData)
                    {
                        LiveData d = GetLiveData(s.Key);
                        foreach (var f in s.Value)
                        {
                            f(d);
                        }
                    }*/
                    LiveData d = GetLiveData("SBIN");
                }
                catch { }
            }
        }
        public static LiveData GetLiveData(string symbol)
        {
            //xMH7BiBu6s24LHCizug3
            //https://www.quandl.com/api/v3/datasets/NSE/ABB.json?api_key=xMH7BiBu6s24LHCizug3
            string mainurl1 = "https://www.nseindia.com/api/option-chain-indices?symbol=BANKNIFTY";
            //var request = (HttpWebRequest)WebRequest.Create("http://nseindia.com/live_market/dynaContent/live_watch/get_quote/GetQuote.jsp?symbol=" + symbol + "&illiquid=0");
            
            var request = (HttpWebRequest)WebRequest.Create(mainurl1);
            request.Accept = "*/*";
            request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            request.Host = "nseindia.com";
            //request.Referer = "http://nseindia.com/live_market/dynaContent/live_watch/get_quote/GetQuote.jsp?symbol=" + symbol + "&illiquid=0";
            request.Referer= mainurl1;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:28.0) Gecko/20100101 Firefox/28.0";
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            int start = responseString.IndexOf("responseDiv");
            start = responseString.IndexOf(',', start);
            int end = responseString.IndexOf("</div>", start);

            responseString = responseString.Substring(start, end - start);
            return readString(responseString);
        }

        private static LiveData readString(string str)
        {
            LiveData d = new LiveData();
            //SortedList<string, string> data = new SortedList<string, string>();
            int start = str.IndexOf(','), end = 0;
            string key = "", val = "";
            while (-1 != start)
            {
                start = str.IndexOf('\"', start);
                start++;
                end = str.IndexOf('\"', start);
                key = str.Substring(start, end - start);
                start = str.IndexOf(':', end);
                start = str.IndexOf('\"', start);
                start++;
                end = str.IndexOf('\"', start);
                val = str.Substring(start, end - start);
                start = str.IndexOf(',', end);
                //data.Add(key, val);
                //d.AddData(key, val);
            }
            return d;
        }
        public static void UpdateData()
        {
            dataSet.Clear();
            foreach (string address in json_path)
            {
                var request = (HttpWebRequest)WebRequest.Create("https://nseindia.com" + address);
                request.Accept = "*/*";
                request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
                request.Host = "nseindia.com";
                request.Referer = "http://nseindia.com";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:28.0) Gecko/20100101 Firefox/28.0";
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                LiveData m = JsonConvert.DeserializeObject<LiveData>(responseString);

                foreach (var d in m.data)
                {
                    if (!dataSet.ContainsKey(d.symbol))
                    {
                        dataSet.Add(d.symbol, d);
                    }
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            StartFetch();
        }
    }
}