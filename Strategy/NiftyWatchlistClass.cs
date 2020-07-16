using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace TOC.Strategy
{
    public class NiftyWatchlistClass
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
            DataTable filteredOptionChainDataTable = OCHelper.FilterOptionChainDataTable(filterConditions);
            //DataTable filteredOptionChainDataTable = OCHelper.toDataTable(MySession.Current.RecordsObject);
            //DataTable filteredDataTable = OCHelper.FilterDataTableRecords(filterConditions);

            return filteredOptionChainDataTable;
        }
    }

    enum enumNWLColumns
    {
        SelectCE,
        CETradingSymbol,
        CEopenInterest,
        CEchangeinOpenInterest,
        CEtotalTradedVolume,
        CEimpliedVolatility,
        CElastPrice,
        CEchange,
        strikePrice,
        ExpiryDate,
        PEchange,
        PElastPrice,
        PEimpliedVolatility,
        PEtotalTradedVolume,
        PEchangeinOpenInterest,
        PEopenInterest,
        PETradingSymbol,
        SelectPE
    }
}