using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TOC.Utility
{
    public class FO
    {
        public double CallBuy(double strikePrice, double premiumPaid, double closingPrice)
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
            return result;
        }
        public double PutBuy(double strikePrice, double premiumPaid, double closingPrice)
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
            return result;
        }
        public double CallSell(double strikePrice, double premiumPaid, double closingPrice)
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
            return result;
        }
        public double PutSell(double strikePrice, double premiumPaid, double closingPrice)
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
            return result;
        }
        public double FutBuy(double buyPrice, double closingPrice)
        {
            double result = 0;
            result = closingPrice - buyPrice;
            return result;
        }
        public double FutSell(double buyPrice, double closingPrice)
        {
            double result = 0;
            result = buyPrice - closingPrice;
            return result;
        }
        public double EQBuy(double buyPrice, double closingPrice)
        {
            double result = 0;
            result = closingPrice - buyPrice;
            return result;
        }
    }
}