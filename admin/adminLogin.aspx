<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminLogin.aspx.cs" Inherits="FYP.adminLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Login</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css"/>
    <style>
        body {
            background-color: #f8f9fa;
        }
        .login-container {
            margin-top: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center login-container">
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header bg-primary text-white">
                            <h4 class="text-center">Admin Login</h4>
                        </div>
                        <div class="card-body">
                            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="text-danger" Visible="false"></asp:Label>
                            <div class="form-group">
                                <label for="username">Username:</label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter username"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="password">Password:</label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter password"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />
                        </div>
                        <div class="card-footer">
                            <asp:Label ID="Label1" runat="server" Text="" CssClass="text-danger" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.2/dist/umd/popper.min.js"></script>
</body>
</html>
