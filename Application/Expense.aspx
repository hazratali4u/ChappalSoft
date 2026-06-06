<%@ Page Title="MobileSoft : Expense Entry" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Expense.aspx.cs" Inherits="Expense" %>
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
                          <li class="breadcrumb-item"><a href="#">Accounts</a></li>
                          <li class="breadcrumb-item active" aria-current="page"> Add Expense</li>
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
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search Expense" onkeyup="filterGridViewExpense()"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3" style="text-align:right;">
                                        <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-success" Text="Add New" OnClick="btnAdd_Click" />
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvExpense" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
                                        OnRowEditing="gvExpense_RowEditing" OnRowDeleting="gvExpense_RowDeleting">
                                        <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="ExpenseID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ExpenseHeadID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ExpenseDate" HeaderText="Expense Date" ItemStyle-Width="15%" ReadOnly="true" />
                                            <asp:BoundField DataField="ExpenseName" HeaderText="Expense Name" ItemStyle-Width="30%" ReadOnly="true" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="20%" ReadOnly="true" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-Width="15%" ReadOnly="true" />
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
                                    <asp:HiddenField ID="hfExpenseID" runat="server" Value="0" />
                                </div>
                                <div class="form-group">
                                    <label for="lblExpenseDate">Expense Date</label>
                                    <br />
                                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="10" disabled></asp:TextBox>
                                        <asp:ImageButton ID="imgFromDate" runat="server" ImageUrl="~/assets/images/date.gif"></asp:ImageButton>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                                            PopupButtonID="imgFromDate" Format="dd-MMM-yyyy" EnableViewState="False">
                                        </cc1:CalendarExtender>
                                </div>
                                <div class="form-group">
                                    <label for="lblExpenseHead">Expense Head *</label>
                                    <asp:DropDownList ID="ddlHead" runat="server" CssClass="form-control form-control-lg" Width="100%" style="text-align:left;"
                                        onfocus="this.style.background='#EAF3DE'; this.style.borderColor='#639922';"
                                                onblur="this.style.background=''; this.style.borderColor='';">                                        
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label for="lblAmount">Expense Amount *</label>
                                    <asp:TextBox ID="txtAmount" runat="server" class="form-control" placeholder="Expense Amount" onkeyup="NameKeyUp()"
                                        onfocus="this.style.background='#EAF3DE'; this.style.borderColor='#639922';"
                                                onblur="this.style.background=''; this.style.borderColor='';"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="lblRemarks">Remarks</label>
                                    <asp:TextBox ID="txtRemarks" runat="server" class="form-control" placeholder="Remarks"
                                        onfocus="this.style.background='#EAF3DE'; this.style.borderColor='#639922';"
                                                onblur="this.style.background=''; this.style.borderColor='';"></asp:TextBox>
                                </div>
                                <asp:Button ID="btnSave" runat="server" class="btn btn-primary mr-2" Text="Save" OnClientClick="return Expense();" OnClick="btnSave_Click" />
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