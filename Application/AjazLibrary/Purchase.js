Sys.Application.add_load(function () {

    // Remove any previous bindings to prevent duplicates
    $("[id$='ddlItem']").off('change').on('change', function () {
        $("[id$='ddlColor']").focus();
    });

    $("[id$='ddlColor']").off('change').on('change', function () {
        $("[id$='ddlSize']").focus();
    });

    $("[id$='ddlSize']").off('change').on('change', function () {
        $("[id$='txtQty']").focus();
    });

    $("[id$='txtQty']").off('change').on('change', function () {
        SetAmount();
        $("[id$='txtPrice']").focus();
    });

    $("[id$='txtPrice']").off('change').on('change', function () {
        SetAmount();
        $("[id$='btnAdd']").focus();
    });  
});
function SetAmount()
{
    $("[id$='txtAmount']").val(0);
    if ($("[id$='txtPrice']").val().length > 0 && $("[id$='txtQty']").val().length > 0) {
        var price = $("[id$='txtPrice']").val();
        var qty = $("[id$='txtQty']").val();
        var amount = parseInt(qty) * parseInt(price);
        $("[id$='txtAmount']").val(amount);
    }
}
function qtyKepress(input, event) {
    var charCode = event.which ? event.which : event.keyCode;
    var currentRow = $(input).closest('tr');

    // Handle Enter and Down Arrow
    if (event.key === 'Enter' || charCode === 13 || event.key === 'ArrowDown' || charCode === 40) {
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
