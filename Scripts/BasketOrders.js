var TRADE_SYMBOL_COL_INDEX = 0;
var CE_PE_COL_INDEX = 1;
var BUY_SELL_COL_INDEX = 2;
var STRIKE_PRICE_COL_INDEX = 3;
var QUANTITY_COL_INDEX = 4;
var PREMIUM_COL_INDEX = 5;

function send(gvStrategyId) {
    var jsonArr = [];
    jsonArr = GetJsonFromGrid(gvStrategyId);

    document.getElementById("basket").value = JSON.stringify(jsonArr);
    document.getElementById("basket-form").submit();
};

function GetJsonFromGrid(gvStrategyId) {

    var gvStrategy = document.getElementById(gvStrategyId);

    var jsonArr = [];
    for (var irowcount = 1; irowcount < gvStrategy.rows.length; irowcount++) {

        jsonArr.push({
            variety: 'regular',
            tradingsymbol: gvStrategy.rows[irowcount].cells[TRADE_SYMBOL_COL_INDEX].innerHTML,
            exchange: 'NFO',
            transaction_type: gvStrategy.rows[irowcount].cells[BUY_SELL_COL_INDEX].innerHTML,
            order_type: 'MARKET',
            price: parseInt(gvStrategy.rows[irowcount].cells[STRIKE_PRICE_COL_INDEX].innerHTML),
            quantity: parseInt(gvStrategy.rows[irowcount].cells[QUANTITY_COL_INDEX].innerHTML),
            product: 'NRML',
            readonly: true
        });
    }
    return jsonArr;
}

function ZerodhaBasketOrder(gvStrategyId) {

    //var jsonArr = [];
    //jsonArr = GetJsonFromGrid(gvStrategyId);

    var gvStrategy = document.getElementById(gvStrategyId);
    //alert("gvStrategy.rows.length:- " + gvStrategy.rows.length);

    kite = new KiteConnect("d805j3f0aeciwx8g");
    kite.link('span:has(button)');
    kite.link("#custom-button");
    kite.renderButton("#custom-button");
    kite.link("#default-button");
    kite.renderButton("#default-button");

    //alert(JSON.stringify(jsonArr));
    //kite.add(JSON.stringify(jsonArr));

    for (var irowcount = 1; irowcount < gvStrategy.rows.length; irowcount++) {

        kite.add({
            variety: 'regular',
            tradingsymbol: gvStrategy.rows[irowcount].cells[TRADE_SYMBOL_COL_INDEX].innerHTML,
            exchange: 'NFO',
            transaction_type: gvStrategy.rows[irowcount].cells[BUY_SELL_COL_INDEX].innerHTML,
            order_type: 'LIMIT',
            price: parseInt(gvStrategy.rows[irowcount].cells[STRIKE_PRICE_COL_INDEX].innerHTML),
            quantity: parseInt(gvStrategy.rows[irowcount].cells[QUANTITY_COL_INDEX].innerHTML),
            product: 'NRML',
            readonly: false
        });
    }
}

function CreateBasketOfOrdersFromNiftyWatchlist(gvNiftyWatchlistId) {
    var gvNiftyWatchlist = document.getElementById(gvNiftyWatchlistId);
    kite = new KiteConnect("d805j3f0aeciwx8g");
    kite.link('span:has(button)');
    //kite.link("#custom-button");
    //kite.renderButton("#custom-button");
    //kite.link("#default-button");
    //kite.renderButton("#default-button");

    //kite.link(':button');
    //kite.link('span:has(button)');
    //kite.link("#custom-button");
    //kite.renderButton("#custom-button");
    //kite.link("#default-button");
    //kite.renderButton("#default-button");
    //alert("gvNiftyWatchlist.rows.length:- " + gvNiftyWatchlist.rows.length);

    for (var irowcount = 1; irowcount < gvNiftyWatchlist.rows.length; irowcount++) {
        var chkCE = gvNiftyWatchlist.rows[irowcount].cells[0].getElementsByTagName("input");
        if (chkCE !== null && chkCE[0] !== null && chkCE[0].type === "checkbox") {
            if (chkCE[0].checked) {
                var tradingsymbolCE = gvNiftyWatchlist.rows[irowcount].cells[1].innerHTML;
                var priceCE = gvNiftyWatchlist.rows[irowcount].cells[6].innerHTML;
                kite.add({
                    variety: 'regular',
                    tradingsymbol: tradingsymbolCE,
                    exchange: 'NFO',
                    transaction_type: 'SELL',
                    order_type: 'LIMIT',
                    price: parseInt(priceCE),
                    quantity: 75,
                    product: 'NRML',
                    readonly: false
                });
            }
        }

        var chkPE = gvNiftyWatchlist.rows[irowcount].cells[27].getElementsByTagName("input");
        if (chkPE !== null && chkPE[0] !== null && chkPE[0].type === "checkbox") {
            if (chkPE[0].checked) {
                var tradingsymbolPE = gvNiftyWatchlist.rows[irowcount].cells[26].innerHTML;
                var pricePE = gvNiftyWatchlist.rows[irowcount].cells[21].innerHTML;
                kite.add({
                    variety: 'regular',
                    tradingsymbol: tradingsymbolPE,
                    exchange: 'NFO',
                    transaction_type: 'SELL',
                    order_type: 'LIMIT',
                    price: parseInt(pricePE),
                    quantity: 75,
                    product: 'NRML',
                    readonly: false
                });
            }
        }
    }
}

function CreateBasketOfOrdersFromBasketOrder(gvBasketOrderId) {
    var gvBasketOrder = document.getElementById(gvBasketOrderId);
    kite = new KiteConnect("d805j3f0aeciwx8g");
    kite.link('span:has(button)');

    for (var irowcount = 1; irowcount < gvBasketOrder.rows.length; irowcount++) {
        var chkCE = gvBasketOrder.rows[irowcount].cells[0].getElementsByTagName("input");
        if (chkCE !== null && chkCE[0] !== null && chkCE[0].type === "checkbox") {
            if (chkCE[0].checked) {
                var tradingsymbolCE = gvBasketOrder.rows[irowcount].cells[10].innerHTML;
                var priceCE = gvBasketOrder.rows[irowcount].cells[7].innerHTML;
                var transactionType = gvBasketOrder.rows[irowcount].cells[3].innerHTML;
                var orderType = gvBasketOrder.rows[irowcount].cells[6].innerHTML;
                var quantity = parseInt(gvBasketOrder.rows[irowcount].cells[8].innerHTML) * 75;
                kite.add({
                    variety: 'regular',
                    tradingsymbol: tradingsymbolCE,
                    exchange: 'NFO',
                    transaction_type: transactionType,
                    order_type: orderType,
                    price: parseInt(priceCE),
                    quantity: quantity,
                    product: 'NRML',
                    readonly: false
                });
            }
        }

        //var chkPE = gvBasketOrder.rows[irowcount].cells[27].getElementsByTagName("input");
        //if (chkPE !== null && chkPE[0] !== null && chkPE[0].type === "checkbox") {
        //    if (chkPE[0].checked) {
        //        var tradingsymbolPE = gvBasketOrder.rows[irowcount].cells[16].innerHTML;
        //        var pricePE = gvBasketOrder.rows[irowcount].cells[11].innerHTML;
        //        kite.add({
        //            variety: 'regular',
        //            tradingsymbol: tradingsymbolPE,
        //            exchange: 'NFO',
        //            transaction_type: 'SELL',
        //            order_type: 'LIMIT',
        //            price: parseInt(pricePE),
        //            quantity: 75,
        //            product: 'NRML',
        //            readonly: false
        //        });
        //    }
        //}
    }
}

function ZerodhaBasketOrder123(gvStrategyId) {

    var jsonArr = [];
    jsonArr = GetJsonFromGrid(gvStrategyId);

    KiteConnect.ready(function () {
        // Initialize a new Kite instance. You can initialize multiple instances if you need.
        var kite = new KiteConnect("OptionsOfOptions");

        // Add a stock to the basket
        //kite.add({
        //    "exchange": "NSE",
        //    "tradingsymbol": "INFY",
        //    "quantity": 5,
        //    "transaction_type": "BUY",
        //    "order_type": "MARKET",
        //    "product": "MIS"
        //});
        // Add another stock
        //kite.add({
        //    "exchange": "NSE",
        //    "tradingsymbol": "SBIN",
        //    "quantity": 1,
        //    "order_type": "LIMIT",
        //    "transaction_type": "SELL",
        //    "price": 105
        //});

        // Render the in-built button inside a given target
        kite.renderButton("#custom-button");
        kite.link("#buy-basket-stock");


        // form
        $(document).ready(function () {
            $("#form1").submit(function () {
                var kite = new KiteConnect("OptionsOfOptions");

                kite.add({
                    "exchange": "NSE",
                    "tradingsymbol": "SBIN",
                    "quantity": 1,
                    "order_type": "LIMIT",
                    "transaction_type": "SELL",
                    "price": 105,
                    "product": "MIS"
                });
                kite.connect();

                return false;
            });
        });
    });
}