using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOC
{
    enum enumPositionStatus
    {
        Open, Close
    }
    enum enumPositionType
    {
        Long, Short
    }
    enum enumTransactionType
    {
        BUY, SELL
    }
    enum enumContractType
    {
        CE,
        PE,
        FUT,
        EQ
    }
    enum enumLotSize
    {
        Nifty=75,
        BankNifty=20
    }
    enum enumExchange
    {
        NSE, BSE, MCX
    }

    public class OptionChainColumns
    {
        public string CEchart;
        public float CEopenInterest;
        public float CEchangeinOpenInterest;
        public float CEtotalTradedVolume;
        public float CEimpliedVolatility;
        public float CElastPrice;
        public float CEchange;
        public float CEbidQty;
        public float CEbidprice;
        public float CEaskPrice;
        public float CEaskQty;
        public float CEstrikePrice;
        public float PEbidQty;
        public float PEbidprice;
        public float PEaskPrice;
        public float PEaskQty;
        public float PEchange;
        public float PElastPrice;
        public float PEimpliedVolatility;
        public float PEtotalTradedVolume;
        public float PEchangeinOpenInterest;
        public float PEopenInterest;
        public string PEchart;

    }
}