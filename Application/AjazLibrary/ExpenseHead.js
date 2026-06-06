$(document).ready(function (){
    $("[id$='txtSearch']").focus();
});
function ExpenseHead() {
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
function filterGridViewExpenseHead() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("childPage_txtSearch");
    filter = input.value.toUpperCase(); // Convert to uppercase for case-insensitive search
    table = document.getElementById("childPage_gvExpenseHead");
    tr = table.getElementsByTagName("tr");

    for (i = 1; i < tr.length; i++) { // Start from 1 to skip the header row
        td = tr[i].getElementsByTagName("td")[1]; // Assuming the 'Color Name' is the first visible column
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = ""; // Show matching row
            } else {
                tr[i].style.display = "none"; // Hide non-matching row
            }
        }
    }
}