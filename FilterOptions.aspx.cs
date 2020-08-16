using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TOC.Strategy;

namespace TOC
{
    public partial class FilterOptions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OCHelper.FillAllExpiryDates(rblOCType.SelectedValue, ddlExpiryDates, true);

            FilterConditions filterConditions = new FilterConditions();
            filterConditions.ContractType = ddlContractType.SelectedValue;
            filterConditions.OCType = rblOCType.SelectedValue;
            filterConditions.StrategyType = string.Empty;
            //filterConditions.TimeGap = new TimeSpan(0, 3, 0);
            filterConditions.ExpiryDate = ddlExpiryDates.SelectedValue;
            filterConditions.PercentageRange = 20;
            filterConditions.SPDifference = 100;
            filterConditions.WeeksToExpiry = Convert.ToInt32(ddlWeeksToExpiry.SelectedValue);

            DataTable dtData = FilterOptionsClass.AddFilterValues(filterConditions);

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FilterConditions filterConditions = new FilterConditions();
            filterConditions.ContractType = ddlContractType.SelectedValue;
            filterConditions.OCType = rblOCType.SelectedValue;
            filterConditions.StrategyType = string.Empty;
            //filterConditions.TimeGap = new TimeSpan(0, 3, 0);
            filterConditions.ExpiryDate = ddlExpiryDates.SelectedValue;
            filterConditions.PercentageRange = 20;
            filterConditions.SPDifference = 100;
            filterConditions.WeeksToExpiry = Convert.ToInt32(ddlWeeksToExpiry.SelectedValue);

            //DataSet dataSetResult = FilterStrategies.FilterAllStrategies(filterConditions);
            //PopulateDataSet(dataSetResult);
            //PopulateFilterFields(filterConditions);
        }
    }
}