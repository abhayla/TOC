using System;
using System.Collections.Generic;
using System.Data;

namespace TOC.Strategy
{
    public class FilterOptionsClass
    {
        public static DataTable AddFilterValues(FilterConditions filterConditions)
        {
            DataTable filteredDataTable = OCHelper.FilterDataTableRecords(filterConditions);

            return filteredDataTable;
        }
    }
}