<%@ Page Language="C#" MasterPageFile="~/admin/Admin.Master" AutoEventWireup="true" CodeBehind="createContent.aspx.cs" Inherits="FYP.createContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <h2>Create New Content</h2>
    <div>
        <asp:Label ID="lblTitle" runat="server" Text="Title: "></asp:Label>
        <asp:TextBox ID="txtTitle" runat="server" placeholder="Enter Title"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" ErrorMessage="Title is required"></asp:RequiredFieldValidator>
    </div>
    <br />
    <div>
        <asp:Label ID="lblDescription" runat="server" Text="Description: "></asp:Label>
        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" placeholder="Enter Description"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="Description is required"></asp:RequiredFieldValidator>
    </div>
    <br />
    <div>
        <asp:Label ID="lblTargetUser" runat="server" Text="For (Target User): "></asp:Label>
        <asp:DropDownList ID="ddlTargetUser" runat="server">
            <asp:ListItem Text="Patient" Value="Patient" />
            <asp:ListItem Text="Third-Party" Value="Third-Party" />
        </asp:DropDownList>
    </div>
    <br />
    <div>
        <asp:Label ID="lblPublishDate" runat="server" Text="Publish Date: "></asp:Label>
        <asp:Calendar ID="calPublishDate" runat="server" OnDayRender="calPublishDate_DayRender"></asp:Calendar>
    </div>
    <br />
    <asp:Button ID="btnCreateContent" runat="server" Text="Create Content" OnClick="btnCreateContent_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" />
</asp:Content>
