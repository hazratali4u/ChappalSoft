<%@ Page Title="ChappalSoft : Stock Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rptSales.aspx.cs" Inherits="rptSales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="main-panel">
                <div class="content-wrapper pb-0" style="min-height: 570px;">
                    <div class="page-header flex-wrap">
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="#">Reports</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Sales Report </li>
                            </ol>
                        </nav>
                    </div>
                    <div class="row">
                        <div class="card" style="width: 100%;">
                            <div class="card-body">
                                <div class="form-group">
                                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" Width="50%">
                                        <asp:ListItem Value="1" Text="Date Wise" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Item Wise"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Category Wise"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Sale Type Wise"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Customer Wise"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="form-group" id="dvCategory" style="display:none;">
                                    <label for="lblCategory">Category</label>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control form-control-lg" Width="100%" Style="text-align: left;">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" id="dvItem" style="display:none;">
                                    <label for="lblItem">Item</label>
                                    <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control form-control-lg" Width="100%" Style="text-align: left;">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfItemIDs" runat="server" Value="" />
                                </div>
                                <div class="form-group">
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
                                <button type="button" id="btnView" class="btn btn-primary mr-2" onclick="ViewSalesReport();"> View Report </button>
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
    <% Response.WriteFile("~/Reports/rptSales.htm");%>
</asp:Content>