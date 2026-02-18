<%@ Page Title="ChappalSoft : Rollback Document" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Rollback.aspx.cs" Inherits="Rollback" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">
    <style type="text/css">
        .HidePanel {
            display: none;
        }
    </style>
    <script language="JavaScript" type="text/javascript">
        function confirmation() {
            return confirm("Are you sure you want Rollback?");
        }
    </script>
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="main-panel">
                <div class="content-wrapper pb-0" style="min-height: 570px;">
                    <div class="page-header flex-wrap">
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="#">Accounts</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Rollback Document</li>
                            </ol>
                        </nav>
                    </div>
                    <div class="row">
                        <div class="card" style="width: 100%;">
                            <div class="card-body">
                                <div class="form-group">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                                <div class="form-group">
                                    <label for="lblCategory">Document Type</label>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control form-control-lg" Width="100%" Style="text-align: left;">
                                        <asp:ListItem Value="1" Text="Sale" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Purchase"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Receipt"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label for="lblFromDate">Date</label>
                                    <br>
                                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" disabled></asp:TextBox>
                                    <asp:ImageButton ID="imgFromDate" runat="server" ImageUrl="~/assets/images/date.gif"></asp:ImageButton>
                                </div>
                                <asp:Button ID="btnGetDate" runat="server" class="btn btn-primary mr-2" Text="Get Data" OnClick="btnGetDate_Click" />
                                <asp:Button ID="btnRollback" runat="server" class="btn btn-danger" Text="Rollback" OnClick="btnRollback_Click" OnClientClick="return confirmation();" />
                                <div class="table-responsive">
                                    <asp:GridView ID="gvSale" runat="server" CssClass="table table-striped" AutoGenerateColumns="False">
                                        <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="SaleID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbInvoice" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SaleType" HeaderText="Sale Type" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:BoundField DataField="Customer" HeaderText="Customer" ItemStyle-Width="30%" ReadOnly="true" />
                                            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="20%" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gvPurchase" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" Visible="false">
                                        <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="PurchaseID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbInvoice" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Supplier" HeaderText="Supplier" ItemStyle-Width="30%" ReadOnly="true" />
                                            <asp:BoundField DataField="PurchaseID" HeaderText="Purchase ID" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="20%" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gvReceipt" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" Visible="false">
                                        <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="RecordID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbInvoice" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Name" HeaderText="Customer" ItemStyle-Width="30%" ReadOnly="true" />
                                            <asp:BoundField DataField="PaymentMode" HeaderText="Payment Mode" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="20%" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="imgFromDate" Format="dd-MMM-yyyy" EnableViewState="False">
                                </cc1:CalendarExtender>
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