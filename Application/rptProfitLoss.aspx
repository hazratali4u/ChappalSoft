<%@ Page Title="ChappalSoft : Profit & Loss Report" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="rptProfitLoss.aspx.cs" Inherits="rptProfitLoss" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">
    <style type="text/css">
        .HidePanel {
            display: none !important;
        }

        .ajax__calendar {
            z-index: 99999 !important;
        }

        .ajax__calendar_container {
            z-index: 99999 !important;
        }

      
    </style>
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="main-panel">
                <div class="content-wrapper pb-0" style="min-height: 570px;">
                    <div class="page-header flex-wrap">
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="#">Reports</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Profit & Loss Report </li>
                            </ol>
                        </nav>
                    </div>
                    <div class="row">
                        <div class="card" style="width: 100%;">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:HiddenField ID="hfShopName" runat="server" Value="" />
                                        <asp:HiddenField ID="hfAddress" runat="server" Value="" />
                                        <asp:HiddenField ID="hfPhone" runat="server" Value="" />
                                        <asp:HiddenField ID="hfContactPerson" runat="server" Value="" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <label for="lblFromDate">From Date</label>
                                    <br />
                                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" disabled></asp:TextBox>
                                        <asp:ImageButton ID="imgFromDate" runat="server" ImageUrl="~/assets/images/date.gif"></asp:ImageButton>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                            PopupButtonID="imgFromDate" Format="dd-MMM-yyyy" EnableViewState="False">
                                        </cc1:CalendarExtender>
                                        <br />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <label for="lblToDate">To Date</label>
                                    <br />
                                    <asp:TextBox ID="txtToDate" runat="server" MaxLength="10" disabled></asp:TextBox>
                                        <asp:ImageButton ID="imgToDate" runat="server" ImageUrl="~/assets/images/date.gif"></asp:ImageButton>
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                                            PopupButtonID="imgToDate" Format="dd-MMM-yyyy" EnableViewState="False">
                                        </cc1:CalendarExtender>
                                        <br />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <br />
                                        <asp:Button ID="btnGetData" runat="server" CssClass="btn btn-primary mr-2" Text="View Report" OnClick="btnGetData_Click" />
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
    <% Response.WriteFile("~/Reports/rptProfitLoss.html");%>
</asp:Content>