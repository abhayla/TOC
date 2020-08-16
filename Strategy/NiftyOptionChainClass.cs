using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace TOC.Strategy
{
    public class NiftyOptionChainClass
    {
        public static DataTable AddFilterValues(FilterConditions filterConditions)
        {
            if (MySession.Current.GlobalFilterConditions == null)
                OCHelper.GetGlobalFilterConditions();
            if (MySession.Current.GlobalFilterConditions.Hide1SDSP)
                filterConditions.PercentageRange = 10;
            if (MySession.Current.GlobalFilterConditions.Hide2SDSP)
                filterConditions.PercentageRange = 15;

            //SP should be above the higher limit and lower than the lower limit
            filterConditions.ShowSPBetweenLowHigh = false;
            DataTable filteredOptionChainDataTable = FilterOptionChainDataTable(filterConditions);
            //DataTable filteredOptionChainDataTable = OCHelper.toDataTable(MySession.Current.RecordsObject);
            //DataTable filteredDataTable = OCHelper.FilterDataTableRecords(filterConditions);

            return filteredOptionChainDataTable;
        }

        public static DataTable FilterOptionChainDataTable(FilterConditions filterConditions)
        {
            DataTable allRecordsDt = OCHelper.toDataTable(MySession.Current.RecordsObject);

            string strFilter = GetFilterString(filterConditions);

            DataRow[] drs = allRecordsDt.Select(strFilter, enumOCColumns.StrikePrice + " ASC");
            DataTable filteredDataTable = allRecordsDt.Clone();
            foreach (DataRow dr in drs)
            {
                filteredDataTable.ImportRow(dr);
            }

            return filteredDataTable;
        }

        public static string GetFilterString(FilterConditions filterConditions)
        {
            int iUpperStrikePriceRange = 0;
            int iLowerStrikePriceRange = 0;

            if (filterConditions.PercentageRange > 0)
            {
                iUpperStrikePriceRange = OCHelper.RoundTo100(MySession.Current.RecordsObject.underlyingValue + (OCHelper.DefaultSP(filterConditions.OCType) * filterConditions.PercentageRange / 100));
                iLowerStrikePriceRange = OCHelper.RoundTo100(MySession.Current.RecordsObject.underlyingValue - (OCHelper.DefaultSP(filterConditions.OCType) * filterConditions.PercentageRange / 100));
            }
            else
            {
                iUpperStrikePriceRange = filterConditions.SPHigherRange;
                iLowerStrikePriceRange = filterConditions.SPLowerRange;
            }

            string strFilter = string.Empty;
            List<string> filters = new List<string>();

            if (filterConditions.ExpiryDate != null && !filterConditions.ExpiryDate.ToUpper().Equals(Constants.ALL))
                filters.Add(" (" + enumOCColumns.ExpiryDate + " = #" + filterConditions.ExpiryDate + "#) ");

            if (filterConditions.ShowSPBetweenLowHigh)
            {
                filters.Add(" ((" + enumOCColumns.StrikePrice + " >= " + iLowerStrikePriceRange + ") AND (" + enumOCColumns.StrikePrice + " <= " + iUpperStrikePriceRange + ")) ");
            }
            else
            {
                filters.Add(" ((" + enumOCColumns.StrikePrice + " <= " + iLowerStrikePriceRange + ") OR (" + enumOCColumns.StrikePrice + " >= " + iUpperStrikePriceRange + ")) ");
            }

            if (filterConditions.ContractType != null && !filterConditions.ContractType.ToUpper().Equals(Constants.ALL))
                filters.Add(" (" + enumOCColumns.ContractType + " = '" + filterConditions.ContractType + "') ");

            if (filterConditions.ImpliedVolatility != null && !filterConditions.ImpliedVolatility.ToUpper().Equals("ALL"))
                filters.Add(" ((" + enumOCColumns.CEimpliedVolatility + " >= '" + filterConditions.ImpliedVolatility + "') OR (" + enumOCColumns.PEimpliedVolatility + " >= '" + filterConditions.ImpliedVolatility + "')) ");

            if (filterConditions.LTP != null && !filterConditions.LTP.ToUpper().Equals("ALL"))
                filters.Add(" ((" + enumOCColumns.CElastPrice + " >= '" + filterConditions.LTP + "') OR (" + enumOCColumns.PElastPrice + " >= '" + filterConditions.LTP + "')) ");
            
            if (filterConditions.ExtrinsicValuePer != null && !filterConditions.ExtrinsicValuePer.ToUpper().Equals("ALL"))
                filters.Add(" ((" + enumOCColumns.CEExtrinsicValuePer + " >= '" + filterConditions.ExtrinsicValuePer + "') OR (" + enumOCColumns.PEExtrinsicValuePer + " >= '" + filterConditions.ExtrinsicValuePer + "')) ");

            if (filterConditions.ExtValPerDay != null && !filterConditions.ExtValPerDay.ToUpper().Equals(Constants.ALL))
                filters.Add(" ((" + enumOCColumns.CEExtrinsicValuePerDay + " >= '" + filterConditions.ExtValPerDay + "') OR (" + enumOCColumns.PEExtrinsicValuePerDay + " >= '" + filterConditions.ExtValPerDay + "')) ");

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
    }

    //enum enumNWLColumns
    //{
    //    SelectCE,
    //    CETradingSymbol,
    //    CEopenInterest,
    //    CEchangeinOpenInterest,
    //    CEtotalTradedVolume,
    //    CEimpliedVolatility,
    //    CElastPrice,
    //    CEchange,
    //    StrikePrice,
    //    ExpiryDate,
    //    PEchange,
    //    PElastPrice,
    //    PEimpliedVolatility,
    //    PEtotalTradedVolume,
    //    PEchangeinOpenInterest,
    //    PEopenInterest,
    //    PETradingSymbol,
    //    SelectPE,
    //    DaysToExpiry,
    //    IntrinsicValue,
    //    IntrinsicValuePer,
    //    SD,
    //    CEDelta,
    //    PEDelta
    //}
}