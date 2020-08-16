using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
//using Telegram.Bot.Types.InlineKeyboardButtons;
//using Telegram.Bot.Types;

namespace TOC
{

    public class MyTelegramBot
    {

        private static string botToken = "823309371:AAFDRExv8PiPpAUSWK8g88lCD2v_EJ_eIqs";
        //private readonly static Td.ClientResultHandler _defaultHandler = new DefaultHandler();
        //private static Td.Client _client = null;

        //private static string[] groups = { "@Puneites", "@Free_Zerodha_Angel_Upstox_Acct", "@AngelBroking_SMC_Upstox_Zerodha" };
        //List<string> groupIds = new List<string>(groups);
        //private long m_ReportGroupChatId = -278455237;  // Exception Reporter
        //private TelegramBotClient m_TelegramBotClient = new TelegramBotClient("replace_your_bot_token");
        //private ReplyKeyboardMarkup m_ReplyMarkupKeyboard = new ReplyKeyboardMarkup();
        //private InlineKeyboardMarkup m_GameKeyboardMarkup = new InlineKeyboardMarkup();
        //private InlineKeyboardMarkup m_AboutKeyboardMarkup = new InlineKeyboardMarkup();

        //private TelegramBotClient m_TelegramBotClient = new TelegramBotClient(botToken);

        //private ReplyKeyboardMarkup m_ReplyMarkupKeyboard = new ReplyKeyboardMarkup();

        //private InlineKeyboardMarkup m_GameKeyboardMarkup = new InlineKeyboardMarkup();
        //private InlineKeyboardMarkup m_AboutKeyboardMarkup = new InlineKeyboardMarkup();

        //public static void ForwardMessage(string toGroupId, string fromGroupId, int messageId)
        public static void ForwardMessage()
        {
            //Telegram.Bot.Types.Update update = new Update();
            //Telegram.Bot.Types.Message message = new Message();
            //Telegram.Bot.Types.Chat chat = new Chat();
            //Telegram.Bot.Types.ChatId chatId = new ChatId("@Puneites");
            //Telegram.Bot.Types.ChatMember chatMember = new ChatMember();
            //Telegram.Bot.Types.ChatPermissions chatPermissions = new ChatPermissions();
            //Telegram.Bot.Types.MessageEntity messageEntity = new MessageEntity();
            //Telegram.Bot.Types.User user = new User();
            //Telegram.Bot.Types.Venue venue = new Venue();
            //Telegram.Bot.Types.WebhookInfo webhookInfo = new WebhookInfo();
            //Telegram.Bot.Requests.ForwardMessageRequest forwardMessageRequest = new Telegram.Bot.Requests.ForwardMessageRequest();
            //Telegram.Bot.Requests.GetChatMemberRequest getChatMemberRequest = new Telegram.Bot.Requests.GetChatMemberRequest();
            //Telegram.Bot.Requests.GetChatMembersCountRequest getChatMembersCountRequest = new Telegram.Bot.Requests.GetChatMembersCountRequest();
            //Telegram.Bot.Requests.GetChatRequest getChatRequest = new Telegram.Bot.Requests.GetChatRequest();
            //Telegram.Bot.Requests.SendMessageRequest sendMessageRequest = new Telegram.Bot.Requests.SendMessageRequest();

            string txtMessage = "How to Make Money in Intraday Trading: A Master Class By One of India’s Most Famous Traders. Check this out.\n https://amzn.to/39x1eMx";
            TelegramBotClient m_TelegramBotClient = new TelegramBotClient(botToken);
            if (isInternetConnected())
            {
                //m_TelegramBotClient.ForwardMessageAsync("@AngelBroking_Upstox_Zerodha", "@Upstox", 1);
                m_TelegramBotClient.SendTextMessageAsync("@AngelBroking_Upstox_Zerodha", txtMessage, ParseMode.Html);
            }
        }

        public static void SendTelegramMessagewithButtons(string message, string[] groupIds)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("👍", "CallBackData"),
                            InlineKeyboardButton.WithCallbackData("👎🏿","CallBackData")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("Upstox", "https://upstox.com/upstox-pro-web/"),
                            InlineKeyboardButton.WithUrl("Zerodha", "https://drive.google.com/open?id=1ImGuf95HhZeksaNSDmBQSzq_8jHTJepo")
                        }
                    });

            TelegramBotClient m_TelegramBotClient = new TelegramBotClient(botToken);
            if (isInternetConnected())
            {
                foreach (var groupId in groupIds)
                {
                    m_TelegramBotClient.SendTextMessageAsync(groupId.Trim(), message, replyMarkup: keyboard);
                }
            }
        }
        public static void SendTelegramMessagewithoutButtons(string message, string[] rowValues)
        {
            TelegramBotClient m_TelegramBotClient = new TelegramBotClient(botToken);
            if (isInternetConnected())
            {
                foreach (var groupId in rowValues)
                {
                    m_TelegramBotClient.SendTextMessageAsync(groupId.Trim(), message, ParseMode.Html);
                }
            }
        }

        private static bool isInternetConnected()
        {
            try
            {
                Telegram.Bot.TelegramBotClient teleBot = new Telegram.Bot.TelegramBotClient(botToken);
                var me = teleBot.GetMeAsync().Result;
                return true;
            }
            catch
            {
                return false;
            }
        }
        //private static void sendMessage(long chatId, string message)
        //{
        //    // initialize reply markup just for testing
        //    TdApi.InlineKeyboardButton[] row = { new TdApi.InlineKeyboardButton("https://telegram.org?1", new TdApi.InlineKeyboardButtonTypeUrl()), new TdApi.InlineKeyboardButton("https://telegram.org?2", new TdApi.InlineKeyboardButtonTypeUrl()), new TdApi.InlineKeyboardButton("https://telegram.org?3", new TdApi.InlineKeyboardButtonTypeUrl()) };
        //    TdApi.ReplyMarkup replyMarkup = new TdApi.ReplyMarkupInlineKeyboard(new TdApi.InlineKeyboardButton[][] { row, row, row });

        //    TdApi.InputMessageContent content = new TdApi.InputMessageText(new TdApi.FormattedText(message, null), false, true);
        //    _client.Send(new TdApi.SendMessage(chatId, 0, null, replyMarkup, content), _defaultHandler);
        //}



        public static void SendTelegramMessage(string message, string[] rowValues)
        {
            try
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
            catch (Exception ex)
            {
                MyTelegramBot.SendTelegramMessage("SendTelegramMessage exception. - " + ex.Message, rowValues);
            }
        }


    }
}