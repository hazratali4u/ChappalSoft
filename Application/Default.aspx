<%@ Page Title="ChappalSoft : Login" Language="C#" MasterPageFile="~/MasterPageLogin.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" EnableEventValidation="false" %>

<asp:Content ID="pageContent" ContentPlaceHolderID="childPage" runat="Server">
    <div class="main-panel">
        <div class="content-wrapper">
            <div class="page-header flex-wrap">
                <h3 class="mb-0">Login Form</h3>
            </div>
            <div class="row">
                <div class="card" style="width:100%;">
                    <div class="card-body">
                        <div class="form-group">
                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label for="lblUsername">Username *</label>
                            <asp:TextBox ID="txtUsername" class="form-control" runat="server" placeholder="Username" onkeyup="UsernameKeyUp()"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="lblPassword">Password *</label>
                            <asp:TextBox ID="txtPassword" TextMode="Password" class="form-control" runat="server" placeholder="Password" onkeyup="PasswordKeyUp()"></asp:TextBox>
                        </div>                        
                        <asp:Button ID="btnLogin" runat="server" class="btn btn-primary mr-2" Text="Login" OnClientClick="return Login();" OnClick="btnLogin_Click" />
                        <button type="button" class="btn btn-danger" onclick="Cancel()">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
        <footer class="footer">
            <div class="d-sm-flex justify-content-center justify-content-sm-between">
                <span class="text-muted d-block text-center text-sm-left d-sm-inline-block">Copyright © AzkoIT 2025</span>
                <span class="float-none float-sm-right d-block mt-1 mt-sm-0 text-center">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </span>
            </div>
        </footer>
    </div>
</asp:Content>
