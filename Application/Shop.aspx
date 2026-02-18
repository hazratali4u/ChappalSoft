<%@ Page Title="ChappalSoft : Shop Setting" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Shop.aspx.cs" Inherits="Shop" %>

<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="main-panel">
                <div class="content-wrapper pb-0" style="min-height: 570px;">
                    <div class="page-header flex-wrap">
                        <nav aria-label="breadcrumb">
                        <ol class="breadcrumb">
                          <li class="breadcrumb-item"><a href="#">Shop Setting</a></li>
                          <li class="breadcrumb-item active" aria-current="page"> Shop </li>
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
                                    <label for="lblShopName">Shop Name *</label>
                                    <asp:TextBox ID="txtShopName" runat="server" class="form-control" placeholder="Shop Name" onkeyup="ShopNameKeyUp()"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="lblShopAddress">Shop Address</label>
                                    <asp:TextBox ID="txtAddress" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="lblShopAddress">Short Address</label>
                                    <asp:TextBox ID="txtShortAddress" runat="server" class="form-control" placeholder="Short Address"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="lblContactPerson">Shop Contact Person</label>
                                    <asp:TextBox ID="txtContactPerson" class="form-control" runat="server" placeholder="Shop Contaxt Person"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="lblConactNumber">Contact Number</label>
                                    <asp:TextBox ID="txtConactNo" class="form-control" runat="server" placeholder="Contaxt Number"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="lblConactNumber">Invoice Footer Note</label>
                                    <asp:TextBox ID="txtInvoiceFooterNote" class="form-control" runat="server" placeholder="Invoice Footer Note" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="lblConactNumber">Invoice Footer Short Note</label>
                                    <asp:TextBox ID="txtInvoiceFooterNoteShort" class="form-control" runat="server" placeholder="Invoice Footer Short Note" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <asp:Button ID="btnSave" runat="server" class="btn btn-primary mr-2" Text="Save" OnClientClick="return SaveShop();" OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel" runat="server" class="btn btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
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