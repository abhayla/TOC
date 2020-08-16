using System;
using System.Web.UI.WebControls;
using System.Data;
using TOC.Strategy;

namespace TOC
{
    public partial class BasketOrder : System.Web.UI.Page
    {
        //private static int MAX_ROWS_ALLOWED = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dataTable = FileClass.ReadCsvFile(Constants.MYFILES_FOLDER_PATH + Constants.BO_FILE_NAME);

                if (dataTable.Rows.Count <= 0)
                    AddBlankRows(dataTable, Constants.BO_ADD_ROW_COUNT);

                MySession.Current.BasketOrderDt = dataTable;
                GridviewDataBind(dataTable);

                btnBuyBasketOrder.Attributes.Add("onclick", "CreateBasketOfOrdersFromBasketOrder('" + gvBasketOrder.ClientID + "')");
            }
        }

        private void GridviewDataBind(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                //Get latest premium value from the OC
                if (row[row.Table.Columns.IndexOf(enumBasketOrderColumns.ContractType.ToString())].ToString().Equals(enumContractType.CE.ToString()))
                {
                    Records.Datum.CE1 cE1 = OCHelper.GetCE(row[row.Table.Columns.IndexOf(enumBasketOrderColumns.OCType.ToString())].ToString(), row[row.Table.Columns.IndexOf(enumBasketOrderColumns.ExpiryDate.ToString())].ToString(),
                        Convert.ToInt32(row[row.Table.Columns.IndexOf(enumBasketOrderColumns.StrikePrice.ToString())]));
                    if (cE1 != null)
                        row[row.Table.Columns.IndexOf(enumBasketOrderColumns.CMP.ToString())] = cE1.lastPrice.ToString();
                    else
                        row[row.Table.Columns.IndexOf(enumBasketOrderColumns.CMP.ToString())] = "0";

                    row[row.Table.Columns.IndexOf(enumBasketOrderColumns.TradingSymbol.ToString())] =
                    string.Concat(enumOCType.NIFTY.ToString(), OCHelper.TradingSymbol_DateFormatter(OCHelper.DefaultExpDate(enumOCType.NIFTY.ToString())),
                    OCHelper.DefaultSP(enumOCType.NIFTY.ToString()), enumContractType.CE.ToString());
                }

                if (row[row.Table.Columns.IndexOf(enumBasketOrderColumns.ContractType.ToString())].ToString().Equals(enumContractType.PE.ToString()))
                {
                    Records.Datum.PE1 pE1 = OCHelper.GetPE(row[row.Table.Columns.IndexOf(enumBasketOrderColumns.OCType.ToString())].ToString(), row[row.Table.Columns.IndexOf(enumBasketOrderColumns.ExpiryDate.ToString())].ToString(),
                        Convert.ToInt32(row[row.Table.Columns.IndexOf(enumBasketOrderColumns.StrikePrice.ToString())]));
                    if (pE1 != null)
                        row[row.Table.Columns.IndexOf(enumBasketOrderColumns.CMP.ToString())] = pE1.lastPrice.ToString();
                    else
                        row[row.Table.Columns.IndexOf(enumBasketOrderColumns.CMP.ToString())] = "0";


                    row[row.Table.Columns.IndexOf(enumBasketOrderColumns.TradingSymbol.ToString())] =
                    string.Concat(enumOCType.NIFTY.ToString(), OCHelper.TradingSymbol_DateFormatter(OCHelper.DefaultExpDate(enumOCType.NIFTY.ToString())),
                    OCHelper.DefaultSP(enumOCType.NIFTY.ToString()), enumContractType.PE.ToString());
                }
            }

            gvBasketOrder.DataSource = dataTable;
            gvBasketOrder.DataBind();
        }

        private void AddBlankRows(DataTable dataTable, int iRowCount)
        {
            int rowstoadd = 0;
            if (dataTable.Rows.Count + iRowCount < Constants.BO_MAX_ROWS)
                rowstoadd = iRowCount;
            if (dataTable.Rows.Count + iRowCount >= Constants.BO_MAX_ROWS)
                rowstoadd = Constants.BO_MAX_ROWS - dataTable.Rows.Count;

            for (int iCount = 0; iCount < rowstoadd; iCount++)
            {
                BasketOrderClass.AddBlankRows(dataTable);
            }
        }

        protected void gvBasketOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dataTable = MySession.Current.BasketOrderDt;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                DropDownList ddlExpiryDates = e.Row.FindControl("ddlExpiryDates") as DropDownList;
                DropDownList ddlContractType = e.Row.FindControl("ddlContractType") as DropDownList;
                DropDownList ddlTransactionType = e.Row.FindControl("ddlTransactionType") as DropDownList;
                DropDownList ddlStrikePrice = e.Row.FindControl("ddlStrikePrice") as DropDownList;
                DropDownList ddlOCType = e.Row.FindControl("ddlOCType") as DropDownList;
                DropDownList ddlOrderType = e.Row.FindControl("ddlOrderType") as DropDownList;
                TextBox txtLimitPrice = e.Row.FindControl("txtLimitPrice") as TextBox;
                DropDownList ddlLots = e.Row.FindControl("ddlLots") as DropDownList;

                chkSelect.Checked = Convert.ToBoolean(dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.Select.ToString())]);

                OCHelper.FillAllExpiryDates(dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.OCType.ToString())].ToString(), ddlExpiryDates, false);
                OCHelper.FillAllStrikePrice(dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.OCType.ToString())].ToString(), ddlStrikePrice, false);

                Utility.SelectDataInCombo(ddlExpiryDates, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.ExpiryDate.ToString())].ToString());
                Utility.SelectDataInCombo(ddlContractType, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.ContractType.ToString())].ToString());
                Utility.SelectDataInCombo(ddlTransactionType, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.TransactionType.ToString())].ToString());
                Utility.SelectDataInCombo(ddlStrikePrice, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.StrikePrice.ToString())].ToString());
                Utility.SelectDataInCombo(ddlOCType, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.OCType.ToString())].ToString());
                Utility.SelectDataInCombo(ddlOrderType, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.OrderType.ToString())].ToString());
                txtLimitPrice.Text = dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.LimitPrice.ToString())].ToString();
                Utility.SelectDataInCombo(ddlLots, dataTable.Rows[e.Row.RowIndex][dataTable.Columns.IndexOf(enumBasketOrderColumns.Lots.ToString())].ToString());
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            DataTable newDataTable = dataTable.Clone();
            foreach (DataRow row in dataTable.Rows)
            {
                if (!Convert.ToBoolean(row[row.Table.Columns.IndexOf(enumBasketOrderColumns.Select.ToString())]))
                {
                    newDataTable.Rows.Add(row.ItemArray);
                }
            }
            MySession.Current.BasketOrderDt = newDataTable;
            GridviewDataBind(newDataTable);
        }

        protected void btnAddRows_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            AddBlankRows(dataTable, Constants.BO_ADD_ROW_COUNT);
            MySession.Current.BasketOrderDt = dataTable;
            GridviewDataBind(dataTable);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            FileClass.WriteDataTable(dataTable, Constants.MYFILES_FOLDER_PATH + Constants.BO_FILE_NAME);
            MySession.Current.BasketOrderDt = dataTable;
            GridviewDataBind(dataTable);
        }

        private DataTable SaveGridviewData()
        {
            DataTable dataTable = BasketOrderClass.AddBOColumns();

            foreach (GridViewRow gvRow in gvBasketOrder.Rows)
            {
                CheckBox chkSelect = gvRow.FindControl("chkSelect") as CheckBox;
                DropDownList ddlExpiryDates = gvRow.FindControl("ddlExpiryDates") as DropDownList;
                DropDownList ddlContractType = gvRow.FindControl("ddlContractType") as DropDownList;
                DropDownList ddlTransactionType = gvRow.FindControl("ddlTransactionType") as DropDownList;
                DropDownList ddlStrikePrice = gvRow.FindControl("ddlStrikePrice") as DropDownList;
                DropDownList ddlOCType = gvRow.FindControl("ddlOCType") as DropDownList;
                DropDownList ddlOrderType = gvRow.FindControl("ddlOrderType") as DropDownList;
                TextBox txtLimitPrice = gvRow.FindControl("txtLimitPrice") as TextBox;
                DropDownList ddlLots = gvRow.FindControl("ddlLots") as DropDownList;

                dataTable.Rows.Add(
                    chkSelect.Checked,
                    ddlExpiryDates.SelectedValue,
                    ddlContractType.SelectedValue,
                    ddlTransactionType.SelectedValue,
                    ddlStrikePrice.SelectedValue,
                    ddlOCType.SelectedValue,
                    ddlOrderType.SelectedValue,
                    txtLimitPrice.Text,
                    ddlLots.SelectedValue,
                    string.Concat(ddlOCType.SelectedValue, OCHelper.TradingSymbol_DateFormatter(ddlExpiryDates.SelectedValue), 
                    ddlStrikePrice.SelectedValue, ddlContractType.SelectedValue));
            }
            return dataTable;
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            DataTable dataPTTable = PositionsClass.AddPTColumns();
            foreach (GridViewRow gvRow in gvBasketOrder.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");

                if (chkSelect.Checked)
                {
                    dataPTTable.Rows.Add(false, gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.ExpiryDate.ToString())].Text,
                        enumContractType.CE.ToString(), enumTransactionType.SELL.ToString(), gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.StrikePrice.ToString())].Text, 1, 0);
                }
            }

            PositionsClass.AddPTRows(dataPTTable);
        }
    }
}