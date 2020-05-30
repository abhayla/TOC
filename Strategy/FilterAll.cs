using System;
using System.Collections.Generic;
using System.Data;

namespace TOC.Strategy
{
    public class FilterAll
    {
        public static DataSet FilterAllStrategies(FilterConditions filterConditions)
        {
            DataSet InputDataSet = GetFilterAllStrategies(filterConditions);
            DataSet dataSetResult = new DataSet();
            bool isAddThisTable = true;

            foreach (DataTable dataTable in InputDataSet.Tables)
            {
                //Reset flag for new dataset
                isAddThisTable = true;
                for (int irowcount = 0; irowcount < dataTable.Rows.Count; irowcount++)
                {
                    if (Convert.ToInt32(dataTable.Rows[irowcount]["Profit/Loss"]) > 0)
                    {
                        isAddThisTable = false;
                    }
                }

                if(isAddThisTable)
                {
                    dataSetResult.Tables.Add(dataTable.Copy());
                }
            }

            
            return dataSetResult;
        }

        private static DataSet GetFilterAllStrategies(FilterConditions filterConditions)
        {
            DataSet dataSetResult = new DataSet();
            DataTable filteredDataTable = OCHelper.AddRecordsToDataTable(filterConditions);
            DataSet dataSetCEBF = new DataSet();
            DataSet dataSetPEBF = new DataSet();
            DataSet dataSetPEIC = new DataSet();

            dataSetCEBF = Butterfly.GetButterflySpreadStrategies(filteredDataTable, enumContractType.CE.ToString(), filterConditions);
            dataSetResult = OCHelper.MergeDataSets(dataSetResult, dataSetCEBF);

            dataSetPEBF = Butterfly.GetButterflySpreadStrategies(filteredDataTable, enumContractType.PE.ToString(), filterConditions);
            dataSetResult = OCHelper.MergeDataSets(dataSetResult, dataSetPEBF);

            dataSetPEIC = IronCondorClass.CalculateIronCondors(filteredDataTable, filterConditions);
            dataSetResult = OCHelper.MergeDataSets(dataSetResult, dataSetPEIC);

            return dataSetResult;
        }
    }
}