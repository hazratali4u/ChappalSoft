function showPrintPreview(data) {
    $("#lblShopName").text(document.getElementById("childPage_hfShopName").value);
    $("#lblAddress").text(document.getElementById("childPage_hfAddress").value);
    $("#lblPhone").text(document.getElementById("childPage_hfPhone").value);
    $("#lblContactPerson").text(document.getElementById("childPage_hfContactPerson").value);
    var tbody = $('#previewBody');
    tbody.empty();
    var total = 0;
    var sr = 1;

    $.each(data, function (i, row) {
        $('#totalSale').text(
    parseFloat(row.TotalSale || 0).toLocaleString()
);

        $('#totalPurchase').text(
            parseFloat(row.TotalPurchase || 0).toLocaleString()
        );

        $('#totalExpense').text(
            parseFloat(row.TotalExpense || 0).toLocaleString()
        );

        $('#totalProfit').text(
            (
                parseFloat(row.TotalSale || 0) -
                parseFloat(row.TotalPurchase || 0) -
                parseFloat(row.TotalExpense || 0)
            ).toLocaleString()
        );
    });

    $('#grandTotal').text(total.toFixed(2));
    $('#printModal').fadeIn(300);
}
function closePreview() {
    $('#printModal').fadeOut(200);
}
function printPreview() {
    var content = document.getElementById('printArea').innerHTML;
    var win = window.open('', '_blank', 'width=800,height=600');
    win.document.write(`
            <html>
            <head>
                <title>Profit & Loss Report</title>
                <style>
                    body { font-family: Arial, sans-serif; padding: 20px; }
                    table { width:100%; border-collapse:collapse; }
                    th, td { padding:8px; border:1px solid #ccc; }
                    th { background:#2c3e50; color:#fff; }
                    @media print { button { display:none; } }
                </style>
            </head>
            <body onload="window.print(); window.close();">
                ${content}
            </body>
            </html>`);
    win.document.close();
}
