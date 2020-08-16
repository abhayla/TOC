using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Drawing;
using TOC.Strategy;

namespace TOC
{
    public partial class StrategyBuilder : System.Web.UI.Page
    {
        DataTable dataTableForColumns = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dataTable = FileClass.ReadCsvFile(Constants.MYFILES_FOLDER_PATH + Constants.SB_FILE_NAME);
                OCHelper.FillAllExpiryDates(enumOCType.NIFTY.ToString(), ddlFilterExpiryDates, true);
                if (dataTable.Rows.Count <= 0)
                    AddBlankRows(dataTable, Constants.SB_ADD_ROW_COUNT);

                MySession.Current.StrategyBuilderDt = dataTable;
                DataTable filteredDataTable = FilterRecords();
                GridviewDataBind(filteredDataTable);
                //GridviewDataBind(dataTable);
                lblLastFetchedTime.Text = MySession.Current.RecordsObject.timestamp;
                lblLastPrice.Text = MySession.Current.RecordsObject.underlyingValue.ToString();
            }
        }

        private void AddBlankRows(DataTable dataTable, int iRowCount)
        {
            int rowstoadd = 0;
            if (dataTable.Rows.Count + iRowCount < Constants.SB_MAX_ROWS)
                rowstoadd = iRowCount;
            if (dataTable.Rows.Count + iRowCount >= Constants.SB_MAX_ROWS)
                rowstoadd = Constants.SB_MAX_ROWS - dataTable.Rows.Count;

            for (int iCount = 0; iCount < rowstoadd; iCount++)
            {
                AddBlankRows(dataTable);
            }
        }

        public void AddBlankRows(DataTable dataTable)
        {
            dataTable.Rows.Add(
                    false,                                                      //Select,
                    OCHelper.DefaultExpDate(enumOCType.NIFTY.ToString()),       //ExpiryDate,
                    enumContractType.CE.ToString(),                             //ContractType,
                    enumTransactionType.SELL.ToString(),                        //TransactionType,
                    OCHelper.DefaultSP(enumOCType.NIFTY.ToString()),            //StrikePrice,
                    1,                                                          //Lots,
                    enumDataSource.Others.ToString(),                           //DataSource,
                    0,                                                          //CMP
                    ddlStrategyListFilter.SelectedValue,                        //StrategyName
                    Guid.NewGuid()                                              //Id
                );
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

            int sumOfLowerPremiums = 0;
            int sumOfHigherPremiums = 0;
            int countOfLowerPremiums = 0;
            int countOfHigherPremiums = 0;
            int avgOfHigherPremiums = 0;
            int avgOfLowerPremiums = 0;

            //dataTable.DefaultView.Sort = enumSBColumns.CMP.ToString() + " ASC";
            //dataTable.DefaultView.Sort = enumSBColumns.StrikePrice.ToString() + " DESC";

            foreach (DataRow row in dataTable.Rows)
            {
                if (!row[row.Table.Columns.IndexOf(enumSBColumns.DataSource.ToString())].ToString().Equals(enumDataSource.Positions.ToString()))
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

                            sumOfHigherPremiums += cutOffLowerSP;
                            countOfHigherPremiums++;
                        }

                        if (row[row.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString().Equals("CE"))
                        {
                            cutOffLowerSP = Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())]) -
                                         Math.Abs(Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())]));

                            sumOfLowerPremiums += cutOffLowerSP;
                            countOfLowerPremiums++;
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

                            sumOfLowerPremiums += cutOffHigherSP;
                            countOfLowerPremiums++;
                        }

                        if (row[row.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString().Equals("CE"))
                        {
                            cutOffHigherSP = Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())]) +
                                        Math.Abs(Convert.ToInt32(row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())]));

                            sumOfHigherPremiums += cutOffHigherSP;
                            countOfHigherPremiums++;
                        }

                        if (!dataTableColumnsList.Contains(cutOffHigherSP))
                            dataTableColumnsList.Add(cutOffHigherSP);
                    }
                }
            }

            //Central SP will now be closest to market price
            int centralSP = OCHelper.RoundTo100(MySession.Current.RecordsObject.underlyingValue);
            dataTableColumnsList.Add(centralSP);

            if (countOfHigherPremiums >= 1)
                avgOfHigherPremiums = RoundToNumber(sumOfHigherPremiums / countOfHigherPremiums, 1);

            if (countOfLowerPremiums >= 1)
                avgOfLowerPremiums = RoundToNumber(sumOfLowerPremiums / countOfLowerPremiums, 1);

            //Add SP, rounded to 100, lower than minimum SP
            int minVal = dataTableColumnsList.Min();

            if (avgOfLowerPremiums > 0 && !dataTableColumnsList.Contains(avgOfLowerPremiums))
                dataTableColumnsList.Add(avgOfLowerPremiums);

            int maxVal = dataTableColumnsList.Max();

            if (avgOfHigherPremiums > 0 && !dataTableColumnsList.Contains(avgOfHigherPremiums))
                dataTableColumnsList.Add(avgOfHigherPremiums);

            minVal = dataTableColumnsList.Min();
            if (OCHelper.RoundTo100(minVal) > minVal)
            {
                if (!dataTableColumnsList.Contains(OCHelper.RoundTo100(minVal) - 100))
                    dataTableColumnsList.Add(OCHelper.RoundTo100(minVal) - 100);
            }
            else if (OCHelper.RoundTo100(minVal) == minVal)
            {
                if (!dataTableColumnsList.Contains(OCHelper.RoundTo100(minVal) - 100))
                    dataTableColumnsList.Add(OCHelper.RoundTo100(minVal) - 100);
            }
            else
            {
                if (!dataTableColumnsList.Contains(OCHelper.RoundTo100(minVal)))
                    dataTableColumnsList.Add(OCHelper.RoundTo100(minVal));
            }

            //Add SP, rounded to 100, higher than maximum SP
            maxVal = dataTableColumnsList.Max();
            if (OCHelper.RoundTo100(maxVal) < maxVal)
            {
                if (!dataTableColumnsList.Contains(OCHelper.RoundTo100(maxVal) + 100))
                    dataTableColumnsList.Add(OCHelper.RoundTo100(maxVal) + 100);
            }
            else if (OCHelper.RoundTo100(maxVal) == maxVal)
            {
                if (!dataTableColumnsList.Contains(OCHelper.RoundTo100(maxVal) + 100))
                    dataTableColumnsList.Add(OCHelper.RoundTo100(maxVal) + 100);
            }
            else
            {
                if (!dataTableColumnsList.Contains(OCHelper.RoundTo100(maxVal)))
                    dataTableColumnsList.Add(OCHelper.RoundTo100(maxVal));
            }

            //Add columns in multiples of 100 between current Min and Max values.
            minVal = dataTableColumnsList.Min();
            maxVal = dataTableColumnsList.Max();
            for (int index = minVal; index < maxVal; index += 100)
            {
                if (!dataTableColumnsList.Contains(index))
                    dataTableColumnsList.Add(index);
            }

            int currentMarketStrikePrice = RoundToNumber(MySession.Current.RecordsObject.underlyingValue, 1);

            if (currentMarketStrikePrice > 0 && !dataTableColumnsList.Contains(currentMarketStrikePrice))
                dataTableColumnsList.Add(currentMarketStrikePrice);

            dataTableColumnsList.Sort();

            dataTableCalc.Columns.Add(enumSBColumns.Id.ToString());
            dataTableCalc.Columns.Add(enumSBColumns.CMP.ToString());

            foreach (int dataListItem in dataTableColumnsList)
            {
                if (!dataTableCalc.Columns.Contains(dataListItem.ToString()))
                {
                    dataTableCalc.Columns.Add(dataListItem.ToString());
                }
            }

            int lotsSize = 0;
            if (rblOCType.SelectedValue.Equals(enumOCType.NIFTY.ToString()))
                lotsSize = Constants.NIFTY_LOT_SIZE;
            if (rblOCType.SelectedValue.Equals(enumOCType.BANKNIFTY.ToString()))
                lotsSize = Constants.BANKNIFTY_LOT_SIZE;

            foreach (DataRow row in dataTable.Rows)
            {
                DataRow dataRowCalc = dataTableCalc.NewRow();
                dataRowCalc[dataRowCalc.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())] = row[row.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())];
                dataRowCalc[dataRowCalc.Table.Columns.IndexOf(enumSBColumns.Id.ToString())] = row[row.Table.Columns.IndexOf(enumSBColumns.Id.ToString())];
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
            dataTableForColumns = dataTable.Copy();
            gvStrategy.DataSource = dataTableCalc;
            gvStrategy.DataBind();
        }

        protected void gvStrategy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int currentMarketStrikePrice = RoundToNumber(MySession.Current.RecordsObject.underlyingValue, 1);
            int currentMarketStrikePriceColumnIndex = OCHelper.GetColumnIndexByName(e.Row, currentMarketStrikePrice.ToString());

            e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumSBColumns.Id.ToString())].Style.Add("display", "none");
            int enumSBColumnsCount = Enum.GetNames(typeof(enumSBColumns)).Length;
            DataTable dataTableForRows = gvStrategy.DataSource as DataTable;

            //Logic for nomal rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                DropDownList ddlExpiryDates = e.Row.FindControl("ddlExpiryDates") as DropDownList;
                DropDownList ddlContractType = e.Row.FindControl("ddlContractType") as DropDownList;
                DropDownList ddlTransactionType = e.Row.FindControl("ddlTransactionType") as DropDownList;
                DropDownList ddlStrikePrice = e.Row.FindControl("ddlStrikePrice") as DropDownList;
                DropDownList ddlLots = e.Row.FindControl("ddlLots") as DropDownList;
                DropDownList ddlStrategyList = e.Row.FindControl("ddlStrategyList") as DropDownList;

                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumSBColumns.CMP.ToString())].Text = dataTableForColumns.Rows[e.Row.RowIndex][dataTableForColumns.Columns.IndexOf(enumSBColumns.CMP.ToString())].ToString();
                chkSelect.Checked = Convert.ToBoolean(dataTableForColumns.Rows[e.Row.RowIndex][dataTableForColumns.Columns.IndexOf(enumSBColumns.Select.ToString())]);

                OCHelper.FillAllExpiryDates(rblOCType.SelectedValue, ddlExpiryDates, false);
                OCHelper.FillAllStrikePrice(rblOCType.SelectedValue, ddlStrikePrice, false);

                Utility.SelectDataInCombo(ddlExpiryDates, dataTableForColumns.Rows[e.Row.RowIndex][dataTableForColumns.Columns.IndexOf(enumSBColumns.ExpiryDate.ToString())].ToString());
                Utility.SelectDataInCombo(ddlContractType, dataTableForColumns.Rows[e.Row.RowIndex][dataTableForColumns.Columns.IndexOf(enumSBColumns.ContractType.ToString())].ToString());
                Utility.SelectDataInCombo(ddlTransactionType, dataTableForColumns.Rows[e.Row.RowIndex][dataTableForColumns.Columns.IndexOf(enumSBColumns.TransactionType.ToString())].ToString());
                Utility.SelectDataInCombo(ddlStrikePrice, dataTableForColumns.Rows[e.Row.RowIndex][dataTableForColumns.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())].ToString());
                Utility.SelectDataInCombo(ddlLots, dataTableForColumns.Rows[e.Row.RowIndex][dataTableForColumns.Columns.IndexOf(enumSBColumns.Lots.ToString())].ToString());
                Utility.SelectDataInCombo(ddlStrategyList, dataTableForColumns.Rows[e.Row.RowIndex][dataTableForColumns.Columns.IndexOf(enumSBColumns.StrategyName.ToString())].ToString());

                for (int icount = enumSBColumnsCount - 2; icount < e.Row.Cells.Count; icount++)
                {
                    e.Row.Cells[icount].HorizontalAlign = HorizontalAlign.Right;
                }

                if (dataTableForColumns.Rows[e.Row.RowIndex][enumSBColumns.DataSource.ToString()].ToString().Equals(enumDataSource.Positions.ToString()))
                {
                    ddlExpiryDates.Enabled = false;
                    ddlContractType.Enabled = false;
                    ddlTransactionType.Enabled = false;
                    ddlStrikePrice.Enabled = false;
                    ddlLots.Enabled = false;
                }

                //DO NOT DELETE
                //Add attributes
                //int rowindex = e.Row.RowIndex + 1;
                //ddlContractType.Attributes.Add("onchange", "CalcExpValForAllCellsInRow('" + gvStrategy.ClientID + "', '" +
                //    rowindex + "','" + rblOCType.ClientID + "');");
                //ddlTransactionType.Attributes.Add("onchange", "CalcExpValForAllCellsInRow('" + gvStrategy.ClientID + "', '" +
                //    rowindex + "','" + rblOCType.ClientID + "');");
                //ddlStrikePrice.Attributes.Add("onchange", "SetGridviewHeaderValues('"
                //    + gvStrategy.ClientID + "'), CalcExpValForAllCellsInRow('" + gvStrategy.ClientID + "', '" + rowindex + "','"
                //    + rblOCType.ClientID + "');");
            }


            GridView gridView = sender as GridView;

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                double sum = 0;
                for (int iCellCount = enumSBColumnsCount - 1; iCellCount < e.Row.Cells.Count; iCellCount++)
                {
                    sum = 0;
                    for (int irowcount = 0; irowcount < gridView.Rows.Count; irowcount++)
                    {
                        sum += Convert.ToDouble(gridView.Rows[irowcount].Cells[iCellCount].Text);
                    }
                    e.Row.Cells[iCellCount].Text = sum.ToString();
                    e.Row.Cells[iCellCount].HorizontalAlign = HorizontalAlign.Right;
                }
            }

            if (currentMarketStrikePriceColumnIndex < e.Row.Cells.Count)
                e.Row.Cells[currentMarketStrikePriceColumnIndex].BackColor = Color.LightGray;
        }

        protected void btnAddRows_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            AddBlankRows(dataTable, Constants.SB_ADD_ROW_COUNT);
            MySession.Current.StrategyBuilderDt = dataTable;
            DataTable filteredDataTable = FilterRecords();
            GridviewDataBind(filteredDataTable);
        }

        private DataTable SaveGridviewData()
        {
            DataTable dataTable = MySession.Current.StrategyBuilderDt;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                foreach (GridViewRow gvRow in gvStrategy.Rows)
                {
                    if (dataRow[enumSBColumns.Id.ToString()].ToString().Equals(gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumSBColumns.Id.ToString())].Text))
                    {
                        CheckBox chkSelect = gvRow.FindControl("chkSelect") as CheckBox;
                        DropDownList ddlExpiryDates = gvRow.FindControl("ddlExpiryDates") as DropDownList;
                        DropDownList ddlContractType = gvRow.FindControl("ddlContractType") as DropDownList;
                        DropDownList ddlTransactionType = gvRow.FindControl("ddlTransactionType") as DropDownList;
                        DropDownList ddlStrikePrice = gvRow.FindControl("ddlStrikePrice") as DropDownList;
                        DropDownList ddlLots = gvRow.FindControl("ddlLots") as DropDownList;
                        DropDownList ddlStrategyList = gvRow.FindControl("ddlStrategyList") as DropDownList;

                        //string strategyName = Constants.ALL;
                        //if (!ddlStrategyListFilter.SelectedValue.Equals(Constants.ALL))
                        //    strategyName = ddlStrategyListFilter.SelectedValue;
                        //else
                        //    strategyName = "Default";

                        //dataTable.Rows.Add(chkSelect.Checked, ddlExpiryDates.SelectedValue, ddlContractType.SelectedValue, ddlTransactionType.SelectedValue, ddlStrikePrice.SelectedValue,
                        //    ddlLots.SelectedValue, MySession.Current.StrategyBuilderDt.Rows[gvRow.RowIndex][enumSBColumns.DataSource.ToString()].ToString(),
                        //    MySession.Current.StrategyBuilderDt.Rows[gvRow.RowIndex][enumSBColumns.CMP.ToString()].ToString(), ddlStrategyList.SelectedValue);

                        //dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.CMP.ToString())] = MySession.Current.StrategyBuilderDt.Rows[gvRow.RowIndex][enumSBColumns.CMP.ToString()].ToString();
                        dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.Select.ToString())] = chkSelect.Checked;
                        dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.ExpiryDate.ToString())] = ddlExpiryDates.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.ContractType.ToString())] = ddlContractType.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.TransactionType.ToString())] = ddlTransactionType.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.StrikePrice.ToString())] = ddlStrikePrice.SelectedValue;
                        dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.Lots.ToString())] = ddlLots.SelectedValue;
                        //dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.DataSource.ToString())] = MySession.Current.StrategyBuilderDt.Rows[gvRow.RowIndex][enumSBColumns.DataSource.ToString()].ToString();
                        dataRow[dataRow.Table.Columns.IndexOf(enumSBColumns.StrategyName.ToString())] = ddlStrategyList.SelectedValue;
                    }
                }
            }
            return dataTable;
        }

        protected void btnUpdateCMP_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            MySession.Current.StrategyBuilderDt = dataTable;
            DataTable filteredDataTable = FilterRecords();
            GridviewDataBind(filteredDataTable);
            //GridviewDataBind(dataTable);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dataTable = SaveGridviewData();
            FileClass.WriteDataTable(dataTable, Constants.MYFILES_FOLDER_PATH + Constants.SB_FILE_NAME);
            MySession.Current.StrategyBuilderDt = dataTable;
            DataTable filteredDataTable = FilterRecords();
            GridviewDataBind(filteredDataTable);
            //GridviewDataBind(dataTable);
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
            DataTable filteredDataTable = FilterRecords();
            GridviewDataBind(filteredDataTable);
            //GridviewDataBind(newDataTable);
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
            DataRow[] drs = dataTable.Select(selectQuery, enumSBColumns.StrategyName.ToString() + " ASC," + enumSBColumns.ExpiryDate.ToString() + " DESC, " + enumSBColumns.StrikePrice.ToString() + " ASC");
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

            if (!ddlFilterExpiryDates.SelectedValue.Equals(Constants.ALL))
                filters.Add(" (" + enumSBColumns.ExpiryDate.ToString() + " = #" + ddlFilterExpiryDates.SelectedValue + "#) ");

            if (!ddlStrategyListFilter.SelectedValue.Equals(Constants.ALL))
                filters.Add(" (" + enumSBColumns.StrategyName.ToString() + " = '" + ddlStrategyListFilter.SelectedValue + "') ");

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

        public static int RoundToNumber(double value, int number)
        {
            int result = (int)Math.Round(value / number);
            if (value > 0 && result == 0)
            {
                result = 1;
            }
            return (int)result * number;
        }

        protected void btnAddToOrderBasket_Click(object sender, EventArgs e)
        {

        }
    }
}