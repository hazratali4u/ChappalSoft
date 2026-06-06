<%@ Page Title="MobileSoft : Add Expense Head" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExpenseHead.aspx.cs" Inherits="ExpenseHead" %>

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
                          <li class="breadcrumb-item"><a href="#">Shop Setting</a></li>
                          <li class="breadcrumb-item active" aria-current="page"> Add Expense Head </li>
                        </ol>
                      </nav>
                    </div>
                    <div class="row" runat="server" id="divView">
                        <div class="card" style="width: 100%;">
                            <div class="card-body">
                                <div class="form-group">
                                    <asp:Label ID="lblError2" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                                <div class="row">
                                    <div class="col-md-9">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search Expense Head" onkeyup="filterGridViewExpenseHead()"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3" style="text-align:right;">
                                        <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-success" Text="Add New" OnClick="btnAdd_Click" />
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvExpenseHead" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
                                        OnRowEditing="gvExpenseHead_RowEditing" OnRowDeleting="gvExpenseHead_RowDeleting" OnRowDataBound="gvExpenseHead_RowDataBound">
                                        <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="ExpenseHeadID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="Expense Head Name" ItemStyle-Width="40%" ReadOnly="true" />
                                            <asp:BoundField DataField="Status" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Active/Inactive">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Active/Inactive?');return false;"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" runat="server" id="divAdd" visible="false">
                        <div class="card" style="width: 100%;">
                            <div class="card-body">
                                <div class="form-group">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:HiddenField ID="hfExpenseHeadID" runat="server" Value="0" />
                                </div>
                                <div class="form-group">
                                    <label for="lblUsername">Expense Head Name *</label>
                                    <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Expense Head Name" onkeyup="NameKeyUp()"></asp:TextBox>
                                </div>
                                <asp:Button ID="btnSave" runat="server" class="btn btn-primary mr-2" Text="Save" OnClientClick="return ExpenseHead();" OnClick="btnSave_Click" />
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