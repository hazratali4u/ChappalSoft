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
        var rowTotal = (row.Amount || 0);
        total += parseFloat(row.Amount || 0);
        tbody.append(`
                <tr style="background:${i % 2 === 0 ? '#fff' : '#f9f9f9'}">
                    <td style="padding:8px; border:1px solid #ccc; text-align:center;">${sr++}</td>
                    <td style="padding:8px; border:1px solid #ccc;">${row.ExpenseHead || ''}</td>
                    <td style="padding:8px; border:1px solid #ccc;">${row.Remarks || ''}</td>
                    <td style="padding:8px; border:1px solid #ccc; text-align:right;">${rowTotal.toFixed(2)}</td>
                </tr>`);
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
                <title>Expense Report Summary</title>
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
function showPrintPreviewDetail(data) {
    $("#lblShopNameDetail").text(document.getElementById("childPage_hfShopName").value);
    $("#lblAddressDetail").text(document.getElementById("childPage_hfAddress").value);
    $("#lblPhoneDetail").text(document.getElementById("childPage_hfPhone").value);
    $("#lblContactPersonDetail").text(document.getElementById("childPage_hfContactPerson").value);

    var tbody = $('#previewBodyDetail');
    tbody.empty();

    // ── Group data by ExpenseDate ──────────────────────────────────────
    var groups = {};
    $.each(data, function (i, row) {
        var expensedate = row.ExpenseDate || 'Unknown';
        if (!groups[expensedate]) groups[expensedate] = [];
        groups[expensedate].push(row);
    });

    var grandTotal = 0;
    var sr = 1;

    // ── Render each expensedate group ──────────────────────────────────────
    $.each(groups, function (expenseDate, rows) {
        var groupTotal = 0;

        // Company header row
        tbody.append(`
            <tr>
                <td colspan="4" style="
                    padding: 8px 10px;
                    background: #d6eaf8;
                    color: #1a5276;
                    font-weight: bold;
                    font-size: 13px;
                    border-top: 2px solid #85c1e9;
                    border-bottom: 1px solid #aed6f1;
                    text-align: center; ">
                    &#9654; ${expenseDate}
                </td>
            </tr>`);

        // Detail rows
        $.each(rows, function (i, row) {
            var price = parseFloat(row.Amount || 0);
            groupTotal += price;

            tbody.append(`
                <tr style="background:${i % 2 === 0 ? '#fff' : '#f9f9f9'}">
                    <td style="padding:7px 10px; border:1px solid #ccc; text-align:center;">${sr++}</td>
                    <td style="padding:7px 10px; border:1px solid #ccc;">${row.ExpenseHead || ''}</td>
                    <td style="padding:7px 10px; border:1px solid #ccc;">${row.Remarks || ''}</td>
                    <td style="padding:7px 10px; border:1px solid #ccc; font-size:11px; color:#666;">${row.Amount || ''}</td>
                </tr>`);
        });

        grandTotal += groupTotal;

        // Group subtotal row
        tbody.append(`
            <tr style="background:#eaf4fb;">
                <td colspan="3" style="
                    padding:6px 10px;
                    border:1px solid #ccc;
                    text-align:right;
                    font-style:italic;
                    color:#555;
                    font-size:12px;">
                    Subtotal &mdash; ${expenseDate}:
                </td>
                <td style="
                    padding:6px 10px;
                    border:1px solid #ccc;
                    text-align:right;
                    font-style:italic;
                    color:#555;
                    font-size:12px;">
                    ${groupTotal.toFixed(2)}
                </td>
            </tr>`);
    });

    // Grand total row
    tbody.append(`
        <tr style="background:#2c3e50; color:#fff;">
            <td colspan="3" style="padding:8px 10px; text-align:right; font-weight:bold;">
                Grand Total:
            </td>
            <td style="padding:8px 10px; text-align:right; font-weight:bold;">
                ${grandTotal.toFixed(2)}
            </td>
        </tr>`);

    $('#printModalDeatil').fadeIn(300);
}
function closePreviewDetail() {
    $('#printModalDeatil').fadeOut(200);
}
function printPreviewDetail() {
    var content = document.getElementById('printAreaDetail').innerHTML;
    var win = window.open('', '_blank', 'width=800,height=600');
    win.document.write(`
            <html>
            <head>
                <title>Expense Report Summary</title>
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