using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TOC.Utility
{
    public class FO
    {
        public static double CallBuy(double strikePrice, double premiumPaid, double closingPrice)
        {
            double result = 0;
            if (closingPrice > strikePrice)
            {
                result = -premiumPaid + closingPrice - strikePrice;
            }
            else if (closingPrice <= strikePrice)
            {
                result = -premiumPaid;
            }
            return Math.Round(result, 0);
        }
        public static double PutBuy(double strikePrice, double premiumPaid, double closingPrice)
        {
            double result = 0;
            if (closingPrice <= strikePrice)
            {
                result = -premiumPaid + strikePrice - closingPrice;
            }
            else if (closingPrice > strikePrice)
            {
                result = -premiumPaid;
            }
            return Math.Round(result, 0);
        }
        public static double CallSell(double strikePrice, double premiumPaid, double closingPrice)
        {
            double result = 0;
            if (closingPrice <= strikePrice)
            {
                result = premiumPaid;
            }
            else if (closingPrice > strikePrice)
            {
                result = premiumPaid + strikePrice - closingPrice;
            }
            return Math.Round(result, 0);
        }
        public static double PutSell(double strikePrice, double premiumPaid, double closingPrice)
        {
            double result = 0;
            if (closingPrice >= strikePrice)
            {
                result = premiumPaid;
            }
            else if (closingPrice < strikePrice)
            {
                result = premiumPaid + closingPrice - strikePrice;
            }
            return Math.Round(result, 0);
        }
        public static double FutBuy(double buyPrice, double closingPrice)
        {
            double result = 0;
            result = closingPrice - buyPrice;
            return Math.Round(result, 0);
        }
        public static double FutSell(double buyPrice, double closingPrice)
        {
            double result = 0;
            result = buyPrice - closingPrice;
            return Math.Round(result, 0);
        }
        public static double EQBuy(double buyPrice, double closingPrice)
        {
            double result = 0;
            result = closingPrice - buyPrice;
            return Math.Round(result, 0);
        }
    }
}