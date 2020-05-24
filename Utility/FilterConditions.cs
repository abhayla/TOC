using System;

namespace TOC
{
    public class FilterConditions
    {
        public int SPExpiry { get; set; }
        public int SPDifference { get; set; }
        public int SPLowerRange { get; set; }
        public int SPHigherRange { get; set; }
        public string ContractType { get; set; }
        public TimeSpan TimeGap { get; set; }
        public double PercentageRange { get; set; }
        public string OcType { get; set; }
        public string StrategyType { get; set; }
        public string ExpiryDate { get; set; }
    }
}