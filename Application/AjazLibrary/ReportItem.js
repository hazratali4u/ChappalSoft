$(document).ready(function () {
    $("[id$='ddlCategory']").change(function () {
        LoadItems();
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
function ViewStockReport() {
    var repotType = 1;
    var radios = $("[id$='rblType']").find("input[type='radio']");
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            repotType = parseInt(radios[i].value);
        }
    }
    ShowSummaryRport(repotType);
}
function ShowSummaryRport(repotType) {
    var categoryID = $("[id$='ddlCategory']").val();
    var itemID = $("[id$='ddlItem']").val();
    var fromDate = $("[id$='txtFromDate']").val();
    var toDate = $("[id$='txtToDate']").val();
    $.ajax
        ({
            type: "POST", //HTTP method
            url: "rptItem.aspx/ShowSummaryRport", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ CategoryID: categoryID, ItemID: itemID, FromDate: fromDate, ToDate: toDate, ReportType: repotType }),
            success: LoadSummaryRport,
        });
}
function LoadSummaryRport(dtReport) {
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

    $('#tblStockSummary').hide();
    $('#tblStockDetail').hide();
    $('#tblItemProfit').hide();
    $('#lblCategoryStock').text('Category: ' + $("[id$='ddlCategory'] option:selected").text());
    $('#lblItemStock').text('Item: ' + $("[id$='ddlItem'] option:selected").text());
    $('#lblFromDateStock').text('From Date: ' + $("[id$='txtFromDate']").val());
    $('#lblToDateStock').text('To Date: ' + $("[id$='txtToDate']").val());
    $("#itemDetailStock").empty();
    $("#itemDetailStockDetail").empty();
    $("#itemDetailItemProfit").empty();
    var opening = 0;
    var purchase = 0;
    var Sale = 0;
    var closing = 0;
    var profitTotal = 0;
    var qty = 0;
    var amount = 0;
    if (repotType == 1 || repotType == 2) {
        for (var i = 0, len = dtReport.length; i < len; i++) {
            opening += dtReport[i].Opening;
            purchase += dtReport[i].Purchased;
            Sale += dtReport[i].Sold;
            var close = dtReport[i].Opening + dtReport[i].Purchased - dtReport[i].Sold;
            closing += close;
            if (repotType == 1) {
                var row = $(' <tr><td style="width:40%;border: 1px solid black;"><h5>' + dtReport[i].ItemName + '</h5></td><td style="width:15%;border: 1px solid black;text-align:right"><h5>' + dtReport[i].Opening + '</h5></td><td style="width:15%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Purchased + '</h5></td><td style="width:15%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Sold + '</h5></td><td style="width:15%;border: 1px solid black;;text-align:right"><h5>' + close + '</h5></td></tr>');
                $("#itemDetailStock").append(row);
            }
            else if (repotType == 2) {
                var row = $(' <tr><td style="width:30%;border: 1px solid black;"><h5>' + dtReport[i].ItemName + '</h5></td><td style="width:15%;border: 1px solid black;"><h5>' + dtReport[i].ColorName + '</h5></td><td style="width:15%;border: 1px solid black;"><h5>' + dtReport[i].SizeName + '</h5></td><td style="width:10%;border: 1px solid black;text-align:right"><h5>' + dtReport[i].Opening + '</h5></td><td style="width:10%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Purchased + '</h5></td><td style="width:10%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Sold + '</h5></td><td style="width:10%;border: 1px solid black;;text-align:right"><h5>' + close + '</h5></td></tr>');
                $("#itemDetailStockDetail").append(row);
            }
        }
    }
    else if (repotType == 3)
    {
        var profit = 0;
        
        for (var i = 0, len = dtReport.length; i < len; i++)
        {
            profit = dtReport[i].Amount - dtReport[i].TotalCost;
            profitTotal += profit;
            qty += dtReport[i].Quantity;
            amount += dtReport[i].Amount;
            var row = $(' <tr><td style="width:40%;border: 1px solid black;"><h5>' + dtReport[i].ItemName + '</h5></td><td style="width:15%;border: 1px solid black;"><h5>' + dtReport[i].ColorName + '</h5></td><td style="width:15%;border: 1px solid black;"><h5>' + dtReport[i].SizeName + '</h5></td><td style="width:10%;border: 1px solid black;text-align:right"><h5>' + dtReport[i].Quantity + '</h5></td><td style="width:10%;border: 1px solid black;;text-align:right"><h5>' + dtReport[i].Amount + '</h5></td><td style="width:10%;border: 1px solid black;;text-align:right"><h5>' + profit + '</h5></td></tr>');
            $("#itemDetailItemProfit").append(row);
        }
    }
    if (repotType == 1) {
        $('#lblOpeningStockSummary').text(opening);
        $('#lblPurchaseStockSummary').text(purchase);
        $('#lblSaleStockSummary').text(Sale);
        $('#lblClosingStockSummary').text(closing);
        $('#lblHeaderStock').text('Stock Summary Report');
        $('#tblStockSummary').show();
    }
    else if (repotType == 2) {
        $('#lblOpeningStockDetail').text(opening);
        $('#lblPurchaseStockDetail').text(purchase);
        $('#lblSaleStockDetail').text(Sale);
        $('#lblClosingStockDetail').text(closing);
        $('#lblHeaderStock').text('Stock Detail Report');
        $('#tblStockDetail').show();
    }
    else if (repotType == 3) {
        $('#lblQtyItemProfit').text(qty);
        $('#lblAmountItemProfit').text(amount);
        $('#lblProfitItemProfit').text(profitTotal);
        $('#lblHeaderStock').text('Item Profit Report');
        $('#tblItemProfit').show();
    }
    $.print("#dvStock");
}