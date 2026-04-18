
function GenerateSticker() {
    var itemID = $("[id$='ddlItem']").val();
    var itemName = $("[id$='ddlItem'] option:selected").text();
    var colorID = $("[id$='ddlColor']").val();
    var colorName = $("[id$='ddlColor'] option:selected").text();
    var size = $("[id$='ddlSize'] option:selected").text();
    $.ajax
        ({
            type: "POST", //HTTP method
            url: "BarCode2.aspx/GenerateSticker", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ItemID: itemID, ItemName: itemName, ColorID: colorID, ColorName: colorName, Size: size }),
            success: LoadSticker,
        });
}
function LoadSticker(dtReport) {
    dtReport = JSON.stringify(dtReport);
    var result = jQuery.parseJSON(dtReport.replace(/&quot;/g, '"'));
    dtReport = eval(result.d);

    $("#stickerDetail").empty();
    if ($("[id$='ddlColor']").val() == "0") {
        var row = "";   // for images
        var row1 = "";  // for names

        for (var i = 0; i < dtReport.length; i++) {
            // build one cell for image
            row += '<td style="width:12.5%;border:1px solid black;">' +
                '<img style="width:100px; height:100px;" src="data:image/png;base64,' + dtReport[i].Image + '" />' +
                '</td>';

            // build one cell for name
            row1 += '<td style="width:12.5%;border:1px solid black;">' +
                dtReport[i].ItemName + '-' + dtReport[i].ColorName + '-' + dtReport[i].Size +
                '</td>';

            // once we reach 8 items OR last record → append rows
            if ((i + 1) % 8 === 0 || i === dtReport.length - 1) {
                $("#stickerDetail").append("<tr>" + row + "</tr>");
                $("#stickerDetail").append("<tr>" + row1 + "</tr>");
                row = "";
                row1 = "";
            }
        }
    }
    else {
        for (var i = 0, len = dtReport.length; i < len; i++) {
            var row = $(
             '<tr><td style="width:25%;border:1px solid black;">' +
               '<img style="width:100px; height:100px;" src="data:image/png;base64,' + dtReport[0].Image + '" />' +
             '</td><td style="width:25%;border:1px solid black;">' +
               '<img style="width:100px; height:100px;" src="data:image/png;base64,' + dtReport[0].Image + '" />' +
               '</td><td style="width:25%;">' +
               '' +
               '</td><td style="width:25%;">' +
               '' +
             '</td></tr>'
           );
            $("#stickerDetail").append(row);

            var row1 = $(
            '<tr><td style="width:25%;border:1px solid black;">' +
            dtReport[0].ItemName + '-' + dtReport[0].ColorName + '-' + dtReport[0].Size +
          '</td><td style="width:25%;border:1px solid black;">' +
            dtReport[0].ItemName + '-' + dtReport[0].ColorName + '-' + dtReport[0].Size +
          '</td></tr>'
        );
            $("#stickerDetail").append(row1);
        }
    }

    $.print("#dvBarCodeStcker");
}