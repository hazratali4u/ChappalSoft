$(document).ready(function () {
    GetItemStock();
    document.getElementById('childPage_ddlCustomer').disabled = true;
    $("[id$='ddlPaymentMode']").change(function () {
        var discount = 0;
        try {
            if ($("[id$='txtDiscount']").val().length > 0) {
                discount = parseInt($("[id$='txtDiscount']").val());
            }
        } catch (e) {
            discount = 0;
        }
        if ($("[id$='ddlPaymentMode']").val() == 3) {
            $("[id$='txtAmount']").val(0);
            $("[id$='lblBalance']").text('Balance : 0');
            $("[id$='lblNet']").text('Net Total : 0');
        }
        else {
            $("[id$='txtAmount']").val(parseInt($("[id$='hfTotalAmount']").val()) - parseInt(discount));
            var payment = 0;
            if ($("[id$='txtAmount']").val().length > 0) {
                payment = parseInt($("[id$='txtAmount']").val());
            }
            $("[id$='lblBalance']").text('Balance : ' + (parseInt(payment) - parseInt($("[id$='hfTotalAmount']").val()) - parseInt(discount)));
        }
        $("[id$='lblNet']").text('Net Total : ' + (parseInt($("[id$='hfTotalAmount']").val()) - parseInt(discount)));
        CalculateAmountPaid();
    });
    $("[id$='rblType']").change(function () {
        document.getElementById('childPage_ddlCustomer').disabled = true;
        document.getElementById('imgCustomer').style.display = 'none';
        var saletype = 1;
        var radios = $("[id$='rblType']").find("input[type='radio']");
        for (var i = 0; i < radios.length; i++) {
            if (radios[i].checked) {
                saletype = parseInt(radios[i].value);
            }
        }
        if (saletype == 2) {
            document.getElementById('childPage_ddlCustomer').disabled = false;
            document.getElementById('imgCustomer').disabled = false;
            document.getElementById('imgCustomer').style.display = 'block';
        }

    });
    document.addEventListener("keydown", function (event) {
        if (event.altKey && event.key.toLowerCase() === "s") {
            event.preventDefault();
            document.getElementById("btnSave").click();
        }
    });
    $("[id$='txtSearch']").focus();
});
function txtSearchKeyPress(e) {
    var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
    if (key == 13) {
        e.preventDefault();
        if ($("[id$='txtSearch']").val() != "") {
            var itemID = $("[id$='txtSearch']").val().split(":")[0];
            var colorID = $("[id$='txtSearch']").val().split(":")[1];
            var sizeName = $("[id$='txtSearch']").val().split(":")[2];
            addRow(itemID, colorID,sizeName);
            $("[id$='txtSearch']").val('');
        }
    }
}
function addRow(itemID, colorID,sizeName) {
    $("[id$='hfItemID']").val(itemID);
    $("[id$='hfColorID']").val(colorID);
    $("[id$='hfSizeName']").val(sizeName);
    AddItemToGrid();
}
function GetItemStock() {
    $.ajax
        ({
            type: "POST", //HTTP method
            url: "Sale.aspx/LoadStock", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: LoadStock,
            error: OnError
        });
}
function LoadStock(dtStock) {
    dtStock = JSON.stringify(dtStock);
    var result = jQuery.parseJSON(dtStock.replace(/&quot;/g, '"'));
    dtStock = eval(result.d);
    dtStock = JSON.stringify(dtStock);
    $("[id$='hfItemStock']").val(dtStock);
}
function AddItemToGrid() {
    var itemid = $("[id$='hfItemID']").val();
    var colorid = $("[id$='hfColorID']").val();
    var sizename = $("[id$='hfSizeName']").val();
    var flag = false;
    $('#tblItemGrid tr').each(function (row, tr) {
        if ($(tr).find("td:eq(0)").text() == itemid && $(tr).find("td:eq(7)").text() == colorid && $(tr).find("td:eq(3)").text() == sizename) {
            flag = true;
            return;
        }
    });
    if (flag) {
        alert('Item with same color and size already exists.');
        return;
    }

    if (parseInt(itemid) > 0 && parseInt(colorid) > 0) {
        var itemname = GetItemName(itemid);
        var colorname = GetColorName(colorid);
        var showname = GetColorShowName(colorid);
        var categoryname = GetCategoryName(itemid);
        var itemprice = GetItemPrice(itemid);
        var row = "";
        if (showname) {
            row = $('<tr><td style="display:none;">' + itemid + '</td><td style="width:20%; white-space: normal;word-wrap: break-word;">' + itemname + '</td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="button" value="' + colorname + '" name="' + colorname + '" id="1" class="btn btn-inverse-info btn-lg" style="width:95%;height:20px;padding:0px;border-color:#198ae3;;cursor:default;" /></td><td>' + sizename + '</td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value=' + itemprice + ' style="text-align: center;width: 100%;" onblur="qtyBlur(this);" onkeydown="return qtyKepress(this,event);"></td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value="1"  style="text-align: center;width: 100%;" onblur="qtyBlur(this);" onkeydown="return qtyKepress(this,event);"></td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value=""  style="text-align: center;width: 100%;" disabled></td><td style="display:none;">' + colorid + '</td><td align="center" onclick="deleteItem(this);" style="width:5%;white-space: normal;word-wrap: break-word;"><a href="#" title="Remove Item"><span class="fa fa-times"></span></a></td><td style="display:none;">' + categoryname + '</td><td style="display:none;"></td></tr>');
            $("#tblItemGrid").append(row);
        }
        else {
            row = $('<tr><td style="display:none;">' + itemid + '</td><td style="width:20%; white-space: normal;word-wrap: break-word;">' + itemname + '</td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="button" value="" name="" id="1" class="btn btn-inverse-info btn-lg" style="margin:5px;width:95%;height:20px;padding:0px;background-color:' + colorname + ';cursor:default;" /></td><td>' + sizename + '</td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value=' + itemprice + ' style="text-align: center;width: 100%;" onblur="qtyBlur(this);" onkeydown="return qtyKepress(this,event);"></td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value="1"  style="text-align: center;width: 100%;" onblur="qtyBlur(this);" onkeydown="return qtyKepress(this,event);"></td><td><input type="text" size="2" value=""  style="text-align: center;width: 100%;" disabled></td><td style="display:none;">' + colorid + '</td><td align="center" onclick="deleteItem(this);" style="width:5%;white-space: normal;word-wrap: break-word;"><a href="#" title="Remove Item"><span class="fa fa-times"></span></a></td><td style="display:none;">' + categoryname + '</td><td style="display:none;"></td></tr>');
            $("#tblItemGrid").append(row);
        }
        $('#tblItemGrid tr').each(function (row, tr) {
            if ($(tr).find("td:eq(0)").text() == itemid && $(tr).find("td:eq(7)").text() == colorid) {
                $(tr).find("td:eq(4) input").focus().select();
                return;
            }
        });
        ClearIDs();
    }
}
function ClearIDs() {
    $("[id$='hfItemID']").val(0);
    $("[id$='hfColorID']").val(0);
    $("[id$='hfSizeName']").val('');
}
function GetItemName(itemid) {
    var itemname = "";
    var lstProducts = $("[id$='hfItemIDs']").val();
    lstProducts = eval(lstProducts);
    for (var i = 0, len = lstProducts.length; i < len; ++i) {
        if (lstProducts[i].ItemID == itemid) {
            itemname = lstProducts[i].Name;
            break;
        }
    }
    return itemname;
}
function GetCategoryName(itemid) {
    var categoryame = "";
    var lstProducts = $("[id$='hfItemIDs']").val();
    lstProducts = eval(lstProducts);
    for (var i = 0, len = lstProducts.length; i < len; ++i) {
        if (lstProducts[i].ItemID == itemid) {
            categoryame = lstProducts[i].CategoryName;
            break;
        }
    }
    return categoryame;
}
function GetColorName2(colorid) {
    var colorname = "";
    var lstColors = $("[id$='hfColorIDs']").val();
    lstColors = eval(lstColors);
    for (var i = 0, len = lstColors.length; i < len; ++i) {
        if (lstColors[i].ColorID == colorid) {
            colorname = lstColors[i].Name;
            break;
        }
    }
    return colorname;
}
function GetSizeID(sizename) {
    var sizeid = "";
    var lstSize = $("[id$='hfSizeIDs']").val();
    lstSize = eval(lstSize);
    for (var i = 0, len = lstSize.length; i < len; ++i) {
        if (lstSize[i].Name == sizename) {
            sizeid = lstSize[i].SizeID;
            break;
        }
    }
    return sizeid;
}
function GetColorName(colorid) {
    var colorname = "";
    var lstColors = $("[id$='hfColorIDs']").val();
    lstColors = eval(lstColors);
    for (var i = 0, len = lstColors.length; i < len; ++i) {
        if (lstColors[i].ColorID == colorid) {
            if (lstColors[i].ShowName) {
                colorname = lstColors[i].Name;
            }
            else {
                colorname = lstColors[i].ColorCode;
            }
            break;
        }
    }
    return colorname;
}
function GetColorShowName(colorid) {
    var showname = false;
    var lstColors = $("[id$='hfColorIDs']").val();
    lstColors = eval(lstColors);
    for (var i = 0, len = lstColors.length; i < len; ++i) {
        if (lstColors[i].ColorID == colorid) {
            showname = lstColors[i].ShowName;
            break;
        }
    }
    return showname;
}
function GetItemPrice(itemid) {
    var itemprice = "";
    var saletype = 1;
    var radios = $("[id$='rblType']").find("input[type='radio']");
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            saletype = parseInt(radios[i].value);
        }
    }
    var lstProducts = $("[id$='hfItemIDs']").val();
    lstProducts = eval(lstProducts);
    for (var i = 0, len = lstProducts.length; i < len; ++i) {
        if (lstProducts[i].ItemID == itemid) {
            if (saletype == 2) {
                itemprice = lstProducts[i].ItemWSPrice;
            }
            else {
                itemprice = lstProducts[i].ItemPrice;
            }
            break;
        }
    }
    return itemprice;
}
function qtyBlur(obj) {

    var rowIndex2 = $(obj).parent();
    var rowIndex = $(rowIndex2).parent();

    if (!/^\d+$/.test($(rowIndex).find('td:eq(4) input').val()) || $(rowIndex).find('td:eq(4) input').val() === "") {
        $(rowIndex).find('td:eq(4) input').val(1);
        return;
    }

    if (!/^\d+$/.test($(rowIndex).find('td:eq(5) input').val()) || $(rowIndex).find('td:eq(5) input').val() === "") {
        $(rowIndex).find('td:eq(6) input').val(0);
        return;
    }


    var qty = $(rowIndex).find('td:eq(5) input').val();
    var price = $(rowIndex).find('td:eq(4) input').val();
    var amount = parseInt(qty) * parseInt(price);
    $(rowIndex).find('td:eq(6) input').val(amount);

    SetTotal();

}
function qtyKepress(input, event) {
    var charCode = event.which ? event.which : event.keyCode;
    var currentRow = $(input).closest('tr');
    if (event.key === 'Enter' || charCode === 13) {
        $("[id$='txtSearch']").focus();
    }
    else if (event.key === 'Tab' || charCode === 9) {
        return true; // allow default tab behavior
    }
        // Allow control keys: Backspace, Delete, Left, Right
    else if (charCode === 8 || charCode === 46 || charCode === 37 || charCode === 39) {
        return true;
    }
        // Allow numeric keys (0-9 on keyboard and numpad)
    else if ((charCode >= 48 && charCode <= 57) || (charCode >= 96 && charCode <= 105)) {
        return true;
    }
        // Block other keys
    else {
        event.preventDefault();
        return false;
    }
}
function qtyKepressPayment(input, event) {
    var charCode = event.which ? event.which : event.keyCode;
    var currentRow = $(input).closest('tr');

    // Handle Enter
    if (event.key === 'Enter' || charCode === 13) {
        SavePayment();
    }
        // Allow Tab key
    else if (event.key === 'Tab' || charCode === 9) {
        return true; // allow default tab behavior
    }
        // Allow control keys: Backspace, Delete, Left, Right
    else if (charCode === 8 || charCode === 46 || charCode === 37 || charCode === 39) {
        return true;
    }
        // Allow numeric keys (0-9 on keyboard and numpad)
    else if ((charCode >= 48 && charCode <= 57) || (charCode >= 96 && charCode <= 105)) {
        return true;
    }
        // Block other keys
    else {
        event.preventDefault();
        return false;
    }
}
function SetTotal() {
    var amount = 0; var qty = 0;
    $('#tblItemGrid tr').each(function (row, tr) {
        if ($(tr).find("td:eq(6) input").val() != "") {
            amount += parseInt($(tr).find("td:eq(6) input").val());
            qty += parseInt($(tr).find("td:eq(5) input").val());
        }
    });
    $("[id$='txtAmount']").val(amount);
    var payment = 0;
    if ($("[id$='txtAmount']").val().length > 0) {
        payment = parseInt($("[id$='txtAmount']").val());
    }
    $("[id$='lblTotal']").text("Total: Amount-" + amount + " Qty-" + qty);
    $("[id$='lblBalance']").text('Balance : ' + (parseInt(payment) - parseInt(amount)));
    $("[id$='lblNet']").text('Net Total : ' + parseInt(amount));
    $("[id$='hfTotalAmount']").val(amount);
}
function deleteItem(btn) {
    var rowIndex = $(btn).parent();
    var RowID = $(rowIndex).find('td:eq(0)').text();
    $(rowIndex).remove();
    SetTotal();
}
function btnCancelClicked() {
    ClearIDs();
    $("[id$='lblTotal']").text("Total: Amount-0 Qty-0");
    $("[id$='hfSaleID']").val(0);
    $("[id$='ddlCustomer']").val(0);
    $("[id$='hfOrderedproducts']").val('');
    $("#tblItemGrid").empty();
    $("[id$='ddlPaymentMode']").val(1);
    $("[id$='txtAmount']").val('');
    $("[id$='hfTotalAmount']").val(0);
    $("[id$='txtDiscount']").val(0);
    $("[id$='lblBalance']").text('Balance : 0');
    $("[id$='lblNet']").text('Net Total : 0');
    $("[id$='txtAmount']").val(0);
}
function SaveOrder() {
    if (CheckStock()) {
        var saletype = 1;
        var radios = $("[id$='rblType']").find("input[type='radio']");
        for (var i = 0; i < radios.length; i++) {
            if (radios[i].checked) {
                saletype = parseInt(radios[i].value);
            }
        }
        if (saletype == 2 && $("[id$='ddlCustomer']").val() == 0) {
            alert('Wholesale: Select Customer');
            $("[id$='ddlCustomer']").focus();
            return;
        }
        $('#ddlPaymentMode').empty();
        if (saletype == 2) {
            var ddl = document.getElementById("ddlPaymentMode");

            var newOptionCash = document.createElement("option");
            newOptionCash.value = "1";
            newOptionCash.text = "Cash";
            ddl.appendChild(newOptionCash);

            var newOptionOnline = document.createElement("option");
            newOptionOnline.value = "2";
            newOptionOnline.text = "Online Transfer";
            ddl.appendChild(newOptionOnline);

            var newOptionCredit = document.createElement("option");
            newOptionCredit.value = "3";
            newOptionCredit.text = "Credit";
            ddl.appendChild(newOptionCredit);
        }
        else {
            var ddl = document.getElementById("ddlPaymentMode");

            var newOptionCash = document.createElement("option");
            newOptionCash.value = "1";
            newOptionCash.text = "Cash";
            ddl.appendChild(newOptionCash);

            var newOptionOnline = document.createElement("option");
            newOptionOnline.value = "2";
            newOptionOnline.text = "Online Transfer";
            ddl.appendChild(newOptionOnline);

        }
        OpenPaymentPopup();
    }
}
function SavePayment() {
    var discount = 0;
    try {
        if ($("[id$='txtDiscount']").val().length > 0) {
            discount = parseInt($("[id$='txtDiscount']").val());
        }
    } catch (e) {
        discount = 0;
    }
    if ($("[id$='ddlPaymentMode']").val() != 3) {
        if ($("[id$='txtAmount']").val().length > 0) {
            if ($("[id$='txtAmount']").val() < parseInt($("[id$='hfTotalAmount']").val()) - parseInt(discount)) {
                alert('Payment Amount can not be less than Net Amount');
                return;
            }
        }
        else {
            alert('Payment Amount can not be less than Net Amount');
            return;
        }
    }
    else {
        if ($("[id$='txtAmount']").val().length > 0) {
            if ($("[id$='txtAmount']").val() > parseInt($("[id$='hfTotalAmount']").val()) - parseInt(discount)) {
                alert('Payment Amount can not be greater Net Amount');
                return;
            }
        }
    }
    SetOrderTable();
    var saletype = 1;
    var grossamount = 0
    var radios = $("[id$='rblType']").find("input[type='radio']");
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            saletype = parseInt(radios[i].value);
        }
    }

    $('#tblItemGrid tr').each(function (row, tr) {
        if ($(tr).find("td:eq(6) input").val() != "") {
            grossamount += parseInt($(tr).find("td:eq(6) input").val());
        }
    });
    var discount = 0;
    var amountpaid = 0;
    if ($("[id$='txtDiscount']").val().length > 0) {
        discount = $("[id$='txtDiscount']").val();
    }
    if ($("[id$='txtAmount']").val().length > 0) {
        amountpaid = $("[id$='txtAmount']").val();
    }

    var radios = document.getElementsByName("rblPrintType");
    var rblPrintType = null;
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            rblPrintType = radios[i].value;
            break;
        }
    }
    if (rblPrintType == 1) {
        rblPrintType = 0;
    }

    $.ajax
        ({
            type: "POST", //HTTP method
            url: "Sale.aspx/SaveOrder", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ dtItems: $("[id$='hfOrderedproducts']").val(), SaleType: saletype, PaymentMode: $("[id$='ddlPaymentMode']").val(), CustomerID: $("[id$='ddlCustomer']").val(), GrossAmount: grossamount, Discount: discount, AmountPaid: amountpaid, UserID: $("[id$='hfUserID']").val(), WorkingDate: $("[id$='hfWorkingDate']").val(), IsPrinted: rblPrintType }),
            success: PaymentSaved,
            error: OnError
        });
}
function PaymentSaved(dtSale) {
    dtSale = JSON.stringify(dtSale);
    var result = jQuery.parseJSON(dtSale.replace(/&quot;/g, '"'));
    dtSale = eval(result.d);
    if (dtSale.length > 0) {
        if (dtSale[0].SaleType == 1) {

            var radios = document.getElementsByName("rblPrintType");
            var selectedValue = null;
            for (var i = 0; i < radios.length; i++) {
                if (radios[i].checked) {
                    selectedValue = radios[i].value;
                    break;
                }
            }
            if (selectedValue == 2) {
                PrintOrderRetail(dtSale);
            }
        }
        else {
            PrintOrder(dtSale);
        }
        btnCancelClicked();
        btnCancelPaymentClicked();
        GetItemStock();
    }
    else {
        alert('Some error occured!')
    }
}
function SetOrderTable() {
    var tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    $("[id$='hfOrderedproducts']").val(tableData);
}
function storeTblValues() {
    var tableData = new Array();
    var rowNo = 0;
    $('#tblItemGrid tr').each(function (row, tr) {
        tableData[rowNo] = {
            "ItemID": $(tr).find('td:eq(0)').text(),
            "ColorID": $(tr).find('td:eq(7)').text(),
            "Price": $(tr).find('td:eq(4) input').val(),
            "Quantity": $(tr).find('td:eq(5) input').val(),
            "SizeID": GetSizeID($(tr).find('td:eq(3)').text())
        }
        rowNo++;
    });
    return tableData;
}
function PrintOrder(OrderItems) {
    const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    const today = new Date();
    const day = today.getDate().toString().padStart(2, '0'); // Ensure 2-digit day
    const monthShort = today.toLocaleString('en-GB', { month: 'short' }); // "Mar"
    const year = today.getFullYear();
    const formattedDate = `${day}-${monthShort}-${year}`;
    const dayName = days[today.getDay()];
    var InvoiceChar = dayName.charAt(0);

    $('#lblDate').text(formattedDate);

    document.getElementById("trDuplicateBill").style.display = "none";

    if ($("[id$='ddlCustomer']").val() != 0) {
        var cus = $("[id$='ddlCustomer'] option:selected").text();
        var name = cus.split(":")[0];
        var contact = cus.split(":")[1];
        $('#lblCustomerName').text(name);
        $('#lblCustomerPhone').text(contact);
    }
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
    const today = new Date();
    const day = today.getDate().toString().padStart(2, '0'); // Ensure 2-digit day
    const monthShort = today.toLocaleString('en-GB', { month: 'short' }); // "Mar"
    const year = today.getFullYear();
    const formattedDate = `${day}-${monthShort}-${year}`;
    const dayName = days[today.getDay()];
    var InvoiceChar = dayName.charAt(0);
    $('#lblDate-Retail').text(formattedDate);

    document.getElementById("trDuplicateBillRetail").style.display = "none";

    var datetime = moment().format('hh:mm A')
    $('#lblTime-Retail').text(datetime);
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
function OpenPaymentPopup() {
    document.getElementById("dvMain").style.pointerEvents = "none";
    document.getElementById("dvMain").style.opacity = "0.5";
    $('#dvPayment').show("slow");
    $("[id$='txtAmount']").focus()
}
function btnCancelPaymentClicked() {
    document.getElementById("dvMain").style.pointerEvents = "auto";
    document.getElementById("dvMain").style.opacity = "1";
    $('#dvPayment').hide("slow");
}
function btnCancelSizeClicked() {
    document.getElementById("dvMain").style.pointerEvents = "auto";
    document.getElementById("dvMain").style.opacity = "1";
    $('#dvSize').hide("slow");
}
function btnDoneSizeClicked() {
    var qty = 0;
    $('#tblItemGrid tr').each(function () {
        if (parseInt($(this).find("td:eq(5) input").val()) > 0) {
            qty = parseInt($(this).find("td:eq(5) input").val());
            if (qty == 0) {
                return;
            }
        }
    });

    if (qty == 0) {
        alert('Must enter quantity!');
        return;
    }
    var itemid = $('#hfItemIDSize').val();
    var colorid = $('#hfColorIDSize').val();
    $('#tblItemGrid tr').each(function (row, tr) {
        if ($(tr).find("td:eq(0)").text() == itemid && $(tr).find("td:eq(7)").text() == colorid) {
            $(tr).find("td:eq(5) input").val(qty);
            $(tr).find("td:eq(4) input").trigger('blur');
            return;
        }
    });

    document.getElementById("dvMain").style.pointerEvents = "auto";
    document.getElementById("dvMain").style.opacity = "1";
    CheckStock();
    $("[id$='txtSearch']").focus();
}
function CalculateAmountPaid() {
    var discount = 0;
    try {
        if ($("[id$='txtDiscount']").val().length > 0) {
            discount = parseInt($("[id$='txtDiscount']").val());
        }
    } catch (e) {
        discount = 0;
    }

    var amountpaid = parseInt($("[id$='hfTotalAmount']").val()) - parseInt(discount);
    if ($("[id$='ddlPaymentMode']").val() != 3) {
        $("[id$='txtAmount']").val(amountpaid);
        var payment = 0;
        if ($("[id$='txtAmount']").val().length > 0) {
            payment = parseInt($("[id$='txtAmount']").val());
        }
        var balacne = parseInt(payment) - parseInt(amountpaid);
        $("[id$='lblBalance']").text('Balance : ' + balacne);
    }
    $("[id$='lblNet']").text('Net Total : ' + amountpaid);
}
function CalculateBalance() {
    var discount = 0;
    try {
        if ($("[id$='txtDiscount']").val().length > 0) {
            discount = parseInt($("[id$='txtDiscount']").val());
        }
    } catch (e) {
        discount = 0;
    }
    if ($("[id$='ddlPaymentMode']").val() == 3) {
        $("[id$='lblBalance']").text('Balance : 0');
    }
    else {
        var amountpaid = parseInt($("[id$='hfTotalAmount']").val()) - parseInt(discount);
        var payment = 0;
        if ($("[id$='txtAmount']").val().length > 0) {
            payment = parseInt($("[id$='txtAmount']").val());
        }
        var balacne = parseInt(payment) - parseInt(amountpaid);
        $("[id$='lblBalance']").text('Balance : ' + balacne);
    }
}
function CheckStock() {
    var itemName = '';
    var flag = true;
    var lstStock = $("[id$='hfItemStock']").val();
    lstStock = eval(lstStock);

    $('#tblItemGrid tr').each(function () {
        var itemID = $(this).find("td:eq(0)").text().trim();
        var qty = $(this).find('td:eq(5) input').val();
        var itemName = $(this).find("td:eq(1)").text().trim();
        var colorID = $(this).find("td:eq(7)").text().trim();
        var sizeID = GetSizeID($(this).find("td:eq(3)").text().trim());

        var found = lstStock.some(stock =>
            stock.ItemID.toString() === itemID &&
            stock.ColorID.toString() === colorID &&
            stock.SizeID.toString() === sizeID.toString() &&
            qty < stock.ClosingStock
            );

        if (!found) {
            flag = false;
            alert('Stock not found for these items: ' + itemName);
        }
    });
    return flag;
}
function GetStock(itemID, colorID, sizeID) {
    var stock = 0;
    var lstStock = $("[id$='hfItemStock']").val();
    lstStock = eval(lstStock);
    if (lstStock.length > 0) {
        for (var i = 0; i < lstStock.length; i++) {
            if (lstStock[i].ItemID == parseInt(itemID) && lstStock[i].ColorID == parseInt(colorID) && lstStock[i].SizeID == parseInt(sizeID)) {
                stock = lstStock[i].ClosingStock;
            }
        }
    }

    return stock;
}
function OnError(xhr, errorType, exception) {
    var responseText;
    responseText = xhr.responseText;
    alert(responseText);
}
function OpenCustomerPopup() {
    document.getElementById("dvMain").style.pointerEvents = "none";
    document.getElementById("dvMain").style.opacity = "0.5";
    $('#dvCustomerAdd').show("slow");
    $("[id$='txtCustomerNameAdd']").focus()
}
function btnCancelCustomerAdd_Clicked() {
    document.getElementById("dvMain").style.pointerEvents = "auto";
    document.getElementById("dvMain").style.opacity = "1";
    $('#txtCustomerNameAdd').val('');
    $('#txtAddressAdd').val('');
    $('#txtContactNoAdd').val('');
    $('#dvCustomerAdd').hide("slow");
}
function btnSaveCustomerAdd_Clicked() {
    if ($('#txtCustomerNameAdd').val().length > 0) {
        $.ajax
        ({
            type: "POST", //HTTP method
            url: "Sale.aspx/InsertCustomer", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ Name: $('#txtCustomerNameAdd').val(), Address: $('#txtAddressAdd').val(), ContactNo: $('#txtContactNoAdd').val() }),
            success: CustomerSaved,
            error: OnError
        });
    }
    else {
        alert('Enter Customer Name');
        $("[id$='txtCustomerNameAdd']").focus()
    }
}
function CustomerSaved(CustomerID) {
    CustomerID = Number(CustomerID.d);;
    if (CustomerID > 0) {
        document.getElementById("dvMain").style.pointerEvents = "auto";
        document.getElementById("dvMain").style.opacity = "1";

        var ddl = document.getElementById('childPage_ddlCustomer');
        var option = document.createElement("option");
        option.value = CustomerID;
        option.text = $('#txtCustomerNameAdd').val();
        ddl.appendChild(option);

        $("[id$='ddlCustomer']").val(CustomerID);
        $('#txtCustomerNameAdd').val('');
        $('#txtAddressAdd').val('');
        $('#txtContactNoAdd').val('');
        $('#dvCustomerAdd').hide("slow");
    }
    else {
        alert('Some error occured.');
    }
}