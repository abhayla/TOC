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

        protected void Page_Load(object sender, EventArgs e)
        {
            var fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Path = FileWatcherPath;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            btnReadOrders_Click(sender, e);
            //throw new NotImplementedException();
        }

        protected void btnReadOrders_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ReadCsvFile();
                gvFileData.DataSource = dt;
                gvFileData.DataBind();
            }
            catch (Exception ex)
            {
                //lblerror.Text = ex.Message;
            }
        }
        public DataTable ReadCsvFile()
        {

            DataTable dtCsv = new DataTable();
            string Fulltext;
            //if (FileUpload1.HasFile)
            //{
            //string FileSaveWithPath = Server.MapPath("~\\~\\~\\~\\~\\Abhay\\P\\Autotrader\\data\\order\\orders.csv");
            //string FileSaveWithPath = "C:\\Abhay\\P\\Autotrader\\data\\order\\orders.csv";
            //FileUpload1.SaveAs(FileSaveWithPath);

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
            //}
            return dtCsv;
        }
    }
}