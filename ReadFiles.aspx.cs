using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Threading;
using System.Data;
using System.IO;
using System.Timers;

namespace TOC
{
    public partial class ReadFiles : System.Web.UI.Page
    {
        private static string FOLDER_PATH = "C:\\Myfiles\\";
        private static string ORDERS_FILE_NAME = "orders.csv";
        private static string MY_ORDERS_FILE_NAME = "myorders.csv";
        private static string GROUPS_AND_CHANNELS = "groupschannels.csv";
        private static string ORDERS_FILE_PATH = "C:\\autotrader\\data\\order\\" + ORDERS_FILE_NAME;
        private static string MY_ORDERS_FILE_PATH = FOLDER_PATH + MY_ORDERS_FILE_NAME;
        private static string GROUPS_AND_CHANNELS_FILE_PATH = FOLDER_PATH + GROUPS_AND_CHANNELS;
        private static string FileWatcherPath = @"C:\autotrader\data\order\";
        DataTable oldOrdersdt = new DataTable();
        DataTable newOrdersdt = new DataTable();
        DataTable alreadySentCallsdt = new DataTable();
        private static System.Timers.Timer aTimer;
        private int TimeoutMillis = 5000; //1000 is 1 sec
        private int TimerInterval = 60000; //60 seconds
        System.Threading.Timer m_timer = null;
        List<String> files = new List<string>();
        private string[] strGroupsChannelsList;
        //TimeSpan timeStartClosePosition = new TimeSpan(15, 0, 0);
        //TimeSpan timeClosePosition = new TimeSpan(15, 15, 0);
        //TimeSpan timeEquityReport = new TimeSpan(15, 35, 0);
        //TimeSpan timeCommodityReport = new TimeSpan(23, 59, 0);
        TimeSpan timeStartClosePosition = new TimeSpan(13, 25, 0);
        TimeSpan timeClosePosition = new TimeSpan(13, 26, 0);
        TimeSpan timeEquityReport = new TimeSpan(23, 55, 0);
        TimeSpan timeCommodityReport = new TimeSpan(13, 28, 0);


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var wh = new AutoResetEvent(false);
                var fileSystemWatcher = new FileSystemWatcher();
                fileSystemWatcher.Path = FileWatcherPath;
                fileSystemWatcher.Changed += FileSystemWatcher_Changed;
                //fileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.FileName;
                //fileSystemWatcher.Filter = "*.csv";
                //ThreadPool.SetMaxThreads(1, 1);
                fileSystemWatcher.EnableRaisingEvents = true;
                fileSystemWatcher.Filter = "orders.csv";
                //wh.Set();

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
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            TimeSpan addOneMinute = new TimeSpan(0, 1, 0);

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
            //throw new NotImplementedException();
        }

        private void CommodityReport()
        {
            string teleMessage = string.Empty;
            //teleMessage = "PLEASE CLOSE YOUR INTRADAY PROSITIONS NOW (OR CONVERT TO DELIVERY)\n" +
            // "Some brokers charge addtional fees for closing your open intraday positions.";
            //Telegram.SendTelegramMessage(teleMessage, rowValues);
        }

        private void EquityReport()
        {
            string teleMessage = string.Empty;
            DataTable mySentOrderdt = RemoveBlankRows(ReadMyOrdersCsvFile());

            DataTable dtCsv = new DataTable();
            dtCsv.Columns.Add("symbol");
            dtCsv.Columns.Add("BuyDate");
            dtCsv.Columns.Add("BuyTradeType");
            dtCsv.Columns.Add("BuyPrice");
            dtCsv.Columns.Add("SellDate");
            dtCsv.Columns.Add("SellTradeType");
            dtCsv.Columns.Add("SellPrice");
            dtCsv.Columns.Add("qty");
            dtCsv.Columns.Add("PositionStatus");
            dtCsv.Columns.Add("PositionType");
            dtCsv.Columns.Add("ProfitLossPercent");
            bool isPresent = false;
            //bool isSellPresent = false;
            foreach (DataRow sentOrdersRow in mySentOrderdt.Rows)
            {
                foreach (DataRow row in dtCsv.Rows)
                {
                    if (string.Compare(row["symbol"].ToString().Trim(), sentOrdersRow["symbol"].ToString().Trim()) == 0)
                    {
                        isPresent = true;
                        if (string.Compare(row["BuyTradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) == 0 &&
                            string.Compare(row["BuyTradeType"].ToString().Trim(), enumTransactionType.SELL.ToString()) != 0)
                        {
                            //DataRow newdr = dtCsv.NewRow();
                            row["symbol"] = sentOrdersRow["symbol"].ToString().Trim();
                            row["SellTradeType"] = sentOrdersRow["TradeType"].ToString().Trim();
                            row["SellPrice"] = sentOrdersRow["priceStr"];
                            row["qty"] = sentOrdersRow["qty"];
                            row["PositionStatus"] = enumPositionStatus.Close.ToString();
                            row["PositionType"] = enumPositionType.Long.ToString();
                        }
                        else if (string.Compare(row["BuyTradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) != 0 &&
                                string.Compare(row["BuyTradeType"].ToString().Trim(), enumTransactionType.SELL.ToString()) == 0)
                        {
                            row["symbol"] = sentOrdersRow["symbol"].ToString().Trim();
                            row["BuyTradeType"] = sentOrdersRow["TradeType"].ToString().Trim();
                            row["BuyPrice"] = sentOrdersRow["priceStr"];
                            row["qty"] = sentOrdersRow["qty"];
                            row["PositionStatus"] = enumPositionStatus.Close.ToString();
                            row["PositionType"] = enumPositionType.Short.ToString();
                        }
                        if (string.Compare(row["BuyTradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) == 0 &&
                                string.Compare(row["SellTradeType"].ToString().Trim(), enumTransactionType.SELL.ToString()) == 0)
                        {
                            if (string.Compare(row["PositionType"].ToString().Trim(), enumPositionType.Long.ToString()) == 0)
                            {
                                row["ProfitLossPercent"] = Math.Round(((Convert.ToDouble(row["SellPrice"]) - Convert.ToDouble(row["BuyPrice"])) * 100 / Convert.ToDouble(row["BuyPrice"])), 2);
                            }
                            else if (string.Compare(row["PositionType"].ToString().Trim(), enumPositionType.Short.ToString()) == 0)
                            {
                                row["ProfitLossPercent"] = Math.Round(((Convert.ToDouble(row["BuyPrice"]) - Convert.ToDouble(row["SellPrice"])) * 100 / Convert.ToDouble(row["SellPrice"])), 2);
                            }
                        }
                    }
                    continue;
                }
                if (!isPresent)
                {
                    DataRow newdr = dtCsv.NewRow();
                    if (string.Compare(sentOrdersRow["TradeType"].ToString().Trim(), enumTransactionType.SELL.ToString()) == 0)
                    {
                        newdr["symbol"] = sentOrdersRow["symbol"].ToString().Trim();
                        newdr["SellTradeType"] = sentOrdersRow["TradeType"].ToString().Trim();
                        newdr["SellPrice"] = sentOrdersRow["priceStr"];
                        newdr["qty"] = sentOrdersRow["qty"];
                        newdr["PositionStatus"] = enumPositionStatus.Open.ToString();
                        newdr["PositionType"] = enumPositionType.Short.ToString();
                    }
                    else if (string.Compare(sentOrdersRow["TradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) == 0)
                    {
                        newdr["symbol"] = sentOrdersRow["symbol"].ToString().Trim();
                        newdr["BuyTradeType"] = sentOrdersRow["TradeType"].ToString().Trim();
                        newdr["BuyPrice"] = sentOrdersRow["priceStr"];
                        newdr["qty"] = sentOrdersRow["qty"];
                        newdr["PositionStatus"] = enumPositionStatus.Open.ToString();
                        newdr["PositionType"] = enumPositionType.Long.ToString();
                    }
                    dtCsv.Rows.Add(newdr);
                }
            }
            teleMessage = "EQUITY REPORT :-\n";
            foreach (DataRow row in dtCsv.Rows)
            {
                if (string.Compare(row["PositionStatus"].ToString().Trim(), enumPositionStatus.Open.ToString()) == 0)
                {
                    if (string.Compare(row["BuyTradeType"].ToString().Trim(), enumTransactionType.BUY.ToString()) == 0)
                    {
                        teleMessage += row["symbol"] + ": " + "BuyPrice: " + Math.Round(Convert.ToDouble(row["BuyPrice"]), 2) + " Position: " + row["PositionStatus"] + "\n";
                    }
                    else
                    {
                        teleMessage += row["symbol"] + ": " + "SellPrice: " + Math.Round(Convert.ToDouble(row["SellPrice"]), 2) + " Position: " + row["PositionStatus"] + "\n";
                    }
                }
                else
                {
                    double percentage = Convert.ToDouble(row["ProfitLossPercent"]);
                    if (percentage > 0)
                    {
                        teleMessage += row["symbol"] + ": " + "BuyPrice: " + Math.Round(Convert.ToDouble(row["BuyPrice"]), 2) + " SellPrice: " + Math.Round(Convert.ToDouble(row["SellPrice"]), 2) + " Profit: " + row["ProfitLossPercent"] + "%\n";
                    }
                    else
                    {
                        teleMessage += row["symbol"] + ": " + "BuyPrice: " + Math.Round(Convert.ToDouble(row["BuyPrice"]), 2) + " SellPrice: " + Math.Round(Convert.ToDouble(row["SellPrice"]), 2) + " Loss: " + row["ProfitLossPercent"] + "%\n";
                    }
                }

            }
            if (strGroupsChannelsList == null)
            {
                strGroupsChannelsList = ReadGroupsChannelsCsvFile();
            }
            Telegram.SendTelegramMessage(teleMessage, strGroupsChannelsList);
        }

        private void ClosePosition()
        {
            string teleMessage = string.Empty;
            teleMessage = "PLEASE CLOSE YOUR INTRADAY PROSITIONS NOW (OR CONVERT TO DELIVERY)\n" +
                "Some brokers charge addtional fees for closing your open intraday positions.";
            Telegram.SendTelegramMessage(teleMessage, strGroupsChannelsList);
        }

        private void StartClosePosition()
        {
            string teleMessage = string.Empty;
            teleMessage = "PLEASE START CLOSING YOUR INTRADAY PROSITIONS (OR CONVERT TO DELIVERY) AND AVOID TAKING ANY ADDITIONAL POSITION\n" +
                "Some brokers charge addtional fees for closing your open intraday positions.";
            Telegram.SendTelegramMessage(teleMessage, strGroupsChannelsList);
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
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

        //add your file processing here
        private void OnWatchedFileChange(object state)
        {
            List<String> backup = new List<string>();
            Mutex mutex = new Mutex(false, "FSW");
            mutex.WaitOne();
            backup.AddRange(files);
            files.Clear();
            mutex.ReleaseMutex();

            foreach (string file in backup)
            {
                FindNewOrders();
            }
        }

        protected void btnReadOrders_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable newdt = new DataTable();
                newdt = ReadOrdersCsvFile();

                //PrepareTelegramMessage(newdt, olddt);

                //olddt = newdt.Copy();

                gvFileData.DataSource = newdt;
                gvFileData.DataBind();
            }
            catch (Exception ex)
            {
                //lblerror.Text = ex.Message;
            }
        }

        private void FindNewOrders()
        {
            DataTable currentOrdersdt = ReadOrdersCsvFile();
            newOrdersdt = currentOrdersdt.Clone();
            alreadySentCallsdt = currentOrdersdt.Clone();

            //compare new file data with old file data to find new orders in the file
            foreach (DataRow newrow in currentOrdersdt.Rows)
            {
                bool isRowExistinFile = false;
                foreach (DataRow oldrow in oldOrdersdt.Rows)
                {
                    if (string.Compare(oldrow["AT_PLACE_ORDER_CMD"].ToString().Trim(), newrow["AT_PLACE_ORDER_CMD"].ToString().Trim()) == 0 &&
                        string.Compare(oldrow["id"].ToString().Trim(), newrow["id"].ToString().Trim()) == 0 &&
                        string.Compare(oldrow["symbol"].ToString().Trim(), newrow["symbol"].ToString().Trim()) == 0 &&
                        string.Compare(oldrow["tradeType"].ToString().Trim(), newrow["tradeType"].ToString().Trim()) == 0 &&
                        string.Compare(oldrow["productType"].ToString().Trim(), newrow["productType"].ToString().Trim()) == 0 &&
                        string.Compare(oldrow["orderType"].ToString().Trim(), newrow["orderType"].ToString().Trim()) == 0 &&
                        string.Compare(oldrow["qty"].ToString().Trim(), newrow["qty"].ToString().Trim()) == 0 &&
                        string.Compare(oldrow["priceStr"].ToString().Trim(), newrow["priceStr"].ToString().Trim()) == 0)
                    {
                        isRowExistinFile = true;
                    }
                }
                if (isRowExistinFile == false)
                {
                    bool isThisMessageSent = false;
                    foreach (DataRow row in newOrdersdt.Rows)
                    {
                        if (string.Compare(row["AT_PLACE_ORDER_CMD"].ToString(), newrow["AT_PLACE_ORDER_CMD"].ToString()) == 0 &&
                        string.Compare(row["symbol"].ToString(), newrow["symbol"].ToString()) == 0 &&
                        newrow["symbol"].ToString().Length > 1 &&
                        row["symbol"].ToString().Length > 1 &&
                        string.Compare(row["tradeType"].ToString(), newrow["tradeType"].ToString()) == 0)
                        {
                            isThisMessageSent = true;
                        }
                    }
                    if (isThisMessageSent == false)
                    {
                        newOrdersdt.Rows.Add(newrow.ItemArray);
                    }
                }
            }
            oldOrdersdt = currentOrdersdt.Copy();

            if (newOrdersdt.Rows.Count > 0)
            {
                PrepareTelegramMessage(newOrdersdt);
            }
        }

        private DataTable RemoveBlankRows(DataTable withBlankRowsdt)
        {
            DataTable withoutBlankRowsdt = withBlankRowsdt.Clone();
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
            }
            return withoutBlankRowsdt;
        }

        private void PrepareTelegramMessage(DataTable newOrdersdt)
        {
            DataTable mySentOrderdt = RemoveBlankRows(ReadMyOrdersCsvFile());
            string teleMessage = string.Empty;

            foreach (DataRow row in newOrdersdt.Rows)
            {
                //set the flag to false for each new order
                bool isMessageNotAlreadySent = true;

                if (string.Compare(row["AT_PLACE_ORDER_CMD"].ToString().Trim(), "PLACE_ORDER") == 0 &&
                row["symbol"].ToString().Length > 1 &&
                ((string.Compare(row["tradeType"].ToString().Trim(), "BUY") == 0) ||
                (string.Compare(row["tradeType"].ToString().Trim(), "SELL") == 0)))
                {
                    foreach (DataRow sentRow in alreadySentCallsdt.Rows)
                    {
                        if (string.Compare(row["AT_PLACE_ORDER_CMD"].ToString().Trim(), sentRow["AT_PLACE_ORDER_CMD"].ToString().Trim()) == 0 &&
                        string.Compare(row["symbol"].ToString().Trim(), sentRow["symbol"].ToString().Trim()) == 0 &&
                        string.Compare(row["tradeType"].ToString().Trim(), sentRow["tradeType"].ToString().Trim()) == 0 &&
                        string.Compare(row["priceStr"].ToString().Trim(), sentRow["priceStr"].ToString().Trim()) == 0)
                        {
                            isMessageNotAlreadySent = false;
                        }
                    }

                    if (isMessageNotAlreadySent)
                    {

                        bool isOrderPresentinFile = false;
                        foreach (DataRow sentOrdersRow in mySentOrderdt.Rows)
                        {
                            if (string.Compare(row["AT_PLACE_ORDER_CMD"].ToString().Trim(), sentOrdersRow["AT_PLACE_ORDER_CMD"].ToString().Trim()) == 0 &&
                            string.Compare(row["symbol"].ToString().Trim(), sentOrdersRow["symbol"].ToString().Trim()) == 0 &&
                            string.Compare(row["tradeType"].ToString().Trim(), sentOrdersRow["tradeType"].ToString().Trim()) == 0 &&
                            string.Compare(row["priceStr"].ToString().Trim(), sentOrdersRow["priceStr"].ToString().Trim()) == 0)
                            {
                                isOrderPresentinFile = true;
                            }
                        }

                        if (!isOrderPresentinFile)
                        {
                            if (row["tradeType"] != null && row["symbol"] != null && row["priceStr"] != null)
                            {
                                //Telegram message to be sent...
                                teleMessage = "TREND ALERT:-\n" +
                                                row["tradeType"].ToString() + " " + row["symbol"].ToString() + " AT " + Math.Round(Convert.ToDouble(row["priceStr"]), 2) + "\n" +
                                                "TARGET OPEN\n" +
                                                "STOP LOSS " + Math.Round(Convert.ToDouble(row["trailingStoplossStr"]), 2) + "\n\n" +
                                                "Call only for learning purposes";

                                //Send telegram message
                                Telegram.SendTelegramMessage(teleMessage, strGroupsChannelsList);

                                //Add sent messages to the list
                                alreadySentCallsdt.Rows.Add(row.ItemArray);
                            }
                        }
                    }
                }
            }
            mySentOrderdt.Merge(alreadySentCallsdt);
            mySentOrderdt.AcceptChanges();
            using (StreamWriter writer = new StreamWriter(MY_ORDERS_FILE_PATH))
            {
                WriteDataTable(mySentOrderdt, writer, true);
                writer.Close();
            }

        }

        public static void WriteDataTable(DataTable sourceTable, TextWriter writer, bool includeHeaders)
        {
            //if (includeHeaders)
            //{
            //    IEnumerable<String> headerValues = sourceTable.Columns
            //        .OfType<DataColumn>()
            //        .Select(column => QuoteValue(column.ColumnName));
            //    writer.WriteLine(String.Join(",", headerValues));
            //}

            IEnumerable<String> items = null;

            foreach (DataRow row in sourceTable.Rows)
            {
                items = row.ItemArray.Select(o => QuoteValue(o?.ToString() ?? String.Empty));
                //items = row.ItemArray.se
                writer.WriteLine(String.Join(",", items));
                //writer.WriteLine(items);
            }

            writer.Flush();
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
                }
            }
            catch (Exception ex)
            {

            }
            return rowValues;
        }

        public DataTable ReadMyOrdersCsvFile()
        {
            DataTable dtCsv = alreadySentCallsdt.Clone();
            string Fulltext;
            try
            {
                using (StreamReader sr = new StreamReader(MY_ORDERS_FILE_PATH))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows 

                        if (dtCsv.Columns.Count == 0)
                        {
                            dtCsv = AddColumns();
                        }

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
                    sr.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return dtCsv;
        }

        private DataTable AddColumns()
        {
            DataTable dtCsv = new DataTable();
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
                }
            }
            catch (Exception ex)
            {

            }
            return dtCsv;
        }

        protected void btnEquityReport_Click(object sender, EventArgs e)
        {
            EquityReport();
        }
    }
}