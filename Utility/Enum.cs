using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOC
{
    //enum enumPTColumns
    //{
    //    Delete,
    //    OC_Type,
    //    Expiry_Date,
    //    Contract_Type,
    //    Transaction_Type,
    //    Strike_Price,
    //    Lots,
    //    Entry_Price,
    //    Exit_Price,
    //    CMP,
    //    PL,
    //    Chg,
    //    Realised_PL,
    //    Strategy,
    //    Position,
    //    Profiles
    //}

    enum enumOutputGridColumns
    {
        TradingSymbol,
        CEPE,
        BuySell,
        StrikePrice,
        Quantity,
        Premium,
        ProfitLoss
    }

    enum enumStrategyColumns
    {
        Stock,
        Identifier,
        TradingSymbol,
        ContractType,
        TransactionType,
        StrikePrice,
        LotSize,
        Premium,
        ExpiryDate,
        WeeksToExpiry,
        IntrinsicValue
    }

    enum enumDataSource
    {
        Positions,
        NiftyWatchlist,
        Others
    }

    enum enumResultColumns
    {
        Trading_Symbol,
        CE_PE,
        Buy_Sell,
        Strike_Price,
        Qty,
        Premium,
        Profit_Loss
    }

    enum enumStrategyType
    {
        BUTTERFLY,
        SPREADS,
        SHORT_STRADDLE,
        IRON_CONDOR

    }
    enum enumOCType
    {
        NIFTY,
        BANKNIFTY,
        NIFTYIT
    }
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
        BUY, 
        SELL
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
        Nifty = 75,
        BankNifty = 20
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