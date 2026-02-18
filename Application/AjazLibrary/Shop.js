$(document).ready(function (){
    $("[id$='txtShopName']").focus();
});
function SaveShop() {
    var flag = true;
    $("[id$='txtShopName']").css("border-color", "#ebedf2");
    
    if ($("[id$='txtShopName']").val() == "") {
        $("[id$='txtShopName']").css("border-color", "red");
        $("[id$='txtShopName']").focus();
        flag = false;
    }
    if (flag) {
        if ($("[id$='txtShopName']").val() != "") {
            flag = true;
        }
    }
    return flag;
}
function ShopNameKeyUp()
{
    if ($("[id$='txtShopName']").val().length == 0)
    {
        $("[id$='txtShopName']").css("border-color", "red");
    }
    else {
        $("[id$='txtShopName']").css("border-color", "#ebedf2");
    }
}