<%@ Page Title="ChappalSoft : Add Price" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Price.aspx.cs" Inherits="Price" %>

<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">
    <style type="text/css">
        .HidePanel {
            display: none;
        }
    </style>
    <script>
        function qtyKepress(obj, event) {
            var charCode = event.which ? event.which : event.keyCode;
            if (charCode < 48 || charCode > 57) {
                event.preventDefault();
                return false;
            }
            return true;
        }
    </script>
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="main-panel">
                <div class="content-wrapper pb-0" style="min-height: 570px;">
                    <div class="page-header flex-wrap">
                        <nav aria-label="breadcrumb">
                        <ol class="breadcrumb">
                          <li class="breadcrumb-item"><a href="#">Item Management</a></li>
                          <li class="breadcrumb-item active" aria-current="page"> Add Item Price </li>
                        </ol>
                      </nav>
                    </div>
                    <div class="card" style="width: 100%;">
                        <div class="card-body">
                            <div class="form-group">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search Item" onkeyup="filterGridViewPrice()"></asp:TextBox>
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="gvPrice" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
                                    OnRowDataBound="gvPrice_RowDataBound">
                                    <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CategoryName" HeaderText="Category" ItemStyle-Width="20%" ReadOnly="true" />
                                        <asp:BoundField DataField="Name" HeaderText="Item Name" ItemStyle-Width="40%" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="Retail Price">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" Width="90%" onkeypress="return qtyKepress(this,event);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>                                        
                                        <asp:BoundField DataField="ItemPrice" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Wholesale Price">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWSPrice" runat="server" CssClass="form-control" Width="90%" onkeypress="return qtyKepress(this,event);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemWSPrice" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>                                
                            </div>
                            <asp:Button ID="btnSave" runat="server" class="btn btn-primary mr-2" Text="Save"  OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" class="btn btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
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