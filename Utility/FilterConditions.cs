using System;

namespace TOC
{
    class FilterConditions
    {
        public int SPExpiry { get; set; }
        public int SPLowerRange { get; set; }
        public int SPHigherRange { get; set; }
        public string ContractType { get; set; }
        public TimeSpan TimeGap { get; set; }
        //public double ProfitAmount { get; set; }
        public string OcType { get; set; }
        public string StrategyType { get; set; }
    }
}