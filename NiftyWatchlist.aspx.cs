using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using TOC.Strategy;
using System.Web.UI.WebControls;

namespace TOC
{
    public partial class NiftyWatchlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                OCHelper.FillAllExpiryDates("NIFTY", ddlExpiryDates, true);

                FilterConditions filterConditions = new FilterConditions();
                filterConditions.OcType = "NIFTY";

                DataTable dtData = NiftyWatchlistClass.AddFilterValues(filterConditions);
                gvNiftyWatchlist.DataSource = dtData;
                gvNiftyWatchlist.DataBind();

                btnBuyBasketOrder.Attributes.Add("onclick", "CreateBasketOfOrders('" + gvNiftyWatchlist.ClientID + "')");
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FilterConditions filterConditions = new FilterConditions();
            filterConditions.OcType = "NIFTY";
            filterConditions.ExpiryDate = ddlExpiryDates.SelectedValue;
            DataTable dtData = NiftyWatchlistClass.AddFilterValues(filterConditions);
            gvNiftyWatchlist.DataSource = dtData;
            gvNiftyWatchlist.DataBind();
        }

        protected void btnAddToStrategyBuilder_Click(object sender, EventArgs e)
        {
            DataTable dataSBTable = Strategy.StrategyBuilderClass.AddSBColumns();
            foreach (GridViewRow gvRow in gvNiftyWatchlist.Rows)
            {
                CheckBox checkBoxPE = (CheckBox)gvRow.FindControl("chkPE");
                CheckBox checkBoxCE = (CheckBox)gvRow.FindControl("chkCE");

                if (checkBoxCE.Checked)
                {
                    dataSBTable.Rows.Add(false, gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumNWLColumns.ExpiryDate.ToString())].Text, 
                        "CE", "SELL", gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumNWLColumns.strikePrice.ToString())].Text, 1, 0);
                }

                if (checkBoxPE.Checked)
                {
                    dataSBTable.Rows.Add(false, gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumNWLColumns.ExpiryDate.ToString())].Text,
                        "PE", "SELL", gvRow.Cells[OCHelper.GetColumnIndexByName(gvRow, enumNWLColumns.strikePrice.ToString())].Text, 1, 0);
                }
            }

            StrategyBuilderClass.AddSBRows(dataSBTable);
        }
    }
}