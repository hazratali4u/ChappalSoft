<%@ Page Title="ChappalSoft : Add Receipt" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Receipt.aspx.cs" Inherits="Receipt" %>

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
                                <li class="breadcrumb-item"><a href="#">Receipt</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Add Receipt </li>
                            </ol>
                        </nav>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="card" style="width: 100%;">
                                <div class="card-body">
                                    <div class="form-group">
                                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:HiddenField ID="hfWorkingDate" runat="server" Value="" />
                                        <asp:HiddenField ID="hfUserID" runat="server" Value="1" />
                                    </div>
                                    <div class="form-group">
                                        <label for="lblPaymentMode">Payment Mode *</label>
                                        <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="1" Text="Cash" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Online Transfer"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="lblAmount">Amount Received *</label>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder="Amount Received" onkeypress="return qtyKepress(this, event);"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="lblAmount">Remarks</label>
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder="Remarks"></asp:TextBox>
                                    </div>
                                    <asp:Button ID="btnSave" runat="server" class="btn btn-primary mr-2" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" class="btn btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
                                    <div class="form-group">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvReceipt" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
                                                OnRowEditing="gvReceipt_RowEditing">
                                                <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="RecordID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PaymentMode" HeaderText="Payment Mode" ReadOnly="true" />
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount" ReadOnly="true" />
                                                    <asp:BoundField DataField="PaymentModeID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Edit" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                            class="fa fa-trash-o" ToolTip="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"/>
                                                </asp:TemplateField>                                                    
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="card" style="width: 100%;">
                                <div class="card-body">
                                    <div class="form-group">
                                        <label for="lblCustomer">Customer</label>
                                        <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                                            <asp:ListItem Value="1" Text="Cash" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Online Transfer"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvCustomer" runat="server" CssClass="table table-striped" AutoGenerateColumns="False">
                                                <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="SaleID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" ReadOnly="true" />
                                                    <asp:BoundField DataField="TimeStamp" HeaderText="Invoice Date" ReadOnly="true" DataFormatString="{0:dd-MMM-yyyy}" />
                                                    <asp:BoundField DataField="LedgerAmount" HeaderText="Amount" ReadOnly="true" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblTotal" CssClass="form-control" runat="server" Style="text-align: right;"></asp:Label>
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
</asp:Content>