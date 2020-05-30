using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Threading;
using System.Data;
using System.IO;
using System.Timers;
using System.Security.Authentication.ExtendedProtection;
using System.Globalization;
//using System.Security.AccessControl;
//using System.Security.Principal;

namespace TOC
{
    public partial class ReadFiles : System.Web.UI.Page
    {
        private static string ORDERS_FILE_NAME = "orders.csv";
        private static string MY_ORDERS_FILE_NAME = "myorders.csv";
        private static string ORDER_TRACKER_FILE_NAME = "OrderTracker.csv";
        private static string ORDER_TRACKER_WEEKLY_FILE_NAME = "OrderTrackerWeekly.csv";
        private static string ORDER_TRACKER_MONTHLY_FILE_NAME = "OrderTrackerMonthly.csv";
        private static string ORDER_TRACKER_YEARLY_FILE_NAME = "OrderTrackerYearly.csv";
        private static string LOG_FILE_NAME = "LogFile.csv";
        private static string GROUPS_AND_CHANNELS = "groupschannels.csv";
        private static string ORDERS_FILE_PATH = "C:\\autotrader\\data\\order\\" + ORDERS_FILE_NAME;
        //private static string TOC_FOLDER_PATH = "C:\\Abhay\\SFDC\\Git\\Others\\TOC\\TOC\\";
        //private static string AUTOTRADER_ORDER_FOLDER_PATH = "C:\\autotrader\\data\\order\\";
        private static string MYFILES_FOLDER_PATH = "C:\\Myfiles\\";
        //private static string SENDORDERS_FOLDER_PATH = "C:\\inetpub\\wwwroot\\sendorders\\";
        private static string GROUPS_AND_CHANNELS_FILE_PATH = MYFILES_FOLDER_PATH + GROUPS_AND_CHANNELS;
        private static string FileWatcherPath = @"C:\autotrader\data\order\";
        private static System.Timers.Timer aTimer;
        private int TimeoutMillis = 5000; //1000 is 1 sec
        private int TimerInterval = 60000; //60 seconds
        System.Threading.Timer m_timer = null;
        List<String> files = new List<string>();
        private string[] strGroupsChannelsList;
        private string[] strExceptionChannelsList = { "@Puneites" };
        private string[] strCommodityList = { "@Zerodha_Upstox_CommodityTalk" };
        TimeSpan timePreTradingStarting = new TimeSpan(8, 59, 0);
        TimeSpan timeTradingStarting = new TimeSpan(9, 14, 0);
        TimeSpan timeStartClosePosition = new TimeSpan(15, 0, 0);
        TimeSpan timeClosePosition = new TimeSpan(15, 15, 0);
        TimeSpan timeEquityReport = new TimeSpan(15, 35, 0);
        TimeSpan timeCommodityReport = new TimeSpan(23, 59, 0);
        TimeSpan timeEquityOpen = new TimeSpan(9, 15, 0);
        TimeSpan timeEquityClose = new TimeSpan(15, 30, 0);
        TimeSpan timeCommodityOpen = new TimeSpan(9, 0, 0);
        TimeSpan timeCommodityClose = new TimeSpan(23, 30, 0);

        //For Testing
        //private string[] strCommodityList = { "@Free_Zerodha_Angel_Upstox_Acct" };
        //TimeSpan timePreTradingStarting = new TimeSpan(22, 29, 0);
        //TimeSpan timeTradingStarting = new TimeSpan(22, 30, 0);
        //TimeSpan timeStartClosePosition = new TimeSpan(13, 25, 0);
        //TimeSpan timeClosePosition = new TimeSpan(13, 26, 0);
        //TimeSpan timeEquityReport = new TimeSpan(23, 55, 0);
        //TimeSpan timeCommodityReport = new TimeSpan(13, 28, 0);
        //TimeSpan timeEquityOpen = new TimeSpan(0, 0, 0);
        //TimeSpan timeEquityClose = new TimeSpan(23, 59, 0);
        //TimeSpan timeCommodityOpen = new TimeSpan(0, 0, 0);
        //TimeSpan timeCommodityClose = new TimeSpan(23, 59, 0);


        DataTable logdt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    AddLogColumns(logdt);

                    var wh = new AutoResetEvent(false);
                    var fileSystemWatcher = new FileSystemWatcher();
                    fileSystemWatcher.Path = FileWatcherPath;
                    fileSystemWatcher.Changed += FileSystemWatcher_Changed;
                    fileSystemWatcher.EnableRaisingEvents = true;
                    fileSystemWatcher.Filter = "orders.csv";

                    if (m_timer == null)
                    {
                        m_timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnWatchedFileChange), null, Timeout.Infinite, Timeout.Infinite);
                    }

                    //Get the list of groups and channels where Telegram message will be posted if these 
                    //group and channels have added the bot
                    strGroupsChannelsList = ReadGroupsChannelsCsvFile();
                    string groupchannelId = string.Empty;
                    foreach (var groupId in strGroupsChannelsList)
                    {
                        groupchannelId += groupId + " ";
                    }
                    lblGroupsChannels.Text = groupchannelId;

                    //Sync Order file with MyOrder files
                    DataTable currentOrdersdt = RemoveOrdersBlankRows(ReadOrdersCsvFile());
                    WriteMyOrdersToCSV(currentOrdersdt);

                    ///Timer events
                    // Create a timer and set a two second interval.
                    aTimer = new System.Timers.Timer();
                    aTimer.Interval = TimerInterval;

                    // Hook up the Elapsed event for the timer. 
                    aTimer.Elapsed += OnTimedEvent;

                    // Have the timer fire repeated events (true is the default)
                    aTimer.AutoReset = true;

                    // Start the timer
                    aTimer.Enabled = true;
                }

                if (logdt.Columns.Count == 0)
                {
                    AddLogColumns(logdt);
                }
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "Page_Load", "Exception", ex.Message);
            }
        }

        private void AddLogColumns(DataTable logdt)
        {
            try
            {
                logdt.Columns.Add("DateTime");
                logdt.Columns.Add("Activity");
                logdt.Columns.Add("ExceptionName");
                logdt.Columns.Add("ExceptionDesc");
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "AddLogColumns", "Exception", ex.Message);
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                //Added mutex to prevent multiple call of this event within a give timeframe by two parallel threads
                Mutex mutex = new Mutex(false, "FSW");
                mutex.WaitOne();

                TimeSpan addOneMinute = new TimeSpan(0, 0, 30);
                if (e.SignalTime.TimeOfDay > timePreTradingStarting && e.SignalTime.TimeOfDay < timePreTradingStarting.Add(addOneMinute))
                {
                    PreTradingStarting();
                }
                if (e.SignalTime.TimeOfDay > timeTradingStarting && e.SignalTime.TimeOfDay < timeTradingStarting.Add(addOneMinute))
                {
                    TradingStarting();
                }
                if (e.SignalTime.TimeOfDay > timeStartClosePosition && e.SignalTime.TimeOfDay < timeStartClosePosition.Add(addOneMinute))
                {
                    StartClosePosition();
                }
                if (e.SignalTime.TimeOfDay > timeClosePosition && e.SignalTime.TimeOfDay < timeClosePosition.Add(addOneMinute))
                {
                    ClosePosition();
                }
                if (e.SignalTime.TimeOfDay > timeEquityReport && e.SignalTime.TimeOfDay < timeEquityReport.Add(addOneMinute))
                {
                    EquityReport();
                }
                if (e.SignalTime.TimeOfDay > timeCommodityReport && e.SignalTime.TimeOfDay < timeCommodityReport.Add(addOneMinute))
                {
                    CommodityReport();
                }

                mutex.ReleaseMutex();
                m_timer.Change(TimeoutMillis, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "OnTimedEvent", "Exception", ex.Message);
            }
        }

        private void PreTradingStarting()
        {
            try
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "PreTradingStarting", "PRE-TRADING", "PRE-TRADING");

                string teleMessage = string.Empty;
                teleMessage = "PRE-TRADING OF MARKETS IS STARTING.\n";
                if (strGroupsChannelsList == null)
                {
                    strGroupsChannelsList = ReadGroupsChannelsCsvFile();
                }

                //MyTelegramBot.SendTelegramMessage(teleMessage, strGroupsChannelsList);
                MyTelegramBot.SendTelegramMessagewithoutButtons(teleMessage, strGroupsChannelsList);
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "PreTradingStarting", "Exception", ex.Message);
            }
        }

        private void TradingStarting()
        {
            try
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "TradingStarting", "Market Starting", "Market Starting");

                string teleMessage = string.Empty;
                teleMessage = "STOCK MARKETS ARE OPEN FOR TRADING NOW.\n";
                if (strGroupsChannelsList == null)
                {
                    strGroupsChannelsList = ReadGroupsChannelsCsvFile();
                }

                //MyTelegramBot.SendTelegramMessage(teleMessage, strGroupsChannelsList);
                MyTelegramBot.SendTelegramMessagewithoutButtons(teleMessage, strGroupsChannelsList);
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "TradingStarting", "Exception", ex.Message);
            }
        }

        private void CommodityReport()
        {
            try
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "CommodityReport", "Commodity report", "Commodity report");

                string teleMessage = string.Empty;
                teleMessage = "COMMODITY REPORT :-\n";
                string teleReport = GetReport(enumExchange.MCX.ToString());
                //teleMessage += GetReport(enumExchange.MCX.ToString());

                if (teleReport.Length > 0)
                {
                    teleMessage += teleReport;
                    if (strGroupsChannelsList == null)
                    {
                        strGroupsChannelsList = ReadGroupsChannelsCsvFile();
                    }
                    //MyTelegramBot.SendTelegramMessage(teleMessage, strGroupsChannelsList);
                    MyTelegramBot.SendTelegramMessagewithoutButtons(teleMessage, strGroupsChannelsList);

                    //Send to Commodity group
                    //MyTelegramBot.SendTelegramMessage(teleMessage, strCommodityList);
                    MyTelegramBot.SendTelegramMessagewithoutButtons(teleMessage, strCommodityList);
                }
                else
                {
                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "CommodityReport", "Blank Commodity report", "Blank Commodity report");
                }
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "CommodityReport", "Exception", ex.Message);
            }
        }

        private void EquityReport()
        {
            try
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "EquityReport", "Equity report", "Equity report");

                string teleMessage = string.Empty;
                teleMessage = "EQUITY REPORT :-\n";
                string teleReport = GetReport(enumExchange.NSE.ToString());
                //teleMessage += GetReport(enumExchange.NSE.ToString());
                if (teleReport.Length > 0)
                {
                    teleMessage += teleReport;
                    if (strGroupsChannelsList == null)
                    {
                        strGroupsChannelsList = ReadGroupsChannelsCsvFile();
                    }
                    //MyTelegramBot.SendTelegramMessage(teleMessage, strGroupsChannelsList);
                    MyTelegramBot.SendTelegramMessagewithoutButtons(teleMessage, strGroupsChannelsList);
                }
                else
                {
                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "EquityReport", "Blank equity report", "Blank equity report");
                }
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "EquityReport", "Exception", ex.Message);
            }
        }

        private DataTable AddColumnsOrderTracker()
        {
            DataTable dtCsv = new DataTable();
            dtCsv.Columns.Add("symbol");
            dtCsv.Columns.Add("exchange");
            dtCsv.Columns.Add("BuyDate");
            dtCsv.Columns.Add("BuyTradeType");
            dtCsv.Columns.Add("BuyPrice");
            dtCsv.Columns.Add("SellDate");
            dtCsv.Columns.Add("SellTradeType");
            dtCsv.Columns.Add("SellPrice");
            dtCsv.Columns.Add("qty");
            dtCsv.Columns.Add("PositionStatus");
            dtCsv.Columns.Add("PositionType");
            dtCsv.Columns.Add("ProfitLoss");
            dtCsv.Columns.Add("ProfitLossPercent");
            return dtCsv;
        }

        private DataTable GetMyOrdersList()
        {
            DataTable mySentOrderdt = new DataTable();
            //if (hasWriteAccessToFolder(TOC_FOLDER_PATH))
            //{
            //    mySentOrderdt = RemoveOrdersBlankRows(ReadMyOrdersCsvFile(TOC_FOLDER_PATH + MY_ORDERS_FILE_NAME));
            //}
            //if (hasWriteAccessToFolder(AUTOTRADER_ORDER_FOLDER_PATH))
            //{
            //    mySentOrderdt = RemoveOrdersBlankRows(ReadMyOrdersCsvFile(AUTOTRADER_ORDER_FOLDER_PATH + MY_ORDERS_FILE_NAME));
            //}
            if (hasWriteAccessToFolder(MYFILES_FOLDER_PATH))
            {
                mySentOrderdt = RemoveOrdersBlankRows(ReadMyOrdersCsvFile(MYFILES_FOLDER_PATH + MY_ORDERS_FILE_NAME));
            }
            //if (hasWriteAccessToFolder(SENDORDERS_FOLDER_PATH))
            //{
            //    mySentOrderdt = RemoveOrdersBlankRows(ReadMyOrdersCsvFile(SENDORDERS_FOLDER_PATH + MY_ORDERS_FILE_NAME));
            //}
            return mySentOrderdt;
        }

        private DataTable GetOrderTrackerRecordsFromCSV()
        {
            DataTable orderTrackerdt = AddColumnsOrderTracker();

            if (hasWriteAccessToFolder(MYFILES_FOLDER_PATH))
            {
                orderTrackerdt = RemoveOrderTrackerBlankRows(ReadOrderTrackerCsvFile(MYFILES_FOLDER_PATH + ORDER_TRACKER_FILE_NAME));
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "GetOrderTrackerRecordsFromCSV", "Has write access in folder ", MYFILES_FOLDER_PATH + ORDER_TRACKER_FILE_NAME);
            }
            else
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "GetOrderTrackerRecordsFromCSV", "No write access in folder ", MYFILES_FOLDER_PATH);
            }

            return orderTrackerdt;
        }

        private DataRow CreateOrderTrackerRecordsandReturnLastOrder(DataRow myOrdersRow)
        {
            bool isPresent = false;
            DataTable orderTrackerdt = GetOrderTrackerRecordsFromCSV();
            DataRow lastOrderdr = orderTrackerdt.NewRow();
            foreach (DataRow orderTrackerRow in orderTrackerdt.Rows)
            {
                if (string.Compare(orderTrackerRow["symbol"].ToString().Trim(), myOrdersRow["symbol"].ToString().Trim()) == 0)
                {
                    isPresent = true;
                    //orderTrackerRow["symbol"] = myOrdersRow["symbol"].ToString().Trim();

                    if (string.Compare(orderTrackerRow["BuyTradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) == 0 &&
                        string.Compare(orderTrackerRow["SellTradeType"].ToString().Trim(), enumTransactionType.SELL.ToString()) != 0 &&
                        string.Compare(myOrdersRow["TradeType"].ToString().Trim(), enumTransactionType.SELL.ToString()) == 0)
                    {
                        orderTrackerRow["SellTradeType"] = myOrdersRow["TradeType"].ToString().Trim();
                        orderTrackerRow["SellPrice"] = myOrdersRow["priceStr"];
                        orderTrackerRow["SellDate"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                        orderTrackerRow["qty"] = myOrdersRow["qty"];
                        orderTrackerRow["ProfitLoss"] = Math.Round(Convert.ToDouble(orderTrackerRow["SellPrice"]) - Convert.ToDouble(orderTrackerRow["BuyPrice"]), 2);
                        orderTrackerRow["ProfitLossPercent"] = Math.Round(((Convert.ToDouble(orderTrackerRow["SellPrice"]) - Convert.ToDouble(orderTrackerRow["BuyPrice"])) * 100 / Convert.ToDouble(orderTrackerRow["BuyPrice"])), 2);
                        orderTrackerRow["PositionStatus"] = enumPositionStatus.Close.ToString();
                        lastOrderdr.ItemArray = orderTrackerRow.ItemArray.Clone() as object[];
                        isPresent = false;

                    }
                    else if (string.Compare(orderTrackerRow["BuyTradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) != 0 &&
                            string.Compare(orderTrackerRow["SellTradeType"].ToString().Trim(), enumTransactionType.SELL.ToString()) == 0 &&
                            string.Compare(myOrdersRow["TradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) == 0)
                    {
                        orderTrackerRow["BuyTradeType"] = myOrdersRow["TradeType"].ToString().Trim();
                        orderTrackerRow["BuyPrice"] = myOrdersRow["priceStr"];
                        orderTrackerRow["BuyDate"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                        orderTrackerRow["qty"] = myOrdersRow["qty"];
                        orderTrackerRow["ProfitLoss"] = Math.Round(Convert.ToDouble(orderTrackerRow["SellPrice"]) - Convert.ToDouble(orderTrackerRow["BuyPrice"]), 2);
                        orderTrackerRow["ProfitLossPercent"] = Math.Round(((Convert.ToDouble(orderTrackerRow["SellPrice"]) - Convert.ToDouble(orderTrackerRow["BuyPrice"])) * 100 / Convert.ToDouble(orderTrackerRow["SellPrice"])), 2);
                        orderTrackerRow["PositionStatus"] = enumPositionStatus.Close.ToString();
                        lastOrderdr.ItemArray = orderTrackerRow.ItemArray.Clone() as object[];
                        isPresent = false;
                    }
                }
                continue;
            }

            if (!isPresent)
            {
                DataRow newdr = orderTrackerdt.NewRow();

                //Common information outside the condition
                newdr["symbol"] = myOrdersRow["symbol"].ToString().Trim();
                newdr["exchange"] = myOrdersRow["exchange"].ToString().Trim();
                newdr["qty"] = myOrdersRow["qty"];
                newdr["PositionStatus"] = enumPositionStatus.Open.ToString();

                if (string.Compare(myOrdersRow["TradeType"].ToString().Trim(), enumTransactionType.SELL.ToString()) == 0)
                {
                    newdr["SellTradeType"] = myOrdersRow["TradeType"].ToString().Trim();
                    newdr["SellPrice"] = myOrdersRow["priceStr"];
                    newdr["SellDate"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    newdr["PositionType"] = enumPositionType.Short.ToString();
                }
                else if (string.Compare(myOrdersRow["TradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) == 0)
                {
                    newdr["BuyTradeType"] = myOrdersRow["TradeType"].ToString().Trim();
                    newdr["BuyPrice"] = myOrdersRow["priceStr"];
                    newdr["BuyDate"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    newdr["PositionType"] = enumPositionType.Long.ToString();
                }
                orderTrackerdt.Rows.Add(newdr);
            }
            WriteDataTable(orderTrackerdt, MYFILES_FOLDER_PATH + ORDER_TRACKER_FILE_NAME);
            logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "CreateOrderTrackerRecords", "Order Tracker file", "filePath: " + MYFILES_FOLDER_PATH + ORDER_TRACKER_FILE_NAME);

            return lastOrderdr;
        }

        private string GetReport(string exhange)
        {
            string teleMessage = string.Empty;
            try
            {
                DataTable orderTrackerdt = new DataTable();
                orderTrackerdt = AddColumnsOrderTracker();
                orderTrackerdt = ReadCsvFile(MYFILES_FOLDER_PATH + ORDER_TRACKER_FILE_NAME, orderTrackerdt);
                orderTrackerdt = RemoveBlankRows(orderTrackerdt);

                foreach (DataRow orderTrackerRow in orderTrackerdt.Rows)
                {
                    DateTime buyDate = DateTime.ParseExact(orderTrackerRow["BuyDate"].ToString(), "MM-dd-yyyy HH:mm:ss", null);
                    DateTime sellDate = DateTime.ParseExact(orderTrackerRow["SellDate"].ToString(), "MM-dd-yyyy HH:mm:ss", null);

                    //Send report only for given exchage
                    //Report to include only those rows which have Buy or Sell date as today date.
                    if (string.Compare(orderTrackerRow["exchange"].ToString().Trim(), exhange) == 0 &&
                        (DateTime.Now.Date.ToString().Equals(buyDate.Date.ToString()) || 
                        DateTime.Now.Date.ToString().Equals(sellDate.Date.ToString())))
                    {
                        if (string.Compare(orderTrackerRow["PositionStatus"].ToString().Trim(), enumPositionStatus.Open.ToString()) == 0)
                        {
                            if (string.Compare(orderTrackerRow["BuyTradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) == 0)
                            {
                                teleMessage += orderTrackerRow["symbol"] + ": " + "BuyPrice: " + Math.Round(Convert.ToDouble(orderTrackerRow["BuyPrice"]), 2) + " Position: " + orderTrackerRow["PositionStatus"] + "\n";
                            }
                            else
                            {
                                teleMessage += orderTrackerRow["symbol"] + ": " + "SellPrice: " + Math.Round(Convert.ToDouble(orderTrackerRow["SellPrice"]), 2) + " Position: " + orderTrackerRow["PositionStatus"] + "\n";
                            }
                        }
                        else
                        {
                            double percentage = Convert.ToDouble(orderTrackerRow["ProfitLossPercent"]);
                            if (percentage > 0)
                            {
                                teleMessage += orderTrackerRow["symbol"] + ": " + "BuyPrice: " + Math.Round(Convert.ToDouble(orderTrackerRow["BuyPrice"]), 2) + " SellPrice: " + Math.Round(Convert.ToDouble(orderTrackerRow["SellPrice"]), 2) + " Profit: " + orderTrackerRow["ProfitLossPercent"] + "%\n";
                            }
                            else
                            {
                                teleMessage += orderTrackerRow["symbol"] + ": " + "BuyPrice: " + Math.Round(Convert.ToDouble(orderTrackerRow["BuyPrice"]), 2) + " SellPrice: " + Math.Round(Convert.ToDouble(orderTrackerRow["SellPrice"]), 2) + " Loss: " + orderTrackerRow["ProfitLossPercent"] + "%\n";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "GetReport", "Exception", ex.Message);
            }
            return teleMessage;
        }

        private void ClosePosition()
        {
            try
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ClosePosition", "Close Position", "Close Position");

                string teleMessage = string.Empty;
                teleMessage = "PLEASE CLOSE YOUR INTRADAY PROSITIONS NOW (OR CONVERT TO DELIVERY)\n" +
                    "Some brokers charge addtional fees for closing your open intraday positions.";
                //MyTelegramBot.SendTelegramMessage(teleMessage, strGroupsChannelsList);
                MyTelegramBot.SendTelegramMessagewithoutButtons(teleMessage, strGroupsChannelsList);
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ClosePosition", "Exception", ex.Message);
            }
        }

        private void StartClosePosition()
        {
            try
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "StartClosePosition", "Start Close Position", "Start Close Position");

                string teleMessage = string.Empty;
                teleMessage = "PLEASE START CLOSING YOUR INTRADAY PROSITIONS (OR CONVERT TO DELIVERY) AND AVOID TAKING ANY ADDITIONAL POSITION\n" +
                    "Some brokers charge addtional fees for closing your open intraday positions.";
                //MyTelegramBot.SendTelegramMessage(teleMessage, strGroupsChannelsList);
                MyTelegramBot.SendTelegramMessagewithoutButtons(teleMessage, strGroupsChannelsList);
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "StartClosePosition", "Exception", ex.Message);
            }
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                Mutex mutex = new Mutex(false, "FSW");
                mutex.WaitOne();

                if (e.Name == ORDERS_FILE_NAME && !files.Contains(e.Name))
                {
                    files.Add(e.Name);
                }

                mutex.ReleaseMutex();
                m_timer.Change(TimeoutMillis, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FileSystemWatcher_Changed", "Exception", ex.Message);
            }
        }

        //add your file processing here
        private void OnWatchedFileChange(object state)
        {
            try
            {
                List<String> backup = new List<string>();
                Mutex mutex = new Mutex(false, "FSW");
                mutex.WaitOne();
                backup.AddRange(files);
                files.Clear();

                foreach (string file in backup)
                {
                    FindNewOrders();
                }

                ViewState["LogTable"] = logdt;
                ShowLog();

                if (hasWriteAccessToFolder(MYFILES_FOLDER_PATH))
                {
                    WriteDataTable(logdt, MYFILES_FOLDER_PATH + LOG_FILE_NAME);
                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "OnWatchedFileChange", "Amibroker order file", "filePath: " + MYFILES_FOLDER_PATH + LOG_FILE_NAME);
                }
                mutex.ReleaseMutex();
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "OnWatchedFileChange", "Exception", ex.Message);
            }
        }

        protected void btnReadOrders_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable newdt = new DataTable();
                newdt = ReadOrdersCsvFile();
                gvFileData.DataSource = newdt;
                gvFileData.DataBind();
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "btnReadOrders_Click", "Exception", ex.Message);
            }
        }

        private void FindNewOrders()
        {
            try
            {
                //double PL = 0.0;
                DataTable currentOrdersdt = RemoveOrdersBlankRows(ReadOrdersCsvFile());
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "ReadOrdersCsvFile", "Row count: " + currentOrdersdt.Rows.Count);

                DataTable mySentOrderdt = GetMyOrdersList();
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "ReadMyOrdersCsvFile", "Row count: " + mySentOrderdt.Rows.Count);

                DataTable addtoSentOrderdt = currentOrdersdt.Clone();
                string teleMessage = string.Empty;

                //compare new file data with old file data to find new orders in the file
                foreach (DataRow currentOrdersRow in currentOrdersdt.Rows)
                {
                    //set the flag to false for each new order
                    bool isNewOrder = true;

                    if (string.Compare(currentOrdersRow["AT_PLACE_ORDER_CMD"].ToString().Trim(), "PLACE_ORDER") == 0 &&
                    currentOrdersRow["symbol"].ToString().Length > 1 &&
                    ((string.Compare(currentOrdersRow["tradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) == 0) ||
                    (string.Compare(currentOrdersRow["tradeType"].ToString().Trim(), enumTransactionType.SELL.ToString()) == 0)))
                    {
                        foreach (DataRow SentOrderRow in mySentOrderdt.Rows)
                        {
                            if (string.Compare(currentOrdersRow["AT_PLACE_ORDER_CMD"].ToString().Trim(), SentOrderRow["AT_PLACE_ORDER_CMD"].ToString().Trim()) == 0 &&
                                string.Compare(currentOrdersRow["symbol"].ToString().Trim(), SentOrderRow["symbol"].ToString().Trim()) == 0 &&
                                string.Compare(currentOrdersRow["tradeType"].ToString().Trim(), SentOrderRow["tradeType"].ToString().Trim()) == 0 &&
                                string.Compare(currentOrdersRow["priceStr"].ToString().Trim(), SentOrderRow["priceStr"].ToString().Trim()) == 0)
                            {
                                isNewOrder = false;
                            }
                        }
                    }
                    else
                    {
                        logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "currentOrdersRow else condition", "Row details: " + mySentOrderdt);
                    }

                    if (isNewOrder)
                    {
                        logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "isNewOrder: ", isNewOrder);

                        if (currentOrdersRow["tradeType"] != null &&
                            currentOrdersRow["symbol"] != null &&
                            currentOrdersRow["targetStr"] != null &&
                            currentOrdersRow["stoplossStr"] != null &&
                            currentOrdersRow["trailingStoplossStr"] != null &&
                            currentOrdersRow["exchange"] != null &&
                            currentOrdersRow["priceStr"] != null)
                        {
                            logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Null check passed", "Not null");

                            //if (chkSendTelegramMessages.Checked)
                            //{
                            if (!DateTime.Now.DayOfWeek.ToString().Equals(DayOfWeek.Saturday) &&
                                !DateTime.Now.DayOfWeek.ToString().Equals(DayOfWeek.Sunday))
                            {
                                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Weekday check passed", DateTime.Now.DayOfWeek.ToString());

                                if ((currentOrdersRow["exchange"].ToString().Equals(enumExchange.MCX.ToString()) && DateTime.Now.TimeOfDay > timeCommodityOpen && DateTime.Now.TimeOfDay < timeCommodityClose) ||
                                    (currentOrdersRow["exchange"].ToString().Equals(enumExchange.NSE.ToString()) && DateTime.Now.TimeOfDay > timeEquityOpen && DateTime.Now.TimeOfDay < timeEquityClose))
                                {
                                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Market time check passed", DateTime.Now.TimeOfDay.ToString());
                                    //Telegram message to be sent
                                    teleMessage = "NEW TREND ALERT:-\n" +
                                                currentOrdersRow["tradeType"].ToString() + " " + currentOrdersRow["symbol"].ToString() + " AT " + Math.Round(Convert.ToDouble(currentOrdersRow["priceStr"]), 1) + "\n" +
                                                "TARGET: OPEN" + "\n" +
                                                "STOP LOSS " + Math.Round(Convert.ToDouble(currentOrdersRow["stoplossStr"]), 1) + "\n" +
                                                "TRAILING SL " + Math.Round(Convert.ToDouble(currentOrdersRow["trailingStoplossStr"]), 1) + " POINTS";

                                    //Add new order to the Sent Order list
                                    addtoSentOrderdt.Rows.Add(currentOrdersRow.ItemArray);
                                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Add new order: ", "currentOrdersRow.ItemArray: " + currentOrdersRow.ItemArray);

                                    //Add new order to order tracker and get the last open order with status
                                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "WriteOrderTrackerToCSV: ", "currentOrdersRow.ItemArray: " + currentOrdersRow.ItemArray);
                                    DataRow lastOrderdr = CreateOrderTrackerRecordsandReturnLastOrder(currentOrdersRow);
                                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "CreateOrderTrackerRecordsandReturnLastOrder return: ", lastOrderdr.ItemArray);

                                    if (lastOrderdr["PositionStatus"] != null && lastOrderdr["PositionStatus"].ToString().Equals(enumPositionStatus.Close.ToString()))
                                    {
                                        logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Position close check passed: ", lastOrderdr["PositionStatus"].ToString());

                                        teleMessage += "\n\n" + "Last trend details: \n" +
                                            "Trend: " + lastOrderdr["PositionType"].ToString() + "\n";
                                        if (lastOrderdr["PositionType"].ToString().Equals(enumPositionType.Long.ToString()))
                                        {
                                            logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Position type: ", lastOrderdr["PositionType"].ToString());

                                            teleMessage += "Buy Price: " + Math.Round(Convert.ToDouble(lastOrderdr["BuyPrice"].ToString()), 1) + "\n" +
                                            "Sell Price: " + Math.Round(Convert.ToDouble(lastOrderdr["SellPrice"].ToString()), 1) + "\n";
                                        }
                                        else
                                        {
                                            logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Position type: ", lastOrderdr["PositionType"].ToString());

                                            teleMessage += "Sell Price: " + Math.Round(Convert.ToDouble(lastOrderdr["SellPrice"].ToString()), 1) + "\n" +
                                            "Buy Price: " + Math.Round(Convert.ToDouble(lastOrderdr["BuyPrice"].ToString()), 1) + "\n";
                                        }

                                        if (Convert.ToDouble(lastOrderdr["ProfitLoss"]) > 0)
                                        {
                                            teleMessage += "Profit : " + lastOrderdr["ProfitLoss"].ToString() + " points \n";
                                        }
                                        else
                                        {
                                            teleMessage += "Loss : " + lastOrderdr["ProfitLoss"].ToString() + " points \n";
                                        }

                                        if (Convert.ToDouble(lastOrderdr["ProfitLossPercent"]) > 0)
                                        {
                                            teleMessage += "Profit %: " + lastOrderdr["ProfitLossPercent"].ToString() + "%";
                                        }
                                        else
                                        {
                                            teleMessage += "Loss %: " + lastOrderdr["ProfitLossPercent"].ToString() + "%";
                                        }
                                    }

                                    //Send telegram message
                                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Sending telegram message: ", "teleMessage and strGroupsChannelsList: " + strGroupsChannelsList + " " + teleMessage);
                                    //MyTelegramBot.SendTelegramMessage(teleMessage, strGroupsChannelsList);
                                    //MyTelegramBot.SendTelegramMessagewithButtons(teleMessage, strGroupsChannelsList);
                                    MyTelegramBot.SendTelegramMessagewithoutButtons(teleMessage, strGroupsChannelsList);

                                    if (currentOrdersRow["exchange"].ToString().Equals(enumExchange.MCX.ToString()))
                                    {
                                        logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Sending telegram message: ", "teleMessage and strCommodityList: " + strCommodityList + " " + teleMessage);
                                        //MyTelegramBot.SendTelegramMessage(teleMessage, strCommodityList);
                                        //MyTelegramBot.SendTelegramMessagewithButtons(teleMessage, strCommodityList);
                                        MyTelegramBot.SendTelegramMessagewithoutButtons(teleMessage, strCommodityList);
                                    }
                                }
                                else
                                {
                                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Outside time range ", "Row details: " + mySentOrderdt);
                                }
                            }
                            else
                            {
                                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Market closed today", DateTime.Now.DayOfWeek.ToString());
                            }
                            //}
                            //else
                            //{
                            //    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Checkbox not checked ", "Checkbox status: " + chkSendTelegramMessages.Checked);
                            //}
                        }
                        else
                        {
                            logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "isMessageNotAlreadySent is false", "Row details: " + mySentOrderdt);
                        }
                    }
                }

                mySentOrderdt.Merge(addtoSentOrderdt);
                mySentOrderdt.AcceptChanges();
                WriteMyOrdersToCSV(mySentOrderdt);
            }
            catch (Exception ex)
            {
                //MyTelegramBot.SendTelegramMessage("FindNewOrders exception. - " + ex.Message, strExceptionChannelsList);
                MyTelegramBot.SendTelegramMessagewithoutButtons("FindNewOrders exception. - " + ex.Message, strExceptionChannelsList);
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "FindNewOrders", "Exception", ex.Message);
            }
        }

        private void WriteMyOrdersToCSV(DataTable mySentOrderdt)
        {
            //if (hasWriteAccessToFolder(TOC_FOLDER_PATH))
            //{
            //    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "WriteMyOrdersToCSV", "Write to file", "filePath: " + TOC_FOLDER_PATH + MY_ORDERS_FILE_NAME);
            //    WriteDataTable(mySentOrderdt, TOC_FOLDER_PATH + MY_ORDERS_FILE_NAME);
            //}
            //if (hasWriteAccessToFolder(AUTOTRADER_ORDER_FOLDER_PATH))
            //{
            //    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "WriteMyOrdersToCSV", "Write to file", "filePath: " + AUTOTRADER_ORDER_FOLDER_PATH + MY_ORDERS_FILE_NAME);
            //    WriteDataTable(mySentOrderdt, AUTOTRADER_ORDER_FOLDER_PATH + MY_ORDERS_FILE_NAME);
            //}
            if (hasWriteAccessToFolder(MYFILES_FOLDER_PATH))
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "WriteMyOrdersToCSV", "Write to file", "filePath: " + MYFILES_FOLDER_PATH + MY_ORDERS_FILE_NAME);
                WriteDataTable(mySentOrderdt, MYFILES_FOLDER_PATH + MY_ORDERS_FILE_NAME);
            }
            //if (hasWriteAccessToFolder(SENDORDERS_FOLDER_PATH))
            //{
            //    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "WriteMyOrdersToCSV", "Write to file", "filePath: " + SENDORDERS_FOLDER_PATH + MY_ORDERS_FILE_NAME);
            //    WriteDataTable(mySentOrderdt, SENDORDERS_FOLDER_PATH + MY_ORDERS_FILE_NAME);
            //}
        }

        private bool hasWriteAccessToFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                return false;
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                //MyTelegramBot.SendTelegramMessage("hasWriteAccessToFolder exception folder - " + folderPath + " - " + ex.Message, strExceptionChannelsList);
                MyTelegramBot.SendTelegramMessagewithoutButtons("hasWriteAccessToFolder exception folder - " + folderPath + " - " + ex.Message, strExceptionChannelsList);
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "hasWriteAccessToFolder ", "Exception ", ex.Message + "Folder Path: " + folderPath);
                return false;
            }
        }

        public void WriteDataTable(DataTable sourceTable, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    IEnumerable<String> items = null;
                    foreach (DataRow row in sourceTable.Rows)
                    {
                        items = row.ItemArray.Select(o => QuoteValue(o?.ToString() ?? String.Empty));
                        writer.WriteLine(String.Join(",", items));
                    }
                    //logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "WriteDataTable", "Write to file", "filePath: " + filePath);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                //MyTelegramBot.SendTelegramMessage("WriteDataTable exception filePath - " + filePath + " - " + ex.Message, strExceptionChannelsList);
                MyTelegramBot.SendTelegramMessagewithoutButtons("WriteDataTable exception filePath - " + filePath + " - " + ex.Message, strExceptionChannelsList);
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "WriteDataTable", "Exception", ex.Message);
            }
        }

        private DataTable RemoveOrderTrackerBlankRows(DataTable withBlankRowsdt)
        {
            DataTable withoutBlankRowsdt = withBlankRowsdt.Clone();
            try
            {
                foreach (DataRow row in withBlankRowsdt.Rows)
                {
                    if (row["symbol"].ToString().Trim().Length >= 3 &&
                        (row["BuyTradeType"].ToString().Trim().Length >= 3 ||
                        row["SellTradeType"].ToString().Trim().Length >= 3))
                    {
                        withoutBlankRowsdt.Rows.Add(row.ItemArray);
                    }
                    else
                    {
                        logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "RemoveOrderTrackerBlankRows", "Blank Row", row);
                    }
                }
                return withoutBlankRowsdt;
            }
            catch (Exception ex)
            {
                //MyTelegramBot.SendTelegramMessage("RemoveOrderTrackerBlankRows exception. - " + ex.Message, strExceptionChannelsList);
                MyTelegramBot.SendTelegramMessagewithoutButtons("RemoveOrderTrackerBlankRows exception. - " + ex.Message, strExceptionChannelsList);
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "RemoveOrderTrackerBlankRows", "Exception", ex.Message);
                return withoutBlankRowsdt;
            }
        }

        private DataTable RemoveBlankRows(DataTable withBlankRowsdt)
        {
            DataTable withoutBlankRowsdt = withBlankRowsdt.Clone();
            try
            {
                foreach (DataRow row in withBlankRowsdt.Rows)
                {
                    if (row["symbol"].ToString().Trim().Length >= 3)
                    {
                        withoutBlankRowsdt.Rows.Add(row.ItemArray);
                    }
                    else
                    {
                        logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "RemoveBlankRows", "This is Blank Row", row);
                    }
                }
                return withoutBlankRowsdt;
            }
            catch (Exception ex)
            {
                //MyTelegramBot.SendTelegramMessage("RemoveBlankRows exception. - " + ex.Message, strExceptionChannelsList);
                MyTelegramBot.SendTelegramMessagewithoutButtons("RemoveBlankRows exception. - " + ex.Message, strExceptionChannelsList);
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "RemoveBlankRows", "Exception", ex.Message);
                return withoutBlankRowsdt;
            }
        }

        private DataTable RemoveOrdersBlankRows(DataTable withBlankRowsdt)
        {
            DataTable withoutBlankRowsdt = withBlankRowsdt.Clone();
            try
            {
                foreach (DataRow row in withBlankRowsdt.Rows)
                {
                    if (row["AT_PLACE_ORDER_CMD"].ToString().Trim().Length >= 3 ||
                        row["symbol"].ToString().Trim().Length >= 3 ||
                        row["tradeType"].ToString().Trim().Length >= 3 ||
                        row["productType"].ToString().Trim().Length >= 3 ||
                        row["orderType"].ToString().Trim().Length >= 3)
                    {
                        withoutBlankRowsdt.Rows.Add(row.ItemArray);
                    }
                    else
                    {
                        logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "RemoveBlankRows", "Blank Row", row);
                    }
                }
                return withoutBlankRowsdt;
            }
            catch (Exception ex)
            {
                //MyTelegramBot.SendTelegramMessage("RemoveBlankRows exception. - " + ex.Message, strExceptionChannelsList);
                MyTelegramBot.SendTelegramMessagewithoutButtons("RemoveBlankRows exception. - " + ex.Message, strExceptionChannelsList);
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "RemoveBlankRows", "Exception", ex.Message);
                return withoutBlankRowsdt;
            }
        }

        private static string QuoteValue(string value)
        {
            string str = String.Concat("", value.Replace("\r\r", "\r"), "");
            return str;
        }

        public string[] ReadGroupsChannelsCsvFile()
        {
            string[] rowValues = { };
            string Fulltext;
            try
            {
                using (StreamReader sr = new StreamReader(GROUPS_AND_CHANNELS_FILE_PATH))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows  

                        for (int i = 0; i < rows.Count(); i++)
                        {
                            rowValues = rows[i].Split(','); //split each row with comma to get individual values
                        }
                    }
                    sr.Close();
                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ReadGroupsChannelsCsvFile", "Reader", "Path: " + GROUPS_AND_CHANNELS_FILE_PATH);
                }
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ReadGroupsChannelsCsvFile", "Exception", ex.Message);
            }
            return rowValues;
        }

        public DataTable ReadOrderTrackerCsvFile(string filePath)
        {
            DataTable dtCsv = AddColumnsOrderTracker();
            string Fulltext;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows 

                        for (int i = 0; i < rows.Count(); i++)
                        {
                            string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values
                            {
                                DataRow dr = dtCsv.NewRow();
                                for (int k = 0; k < rowValues.Count(); k++)
                                {
                                    dr[k] = rowValues[k].ToString().Replace("\r", "");
                                }
                                dtCsv.Rows.Add(dr); //add other rows
                            }
                        }
                    }
                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ReadOrderTrackerCsvFile", "Reader", "Path: " + filePath);
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ReadOrderTrackerCsvFile", "Exception", ex.Message);
            }
            return dtCsv;
        }

        public DataTable ReadCsvFile(string filePath, DataTable dtCsv)
        {
            string Fulltext;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows 

                        for (int i = 0; i < rows.Count(); i++)
                        {
                            string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values
                            {
                                DataRow dr = dtCsv.NewRow();
                                for (int k = 0; k < rowValues.Count(); k++)
                                {
                                    dr[k] = rowValues[k].ToString().Replace("\r", "");
                                }
                                dtCsv.Rows.Add(dr); //add other rows
                            }
                        }
                    }
                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ReadOrderTrackerCsvFile", "Reader", "Path: " + filePath);
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ReadOrderTrackerCsvFile", "Exception", ex.Message);
            }
            return dtCsv;
        }

        public DataTable ReadMyOrdersCsvFile(string filePath)
        {
            DataTable dtCsv = AddColumnsToAmibrokerOrders();
            string Fulltext;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows 

                        for (int i = 0; i < rows.Count(); i++)
                        {
                            string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values
                            {
                                DataRow dr = dtCsv.NewRow();
                                for (int k = 0; k < rowValues.Count(); k++)
                                {
                                    dr[k] = rowValues[k].ToString().Replace("\r", "");
                                }
                                dtCsv.Rows.Add(dr); //add other rows
                            }
                        }
                    }
                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ReadMyOrdersCsvFile", "Reader", "Path: " + filePath);
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ReadMyOrdersCsvFile", "Exception", ex.Message);
            }
            return dtCsv;
        }

        private DataTable AddColumnsToAmibrokerOrders()
        {
            DataTable dtCsv = new DataTable();
            try
            {
                dtCsv.Columns.Add("AT_PLACE_ORDER_CMD");
                dtCsv.Columns.Add("id");
                dtCsv.Columns.Add("symbol");
                dtCsv.Columns.Add("tradeType");
                dtCsv.Columns.Add("productType");
                dtCsv.Columns.Add("orderType");
                dtCsv.Columns.Add("qty");
                dtCsv.Columns.Add("priceStr");
                dtCsv.Columns.Add("triggerPriceStr");
                dtCsv.Columns.Add("disclosedQuantity");
                dtCsv.Columns.Add("exchange");
                dtCsv.Columns.Add("instrument");
                dtCsv.Columns.Add("optionType");
                dtCsv.Columns.Add("strikePriceStr");
                dtCsv.Columns.Add("expiry");
                dtCsv.Columns.Add("clientId");
                dtCsv.Columns.Add("validity");
                dtCsv.Columns.Add("traderType");
                dtCsv.Columns.Add("marketProtectionPctStr");
                dtCsv.Columns.Add("strategyId");
                dtCsv.Columns.Add("publishTime");
                dtCsv.Columns.Add("commentsStr");
                dtCsv.Columns.Add("variety");
                dtCsv.Columns.Add("targetStr");
                dtCsv.Columns.Add("stoplossStr");
                dtCsv.Columns.Add("trailingStoplossStr");
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "AddColumns", "Exception", ex.Message);
            }
            return dtCsv;
        }

        public DataTable ReadOrdersCsvFile()
        {
            DataTable dtCsv = new DataTable();
            string Fulltext;
            try
            {
                using (StreamReader sr = new StreamReader(ORDERS_FILE_PATH))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows

                        dtCsv = AddColumnsToAmibrokerOrders();

                        for (int i = 0; i < rows.Count(); i++)
                        {
                            string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values
                            {
                                DataRow dr = dtCsv.NewRow();
                                for (int k = 0; k < rowValues.Count(); k++)
                                {
                                    dr[k] = rowValues[k].ToString();
                                }
                                dtCsv.Rows.Add(dr); //add other rows
                            }
                        }
                    }
                    logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ReadOrdersCsvFile", "Reader", "Path: " + ORDERS_FILE_PATH);
                }
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ReadOrdersCsvFile", "Exception", ex.Message);
            }
            return dtCsv;
        }

        protected void btnEquityReport_Click(object sender, EventArgs e)
        {
            EquityReport();
            CommodityReport();
        }

        protected void btnShowLogs_Click(object sender, EventArgs e)
        {
            ShowLog();
        }

        private void Page_LoadComplete(object sender, EventArgs e)
        {
            ShowLog();
        }

        private void ShowLog()
        {
            try
            {
                if (ViewState["LogTable"] != null)
                {
                    logdt = (DataTable)ViewState["LogTable"];
                }
                gvLog.DataSource = logdt;
                gvLog.DataBind();
            }
            catch (Exception ex)
            {
                logdt.Rows.Add(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), "ShowLog", "Exception", ex.Message);
            }
        }

        protected void btnSyncOrders_Click(object sender, EventArgs e)
        {
            //Sync Order file with MyOrder files
            DataTable currentOrdersdt = RemoveOrdersBlankRows(ReadOrdersCsvFile());
            WriteMyOrdersToCSV(currentOrdersdt);
        }
    }
}