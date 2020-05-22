using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telegram.Bot.Requests;
using TOC.Strategy;

namespace TOC
{
    public partial class ButterflySpread : System.Web.UI.Page
    {
        private static int iPercentageRage = 3;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillExpiryDates(ddlExpiryDates);
                DataSet dataSet = Butterfly.GetButterflySpreadStrategies(rblOCType.SelectedValue, iPercentageRage, ddlExpiryDates.SelectedValue, ddlContractType.SelectedValue);
                PopulateDataSet(dataSet);
                lblLastFetchedTime.Text = MySession.Current.RecordsObject.timestamp;
                lblLastPrice.Text = MySession.Current.RecordsObject.underlyingValue.ToString();
            }
        }

        private void FillExpiryDates(DropDownList ddlExpDt)
        {
            List<string> expiryDates = OCHelper.GetOCExpList(rblOCType.SelectedValue);
            foreach (string item in expiryDates)
            {
                ddlExpDt.Items.Add(item);
            }
        }

        protected void btnFetch_Click(object sender, EventArgs e)
        {
            DataSet dataSet = Butterfly.GetButterflySpreadStrategies(rblOCType.SelectedValue, iPercentageRage, ddlExpiryDates.SelectedValue, ddlContractType.SelectedValue);
            PopulateDataSet(dataSet);
            lblLastFetchedTime.Text = MySession.Current.RecordsObject.timestamp;
            lblLastPrice.Text = MySession.Current.RecordsObject.underlyingValue.ToString();
        }

        private void PopulateDataSet(DataSet dataSet)
        {
            for (int iCount = 0; iCount < dataSet.Tables.Count; iCount++)
            {
                DataTable dataTable = dataSet.Tables[iCount];
                //AddBasketOrderColumn(dataTable);
                dataTable.Columns.Add("Basket Order");
                GridView gridView = new GridView();
                gridView.ID = "gv" + iCount;
                gridView.ClientIDMode = ClientIDMode.Static;
                gridView.RowDataBound += new GridViewRowEventHandler(gridView_RowDataBound);
                gridView.DataSource = dataTable;
                divMain.Controls.Add(gridView);
                divMain.Controls.Add(Page.ParseControl("<br />"));
            }
            Page.DataBind();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //StringBuilder cstext1 = new StringBuilder();
                //cstext1.Append(" ");
                //cstext1.Append("");
                //cstext1.Append("KiteConnect.ready(function() {");
                //cstext1.Append(" var kite = new KiteConnect(\"OptionsOfOptions\");");
                //cstext1.Append("kite.add({");
                //cstext1.Append("\"exchange\": \"NFO\",");
                //cstext1.Append("\"tradingsymbol\": \"" + "BANKNIFTY20JUN16000PE" + "\",");
                //cstext1.Append("\"quantity\": \"" + 20 + "\",");
                ////cstext1.Append("quantity: " + 20 + ",");
                //cstext1.Append("\"transaction_type\": \"" + "SELL" + "\",");
                //cstext1.Append("\"product\": \"NRML\",");
                //cstext1.Append("\"order_type\": \"MARKET\",");
                //cstext1.Append(" });");
                //cstext1.Append("kite.add({");
                //cstext1.Append("\"exchange\": \"NFO\",");
                //cstext1.Append("\"tradingsymbol\": \"" + "NIFTY20MAY9500PE" + "\",");
                //cstext1.Append("\"quantity\": \"" + 75 + "\",");
                //cstext1.Append("\"transaction_type\": \"" + "SELL" + "\",");
                //cstext1.Append("\"product\": \"NRML\",");
                ////cstext1.Append("\"trigger_price\": \"" + SLTrigger + "\",");
                //cstext1.Append("\"order_type\": \"MARKET\",");
                //cstext1.Append(" });");
                //cstext1.Append(" kite.renderButton(\"#default-button\");");
                //cstext1.Append("kite.link(\"#custom-button\");");
                //cstext1.Append(" });");
                //cstext1.Append("");
                //cstext1.Append("<span><button type=\"button\" class=\"btn btn-success\" onclick=\"ZerodhaBasketOrder()\">Execute Orders</button></span>");
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "", cstext1.ToString(), true);


                //Button btnZerodha = new Button();
                //btnZerodha.Text = "Place Zerodha Order";
                //btnZerodha.ID = "btnPlaceZerodhaOrder";
                //btnZerodha.Width = 200;
                //btnZerodha.Attributes.Add("onclick", "ZerodhaBasketOrder();");

                //Button btnUpstox = new Button();
                //btnUpstox.Text = "Place Upstox Order";
                //btnUpstox.ID = "btnPlaceUpstoxOrder";
                //btnUpstox.Width = 200;

                //Control control = new Control();
                //HtmlInputButton btnZerodhaCustom = new HtmlInputButton();
                //btnZerodhaCustom.Value = "Submit";
                string clientId = ((System.Web.UI.Control)sender).ClientID;
                //btnZerodhaCustom.Attributes.Add("onclick", "send('" + clientId + "')");

                //HtmlInputButton btnZerodhaCustom1 = new HtmlInputButton();
                //btnZerodhaCustom1.Value = "Zerodha";
                //btnZerodhaCustom1.ID = "custom-button";
                //btnZerodhaCustom1.Attributes.Add("onclick", "return  ZerodhaBasketOrder('" + clientId + "')");

                //LiteralControl literalControl = new LiteralControl("<button id=\"custom-button\">Buy the basket</button>");
                //LiteralControl literalControl = new LiteralControl("<a href=\"#\" id=\"buy-basket-stock\"><strong>Buy a basket of stocks</strong></a>");
                //LiteralControl literalControl = new LiteralControl("<span><button type=\"button\" class=\"btn btn-success\" onclick=\"ZerodhaBasketOrder()\">Execute Orders</button></span>");
                //LiteralControl literalControl = new LiteralControl();
                //literalControl.Text = cstext1.ToString();
                //HtmlGenericControl divControl = new HtmlGenericControl();
                //divControl.InnerHtml = cstext1.ToString();

                //Unit rowHeight = 0;
                if (e.Row.RowIndex == 0)
                {
                    //rowHeight = e.Row.Height;
                    //Add Zerodha button
                    //e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(btnZerodhaCustom);

                    //e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(Page.ParseControl("<p id=\"default-button\"> </p>"));
                    //e.Row.Cells[e.Row.Cells.Count - 1].Text = HttpUtility.HtmlDecode("<p id=\"default-button\"> </p>");
                }
                if (e.Row.RowIndex == 1)
                {
                    //e.Row.Height = 10;
                    //Add Zerodha button
                    //e.Row.Cells[e.Row.Cells.Count-1].Controls.Add(btnUpstox);
                    //e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(new LiteralControl("<p id=\"default-button\"> </p>"));
                    //e.Row.Cells[e.Row.Cells.Count - 1].Text = HttpUtility.HtmlDecode("<p id=\"default-button\"> </p>");
                }
                if (e.Row.RowIndex == 2)
                {
                    //Add Zerodha button
                    //e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(btnZerodhaCustom1);
                    //e.Row.Cells[e.Row.Cells.Count - 1].Text = HttpUtility.HtmlDecode("<button id=\"custom-button\">Buy the basket</button>");

                    //e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(new LiteralControl("<span><button type=\"button\" class=\"btn btn-success\" onclick=\"ZerodhaBasketOrder('gv0')\">Execute Orders</button></span>"));
                    e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(new LiteralControl("<span><button type=\"button\" class=\"btn btn-success\" onclick=\"ZerodhaBasketOrder('" + clientId + "')\">Execute Orders</button></span>"));

                    //e.Row.Cells[e.Row.Cells.Count - 1].Text = HttpUtility.HtmlDecode("<button id=\"custom-button\">Buy the basket</button>");
                    //e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(new LiteralControl("<button id=\"custom-button\">Buy the basket</button>"));
                    //e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(divControl);
                    //e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(literalControl);
                }
            }
        }
    }
}