using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using TOC.Utility;

namespace TOC
{
    public partial class StrategyBuilder : System.Web.UI.Page
    {
        private static string NIFTY = "NIFTY";
        private static int NIFTY_COL_DIFF = 100;
        private static int NIFTY_LOT_SIZE = 75;

        private static string BANKNIFTY = "BANKNIFTY";
        private static int BANKNIFTY_COL_DIFF = 200;
        private static int BANKNIFTY_LOT_SIZE = 20;

        //Gridview columns
        private static int DELETE_COL_INDEX = 0;
        private static int EXP_DATE_COL_INDEX = 1;
        private static int CONTRACT_TYP_COL_INDEX = 2;
        private static int TRANSTYP_COL_INDEX = 3;
        private static int SP_COL_INDEX = 4;
        private static int CMP_COL_INDEX = 5;
        private static int PREMINUM_COL_INDEX = 6;
        private static int LOTS_COL_INDEX = 7;
        private static int SELECTED_SP_COL_INDEX = 18;
        private static int LOWER_SP_COL_START_INDEX = 8;
        private static int HIGHER_SP_COL_END_INDEX = 28;

        private static int MAX_ROWS_ALLOWED = 15;

        private static string SB_FILE_NAME = "StrategyBuilder.csv";
        private static string MYFILES_FOLDER_PATH = "C:\\Myfiles\\";

        Records recordsObject = new Records();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dataTable = FileClass.ReadCsvFile(MYFILES_FOLDER_PATH + SB_FILE_NAME);
                //AddBlankRows(dataTable, 2);
                MySession.Current.StrategyBuilderDt = dataTable;
                //FillExpiryDates(ddlExpiryDates);
                GridviewDataBind(dataTable);
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
                dataTable.Rows.Add(false, "", "CE", "BUY", OCHelper.DefaultSP(rblOCType.SelectedValue), "", "", "1",
                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
            }
        }

        protected void rblOCType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillExpiryDates(ddlExpiryDates);
        }

        private void GridviewDataBind(DataTable dataTable)
        {
            int avgOfSP = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Strike Price"] != null && row["Strike Price"].ToString().Trim().Length > 0)
                    avgOfSP += Convert.ToInt32(row["Strike Price"]);
            }

            if (dataTable.Rows.Count > 0)
                avgOfSP = avgOfSP / dataTable.Rows.Count;

            int columnDiff = 0;
            if (rblOCType.SelectedValue.Equals(NIFTY))
                columnDiff = NIFTY_COL_DIFF;
            if (rblOCType.SelectedValue.Equals(BANKNIFTY))
                columnDiff = BANKNIFTY_COL_DIFF;

            //Assign header value to the center column
            dataTable.Columns[SELECTED_SP_COL_INDEX].ColumnName = avgOfSP.ToString();
            for (int icount = SELECTED_SP_COL_INDEX - 1; icount >= LOWER_SP_COL_START_INDEX; icount--)
            {
                dataTable.Columns[icount].ColumnName =
                    Math.Round(Convert.ToDouble(avgOfSP - (SELECTED_SP_COL_INDEX - icount) * columnDiff), 0).ToString();
            }
            //Assign value to the right columns from the center
            for (int icount = SELECTED_SP_COL_INDEX + 1; icount <= HIGHER_SP_COL_END_INDEX; icount++)
            {
                dataTable.Columns[icount].ColumnName =
                    Math.Round(Convert.ToDouble(avgOfSP + (icount - SELECTED_SP_COL_INDEX) * columnDiff), 0).ToString();
            }

            int lotsSize = 0;
            if (rblOCType.SelectedValue.Equals(NIFTY))
                lotsSize = NIFTY_LOT_SIZE;
            if (rblOCType.SelectedValue.Equals(BANKNIFTY))
                lotsSize = BANKNIFTY_LOT_SIZE;

            foreach (DataRow row in dataTable.Rows)
            {
                //Get latest premium value from the OC
                if (row[CONTRACT_TYP_COL_INDEX].ToString().Equals(enumContractType.CE.ToString()))
                {
                    Records.Datum.CE1 cE1 = OCHelper.GetCE(rblOCType.SelectedValue, row[EXP_DATE_COL_INDEX].ToString(),
                        Convert.ToInt32(row[SP_COL_INDEX]));
                    if (cE1 != null)
                        row[CMP_COL_INDEX] = cE1.lastPrice.ToString();
                    else
                        row[CMP_COL_INDEX] = "0";

                }

                if (row[CONTRACT_TYP_COL_INDEX].ToString().Equals(enumContractType.PE.ToString()))
                {
                    Records.Datum.PE1 pE1 = OCHelper.GetPE(rblOCType.SelectedValue, row[EXP_DATE_COL_INDEX].ToString(),
                        Convert.ToInt32(row[SP_COL_INDEX]));
                    if (pE1 != null)
                        row[CMP_COL_INDEX] = pE1.lastPrice.ToString();
                    else
                        row[CMP_COL_INDEX] = "0";
                }

                for (int icount = LOWER_SP_COL_START_INDEX; icount <= HIGHER_SP_COL_END_INDEX; icount++)
                {
                    row[icount] = Convert.ToString(Convert.ToInt32(row[LOTS_COL_INDEX]) * lotsSize *
                                CalculateExpiryValue(row[CONTRACT_TYP_COL_INDEX].ToString(),
                                row[TRANSTYP_COL_INDEX].ToString(),
                                Convert.ToDouble(row[SP_COL_INDEX]),
                                Convert.ToDouble(row[CMP_COL_INDEX]),
                                Convert.ToDouble(dataTable.Columns[icount].ColumnName)));
                }
            }

            MySession.Current.StrategyBuilderDt = dataTable;
            gvStrategy.DataSource = dataTable;
            gvStrategy.DataBind();
        }

        protected void gvStrategy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dataTable = MySession.Current.StrategyBuilderDt;

            //Logic for header rows
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[SELECTED_SP_COL_INDEX].Text = dataTable.Columns[SELECTED_SP_COL_INDEX].ColumnName;
                //gvStrategy.Columns[SELECTED_SP_COL_INDEX].HeaderText = dataTable.Columns[SELECTED_SP_COL_INDEX].ColumnName;

                //Assign value to the left columns from the center
                for (int icount = SELECTED_SP_COL_INDEX - 1; icount >= LOWER_SP_COL_START_INDEX; icount--)
                {
                    e.Row.Cells[icount].Text = dataTable.Columns[icount].ColumnName;
                    //gvStrategy.Columns[icount].HeaderText = dataTable.Columns[icount].ColumnName;
                }

                //Assign value to the right columns from the center
                for (int icount = SELECTED_SP_COL_INDEX + 1; icount <= HIGHER_SP_COL_END_INDEX; icount++)
                {
                    e.Row.Cells[icount].Text = dataTable.Columns[icount].ColumnName;
                    //gvStrategy.Columns[icount].HeaderText = dataTable.Columns[icount].ColumnName;
                }
            }

            //Logic for nomal rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkDelete = e.Row.FindControl("chkDelete") as CheckBox;
                DropDownList ddlExpiryDates = e.Row.FindControl("ddlExpiryDates") as DropDownList;
                DropDownList ddlContractType = e.Row.FindControl("ddlContractType") as DropDownList;
                DropDownList ddlTransactionType = e.Row.FindControl("ddlTransactionType") as DropDownList;
                DropDownList ddlStrikePrice = e.Row.FindControl("ddlStrikePrice") as DropDownList;
                TextBox txtPremium = e.Row.FindControl("txtPremium") as TextBox;
                DropDownList ddlLots = e.Row.FindControl("ddlLots") as DropDownList;
                e.Row.Cells[CMP_COL_INDEX].Text = dataTable.Rows[e.Row.RowIndex][CMP_COL_INDEX].ToString();
                chkDelete.Checked = Convert.ToBoolean(dataTable.Rows[e.Row.RowIndex][DELETE_COL_INDEX]);
                FillExpiryDates(ddlExpiryDates);
                FillStrikePrice(ddlStrikePrice);
                Utility.SelectDataInCombo(ddlExpiryDates, dataTable.Rows[e.Row.RowIndex][EXP_DATE_COL_INDEX].ToString());
                Utility.SelectDataInCombo(ddlContractType, dataTable.Rows[e.Row.RowIndex][CONTRACT_TYP_COL_INDEX].ToString());
                Utility.SelectDataInCombo(ddlTransactionType, dataTable.Rows[e.Row.RowIndex][TRANSTYP_COL_INDEX].ToString());
                Utility.SelectDataInCombo(ddlStrikePrice, dataTable.Rows[e.Row.RowIndex][SP_COL_INDEX].ToString());

                txtPremium.Text = dataTable.Rows[e.Row.RowIndex][PREMINUM_COL_INDEX].ToString();
                Utility.SelectDataInCombo(ddlLots, dataTable.Rows[e.Row.RowIndex][LOTS_COL_INDEX].ToString());

                for (int icount = LOWER_SP_COL_START_INDEX; icount <= HIGHER_SP_COL_END_INDEX; icount++)
                {
                    e.Row.Cells[icount].Text = dataTable.Rows[e.Row.RowIndex][icount].ToString();
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
                txtPremium.Attributes.Add("onchange", "CalcExpValForAllCellsInRow('" + gvStrategy.ClientID + "', '"
                    + rowindex + "','" + rblOCType.ClientID + "');");
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int sum = 0;
                for (int icount = LOWER_SP_COL_START_INDEX; icount <= HIGHER_SP_COL_END_INDEX; icount++)
                {
                    sum = 0;
                    for (int irowcount = 0; irowcount < dataTable.Rows.Count; irowcount++)
                    {
                        sum += Convert.ToInt32(dataTable.Rows[irowcount][icount]);
                        //e.Row.Cells[icount].Text += dataTable.Rows[irowcount][icount];
                    }
                    e.Row.Cells[icount].Text = sum.ToString();
                }
            }
        }

        protected double CalculateExpiryValue(string contractType, string transactionType, double strikePrice,
            double premiumPaid, double expiryPrice)
        {
            double result = 0;
            if (contractType == enumContractType.PE.ToString() && transactionType == enumTransactionType.BUY.ToString())
                result = FO.PutBuy(strikePrice, premiumPaid, expiryPrice);
            if (contractType == enumContractType.PE.ToString() && transactionType == enumTransactionType.SELL.ToString())
                result = FO.PutSell(strikePrice, premiumPaid, expiryPrice);
            if (contractType == enumContractType.CE.ToString() && transactionType == enumTransactionType.BUY.ToString())
                result = FO.CallBuy(strikePrice, premiumPaid, expiryPrice);
            if (contractType == enumContractType.CE.ToString() && transactionType == enumTransactionType.SELL.ToString())
                result = FO.CallSell(strikePrice, premiumPaid, expiryPrice);
            if (contractType == enumContractType.EQ.ToString() && transactionType == enumTransactionType.BUY.ToString())
                result = FO.EQBuy(strikePrice, expiryPrice);
            if (contractType == enumContractType.FUT.ToString() && transactionType == enumTransactionType.BUY.ToString())
                result = FO.FutBuy(strikePrice, expiryPrice);
            if (contractType == enumContractType.FUT.ToString() && transactionType == enumTransactionType.SELL.ToString())
                result = FO.FutSell(strikePrice, expiryPrice);

            return result;
        }

        private void FillExpiryDates(DropDownList ddlExpDt)
        {
            List<string> expiryDates = OCHelper.GetOCExpList(rblOCType.SelectedValue);
            foreach (string item in expiryDates)
            {
                ddlExpDt.Items.Add(item);
            }
        }

        private void FillStrikePrice(DropDownList ddlSP)
        {
            List<int> expiryDates = OCHelper.GetOCSPList(rblOCType.SelectedValue);
            foreach (int item in expiryDates)
            {
                ddlSP.Items.Add(item.ToString());
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
            DataTable dataTable = FileClass.AddColumns();

            foreach (GridViewRow gvRow in gvStrategy.Rows)
            {
                CheckBox chkDelete = gvRow.Cells[DELETE_COL_INDEX].FindControl("chkDelete") as CheckBox;
                DropDownList ddlExpiryDates = gvRow.Cells[EXP_DATE_COL_INDEX].FindControl("ddlExpiryDates") as DropDownList;
                DropDownList ddlContractType = gvRow.Cells[CONTRACT_TYP_COL_INDEX].FindControl("ddlContractType") as DropDownList;
                DropDownList ddlTransactionType = gvRow.Cells[TRANSTYP_COL_INDEX].FindControl("ddlTransactionType") as DropDownList;
                DropDownList ddlStrikePrice = gvRow.Cells[SP_COL_INDEX].FindControl("ddlStrikePrice") as DropDownList;
                TextBox txtPremium = gvRow.Cells[PREMINUM_COL_INDEX].FindControl("txtPremium") as TextBox;
                DropDownList ddlLots = gvRow.Cells[LOTS_COL_INDEX].FindControl("ddlLots") as DropDownList;

                dataTable.Rows.Add(chkDelete.Checked, ddlExpiryDates.SelectedValue, ddlContractType.SelectedValue, ddlTransactionType.SelectedValue,
                    ddlStrikePrice.SelectedValue, "", txtPremium.Text, ddlLots.SelectedValue, "", "", "", "", "", "", "",
                    "", "", "", "", "", "", "", "", "", "", "", "", "", "");
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
                if (!Convert.ToBoolean(row[DELETE_COL_INDEX]))
                {
                    newDataTable.Rows.Add(row.ItemArray);
                }
            }
            MySession.Current.StrategyBuilderDt = newDataTable;
            GridviewDataBind(newDataTable);
        }
    }
}