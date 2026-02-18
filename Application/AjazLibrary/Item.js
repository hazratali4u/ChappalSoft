$(document).ready(function (){
    $("[id$='txtSearch']").focus();
});
function Item() {
    var flag = true;
    $("[id$='txtName']").css("border-color", "#ebedf2");
    
    if ($("[id$='txtName']").val() == "") {
        $("[id$='txtName']").css("border-color", "red");
        $("[id$='txtName']").focus();
        flag = false;
    }
    if (flag) {
        if ($("[id$='txtName']").val() != "") {
            flag = true;
        }
    }
    return flag;
}
function NameKeyUp()
{
    if ($("[id$='txtName']").val().length == 0)
    {
        $("[id$='txtName']").css("border-color", "red");
    }
    else {
        $("[id$='txtName']").css("border-color", "#ebedf2");
    }
}
function filterGridViewItem() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("childPage_txtSearch");
    filter = input.value.toUpperCase();
    table = document.getElementById("childPage_gvItem");
    tr = table.getElementsByTagName("tr");

    for (i = 1; i < tr.length; i++) {
        td1 = tr[i].getElementsByTagName("td")[1];
        td2 = tr[i].getElementsByTagName("td")[2];
        if (td1 || td2) {
            txtValue1 = td1 ? td1.textContent || td1.innerText : "";
            txtValue2 = td2 ? td2.textContent || td2.innerText : "";
            if (txtValue1.toUpperCase().indexOf(filter) > -1 || txtValue2.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}