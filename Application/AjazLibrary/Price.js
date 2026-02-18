$(document).ready(function (){
    $("[id$='txtSearch']").focus();
});
function filterGridViewPrice() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("childPage_txtSearch");
    filter = input.value.toUpperCase();
    table = document.getElementById("childPage_gvPrice");
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