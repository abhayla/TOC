using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace TOC
{
    public partial class ReadFiles : System.Web.UI.Page
    {
        private static string FileSaveWithPath = "C:\\Abhay\\P\\Autotrader\\data\\order\\orders.csv";
        private static string FileWatcherPath = @"C:\Abhay\P\Autotrader\data\order\";
        DataTable olddt = new DataTable();
        DataTable newOrdersdt = new DataTable();
        DataTable alreadySentCallsdt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                var fileSystemWatcher = new FileSystemWatcher();
                fileSystemWatcher.Path = FileWatcherPath;
                fileSystemWatcher.Changed += FileSystemWatcher_Changed;
                //fileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.FileName;
                //fileSystemWatcher.Filter = "*.csv";
                //Control.CheckForIllegalCrossThreadCalls = false;
                fileSystemWatcher.EnableRaisingEvents = true;
            }
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //btnReadOrders_Click(sender, e);
            //throw new NotImplementedException();
            FindNewOrders();
        }

        protected void btnReadOrders_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable newdt = new DataTable();
                newdt = ReadCsvFile();

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
            DataTable newdt = new DataTable();
            newdt = ReadCsvFile();
            newOrdersdt = newdt.Clone();
            alreadySentCallsdt = newdt.Clone();
            bool isRowExistinFile = false;
            bool isThisMessageSent = false;

            //compare new datatable with old to find new orders in the file
            foreach (DataRow newrow in newdt.Rows)
            {
                foreach (DataRow oldrow in olddt.Rows)
                {
                    if (string.Compare(oldrow["AT_PLACE_ORDER_CMD"].ToString(), newrow["AT_PLACE_ORDER_CMD"].ToString()) == 0 &&
                        string.Compare(oldrow["symbol"].ToString(), newrow["symbol"].ToString()) == 0 &&
                        string.Compare(oldrow["tradeType"].ToString(), newrow["tradeType"].ToString()) == 0)
                    {
                        isRowExistinFile = true;
                    }
                }
                if (isRowExistinFile == false)
                {
                    foreach (DataRow row in newOrdersdt.Rows)
                    {
                        if (string.Compare(row["AT_PLACE_ORDER_CMD"].ToString(), newrow["AT_PLACE_ORDER_CMD"].ToString()) == 0 &&
                        string.Compare(row["symbol"].ToString(), newrow["symbol"].ToString()) == 0 &&
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
            olddt = newdt.Copy();

            if (newOrdersdt.Rows.Count > 0)
            {
                PrepareTelegramMessage(newOrdersdt);
            }
        }

        private void PrepareTelegramMessage(DataTable newOrdersdt)
        {
            string teleMessage = string.Empty;
            bool isMessageNotAlreadySent = true;
            //var diff = newOrdersdt.AsEnumerable().Except(olddt.AsEnumerable(), DataRowComparer.Default);
            foreach (DataRow row in newOrdersdt.Rows)
            {
                //set the flag to false for each new order
                isMessageNotAlreadySent = true;

                if (string.Compare(row["AT_PLACE_ORDER_CMD"].ToString(), "PLACE_ORDER") == 0 &&
                row["symbol"].ToString().Length > 1 &&
                ((string.Compare(row["tradeType"].ToString(), "BUY") == 0) ||
                (string.Compare(row["tradeType"].ToString(), "SELL") == 0)))
                {
                    foreach (DataRow sentRow in alreadySentCallsdt.Rows)
                    {
                        if (string.Compare(row["AT_PLACE_ORDER_CMD"].ToString(), sentRow["AT_PLACE_ORDER_CMD"].ToString()) == 0 &&
                        string.Compare(row["symbol"].ToString(), sentRow["symbol"].ToString()) == 0 &&
                        string.Compare(row["priceStr"].ToString(), sentRow["priceStr"].ToString()) == 0)
                        {
                            isMessageNotAlreadySent = false;
                        }
                    }

                    if (isMessageNotAlreadySent)
                    {
                        //Telegram message to be sent...
                        teleMessage = row["tradeType"].ToString() + " " + row["symbol"].ToString() + " AT " + row["priceStr"].ToString();

                        //Send telegram message
                        Telegram.SendTelegramMessage(teleMessage);

                        //Add sent messages to the list
                        alreadySentCallsdt.Rows.Add(row.ItemArray);
                    }
                }

            }
        }

        public DataTable ReadCsvFile()
        {
            DataTable dtCsv = new DataTable();
            string Fulltext;
            try
            {
                using (StreamReader sr = new StreamReader(FileSaveWithPath))
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

                        for (int i = 0; i < rows.Count() - 1; i++)
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
    }
}