//Help link - https://www.dotnetcurry.com/ShowArticle.aspx?ID=274

var NIFTY = "NIFTY";
var NIFTY_COL_DIFF = 100;
var NIFTY_LOT_SIZE = 75;

var BANKNIFTY = "BANKNIFTY";
var BANKNIFTY_COL_DIFF = 200;
var BANKNIFTY_LOT_SIZE = 20;

var DELETE_COL_INDEX = 0;
var CONTRACT_TYP_COL_INDEX = 1;
var TRANSTYP_COL_INDEX = 2;
var SP_COL_INDEX = 3;
var CMP_COL_INDEX = 4;
var PREMINUM_COL_INDEX = 5;
var LOTS_COL_INDEX = 6;
var SELECTED_SP_COL_INDEX = 17;
var LOWER_SP_COL_START_INDEX = 7;
var HIGHER_SP_COL_END_INDEX = 27;

function SetGridviewHeaderValues(gvStrategyId) {
    var gvStrategy = document.getElementById(gvStrategyId);

    var strikeavg = 0;
    for (var irowcount = 1; irowcount < gvStrategy.rows.length; irowcount++) {
        strikeavg += eval(gvStrategy.rows[irowcount].cells[SP_COL_INDEX].children[0].value);
    }
    strikeavg = Math.round(strikeavg / (gvStrategy.rows.length - 1), 0);

    //Set column difference based on index
    var columnDiff = 0;
    var selectedOCType = RblOCTypeSelectedValue();
    if (selectedOCType === NIFTY)
        columnDiff = NIFTY_COL_DIFF;
    if (selectedOCType === BANKNIFTY)
        columnDiff = BANKNIFTY_COL_DIFF;

    //Assign header value to the center column
    gvStrategy.rows[0].cells[SELECTED_SP_COL_INDEX].innerHTML = strikeavg;

    //Assign value to the left columns from the center
    for (irowcount = 1; irowcount <= 10; irowcount++) {
        gvStrategy.rows[0].cells[SELECTED_SP_COL_INDEX - irowcount].innerHTML = Math.round((strikeavg - (irowcount * columnDiff)), 2);
    }

    //Assign value to the right columns from the center
    for (irowcount = 1; irowcount <= 10; irowcount++) {
        gvStrategy.rows[0].cells[SELECTED_SP_COL_INDEX + irowcount].innerHTML = Math.round((strikeavg + (irowcount * columnDiff)), 2);
    }
}

function CalculateExpiryValueForAllRowsANDCells(gvStrategyId, rblOCTypeId) {

    var gvStrategy = document.getElementById(gvStrategyId);
    //var rblOCType = document.getElementById(rblOCTypeId);

    for (var irowcount = 1; irowcount < gvStrategy.rows.length; irowcount++) {

        var contractType = gvStrategy.rows[irowcount].cells[CONTRACT_TYP_COL_INDEX].children[0].value;
        var transactionType = gvStrategy.rows[irowcount].cells[TRANSTYP_COL_INDEX].children[0].value;
        var strikePrice = gvStrategy.rows[irowcount].cells[SP_COL_INDEX].children[0].value;
        var premiumPaid = gvStrategy.rows[irowcount].cells[PREMINUM_COL_INDEX].children[0].value;
        var lots = gvStrategy.rows[irowcount].cells[LOTS_COL_INDEX].children[0].value;

        var lotSize = 0;
        var selectedOCType = RblOCTypeSelectedValue();
        if (selectedOCType === NIFTY)
            lotSize = NIFTY_LOT_SIZE;
        if (selectedOCType === BANKNIFTY)
            lotSize = BANKNIFTY_LOT_SIZE;


        for (var icellcount = LOWER_SP_COL_START_INDEX; icellcount < gvStrategy.rows[irowcount].cells.length; icellcount++) {

            var expiryPrice = gvStrategy.rows[0].cells[icellcount].innerHTML;

            gvStrategy.rows[irowcount].cells[icellcount].innerHTML =
                CalculateExpiryValue(contractType, transactionType, strikePrice, premiumPaid, expiryPrice) * lots * lotSize;
        }
    }

    CalculateSumofColumn(gvStrategyId);
}

function RblOCTypeSelectedValue() {
    var radioButtons = document.getElementsByName("rblOCType");
    for (var x = 0; x < radioButtons.length; x++) {
        if (radioButtons[x].checked) {
            return radioButtons[x].value;
        }
    }
}

function CalcExpValForAllCellsInRow(gvStrategyId, irowcount, rblOCTypeId) {

    var gvStrategy = document.getElementById(gvStrategyId);
    //var rblOCType = document.getElementById(rblOCTypeId);

    var contractType = gvStrategy.rows[irowcount].cells[CONTRACT_TYP_COL_INDEX].children[0].value;
    var transactionType = gvStrategy.rows[irowcount].cells[TRANSTYP_COL_INDEX].children[0].value;
    var strikePrice = gvStrategy.rows[irowcount].cells[SP_COL_INDEX].children[0].value;
    var premiumPaid = gvStrategy.rows[irowcount].cells[PREMINUM_COL_INDEX].children[0].value;

    var lots = gvStrategy.rows[irowcount].cells[LOTS_COL_INDEX].children[0].value;

    var lotSize = 0;
    var selectedOCType = RblOCTypeSelectedValue();
    if (selectedOCType === NIFTY)
        lotSize = NIFTY_LOT_SIZE;
    if (selectedOCType === BANKNIFTY)
        lotSize = BANKNIFTY_LOT_SIZE;

    for (var icellcount = LOWER_SP_COL_START_INDEX; icellcount < gvStrategy.rows[irowcount].cells.length; icellcount++) {

        var expiryPrice = gvStrategy.rows[0].cells[icellcount].innerHTML;
        eval(expiryPrice);

        gvStrategy.rows[irowcount].cells[icellcount].innerHTML =
            CalculateExpiryValue(contractType, transactionType, strikePrice, premiumPaid, expiryPrice) * lots * lotSize;
    }

    CalculateSumofColumn(gvStrategyId);
}

function CalculateSumofColumn(gvStrategyId) {
    var gvStrategy = document.getElementById(gvStrategyId);
    var HEADER_ROW_INDEX = 0;
    var FOOTER_ROW_INDEX = gvStrategy.rows.length;
    var sum = 0;
    for (var icellcount = LOWER_SP_COL_START_INDEX; icellcount < gvStrategy.rows[gvStrategy.rows.length - 1].cells.length; icellcount++) {
        sum = 0;
        for (var irowcount = 1; irowcount < gvStrategy.rows.length - 1; irowcount++) {
            sum += eval(gvStrategy.rows[irowcount].cells[icellcount].innerHTML);
        }
        gvStrategy.rows[gvStrategy.rows.length - 1].cells[icellcount].innerHTML = sum;
    }
}

function CalculateExpiryValue(contractType, transactionType, strikePrice, premiumPaid, expiryPrice) {

    var result = 0;
    if (contractType === "PE" && transactionType === "BUY")
        result = PEBuy(strikePrice, premiumPaid, expiryPrice);
    if (contractType === "PE" && transactionType === "SELL")
        result = PESell(strikePrice, premiumPaid, expiryPrice);
    if (contractType === "CE" && transactionType === "BUY")
        result = CEBuy(strikePrice, premiumPaid, expiryPrice);
    if (contractType === "CE" && transactionType === "SELL")
        result = CESell(strikePrice, premiumPaid, expiryPrice);
    if (contractType === "EQ" && transactionType === "BUY")
        result = EQBuy(strikePrice, expiryPrice);
    if (contractType === "FUT" && transactionType === "BUY")
        result = FutBuy(strikePrice, expiryPrice);
    if (contractType === "FUT" && transactionType === "SELL")
        result = FutSell(strikePrice, expiryPrice);

    return result;
}

function FutBuy(strikePrice, expiryPrice) {
    var result = 0;
    result = eval(expiryPrice) - eval(strikePrice);
    return Math.round(result, 0);
}

function FutSell(strikePrice, expiryPrice) {
    var result = 0;
    result = eval(strikePrice) - eval(expiryPrice);
    return Math.round(result, 0);
}

function EQBuy(strikePrice, expiryPrice) {
    var result = 0;
    result = eval(expiryPrice) - eval(strikePrice);
    return Math.round(result, 0);
}

function PEBuy(strikePrice, premiumPaid, expiryPrice) {
    var result = 0;
    if (expiryPrice < strikePrice) {
        result = eval(strikePrice) - eval(expiryPrice) - eval(premiumPaid);
    }
    else if (expiryPrice >= strikePrice) {
        result = -premiumPaid;
    }
    return Math.round(result, 0);
}

function PESell(strikePrice, premiumPaid, expiryPrice) {
    var result = 0;
    if (expiryPrice >= strikePrice) {
        result = premiumPaid;
    }
    else if (expiryPrice < strikePrice) {
        result = eval(premiumPaid) + eval(expiryPrice) - eval(strikePrice);
    }
    return Math.round(result, 0);
}

function CESell(strikePrice, premiumPaid, expiryPrice) {
    var result = 0;
    if (expiryPrice <= strikePrice) {
        result = premiumPaid;
    }
    else if (expiryPrice > strikePrice) {
        result = eval(premiumPaid) + eval(strikePrice) - eval(expiryPrice);
    }
    return Math.round(result, 0);
}

function CEBuy(strikePrice, premiumPaid, expiryPrice) {
    var result = 0;
    if (expiryPrice > strikePrice) {
        result = eval(expiryPrice) - eval(strikePrice) - eval(premiumPaid);
    }
    else if (expiryPrice <= strikePrice) {
        result = -premiumPaid;
    }
    return Math.round(result, 0);
}