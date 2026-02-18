<%@ Page Title="ChappalSoft : Edit Sale" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditSale.aspx.cs" Inherits="EditSale" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">
    <script type="text/javascript">
        function qtyKeyPress(input, event) {

            // Allow: Tab
            if (event.key === "Tab") return true;

            // Allow: Backspace, Delete, Arrow keys
            if (
                event.key === "Backspace" ||
                event.key === "Delete" ||
                event.key === "ArrowLeft" ||
                event.key === "ArrowRight"
            ) {
                return true;
            }

            // Allow digits only (0–9)
            if (event.key >= "0" && event.key <= "9") {
                return true;
            }

            // Block everything else (including i)
            event.preventDefault();
            return false;
        }

        function checkReturnQty(txt) {
            var returnQty = parseFloat(txt.value) || 0;
            var row = txt.closest("tr");
            var saleQtyCell = row.cells[5];
            var saleQty = parseFloat(saleQtyCell.innerText.trim()) || 0;
            if (returnQty > saleQty) {
                alert("New quantity cannot be greater than Sold quantity!");
                txt.value = saleQty; // Optionally reset to max allowed
                txt.focus();
                return false;
            }
            return true;
        }
        function checkDate(sender, args) {

            var selectedDate = sender.get_selectedDate();
            var today = new Date();
            // Last 1 month
            var minDate = new Date();
            minDate.setMonth(today.getMonth() - 1);
            // Clear time part
            selectedDate.setHours(0, 0, 0, 0);
            today.setHours(0, 0, 0, 0);
            minDate.setHours(0, 0, 0, 0);
            if (selectedDate < minDate) {
                alert("Please select From Date within the last 1 month only.");
            }
        }
    </script>
    <style type="text/css">
        .HidePanel {
            display: none;
        }
    </style>
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="main-panel">
                <div class="content-wrapper pb-0" style="min-height: 570px;">
                    <div class="page-header flex-wrap">
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="#">Sales</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Edit Sale</li>
                            </ol>
                        </nav>
                    </div>
                    <div class="row">
                        <div class="card" style="width: 100%;">
                            <div class="card-body" runat="server" id="dvView">
                                <div class="form-group">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                    <br />
                                    <label for="lblFromDate">From Date</label>
                                    <br>
                                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" disabled></asp:TextBox>
                                    <asp:ImageButton ID="imgFromDate" runat="server" ImageUrl="~/assets/images/date.gif"></asp:ImageButton>
                                </div>
                                <div class="form-group">
                                    <label for="lblToDate">To Date</label>
                                    <br>
                                    <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" disabled></asp:TextBox>
                                    <asp:ImageButton ID="imgToDate" runat="server" ImageUrl="~/assets/images/date.gif"></asp:ImageButton>
                                </div>
                                <asp:Button ID="btnGetInvoices" runat="server" class="btn btn-primary mr-2" Text="Get Invocies" OnClick="btnGetInvoices_Click" />
                                <div class="table-responsive">
                                    <asp:GridView ID="gvSale" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
                                        OnRowEditing="gvSale_RowEditing">
                                        <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="SaleID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TimeStamp" HeaderText="Date" ItemStyle-Width="20%" ReadOnly="true" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:BoundField DataField="Customer" HeaderText="Customer" ItemStyle-Width="30%" ReadOnly="true" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" CommandName="Edit" ToolTip="Edit" runat="server" Text="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="imgFromDate" Format="dd-MMM-yyyy" EnableViewState="False"
                                    OnClientDateSelectionChanged="checkDate">
                                </cc1:CalendarExtender>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                    PopupButtonID="imgToDate" Format="dd-MMM-yyyy" EnableViewState="False">
                                </cc1:CalendarExtender>
                            </div>
                            <div class="card-body" runat="server" id="dvEdit" visible="false">
                                <div class="form-group">
                                    <h5>
                                        <asp:Label ID="lblInvoiceNo" runat="server"></asp:Label></h5>
                                    <asp:HiddenField ID="hfSaleID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfDate" runat="server" Value="0" />
                                </div>
                                <div class="form-group">
                                    <h5>
                                        <asp:Label ID="lblCustomerName" runat="server"></asp:Label></h5>
                                </div>
                                <div class="form-group">
                                    <h5>
                                        <asp:Label ID="lblDate" runat="server"></asp:Label></h5>
                                </div>
                                <div class="form-group">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvDetail" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
                                            OnRowDataBound="gvDetail_RowDataBound">
                                            <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                            <Columns>
                                                <asp:BoundField DataField="SaleDetailID" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Name" HeaderText="Item Name" ItemStyle-Width="20%" ReadOnly="true" />
                                                <asp:BoundField DataField="ColorName" HeaderText="Color" ItemStyle-Width="15%" ReadOnly="true" />
                                                <asp:BoundField DataField="SizeName" HeaderText="Size" ItemStyle-Width="15%" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Price">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" Width="90%" onkeydown="qtyKeyPress(this, event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Quantity" HeaderText="Sold Quantity" ItemStyle-Width="10%" ReadOnly="true"></asp:BoundField>
                                                <asp:TemplateField HeaderText="New Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Width="90%" onkeydown="qtyKeyPress(this, event)" onblur="checkReturnQty(this);"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="StockDate" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ItemID" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ColorID" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SizeID" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Price" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnDone" runat="server" class="btn btn-primary mr-2" Text="Update Sale" OnClick="btnDone_Click" OnClientClick="javascript:return confirm('Are you sure you want to Update?');return false;" />
                                    <asp:Button ID="btnCancel" runat="server" class="btn btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <footer class="footer">
                    <div class="d-sm-flex justify-content-center justify-content-sm-between">
                        <span class="text-muted d-block text-center text-sm-left d-sm-inline-block">Copyright © AzkoIT 2025</span>
                    </div>
                </footer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>