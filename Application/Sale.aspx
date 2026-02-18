<%@ Page Title="ChappalSoft : POS" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sale.aspx.cs" Inherits="Sale" %>

<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">
    <style type="text/css">
        .HidePanel {
            display: none;
        }
    </style>
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="main-panel" id="dvMain">
                <div class="content-wrapper pb-0" style="min-height: 570px;">
                    <div class="page-header flex-wrap">
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="#">Sales</a></li>
                                <li class="breadcrumb-item active" aria-current="page">POS </li>
                            </ol>
                        </nav>
                        <asp:HiddenField ID="hfTotalAmount" runat="server" Value="0" />
                    </div>
                    <div class="row">
                        <div class="col-md-5" style="padding-right: 5px; padding-left: 5px;">
                            <asp:DropDownList ID="ddlCategory" ToolTip="Select Category" runat="server" CssClass="form-control form-control-lg" Width="100%" Style="text-align: left;">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-2" style="padding-right: 5px; padding-left: 15px;">
                            <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" ToolTip="Sale Type">
                                <asp:ListItem Value="1" Text="Retail" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Wholesale"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-md-2" style="padding-right: 5px; padding-left: 15px;">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 80%;">
                                        <asp:DropDownList ID="ddlCustomer" runat="server" ToolTip="Select Customer" CssClass="form-control form-control-lg" Width="100%" Style="text-align: left;">
                                            <asp:ListItem Value="0" Text="Select Customer"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 20%;">
                                        <img src="assets/images/plus.png" id="imgCustomer" style="display: none;" title="Add Customer" onclick="OpenCustomerPopup()" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-3" style="padding-right: 20px; text-align: right;">
                            <asp:Label ID="lblTotal" runat="server" Text="Total: Amount-0 Qty-0"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3" style="padding-right: 5px; padding-left: 5px;">
                            <div class="card" style="width: 100%;">
                                <asp:HiddenField ID="hfItemIDs" runat="server" Value="" />
                                <asp:HiddenField ID="hfItemStock" runat="server" Value="" />
                                <asp:HiddenField ID="hfColorIDs" runat="server" Value="" />
                                <asp:HiddenField ID="hfItemID" runat="server" Value="0" />
                                <asp:HiddenField ID="hfColorID" runat="server" Value="0" />
                                <asp:HiddenField ID="hfSizeIDs" runat="server" Value="0" />
                                <asp:HiddenField ID="hfOrderedproducts" runat="server" Value="" />
                                <asp:HiddenField ID="hfAddress" runat="server" Value="" />
                                <asp:HiddenField ID="hfAddressShort" runat="server" Value="" />
                                <asp:HiddenField ID="hfInvoiceFooterNote" runat="server" Value="" />
                                <asp:HiddenField ID="hfInvoiceFooterNoteShort" runat="server" Value="" />
                                <asp:HiddenField ID="hfPhone" runat="server" Value="" />
                                <asp:HiddenField ID="hfSaleID" runat="server" Value="0" />
                                <asp:HiddenField ID="hfWorkingDate" runat="server" Value="" />
                                <asp:HiddenField ID="hfUserID" runat="server" Value="1" />
                                <div class="card-body" style="width: 100%; padding: 0px;">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search Item" ToolTip="Search Item" onkeyup="LoadItems()"></asp:TextBox>
                                    <div class="col-md-12 item_pd_right" id="dvProductsPanel" style="width: 100%; height: 400px; overflow-y: auto; padding: 0px;">
                                        <div class="bg-product scrolla col" style="padding-bottom: 0px;">
                                            <div class="pad" style="margin-top: 0px;" id="dvProducts">
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2" style="padding-right: 5px; padding-left: 5px;">
                            <div class="card" style="width: 100%;">
                                <div class="card-body" style="width: 100%; padding: 0px;">
                                    <asp:TextBox ID="txtSearchColor" runat="server" ToolTip="Search Color" CssClass="form-control" placeholder="Search Color" onkeyup="LoadColor()"></asp:TextBox>
                                    <div class="col-md-12 item_pd_right" id="dvColorPanel" style="width: 100%; height: 400px; overflow-y: auto; padding: 0px;">
                                        <div class="bg-product scrolla col" style="padding-bottom: 0px;">
                                            <div class="pad" style="margin-top: 0px;" id="dvColors">
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-7" style="padding-right: 5px; padding-left: 5px;">
                            <div class="card-body" style="width: 100%; padding: 0px;">
                                <div class="col-md-12 item_pd_right" id="dvItemGridPanel" style="width: 100%; height: 400px; overflow-y: auto; padding: 0px;">
                                    <div class="bg-product scrolla col" style="padding-bottom: 0px;">
                                        <div class="table-responsive">
                                            <table class="table table-striped" style="width: 100%;">
                                                <thead>
                                                    <tr style="color: White; background-color: #4CAF50;">
                                                        <th style="width: 20%;">Item</th>
                                                        <th style="width: 15%;">Color</th>
                                                        <th style="width: 15%;">Price</th>
                                                        <th style="width: 15%;">Amount</th>
                                                        <th style="width: 15%;">Size</th>
                                                        <th style="width: 15%;">Quantity</th>
                                                        <th style="width: 5%;">Remove</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tblItemGrid">
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="clear">
                                    </div>
                                </div>
                                <div class="col-md-12 item_pd_right" id="dvButtonPanel" style="width: 100%; height: 50px; padding: 0px;">
                                    <div class="bg-product scrolla col" style="padding-bottom: 0px;">
                                        <div class="pad" style="margin-top: 0px;" id="dvButtn">
                                            <button type="button" id="btnCancel" title="Cancel Order" class="btn btn-danger" onclick="btnCancelClicked();">Cancel </button>
                                            <button type="button" id="btnSave" title="Save Order" class="btn btn-primary mr-2" onclick="SaveOrder();">Save </button>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div class="clear">
                                    </div>
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
    <% Response.WriteFile("printOrder.htm");%>
    <% Response.WriteFile("printOrderRetail.htm");%>
    <% Response.WriteFile("PopupPayment.htm");%>
    <% Response.WriteFile("PopupCustomer.htm");%>
    <% Response.WriteFile("sizePopup.htm");%>
</asp:Content>
