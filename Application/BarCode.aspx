<%@ Page Title="ChappalSoft : Bar Code Sticker" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BarCode.aspx.cs" Inherits="BarCode" %>
<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="main-panel">
                <div class="content-wrapper pb-0" style="min-height: 570px;">
                    <div class="page-header flex-wrap">
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="#">Shop Setting</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Bar Code Sticker </li>
                            </ol>
                        </nav>
                    </div>
                    <div class="row">
                        <div class="card" style="width: 100%;">
                            <div class="card-body">
                                <div class="form-group">
                                    <label for="lblCategory">Category</label>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control form-control-lg" Width="100%" Style="text-align: left;">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label for="lblItem">Item</label>
                                    <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control form-control-lg" Width="100%" Style="text-align: left;">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label for="lblColor">Color</label>
                                    <asp:DropDownList ID="ddlColor" runat="server" CssClass="form-control form-control-lg" Width="100%" Style="text-align: left;">
                                    </asp:DropDownList>
                                </div>
                                <button type="button" id="btnGenerate" class="btn btn-primary mr-2" onclick="GenerateSticker();"> Generate Sticker </button>                                
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
    <% Response.WriteFile("~/Reports/rptBarCodeSticker.htm");%>
</asp:Content>