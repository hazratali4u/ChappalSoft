function printInvoice(SaleID) {
    $.ajax
        ({
            type: "POST", //HTTP method
            url: "Reprint.aspx/GetInvoice", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ SaleID: SaleID }),
            success: ShowInvoice,
        });
}
function ShowInvoice(dtSale) {    
    dtSale = JSON.stringify(dtSale);
    var result = jQuery.parseJSON(dtSale.replace(/&quot;/g, '"'));
    dtSale = eval(result.d);
    if (dtSale[0].SaleType == 1) {
        PrintOrderRetail(dtSale);
    }
    else {
        PrintOrder(dtSale);
    }
}
function PrintOrder(OrderItems) {
    const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    const timestamp = parseInt(OrderItems[0].TimeStamp.match(/\d+/)[0], 10);
    const today = new Date(timestamp);
    const day = today.getDate().toString().padStart(2, '0'); // Ensure 2-digit day
    const monthShort = today.toLocaleString('en-GB', { month: 'short' }); // "Mar"
    const year = today.getFullYear();
    const formattedDate = `${day}-${monthShort}-${year}`;
    const dayName = days[today.getDay()];
    var InvoiceChar = dayName.charAt(0);

    $('#lblDate').text(formattedDate);

    document.getElementById("br1").style.display = "none";
    document.getElementById("br2").style.display = "none";
    document.getElementById("br3").style.display = "none";
    document.getElementById("trDuplicateBill").style.display = "table-row";
    $('#lblCustomerName').text(OrderItems[0].CustomerName);
    $('#lblCustomerPhone').text(OrderItems[0].ContactNo);

    $('#trDiscount').hide();
    $('#trNetTotal').hide();
    $('#lblAddress').text($("[id$='hfAddress']").val());
    $('#lblPhone').text($("[id$='hfPhone']").val());
    $('#lblInvoiceFooterNote').text($("[id$='hfInvoiceFooterNote']").val());
    var subtotal = 0;
    var totalqty = 0;
    $("#orderDetail").empty();
    for (var i = 0, len = OrderItems.length; i < len; i++) {
        var row = $(' <tr><td><h5>' + OrderItems[i].ItemName + '</h5></td><td class="text-right"><h5>' + OrderItems[i].Quantity + '</h5></td><td class="text-right"><h5>' + OrderItems[i].Price + '</h5></td><td class="text-right"><h5>' + OrderItems[i].Amount + '</h5></td></tr>');
        subtotal += parseInt(OrderItems[i].Amount);
        totalqty += parseInt(OrderItems[i].Quantity);
        $('#orderDetail').append(row);
    }
    $('#lblTotalQTY').text(totalqty);
    $('#lblTotalAMOUNT').text(subtotal);

    if (OrderItems.length > 0) {
        var dummyinvoiceno = 1718 + OrderItems[0].SaleID;
        $('#lblInvocieNo').text(OrderItems[0].InvoiceNo + InvoiceChar + dummyinvoiceno);
        if (parseInt(OrderItems[0].Discount) > 0) {
            $('#lblTotalDiscount').text(OrderItems[0].Discount);
            $('#lblInvoiceNetTotal').text(parseInt(subtotal) - parseInt(OrderItems[0].Discount));
            $('#trDiscount').show();
            $('#trNetTotal').show();
        }
        $('#lblCustomerBalance').text('');
        $('#lblCustomerBalance').text(OrderItems[0].OpeningBalance);
        $('#lblSubTotal').text(parseInt(subtotal) + parseInt(OrderItems[0].OpeningBalance));
        $('#lblReceived').text(parseInt(subtotal) - parseInt(OrderItems[0].InvoiceBalance));
        $('#lblTotalBalance').text(parseInt(OrderItems[0].OpeningBalance) + parseInt(OrderItems[0].InvoiceBalance));
    }

    $.print("#dvPrintOrder");
}
function PrintOrderRetail(OrderItems) {
    const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    const timestamp = parseInt(OrderItems[0].TimeStamp.match(/\d+/)[0], 10);
    const date = new Date(timestamp);
    const optionsDate = { day: '2-digit', month: 'short', year: 'numeric' };
    const formattedDate = date.toLocaleDateString('en-GB', optionsDate).replace(/ /g, '-');
    const hours = date.getHours() % 12 || 12;
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const seconds = date.getSeconds().toString().padStart(2, '0');
    const formattedTime = `${hours}:${minutes}:${seconds}`;

    $('#lblDate-Retail').text(formattedDate);
    $('#lblTime-Retail').text(formattedTime);

    const dayName = days[date.getDay()];
    var InvoiceChar = dayName.charAt(0);

    document.getElementById("trDuplicateBillRetail").style.display = "table-row";

    $('#trDiscount-Retail').hide();
    $('#trNetTotal-Retail').hide();
    $('#lblAddress-Retail').text($("[id$='hfAddressShort']").val());
    $('#lblPhone-Retail').text($("[id$='hfPhone']").val());
    $('#lblInvoiceFooterNote-Retail').text($("[id$='hfInvoiceFooterNoteShort']").val());
    var subtotal = 0;
    var totalqty = 0;
    $("#orderDetail-Retail").empty();
    for (var i = 0, len = OrderItems.length; i < len; i++) {
        var row = $(' <tr><td><h5>' + OrderItems[i].ItemName + '</h5></td><td class="text-right"><h5>' + OrderItems[i].Quantity + '</h5></td><td class="text-right"><h5>' + OrderItems[i].Price + '</h5></td><td class="text-right"><h5>' + OrderItems[i].Amount + '</h5></td></tr>');
        subtotal += parseInt(OrderItems[i].Amount);
        totalqty += parseInt(OrderItems[i].Quantity);
        $('#orderDetail-Retail').append(row);
    }
    $('#lblTotalQTY-Retail').text(totalqty);
    $('#lblTotalAMOUNT-Retail').text(subtotal);

    if (OrderItems.length > 0) {
        var dummyinvoiceno = 1718 + OrderItems[0].SaleID;
        $('#lblInvocieNo-Retail').text(OrderItems[0].InvoiceNo + InvoiceChar + dummyinvoiceno);
        if (parseInt(OrderItems[0].Discount) > 0) {
            $('#lblTotalDiscount-Retail').text(OrderItems[0].Discount);
            $('#lblInvoiceNetTotal-Retail').text(parseInt(subtotal) - parseInt(OrderItems[0].Discount));
            $('#trDiscount-Retail').show();
            $('#trNetTotal-Retail').show();
        }
    }

    $.print("#dvPrintOrder-Retail");
}