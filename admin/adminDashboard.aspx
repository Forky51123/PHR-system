<%@ Page Language="C#" MasterPageFile="~/admin/Admin.Master" AutoEventWireup="true" CodeBehind="adminDashboard.aspx.cs" Inherits="FYP.adminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
        <asp:ScriptManager runat="server"></asp:ScriptManager>
    <asp:Button ID="selectOption1" runat="server" Text="User Activity" CssClass="btn" Value="userActivityPanel" OnClick="SelectOption_Click" />
    <asp:Button ID="selectOption2" runat="server" Text="User Management" CssClass="btn" Value="userManagementPanel" OnClick="SelectOption_Click" />
    <asp:Button ID="selectOption3" runat="server" Text="Content Management" CssClass="btn" Value="contentManagementPanel" OnClick="SelectOption_Click" />
    <asp:Button ID="selectOption4" runat="server" Text="Access Control" CssClass="btn" Value="accessControlPanel" OnClick="SelectOption_Click" />
    <asp:Button ID="selectOption5" runat="server" Text="System Configuration" CssClass="btn" Value="systemConfigurationPanel" OnClick="SelectOption_Click"/>
    

    <!-- User Activity Panel -->
    <asp:UpdatePanel ID="userActivityUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
       
    </ContentTemplate>
    </asp:UpdatePanel>

    <!-- User Management Panel -->
    <asp:UpdatePanel ID="userManagementUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Display current user -->
            <h2>Patient</h2>
            <asp:GridView ID="gvPatientManage" runat="server" DataSourceID="SqlDataSource2" AutoGenerateColumns="False" DataKeyNames="patientID,patientName"
                OnRowDeleting="GvPatientManage_DeleteCommand" OnSelectedIndexChanged="gvPatientManage_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="patientID" HeaderText="ID" ReadOnly="True" SortExpression="patientID"></asp:BoundField>
                    <asp:BoundField DataField="patientName" HeaderText="Name" ReadOnly="True" SortExpression="patientName"></asp:BoundField>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" HeaderText="Action"></asp:CommandField>
                </Columns>
            </asp:GridView>
            
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PHR %>" DeleteCommand="DELETE FROM [patient] WHERE [patientID] = @PatientID" SelectCommand="SELECT patient.patientID, personalInfo.patientName, patient.role FROM patient INNER JOIN personalInfo ON patient.patientID = personalInfo.patientID">
                <DeleteParameters>
                    <asp:Parameter Name="PatientID"></asp:Parameter>
                </DeleteParameters>
            </asp:SqlDataSource> 
            <asp:GridView ID="gvPatientAppointment" Visible ="false" runat="server"></asp:GridView>
            
            <h2>Third-Party</h2>           
            <asp:GridView ID="gvTpManage" runat="server" DataSourceID="SqlDataSource3" AutoGenerateColumns="False"
                DataKeyNames="thirdID" OnRowDeleting="GvTpManage_DeleteCommand" OnSelectedIndexChanged="gvTpManage_SelectedIndexChanged">           
                <Columns>
                    <asp:BoundField DataField="thirdID" HeaderText="ID" ReadOnly="True" SortExpression="thirdID"></asp:BoundField>
                    <asp:BoundField DataField="thirdName" HeaderText="Name" SortExpression="thirdName"></asp:BoundField>
                    <asp:CommandField ShowDeleteButton="True" ShowSelectButton="True" HeaderText="Action"></asp:CommandField>
                </Columns>
            </asp:GridView>
<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PHR %>" DeleteCommand="DELETE FROM [thirdParty] WHERE [thirdID] = @ThirdID" SelectCommand="SELECT [thirdID], [thirdName], [role] FROM [thirdParty]"></asp:SqlDataSource>        
            <asp:GridView ID="gvThirdPartyAppointment" Visible="false" runat="server"></asp:GridView>
        </ContentTemplate>
    
    </asp:UpdatePanel>

    <!-- Content Management Panel -->
    <script type="text/javascript">
        function confirmDelete() {
            return confirm("Are you sure you want to delete this content?");
        }
    </script>
    <asp:UpdatePanel ID="contentManagementUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <!-- Display existing content (GridView) -->
            <asp:GridView ID="gvContent" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="contentID" OnRowDeleting="GvContent_DeleteCommand">
                <Columns>
                    <asp:BoundField DataField="contentPublishDate" HeaderText="Publish Date" SortExpression="contentPublishDate"></asp:BoundField>
                    <asp:BoundField DataField="contentTitle" HeaderText="Title" SortExpression="contentTitle"></asp:BoundField>
                    <asp:BoundField DataField="contentDescription" HeaderText="Description" SortExpression="contentDescription"></asp:BoundField>
                    <asp:BoundField DataField="targetUser" HeaderText="For" SortExpression="targetUser"></asp:BoundField>
                    <asp:CommandField ShowDeleteButton="True" HeaderText="Action"></asp:CommandField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:adminPart %>" SelectCommand="SELECT * FROM [Content]" DeleteCommand="DELETE FROM [Content] WHERE [contentID] = @contentID" InsertCommand="INSERT INTO [Content] ([contentID], [contentTitle], [contentDescription], [contentPublishDate], [targetUser]) VALUES (@contentID, @contentTitle, @contentDescription, @contentPublishDate, @targetUser)" UpdateCommand="UPDATE [Content] SET [contentTitle] = @contentTitle, [contentDescription] = @contentDescription, [contentPublishDate] = @contentPublishDate, [targetUser] = @targetUser WHERE [contentID] = @contentID">
                <DeleteParameters>
                    <asp:Parameter Name="contentID" Type="String"></asp:Parameter>
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="contentID" Type="String"></asp:Parameter>
                    <asp:Parameter Name="contentTitle" Type="String"></asp:Parameter>
                    <asp:Parameter Name="contentDescription" Type="String"></asp:Parameter>
                    <asp:Parameter Name="contentPublishDate" Type="DateTime"></asp:Parameter>
                    <asp:Parameter Name="targetUser" Type="String"></asp:Parameter>
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="contentTitle" Type="String"></asp:Parameter>
                    <asp:Parameter Name="contentDescription" Type="String"></asp:Parameter>
                    <asp:Parameter Name="contentPublishDate" Type="DateTime"></asp:Parameter>
                    <asp:Parameter Name="targetUser" Type="String"></asp:Parameter>
                    <asp:Parameter Name="contentID" Type="String"></asp:Parameter>
                </UpdateParameters>
            </asp:SqlDataSource>
            <asp:Button ID="createContent" runat="server" Text="Create Content" OnClick="btnCreateContent_Click" />
            </ContentTemplate>
</asp:UpdatePanel>


    <!-- Access Control Panel -->
    <asp:UpdatePanel ID="accessControlUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Content for Access Control goes here -->
             <div>
            <h2>Assign Roles to Users</h2>
            <div>
                <label for="lblPatient">Select Patient:</label>
                <asp:DropDownList ID="ddlPatient" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPatient_SelectedIndexChanged" DataSourceID="SqlDataSource4" DataTextField="patientID" DataValueField="patientID">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList> </div>
                 <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:PHR %>" SelectCommand="SELECT [patientID] FROM [patient]"></asp:SqlDataSource>
        <div>
    <label for="lblThirdParty">Select Third-Party:</label>
            <asp:DropDownList ID="ddlThirdParty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlThirdParty_SelectedIndexChanged" DataSourceID="SqlDataSource5" DataTextField="thirdID" DataValueField="thirdID">
                <asp:ListItem></asp:ListItem>
    </asp:DropDownList> </div>
                 <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:PHR %>" SelectCommand="SELECT [thirdID] FROM [thirdParty]"></asp:SqlDataSource>
                 <label for="lbllAssignRole">Assign role:</label>
                 <asp:DropDownList ID="ddlAssignRole" runat="server">
                     <asp:ListItem>Patient</asp:ListItem>
                     <asp:ListItem>Third-Party</asp:ListItem>
                 </asp:DropDownList>
                 <asp:Button ID="btnAssignRole" runat="server" Text="Assign" OnClick="btnAssignRole_Click"/>
                 </ContentTemplate>
    </asp:UpdatePanel>

  <!-- System Configuration Panel -->
<asp:UpdatePanel ID="systemConfigurationUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <!-- Content for System Configuration goes here -->
        <h2>Log Configuration</h2>

        <p>Current Log Level: <asp:Label ID="lblCurrentLogLevel" runat="server"></asp:Label></p>

        <label for="logLevel">Log Level:</label>
        <asp:DropDownList ID="logLevel" runat="server">
            <asp:ListItem Text="DEBUG" Value="DEBUG" />
            <asp:ListItem Text="INFO" Value="INFO" />
            <asp:ListItem Text="WARN" Value="WARN" />
            <asp:ListItem Text="ERROR" Value="ERROR" />
            <asp:ListItem Text="FATAL" Value="FATAL" />
        </asp:DropDownList>
        <br /><br />
        <asp:Button ID="btnSaveLogConfig" runat="server" Text="Save Log Configuration" OnClick="btnSaveLogConfig_Click" />
               <h2>Backup Settings</h2>

<label for="ddlBackupType">Backup Type:</label>
<asp:DropDownList ID="ddlBackupType" runat="server">
    <asp:ListItem Text="Full Backup" Value="FullBackup"></asp:ListItem>
    <asp:ListItem Text="Incremental Backup" Value="IncrementalBackup"></asp:ListItem>
</asp:DropDownList>

<label for="txtBackupSchedule">Auto Backup Date and Time:</label>
<asp:TextBox ID="txtBackupSchedule" runat="server" placeholder="Enter schedule (e.g., Daily at 2 AM)"></asp:TextBox>
<br /><br />
<asp:Button ID="btnSaveBackupSettings" runat="server" Text="Save Backup Settings" OnClick="btnSaveBackupSettings_Click" /><br />
<br /><br />
<asp:Button ID="btnPerformRecovery" runat="server" Text="Perform Recovery" OnClick="btnPerformRecovery_Click" />
        </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
