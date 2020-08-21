using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using TOC.Strategy;
using MySql.Data.MySqlClient;

namespace TOC
{
    public class FileClass
    {
        public static void WriteDataTable(DataTable sourceTable, string filePath)
        {
            try
            {
                //DatabaseClass.UpsertPositions(sourceTable, filePath);
                //UpsertStrategyBuilder(sourceTable, filePath);
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

            if (paths[paths.Length - 1].Equals(Constants.SB_FILE_NAME))
            {
                dtCsv = Strategy.StrategyBuilderClass.AddSBColumns();
            }
            if (paths[paths.Length - 1].Equals(Constants.PT_FILE_NAME))
            {
                dtCsv = Strategy.PositionsClass.AddPTColumns();
            }
            if (paths[paths.Length - 1].Equals(Constants.BO_FILE_NAME))
            {
                dtCsv = Strategy.BasketOrderClass.AddBOColumns();
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
    }
}