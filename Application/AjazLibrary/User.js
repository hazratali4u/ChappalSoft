$(document).ready(function (){
    $("[id$='txtSearch']").focus();
});
function User() {
    var flag = true;
    $("[id$='txtEmployeeName']").css("border-color", "#ebedf2");
    $("[id$='txtUsername']").css("border-color", "#ebedf2");
    $("[id$='txtPassword']").css("border-color", "#ebedf2");
    
    if ($("[id$='txtPassword']").val() == "") {
        $("[id$='txtPassword']").css("border-color", "red");
        $("[id$='txtPassword']").focus();
        flag = false;
    }
    if ($("[id$='txtUsername']").val() == "") {
        $("[id$='txtUsername']").css("border-color", "red");
        $("[id$='txtUsername']").focus();
        flag = false;
    }
    if ($("[id$='txtEmployeeName']").val() == "") {
        $("[id$='txtEmployeeName']").css("border-color", "red");
        $("[id$='txtEmployeeName']").focus();
        flag = false;
    }
    if (flag) {
        if ($("[id$='txtUsername']").val() != "" && $("[id$='txtPassword']").val() != "" && $("[id$='txtEmployeeName']").val() != "") {
            flag = true;
        }
    }
    return flag;
}
function EmployeeNameKeyUp() {
    if ($("[id$='txtEmployeeName']").val().length == 0) {
        $("[id$='txtEmployeeName']").css("border-color", "red");
    }
    else {
        $("[id$='txtEmployeeName']").css("border-color", "#ebedf2");
    }
}
function UsernameKeyUp()
{
    if ($("[id$='txtUsername']").val().length == 0)
    {
        $("[id$='txtUsername']").css("border-color", "red");
    }
    else {
        $("[id$='txtUsername']").css("border-color", "#ebedf2");
    }
}
function PasswordKeyUp() {
    if ($("[id$='txtPassword']").val().length == 0) {
        $("[id$='txtPassword']").css("border-color", "red");
    }
    else {
        $("[id$='txtPassword']").css("border-color", "#ebedf2");
    }
}
function filterGridViewUser() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("childPage_txtSearch");
    filter = input.value.toUpperCase();
    table = document.getElementById("childPage_gvUsers");
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