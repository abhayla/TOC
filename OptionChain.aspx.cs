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
    public partial class OptionChain : System.Web.UI.Page
    {
        string mainurl = "https://www.nseindia.com/api/option-chain-indices?symbol=BANKNIFTY";
        protected void Page_Load(object sender, EventArgs e)
        {
            Uri myUri = new Uri(mainurl, UriKind.Absolute);
            System.Net.WebRequest request = System.Net.HttpWebRequest.Create(myUri);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
            String newestversion = sr.ReadToEnd();
            /*
            WebRequest webRequest = WebRequest.Create(mainurl);
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            if (response.StatusDescription == "OK")
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //dynamic data = JObject.Parse(responseFromServer);

                //Console.Write(data.name);
            }
            */
            //LoadJson();
        }

        public void LoadJson()
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(mainurl);
            request.ContentType = "application/json";
            //request.UserAgent = "Nothing";
            //request.Accept = "*/*";
            //request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            //request.Host = "nseindia.com";
            //request.Referer = "http://nseindia.com";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:28.0) Gecko/20100101 Firefox/28.0";
            //request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            OCItem m = JsonConvert.DeserializeObject<OCItem>(responseString);
        }

        public class OCItem
        {
            public string CEchart;
            public float CEopenInterest;
            public float CEchangeinOpenInterest;
            public float CEtotalTradedVolume;
            public float CEimpliedVolatility;
            public float CElastPrice;
            public float CEchange;
            public float CEbidQty;
            public float CEbidprice;
            public float CEaskPrice;
            public float CEaskQty;
            public float CEstrikePrice;
            public float PEbidQty;
            public float PEbidprice;
            public float PEaskPrice;
            public float PEaskQty;
            public float PEchange;
            public float PElastPrice;
            public float PEimpliedVolatility;
            public float PEtotalTradedVolume;
            public float PEchangeinOpenInterest;
            public float PEopenInterest;
            public string PEchart;

        }
    }
}