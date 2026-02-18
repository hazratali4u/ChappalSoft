<%@ Page Title="ChappalSoft : Stock Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reprint.aspx.cs" Inherits="Reprint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">
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
                                <li class="breadcrumb-item active" aria-current="page">Reprint Invoice</li>
                            </ol>
                        </nav>
                    </div>
                    <div class="row">
                        <div class="card" style="width: 100%;">
                            <div class="card-body">
                                <div class="form-group">
                                    <label for="lblFromDate">From Date</label>
                                    <br>
                                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" disabled></asp:TextBox>
                                    <asp:ImageButton ID="imgFromDate" runat="server" ImageUrl="~/assets/images/date.gif"></asp:ImageButton>
                                    <asp:HiddenField ID="hfAddress" runat="server" Value="" />
                                    <asp:HiddenField ID="hfAddressShort" runat="server" Value="" />
                                    <asp:HiddenField ID="hfInvoiceFooterNote" runat="server" Value="" />
                                    <asp:HiddenField ID="hfInvoiceFooterNoteShort" runat="server" Value="" />
                                    <asp:HiddenField ID="hfPhone" runat="server" Value="" />
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
                                        OnRowDataBound="gvSale_RowDataBound">
                                        <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="SaleID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TimeStamp" HeaderText="Date" ItemStyle-Width="20%" ReadOnly="true" DataFormatString="{0:dd-MMM-yyyy}"/>
                                            <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:BoundField DataField="Customer" HeaderText="Customer" ItemStyle-Width="30%" ReadOnly="true" />                                            
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Print">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnPrint" runat="server" Text="Print"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="imgFromDate" Format="dd-MMM-yyyy" EnableViewState="False">
                                </cc1:CalendarExtender>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                    PopupButtonID="imgToDate" Format="dd-MMM-yyyy" EnableViewState="False">
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
    <% Response.WriteFile("printOrder.htm");%>
    <% Response.WriteFile("printOrderRetail.htm");%>
</asp:Content>