$(document).ready(function (){
    $("[id$='txtUsername']").focus();
});
function Login() {
    var flag = true;
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
    if (flag) {
        if ($("[id$='txtUsername']").val() != "" && $("[id$='txtPassword']").val() != "") {
            flag = true;
        }
    }
    return flag;
}
function Cancel()
{
    $("[id$='txtUsername']").val('');
    $("[id$='txtPassword']").val('');
    $("[id$='txtUsername']").focus();
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