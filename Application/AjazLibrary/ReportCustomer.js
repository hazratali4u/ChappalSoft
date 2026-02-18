$(document).ready(function () {
    $("[id$='rblType']").change(function () {
        document.getElementById('childPage_ddlCustomer').disabled = false;
        document.getElementById('dvFromDate').style.display = 'block';
        document.getElementById('dvCustomer').style.display = 'block';
        var saletype = 1;
        var radios = $("[id$='rblType']").find("input[type='radio']");
        for (var i = 0; i < radios.length; i++) {
            if (radios[i].checked) {
                saletype = parseInt(radios[i].value);
            }
        }
        if (saletype == 2) {
            document.getElementById('childPage_ddlCustomer').disabled = true;
            document.getElementById('dvCustomer').style.display = 'none';
            document.getElementById('dvFromDate').style.display = 'none';
        }

    });
});
function ShowLedgerRport()
{
    var customerID = $("[id$='ddlCustomer']").val();
    var fromDate = $("[id$='txtFromDate']").val();
    var toDate = $("[id$='txtToDate']").val();
    var repotType = 1;
    var radios = $("[id$='rblType']").find("input[type='radio']");
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            repotType = parseInt(radios[i].value);
        }
    }

    $.ajax
        ({
            type: "POST", //HTTP method
            url: "rptCustomer.aspx/ShowLedgerRport", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ CustomerID: customerID, FromDate: fromDate, ToDate: toDate, ReportType: repotType }),
            success: LoadLedgerRport,
        });
}

function LoadLedgerRport(dtReport) {    
    dtReport = $.parseJSON(dtReport.d);
    var dtOpening = dtReport.Table1;
    dtReport = dtReport.Table;

    var repotType = 1;
    var radios = $("[id$='rblType']").find("input[type='radio']");
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            repotType = parseInt(radios[i].value);
        }
    }


    $('#lblCustomerLedger').text('Customer: ' + $("[id$='ddlCustomer'] option:selected").text());
    $('#lblFromDateLedger').text('From Date: ' + $("[id$='txtFromDate']").val());
    $('#lblToDateLedger').text('To Date: ' + $("[id$='txtToDate']").val());

    $('#dtCustomerLedger').hide();
    $('#dtCustomerBalance').hide();
    $("#itemDetailLedger").empty();
    $('#itemDetailCustomerBalance').empty();

    if (repotType == 1) {
        $('#dtCustomerLedger').show();
        var debit = 0;
        var credit = 0;
        var balance = 0;
        if (dtOpening.length > 0) {
            balance = dtOpening[0].OpeningBalance;
        }
        var rowOpening = $(' <tr><td style="width:15%;"><h5></h5></td><td style="width:40%;"><h5></h5></td><td style="width:15%;text-align:right;"><h5></h5></td><td style="width:15%;text-align:right;"><h5></h5></td><td style="width:15%;text-align:right"><h5>' + balance + '</h5></td></tr>');
        $("#itemDetailLedger").append(rowOpening);
        for (var i = 0, len = dtReport.length; i < len; i++) {
            debit += dtReport[i].Debit;
            credit += dtReport[i].Credit;
            balance += (dtReport[i].Debit - dtReport[i].Credit);
            var row = $(' <tr><td style="width:15%;border: 1px solid black;"><h5>' + formatMSJsonDate(dtReport[i].DocDate) + '</h5></td><td style="width:40%;border: 1px solid black;"><h5>' + dtReport[i].Remarks + '</h5></td><td style="width:15%;border: 1px solid black;text-align:right;"><h5>' + dtReport[i].Debit + '</h5></td><td style="width:15%;border: 1px solid black;text-align:right;"><h5>' + dtReport[i].Credit + '</h5></td><td style="width:15%;border: 1px solid black;text-align:right"><h5>' + balance + '</h5></td></tr>');
            $("#itemDetailLedger").append(row);
        }
        $('#lblDebitLedger').text(debit);
        $('#lblCreditLedger').text(credit);
        $('#lblBalanceLedger').text(balance);
        $('#lblHeaderLedger').text('Customer Ledger');
    }
    else if (repotType == 2)
    {
        var balance = 0;
        $('#dtCustomerBalance').show();
        for (var i = 0, len = dtReport.length; i < len; i++) {
            balance += dtReport[i].ClosingBalance;
            var row = $(' <tr><td style="width:60%;border: 1px solid black;"><h5>' + dtReport[i].Name + '</h5></td><td style="width:40%;border: 1px solid black;text-align:right;"><h5>' + dtReport[i].ClosingBalance + '</h5></td></tr>');
            $("#itemDetailCustomerBalance").append(row);
        }
        $('#lblBalanceTotal').text(balance);
        $('#lblHeaderLedger').text('Customer Balance Report');
    }
    $.print("#dvCustomerLedger");
}

function formatMSJsonDate(input) {
    let timestamp;

    if (typeof input === 'string') {
        // Match /Date(1745780400000)/
        const match = /\/Date\((\d+)\)\//.exec(input);
        if (match) {
            timestamp = parseInt(match[1], 10);
        } else if (!isNaN(Date.parse(input))) {
            // ISO string (e.g., 2025-04-29T00:00:00)
            return formatDate(new Date(input));
        } else {
            return "Invalid Date";
        }
    } else if (typeof input === 'number') {
        timestamp = input;
    }

    if (!timestamp || isNaN(timestamp)) return "Invalid Date";

    return formatDate(new Date(timestamp));
}

function formatDate(date) {
    const day = String(date.getDate()).padStart(2, '0');
    const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
                        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    const month = monthNames[date.getMonth()];
    const year = date.getFullYear();

    return `${day}-${month}-${year}`;
}