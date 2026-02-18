<%@ Page Title="ChappalSoft : Add Purchase" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Purchase.aspx.cs" Inherits="Purchase" %>

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
            <div class="main-panel" id="dvMain">
                <div class="content-wrapper pb-0" style="min-height: 570px;">
                    <div class="page-header flex-wrap">
                        <nav aria-label="breadcrumb">
                            <ol class="breadcrumb">
                                <li class="breadcrumb-item"><a href="#">Purchases</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Add Purchase </li>
                            </ol>
                        </nav>
                    </div>
                    <div class="card" style="width: 100%;">
                        <div class="card-body">
                            <div class="form-group">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                <asp:HiddenField ID="hfItemIDs" runat="server" Value="" />
                                <asp:HiddenField ID="hfSizeQtyJson" runat="server" Value="" />
                                <asp:HiddenField ID="hfWorkingDate" runat="server" Value="" />
                                <asp:HiddenField ID="hfUserID" runat="server" Value="1" />
                            </div>
                            <div class="form-group">
                                <asp:DropDownList ID="ddlDocNo" runat="server" Width="50%" CssClass="form-control form-control-lg"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDocNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <asp:DropDownList ID="ddlSupplier" runat="server" Width="50%" CssClass="form-control form-control-lg"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtPurchaseNo" Width="50%" runat="server" class="form-control" placeholder="Purchase No"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:DropDownList ID="ddlCategory" runat="server" Width="50%" CssClass="form-control form-control-lg">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-12">
                                        <table style="width: 100%;">
                                            <tr style="color: White; background-color: #4CAF50;">
                                                <td style="width: 40%;">Item
                                                </td>
                                                <td style="width: 10%;">Color
                                                </td>
                                                <td style="width: 10%;">Size
                                                </td>
                                                <td style="width: 10%;">Quantity
                                                </td>
                                                <td style="width: 10%;">Pur. Price
                                                </td>
                                                <td style="width: 10%;">Amount
                                                </td>
                                                <td style="width: 10%;">Action
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 40%;">
                                                    <asp:DropDownList ID="ddlItem" runat="server" Width="100%" CssClass="form-control form-control-lg">
                                                        <asp:ListItem Value="0" Text="ABC"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlColor" runat="server" Width="100%" CssClass="form-control form-control-lg">
                                                        <asp:ListItem Value="0" Text="ABC"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 10%; text-align: center;">
                                                    <asp:LinkButton ID="lnkSize" runat="server" OnClick="lnkSize_Click"><span class="fa fa-plus"></span></asp:LinkButton>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtQty" runat="server" class="form-control" disabled placeholder="Quantity"></asp:TextBox>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtPrice" runat="server" class="form-control" placeholder="Price" onkeypress="return qtyKepress(this,event);"></asp:TextBox>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:TextBox ID="txtAmount" runat="server" class="form-control" placeholder="Amount" disabled></asp:TextBox>
                                                </td>
                                                <td style="width: 10%; text-align: center;">
                                                    <asp:Button ID="btnAdd" runat="server" class="btn btn-primary mr-2" Text="Add" OnClick="btnAdd_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvItem" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" ShowHeader="false"
                                            OnRowDeleting="gvItem_RowDeleting">
                                            <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                            <Columns>
                                                <asp:BoundField DataField="ItemID" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ItemName" HeaderText="Item Name" ItemStyle-Width="40%" />
                                                <asp:BoundField DataField="Color" HeaderText="Color" ItemStyle-Width="10%" />
                                                <asp:BoundField DataField="Size" HeaderText="Size" ItemStyle-Width="10%" />
                                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-Width="10%" />
                                                <asp:BoundField DataField="Price" HeaderText="Price" ItemStyle-Width="10%" />
                                                <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="10%" />
                                                <asp:BoundField DataField="ColorID" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SizeID" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="StockDate" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SizeQtyJson" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                            class="fa fa-trash-o" ToolTip="Delete"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <asp:Button ID="btnSave" runat="server" class="btn btn-primary mr-2" Text="Save" OnClick="btnSave_Click" />
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
            <div id="dvSize" runat="server" visible="false" style="max-width: 450px; z-index: 5000; top: 50%; left: 50%; right: auto; position: absolute; background-color: #c6c6c6; border: 2px solid #000; padding: 10px; border-radius: 7px; transform: translate(-50%, -50%);">
                <div class="main-panel" style="min-width: 400px; padding-top: 10px;">
                    <div class="content-wrapper pb-0" style="min-height: 570px;">
                        <div class="row">
                            <div class="card" style="width: 100%;">
                                <div class="card-body">
                                    <div class="form-group" style="margin-bottom:0px;">
                                        <h4><label id="lblNameSize" runat="server"></label></h4>
                                    </div>
                                    <div class="form-group" style="margin-bottom:0px;">
                                        <h4><label id="lblIColoeSize" runat="server"></label></h4>
                                    </div>
                                    <div class="form-group">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvSize" runat="server" CssClass="table table-striped" AutoGenerateColumns="False">
                                                <HeaderStyle BackColor="#4CAF50" ForeColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="SizeID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Name" ItemStyle-Font-Bold="true" HeaderText="Size" ItemStyle-Width="50%" ReadOnly="true" />
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Width="75%" Height="15px" onkeypress="return qtyKepress(this,event);" onkeydown="return qtyKepress(this,event);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20%"/>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <asp:Button ID="btnCancelSize" runat="server" class="btn btn-danger" Text="Cancel" OnClick="btnCancelSize_Click" />
                                    <asp:Button ID="btnDoneSize" runat="server" class="btn btn-success" Text="Done" OnClick="btnDoneSize_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>