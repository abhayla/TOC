using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using System.Net;
using System.IO;
using System.Text;

namespace TOC
{
    public class Telegram
    {
        private static string botToken = "823309371:AAFDRExv8PiPpAUSWK8g88lCD2v_EJ_eIqs";


        //private static string[] groups = { "@Puneites", "@Free_Zerodha_Angel_Upstox_Acct", "@AngelBroking_SMC_Upstox_Zerodha" };
        //List<string> groupIds = new List<string>(groups);
        private static bool isInternetConnected()
        {
            try
            {
                TelegramBotClient teleBot = new TelegramBotClient(botToken);
                var me = teleBot.GetMeAsync().Result;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void SendTelegramMessage(string message, string[] rowValues)
        {

            string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
            if (isInternetConnected())
            {
                foreach (var groupId in rowValues)
                {
                    string urlString1 = String.Format(urlString, botToken, groupId.Trim(), message);
                    WebRequest request = WebRequest.Create(urlString1);
                    Stream rs = request.GetResponse().GetResponseStream();
                }
            }
        }
    }
}