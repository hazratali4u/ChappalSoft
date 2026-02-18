$(document).ready(function () {
    $("[id$='ddlCategory']").change(function () {
        LoadItems();
    });

    $("[id$='rblType']").change(function () {
        document.getElementById('dvCategory').style.display = 'none';
        document.getElementById('dvItem').style.display = 'none';
        var saletype = 1;
        var radios = $("[id$='rblType']").find("input[type='radio']");
        for (var i = 0; i < radios.length; i++) {
            if (radios[i].checked) {
                saletype = parseInt(radios[i].value);
            }
        }
        if (saletype == 2) {
            document.getElementById('dvCategory').style.display = 'block';
            document.getElementById('dvItem').style.display = 'block';
        }
        else if (saletype == 3) {
            document.getElementById('dvCategory').style.display = 'block';
        }

    });

});
function LoadItems() {
    var categoryid = $("[id$='ddlCategory']").val();
    var lstProducts = $("[id$='hfItemIDs']").val();
    lstProducts = eval(lstProducts);
    var $itemDropdown = $("[id$='ddlItem']");
    $itemDropdown.empty();
    $itemDropdown.append($('<option>', {
        value: 0,
        text: 'All Items'
    }));
    for (var i = 0, len = lstProducts.length; i < len; ++i) {
        if (lstProducts[i].CategoryID == categoryid || categoryid == 0) {
            $itemDropdown.append($('<option>', {
                value: lstProducts[i].ItemID,
                text: lstProducts[i].Name
            }));
        }
    }

}
function ViewSalesReport() {
    var repotType = 1;
    var radios = $("[id$='rblType']").find("input[type='radio']");
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            repotType = parseInt(radios[i].value);
        }
    }
    ShowRport(repotType);
}
function ShowRport(repotType) {
    var categoryID = $("[id$='ddlCategory']").val();
    var itemID = $("[id$='ddlItem']").val();
    var fromDate = $("[id$='txtFromDate']").val();
    var toDate = $("[id$='txtToDate']").val();
    $.ajax
        ({
            type: "POST", //HTTP method
            url: "rptSales.aspx/ShowRport", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ CategoryID: categoryID, ItemID: itemID, FromDate: fromDate, ToDate: toDate, ReportType: repotType }),
            success: LoadReport,
        });
}
function LoadReport(dtReport) {
    dtReport = JSON.stringify(dtReport);
    var result = jQuery.parseJSON(dtReport.replace(/&quot;/g, '"'));
    dtReport = eval(result.d);

    var repotType = 1;
    var radios = $("[id$='rblType']").find("input[type='radio']");
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            repotType = parseInt(radios[i].value);
        }
    }
    $('#lblCategorySales').text('Category: ' + $("[id$='ddlCategory'] option:selected").text());
    $('#lblItemSales').text('Item: ' + $("[id$='ddlItem'] option:selected").text());
    $('#lblFromDateSales').text('From Date: ' + $("[id$='txtFromDate']").val());
    $('#lblToDateSales').text('To Date: ' + $("[id$='txtToDate']").val());


    $('#tblSalesSummary').hide();
    $('#tblSalesDetail').hide();
    $("#itemDetailSales").empty();    
    var amount = 0;
    var qty = 0;
    var totalamount = 0;
    for (var i = 0, len = dtReport.length; i < len; i++) {
        amount = dtReport[i].Price * dtReport[i].Quantity;
        totalamount += dtReport[i].Amount;
        qty += dtReport[i].Quantity;
        if (repotType == 1)
        {
            var row = $(' <tr><td style="width:40%;border: 1px solid black;"><h5>' + formatMSJsonDate(dtReport[i].TimeStamp) + '</h5></td><td style="width:30%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Quantity + '</h5></td><td style="width:30%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Amount + '</h5></td></tr>');
        }
        else if (repotType == 2 || repotType == 3 || repotType == 5) {
            var row = $(' <tr><td style="width:40%;border: 1px solid black;"><h5>' + dtReport[i].Name + '</h5></td><td style="width:30%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Quantity + '</h5></td><td style="width:30%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Amount + '</h5></td></tr>');
        }
        else if (repotType == 4) {
            var row = $(' <tr><td style="width:40%;border: 1px solid black;"><h5>' + dtReport[i].SaleType + '</h5></td><td style="width:30%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Quantity + '</h5></td><td style="width:30%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Amount + '</h5></td></tr>');
        }
        $("#itemDetailSales").append(row);
    }

    if (repotType == 1) {
        $('#lblColumnName').text('Date');
        $('#lblQtySaleSummary').text(qty);
        $('#lblAmountSaleSummary').text(totalamount);
        $('#lblHeaderSales').text('Date Wise Sales Report');
        $('#tblSalesSummary').show();
    }
    else if (repotType == 2 ) {
        $('#lblColumnName').text('Item');
        $('#lblQtySaleSummary').text(qty);
        $('#lblAmountSaleSummary').text(totalamount);
        $('#lblHeaderSales').text('Item Wise Sales Report');
        $('#tblSalesSummary').show();
    }
    else if (repotType == 3) {
        $('#lblColumnName').text('Category');
        $('#lblQtySaleSummary').text(qty);
        $('#lblAmountSaleSummary').text(totalamount);
        $('#lblHeaderSales').text('Category Wise Sales Report');
        $('#tblSalesSummary').show();
    }
    else if (repotType == 4) {
        $('#lblColumnName').text('Sale Type');
        $('#lblQtySaleSummary').text(qty);
        $('#lblAmountSaleSummary').text(totalamount);
        $('#lblHeaderSales').text('Sale Type Wise Sales Report');
        $('#tblSalesSummary').show();
    }
    else if (repotType == 5) {
        $('#lblColumnName').text('Customer Name');
        $('#lblQtySaleSummary').text(qty);
        $('#lblAmountSaleSummary').text(totalamount);
        $('#lblHeaderSales').text('Customer Wise Sales Report');
        $('#tblSalesSummary').show();
    }
    $.print("#dvSales");
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