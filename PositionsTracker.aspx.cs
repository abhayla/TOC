using Antlr.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TOC
{
    public partial class PositionsTracker : System.Web.UI.Page
    {

        //Recommendation to close an open position based on current returns and remaining time in expiry
        private static string NIFTY = "NIFTY";
        //private static int NIFTY_COL_DIFF = 100;
        private static int NIFTY_LOT_SIZE = 75;

        private static string BANKNIFTY = "BANKNIFTY";
        //private static int BANKNIFTY_COL_DIFF = 200;
        private static int BANKNIFTY_LOT_SIZE = 20;

        //Gridview columns
        private static int DELETE_COL_INDEX = 0;
        private static int OC_TYPE_COL_INDEX = 1;
        private static int EXP_DATE_COL_INDEX = 2;
        private static int CONTRACT_TYP_COL_INDEX = 3;
        private static int TRANSTYP_COL_INDEX = 4;
        private static int SP_COL_INDEX = 5;
        private static int LOTS_COL_INDEX = 6;
        private static int ENTRY_PRICE_COL_INDEX = 7;
        private static int EXIT_PRICE_COL_INDEX = 8;
        private static int CMP_COL_INDEX = 9;
        private static int PL_UR_COL_INDEX = 10;
        private static int CHG_COL_INDEX = 11;
        private static int PL_R_COL_INDEX = 12;
        private static int STRATEGY_COL_INDEX = 13;
        private static int PROFILE_COL_INDEX = 14;
        private static int POSITION_COL_INDEX = 15;
        private static int ID_COL_INDEX = 16;
        private static int MAX_PROFIT_COL_INDEX = 17;

        private static int MAX_ROWS_ALLOWED = 500;

        private static string PT_FILE_NAME = "PositionsTracker.csv";
        private static string MYFILES_FOLDER_PATH = "C:\\Myfiles\\";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dataTable = FileClass.ReadSBCsvFile(MYFILES_FOLDER_PATH + PT_FILE_NAME);

                if (dataTable.Rows.Count <= 0)
                    AddBlankRows(dataTable, 2);

                MySession.Current.PositionsTrackerDt = dataTable;
                GridviewDataBind(dataTable);

                FillAllExpiryDates(NIFTY, ddlFilterExpiryDates, true);
            }
        }

        protected void gvPosTracker_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //DataTable dataTable = MySession.Current.PositionsTrackerDt;
            DataTable dataTable = gvPosTracker.DataSource as DataTable;

            //Logic for header rows
            if (e.Row.RowType == DataControlRowType.Header)
            {

            }

            //Logic for nomal rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkDelete = e.Row.FindControl("chkDelete") as CheckBox;
                chkDelete.Checked = Convert.ToBoolean(dataTable.Rows[e.Row.RowIndex][DELETE_COL_INDEX]);

                DropDownList ddlOCType = e.Row.FindControl("ddlOCType") as DropDownList;
                DropDownList ddlExpiryDates = e.Row.FindControl("ddlExpiryDates") as DropDownList;
                DropDownList ddlContractType = e.Row.FindControl("ddlContractType") as DropDownList;
                DropDownList ddlTransactionType = e.Row.FindControl("ddlTransactionType") as DropDownList;
                DropDownList ddlStrikePrice = e.Row.FindControl("ddlStrikePrice") as DropDownList;
                DropDownList ddlLots = e.Row.FindControl("ddlLots") as DropDownList;
                DropDownList ddlStrategy = e.Row.FindControl("ddlStrategy") as DropDownList;
                //DropDownList ddlPosition = e.Row.FindControl("ddlPosition") as DropDownList;
                DropDownList ddlProfile = e.Row.FindControl("ddlProfile") as DropDownList;

                TextBox txtEntryPrice = e.Row.FindControl("txtEntryPrice") as TextBox;
                TextBox txtExitPrice = e.Row.FindControl("txtExitPrice") as TextBox;
                txtEntryPrice.Style["text-align"] = "right";
                txtExitPrice.Style["text-align"] = "right";

                e.Row.Cells[CMP_COL_INDEX].Text = dataTable.Rows[e.Row.RowIndex][CMP_COL_INDEX].ToString();
                e.Row.Cells[PL_UR_COL_INDEX].Text = dataTable.Rows[e.Row.RowIndex][PL_UR_COL_INDEX].ToString();
                e.Row.Cells[PL_R_COL_INDEX].Text = dataTable.Rows[e.Row.RowIndex][PL_R_COL_INDEX].ToString();
                e.Row.Cells[CHG_COL_INDEX].Text = dataTable.Rows[e.Row.RowIndex][CHG_COL_INDEX].ToString() + "%";
                Utility.SelectDataInCombo(ddlOCType, dataTable.Rows[e.Row.RowIndex][OC_TYPE_COL_INDEX].ToString());
                //FillExpiryDates(ddlExpiryDates);
                FillAllExpiryDates(dataTable.Rows[e.Row.RowIndex][OC_TYPE_COL_INDEX].ToString(), ddlExpiryDates, false);
                FillStrikePrice(ddlStrikePrice);
                Utility.SelectDataInCombo(ddlExpiryDates, dataTable.Rows[e.Row.RowIndex][EXP_DATE_COL_INDEX].ToString());
                Utility.SelectDataInCombo(ddlContractType, dataTable.Rows[e.Row.RowIndex][CONTRACT_TYP_COL_INDEX].ToString());
                Utility.SelectDataInCombo(ddlTransactionType, dataTable.Rows[e.Row.RowIndex][TRANSTYP_COL_INDEX].ToString());
                Utility.SelectDataInCombo(ddlStrikePrice, dataTable.Rows[e.Row.RowIndex][SP_COL_INDEX].ToString());

                txtEntryPrice.Text = dataTable.Rows[e.Row.RowIndex][ENTRY_PRICE_COL_INDEX].ToString();
                txtExitPrice.Text = dataTable.Rows[e.Row.RowIndex][EXIT_PRICE_COL_INDEX].ToString();
                Utility.SelectDataInCombo(ddlLots, dataTable.Rows[e.Row.RowIndex][LOTS_COL_INDEX].ToString());
                Utility.SelectDataInCombo(ddlStrategy, dataTable.Rows[e.Row.RowIndex][STRATEGY_COL_INDEX].ToString());
                //Utility.SelectDataInCombo(ddlPosition, dataTable.Rows[e.Row.RowIndex][POSITION_COL_START_INDEX].ToString());
                Utility.SelectDataInCombo(ddlProfile, dataTable.Rows[e.Row.RowIndex][PROFILE_COL_INDEX].ToString());

                //if(dataTable.Rows[e.Row.RowIndex][POSITION_COL_START_INDEX].ToString().Equals("Close"))
                //{
                //    e.Row.Enabled = false;
                //}
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                double realisedProfit = 0;
                double unRealisedProfit = 0;
                double maxProfit = 0;
                for (int irowcount = 0; irowcount < dataTable.Rows.Count; irowcount++)
                {
                    if (dataTable.Rows[irowcount][POSITION_COL_INDEX].ToString().Equals("Open"))
                    {
                        unRealisedProfit += Convert.ToDouble(dataTable.Rows[irowcount][PL_UR_COL_INDEX]);
                        maxProfit += Convert.ToDouble(dataTable.Rows[irowcount][MAX_PROFIT_COL_INDEX]);
                    }
                        
                    if (dataTable.Rows[irowcount][POSITION_COL_INDEX].ToString().Equals("Close"))
                        realisedProfit += Convert.ToDouble(dataTable.Rows[irowcount][PL_R_COL_INDEX]);
                }
                e.Row.Cells[PL_UR_COL_INDEX].Text = unRealisedProfit.ToString();
                e.Row.Cells[PL_R_COL_INDEX].Text = realisedProfit.ToString();
                e.Row.Cells[MAX_PROFIT_COL_INDEX].Text = maxProfit.ToString();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            DataTable newDataTable = dataTable.Clone();
            foreach (DataRow row in dataTable.Rows)
            {
                if (!Convert.ToBoolean(row[DELETE_COL_INDEX]))
                {
                    newDataTable.Rows.Add(row.ItemArray);
                }
            }
            MySession.Current.PositionsTrackerDt = newDataTable;
            GridviewDataBind(newDataTable);
        }

        protected void btnAddRows_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            AddBlankRows(dataTable, 2);
            MySession.Current.PositionsTrackerDt = dataTable;
            GridviewDataBind(dataTable);
        }

        protected void btnUpdateCMP_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            MySession.Current.PositionsTrackerDt = dataTable;
            GridviewDataBind(dataTable);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            FileClass.WriteDataTable(dataTable, MYFILES_FOLDER_PATH + PT_FILE_NAME);
            MySession.Current.PositionsTrackerDt = dataTable;
            GridviewDataBind(dataTable);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            DataTable dataTable = MySession.Current.PositionsTrackerDt;
            string selectQuery = GetFilterString();
            DataRow[] drs = dataTable.Select(selectQuery);
            DataTable filteredDataTable = dataTable.Clone();
            foreach (DataRow dr in drs)
            {
                filteredDataTable.ImportRow(dr);
            }
            GridviewDataBind(filteredDataTable);
        }

        private string GetFilterString()
        {
            string strFilter = string.Empty;
            List<string> filters = new List<string>();

            if (!ddlFilterExpiryDates.SelectedValue.Equals("All"))
                filters.Add(" ([Expiry Date] = #" + ddlFilterExpiryDates.SelectedValue + "#) ");

            if (!ddlFilterProfiles.SelectedValue.Equals("All"))
                filters.Add(" (Profile = '" + ddlFilterProfiles.SelectedValue + "') ");

            if (!ddlFilterStrategy.SelectedValue.Equals("All"))
                filters.Add(" (Strategy = '" + ddlFilterStrategy.SelectedValue + "') ");

            if (!ddlFilterPostionStatus.SelectedValue.Equals("All"))
                filters.Add(" (Position = '" + ddlFilterPostionStatus.SelectedValue + "') ");

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
                if (row[CONTRACT_TYP_COL_INDEX].ToString().Equals(enumContractType.CE.ToString()))
                {
                    Records.Datum.CE1 cE1 = OCHelper.GetCE(row[OC_TYPE_COL_INDEX].ToString(), row[EXP_DATE_COL_INDEX].ToString(),
                        Convert.ToInt32(row[SP_COL_INDEX]));
                    if (cE1 != null)
                        row[CMP_COL_INDEX] = cE1.lastPrice.ToString();
                    else
                        row[CMP_COL_INDEX] = "0";
                }

                if (row[CONTRACT_TYP_COL_INDEX].ToString().Equals(enumContractType.PE.ToString()))
                {
                    Records.Datum.PE1 pE1 = OCHelper.GetPE(row[OC_TYPE_COL_INDEX].ToString(), row[EXP_DATE_COL_INDEX].ToString(),
                        Convert.ToInt32(row[SP_COL_INDEX]));
                    if (pE1 != null)
                        row[CMP_COL_INDEX] = pE1.lastPrice.ToString();
                    else
                        row[CMP_COL_INDEX] = "0";
                }

                int lotSize = 0;
                if (row[OC_TYPE_COL_INDEX].ToString().Equals(NIFTY))
                    lotSize = NIFTY_LOT_SIZE;
                if (row[OC_TYPE_COL_INDEX].ToString().Equals(BANKNIFTY))
                    lotSize = BANKNIFTY_LOT_SIZE;

                //If current expiry date is not in the Option Chain fetched list then mark the current row position as close
                //if (!MySession.Current.PositionsTrackerDt.Columns["Expiry Date"].ToString().Contains(row["Expiry Date"].ToString()))

                if (!IsContainExpiryDate(row["Expiry Date"].ToString()))
                {
                    row[POSITION_COL_INDEX] = "Close";
                }
                else if (Convert.ToDouble(row[EXIT_PRICE_COL_INDEX]) > 0)
                {
                    row[POSITION_COL_INDEX] = "Close";
                }
                else
                {
                    row[POSITION_COL_INDEX] = "Open";
                }

                double entryPrice = Convert.ToDouble(row[ENTRY_PRICE_COL_INDEX]);
                double exitPrice = 0.0;

                if (row[POSITION_COL_INDEX].ToString().Equals("Close"))
                {
                    exitPrice = Convert.ToDouble(row[EXIT_PRICE_COL_INDEX]);

                    if (row[TRANSTYP_COL_INDEX].ToString().Equals("BUY"))
                        row[PL_R_COL_INDEX] = Math.Round((exitPrice - entryPrice) * Convert.ToInt32(row[LOTS_COL_INDEX]), 2) * lotSize;

                    if (row[TRANSTYP_COL_INDEX].ToString().Equals("SELL"))
                        row[PL_R_COL_INDEX] = Math.Round((entryPrice - exitPrice) * Convert.ToInt32(row[LOTS_COL_INDEX]), 2) * lotSize;
                }

                if (row[POSITION_COL_INDEX].ToString().Equals("Open"))
                {
                    exitPrice = Convert.ToDouble(row[CMP_COL_INDEX]);

                    if (row[TRANSTYP_COL_INDEX].ToString().Equals("BUY"))
                        row[PL_UR_COL_INDEX] = Math.Round((exitPrice - entryPrice) * Convert.ToInt32(row[LOTS_COL_INDEX]), 2) * lotSize;

                    if (row[TRANSTYP_COL_INDEX].ToString().Equals("SELL"))
                        row[PL_UR_COL_INDEX] = Math.Round((entryPrice - exitPrice) * Convert.ToInt32(row[LOTS_COL_INDEX]), 2) * lotSize;

                    row[MAX_PROFIT_COL_INDEX] = Math.Round(entryPrice * Convert.ToInt32(row[LOTS_COL_INDEX]), 2) * lotSize;
                }

                if (row[TRANSTYP_COL_INDEX].ToString().Equals("BUY"))
                    row[CHG_COL_INDEX] = Math.Round((exitPrice - entryPrice) * 100 / entryPrice, 2);

                if (row[TRANSTYP_COL_INDEX].ToString().Equals("SELL"))
                    row[CHG_COL_INDEX] = Math.Round((entryPrice - exitPrice) * 100 / entryPrice, 2);

                if (row[ID_COL_INDEX].ToString().Trim().Equals(string.Empty))
                    row[ID_COL_INDEX] = Guid.NewGuid();
            }

            //MySession.Current.PositionsTrackerDt = dataTable;
            gvPosTracker.DataSource = dataTable;
            gvPosTracker.DataBind();
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
                dataTable.Rows.Add(false, NIFTY, OCHelper.DefaultExpDate(NIFTY), "CE", "BUY", OCHelper.DefaultSP(NIFTY),
                    "1", "0", "0", "0", "0", "0", "0", "Butterfly", "Abhay", "Open", Guid.NewGuid(),"0");

                //dataTable.Columns.Add("Delete");
                //dataTable.Columns.Add("OC Type");
                //dataTable.Columns.Add("OC Type");
                //dataTable.Columns.Add("Expiry Date");
                //dataTable.Columns.Add("Contract Type");
                //dataTable.Columns.Add("Transaction Type");
                //dataTable.Columns.Add("Strike Price");
                //dataTable.Columns.Add("Lots");
                //dataTable.Columns.Add("Entry Price");
                //dataTable.Columns.Add("Exit Price");
                //dataTable.Columns.Add("CMP");
                //dataTable.Columns.Add("P/L");
                //dataTable.Columns.Add("Chg %");
                //dataTable.Columns.Add("Realised P/L");
                //dataTable.Columns.Add("Strategy");
                //dataTable.Columns.Add("Position");
                //dataTable.Columns.Add("Profiles");
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
                    if (dataRow["Id"].ToString().Equals(gvRow.Cells[ID_COL_INDEX].Text))
                    {
                        CheckBox chkDelete = gvRow.Cells[DELETE_COL_INDEX].FindControl("chkDelete") as CheckBox;
                        DropDownList ddlOCType = gvRow.Cells[OC_TYPE_COL_INDEX].FindControl("ddlOCType") as DropDownList;
                        DropDownList ddlExpiryDates = gvRow.Cells[EXP_DATE_COL_INDEX].FindControl("ddlExpiryDates") as DropDownList;
                        DropDownList ddlContractType = gvRow.Cells[CONTRACT_TYP_COL_INDEX].FindControl("ddlContractType") as DropDownList;
                        DropDownList ddlTransactionType = gvRow.Cells[TRANSTYP_COL_INDEX].FindControl("ddlTransactionType") as DropDownList;
                        DropDownList ddlStrikePrice = gvRow.Cells[SP_COL_INDEX].FindControl("ddlStrikePrice") as DropDownList;
                        TextBox txtEntryPrice = gvRow.Cells[ENTRY_PRICE_COL_INDEX].FindControl("txtEntryPrice") as TextBox;
                        TextBox txtExitPrice = gvRow.Cells[EXIT_PRICE_COL_INDEX].FindControl("txtExitPrice") as TextBox;
                        DropDownList ddlLots = gvRow.Cells[LOTS_COL_INDEX].FindControl("ddlLots") as DropDownList;
                        DropDownList ddlStrategy = gvRow.Cells[STRATEGY_COL_INDEX].FindControl("ddlStrategy") as DropDownList;
                        //DropDownList ddlPosition = gvRow.Cells[POSITION_COL_START_INDEX].FindControl("ddlPosition") as DropDownList;
                        DropDownList ddlProfile = gvRow.Cells[PROFILE_COL_INDEX].FindControl("ddlProfile") as DropDownList;

                        //dataTable.Rows.Add(chkDelete.Checked, ddlOCType.SelectedValue, ddlExpiryDates.SelectedValue, ddlContractType.SelectedValue,
                        //    ddlTransactionType.SelectedValue, ddlStrikePrice.SelectedValue, ddlLots.SelectedValue, txtEntryPrice.Text,
                        //    txtExitPrice.Text, "0", "0", "0", "0", ddlStrategy.SelectedValue, ddlProfile.SelectedValue, "", Guid.NewGuid());

                        dataRow[DELETE_COL_INDEX] = chkDelete.Checked;
                        dataRow[OC_TYPE_COL_INDEX] = ddlOCType.SelectedValue;
                        dataRow[EXP_DATE_COL_INDEX] = ddlExpiryDates.SelectedValue;
                        dataRow[CONTRACT_TYP_COL_INDEX] = ddlContractType.SelectedValue;
                        dataRow[TRANSTYP_COL_INDEX] = ddlTransactionType.SelectedValue;
                        dataRow[SP_COL_INDEX] = ddlStrikePrice.SelectedValue;
                        dataRow[LOTS_COL_INDEX] = ddlLots.SelectedValue;
                        dataRow[ENTRY_PRICE_COL_INDEX] = txtEntryPrice.Text;
                        dataRow[EXIT_PRICE_COL_INDEX] = txtExitPrice.Text;
                        dataRow[STRATEGY_COL_INDEX] = ddlStrategy.SelectedValue;
                        dataRow[PROFILE_COL_INDEX] = ddlProfile.SelectedValue;
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
                string item = MySession.Current.PositionsTrackerDt.Rows[index]["Expiry Date"].ToString();
                if (!ddlExpDt.Items.Contains(new ListItem(item)))
                    ddlExpDt.Items.Add(item);
            }
        }

        private void FillStrikePrice(DropDownList ddlSP)
        {
            List<int> expiryDates = new List<int>();

            //Add Nifty Strike Price
            expiryDates = OCHelper.GetOCSPList(NIFTY);
            foreach (int item in expiryDates)
            {
                ddlSP.Items.Add(item.ToString());
            }

            //Add BankNifty Strike Price
            expiryDates = OCHelper.GetOCSPList(BANKNIFTY);
            foreach (int item in expiryDates)
            {
                ddlSP.Items.Add(item.ToString());
            }

            for (int index = 0; index < MySession.Current.PositionsTrackerDt.Rows.Count; index++)
            {
                string item = MySession.Current.PositionsTrackerDt.Rows[index]["Strike Price"].ToString();
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
    }
}