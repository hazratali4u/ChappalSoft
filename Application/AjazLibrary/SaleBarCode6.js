
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
        if($("[id$='ddlPaymentMode']").val() == 3)    
        {
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
        if(saletype==2)
        {
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
            addRow(itemID, colorID);
            $("[id$='txtSearch']").val('');
        }
    }
}
function addRow(itemID,colorID)
{
    $("[id$='hfItemID']").val(itemID);
    $("[id$='hfColorID']").val(colorID);
    AddItemToGrid();
}
function GetItemStock()
{
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
function LoadStock(dtStock)
{
    dtStock = JSON.stringify(dtStock);
    var result = jQuery.parseJSON(dtStock.replace(/&quot;/g, '"'));
    dtStock = eval(result.d);
    dtStock = JSON.stringify(dtStock);
    $("[id$='hfItemStock']").val(dtStock);
}
function LoadSize(itemID, colorID, sizeQty) {
    var focused = false;
    $('#tblItemSize').empty();
     var lstSize = $("[id$='hfSizeIDs']").val();
     lstSize = eval(lstSize);     
     for (var i = 0, len = lstSize.length; i < len; ++i) {
         var stock = GetStock(itemID, colorID, lstSize[i].SizeID);
         var row = $(' <tr><td style="display:none;">' + lstSize[i].SizeID + '</td><td style="width:30%;"><h5>' + lstSize[i].Name + '</h5></td><td style="width:40%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value="" style="text-align: center;width: 100%;" onblur="CheckSizeStock(' + lstSize[i].SizeID + ');" onkeydown="return qtyKepressSizePopup(this,event);"></td><td style="width:30%;white-space: normal;word-wrap: break-word;"><input disabled type="text" size="2" value="' + stock + '" style="text-align: center;width: 100%;"></td></tr>');
        $('#tblItemSize').append(row);
    }
    if (sizeQty.length > 0) {
        let sizeQtyList = JSON.parse(sizeQty);
        sizeQtyList.forEach(item => {            
            $('#tblItemSize tr').each(function () {
                if ($(this).find("td:eq(0)").text() == item.sizeID) {
                    $(this).find("td:eq(2) input").val(item.Qty);
                }
            });

        });
    }

    $('#tblItemSize tr').each(function () {
        if (!focused) {
            $(this).find("td:eq(2) input").focus();
            focused = true;
        }
    });
}
function CheckSizeStock(sizeID)
{
    $('#tblItemSize tr').each(function () {
        if ($(this).find("td:eq(0)").text() == sizeID) {
            if ($(this).find("td:eq(2) input").val().length > 0) {
                if (parseInt($(this).find("td:eq(2) input").val()) > parseInt($(this).find("td:eq(3) input").val())) {
                    $(this).find("td:eq(2) input").val(0);
                    alert('Quantity can not be greater than available stock');
                    return;
                }
            }
        }
    });
}
function ColorButtonClick(buttonid)
{
    $("[id$='hfColorID']").val(buttonid);
    if (parseInt($("[id$='hfItemID']").val()) > 0) {
        AddItemToGrid();
    }
}
function ItemButtonClick(buttonid)
{
    $("[id$='hfItemID']").val(buttonid);
    if (parseInt($("[id$='hfColorID']").val()) > 0)
    {
        AddItemToGrid();
    }
}
function AddItemToGrid() {
    var itemid = $("[id$='hfItemID']").val();
    var colorid = $("[id$='hfColorID']").val();
    var flag = false;
    $('#tblItemGrid tr').each(function (row, tr) {
        if ($(tr).find("td:eq(0)").text() == itemid && $(tr).find("td:eq(7)").text() == colorid) {
            flag = true;
            return;
        }
    });
    if (flag)
    {
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
            row = $('<tr><td style="display:none;">' + itemid + '</td><td style="width:20%; white-space: normal;word-wrap: break-word;">' + itemname + '</td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="button" value="' + colorname + '" name="' + colorname + '" id="1" class="btn btn-inverse-info btn-lg" style="width:95%;height:20px;padding:0px;border-color:#198ae3;;cursor:default;" /></td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value=' + itemprice + ' style="text-align: center;width: 100%;" onblur="qtyBlur(this);" onkeydown="return qtyKepress(this,event);"></td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value=""  style="text-align: center;width: 100%;" disabled></td><td id="tdAddSize" align="center" onclick="addSize(this);"><a href="#" title="Open Size Popup"><span class="fa fa-plus"></span></a></td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value=""  style="text-align: center;width: 100%;"  disabled></td><td style="display:none;">' + colorid + '</td><td align="center" onclick="deleteItem(this);" style="width:5%;white-space: normal;word-wrap: break-word;"><a href="#" title="Remove Item"><span class="fa fa-times"></span></a></td><td style="display:none;">' + categoryname + '</td><td style="display:none;"></td></tr>');
            $("#tblItemGrid").append(row);
        }
        else {
            row = $('<tr><td style="display:none;">' + itemid + '</td><td style="width:20%; white-space: normal;word-wrap: break-word;">' + itemname + '</td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="button" value="" name="" id="1" class="btn btn-inverse-info btn-lg" style="margin:5px;width:95%;height:20px;padding:0px;background-color:' + colorname + ';cursor:default;" /></td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value=' + itemprice + ' style="text-align: center;width: 100%;" onblur="qtyBlur(this);" onkeydown="return qtyKepress(this,event);"></td><td><input type="text" size="2" value=""  style="text-align: center;width: 100%;" disabled></td><td id="tdAddSize" align="center" onclick="addSize(this);" style="width:15%;white-space: normal;word-wrap: break-word;"><a href="#" title="Open Size Popup"><span class="fa fa-plus"></span></a></td><td style="width:15%;white-space: normal;word-wrap: break-word;"><input type="text" size="2" value=""  style="text-align: center;width: 100%;" disabled></td><td style="display:none;">' + colorid + '</td><td align="center" onclick="deleteItem(this);" style="width:5%;white-space: normal;word-wrap: break-word;"><a href="#" title="Remove Item"><span class="fa fa-times"></span></a></td><td style="display:none;">' + categoryname + '</td><td style="display:none;"></td></tr>');
            $("#tblItemGrid").append(row);
        }
        $('#tblItemGrid tr').each(function (row, tr) {
            if ($(tr).find("td:eq(0)").text() == itemid && $(tr).find("td:eq(7)").text() == colorid) {
                $(tr).find("td:eq(3) input").focus().select();
                return;
            }
        });
        ClearIDs();
    }
}
function ClearIDs()
{    
    $("[id$='hfItemID']").val(0);
    $("[id$='hfColorID']").val(0);
}
function GetItemName(itemid)
{
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
function GetSizeName(sizeid) {
    var sizename= "";
    var lstSize = $("[id$='hfSizeIDs']").val();
    lstSize = eval(lstSize);
    for (var i = 0, len = lstSize.length; i < len; ++i)
    {
        if(lstSize[i].SizeID == sizeid)
        {
            sizename = lstSize[i].Name;
            break;
        }
    }
    return sizename;
}
function GetColorName(colorid)
{
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
            if (saletype == 2)
            {
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

    if (!/^\d+$/.test($(rowIndex).find('td:eq(3) input').val()) || $(rowIndex).find('td:eq(3) input').val() === "") {
        $(rowIndex).find('td:eq(3) input').val(1);
        return;
    }

    if (!/^\d+$/.test($(rowIndex).find('td:eq(4) input').val()) || $(rowIndex).find('td:eq(4) input').val() === "") {
        $(rowIndex).find('td:eq(4) input').val(0);
        return;
    }

    
    var qty = $(rowIndex).find('td:eq(6) input').val();
    var price = $(rowIndex).find('td:eq(3) input').val();
    var amount = parseInt(qty) * parseInt(price);
    $(rowIndex).find('td:eq(4) input').val(amount);

    SetTotal();
    
}
function qtyKepress(input, event) {
    var charCode = event.which ? event.which : event.keyCode;
    var currentRow = $(input).closest('tr');

    // Handle Enter
    if (event.key === 'Enter' || charCode === 13) {
        var td = document.getElementById("tdAddSize");
        addSize(td)
    }
        //Down Arrow
    else if (event.key === 'ArrowDown' || charCode === 40) {
        event.preventDefault();
        var nextRow = currentRow.next('tr');
        var nextInput = nextRow.find('td:eq(2) input[type="text"]:not([disabled])');
        if (nextInput.length) {
            nextInput.focus().select();
        } else {
            $('#btnDoneSize').focus();
        }
        return false;
    }
        // Handle Up Arrow
    else if (event.key === 'ArrowUp' || charCode === 38) {
        event.preventDefault();
        var prevRow = currentRow.prev('tr');
        var prevInput = prevRow.find('td:eq(2) input[type="text"]:not([disabled])');
        if (prevInput.length) {
            prevInput.focus().select();
        }
        return false;
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
function qtyKepressSizePopup(input, event) {
    var charCode = event.which ? event.which : event.keyCode;
    var currentRow = $(input).closest('tr');

    // Handle Enter
    if (event.key === 'Enter' || charCode === 13) {
        btnDoneSizeClicked();
    }
        //Down Arrow
    else if (event.key === 'ArrowDown' || charCode === 40) {
        event.preventDefault();
        var nextRow = currentRow.next('tr');
        var nextInput = nextRow.find('td:eq(2) input[type="text"]:not([disabled])');
        if (nextInput.length) {
            nextInput.focus().select();
        } else {
            $('#btnDoneSize').focus();
        }
        return false;
    }
        // Handle Up Arrow
    else if (event.key === 'ArrowUp' || charCode === 38) {
        event.preventDefault();
        var prevRow = currentRow.prev('tr');
        var prevInput = prevRow.find('td:eq(2) input[type="text"]:not([disabled])');
        if (prevInput.length) {
            prevInput.focus().select();
        }
        return false;
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
function SetTotal()
{
    var amount = 0;    var qty = 0;
    $('#tblItemGrid tr').each(function (row, tr) {
        if ($(tr).find("td:eq(5) input").val() != "") {
            amount += parseInt($(tr).find("td:eq(4) input").val());
            qty += parseInt($(tr).find("td:eq(6) input").val());
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
function addSize(btn)
{
    var rowIndex = $(btn).parent();
    var itemID = $(rowIndex).find('td:eq(0)').text();
    var itemName = $(rowIndex).find('td:eq(1)').text();
    var colorID = $(rowIndex).find('td:eq(7)').text();
    var colorName = GetColorName2(colorID);
    var sizeQty = $(rowIndex).find('td:eq(10)').text();  
    $('#lblNameSize').text('Item: ' + itemName);
    $('#lblIColoeSize').text('Color: ' + colorName);
    $('#hfItemIDSize').val(itemID);
    $('#hfColorIDSize').val(colorID);
    document.getElementById("dvMain").style.pointerEvents = "none";
    document.getElementById("dvMain").style.opacity = "0.5";
    $('#dvSize').show("slow");
    LoadSize(itemID,colorID, sizeQty);
}
function btnCancelClicked()
{
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
    var flag = true;
    $('#tblItemGrid tr').each(function () {
        var sizeQtyList = $(this).find('td:eq(10)').text();
        if(sizeQtyList.length == 0)
        {
            flag = false;
            return;
        }
    });
    if (flag) {
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
    else {
        alert('Some of item(s) Quantity or Size not found.');
    }
}
function SavePayment()
{
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
            if ($("[id$='txtAmount']").val() < parseInt($("[id$='hfTotalAmount']").val()) - parseInt(discount))
            {
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
        if ($("[id$='txtAmount']").val().length > 0)
        {
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
        if ($(tr).find("td:eq(5) input").val() != "") {
            grossamount += parseInt($(tr).find("td:eq(4) input").val());
        }
    });
    var discount = 0;
    var amountpaid = 0;
    if ($("[id$='txtDiscount']").val().length > 0)
    {
        discount = $("[id$='txtDiscount']").val();
    }
    if ($("[id$='txtAmount']").val().length > 0)
    {
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
function SetOrderTable()
{
    var tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    $("[id$='hfOrderedproducts']").val(tableData);
}
function storeTblValues() {
    var tableData = new Array();
    var rowNo = 0;
    $('#tblItemGrid tr').each(function (row, tr) {

        let sizeQtyList = JSON.parse($(tr).find('td:eq(10)').text());
        sizeQtyList.forEach(item => {
            tableData[rowNo] = {
                "ItemID": $(tr).find('td:eq(0)').text(),
                "ColorID": $(tr).find('td:eq(7)').text(),
                "Price": $(tr).find('td:eq(3) input').val(),
                "Quantity": item.Qty,
                "SizeID": item.sizeID
            }
            rowNo++;
        });

    });
    return tableData;
}
function PrintOrder(OrderItems)
{
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
    for (var i = 0, len = OrderItems.length; i < len; i++)
    {
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

   var datetime =  moment().format('hh:mm A')
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
function OpenPaymentPopup()
{
    document.getElementById("dvMain").style.pointerEvents = "none";
    document.getElementById("dvMain").style.opacity = "0.5";    
    $('#dvPayment').show("slow");
    $("[id$='txtAmount']").focus()
}
function btnCancelPaymentClicked()
{
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
    var sizeQtyList = [];
    $('#tblItemSize tr').each(function () {
        if ($(this).find("td:eq(2) input").val().length > 0) {
            if (parseInt($(this).find("td:eq(2) input").val()) > 0) {
                qty += parseInt($(this).find("td:eq(2) input").val());
                sizeQtyList.push({ sizeID: $(this).find("td:eq(0)").text(), Qty: $(this).find("td:eq(2) input").val() });
            }
        }
    });
    let serialized = JSON.stringify(sizeQtyList);
    if (qty == 0)
    {
        alert('Must enter size quantity!');
        return;
    }
    var itemid = $('#hfItemIDSize').val();
    var colorid = $('#hfColorIDSize').val();
    $('#tblItemGrid tr').each(function (row, tr) {
        if ($(tr).find("td:eq(0)").text() == itemid && $(tr).find("td:eq(7)").text() == colorid) {
            $(tr).find("td:eq(10)").text(serialized);
            $(tr).find("td:eq(6) input").val(qty);
            $(tr).find("td:eq(3) input").trigger('blur');
            return;
        }
    });

    document.getElementById("dvMain").style.pointerEvents = "auto";
    document.getElementById("dvMain").style.opacity = "1";
    $('#dvSize').hide("slow");
    $("[id$='txtSearch']").focus();
}
function CalculateAmountPaid()
{
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
        if ($("[id$='txtAmount']").val().length > 0)
        {
            payment = parseInt($("[id$='txtAmount']").val());
        }
        var balacne = parseInt(payment) - parseInt(amountpaid);
        $("[id$='lblBalance']").text('Balance : ' + balacne);
    }
}
function CheckStock() {
    var itemnames = '';
    var flag = true;
    var lstStock = $("[id$='hfItemStock']").val();
    lstStock = eval(lstStock);

    $('#tblItemGrid tr').each(function () {
        var itemID = $(this).find("td:eq(0)").text().trim();
        var itemName = $(this).find("td:eq(1)").text().trim();
        var colorID = $(this).find("td:eq(7)").text().trim();
        var colorname = GetColorName2(colorID);

        let sizeQtyList = JSON.parse($(this).find('td:eq(10)').text());

        sizeQtyList.forEach(item => {
            var sizeID = item.sizeID;
            var sizename = GetSizeName(sizeID);
            var found = lstStock.some(stock =>
                stock.ItemID.toString() === itemID &&
                stock.ColorID.toString() === colorID &&
                stock.SizeID.toString() === sizeID &&
                item.Qty <= stock.ClosingStock
            );

            if (!found) {
                flag = false;
                itemnames += itemName + ',Color:' + colorname + ',Size:' + sizename + ', ';
            }
        });
    });

    if (!flag) {
        alert('Stock not found for these items: ' + itemnames.slice(0, -2));
    }

    return flag;
}
function GetStock(itemID, colorID, sizeID) {
    var stock = 0;
    var lstStock = $("[id$='hfItemStock']").val();
    lstStock = eval(lstStock);
    if(lstStock.length > 0)
    {
        for (var i = 0; i < lstStock.length; i++)
        {
            if (lstStock[i].ItemID == parseInt(itemID) && lstStock[i].ColorID == parseInt(colorID) && lstStock[i].SizeID == parseInt(sizeID))
            {
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
function OpenCustomerPopup()
{
    document.getElementById("dvMain").style.pointerEvents = "none";
    document.getElementById("dvMain").style.opacity = "0.5";
    $('#dvCustomerAdd').show("slow");
    $("[id$='txtCustomerNameAdd']").focus()
}
function btnCancelCustomerAdd_Clicked()
{
    document.getElementById("dvMain").style.pointerEvents = "auto";
    document.getElementById("dvMain").style.opacity = "1";
    $('#txtCustomerNameAdd').val('');
    $('#txtAddressAdd').val('');
    $('#txtContactNoAdd').val('');
    $('#dvCustomerAdd').hide("slow");
}
function btnSaveCustomerAdd_Clicked()
{
    if($('#txtCustomerNameAdd').val().length > 0)
    {
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
function CustomerSaved(CustomerID)
{
    CustomerID = Number(CustomerID.d);;
    if(CustomerID > 0)
    {
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