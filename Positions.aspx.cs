using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using TOC.Strategy;

namespace TOC
{
    public partial class PositionsTracker : System.Web.UI.Page
    {

        //Recommendation to close an open position based on current returns and remaining time in expiry
        //private static string NIFTY = "NIFTY";
        //private static int NIFTY_COL_DIFF = 100;
        //private static int NIFTY_LOT_SIZE = 75;

        //private static string BANKNIFTY = "BANKNIFTY";
        //private static int BANKNIFTY_COL_DIFF = 200;
        //private static int BANKNIFTY_LOT_SIZE_BEF_JUL = 20;
        //private static int BANKNIFTY_LOT_SIZE_AFT_JUL = 25;

        //Gridview columns
        //private static int DELETE_COL_INDEX = 0;
        //private static int OC_TYPE_COL_INDEX = 1;
        //private static int EXP_DATE_COL_INDEX = 2;
        //private static int CONTRACT_TYP_COL_INDEX = 3;
        //private static int TRANSTYP_COL_INDEX = 4;
        //private static int SP_COL_INDEX = 5;
        //private static int LOTS_COL_INDEX = 6;
        //private static int ENTRY_PRICE_COL_INDEX = 7;
        //private static int EXIT_PRICE_COL_INDEX = 8;
        //private static int CMP_COL_INDEX = 9;
        //private static int PL_UR_COL_INDEX = 10;
        //private static int CHG_COL_INDEX = 11;
        //private static int PL_R_COL_INDEX = 12;
        //private static int MAX_PROFIT_COL_INDEX = 13;
        //private static int RECOM_COL_INDEX = 14;
        //private static int STRATEGY_COL_INDEX = 15;
        //private static int PROFILE_COL_INDEX = 16;
        //private static int POSITION_COL_INDEX = 17;
        //private static int ID_COL_INDEX = 18;

        //private static int MAX_ROWS_ALLOWED = 100;

        //private static string PT_FILE_NAME = "PositionsTracker.csv";
        //private static string MYFILES_FOLDER_PATH = "C:\\Myfiles\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dataTable = FileClass.ReadCsvFile(Constants.MYFILES_FOLDER_PATH + Constants.PT_FILE_NAME);

                if (dataTable.Rows.Count <= 0)
                    AddBlankRows(dataTable, Constants.POS_ADD_ROW_COUNT);

                MySession.Current.PositionsTrackerDt = dataTable;
                GridviewDataBind(dataTable);

                FillAllExpiryDates(enumOCType.NIFTY.ToString(), ddlFilterExpiryDates, true);
            }
        }

        protected void gvPosTracker_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumPTColumns.Id.ToString())].Style.Add("display", "none");
            e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumPTColumns.Position.ToString())].Style.Add("display", "none");

            //DataTable dataTable = MySession.Current.PositionsTrackerDt;
            DataTable dataTable = gvPosTracker.DataSource as DataTable;

            //Logic for header rows
            if (e.Row.RowType == DataControlRowType.Header)
            {

            }

            //Logic for nomal rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;

                //if (dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.Select.ToString())] != true)
                chkSelect.Checked = Convert.ToBoolean(dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.Select.ToString())]);

                DropDownList ddlOCType = e.Row.FindControl("ddlOCType") as DropDownList;
                DropDownList ddlExpiryDates = e.Row.FindControl("ddlExpiryDates") as DropDownList;
                DropDownList ddlContractType = e.Row.FindControl("ddlContractType") as DropDownList;
                DropDownList ddlTransactionType = e.Row.FindControl("ddlTransactionType") as DropDownList;
                DropDownList ddlStrikePrice = e.Row.FindControl("ddlStrikePrice") as DropDownList;
                DropDownList ddlLots = e.Row.FindControl("ddlLots") as DropDownList;
                DropDownList ddlStrategy = e.Row.FindControl("ddlStrategy") as DropDownList;
                DropDownList ddlProfile = e.Row.FindControl("ddlProfile") as DropDownList;

                TextBox txtEntryPrice = e.Row.FindControl("txtEntryPrice") as TextBox;
                TextBox txtExitPrice = e.Row.FindControl("txtExitPrice") as TextBox;
                txtEntryPrice.Style["text-align"] = "right";
                txtExitPrice.Style["text-align"] = "right";

                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumPTColumns.CMP.ToString())].Text = dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.CMP.ToString())].ToString();
                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumPTColumns.UnRealisedPL.ToString())].Text = dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.UnRealisedPL.ToString())].ToString();
                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumPTColumns.RealisedPL.ToString())].Text = dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.RealisedPL.ToString())].ToString();
                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumPTColumns.ChgPer.ToString())].Text = dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.ChgPer.ToString())].ToString() + "%";

                Utility.SelectDataInCombo(ddlOCType, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.OCType.ToString())].ToString());
                FillAllExpiryDates(dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.OCType.ToString())].ToString(), ddlExpiryDates, false);
                FillStrikePrice(ddlStrikePrice);
                Utility.SelectDataInCombo(ddlExpiryDates, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.ExpiryDate.ToString())].ToString());
                Utility.SelectDataInCombo(ddlContractType, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.ContractType.ToString())].ToString());
                Utility.SelectDataInCombo(ddlTransactionType, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.TransactionType.ToString())].ToString());
                Utility.SelectDataInCombo(ddlStrikePrice, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.StrikePrice.ToString())].ToString());

                txtEntryPrice.Text = dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.EntryPrice.ToString())].ToString();
                txtExitPrice.Text = dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.ExitPrice.ToString())].ToString();
                Utility.SelectDataInCombo(ddlLots, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.Lots.ToString())].ToString());
                Utility.SelectDataInCombo(ddlStrategy, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.Strategy.ToString())].ToString());
                Utility.SelectDataInCombo(ddlProfile, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumPTColumns.Profile.ToString())].ToString());
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                double realisedProfit = 0;
                double unRealisedProfit = 0;
                double maxProfit = 0;
                for (int irowcount = 0; irowcount < dataTable.Rows.Count; irowcount++)
                {
                    if (dataTable.Rows[irowcount][dataTable.Columns.IndexOf(enumPTColumns.Position.ToString())].ToString().Equals("Open"))
                    {
                        unRealisedProfit += Convert.ToDouble(dataTable.Rows[irowcount][dataTable.Columns.IndexOf(enumPTColumns.UnRealisedPL.ToString())]);
                        maxProfit += Convert.ToDouble(dataTable.Rows[irowcount][dataTable.Columns.IndexOf(enumPTColumns.MaxProfit.ToString())]);
                    }

                    if (dataTable.Rows[irowcount][dataTable.Columns.IndexOf(enumPTColumns.Position.ToString())].ToString().Equals("Close"))
                        realisedProfit += Convert.ToDouble(dataTable.Rows[irowcount][dataTable.Columns.IndexOf(enumPTColumns.RealisedPL.ToString())]);
                }
                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumPTColumns.UnRealisedPL.ToString())].Text = unRealisedProfit.ToString();
                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumPTColumns.RealisedPL.ToString())].Text = realisedProfit.ToString();
                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumPTColumns.MaxProfit.ToString())].Text = maxProfit.ToString();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            DataTable newDataTable = dataTable.Clone();
            foreach (DataRow row in dataTable.Rows)
            {
                if (!Convert.ToBoolean(row[dataTable.Columns.IndexOf(enumPTColumns.Select.ToString())]))
                {
                    newDataTable.Rows.Add(row.ItemArray);
                }
            }
            MySession.Current.PositionsTrackerDt = newDataTable;
            DataTable filteredDataTable = FilterRecords();
            GridviewDataBind(filteredDataTable);
        }

        protected void btnAddRows_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            AddBlankRows(dataTable, Constants.POS_ADD_ROW_COUNT);
            MySession.Current.PositionsTrackerDt = dataTable;
            DataTable filteredDataTable = FilterRecords();
            GridviewDataBind(filteredDataTable);
        }

        protected void btnUpdateCMP_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            MySession.Current.PositionsTrackerDt = dataTable;
            DataTable filteredDataTable = FilterRecords();
            GridviewDataBind(filteredDataTable);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            FileClass.WriteDataTable(dataTable, Constants.MYFILES_FOLDER_PATH + Constants.PT_FILE_NAME);
            MySession.Current.PositionsTrackerDt = dataTable;
            DataTable filteredDataTable = FilterRecords();
            GridviewDataBind(filteredDataTable);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            DataTable filteredDataTable = FilterRecords();
            GridviewDataBind(filteredDataTable);
        }

        private DataTable FilterRecords()
        {
            DataTable dataTable = MySession.Current.PositionsTrackerDt;
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
                filters.Add(" (" + enumPTColumns.ExpiryDate.ToString() + " = #" + ddlFilterExpiryDates.SelectedValue + "#) ");

            if (!ddlContractType.SelectedValue.Equals("All"))
                filters.Add(" (" + enumPTColumns.ContractType.ToString() + " = '" + ddlContractType.SelectedValue + "') ");

            if (!ddlFilterProfiles.SelectedValue.Equals("All"))
                filters.Add(" (" + enumPTColumns.Profile.ToString() + " = '" + ddlFilterProfiles.SelectedValue + "') ");

            if (!ddlFilterStrategy.SelectedValue.Equals("All"))
                filters.Add(" (" + enumPTColumns.Strategy.ToString() + " = '" + ddlFilterStrategy.SelectedValue + "') ");

            if (!ddlFilterPostionStatus.SelectedValue.Equals("All"))
                filters.Add(" (" + enumPTColumns.Position.ToString() + " = '" + ddlFilterPostionStatus.SelectedValue + "') ");

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



        private void GridviewDataBind(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                //Get latest premium value from the OC
                if (row[row.Table.Columns.IndexOf(enumPTColumns.ContractType.ToString())].ToString().Equals(enumContractType.CE.ToString()))
                {
                    Records.Datum.CE1 cE1 = OCHelper.GetCE(row[row.Table.Columns.IndexOf(enumPTColumns.OCType.ToString())].ToString(), row[row.Table.Columns.IndexOf(enumPTColumns.ExpiryDate.ToString())].ToString(),
                        Convert.ToInt32(row[row.Table.Columns.IndexOf(enumPTColumns.StrikePrice.ToString())]));
                    if (cE1 != null)
                        row[row.Table.Columns.IndexOf(enumPTColumns.CMP.ToString())] = cE1.lastPrice.ToString();
                    else
                        row[row.Table.Columns.IndexOf(enumPTColumns.CMP.ToString())] = "0";
                }

                if (row[row.Table.Columns.IndexOf(enumPTColumns.ContractType.ToString())].ToString().Equals(enumContractType.PE.ToString()))
                {
                    Records.Datum.PE1 pE1 = OCHelper.GetPE(row[row.Table.Columns.IndexOf(enumPTColumns.OCType.ToString())].ToString(), row[row.Table.Columns.IndexOf(enumPTColumns.ExpiryDate.ToString())].ToString(),
                        Convert.ToInt32(row[row.Table.Columns.IndexOf(enumPTColumns.StrikePrice.ToString())]));
                    if (pE1 != null)
                        row[row.Table.Columns.IndexOf(enumPTColumns.CMP.ToString())] = pE1.lastPrice.ToString();
                    else
                        row[row.Table.Columns.IndexOf(enumPTColumns.CMP.ToString())] = "0";
                }

                if (!row[row.Table.Columns.IndexOf(enumPTColumns.ExpiryDate.ToString())].ToString().Equals(string.Empty))
                    row[row.Table.Columns.IndexOf(enumPTColumns.DaysToExpiry.ToString())] = DateTime.Parse(row[row.Table.Columns.IndexOf(enumPTColumns.ExpiryDate.ToString())].ToString()).Subtract(DateTime.Now).Days;

                int lotSize = 0;
                if (row[row.Table.Columns.IndexOf(enumPTColumns.OCType.ToString())].ToString().Equals(enumOCType.NIFTY.ToString()))
                    lotSize = Constants.NIFTY_LOT_SIZE;
                if (row[row.Table.Columns.IndexOf(enumPTColumns.OCType.ToString())].ToString().Equals(enumOCType.BANKNIFTY.ToString()))
                    lotSize = Constants.BANKNIFTY_LOT_SIZE;

                //If current expiry date is not in the Option Chain fetched list then mark the current row position as close
                //if (!MySession.Current.PositionsTrackerDt.Columns["Expiry Date"].ToString().Contains(row["Expiry Date"].ToString()))

                if (!IsContainExpiryDate(row[row.Table.Columns.IndexOf(enumPTColumns.ExpiryDate.ToString())].ToString()))
                {
                    row[row.Table.Columns.IndexOf(enumPTColumns.Position.ToString())] = "Close";
                }
                else if (Convert.ToDouble(row[row.Table.Columns.IndexOf(enumPTColumns.ExitPrice.ToString())]) > 0)
                {
                    row[row.Table.Columns.IndexOf(enumPTColumns.Position.ToString())] = "Close";
                }
                else
                {
                    row[row.Table.Columns.IndexOf(enumPTColumns.Position.ToString())] = "Open";
                }

                double exitPrice = 0.0;
                double entryPrice = 0.0;

                if (!row[row.Table.Columns.IndexOf(enumPTColumns.EntryPrice.ToString())].ToString().Equals(string.Empty))
                    entryPrice = Convert.ToDouble(row[row.Table.Columns.IndexOf(enumPTColumns.EntryPrice.ToString())]);

                if (row[row.Table.Columns.IndexOf(enumPTColumns.Position.ToString())].ToString().Equals("Close"))
                {
                    if (!row[row.Table.Columns.IndexOf(enumPTColumns.ExitPrice.ToString())].ToString().Equals(string.Empty))
                        exitPrice = Convert.ToDouble(row[row.Table.Columns.IndexOf(enumPTColumns.ExitPrice.ToString())]);

                    if (row[row.Table.Columns.IndexOf(enumPTColumns.TransactionType.ToString())].ToString().Equals("BUY"))
                        row[row.Table.Columns.IndexOf(enumPTColumns.RealisedPL.ToString())] = Math.Round((exitPrice - entryPrice) * Convert.ToInt32(row[row.Table.Columns.IndexOf(enumPTColumns.Lots.ToString())]), 2) * lotSize;

                    if (row[row.Table.Columns.IndexOf(enumPTColumns.TransactionType.ToString())].ToString().Equals("SELL"))
                        row[row.Table.Columns.IndexOf(enumPTColumns.RealisedPL.ToString())] = Math.Round((entryPrice - exitPrice) * Convert.ToInt32(row[row.Table.Columns.IndexOf(enumPTColumns.Lots.ToString())]), 2) * lotSize;
                }

                if (row[row.Table.Columns.IndexOf(enumPTColumns.Position.ToString())].ToString().Equals("Open"))
                {
                    if (!row[row.Table.Columns.IndexOf(enumPTColumns.CMP.ToString())].ToString().Equals(string.Empty))
                        exitPrice = Convert.ToDouble(row[row.Table.Columns.IndexOf(enumPTColumns.CMP.ToString())]);

                    if (row[row.Table.Columns.IndexOf(enumPTColumns.TransactionType.ToString())].ToString().Equals("BUY"))
                        row[row.Table.Columns.IndexOf(enumPTColumns.UnRealisedPL.ToString())] = Math.Round((exitPrice - entryPrice) * Convert.ToInt32(row[row.Table.Columns.IndexOf(enumPTColumns.Lots.ToString())]), 2) * lotSize;

                    if (row[row.Table.Columns.IndexOf(enumPTColumns.TransactionType.ToString())].ToString().Equals("SELL"))
                        row[row.Table.Columns.IndexOf(enumPTColumns.UnRealisedPL.ToString())] = Math.Round((entryPrice - exitPrice) * Convert.ToInt32(row[row.Table.Columns.IndexOf(enumPTColumns.Lots.ToString())]), 2) * lotSize;

                    row[row.Table.Columns.IndexOf(enumPTColumns.MaxProfit.ToString())] = Math.Round(entryPrice * Convert.ToInt32(row[row.Table.Columns.IndexOf(enumPTColumns.Lots.ToString())]), 2) * lotSize;
                }

                if (row[row.Table.Columns.IndexOf(enumPTColumns.TransactionType.ToString())].ToString().Equals("BUY"))
                    row[row.Table.Columns.IndexOf(enumPTColumns.ChgPer.ToString())] = Math.Round((exitPrice - entryPrice) * 100 / entryPrice, 2);

                if (row[row.Table.Columns.IndexOf(enumPTColumns.TransactionType.ToString())].ToString().Equals("SELL"))
                    row[row.Table.Columns.IndexOf(enumPTColumns.ChgPer.ToString())] = Math.Round((entryPrice - exitPrice) * 100 / entryPrice, 2);

                if (row[row.Table.Columns.IndexOf(enumPTColumns.Id.ToString())].ToString().Trim().Equals(string.Empty))
                    row[row.Table.Columns.IndexOf(enumPTColumns.Id.ToString())] = Guid.NewGuid();
            }

            //MySession.Current.PositionsTrackerDt = dataTable;
            //gvPosTracker.Columns
            gvPosTracker.DataSource = dataTable;
            gvPosTracker.DataBind();
        }

        private void AddBlankRows(DataTable dataTable, int iRowCount)
        {
            int rowstoadd = 0;
            if (dataTable.Rows.Count + iRowCount < Constants.POS_MAX_ROWS)
                rowstoadd = iRowCount;
            if (dataTable.Rows.Count + iRowCount >= Constants.POS_MAX_ROWS)
                rowstoadd = Constants.POS_MAX_ROWS - dataTable.Rows.Count;

            for (int iCount = 0; iCount < rowstoadd; iCount++)
            {
                //dataTable.Rows.Add(false, NIFTY, OCHelper.DefaultExpDate(NIFTY), "CE", "SELL", OCHelper.DefaultSP(NIFTY),
                //    "1", "0", "0", "0", "0", "0", "0", "0", "", "Butterfly", "Abhay", "Open", Guid.NewGuid(), "0", "0", "09/07/2020");
                //dataTable.Rows.Add(dataTable.NewRow());
                PositionsClass.AddBlankRows(dataTable);
            }
        }

        private DataTable SaveGridviewData()
        {
            //DataTable dataTable = FileClass.AddPTColumns();
            DataTable dataTable = MySession.Current.PositionsTrackerDt;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                foreach (GridViewRow gvRow in gvPosTracker.Rows)
                {
                    if (dataRow[enumPTColumns.Id.ToString()].ToString().Equals(gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumPTColumns.Id.ToString())].Text))
                    {
                        CheckBox chkSelect = gvRow.FindControl("chkSelect") as CheckBox;
                        DropDownList ddlOCType = gvRow.FindControl("ddlOCType") as DropDownList;
                        DropDownList ddlExpiryDates = gvRow.FindControl("ddlExpiryDates") as DropDownList;
                        DropDownList ddlContractType = gvRow.FindControl("ddlContractType") as DropDownList;
                        DropDownList ddlTransactionType = gvRow.FindControl("ddlTransactionType") as DropDownList;
                        DropDownList ddlStrikePrice = gvRow.FindControl("ddlStrikePrice") as DropDownList;
                        TextBox txtEntryPrice = gvRow.FindControl("txtEntryPrice") as TextBox;
                        TextBox txtExitPrice = gvRow.FindControl("txtExitPrice") as TextBox;
                        DropDownList ddlLots = gvRow.FindControl("ddlLots") as DropDownList;
                        DropDownList ddlStrategy = gvRow.FindControl("ddlStrategy") as DropDownList;
                        DropDownList ddlProfile = gvRow.FindControl("ddlProfile") as DropDownList;

                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.CMP.ToString())] = "0";
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.UnRealisedPL.ToString())] = "0";
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ChgPer.ToString())] = "0";
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.RealisedPL.ToString())] = "0";
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.MaxProfit.ToString())] = "0";
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Select.ToString())] = chkSelect.Checked;
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.OCType.ToString())] = ddlOCType.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ExpiryDate.ToString())] = ddlExpiryDates.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ContractType.ToString())] = ddlContractType.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.TransactionType.ToString())] = ddlTransactionType.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.StrikePrice.ToString())] = ddlStrikePrice.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Lots.ToString())] = ddlLots.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.EntryPrice.ToString())] = txtEntryPrice.Text;
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.ExitPrice.ToString())] = txtExitPrice.Text;
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Strategy.ToString())] = ddlStrategy.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumPTColumns.Profile.ToString())] = ddlProfile.SelectedValue;
                    }
                }
            }
            return dataTable;
        }

        public void FillAllExpiryDates(string ocType, System.Web.UI.WebControls.DropDownList ddlExpDt, bool IsAddAll)
        {
            if (IsAddAll)
                ddlExpDt.Items.Add("All");

            List<string> expiryDates = OCHelper.GetOCExpList(ocType);
            foreach (string item in expiryDates)
            {
                ddlExpDt.Items.Add(item);
            }

            for (int index = 0; index < MySession.Current.PositionsTrackerDt.Rows.Count; index++)
            {
                string item = MySession.Current.PositionsTrackerDt.Rows[index][enumPTColumns.ExpiryDate.ToString()].ToString();
                if (!ddlExpDt.Items.Contains(new ListItem(item)))
                    ddlExpDt.Items.Add(item);
            }
        }

        private void FillStrikePrice(DropDownList ddlSP)
        {
            List<int> expiryDates = new List<int>();

            //Add Nifty Strike Price
            expiryDates = OCHelper.GetOCSPList(enumOCType.NIFTY.ToString());
            foreach (int item in expiryDates)
            {
                ddlSP.Items.Add(item.ToString());
            }

            //Add BankNifty Strike Price
            expiryDates = OCHelper.GetOCSPList(enumOCType.BANKNIFTY.ToString());
            foreach (int item in expiryDates)
            {
                ddlSP.Items.Add(item.ToString());
            }

            for (int index = 0; index < MySession.Current.PositionsTrackerDt.Rows.Count; index++)
            {
                string item = MySession.Current.PositionsTrackerDt.Rows[index][enumPTColumns.StrikePrice.ToString()].ToString();
                if (!ddlSP.Items.Contains(new ListItem(item)))
                    ddlSP.Items.Add(item);
            }
        }

        private bool IsContainExpiryDate(string value)
        {
            bool result = false;
            foreach (string currentDate in MySession.Current.RecordsObject.expiryDates)
            {
                if (currentDate.Equals(value))
                {
                    result = true;
                    return result;
                }
            }
            return result;
        }

        protected void btnAddToStrategyBuilder_Click(object sender, EventArgs e)
        {
            DataTable dataSBTable = StrategyBuilderClass.AddSBColumns();
            foreach (GridViewRow gvRow in gvPosTracker.Rows)
            {
                CheckBox chkSelect = gvRow.FindControl("chkSelect") as CheckBox;

                DropDownList ddlExpiryDates = gvRow.FindControl("ddlExpiryDates") as DropDownList;
                DropDownList ddlContractType = gvRow.FindControl("ddlContractType") as DropDownList;
                DropDownList ddlTransactionType = gvRow.FindControl("ddlTransactionType") as DropDownList;
                DropDownList ddlStrikePrice = gvRow.FindControl("ddlStrikePrice") as DropDownList;
                TextBox txtEntryPrice = gvRow.FindControl("txtEntryPrice") as TextBox;
                DropDownList ddlLots = gvRow.FindControl("ddlLots") as DropDownList;

                if (chkSelect.Checked)
                {
                    dataSBTable.Rows.Add(false, ddlExpiryDates.SelectedValue, ddlContractType.SelectedValue, ddlTransactionType.SelectedValue,
                        ddlStrikePrice.SelectedValue, ddlLots.SelectedValue, enumDataSource.Positions.ToString(), Convert.ToDouble(txtEntryPrice.Text),
                        enumDataSource.Positions.ToString(), Guid.NewGuid());
                }
            }
            StrategyBuilderClass.AddSBRows(dataSBTable);
        }
    }
}