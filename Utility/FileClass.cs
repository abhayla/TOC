using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;

namespace TOC
{
    public class FileClass
    {
        public static void WriteDataTable(DataTable sourceTable, string filePath)
        {
            try
            {
                //Clear the file before writing new content
                System.IO.File.WriteAllText(filePath, string.Empty);

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    IEnumerable<String> items = null;
                    foreach (DataRow row in sourceTable.Rows)
                    {
                        items = row.ItemArray.Select(o => QuoteValue(o?.ToString() ?? String.Empty));
                        writer.WriteLine(String.Join(",", items));
                    }
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                //MyTelegramBot.SendTelegramMessagewithoutButtons("WriteDataTable exception filePath - " + filePath + " - " + ex.Message, strExceptionChannelsList);
            }
        }

        private static string QuoteValue(string value)
        {
            string str = String.Concat("", value.Replace("\r\r", "\r"), "");
            return str;
        }

        public static DataTable ReadCsvFile(string filePath)
        {
            DataTable dtCsv = new DataTable();
            string[] paths = filePath.Split('\\');

            if (paths[paths.Length - 1].Equals("StrategyBuilder.csv"))
            {
                dtCsv = Strategy.StrategyBuilderClass.AddSBColumns();
            }
            if (paths[paths.Length - 1].Equals("PositionsTracker.csv"))
            {
                dtCsv = Strategy.PositionsClass.AddPTColumns();
            }

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
                    sr.Close();
                }
                dtCsv = RemoveBlankRows(dtCsv);
            }
            catch (Exception ex)
            {
            }
            return dtCsv;
        }

        private static DataTable RemoveBlankRows(DataTable withBlankRowsdt)
        {
            DataTable withoutBlankRowsdt = withBlankRowsdt.Clone();

            foreach (DataRow row in withBlankRowsdt.Rows)
            {
                if (row["Contract Type"].ToString().Trim().Length >= 2 &&
                    row["Transaction Type"].ToString().Trim().Length >= 3 &&
                    row["Strike Price"].ToString().Trim().Length >= 3)
                {
                    withoutBlankRowsdt.Rows.Add(row.ItemArray);
                }
            }
            return withoutBlankRowsdt;
        }

        //public static DataTable AddPTColumns()
        //{
        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("Delete");
        //    dataTable.Columns.Add("OC Type");
        //    dataTable.Columns.Add("Expiry Date");
        //    dataTable.Columns.Add("Contract Type");
        //    dataTable.Columns.Add("Transaction Type");
        //    dataTable.Columns.Add("Strike Price");
        //    dataTable.Columns.Add("Lots");
        //    dataTable.Columns.Add("Entry Price");
        //    dataTable.Columns.Add("Exit Price");
        //    dataTable.Columns.Add("CMP");
        //    dataTable.Columns.Add("P/L");
        //    dataTable.Columns.Add("Chg %");
        //    dataTable.Columns.Add("Realised P/L");
        //    dataTable.Columns.Add("Max Profit");
        //    dataTable.Columns.Add("Recommendation");
        //    dataTable.Columns.Add("Strategy");
        //    dataTable.Columns.Add("Profile");
        //    dataTable.Columns.Add("Position");
        //    dataTable.Columns.Add("Id");
        //    dataTable.Columns.Add("Days To Expiry");
        //    dataTable.Columns.Add("Days Held");
        //    dataTable.Columns.Add("Entry Date");
        //    return dataTable;
        //}

        //public static DataTable AddSBColumns()
        //{
        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("Delete");
        //    dataTable.Columns.Add("Expiry Date");
        //    dataTable.Columns.Add("Contract Type");
        //    dataTable.Columns.Add("Transaction Type");
        //    dataTable.Columns.Add("Strike Price");
        //    dataTable.Columns.Add("CMP");
        //    //dataTable.Columns.Add("Premium");
        //    dataTable.Columns.Add("Lots");
        //    dataTable.Columns.Add("11");
        //    dataTable.Columns.Add("12");
        //    dataTable.Columns.Add("13");
        //    dataTable.Columns.Add("14");
        //    dataTable.Columns.Add("15");
        //    dataTable.Columns.Add("16");
        //    dataTable.Columns.Add("17");
        //    dataTable.Columns.Add("18");
        //    dataTable.Columns.Add("19");
        //    dataTable.Columns.Add("20");
        //    dataTable.Columns.Add("1");
        //    dataTable.Columns.Add("21");
        //    dataTable.Columns.Add("22");
        //    dataTable.Columns.Add("23");
        //    dataTable.Columns.Add("24");
        //    dataTable.Columns.Add("25");
        //    dataTable.Columns.Add("26");
        //    dataTable.Columns.Add("27");
        //    dataTable.Columns.Add("28");
        //    dataTable.Columns.Add("29");
        //    dataTable.Columns.Add("30");
        //    return dataTable;
        //}

    }
}