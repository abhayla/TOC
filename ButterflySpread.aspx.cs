using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TOC.Strategy;

namespace TOC
{
    public partial class ButterflySpread : System.Web.UI.Page
    {
        private static double iPercentageRage = 5;
        TimeSpan timeAddGap = new TimeSpan(0, 3, 0);

        protected void Page_Load(object sender, EventArgs e)
        {
            lblStart.Text = DateTime.Now.ToString();
            if (!IsPostBack)
            {
                FillExpiryDates(ddlExpiryDates);

                FilterConditions filterConditions = new FilterConditions();
                //filterConditions.SPLowerRange = MySession.Current.RecordsObject.strikePrices.Min();
                //filterConditions.SPHigherRange = MySession.Current.RecordsObject.strikePrices.Max();
                //filterConditions.SPExpiry = Convert.ToInt32(ddlSPExpiry.SelectedValue);
                filterConditions.ContractType = ddlContractType.SelectedValue;
                filterConditions.OcType = rblOCType.SelectedValue;
                filterConditions.StrategyType = enumStrategyType.BUTTERFLY.ToString();
                filterConditions.TimeGap = timeAddGap;
                filterConditions.ExpiryDate = ddlExpiryDates.SelectedValue;
                filterConditions.PercentageRange = iPercentageRage;
                filterConditions.SPDifference = 100;

                DataSet dataSetResult = Butterfly.GetButterflySpreadStrategies(filterConditions);
                PopulateDataSet(dataSetResult);

                PopulateFilterFields(filterConditions);
                Page.LoadComplete += new EventHandler(Page_LoadComplete);
            }
        }

        private void Page_LoadComplete(object sender, EventArgs e)
        {
            lblEnd.Text = DateTime.Now.ToString();
            //throw new NotImplementedException();
        }

        private void PopulateFilterFields(FilterConditions filterConditions)
        {
            lblLastFetchedTime.Text = MySession.Current.RecordsObject.timestamp;
            lblLastPrice.Text = MySession.Current.RecordsObject.underlyingValue.ToString();

            PopulateSPLowerRange(filterConditions);
            PopulateSPHigherRange(filterConditions);
            PopulateSPExpiry(filterConditions);
        }

        private void PopulateSPExpiry(FilterConditions filterConditions)
        {
            int iUpperStrikePriceRange = OCHelper.RoundTo100(MySession.Current.RecordsObject.underlyingValue + (OCHelper.DefaultSP(rblOCType.SelectedValue) * iPercentageRage / 100));
            int iLowerStrikePriceRange = OCHelper.RoundTo100(MySession.Current.RecordsObject.underlyingValue - (OCHelper.DefaultSP(rblOCType.SelectedValue) * iPercentageRage / 100));

            //List<int> strikePrices = MySession.Current.RecordsObject.strikePrices;
            ddlSPExpiry.Items.Clear();

            ddlSPExpiry.Items.Add("NONE");

            foreach (var item in MySession.Current.RecordsObject.strikePrices)
            {
                if (item < iUpperStrikePriceRange && item > iLowerStrikePriceRange && item % filterConditions.SPDifference == 0 &&
                    item < filterConditions.SPHigherRange && item > filterConditions.SPLowerRange)
                {
                    ddlSPExpiry.Items.Add(item.ToString());
                }
            }

            if (filterConditions.SPExpiry != 0)
                Utility.SelectDataInCombo(ddlSPExpiry, filterConditions.SPExpiry.ToString());
        }

        private void PopulateSPHigherRange(FilterConditions filterConditions)
        {
            int iUpperStrikePriceRange = OCHelper.RoundTo100(MySession.Current.RecordsObject.underlyingValue + (OCHelper.DefaultSP(rblOCType.SelectedValue) * iPercentageRage / 100));

            int iLowerStrikePrice = iUpperStrikePriceRange;

            ddlSPHigherRange.Items.Clear();

            foreach (var item in MySession.Current.RecordsObject.strikePrices)
            {
                if (item <= iUpperStrikePriceRange && item > MySession.Current.RecordsObject.underlyingValue && item % filterConditions.SPDifference == 0 &&
                    item > filterConditions.SPLowerRange + filterConditions.SPDifference)
                {
                    ddlSPHigherRange.Items.Add(item.ToString());

                    //find the Min strike price which meets this criteria to reduce filter size
                    iLowerStrikePrice = Math.Min(iLowerStrikePrice, item);
                }
            }

            //Select the Min strike price in the ddlb
            if (filterConditions.SPHigherRange == 0)
            {
                filterConditions.SPHigherRange = iLowerStrikePrice;
                //Utility.SelectDataInCombo(ddlSPHigherRange, iLowerStrikePrice.ToString());
            }

            Utility.SelectDataInCombo(ddlSPHigherRange, filterConditions.SPHigherRange.ToString());

        }

        private void PopulateSPLowerRange(FilterConditions filterConditions)
        {
            int iLowerStrikePriceRange = OCHelper.RoundTo100(MySession.Current.RecordsObject.underlyingValue - (OCHelper.DefaultSP(rblOCType.SelectedValue) * iPercentageRage / 100));

            int iUpperStrikePrice = iLowerStrikePriceRange;

            ddlSPLowerRange.Items.Clear();

            foreach (int item in MySession.Current.RecordsObject.strikePrices)
            {
                if (item >= iLowerStrikePriceRange && item < MySession.Current.RecordsObject.underlyingValue && item % filterConditions.SPDifference == 0)
                {
                    ddlSPLowerRange.Items.Add(item.ToString());

                    //find the max strike price which meets this criteria to reduce filter size
                    iUpperStrikePrice = Math.Max(iUpperStrikePrice, item);
                }
            }

            //Select the Max strike price in the ddlb
            if (filterConditions.SPLowerRange == 0)
            {
                filterConditions.SPLowerRange = iUpperStrikePrice;
                //Utility.SelectDataInCombo(ddlSPLowerRange, iUpperStrikePrice.ToString());
            }
            Utility.SelectDataInCombo(ddlSPLowerRange, filterConditions.SPLowerRange.ToString());
        }

        private void FillExpiryDates(DropDownList ddlExpDt)
        {
            List<string> expiryDates;

            if (MySession.Current.RecordsObject == null)
                expiryDates = OCHelper.GetOCExpList(rblOCType.SelectedValue);
            else
                expiryDates = MySession.Current.RecordsObject.expiryDates;

            foreach (string item in expiryDates)
            {
                ddlExpDt.Items.Add(item);
            }
        }

        //protected void btnFetch_Click(object sender, EventArgs e)
        //{
        //    FilterConditions filterConditions = new FilterConditions();
        //    filterConditions.SPLowerRange = Convert.ToInt32(ddlSPLowerRange.SelectedValue);
        //    filterConditions.SPHigherRange = Convert.ToInt32(ddlSPHigherRange.SelectedValue);
        //    filterConditions.SPExpiry = Convert.ToInt32(ddlSPExpiry.SelectedValue);
        //    filterConditions.ContractType = ddlContractType.SelectedValue;
        //    filterConditions.OcType = rblOCType.SelectedValue;
        //    filterConditions.StrategyType = enumStrategyType.BUTTERFLY.ToString();
        //    filterConditions.TimeGap = timeAddGap;
        //    filterConditions.ExpiryDate = ddlExpiryDates.SelectedValue;
        //    filterConditions.PercentageRange = 0;

        //    DataSet dataSetResult = Butterfly.GetButterflySpreadStrategies(filterConditions);
        //    PopulateDataSet(dataSetResult);

        //    PopulateFilterFields();
        //}

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
                string clientId = ((System.Web.UI.Control)sender).ClientID;

                if (e.Row.RowIndex == 0)
                {
                    //Add Zerodha button
                    e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(new LiteralControl("<span><button type=\"button\" class=\"btn btn-success\" onclick=\"ZerodhaBasketOrder('" + clientId + "')\">Execute Orders</button></span>"));

                }
            }
        }

        protected void btnFilterResults_Click(object sender, EventArgs e)
        {
            FilterConditions filterConditions = new FilterConditions();
            filterConditions.SPLowerRange = Convert.ToInt32(ddlSPLowerRange.SelectedValue);
            filterConditions.SPHigherRange = Convert.ToInt32(ddlSPHigherRange.SelectedValue);

            if (ddlSPExpiry.SelectedValue.Equals("NONE"))
                filterConditions.SPExpiry = 0;
            else
                filterConditions.SPExpiry = Convert.ToInt32(ddlSPExpiry.SelectedValue);

            filterConditions.ContractType = ddlContractType.SelectedValue;
            filterConditions.OcType = rblOCType.SelectedValue;
            filterConditions.StrategyType = enumStrategyType.BUTTERFLY.ToString();
            filterConditions.TimeGap = timeAddGap;
            filterConditions.ExpiryDate = ddlExpiryDates.SelectedValue;
            filterConditions.PercentageRange = 0;
            filterConditions.SPDifference = 100;

            DataSet dataSetResult = Butterfly.GetButterflySpreadStrategies(filterConditions);
            PopulateDataSet(dataSetResult);

            PopulateFilterFields(filterConditions);

            //DataSet filteredDataSet = new DataSet();
            //foreach (DataTable dataTable in dataSetResult.Tables)
            //{
            //    //List<string> sp = dataTable.AsEnumerable().Select(x => x["Strike Price"].ToString()).ToList();

            //    //Find Lowest and Highest value in the given table column
            //    int iLowestSP = int.MaxValue;
            //    int iHighestSP = int.MinValue;
            //    int iMiddleSP = 0;
            //    foreach (DataRow dr in dataTable.Rows)
            //    {
            //        int currentSP = Convert.ToInt32(dr.Field<string>("StrikePrice"));
            //        iLowestSP = Math.Min(iLowestSP, currentSP);
            //        iMiddleSP = Math.Max(iLowestSP, currentSP);
            //        iMiddleSP = Math.Min(iHighestSP, currentSP);
            //        iHighestSP = Math.Max(iHighestSP, currentSP);
            //    }

            //    if (filterConditions.SPLowerRange == iLowestSP &&
            //        filterConditions.SPHigherRange == iHighestSP &&
            //        filterConditions.SPExpiry == iMiddleSP)
            //    {
            //        filteredDataSet.Tables.Add(dataTable.Copy());
            //    }
            //}
            ////dataSetResult = Butterfly.GetButterflySpreadStrategies(filterConditions);
            //PopulateDataSet(filteredDataSet);
        }
    }
}