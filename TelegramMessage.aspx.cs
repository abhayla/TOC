using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telegram.Bot;
using System.Net;
using System.IO;
using System.Text;

namespace TOC
{
    public partial class TelegramMessage : System.Web.UI.Page
    {
        static string botToken = "";
        private static TelegramBotClient teleBot = new TelegramBotClient(botToken);
        string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
        string groupId = "@Puneites";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public static bool isInternetConnected()
        {
            try
            {
                var me = teleBot.GetMeAsync().Result;
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void btnSendTelegramMessage_Click(object sender, EventArgs e)
        {
            if (isInternetConnected())
            {
                string msgtoSend = txtSendMsg.Text;
                urlString = String.Format(urlString, botToken, groupId, msgtoSend);
                WebRequest request = WebRequest.Create(urlString);
                Stream rs = request.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(rs);
                string line = "";
                StringBuilder sb = new StringBuilder();
                while (line != null)
                {
                    line = reader.ReadLine();
                    if (line != null)
                        sb.Append(line);
                }
                string response = sb.ToString();

            }
        }
    }
}