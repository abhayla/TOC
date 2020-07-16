using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TOC.Strategy;

namespace TOC
{
    public partial class StrategyBuilder : System.Web.UI.Page
    {
        private static string NIFTY = "NIFTY";
        private static int NIFTY_LOT_SIZE = 75;

        private static string BANKNIFTY = "BANKNIFTY";
        private static int BANKNIFTY_LOT_SIZE = 20;

        private static int MAX_ROWS_ALLOWED = 15;

        private static string SB_FILE_NAME = "StrategyBuilder.csv";
        private static string MYFILES_FOLDER_PATH = "C:\\Myfiles\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dataTable = FileClass.ReadCsvFile(MYFILES_FOLDER_PATH + SB_FILE_NAME);

                if (dataTable.Rows.Count <= 0)
                    AddBlankRows(dataTable, 2);

                MySession.Current.StrategyBuilderDt = dataTable;
                GridviewDataBind(dataTable);
                lblLastFetchedTime.Text = MySession.Current.RecordsObject.timestamp;
                lblLastPrice.Text = MySession.Current.RecordsObject.underlyingValue.ToString();
                OCHelper.FillAllExpiryDates(NIFTY, ddlFilterExpiryDates, true);
            }
        }

        private void AddBlankRows(DataTable dataTable, int iRowCount)
        {
            int rowstoadd = 0;
            if (dataTable.Rows.Count + iRowCount < MAX_ROWS_ALLOWED)
                rowstoadd = iRowCount;
            if (dataTable.Rows.Count + iRowCount >= MAX_ROWS_ALLOWED)
                rowstoadd = MAX_ROWS_ALLOWED - dataTable.Rows.Count;

            for (int iCount = 0; iCount < rowstoadd; iCount++)
            {
                StrategyBuilderClass.AddBlankRows(dataTable);
            }
        }

        protected void rblOCType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillExpiryDates(ddlExpiryDates);
        }

        private void GridviewDataBind(DataTable dataTable)
        {
            int cutOffHigherSP = 0;
            int cutOffLowerSP = 0;
            List<int> dataTableColumnsList = new List<int>();

            DataTable dataTableCalc = new DataTable();

            foreach (DataRow row in dataTable.Rows)
            {
                //Get latest premium value from the OC
                if (row[row.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString().Equals(enumContractType.CE.ToString()))
                {
                    Records.Datum.CE1 cE1 = OCHelper.GetCE(rblOCType.SelectedValue, row[row.Table.Columns.IndexOf(enumSBColumns.ExpiryDate.ToString())].ToString(),
                        Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())]));
                    if (cE1 != null)
                        row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())] = cE1.lastPrice;
                    else
                        row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())] = 0;
                }

                if (row[row.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString().Equals(enumContractType.PE.ToString()))
                {
                    Records.Datum.PE1 pE1 = OCHelper.GetPE(rblOCType.SelectedValue, row[row.Table.Columns.IndexOf(enumSBColumns.ExpiryDate.ToString())].ToString(),
                        Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())]));
                    if (pE1 != null)
                        row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())] = pE1.lastPrice;
                    else
                        row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())] = 0;
                }

                if (row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())] != null &&
                    row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())].ToString().Trim().Length > 0 &&
                    row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())] != null &&
                    row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())].ToString().Trim().Length > 0 &&
                    Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())]) > 0)
                {
                    dataTableColumnsList.Add(Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())]));

                    if (row[row.Table.Columns.IndexOf(enumSBColumns.TransactionType.ToString())].ToString().Equals("BUY"))
                    {
                        if (row[row.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString().Equals("PE"))
                        {
                            cutOffLowerSP = Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())]) +
                                           Math.Abs(Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())]));
                        }
                        if (row[row.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString().Equals("CE"))
                        {
                            cutOffLowerSP = Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())]) -
                                         Math.Abs(Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())]));
                        }

                        if (!dataTableColumnsList.Contains(cutOffLowerSP))
                            dataTableColumnsList.Add(cutOffLowerSP);
                    }

                    if (row[row.Table.Columns.IndexOf(enumSBColumns.TransactionType.ToString())].ToString().Equals("SELL"))
                    {
                        if (row[row.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString().Equals("PE"))
                        {
                            cutOffHigherSP = Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())]) -
                                        Math.Abs(Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())]));
                        }
                        if (row[row.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString().Equals("CE"))
                        {
                            cutOffHigherSP = Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())]) +
                                        Math.Abs(Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())]));
                        }

                        if (!dataTableColumnsList.Contains(cutOffHigherSP))
                            dataTableColumnsList.Add(cutOffHigherSP);
                    }
                }
            }

            //Central SP will now be closest to market price
            int centralSP = OCHelper.RoundTo100(MySession.Current.RecordsObject.underlyingValue);
            dataTableColumnsList.Add(centralSP);

            dataTableColumnsList.Sort();

            //dataTable.Columns.Add(enumSBColumns.CMP.ToString());
            dataTableCalc.Columns.Add(enumSBColumns.CMP.ToString());

            foreach (int dataListItem in dataTableColumnsList)
            {
                //if (!dataTable.Columns.Contains(dataListItem.ToString()))
                //{
                //    dataTable.Columns.Add(dataListItem.ToString());
                //}

                if (!dataTableCalc.Columns.Contains(dataListItem.ToString()))
                {
                    dataTableCalc.Columns.Add(dataListItem.ToString());
                }
            }

            int lotsSize = 0;
            if (rblOCType.SelectedValue.Equals(NIFTY))
                lotsSize = NIFTY_LOT_SIZE;
            if (rblOCType.SelectedValue.Equals(BANKNIFTY))
                lotsSize = BANKNIFTY_LOT_SIZE;

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow dataRowCalc = dataTableCalc.NewRow();
                dataRowCalc[dataRowCalc.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())] = row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())];
                foreach (int dataListItem in dataTableColumnsList)
                {
                    dataRowCalc[dataTableCalc.Columns.IndexOf(dataListItem.ToString())] =
                            Convert.ToString(Convert.ToInt32(row[dataTable.Columns.IndexOf(enumSBColumns.Lots.ToString())]) * lotsSize *
                            FO.CalcExpVal(row[dataTable.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString(),
                            row[dataTable.Columns.IndexOf(enumSBColumns.TransactionType.ToString())].ToString(),
                            Convert.ToInt32(row[dataTable.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())]),
                            Convert.ToDouble(row[dataTable.Columns.IndexOf(enumSBColumns.CMP.ToString())]),
                            Convert.ToInt32(dataTableCalc.Columns[dataListItem.ToString()].ColumnName)));

                    //dataRowCalc[dataTableCalc.Columns.IndexOf(dataListItem.ToString())] = row[dataTable.Columns.IndexOf(dataListItem.ToString())];
                }
                dataTableCalc.Rows.Add(dataRowCalc);
            }

            //MySession.Current.StrategyBuilderDt = dataTable;
            //Create data only Datatable

            gvStrategy.DataSource = dataTableCalc;
            gvStrategy.DataBind();
        }

        protected void gvStrategy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dataTable = MySession.Current.StrategyBuilderDt;

            //Logic for header rows
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    for (int icount = 6; icount < e.Row.Cells.Count; icount++)
            //    {
            //        e.Row.Cells[icount].Text = dataTable.Columns[icount].ColumnName;
            //    }
            //}

            //Logic for nomal rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                DropDownList ddlExpiryDates = e.Row.FindControl("ddlExpiryDates") as DropDownList;
                DropDownList ddlContractType = e.Row.FindControl("ddlContractType") as DropDownList;
                DropDownList ddlTransactionType = e.Row.FindControl("ddlTransactionType") as DropDownList;
                DropDownList ddlStrikePrice = e.Row.FindControl("ddlStrikePrice") as DropDownList;
                DropDownList ddlLots = e.Row.FindControl("ddlLots") as DropDownList;

                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumSBColumns.CMP.ToString())].Text = dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumSBColumns.CMP.ToString())].ToString();
                chkSelect.Checked = Convert.ToBoolean(dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumSBColumns.Select.ToString())]);

                OCHelper.FillAllExpiryDates(rblOCType.SelectedValue, ddlExpiryDates, false);
                OCHelper.FillAllStrikePrice(rblOCType.SelectedValue, ddlStrikePrice, false);

                Utility.SelectDataInCombo(ddlExpiryDates, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumSBColumns.ExpiryDate.ToString())].ToString());
                Utility.SelectDataInCombo(ddlContractType, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString());
                Utility.SelectDataInCombo(ddlTransactionType, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumSBColumns.TransactionType.ToString())].ToString());
                Utility.SelectDataInCombo(ddlStrikePrice, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())].ToString());
                Utility.SelectDataInCombo(ddlLots, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumSBColumns.Lots.ToString())].ToString());

                for (int icount = 6; icount < e.Row.Cells.Count; icount++)
                {
                    //e.Row.Cells[icount].Text = dataTable.Rows[e.Row.RowIndex][icount].ToString();
                    e.Row.Cells[icount].HorizontalAlign = HorizontalAlign.Right;
                }

                //Add attributes
                int rowindex = e.Row.RowIndex + 1;
                ddlContractType.Attributes.Add("onchange", "CalcExpValForAllCellsInRow('" + gvStrategy.ClientID + "', '" +
                    rowindex + "','" + rblOCType.ClientID + "');");
                ddlTransactionType.Attributes.Add("onchange", "CalcExpValForAllCellsInRow('" + gvStrategy.ClientID + "', '" +
                    rowindex + "','" + rblOCType.ClientID + "');");
                ddlStrikePrice.Attributes.Add("onchange", "SetGridviewHeaderValues('"
                    + gvStrategy.ClientID + "'), CalcExpValForAllCellsInRow('" + gvStrategy.ClientID + "', '" + rowindex + "','"
                    + rblOCType.ClientID + "');");
            }

            GridView gridView = sender as GridView;

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int sum = 0;
                for (int iCellCount = 6; iCellCount < e.Row.Cells.Count; iCellCount++)
                {
                    sum = 0;
                    for (int irowcount = 0; irowcount < gridView.Rows.Count; irowcount++)
                    {
                        sum += Convert.ToInt32(gridView.Rows[irowcount].Cells[iCellCount].Text);
                    }
                    e.Row.Cells[iCellCount].Text = sum.ToString();
                    e.Row.Cells[iCellCount].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }

        protected void btnAddRows_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            AddBlankRows(dataTable, 2);
            MySession.Current.StrategyBuilderDt = dataTable;
            GridviewDataBind(dataTable);
        }

        private DataTable SaveGridviewData()
        {
            DataTable dataTable = StrategyBuilderClass.AddSBColumns();

            foreach (GridViewRow gvRow in gvStrategy.Rows)
            {
                CheckBox chkSelect = gvRow.FindControl("chkSelect") as CheckBox;
                DropDownList ddlExpiryDates = gvRow.FindControl("ddlExpiryDates") as DropDownList;
                DropDownList ddlContractType = gvRow.FindControl("ddlContractType") as DropDownList;
                DropDownList ddlTransactionType = gvRow.FindControl("ddlTransactionType") as DropDownList;
                DropDownList ddlStrikePrice = gvRow.FindControl("ddlStrikePrice") as DropDownList;
                DropDownList ddlLots = gvRow.FindControl("ddlLots") as DropDownList;

                dataTable.Rows.Add(chkSelect.Checked, ddlExpiryDates.SelectedValue, ddlContractType.SelectedValue, ddlTransactionType.SelectedValue,
                    ddlStrikePrice.SelectedValue, ddlLots.SelectedValue);
            }
            return dataTable;
        }

        protected void btnUpdateCMP_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            MySession.Current.StrategyBuilderDt = dataTable;
            GridviewDataBind(dataTable);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            FileClass.WriteDataTable(dataTable, MYFILES_FOLDER_PATH + SB_FILE_NAME);
            MySession.Current.StrategyBuilderDt = dataTable;
            GridviewDataBind(dataTable);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            DataTable newDataTable = dataTable.Clone();
            foreach (DataRow row in dataTable.Rows)
            {
                if (!Convert.ToBoolean(row[row.Table.Columns.IndexOf(enumSBColumns.Select.ToString())]))
                {
                    newDataTable.Rows.Add(row.ItemArray);
                }
            }
            MySession.Current.StrategyBuilderDt = newDataTable;
            GridviewDataBind(newDataTable);
        }

        protected void btnAddToPositions_Click(object sender, EventArgs e)
        {

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            DataTable filteredDataTable = FilterRecords();
            GridviewDataBind(filteredDataTable);
        }

        private DataTable FilterRecords()
        {
            DataTable dataTable = MySession.Current.StrategyBuilderDt;
            string selectQuery = GetFilterString();
            DataRow[] drs = dataTable.Select(selectQuery);
            DataTable filteredDataTable = dataTable.Clone();
            foreach (DataRow dr in drs)
            {
                filteredDataTable.ImportRow(dr);
            }

            return filteredDataTable;
        }

        private string GetFilterString()
        {
            string strFilter = string.Empty;
            List<string> filters = new List<string>();

            if (!ddlFilterExpiryDates.SelectedValue.Equals("All"))
                filters.Add(" (" + enumSBColumns.ExpiryDate.ToString() + " = #" + ddlFilterExpiryDates.SelectedValue + "#) ");

            for (int index = 0; index < filters.Count; index++)
            {
                if (index < filters.Count - 1)
                {
                    strFilter += filters[index] + " AND ";
                }
                else
                {
                    strFilter += filters[index];
                }
            }

            return strFilter;
        }

        //public void FillAllExpiryDates(string ocType, System.Web.UI.WebControls.DropDownList ddlExpDt, bool IsAddAll)
        //{
        //    if (IsAddAll)
        //        ddlExpDt.Items.Add("All");

        //    List<string> expiryDates = OCHelper.GetOCExpList(ocType);
        //    foreach (string item in expiryDates)
        //    {
        //        ddlExpDt.Items.Add(item);
        //    }

        //    for (int index = 0; index < MySession.Current.PositionsTrackerDt.Rows.Count; index++)
        //    {
        //        string item = MySession.Current.PositionsTrackerDt.Rows[index][enumPTColumns.ExpiryDate.ToString()].ToString();
        //        if (!ddlExpDt.Items.Contains(new ListItem(item)))
        //            ddlExpDt.Items.Add(item);
        //    }
        //}
    }
}