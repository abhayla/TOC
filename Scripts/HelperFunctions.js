/*********************************************************************
/// <summary>
// Name of File     : HelperFunctions.js 
/// </summary>
************************************************************************/
//Session timeout variable, it is set from PopUpMaster.js and MasterPage.js
var isTimedOut = false;
//Session Timeout for pop up
//this governs that after timeout, 
//pop up windows will close 
var TIME_OUT_VALUE = 28740000;
var isErrorOccured = false;
var doneStatusMessage;
var CONST_LEGAL_INDICATOR_SET = "LEGALSET";
var FLAG_YES = "Y";
var FLAG_TRUE = "true";
var FLAG_FALSE = "false";

//MOD001-R78 - Start
var SI_ADJUSTMENT = "FIN05VC";
var SI_BANK_CORRECTION = "FIN06VC";
var SI_REFUND = "FIN03VC";
var SI_DEPOSIT_REFUND = "FIN04VC";
var SI_TRANSFER_ACCOUNT_BALANCE = "FIN11VC";
var SI_TRANSFER_PAYMENT = "FIN12VC";
var SI_REDIRECT_PAYMENT = "FIN14VC";
var SI_REDIRECT_ACCOUNT_BALANCE = "FIN13VC";
var SI_EARMARK_PAYMENT = "FIN15VC";
var SI_DEPOSIT_REQUEST = "FIN19VC";
var SI_DEPOSIT_ADJUSTMENT = "FIN20VC";
var SI_DEPOSIT_RECEIPTS = "FIN22VC";
var SI_ESTIMATE_DEPOSIT = "FIN23VC";
var SI_MANUAL_DISCONNECT_RECONNECT = "ORD20VC";
var SI_MOVEIN_ORDER_DETAILS = "ORD03VC";
var SI_MOVEIN_PRODUCT_AND_SERVICES = "ORD04VC";
var SI_MOVEIN_RECAP = "ORD05VC";
var SI_MOVEIN_SERVICE_DETAILS = "ORD02VC";
var SI_MOVEOUT_ORDER_DETAILS = "ORD07VC";
var SI_MOVEOUT_RECAP = "ORD11VC";
var SI_MOVEOUT_SERVICE_DETAILS = "ORD06VC";
var SI_GAS_TROUBLE_ORDER = "ORD15VC";
var SI_ELECTRIC_TROUBLE_ORDER = "ORD14VC";
var SI_METER_ORDER_DETAILS = "ORD19VC";
//MOD001-R78 - End

/*******************************************************************************
/Function Name   :  FireOnTimeOut
/
/Description     :  Checks for time out.
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function FireOnTimeOut() {
    isTimedOut = true;
    // Redirect to a page
    window.location.href = 'SessionTimeOut.aspx';
}

setTimeout("FireOnTimeOut();", TIME_OUT_VALUE);

/*******************************************************************************
/Function Name   :  GetFriendlyMessage
/
/Description     :  Gets friendly error message for the error code provided.
/                   It replaces the placeholders of the error message with parameters provided
/                   and returns the message
/                   
/Input Parameter :  None
/
/Return          :  errorMessage
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function GetFriendlyMessage() {
    try {
        var errorArray = GetFriendlyMessage.arguments;
        if (errorArray.length < 1) {
            return '';
        }
        var errorMessage = errorArray[0];

        if (errorMessage != "") {
            //var opt = ctrl.options[ctrl.selectedIndex];
            //var errorMessage = opt.innerText;
            //Modified by Nidhish-12/06/2005
            var splitMessage = errorMessage.split('&apos;');
            if (splitMessage.length > 0) {
                for (var counter = 0; counter < splitMessage.length; counter++) {
                    errorMessage = errorMessage.replace("&apos;", "'");
                }
            }
            //Modified by Nidhish-12/06/2005
            for (var i = 1; i < errorArray.length; i++) {
                var searchStr = "{" + (i - 1).toString() + "}";

                if (errorMessage.indexOf(searchStr) >= 0) {
                    errorMessage = errorMessage.replace(searchStr, errorArray[i]);
                    // alert(errorMessage);
                }
                else {
                    break;
                }
            }
        }
        return errorMessage;
    }
    catch (e) {
        return '';
    }
}

/*******************************************************************************
/Function Name   :  IsNumeric
/
/Description     :  Checks numeric value.
/                   returns true if numeric else returns false.
/                   
/Input Parameter :  val
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function IsNumeric(val) {
    var functionNumber = val;
    var functionNumberPattern = /^[0-9](.+)\.[0-9](.+)$/;
    var matchArray = functionNumber.match(functionNumberPattern);
    if (matchArray == null) {
        return false;
    }
    else {
        return true;
    }
}

/*******************************************************************************
/Function Name   :  ChangeHeight
/
/Description     :  changes the height of the object depending upon resolution
/                   
/                   
/Input Parameter :  obj,hgt
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function ChangeHeight(obj, hgt) {

    if (screen.availWidth == 800) {
        if (obj.height != null) {
            obj.height = hgt;
        }
        if (obj.style.height != null) {
            obj.style.height = hgt;
        }
    }
}

/*******************************************************************************
/Function Name   :  ChangeHeightForServerControl
/
/Description     :  changes the height of the id depending on sreen resolution
/                   
/                   
/Input Parameter :  id,hgt
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/

function ChangeHeightForServerControl(id, hgt) {
    if (id != null) {
        var controlIdValue = GetControlId();
        var obj = GetObjectForId(controlIdValue + "_" + id);
        if (obj != null) {
            if (screen.availWidth == 800) {
                if (obj.height != null) {
                    obj.height = hgt;
                }
                if (obj.style.height != null) {
                    obj.style.height = hgt;
                }
            }
        }
    }
}

/*******************************************************************************
/Function Name   :  ChangeWidth
/
/Description     :  changes the width of the object depending upon resolution
/                   
/                   
/Input Parameter :  obj,wdth
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function ChangeWidth(obj, wdth) {
    if (screen.availWidth == 800) {
        if (obj.height != null) {
            obj.width = wdth;
        }
        if (obj.style.height != null) {
            obj.style.width = wdth;
        }
    }
}

/*******************************************************************************
/Function Name   :  ChangeWidthForServerControl
/
/Description     :  changes the width of the object depending upon resolution
/                   
/                   
/Input Parameter :  id,wdth
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function ChangeWidthForServerControl(id, wdth) {
    if (id != null) {
        var controlIdValue = GetControlId();
        var obj = GetObjectForId(controlIdValue + "_" + id);
        if (obj != null) {
            if (screen.availWidth == 800) {
                if (obj.height != null) {
                    obj.width = wdth;
                }
                if (obj.style.height != null) {
                    obj.style.width = wdth;
                }
            }
        }
    }
}

/*******************************************************************************
/Function Name   :  CommaFormatted
/
/Description     :  Formats the amount with thousand separator
/                   
/                   
/Input Parameter :  amount
/
/Return          :  amount
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function CommaFormatted(amount) {
    var delimiter = ","; // replace comma if desired
    var a = amount.split('.', 2)
    var d = a[1];
    var i = parseInt(a[0]);
    if (isNaN(i)) { return ''; }
    var minus = '';
    if (i < 0) { minus = '-'; }
    i = Math.abs(i);
    var n = new String(i);
    var a = [];
    while (n.length > 3) {
        var nn = n.substr(n.length - 3);
        a.unshift(nn);
        n = n.substr(0, n.length - 3);
    }
    if (n.length > 0) { a.unshift(n); }
    n = a.join(delimiter);
    if (d.length < 1) { amount = n; }
    else { amount = n + '.' + d; }
    amount = minus + amount;
    return amount;
}

/*******************************************************************************
/Function Name   :  CurrencyFormatted
/
/Description     :  Formats the Amount upto two decimal places
/                   
/                   
/Input Parameter :  amount
/
/Return          :  s
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function CurrencyFormatted(amount) {
    var i = parseFloat(amount);
    if (isNaN(i)) { i = 0.00; }
    var minus = '';
    if (i < 0) { minus = '-'; }
    i = Math.abs(i);
    i = parseInt((i + .005) * 100);
    i = i / 100;
    s = new String(i);
    if (s.indexOf('.') < 0) { s += '.00'; }
    if (s.indexOf('.') == (s.length - 2)) { s += '0'; }
    s = minus + s;
    return s;
}
/*******************************************************************************
/Function Name   :  AdjustControl
/
/Description     :  Adjust the Control according to the controlString
/                   ControlString is of the format controlId:Property:propertyValue
/                   eg: TxtName:disabled:false
/Input Parameter :  controlString
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
var controlSeparator = ':';
function AdjustControl(controlString) {
    var arr = controlString.split(controlSeparator);
    if (arr.length > 2) {
        var controlID = document.getElementById("controlID").value;
        if (controlID != null) {
            var cntrl = document.getElementById(controlID + "_" + arr[0]);
            if (cntrl != null) {
                switch (arr[1]) {
                    case 'disabled':
                    case 'enabled':
                        {
                            if (arr[2] == 'false' || arr[2] == 'False') {
                                cntrl.disabled = '';
                            }
                            else {
                                cntrl.disabled = 'disabled';
                            }

                            break;
                        }
                    case 'value':
                        {
                            cntrl.value = arr[2];
                            break;
                        }
                    case 'visible':
                    case 'display':
                        {
                            if (arr[2] == 'false' || arr[2] == 'False') {
                                cntrl.style.display = 'none';
                            }
                            else {
                                cntrl.style.display = '';
                            }
                            break;
                        }
                    case 'checked':
                        {
                            if (arr[2] == 'false' || arr[2] == 'False') {
                                cntrl.checked = false;
                            }
                            else {
                                cntrl.checked = true;
                            }
                            break;
                        }
                    case 'checkboxdisable':
                        {

                            if (arr[2] == 'false' || arr[2] == 'False') {
                                cntrl.parentElement.disabled = '';
                                cntrl.disabled = '';
                            }
                            else {
                                cntrl.parentElement.disabled = 'disabled';
                                cntrl.disabled = 'disabled';
                            }
                            break;
                        }
                }
            }

        }
    }
}

/*******************************************************************************
/Function Name   :  UnderConstruction
/
/Description     :  shows a popup under construction message
/                   
/                   
/Input Parameter :  None
/
/Return          :  false
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function UnderConstruction() {
    window.showModalDialog('HTML/Blank.htm', '', 'center:yes;resizable:yes;dialogHeight:300px;dialogWidth:300px;scroll:off;help:off;status:off');
    return false;
}

/*******************************************************************************
/Function Name   :  fnTrapInvalidNumber
/
/Description     :  This allows to type only numeric values.
/                   
/                   
/Input Parameter :  None
/
/Return          :  True/False
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function fnTrapInvalidNumber() {
    if ((event.keyCode >= 48 && event.keyCode <= 57)
        || (event.keyCode >= 37 && event.keyCode <= 40)
        || (event.keyCode >= 96 && event.keyCode <= 105)
        || event.keyCode == 110
        || event.keyCode == 8
        || event.keyCode == 9
        || event.keyCode == 46
        || event.keyCode == 36
        || event.keyCode == 35
        || (event.shiftKey && event.keyCode == 45)
        || (event.ctrlKey &&
            (event.keyCode == 67
                || event.keyCode == 86
                || event.keyCode == 65
                || event.keyCode == 88
                || event.keyCode == 45))) {
        //MOD006 - Start
        return true;
        //MOD006 - End
    }
    else {
        event.returnValue = false;
        event.cancel = true;
    }
}

/*******************************************************************************
/Function Name   :  fnTrapInvalidAmount
/
/Description     :  this allows to type only numeric values which can contain dot also.
/                   
/                   
/Input Parameter :  None
/
/Return          :  True/False
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function fnTrapInvalidAmount() {
    if ((event.keyCode >= 48 && event.keyCode <= 57)
        || (event.keyCode >= 37 && event.keyCode <= 40)
        || (event.keyCode >= 96 && event.keyCode <= 105)
        || event.keyCode == 110
        || event.keyCode == 190
        || event.keyCode == 46
        || event.keyCode == 8
        || event.keyCode == 9
        || event.keyCode == 36
        || event.keyCode == 35
        || (event.shiftKey && event.keyCode == 45)
        || (event.ctrlKey &&
            (event.keyCode == 67
                || event.keyCode == 86
                || event.keyCode == 65
                || event.keyCode == 88
                || event.keyCode == 45))) {
        //MOD006 - Start
        return true;
        //MOD006 - End
    }
    else {
        event.returnValue = false;
        event.cancel = true;
    }
}

/*******************************************************************************
/Function Name   :  ShowPopUpPage
/
/Description     :  Function to open a Pop-up Page
/                   
/                   
/Input Parameter :  screenName - contains the name of the screen to be opened
/                   parameters - contains the paramters to be passed to the screen
/                                incase of mutliple parameters takes all the parameters seperated by ','
/                   screenwidth- contains the width of the screen to be opened
/                   screenheight- contains the height of the screen to be opened
/                   screenTitle - Contains the title of the screen to be opened
/
/Return          :  returnValue
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function ShowPopUpPage(screenName, parameters, screenwidth, screenheight, screenTitle) {
    if (isTimedOut) {
        alert(EC_115);
        return;
    }
    if (screenTitle == null) {
        screenTitle = screenName;
    }

    var dateToBeUsed = new Date();
    var url = 'PopUpPage.aspx?ScreenName=' + screenName + '&Parameter=' + parameters + '&ScreenTitle=' + screenTitle + '&Time=' + dateToBeUsed.toString() + '' + dateToBeUsed.getMilliseconds();

    var screenSettings = 'help:off;status:off;resizable:yes;dialogHeight:' + screenheight + 'px;dialogWidth:' + screenwidth + 'px;';
    var returnValue;
    if (screenName == '') {
        returnValue = window.showModalDialog(url, '', screenSettings);
    }
    else {
        returnValue = window.showModalDialog(url, '', screenSettings);
    }
    if (returnValue == null) {
        returnValue = false;
    }
    //Do a checking if it actually opens a page inherited by WebFormView or not and then reset
    if (parameters.indexOf('ScreenId') >= 0) {
        var callbackReset = GetObjectForId('__C2CALLBACKRESET');
        if (callbackReset != null) {
            callbackReset.value = 'C2ControllerResetFromHeader';
        }
        ReplaceCallBackResetFromPostData();
    }
    return returnValue;
}
/*******************************************************************************
/Function Name   :  ReplaceCallBackResetFromPostData
/
/Description     :  This function replaces the Call back reset parameter after post back.
/                   
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#      Description
/ 05/05/2005        Nidhish Singh                            Created
/ 05/12/2006        Dawn Brodeur        OT-53-PI             Getting Javascript error when 'Done'
/                                                            button was clicked from popup launched by
/                                                            a popup.
/                                                      
*******************************************************************************/
function ReplaceCallBackResetFromPostData() {
    // OT-53-PI -- corrects 'Done' button click error when popup was launched from
    //             another popup
    if (typeof __theFormPostData == 'undefined' || __theFormPostData == null) {
        return;
    }
    //Modified by nidhish-Start-04/12/2006
    // var idFormPostData= document.all["__theFormPostData"];
    if (__theFormPostData != null) {
        //Modified by nidhish-End-04/12/2006
        var i = __theFormPostData.indexOf('__C2CALLBACKRESET');
        if (i >= 0) {
            var lastIndexOfAmp = __theFormPostData.substr(i).indexOf('&');
            var tobeReplacedString = __theFormPostData.substr(i, lastIndexOfAmp);
            __theFormPostData = __theFormPostData.replace(tobeReplacedString, '__C2CALLBACKRESET=C2ControllerResetFromHeader');
        }
        else {
            __theFormPostData = __theFormPostData +
                "__C2CALLBACKRESET=C2ControllerResetFromHeader&";
        }
    }
}

/*******************************************************************************
/Function Name   :  ShowOpenPopUpPage
/
/Description     :  Function used to open a Main Page as a popup Page
/                   
/                   
/Input Parameter :  screenId,screenheight,screenwidth
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function ShowOpenPopUpPage(screenId, screenheight, screenwidth) {

    if (isTimedOut) {
        alert(EC_115);
        return;
    }
    var url = 'OpenPopUpPage.aspx?ScreenId=' + screenId;
    var screenSettings = 'help:off;status:off;resizable:yes;dialogHeight:' + screenheight + 'px;dialogWidth:' + screenwidth + 'px;';
    var returnValue = window.showModalDialog(url, '', screenSettings);
    if (returnValue == null) {
        returnValue = false;
    }
    /*var callbackReset =  GetObjectForId('__C2CALLBACKRESET');
    if (callbackReset!=null)
    {
        callbackReset.value = 'C2ControllerResetFromHeader';
    }*/
    ReplaceCallBackResetFromPostData();
    if (OpenedMasterPage == 'MasterPage') {
        CallServerHeaderMenu('Controller', 'Controller#####' + returnValue.toString());
    }
    return returnValue;
}

/*******************************************************************************
/Function Name   :  GetControlId
/
/Description     :  Returns the control id 
/                   
/                   
/Input Parameter :  None
/
/Return          :  Control id
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
var __controlId = '';
function GetControlId() {
    if (__controlId == '') {
        var controlId = document.getElementById("controlID");
        if (controlId == null) {
            __controlId = '';
        }
        else {
            __controlId = controlId.value;
        }
    }
    return __controlId;
}

/*******************************************************************************
/Function Name   :  GetObjectForId
/
/Description     :  Returns the object associated with the id 
/                   
/                   
/Input Parameter :  id
/
/Return          :  Object
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
//
function GetObjectForId(id) {
    return document.getElementById(id);
}

/*******************************************************************************
/Function Name   :  Trim
/
/Description     :  Trims the text and returns the trimmed data
/                   
/                   
/Input Parameter :  str
/
/Return          :  Trimmed string
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function Trim(str) {
    return str.replace(/^\s*|\s*$/g, "");
}

/*******************************************************************************
/Function Name   :  EnableLinkButtons
/
/Description     :  Enables or disables the link buttons depending upon the flag
/                   
/                   
/Input Parameter :  linkButton, flag
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/ 02/11/2006        Mrityunjoy          MOD001            Added code for link class style.
*******************************************************************************/
function EnableLinkButtons(linkButton, flag) {

    if (linkButton != null) {
        if (flag) {
            linkButton.disabled = '';
            if (linkButton.href == null || linkButton.href.length == 0) {
                linkButton.href = '';
            }
            //MOD001-Start
            linkButton.className = 'StyleLinkText';
            //MOD001-End
            linkButton.style.cursor = 'hand';
        }
        else {
            linkButton.disabled = true;
            //MOD001-Start
            linkButton.className = 'StyleDisabledLinkText';
            //MOD001-End
            linkButton.style.cursor = 'text';
        }
    }
}
/*******************************************************************************
/Function Name   :  EnableDisableCalendar
/
/Description     :  Enables or disables the calendar depending upon the flag
/                   
/                   
/Input Parameter :  linkButton, flag
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 12/20/2005        Raman Chirania                                  Created
/
*******************************************************************************/
function EnableDisableCalendar(calendar, flag) {
    if (calendar != null) {
        var ctrlArr = calendar.getElementsByTagName('input');
        var ctrlImgArr = calendar.getElementsByTagName('IMG');

        if (ctrlArr.length > 0) {
            if (ctrlArr[0].type == 'text') {
                if (flag) {
                    ctrlArr[0].readOnly = '';
                    ctrlArr[0].className = 'DateHolderTextBox';
                    calendar.readOnly = '';

                    if (ctrlImgArr.length > 0) {
                        ctrlImgArr[0].style.cursor = 'hand';
                    }
                }
                else {
                    ctrlArr[0].readOnly = 'readOnly';
                    ctrlArr[0].className = 'DateDisabledTextBox';
                    calendar.readOnly = 'readOnly';

                    if (ctrlImgArr.length > 0) {
                        ctrlImgArr[0].style.cursor = 'default';
                    }
                }
            }
        }

    }
}
/*******************************************************************************
/Function Name   :  EnableDisableText
/
/Description     :  Enables or disables the text boxes depending upon the flag
/                   
/                   
/Input Parameter :  text, flag
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function EnableDisableText(text, flag) {

    if (text != null) {
        if (flag) {
            text.readOnly = '';
            text.className = 'StyleNormalText';
        }
        else {
            text.readOnly = 'readOnly';
            text.className = 'StyleDisabledText';
        }
    }
}
/*******************************************************************************
/Function Name   :  EnableDisableDropdown
/
/Description     :  Enables or disables the dropdown boxes depending upon the flag
/                   
/                   
/Input Parameter :  ddlb, flag
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function EnableDisableDropdown(ddlb, flag) {

    if (ddlb != null) {
        if (flag) {
            ddlb.disabled = false;
            ddlb.className = 'StyleNormalText';
        }
        else {
            ddlb.disabled = true;
            ddlb.className = 'StyleDisabledText';
        }
    }
}

/*******************************************************************************
/Function Name   :  OpenErrorMessage
/
/Description     :  Opens the Error Message Box
/                   
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function OpenErrorMessage() {
    if (isTimedOut) {
        alert(EC_115);
        return;
    }
    var options;
    if (screen.availWidth == 1024) {
        options = 'dialogHeight:205px;dialogWidth:600px;status:no;help:no;center:yes;scroll:off;resizable:no;';
    }
    else {
        options = 'dialogHeight:205px;dialogWidth:500px;status:no;help:no;center:yes;scroll:off;resizable:no;';
    }
    var url = 'HTML/ErrorFrame.htm';
    var win = window.showModalDialog(url, window, options);
    if (win == true) {
        if (OpenedMasterPage == 'MasterPage') {
            /* if (CurrentScreen()!='SEA01VC')
            {
                window.location.replace('Search.aspx');
            }*/
        }
        else {
            self.close();
        }
    }
    return false;
}

/*******************************************************************************
/Function Name   :  WriteMobiusInfoToFile
/
/Description     :  This function writes the reply string of Mobius into the 
/                   filePath  specified.
/                   
/Input Parameter :  filePath, fileContent
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/07/2005        Rajesh Mishra                                   Created
/ 11/09/2005        Nidhish Singh                                   Modified and added standard comment
/
*******************************************************************************/
function WriteMobiusInfoToFile(filePath, fileContent) {
    try {
        var fileSysObj, fileObject;
        fileSysObj = new ActiveXObject("Scripting.FileSystemObject");
        fileObject = fileSysObj.CreateTextFile(filePath, true);
        fileObject.writeline(fileContent);
        fileObject.Close();
        isErrorOccured = false;
    }
    catch (err) {
        var isCustomMessage = "Error : " + err.number + " : " + err.description;
        alert(isCustomMessage);
        isErrorOccured = true;
    }
}

/*******************************************************************************
/Function Name   :  RunMobius
/
/Description     :  This function excutes the mobius exe.
/                   
/                   
/Input Parameter :  mobiusRun
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/07/2005        Rajesh Mishra                                   Created
/ 11/09/2005        Nidhish Singh                                   Modified and added standard comment
/
*******************************************************************************/
function RunMobius(mobiusRun) {
    try {
        if (isErrorOccured == false) {
            var oWshShell;
            oWshShell = new ActiveXObject("WScript.Shell");
            oWshShell.Run(mobiusRun);
        }
        else {
            isCustomMessage = "Error : " + "Document Direct must be installed in order to view bills";
            alert(isCustomMessage);
        }
    }
    catch (err) {
        isCustomMessage = "Error : " + "Document Direct must be installed in order to view bills";
        alert(isCustomMessage);
    }
}

/*******************************************************************************
/Function Name   :  ParseDateString
/
/Description     :  Parses the data input for a valid date. If input is valid date it returns true
/                   
/                   
/Input Parameter :  objDateHolder
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function ParseDateString(objDateHolder) {
    var stringOfDate = objDateHolder.value;
    var dateToBeReturned = new Date();

    //Unsat 2180 Start
    if (stringOfDate.length != 10)
        return false;
    //Unsat 2180 End

    if (stringOfDate.substr(6, 4) == "0000")
        return false;

    dateToBeReturned.setFullYear(
        stringOfDate.substr(6, 4),
        eval(stringOfDate.substr(0, 2) - 1),
        stringOfDate.substr(3, 2)
    );
    if (dateToBeReturned.getFullYear() == stringOfDate.substr(6, 4)
        && dateToBeReturned.getDate() == stringOfDate.substr(3, 2)
        && dateToBeReturned.getMonth() == stringOfDate.substr(0, 2) - 1
    ) {
        return dateToBeReturned;
    }
    else {
        return false;
    }
}

/*******************************************************************************
/Function Name   :  ExecuteScripts
/
/Description     :  This function is called to execute the string
/                   
/Input Parameter :  strHTML
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function ExecuteScripts(strHTML) {
    var spanTag = document.createElement("span");
    spanTag.innerHTML = strHTML;
    var scriptTags = spanTag.getElementsByTagName("script");
    scriptTagsToBeExecuted = new Array();
    for (var i = 0; i < scriptTags.length; i++) {
        if (scriptTags[i].src == null || scriptTags[i].src == "") {
            var tempStr = scriptTags[i].innerHTML;
            tempStr = tempStr.replace("<!--", "");
            tempStr = tempStr.replace("-->", "");
            scriptTagsToBeExecuted[scriptTagsToBeExecuted.length] = tempStr;
        }
    }
    ExecuteScriptsAfterTimeout();
}
/*******************************************************************************
/Function Name   :  ExecuteScriptsAfterTimeout
/
/Description     :  This function is called to execute the string after time out
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function ExecuteScriptsAfterTimeout() {
    for (var scriptsCounter = 0; scriptsCounter < scriptTagsToBeExecuted.length;
        scriptsCounter++) {
        eval(scriptTagsToBeExecuted[scriptsCounter]);
    }
}

/*******************************************************************************
/Function Name   :  EnableDisableButton
/
/Description     :  This function is called to change the cursor for enabled and disabled buttons
                           . It also handles for callback buttons
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/16/08/2005				  Indranil De      UNSAT # 452                  Created for changing the cursor on  
/                                                                                            mouseover for buttons which trigger 
/                                                                                            CallBack functionality
*******************************************************************************/
function EnableDisableButton(buttonId, isEnabled) {
    var button = buttonId;
    if (button == null) return;
    if (button.attributes["Security"] != null &&
        button.attributes["Security"].value == "Disabled")
        return;

    if (!isEnabled) {
        button.rows[0].cells[1].className = button.CssClass + "MiddleDisabled";
        button.style.cursor = 'text';
        button.disabled = true;
    }
    else {
        button.rows[0].cells[1].className = button.CssClass + "Middle";
        button.style.cursor = 'hand';
        button.disabled = false;
    }

}
/*******************************************************************************
/Function Name   :  SetSelectedIndex
/
/Description     :  This function is used to set the selected index in city drop down.
                           
/                   
/Input Parameter :  None
/
/Return          :  True/False
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/08/20/2005			 Indranil De                                   Created   
/                                                                                            mouseover for buttons which trigger 
/                                                                                            CallBack functionality
*******************************************************************************/
function SetSelectedIndex() {
    var obj = window.event.srcElement;
    if (obj.type.toLowerCase().indexOf('select') == -1)
        return true;
    if (window.event.keyCode == 27) //escape key
    {
        obj.selectedString = '';
        obj.selectedIndex = -1;
    }
    else if (window.event.keyCode == 9 || window.event.keyCode == 38 || window.event.keyCode == 40 || window.event.ctrlKey || window.event.altKey || window.event.type.toLowerCase() == 'blur') // tab, arrow-up, arrow-down, ctrl, or alt keys
    {
        obj.selectedString = '';
        return true;
    }
    else {
        if (typeof obj.selectedString == 'undefined') obj.selectedString = '';
        if (window.event.keyCode == 8) // backspace
            obj.selectedString = (obj.selectedString.length != 0) ? obj.selectedString.substring(0, obj.selectedString.length - 1) : '';
        else
            obj.selectedString = obj.selectedString + String.fromCharCode(window.event.keyCode);
        var newSelectedIndex = -1;
        for (var i = 0; i < obj.options.length; i++) {
            if (obj.options[i].text.toLowerCase().indexOf(obj.selectedString.toLowerCase()) == 0) {
                newSelectedIndex = i;
                break;
            }
        }
        if (newSelectedIndex != -1) {
            obj.selectedIndex = newSelectedIndex;
            //trigger the onchange event
            try {
                window.event.srcElement.onchange();
            }
            catch (e) {
                //if control does not have an onchange event catch the 
                //exception and do nothing
            }
        }
        else {
            obj.selectedString = String.fromCharCode(window.event.keyCode);
            return true;
        }
    }
    window.event.returnValue = false;
}


/*******************************************************************************
/Function Name   :  SetSelectedIndex
/
/Description     :  This function is used to set the browserCrossClicked.
                           
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/09/13/2005			  Bikash                                       Created   
*******************************************************************************/

/*window.onbeforeunload = function()
{
  if((window.event.clientX<0) || (window.event.clientY<0)) 
  {
        try
        {
            document.forms[0].elements["browserCrossClicked"].value = "1";
            document.forms[0].submit();
        }
        catch(e)
        {
        }
  }
}*/

/*******************************************************************************
/Function Name   :  ClosePopUpPage
/
/Description     :  changes the height of the id depending on sreen resolution
/                   
/                   
/Input Parameter :  id,hgt
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function ClosePopUpPage() {
    window.returnValue = false;
    window.close();
}

//MOD007 Start
/*******************************************************************************
/Function Name   :  IsPopupPage
/
/Description     :  Returns flag depending upon the window opening in pop-up mode or not
/                   
/                   
/Input Parameter :  id,hgt
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function IsPopUpPage() {
    if (OpenedMasterPage == 'MasterPage') {
        return false;
    }
    else {
        return true;
    }
}
//MOD007 End
/*******************************************************************************
/Function Name   :  ChangeHeightForServerControl
/
/Description     :  Function selects the value in Dropdown
/                   if IsText is true comparison is done on Text otherwise value
/                   
/Input Parameter :  ctrl,value,isText
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function SelectDataInDropDown(ctrl, value, isText) {
    if (ctrl == null) return;
    var opts = ctrl.options;
    var selIndex = -1;

    if (isText) {
        for (var i = 0; i < opts.length; i++) {
            if (opts[i].innerText == value) {
                selIndex = i;
                break;
            }
        }
    }
    else {
        for (var i = 0; i < opts.length; i++) {
            if (opts[i].value == value) {
                selIndex = i;
                break;
            }
        }
    }
    if (selIndex >= 0) {
        if (ctrl.selectedIndex >= 0) {
            ctrl.options[ctrl.selectedIndex].selected = false;
        }
        ctrl.selectedIndex = selIndex;
    }
}

/*******************************************************************************
/Function Name   :  ChangeCharToUpper
/
/Description     :  This function is fired when on key up of editable fields
/                   to change characters to uppercase.
/                   
/Input Parameter :  para
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
//MOD002-Start
function ChangeCharToUpper(para) {
    var index, count;
    var newstring = '';
    for (index = 0; index < para.length; index++) {
        count = para.substring(index, index + 1);
        count = count.toUpperCase();
        newstring = newstring + count;
    }
    return newstring;
}

/*******************************************************************************
/Function Name   :  UpperCase
/
/Description     :  This function is fired when on key up of editable fields
/                   to change string to uppercase.
/                   
/                   
/Input Parameter :  control
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function UpperCase(control) {
    /*if(event.keyCode >= 64 && event.keyCode <=90)        
      {    
       var name=document.getElementById(control);
       if( name!=null)
        {
        var para=name.value;
        name.value=ChangeCharToUpper(para);
        }
      }*/
    if (event.keyCode <= 97 && event.keyCode >= 122) {
        /*var name=document.getElementById(control);
        if( name!=null)
         {
         var para=name.value;
         name.value=ChangeCharToUpper(para);
         }*/
        name.value = name.value.toUpperCase();
        //event.keyCode = event.keyCode - 32;
    }
}

/*******************************************************************************
/Function Name   :  ConvertToUpperCase
/
/Description     :  This function converts the alphabets entered into upper case
/                   
/                   
/Input Parameter :  control id as string.
/
/Return          :  None
/
/Special Note    :  This function needs to be added as an attribute to the control.
/                   Two events "onattrmodified" and "onpropertychange" needs 
/                   to be called simulateously for use.        
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function ConvertToUpperCase(controlID) {
    var cntrId = document.getElementById(controlID);
    if (cntrId != null) {
        if (/[a-z]/.test(cntrId.value)) {
            cntrId.value = cntrId.value.toUpperCase();
        }
    }
}

//MOD002-End   
//MOD003
/*******************************************************************************
/Function Name   :  TabNext
/
/Description     :  Function to auto-tab phone field
/                   
/                   
/Input Parameter :   obj   - The input object (this)
/                    event - Either 'up' or 'down' depending on the keypress event
/                    len   - Max length of field - tab when input reaches this length
/                    next_field - input object to get focus after this one
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
var phone_field_length = 0;
function TabNext(obj, event, len, next_field) {
    if (event == "down") {
        phone_field_length = obj.value.length;
    }
    else if (event == "up") {
        if (obj.value.length != phone_field_length) {
            phone_field_length = obj.value.length;
            if (phone_field_length == len) {
                next_field = GetObjectForId(next_field);
                next_field.focus();
            }
        }
    }
}
/*******************************************************************************
/Function Name   :  CallPostBack
/
/Description     :  Call Back functions
/                   
/                   
/Input Parameter :  eventTarget,eventArgument
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function CallPostBack(eventTarget, eventArgument) {
    __doPostBack(eventTarget, eventArgument);
}
/*******************************************************************************
/Function Name   :  OpenMiniSearch
/
/Description     :  This function will open mini search
/                   
/                   
/Input Parameter :  minisearchMode
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function OpenMiniSearch(minisearchMode) {
    if (minisearchMode == null) minisearchMode = '';
    //Unsat 1579 Start
    return ShowPopUpPage("", "ScreenId=" + SEARCH + ",MiniSearch,MiniSearchMode=" + minisearchMode, (screen.availWidth - 10), (screen.availHeight - 70), "Mini Search");
    //Unsat 1579 End
}

/*******************************************************************************
/Function Name   :  GetSelectedIndexOfGrid
/
/Description     :  This function gets the selected index of grid id passed
/
/                   
/Input Parameter :  grid id
/
/Return          :  selected index
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 10/08/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function GetSelectedIndexOfGrid(objGrid) {
    var selectedIndex = -1;
    if (objGrid != null) {
        var htmlCode = objGrid.innerHTML;
        var arrOfHTML = htmlCode.split("<TR");
        if (arrOfHTML.length > 0) {
            var totalCount = arrOfHTML.length;
            for (var count = 0; count < totalCount; count++) {
                if (arrOfHTML[count].indexOf("StyleGridSelectedRow") != (-1)) {
                    selectedIndex = count - 2;
                    break;
                }
            }
        }
    }
    return selectedIndex;
}

//MOD004 - Start
/*******************************************************************************
/Function Name   :  MarkForError
/
/Description     :  This function will take the control id and the marking indicator
/                   as input and will make visible or enable the control accordingly.
/                   
/                   
/Input Parameter :  Control Id, Marking Indicator
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 05/05/2005        Nidhish Singh                                   Created
/
*******************************************************************************/
function MarkForError(control, markIndicator) {
    var panel;
    var actualCtrl = GetObjectForId(control);
    if (actualCtrl == null || actualCtrl == 'undefined') {
        actualCtrl = control;
    }
    //Get the panel object for the control as passed.
    /* alert(control.innerText);*/
    if (actualCtrl != null) {
        panel = actualCtrl.parentElement;

    }

    //Check if the panel information is there or not.
    if (panel != null) {
        //Check if the marking indicator passed is true.
        if (markIndicator == true) {
            //Unsat 1236 Start
            var img = panel.getElementsByTagName('img');
            if (img.length > 0) {
                //MOD008 added to ignore in case of calendar control
                if (img[0].id != control + 'Image') {
                    img[0].style.display = '';
                }
            }
            //Unsat 1236 End
            panel.className = "styleErrorControl";
        }
        else {
            //Unsat 1236 Start
            var img = panel.getElementsByTagName('img');
            //UNSAT-3812 Start
            /*if(img.length==0)
            {
                return;
            }*/
            //UNSAT-3812 End
            if (img.length > 0) {
                //MOD008 added to ignore in case of calendar control
                if (img[0].id != control + 'Image') {
                    img[0].style.display = 'none';
                }
            }
            //Unsat 1236 End
            panel.className = "styleNormalErrorControl";
        }
    }
}
//MOD004 - End

/*******************************************************************************
/Function Name   :  Indicator_ClientClick
/
/Description     :  This function is called on click event of indicator icon. 
/                   Currently it opens Under Construction pop up.
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function Indicator_ClientClick(targetScreen, code) {
    if (targetScreen == "UnderConstruction") {
        UnderConstruction();
        return false;
    }

    if (targetScreen.length == 7) {
        //Unsat 1740 Start
        var returnValue = true;
        try {
            returnValue = CheckForInboxNavigatingToOtherPage();
        }
        catch (e) {
        }
        return returnValue;
        //Unsat 1740 End  
    }
    else if (targetScreen.indexOf("CACS:") != -1) {
        NavigateToCACS(targetScreen);
    }
    return false;
}

/*******************************************************************************
/Function Name   :  NavigateToCACS
/
/Description     :  This function is used to call the cacs function
/                   
/Input Parameter :  cacsUrl
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 06/24/2005          Nidhish                                      Created
*******************************************************************************/

function NavigateToCACS(cacsUrl) {
    try {
        var queryString;
        var firstIndex = cacsUrl.indexOf(":");
        queryString = cacsUrl.substr(firstIndex + 1);
        var windowName = "navigator";
        var redirectedUrl = "navigator.html?queryString=" + queryString;
        redirectedUrl += ",WindowName=" + windowName + ",CallfromC2=true";
        var options = "toolbar=no,status=yes,menubar=no,scrollbars=yes,width=5,height=5";
        navHandle = window.open("", windowName, options);
        navHandle.close();
        window.open(redirectedUrl, windowName, options);

        //navHandle=window.open(redirectedUrl,windowName,options);
        //navHandle.opener = this;
        //alert(queryString);
        //window.open(queryString,"CACSAccountNavigatorC2", options);
    }
    catch (ex) {
        alert('An error occurred in submitPage: ' + ex);
    }

    //window.open("http://localhost:4174/website5/default.aspx");
}
/*******************************************************************************
/Function Name   :  OnlyNumericInputs
/
/Description     :  This function is for only numeric input validation
/
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function OnlyNumericInputs() {
    if ((event.keyCode >= 48 && event.keyCode <= 57)
        || (event.keyCode >= 37 && event.keyCode <= 40)
        || (event.keyCode >= 96 && event.keyCode <= 105)
        || event.keyCode == 110
        || event.keyCode == 8
        || event.keyCode == 9
        || event.keyCode == 46
        || event.keyCode == 36
        || event.keyCode == 35
        || (event.shiftKey && event.keyCode == 45)
        || (event.ctrlKey && (event.keyCode == 67
            || event.keyCode == 86
            || event.keyCode == 65
            || event.keyCode == 88
            || event.keyCode == 45))) {
        if (event.shiftKey && (event.keyCode >= 48 && event.keyCode <= 57)) {
            event.returnValue = false;
            event.cancel = true;
        }
    }
    else {
        event.returnValue = false;
        event.cancel = true;
    }
}

/*******************************************************************************
/Function Name   :  OnlyAlphaNumericInputs
/
/Description     :   This function allows the user to enter only numeric and
/                    alphabet characters in the search input field.
/
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function OnlyAlphaNumericInputs() {
    if ((event.keyCode >= 65 && event.keyCode <= 90)
        || (event.keyCode >= 48 && event.keyCode <= 57)
        || (event.keyCode >= 37 && event.keyCode <= 40)
        || (event.keyCode >= 96 && event.keyCode <= 105)
        || event.keyCode == 110
        || event.keyCode == 8
        || event.keyCode == 9
        || event.keyCode == 46
        || event.keyCode == 36
        || event.keyCode == 35
        || (event.shiftKey && event.keyCode == 45)
        || (event.ctrlKey && (event.keyCode == 67
            || event.keyCode == 86
            || event.keyCode == 65
            || event.keyCode == 88
            || event.keyCode == 45))) {
        if (event.shiftKey && (event.keyCode >= 48 && event.keyCode <= 57)) {
            event.returnValue = false;
            event.cancel = true;
        }
    }
    else {
        event.returnValue = false;
        event.cancel = true;
    }
}

/*******************************************************************************
/Function Name   :  OnlyNumericAndDashInputs
/
/Description     :  This function allows the user to enter only numeric
/                   characters in the search input field.
/
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function OnlyNumericAndDashInputs() {
    if (!event.shiftKey) {
        if ((event.keyCode >= 48 && event.keyCode <= 57)
            || (event.keyCode >= 37 && event.keyCode <= 40)
            || (event.keyCode >= 96 && event.keyCode <= 105)
            || event.keyCode == 110
            || event.keyCode == 8
            || event.keyCode == 9
            || event.keyCode == 46
            || event.keyCode == 36
            || event.keyCode == 35
            || (event.shiftKey && event.keyCode == 45)
            || (event.ctrlKey && (event.keyCode == 67
                || event.keyCode == 86
                || event.keyCode == 65
                || event.keyCode == 88 || event.keyCode == 45))) {
            //Do nothing 
        }
        else {
            event.returnValue = false;
            event.cancel = true;
        }
    }
}

/*******************************************************************************
/Function Name   :  AlphaNumericWithSpace
/
/Description     :  This function allows the user to enter only numeric and
/                   alphabet characters with space in the search input field.
/
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function AlphaNumericWithSpace() {
    if (event.keyCode == 8
        || event.keyCode == 9
        || event.keyCode == 32
        || event.keyCode == 35
        || event.keyCode == 36
        || (event.keyCode >= 37 && event.keyCode <= 40)
        || event.keyCode == 45
        || (event.shiftKey && event.keyCode == 45)
        || event.keyCode == 46
        || (event.keyCode >= 48 && event.keyCode <= 57)
        || event.keyCode == 65
        || (event.keyCode >= 65 && event.keyCode <= 90)
        || (event.keyCode >= 96 && event.keyCode <= 105)
        || event.keyCode == 110
        || (event.ctrlKey && (event.keyCode == 67 || event.keyCode == 86 || event.keyCode == 88))) {
        if (event.shiftKey && (event.keyCode >= 48 && event.keyCode <= 57)) {
            event.returnValue = false;
            event.cancel = true;
        }
    }
    else {
        event.returnValue = false;
        event.cancel = true;
    }
}

/*******************************************************************************
/Function Name   :  OnlyAlphabetsInputs
/
/Description     :  This function allows the user to enter only alphabets
/                   characters in the search input field.
/
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 06/29/2005           RajSekhar                                     Created
/
*******************************************************************************/
function OnlyAlphabetsInputs() {
    if (!event.shiftKey && (event.keyCode >= 48 && event.keyCode <= 57)
        ||
        (event.keyCode >= 96 && event.keyCode <= 105)) {
        event.returnValue = false;
        event.cancel = true;
    }
    if (event.shiftKey && (event.keyCode >= 48 && event.keyCode <= 57)) {
        event.returnValue = false;
        event.cancel = true;
    }
}

/*******************************************************************************
/Function Name   :  StandardPhoneInputs
/
/Description     :  Standard input for Phone Number field.
/
/                   
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function StandardPhoneInputs() {
    if ((event.keyCode >= 37 && event.keyCode <= 39)
        || event.keyCode == 110 || event.keyCode == 8
        || event.keyCode == 9
        || event.keyCode == 46
        || event.keyCode == 36
        || (event.ctrlKey && (event.keyCode == 67
            || event.keyCode == 86
            || event.keyCode == 65
            || event.keyCode == 88))
        || (event.shiftKey && (event.keyCode >= 49 && event.keyCode <= 56))
        || (event.keyCode >= 65 && event.keyCode <= 90)) {
        if (!(event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 35)) {
            event.returnValue = false;
            event.cancel = true;
        }
    }
}

/*******************************************************************************
/Function Name   :  CompareDates
/
/Description     :  This function returns the difference between two given dates.
/
/                   
/Input Parameter :  firstDate, secondDate
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function CompareDates(firstDate, secondDate) {
    datDate1 = Date.parse(firstDate);
    datDate2 = Date.parse(secondDate);
    if (firstDate == "") {
        return true;
    }
    if (datDate1 >= datDate2) {
        return true;
    }
    else {
        return false;
    }
}

/*******************************************************************************
/Function Name   :  DateDifference
/
/Description     :  This function returns the difference between two given dates.
/
/                   
/Input Parameter :  firstDate, secondDate
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function DateDifference(fromDate, toDate) {
    datDate1 = Date.parse(fromDate);
    datDate2 = Date.parse(toDate);
    dateDiff = ((datDate2 - datDate1) / (24 * 60 * 60 * 1000));
    return dateDiff;
}

/*******************************************************************************
/Function Name   :  CompareDate
/
/Description     :  Date1 is in the form mm/yyyy and date2 is in the form dd/mm/yyyy
/                   1. Compare the year. if the year1 < year2 return false
/                   2. else compare month1
/      
/Input Parameter :  date1,date2
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function CompareDate(date1, date2) {
    //Date1 is in the form mm/yyyy and date2 is in the form dd/mm/yyyy
    //1. Compare the year. if the year1 < year2 return false
    //2. else compare month1 
    var strOfDate1 = date1;
    var strOfDate2 = date2;

    //Get the months
    var monthOfDate1 = strOfDate1.substr(0, 2);
    var monthOfDate2 = strOfDate2.substr(0, 2);

    //Get the Years
    var yearOfDate1 = strOfDate1.substr(3, 4);
    var yearOfDate2 = strOfDate2.substr(3, 4);



    if (eval(yearOfDate1) > eval(yearOfDate2)) {
        return true;
    }
    else {
        //So Year1 <=Year2 
        if (eval(yearOfDate1) == eval(yearOfDate2)) {
            //Check for month
            if (eval(monthOfDate1) >= eval(monthOfDate2)) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            //Control comes here if the Year1 < Year 2, which is not allowed
            return false;
        }
    }
}

/*******************************************************************************
/Function Name   :  daysInFebruary
/
/Description     :  This function takes input year and returns the number of days
/                   in february month of that year
/      
/Input Parameter :  year
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function daysInFebruary(year) {
    // February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
}

/*******************************************************************************
/Function Name   :  isInteger
/
/Description     :  This function validates for numeric characters
/                   
/      
/Input Parameter :  year
/
/Return          :  None
/
/Modification Log:
/
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 04/29/2005             Infosys                                    Created
*******************************************************************************/
function isInteger(input) {
    var len;
    for (len = 0; len < input.length; len++) {
        // Check that current character is number.
        var currChar = input.charAt(len);
        if (((currChar < "0") || (currChar > "9")))
            return false;
    }
    // All characters are numbers.
    return true;
}

/*******************************************************************************
/Function Name   :  AutoTab
/
/Description     :  This function sets the cursor focus on second textbox field
/                   when the first textbox max limit of digits are entered
/
/Input Parameter :  None
/
/Return          :  None
/
/Modification Log:
/ Date(mm/dd/yyyy)    Modified By       Change Request#            Description
/ 06/29/2005           RajSekhar                                     Created
/
*******************************************************************************/
function AutoTab(txtInFocus, txtOutFocus, tabLength) {
    //UNSAT2168 - Start
    if (TabOut == false)
        return;
    //UNSAT2168 - End

    var contrInFocus = document.getElementById(txtInFocus);
    var contrOutFocus = document.getElementById(txtOutFocus);
    if ((event.keyCode >= 48 && event.keyCode <= 57)
        || (event.keyCode >= 96 && event.keyCode <= 105)) {
        if (contrInFocus != null && contrOutFocus != null) {
            if (contrInFocus.value.length >= tabLength) {
                contrInFocus.value = contrInFocus.value.substring(0, tabLength);
                contrOutFocus.focus();
            }
        }
    }
}

//UNSAT2168 - Start
/*******************************************************************************
/Function Name   :  RestrictAutoTab
/
/Description     :  This function restrict the tab out.
*******************************************************************************/
var TabOut = true;
function RestrictAutoTab() {
    TabOut = false;
}
/*******************************************************************************
/Function Name   :  AllowAutoTab
/
/Description     :  This function allow tab out functionality.
*******************************************************************************/
function AllowAutoTab() {
    TabOut = true;
}
//UNSAT2168 - End
//MOD005-Start
/*******************************************************************************
/Function Name   :  GetMasterPageId
/
/Description     :  This function gets the master page id.
*******************************************************************************/
var __masterPageId = null;
function GetMasterPageId() {
    if (__masterPageId == null) {
        __masterPageId = document.getElementById("masterPageId").value;
    }
    return __masterPageId;
}
/*******************************************************************************
/Function Name   :  GetDirtyFlagId
/
/Description     :  This function gets the dirty flag id.
*******************************************************************************/
var __dirtyFlagId = null;
function GetDirtyFlagId() {
    if (__dirtyFlagId == null) {
        var masterId = GetMasterPageId();
        if (masterId != null) {
            __dirtyFlagId = document.getElementById(masterId + "_DirtyFlagHidden");
        }
    }

    return __dirtyFlagId;
}

/*******************************************************************************
/Function Name   :  SetDirtyFlag
/
/Description     :  This function sets the hidden variable on data change.
*******************************************************************************/
function SetDirtyFlag() {
    var dirtyFlagObject = GetDirtyFlagId();
    if (dirtyFlagObject != null) {
        dirtyFlagObject.value = 'Y';
    }
}
/*******************************************************************************
/Function Name   :  IsDirtyFlagSet
/
/Description     :  This checks the status of Dirty Flag.
*******************************************************************************/
function IsDirtyFlagSet() {
    var dirtyFlagObject = GetDirtyFlagId();
    if (dirtyFlagObject != null && dirtyFlagObject.value.length > 0) {
        return true;
    }
    return false;
}

/*******************************************************************************
/Function Name   :  SetDoneMessageReturn
/
/Description     :  This function sets the Done Message Return value
*******************************************************************************/
function SetDoneMessageReturn(value) {
    var masterId = GetMasterPageId();
    var isSaveChangesObject = GetObjectForId(masterId + '_IsSaveChanges');
    if (value == '' || value == 'Yes' || value == 'No') {
        isSaveChangesObject.value = value;
    }
}
/*******************************************************************************
/Function Name   :  PerformDoneOperation
/
/Description     :  This function decides the navigation on DONE button.
*******************************************************************************/
function PerformDoneOperation() {
    if (IsDirtyFlagSet()) {
        var masterId = GetMasterPageId();
        var isSaveChangesObject = GetObjectForId(masterId + '_IsSaveChanges');
        var message;
        var messageBoxId;

        if (MESSAGE_BOX_ID != '' && ERROR_CODE != '') {
            message = GetFriendlyMessage(ERROR_CODE);
            messageBoxId = MESSAGE_BOX_ID;
        }
        else {
            message = GetFriendlyMessage(EC_6241);
            messageBoxId = masterId + "_DoneMessageBox";
        }
        ModifyInformation(messageBoxId, message, "", "Question");
        doneStatusMessage = ShowMessageBox(messageBoxId);
        var returnOfProcessFunction = true;
        try {

            returnOfProcessFunction = eval("ProcessAfterDoneClick")(doneStatusMessage);
        }
        catch (e) {
        }
        if (doneStatusMessage == "Yes" && returnOfProcessFunction) {
            isSaveChangesObject.value = 'Yes';
            return returnOfProcessFunction;
        }
        else if (doneStatusMessage == "No" && returnOfProcessFunction) {
            isSaveChangesObject.value = 'No';
            return returnOfProcessFunction;
        }
        else {
            return false;
        }
    }
    else {
        return true;
    }
}
//MOD007 Start
/*******************************************************************************
/Function Name   :  PerformCancelOperation
/
/Description     :  This function performs the operation on Cancel button click.
*******************************************************************************/
function PerformCancelOperation() {
    if (IsPopUpPage()) {
        ClosePopUpPage();
    }
    else {
        return true;
    }
}
//MOD007 End
//MOD005-End
//MOD006 - Start

var MESSAGE_BOX_ID = '';
var ERROR_CODE = '';
var RETURN_ON_NO_BUTTON = true;
/*******************************************************************************
/Function Name   :  SetDoneErrorMessage
/
/Description     :  This function sets the message box id and error code.
*******************************************************************************/
function SetDoneErrorMessage(messageBoxId, errorCode, isReturnOnNoButton) {
    MESSAGE_BOX_ID = messageBoxId;
    ERROR_CODE = eval(errorCode);
    RETURN_ON_NO_BUTTON = isReturnOnNoButton;
}

/*******************************************************************************
/Function Name   :  EnableDisablePageButton
/
/Description     :  This function enables and disables the button.
*******************************************************************************/
function EnableDisablePageButton(buttonList) {
    var buttonControlList = buttonList.split(',');
    var controlID = GetControlId();
    if (controlID != '') {
        for (var count = 0; count < buttonControlList.length; count++) {
            var arrButton = buttonControlList[count].split(':');
            if (arrButton.length == 2) {
                var btnCntrl = document.getElementById(controlID + "_" + arrButton[0]);
                if (btnCntrl != null) {
                    if (arrButton[1] == 'false') {
                        EnableDisableButton(btnCntrl, false);
                    }
                    else {
                        EnableDisableButton(btnCntrl, true);
                    }
                }

            }
        }
    }
}

// The function opens the Maintenance Log for the given Big five level
function OpenMaintenanceLog(screenTitle, bigFiveLevel, bigFiveLevelId) {
    var retValue;
    if (screen.availWidth >= 1024) {
        retValue = ShowPopUpPage('MaintenanceLog', 'Level=' + bigFiveLevel + ',BigFiveLevelId=' + bigFiveLevelId, 650, 340, screenTitle);
    }
    else {
        retValue = ShowPopUpPage('MaintenanceLog', 'Level=' + bigFiveLevel + ',BigFiveLevelId=' + bigFiveLevelId, 650, 340, screenTitle);
    }
    return retValue;
}

// The function opens the Address Maintenance
function OpenAddressMaintenance(screenTitle, addressParameters) {
    var retValue;
    if (screen.availWidth >= 1024) {
        retValue = ShowPopUpPage('Address', addressParameters, 990, 340, screenTitle);
    }
    else {
        retValue = ShowPopUpPage('Address', addressParameters, 770, 378, screenTitle);
    }
    return retValue;
}


//MOD009 - Start

/*******************************************************************************
/Function Name   :  HandleCopyPaste
/
/Description     :  This function will handle copy paste validations and formatting
/                   in textboxes
*******************************************************************************/
function CallHandleCopyPaste(pasteControl, operationLength) {
    var PasteControl = document.getElementById(pasteControl);

    operationLength = parseInt(operationLength);

    if (PasteControl != null) {
        var pasteData = window.clipboardData.getData("Text");
        pasteData = Trim(pasteData);

        if (pasteData.length <= operationLength) {
            PasteControl.value = pasteData;
        }
        else {
            PasteControl.value = pasteData.substring(0, operationLength);
        }
        return false;

        if (pasteData.length <= operationLength) {
            pasteData = window.clipboardData.setData('Text', pasteData.substring(0, operationLength));
        }
        else {
            pasteData = window.clipboardData.setData('Text', pasteData);
        }
        return true;
    }
    else {
        return false;
    }
}

//MOD009 - End
ImportJavaScript('NU.C2.Controls/ButtonImageChange.js');
ImportJavaScript('NU.C2.Controls/Calendar.js');
ImportJavaScript('NU.C2.Controls/ClientCallBack.js');
ImportJavaScript('NU.C2.Controls/CrossValidator.js');
ImportJavaScript('NU.C2.Controls/DropDownList.js');
ImportJavaScript('NU.C2.Controls/ErrorDisplay.js');
ImportJavaScript('NU.C2.Controls/FlagDisplay.js');
ImportJavaScript('NU.C2.Controls/MessageBox.js');
ImportJavaScript('NU.C2.Controls/Panel.js');
ImportJavaScript('NU.C2.Controls/TabControl.js');
ImportJavaScript('NU.C2.Controls/TextBox.js');
ImportJavaScript('NU.C2.Controls/TreeView.js');
ImportJavaScript('ScreenId.js');

//Imports javascript files to the page
function ImportJavaScript(path) {
    var base = "Javascripts/";
    document.write("<" + "script src=\"" + base + path + "\"></" + "script>\n");

}
//MOD010-Start
/*******************************************************************************
/Function Name   :  FormatAmount
/
/Description     :  The function formats the amount field
*******************************************************************************/
function FormatAmount(total) {
    var returnVal = CurrencyFormatted(total);
    returnVal = CommaFormatted(returnVal);
    return returnVal;

}
//MOD010-End

//MOD011 - Start

/*******************************************************************************
/Function Name   :  CallProcessBeforeCallBack
/
/Description     :  The function which restores the hidden data during Grid Call Back.
*******************************************************************************/
function CallProcessBeforeCallBack() {
    if (__theFormPostData != null) {
        __theFormPostData = '';
        WebForm_InitCallback();
    }

}

//MOD011 - End

//MOD001-R78
function CheckForLegalindicatorIsSet(screenId, legalIndicatorValue) {
    try {
        var masterId = GetMasterPageId();
        var headerID = document.getElementById("headerID").value;
        var roleId = "";
        if (CheckForScreenId(screenId)) {
            if (masterId != null) {
                roleId = GetObjectForId(masterId + "_RoleIsOfMarketer");
            }

            if (IsPopUpPage() && legalIndicatorValue == null) {
                return true;
            }
            else {
                if (!IsPopUpPage()) {
                    var legalIndicator = GetObjectForId(headerID + "_LegelIndicatorSet");
                    legalIndicatorValue = legalIndicator.value;
                }

                if (legalIndicatorValue == FLAG_YES
                    && roleId.value == FLAG_YES) {
                    return ShowmessageForLegal();
                }
                return true;
            }
        }
        else {
            return true;
        }
    }
    catch (e) {
        return true;
    }

}

function ShowmessageForLegal() {
    var message = GetFriendlyMessage(EC_10060);
    var masterId = GetMasterPageId();
    messageBoxId = masterId + "_LegalIndicatorMessageBox";
    ModifyInformation(messageBoxId, message, "", "Question");
    var doneStatusMessage = ShowMessageBox(messageBoxId);
    if (doneStatusMessage == "Yes") {
        return true;
    }
    else {
        return false;
    }
}

function CheckForScreenId(screenId) {
    if (screenId == null) {
        return false;
    }
    else {
        if
            (
            screenId == SI_ADJUSTMENT ||
            screenId == SI_BANK_CORRECTION ||
            screenId == SI_REFUND ||
            screenId == SI_DEPOSIT_REFUND ||
            screenId == SI_TRANSFER_ACCOUNT_BALANCE ||
            screenId == SI_TRANSFER_PAYMENT ||
            screenId == SI_REDIRECT_PAYMENT ||
            screenId == SI_REDIRECT_ACCOUNT_BALANCE ||
            screenId == SI_EARMARK_PAYMENT ||
            screenId == SI_DEPOSIT_REQUEST ||
            screenId == SI_DEPOSIT_ADJUSTMENT ||
            screenId == SI_DEPOSIT_RECEIPTS ||
            screenId == SI_ESTIMATE_DEPOSIT ||
            screenId == SI_MANUAL_DISCONNECT_RECONNECT ||
            screenId == SI_MOVEIN_ORDER_DETAILS ||
            screenId == SI_MOVEIN_PRODUCT_AND_SERVICES ||
            screenId == SI_MOVEIN_RECAP ||
            screenId == SI_MOVEIN_SERVICE_DETAILS ||
            screenId == SI_MOVEOUT_ORDER_DETAILS ||
            screenId == SI_MOVEOUT_RECAP ||
            screenId == SI_MOVEOUT_SERVICE_DETAILS ||
            screenId == SI_GAS_TROUBLE_ORDER ||
            screenId == SI_ELECTRIC_TROUBLE_ORDER ||
            screenId == SI_METER_ORDER_DETAILS
        ) {
            return true;
        }
    }
}
//MOD001-R78-End