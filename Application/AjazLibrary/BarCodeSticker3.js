Sys.Application.add_load(function () {
    $("[id$='ddlCategory']").off('change').on('change', function () {
        var categoryid = $("[id$='ddlCategory']").val();
        var lstProducts = $("[id$='hfItemIDs']").val();
        lstProducts = eval(lstProducts);
        var $itemDropdown = $("[id$='ddlItem']");
        $itemDropdown.empty();
        for (var i = 0, len = lstProducts.length; i < len; ++i) {
            if (lstProducts[i].CategoryID == categoryid || categoryid == 0) {
                $itemDropdown.append($('<option>', {
                    value: lstProducts[i].ItemID,
                    text: lstProducts[i].Name
                }));
            }
        }
    });
});
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
    var html = '';
    html += `
            <tr>
                <td style="
                    width:2in;
                    height:1in;
                    padding:0;
                    margin:0;
                    text-align:center;
                    vertical-align:middle;
                    overflow:hidden;
                    border:0;
                    font-family:Arial;
                ">
                    <img
                        src="data:image/png;base64,${dtReport[0].Image}"
                        style="
                            width: 100%;
                            height:0.45in;
                            object-fit:contain;
                            display:block;
                            margin:0 auto;
                        "
                    />

                    <div style="
                        font-size: 12px;
                        font-weight: bold;
                        line-height: 14px;
                        white-space: nowrap;
                                                overflow: hidden;
                        text-align: center;
                        margin-top: 2px;
                        font-family: Arial, sans-serif;
                    ">
                        ${dtReport[0].ColorName}
                    </div>

                </td>
            </tr>
        `;
    $("#stickerDetail").html(html);
    setTimeout(function () {
        $.print("#dvBarCodeSticker");

    }, 500);
}