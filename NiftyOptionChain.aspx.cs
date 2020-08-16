using System;
using System.Web.UI;
using System.Data;
using TOC.Strategy;
using System.Web.UI.WebControls;
using System.Drawing;

namespace TOC
{
    public partial class NiftyOptionChain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                OCHelper.FillAllExpiryDates(enumOCType.NIFTY.ToString(), ddlExpiryDates, true);
                Utility.SelectDataInCombo(ddlExpiryDates, OCHelper.DefaultExpDate(enumOCType.NIFTY.ToString()));

                FilterConditions filterConditions = new FilterConditions();
                filterConditions.OCType = enumOCType.NIFTY.ToString();
                filterConditions.PercentageRange = 0;
                filterConditions.ExpiryDate = ddlExpiryDates.SelectedValue;
                DataTable dtData = NiftyOptionChainClass.AddFilterValues(filterConditions);
                gvNiftyWatchlist.DataSource = dtData;
                gvNiftyWatchlist.DataBind();

                btnBuyBasketOrder.Attributes.Add("onclick", "CreateBasketOfOrdersFromNiftyWatchlist('" + gvNiftyWatchlist.ClientID + "')");

                lblLastPrice.Text = MySession.Current.RecordsObject.underlyingValue.ToString();
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FilterConditions filterConditions = new FilterConditions();
            filterConditions.OCType = enumOCType.NIFTY.ToString();
            filterConditions.ExpiryDate = ddlExpiryDates.SelectedValue;
            filterConditions.ContractType = ddlContractType.SelectedValue;
            filterConditions.ExtrinsicValuePer = ddlExtrinsicValuePer.SelectedValue;
            filterConditions.ImpliedVolatility = ddlImpliedVolatility.SelectedValue;
            filterConditions.LTP = ddlLTP.SelectedValue;
            filterConditions.ExtValPerDay = ddlExtValPerDay.SelectedValue;
            filterConditions.PercentageRange = 0;
            DataTable dtData = NiftyOptionChainClass.AddFilterValues(filterConditions);
            gvNiftyWatchlist.DataSource = dtData;
            gvNiftyWatchlist.DataBind();
        }

        protected void gvNiftyWatchlist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double currentMktPrice = MySession.Current.RecordsObject.underlyingValue;
                int currentStrikePrice = Convert.ToInt32(e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.StrikePrice.ToString())].Text);

                if (currentStrikePrice <= currentMktPrice)
                {
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CETradingSymbol.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CEopenInterest.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CEchangeinOpenInterest.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CEtotalTradedVolume.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CEimpliedVolatility.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CElastPrice.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CEchange.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CEDelta.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CEExtrinsicValue.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CEExtrinsicValuePer.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.CEExtrinsicValuePerDay.ToString())].BackColor = Color.LightYellow;
                }

                if (currentStrikePrice >= currentMktPrice)
                {
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PETradingSymbol.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PEopenInterest.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PEchangeinOpenInterest.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PEtotalTradedVolume.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PEimpliedVolatility.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PElastPrice.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PEchange.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PEDelta.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PEExtrinsicValue.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PEExtrinsicValuePer.ToString())].BackColor = Color.LightYellow;
                    e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.PEExtrinsicValuePerDay.ToString())].BackColor = Color.LightYellow;
                }

                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.StrikePrice.ToString())].BackColor = Color.LightGray;
                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.ExpiryDate.ToString())].BackColor = Color.LightGray;
                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.DaysToExpiry.ToString())].BackColor = Color.LightGray;
                e.Row.Cells[OCHelper.GetColumnIndexByName(e.Row, enumOCColumns.SD.ToString())].BackColor = Color.LightGray;
            }
        }

        protected void btnAddToStrategyBuilder_Click(object sender, EventArgs e)
        {
            DataTable dataSBTable = StrategyBuilderClass.AddSBColumns();
            foreach (GridViewRow gvRow in gvNiftyWatchlist.Rows)
            {
                CheckBox checkBoxPE = (CheckBox)gvRow.FindControl("chkPE");
                CheckBox checkBoxCE = (CheckBox)gvRow.FindControl("chkCE");

                if (checkBoxCE.Checked)
                {
                    dataSBTable.Rows.Add(false, gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.ExpiryDate.ToString())].Text,
                        enumContractType.CE.ToString(), enumTransactionType.SELL.ToString(), gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.StrikePrice.ToString())].Text,
                        1, enumDataSource.NiftyWatchlist.ToString(), 0, enumDataSource.NiftyWatchlist.ToString(), Guid.NewGuid());
                }

                if (checkBoxPE.Checked)
                {
                    dataSBTable.Rows.Add(false, gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.ExpiryDate.ToString())].Text,
                        enumContractType.PE.ToString(), enumTransactionType.SELL.ToString(), gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.StrikePrice.ToString())].Text,
                        1, enumDataSource.NiftyWatchlist.ToString(), 0, enumDataSource.NiftyWatchlist.ToString(), Guid.NewGuid());
                }
            }

            StrategyBuilderClass.AddSBRows(dataSBTable);
        }

        protected void btnAddToPositions_Click(object sender, EventArgs e)
        {
            DataTable dataPTTable = PositionsClass.AddPTColumns();
            foreach (GridViewRow gvRow in gvNiftyWatchlist.Rows)
            {
                CheckBox checkBoxPE = (CheckBox)gvRow.FindControl("chkPE");
                CheckBox checkBoxCE = (CheckBox)gvRow.FindControl("chkCE");

                if (checkBoxCE.Checked)
                {
                    dataPTTable.Rows.Add(false, enumOCType.NIFTY.ToString(), gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.ExpiryDate.ToString())].Text,
                        enumContractType.CE.ToString(), enumTransactionType.SELL.ToString(), gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.StrikePrice.ToString())].Text,
                        1, Convert.ToDouble(gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.CElastPrice.ToString())].Text), 0,
                        Convert.ToDouble(gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.CElastPrice.ToString())].Text),
                        0, 0, 0, 0, string.Empty, enumStrategyType.SHORT_STRADDLE.ToString(), "Abhay", "Open", Guid.NewGuid(), 0, 0, DateTime.Now.Date);
                }

                if (checkBoxPE.Checked)
                {
                    dataPTTable.Rows.Add(false, enumOCType.NIFTY.ToString(), gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.ExpiryDate.ToString())].Text,
                        enumContractType.PE.ToString(), enumTransactionType.SELL.ToString(), gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.StrikePrice.ToString())].Text,
                        1, Convert.ToDouble(gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.PElastPrice.ToString())].Text), 0,
                        Convert.ToDouble(gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumOCColumns.PElastPrice.ToString())].Text),
                        0, 0, 0, 0, string.Empty, enumStrategyType.SHORT_STRADDLE.ToString(), "Abhay", "Open", Guid.NewGuid(), 0, 0, DateTime.Now.Date);
                }
            }

            PositionsClass.AddPTRows(dataPTTable);
        }
    }
}