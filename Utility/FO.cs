using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot.Types;

namespace TOC
{
    public class FO
    {
        public static double CalcExpVal(string contractType, string transactionType, double strikePrice, double premiumPaid, double closingPrice)
        {
            double result = 0;
            if (contractType.Equals(enumContractType.CE.ToString()))
            {
                if (transactionType.Equals(enumTransactionType.BUY.ToString()))
                {
                    result = CallBuy(strikePrice, premiumPaid, closingPrice);
                }
                if (transactionType.Equals(enumTransactionType.SELL.ToString()))
                {
                    result = CallSell(strikePrice, premiumPaid, closingPrice);
                }
            }

            if (contractType.Equals(enumContractType.PE.ToString()))
            {
                if (transactionType.Equals(enumTransactionType.BUY.ToString()))
                {
                    result = PutBuy(strikePrice, premiumPaid, closingPrice);
                }
                if (transactionType.Equals(enumTransactionType.SELL.ToString()))
                {
                    result = PutSell(strikePrice, premiumPaid, closingPrice);
                }
            }

            if (contractType.Equals(enumContractType.FUT.ToString()))
            {
                if (transactionType.Equals(enumTransactionType.BUY.ToString()))
                {
                    result = FutBuy(strikePrice, closingPrice);
                }
                if (transactionType.Equals(enumTransactionType.SELL.ToString()))
                {
                    result = FutSell(strikePrice, closingPrice);
                }
            }

            if (contractType.Equals(enumContractType.EQ.ToString()))
            {
                if (transactionType.Equals(enumTransactionType.BUY.ToString()))
                {
                    result = EQBuy(strikePrice, closingPrice);
                }
            }

            return Math.Round(result, 0);
        }
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
            if (closingPrice < strikePrice)
            {
                result = strikePrice - closingPrice - premiumPaid;
            }
            else if (closingPrice >= strikePrice)
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