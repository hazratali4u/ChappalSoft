<%@ Page Title="ChappalSoft : Add Party" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Party.aspx.cs" Inherits="Party" %>

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
                          <li class="breadcrumb-item"><a href="#">Shop Setting</a></li>
                          <li class="breadcrumb-item active" aria-current="page"> Add Party </li>
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
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search Party" onkeyup="filterGridViewParty()"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3" style="text-align:right;">
                                        <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-success" Text="Add New" OnClick="btnAdd_Click" />
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvCustomer" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
                                        OnRowEditing="gvCustomer_RowEditing" OnRowDeleting="gvCustomer_RowDeleting" OnRowDataBound="gvCustomer_RowDataBound">
                                        <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="PartyID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="Party Name" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:BoundField DataField="ContactNo" HeaderText="ContactNo" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:BoundField DataField="Status" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OpeningBalance" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PartyType" HeaderText="Party Type" ItemStyle-Width="20%" ReadOnly="true" />
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
                                    <asp:HiddenField ID="hfPartyID" runat="server" Value="0" />
                                </div>
                                <div class="form-group">
                                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" Width="50%"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                        <asp:ListItem Value="1" Text="Customer" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Supplier"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="form-group">
                                    <label for="lblName">Party Name *</label>
                                    <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Party Name"  onkeyup="NameKeyUpParty()" onblur="NameKeyUpParty()"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="lblAddress">Address</label>
                                    <asp:TextBox ID="txtAddress" runat="server" class="form-control" placeholder="Party Address"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="lblContact">Contact No</label>
                                    <asp:TextBox ID="txtContact" runat="server" class="form-control" placeholder="Party Contact No"></asp:TextBox>
                                </div>                                
                                <div class="form-group" runat="server">
                                    <label for="lblOpeningBalance">Opening Balance</label>
                                    <asp:TextBox ID="txtOpeningBalance" runat="server" class="form-control" placeholder="Party Opening Balance" onkeypress="return qtyKepress(this,event);"></asp:TextBox>
                                </div>
                                <asp:Button ID="btnSave" runat="server" class="btn btn-primary mr-2" Text="Save" OnClick="btnSave_Click" />
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
        <Triggers>
        <asp:PostBackTrigger ControlID="btnSave" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Content>