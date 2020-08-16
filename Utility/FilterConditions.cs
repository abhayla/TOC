using System;

namespace TOC
{
    public class FilterConditions
    {
        public int SPExpiry { get; set; }
        public int SPDifference { get; set; } = 100;
        public int SPLowerRange { get; set; }
        public int SPHigherRange { get; set; }
        public string ContractType { get; set; } = "ALL";
        public TimeSpan TimeGap { get; set; } = new TimeSpan(0, 1, 0);
        public double PercentageRange { get; set; } = 2;
        public string OCType { get; set; }
        public string StrategyType { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = "ALL";
        public int WeeksToExpiry { get; set; }
        public string TransactionType { get; set; } = "ALL";
        public bool ShowSPBetweenLowHigh { get; set; } = true;
        public string LTP { get; set; } = "ALL";
        public string ExtrinsicValuePer { get; set; } = "ALL";
        public string ExtValPerDay { get; set; } = "ALL";
        public string ImpliedVolatility { get; set; } = "ALL";
    }
}