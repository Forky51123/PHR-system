<%@ Page Language="C#" MasterPageFile="~/admin/Admin.Master" AutoEventWireup="true" CodeBehind="addUser.aspx.cs" Inherits="FYP.addUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Add User</h2>
    <asp:Label ID="lblType" runat="server" Text="Choose User Type:"></asp:Label>
    <asp:DropDownList ID="ddl_userType" runat="server">
        <asp:ListItem>Patient</asp:ListItem>
        <asp:ListItem>Third-Party</asp:ListItem>
    </asp:DropDownList><br />
    <asp:Button ID="btnSubmit" runat="server" Text="Add" OnClick="btnSubmit_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"/>
</asp:Content>