$(document).ready(function () {
    $("[id$='rblType']").change(function () {
        document.getElementById('dvCustomer').style.display = 'none';
        document.getElementById('dvSupplier').style.display = 'none';
        var reporttype = 1;
        var radios = $("[id$='rblType']").find("input[type='radio']");
        for (var i = 0; i < radios.length; i++) {
            if (radios[i].checked) {
                reporttype = parseInt(radios[i].value);
            }
        }
        if (reporttype == 1 || reporttype == 4) {
            document.getElementById('dvCustomer').style.display = 'block';
        }
        else if (reporttype == 3) {
            document.getElementById('dvSupplier').style.display = 'block';
        }
    });
});
function ShowRport() {
    var fromDate = $("[id$='txtFromDate']").val();
    var toDate = $("[id$='txtToDate']").val();
    var reporttype = 1;
    var viewtype = $("[id$='ddlType']").val();
    var radios = $("[id$='rblType']").find("input[type='radio']");
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            reporttype = parseInt(radios[i].value);
        }
    }
    var partyID = 0;
    if (reporttype == 1 || reporttype == 4) {
        partyID = $("[id$='ddlCustomer']").val();
    }
    else if (reporttype == 3) {
        partyID = $("[id$='ddlSupplier']").val();
    }

    $.ajax
        ({
            type: "POST", //HTTP method
            url: "rptDocument.aspx/ShowDocumentReport", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ PartyID: partyID, FromDate: fromDate, ToDate: toDate, ReportType: reporttype, ViewType:viewtype }),
            success: LoadDocReport,
        });
}
function LoadDocReport(dtReport) {
    dtReport = JSON.stringify(dtReport);
    var result = jQuery.parseJSON(dtReport.replace(/&quot;/g, '"'));
    dtReport = eval(result.d);

    var reportType = 1;
    var viewtype = $("[id$='ddlType']").val();
    var radios = $("[id$='rblType']").find("input[type='radio']");
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            reportType = parseInt(radios[i].value);
        }
    }
    if (reportType == 1) {        
        $('#lblHeader').text('Document Wise Sales (Wholesale) Report');
        $('#lblFromDate').text('From Date: ' + $("[id$='txtFromDate']").val());
        $('#lblToDate').text('To Date: ' + $("[id$='txtToDate']").val());
        if (viewtype == 1) {
            var groupedData = {};

            dtReport.forEach(function (row) {
                if (!groupedData[row.SaleID]) {
                    groupedData[row.SaleID] = {
                        InvoiceNo: row.InvoiceNo,
                        CustomerName: row.Name,
                        TimeStamp: row.TimeStamp,
                        Discount: row.Discount,
                        SaleType: row.SaleType,
                        ContactNo: row.ContactNo,
                        SaleID: row.SaleID,
                        Items: []
                    };
                }

                groupedData[row.SaleID].Items.push({
                    ItemName: row.ItemName,
                    Quantity: row.Quantity,
                    Price: row.Price,
                    Amount: row.Amount
                });
            });

            var invoices = Object.values(groupedData);

            $("#documentDetail").empty();
            invoices.forEach(function (inv, index) {
                var dayname = getDayName(inv.TimeStamp);
                var InvoiceChar = dayname.charAt(0);
                var dummyinvoiceno = 1718 + inv.SaleID;
                var html = '<div class="invoice-page">'; // wrapper for page
                html += '<table>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>BILL TO</h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '<h5>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;INVOICE #</h5>';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '<h5>DATE</h5>';
                html += '</td>';
                html += '</tr>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>Name: <label id="lblCustomerName" style="text-decoration:underline;">' + inv.CustomerName + '</label></h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '<h5>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label id="lblInvocieNo">' + inv.InvoiceNo + InvoiceChar + dummyinvoiceno + '</label></h5>';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '<h5><label id="lblDate"></label>' + formatMSJsonDate(inv.TimeStamp) + '</h5>';
                html += '</td>';
                html += '</tr>';
                html += '</table>';
                html += '<h5>Phone: ' + inv.ContactNo + '</h5>';
                html += '<table border="1" width="100%" style="border-collapse:collapse;">';
                html += '<tr><th style="width:50%;" align="center"><h4>DESCRIPTION</h4></th><th style="width:10%;" align="right"><h4>QTY</h4></th><th style="width:20%;" align="right"><h4>PRICE</h4></th><th style="width:20%;" align="right"><h4>AMOUNT</h4></th></tr>';
                var totalqty = 0;
                var totalamount = 0;
                inv.Items.forEach(function (item) {
                    totalqty += item.Quantity;
                    totalamount += item.Amount;
                    html += '<tr>' +
                        '<td>' + item.ItemName + '</td>' +
                        '<td>' + item.Quantity + '</td>' +
                        '<td>' + item.Price + '</td>' +
                        '<td>' + item.Amount + '</td>' +
                        '</tr>';
                });
                html += '<tr>' +
                        '<td><h4>TOTAL</h4></td>' +
                        '<td>' + totalqty + '</td>' +
                        '<td></td>' +
                        '<td>' + totalamount + '</td>' +
                        '</tr>';
                html += '</table>';
                html += '</div>';
                html += '<br />';
                html += '<br />';
                $("#documentDetail").append(html);
            });
        }
        else {
            var groupedData = {};

            dtReport.forEach(function (row) {
                if (!groupedData[row.SaleID]) {
                    groupedData[row.SaleID] = {
                        InvoiceNo: row.InvoiceNo,
                        CustomerName: row.Name,
                        TimeStamp: row.TimeStamp,
                        Discount: row.Discount,
                        SaleType: row.SaleType,
                        ContactNo: row.ContactNo,
                        SaleID: row.SaleID,
                        Items: []
                    };
                }

                groupedData[row.SaleID].Items.push({
                    ItemName: row.ItemName,
                    ColorName:row.ColorName,
                    SizeName:row.SizeName,
                    Quantity: row.Quantity,
                    Price: row.Price,
                    Amount: row.Amount
                });
            });

            var invoices = Object.values(groupedData);

            $("#documentDetail").empty();
            invoices.forEach(function (inv, index) {
                var dayname = getDayName(inv.TimeStamp);
                var InvoiceChar = dayname.charAt(0);
                var dummyinvoiceno = 1718 + inv.SaleID;
                var html = '<div class="invoice-page">'; // wrapper for page
                html += '<table>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>BILL TO</h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '<h5>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;INVOICE #</h5>';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '<h5>DATE</h5>';
                html += '</td>';
                html += '</tr>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>Name: <label id="lblCustomerName" style="text-decoration:underline;">' + inv.CustomerName + '</label></h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '<h5>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label id="lblInvocieNo">' + inv.InvoiceNo + InvoiceChar + dummyinvoiceno + '</label></h5>';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '<h5><label id="lblDate"></label>' + formatMSJsonDate(inv.TimeStamp) + '</h5>';
                html += '</td>';
                html += '</tr>';
                html += '</table>';
                html += '<h5>Phone: ' + inv.ContactNo + '</h5>';
                html += '<table border="1" width="100%" style="border-collapse:collapse;">';
                html += '<tr><th style="width:30%;" align="center"><h4>DESCRIPTION</h4></th><th style="width:15%;" align="center"><h4>COLOR</h4></th><th style="width:10%;" align="center"><h4>SIZE</h4></th><th style="width:10%;" align="right"><h4>QTY</h4></th><th style="width:20%;" align="right"><h4>PRICE</h4></th><th style="width:15%;" align="right"><h4>AMOUNT</h4></th></tr>';
                var totalqty = 0;
                var totalamount = 0;
                inv.Items.forEach(function (item) {
                    totalqty += item.Quantity;
                    totalamount += item.Amount;
                    html += '<tr>' +
                        '<td>' + item.ItemName + '</td>' +
                        '<td>' + item.ColorName + '</td>' +
                        '<td>' + item.SizeName + '</td>' +
                        '<td>' + item.Quantity + '</td>' +
                        '<td>' + item.Price + '</td>' +
                        '<td>' + item.Amount + '</td>' +
                        '</tr>';
                });
                html += '<tr>' +
                        '<td><h4>TOTAL</h4></td>' +
                        '<td></td>' +
                        '<td></td>' +
                        '<td>' + totalqty + '</td>' +
                        '<td></td>' +
                        '<td>' + totalamount + '</td>' +
                        '</tr>';
                html += '</table>';
                html += '</div>';
                html += '<br />';
                html += '<br />';
                $("#documentDetail").append(html);
            });
        }
    }
    else if (reportType == 2) {

        $('#lblHeader').text('Document Wise Sales (Retail) Report');
        $('#lblFromDate').text('From Date: ' + $("[id$='txtFromDate']").val());
        $('#lblToDate').text('To Date: ' + $("[id$='txtToDate']").val());
        if (viewtype == 1) {
            var groupedData = {};
            dtReport.forEach(function (row) {
                if (!groupedData[row.SaleID]) {
                    groupedData[row.SaleID] = {
                        InvoiceNo: row.InvoiceNo,
                        TimeStamp: row.TimeStamp,
                        Discount: row.Discount,
                        SaleType: row.SaleType,
                        ContactNo: row.ContactNo,
                        SaleID: row.SaleID,
                        Items: []
                    };
                }

                groupedData[row.SaleID].Items.push({
                    ItemName: row.ItemName,
                    Quantity: row.Quantity,
                    Price: row.Price,
                    Amount: row.Amount
                });
            });

            var invoices = Object.values(groupedData);

            $("#documentDetail").empty();
            invoices.forEach(function (inv, index) {
                var dayname = getDayName(inv.TimeStamp);
                var InvoiceChar = dayname.charAt(0);
                var dummyinvoiceno = 1718 + inv.SaleID;
                var html = '<div class="invoice-page">'; // wrapper for page
                html += '<table>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;INVOICE #</h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '<h5>DATE</h5>';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '</tr>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label id="lblInvocieNo">' + inv.InvoiceNo + InvoiceChar + dummyinvoiceno + '</label></h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '<h5><label id="lblDate"></label>' + formatMSJsonDate(inv.TimeStamp) + '</h5>';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '</tr>';
                html += '</table>';
                html += '<table border="1" width="100%" style="border-collapse:collapse;">';
                html += '<tr><th style="width:50%;" align="center"><h4>DESCRIPTION</h4></th><th style="width:10%;" align="right"><h4>QTY</h4></th><th style="width:20%;" align="right"><h4>PRICE</h4></th><th style="width:20%;" align="right"><h4>AMOUNT</h4></th></tr>';
                var totalqty = 0;
                var totalamount = 0;
                inv.Items.forEach(function (item) {
                    totalqty += item.Quantity;
                    totalamount += item.Amount;
                    html += '<tr>' +
                        '<td>' + item.ItemName + '</td>' +
                        '<td>' + item.Quantity + '</td>' +
                        '<td>' + item.Price + '</td>' +
                        '<td>' + item.Amount + '</td>' +
                        '</tr>';
                });
                html += '<tr>' +
                        '<td><h4>TOTAL</h4></td>' +
                        '<td>' + totalqty + '</td>' +
                        '<td></td>' +
                        '<td>' + totalamount + '</td>' +
                        '</tr>';
                html += '</table>';
                html += '</div>';
                html += '<br />';
                html += '<br />';
                $("#documentDetail").append(html);
            });
        }
        else {
            var groupedData = {};
            dtReport.forEach(function (row) {
                if (!groupedData[row.SaleID]) {
                    groupedData[row.SaleID] = {
                        InvoiceNo: row.InvoiceNo,
                        TimeStamp: row.TimeStamp,
                        Discount: row.Discount,
                        SaleType: row.SaleType,
                        ContactNo: row.ContactNo,
                        SaleID: row.SaleID,
                        Items: []
                    };
                }

                groupedData[row.SaleID].Items.push({
                    ItemName: row.ItemName,
                    ColorName: row.ColorName,
                    SizeName:row.SizeName,
                    Quantity: row.Quantity,
                    Price: row.Price,
                    Amount: row.Amount
                });
            });

            var invoices = Object.values(groupedData);

            $("#documentDetail").empty();
            invoices.forEach(function (inv, index) {
                var dayname = getDayName(inv.TimeStamp);
                var InvoiceChar = dayname.charAt(0);
                var dummyinvoiceno = 1718 + inv.SaleID;
                var html = '<div class="invoice-page">'; // wrapper for page
                html += '<table>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;INVOICE #</h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '<h5>DATE</h5>';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '</tr>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label id="lblInvocieNo">' + inv.InvoiceNo + InvoiceChar + dummyinvoiceno + '</label></h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '<h5><label id="lblDate"></label>' + formatMSJsonDate(inv.TimeStamp) + '</h5>';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '</tr>';
                html += '</table>';
                html += '<table border="1" width="100%" style="border-collapse:collapse;">';
                html += '<tr><th style="width:30%;" align="center"><h4>DESCRIPTION</h4></th><th style="width:15%;" align="center"><h4>COLOR</h4></th><th style="width:10%;" align="center"><h4>SIZE</h4></th><th style="width:10%;" align="right"><h4>QTY</h4></th><th style="width:20%;" align="right"><h4>PRICE</h4></th><th style="width:15%;" align="right"><h4>AMOUNT</h4></th></tr>';
                var totalqty = 0;
                var totalamount = 0;
                inv.Items.forEach(function (item) {
                    totalqty += item.Quantity;
                    totalamount += item.Amount;
                    html += '<tr>' +
                        '<td>' + item.ItemName + '</td>' +
                        '<td>' + item.ColorName + '</td>' +
                        '<td>' + item.SizeName + '</td>' +
                        '<td>' + item.Quantity + '</td>' +
                        '<td>' + item.Price + '</td>' +
                        '<td>' + item.Amount + '</td>' +
                        '</tr>';
                });
                html += '<tr>' +
                        '<td><h4>TOTAL</h4></td>' +
                        '<td></td>' +
                        '<td></td>' +
                        '<td>' + totalqty + '</td>' +
                        '<td></td>' +
                        '<td>' + totalamount + '</td>' +
                        '</tr>';
                html += '</table>';
                html += '</div>';
                html += '<br />';
                html += '<br />';
                $("#documentDetail").append(html);
            });
        }
    }
    else if (reportType == 3) {

        $('#lblHeader').text('Document Wise Purchase Report');
        $('#lblFromDate').text('From Date: ' + $("[id$='txtFromDate']").val());
        $('#lblToDate').text('To Date: ' + $("[id$='txtToDate']").val());
        if (viewtype == 1) {
            var groupedData = {};

            dtReport.forEach(function (row) {
                if (!groupedData[row.SaleID]) {
                    groupedData[row.SaleID] = {
                        InvoiceNo: row.InvoiceNo,
                        CustomerName: row.Name,
                        TimeStamp: row.TimeStamp,
                        SaleType: row.SaleType,
                        ContactNo: row.ContactNo,
                        SaleID: row.SaleID,
                        Items: []
                    };
                }

                groupedData[row.SaleID].Items.push({
                    ItemName: row.ItemName,
                    Quantity: row.Quantity,
                    Price: row.Price,
                    Amount: row.Amount
                });
            });

            var invoices = Object.values(groupedData);

            $("#documentDetail").empty();
            invoices.forEach(function (inv, index) {
                var html = '<div class="invoice-page">'; // wrapper for page
                html += '<table>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>Supplier: <label id="lblCustomerName" style="text-decoration:underline;">' + inv.CustomerName + '</label></h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '</tr>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>Date: ' + formatMSJsonDate(inv.TimeStamp) + '</h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '</tr>';
                html += '</table>';
                html += '<table border="1" width="100%" style="border-collapse:collapse;">';
                html += '<tr><th style="width:50%;" align="center"><h4>DESCRIPTION</h4></th><th style="width:10%;" align="right"><h4>QTY</h4></th><th style="width:20%;" align="right"><h4>PRICE</h4></th><th style="width:20%;" align="right"><h4>AMOUNT</h4></th></tr>';
                var totalqty = 0;
                var totalamount = 0;
                inv.Items.forEach(function (item) {
                    totalqty += item.Quantity;
                    totalamount += item.Amount;
                    html += '<tr>' +
                        '<td>' + item.ItemName + '</td>' +
                        '<td>' + item.Quantity + '</td>' +
                        '<td>' + item.Price + '</td>' +
                        '<td>' + item.Amount + '</td>' +
                        '</tr>';
                });
                html += '<tr>' +
                        '<td><h4>TOTAL</h4></td>' +
                        '<td>' + totalqty + '</td>' +
                        '<td></td>' +
                        '<td>' + totalamount + '</td>' +
                        '</tr>';
                html += '</table>';
                html += '</div>';
                html += '<br />';
                html += '<br />';
                $("#documentDetail").append(html);
            });
        }
        else {
            var groupedData = {};

            dtReport.forEach(function (row) {
                if (!groupedData[row.SaleID]) {
                    groupedData[row.SaleID] = {
                        InvoiceNo: row.InvoiceNo,
                        CustomerName: row.Name,
                        TimeStamp: row.TimeStamp,
                        SaleType: row.SaleType,
                        ContactNo: row.ContactNo,
                        SaleID: row.SaleID,
                        Items: []
                    };
                }

                groupedData[row.SaleID].Items.push({
                    ItemName: row.ItemName,
                    ColorName: row.ColorName,
                    SizeName:row.SizeName,
                    Quantity: row.Quantity,
                    Price: row.Price,
                    Amount: row.Amount
                });
            });

            var invoices = Object.values(groupedData);

            $("#documentDetail").empty();
            invoices.forEach(function (inv, index) {
                var html = '<div class="invoice-page">'; // wrapper for page
                html += '<table>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>Supplier: <label id="lblCustomerName" style="text-decoration:underline;">' + inv.CustomerName + '</label></h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '</tr>';
                html += '<tr>';
                html += '<td style="width:45%;">';
                html += '<h5>Date: ' + formatMSJsonDate(inv.TimeStamp) + '</h5>';
                html += '</td>';
                html += '<td style="width:5%;"></td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '<td style="width:25%;">';
                html += '</td>';
                html += '</tr>';
                html += '</table>';
                html += '<table border="1" width="100%" style="border-collapse:collapse;">';
                html += '<tr><th style="width:30%;" align="center"><h4>DESCRIPTION</h4></th><th style="width:15%;" align="center"><h4>COLOR</h4></th><th style="width:10%;" align="center"><h4>SIZE</h4></th><th style="width:10%;" align="right"><h4>QTY</h4></th><th style="width:20%;" align="right"><h4>PRICE</h4></th><th style="width:15%;" align="right"><h4>AMOUNT</h4></th></tr>';
                var totalqty = 0;
                var totalamount = 0;
                inv.Items.forEach(function (item) {
                    totalqty += item.Quantity;
                    totalamount += item.Amount;
                    html += '<tr>' +
                        '<td>' + item.ItemName + '</td>' +
                        '<td>' + item.ColorName + '</td>' +
                        '<td>' + item.SizeName + '</td>' +
                        '<td>' + item.Quantity + '</td>' +
                        '<td>' + item.Price + '</td>' +
                        '<td>' + item.Amount + '</td>' +
                        '</tr>';
                });
                html += '<tr>' +
                        '<td><h4>TOTAL</h4></td>' +
                        '<td></td>' +
                        '<td></td>' +
                        '<td>' + totalqty + '</td>' +
                        '<td></td>' +
                        '<td>' + totalamount + '</td>' +
                        '</tr>';
                html += '</table>';
                html += '</div>';
                html += '<br />';
                html += '<br />';
                $("#documentDetail").append(html);
            });
        }
    }
    else if (reportType == 4) {

        $('#lblHeader').text('Document Wise Receipt Report');
        $('#lblFromDate').text('From Date: ' + $("[id$='txtFromDate']").val());
        $('#lblToDate').text('To Date: ' + $("[id$='txtToDate']").val());
        $("#documentDetail").empty();
        var rowHeader = $(' <tr><th style="width:15%;border: 1px solid black;" align="center"><h4>Date</h4></th><th style="width:30%;border: 1px solid black;" align="center"><h4>Customer Name</h4></th><th style="width:20%;border: 1px solid black;" align="right"><h4>Contact No</h4></th><th style="width:20%;border: 1px solid black;" align="right"><h4>Payment Mode</h4></th><th style="width:15%;border: 1px solid black;" align="right"><h4>AMOUNT</h4></th></tr>');
        $("#documentDetail").append(rowHeader);

        for (var i = 0, len = dtReport.length; i < len; i++)
        {
            var row = $(' <tr><td style="width:15%;border: 1px solid black;">' + formatMSJsonDate(dtReport[i].TimeStamp) + '</td><td style="width:30%;border: 1px solid black;">' + dtReport[i].Name + '</td><td style="width:20%;border: 1px solid black;">' + dtReport[i].ContactNo + '</td><td style="width:20%;border: 1px solid black;">' + dtReport[i].PaymentMode + '</td><td style="width:15%;border: 1px solid black;">' + dtReport[i].Amount + '</td></tr>');
            $("#documentDetail").append(row);
        }
    }
    $.print("#dvDocumentReport");
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

function getDayName(dotNetDate) {
    // Extract the number inside /Date(…)/ 
    const match = /\/Date\((\d+)\)\//.exec(dotNetDate);
    if (!match) return "";

    const timestamp = parseInt(match[1], 10);
    const date = new Date(timestamp);

    // Full weekday name (Sunday, Monday, etc.)
    return date.toLocaleString('en-GB', { weekday: 'long' });
}
