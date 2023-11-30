using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP
{
    public partial class adminDashboard : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(adminDashboard));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userActivityUpdatePanel.Visible = false;
                userManagementUpdatePanel.Visible = false;
                contentManagementUpdatePanel.Visible = false;
                accessControlUpdatePanel.Visible = false;
                systemConfigurationUpdatePanel.Visible = false;
            }
            
            //return back to the state where Content Mangaement tab is clicked
            if (!string.IsNullOrEmpty(Request.QueryString["tab"]) && Request.QueryString["tab"] == "content")
            {
                // Hide all content panels except the content management panel
                userActivityUpdatePanel.Visible = false;
                userManagementUpdatePanel.Visible = false;
                contentManagementUpdatePanel.Visible = true; // Display the content management panel
                accessControlUpdatePanel.Visible = false;
                systemConfigurationUpdatePanel.Visible = false;
           

               
            }
   
        }


        protected void SelectOption_Click(object sender, EventArgs e)
        {
            try
            {
                // Hide all content panels
                userActivityUpdatePanel.Visible = false;
                userManagementUpdatePanel.Visible = false;
                contentManagementUpdatePanel.Visible = false;
                accessControlUpdatePanel.Visible = false;
                systemConfigurationUpdatePanel.Visible = false;
             

                // Determine which button was clicked
                Button selectedButton = (Button)sender;

                // Log the selected option
                log.Info($"Selected option: {selectedButton.Text}");

                // Show the corresponding content panel
                switch (selectedButton.ID) // Use ID instead of Value
                {
                    case "selectOption1":
                        userActivityUpdatePanel.Visible = true;
                        break;
                    case "selectOption2":
                        userManagementUpdatePanel.Visible = true;
                        break;
                    case "selectOption3":
                        contentManagementUpdatePanel.Visible = true;
                        break;
                    case "selectOption4":
                        accessControlUpdatePanel.Visible = true;
                        break;
                    case "selectOption5":
                        systemConfigurationUpdatePanel.Visible = true;
                        break;
                   
                    default:
                        
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log an error if an exception occurs
                log.Error("Error in selectOption_Click", ex);
            }
        }

        protected void GvPatientManage_DeleteCommand(object sender,GridViewDeleteEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < gvPatientManage.Rows.Count)
            {
                // Get the data key of the selected row
                string patientID = gvPatientManage.DataKeys[e.RowIndex]["patientID"].ToString();
                // Call a method to delete the row in the database using patientID
                DeletePatientFromDatabase(patientID);
                gvPatientManage.DataBind();
            }
        }
        protected bool DeletePatientFromDatabase(string patientID)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='E:\PHR System\PHR System\PHR System\App_Data\PHR.mdf';Integrated Security=True;Connect Timeout=30";

            // SQL queries to update related records and delete the patient along with personal info
            string deleteAppointmentQuery = "DELETE FROM [appointment] WHERE [patientID] = @PatientID"; // Delete records in appointment table
            string deletePersonalInfoQuery = "DELETE FROM [personalInfo] WHERE [patientID] = @PatientID"; // Delete records from personalInfo table
            string deletePatientQuery = "DELETE FROM [patient] WHERE [patientID] = @PatientID"; // Delete patient record
            string deleteHealthInfoQuery = "DELETE FROM [healthInfo] WHERE [patientID] = @PatientID"; // Delete healthInfo record
            string deleteShareHealthQuery = "DELETE FROM [shareHealth] WHERE [patientID] = @PatientID"; // Delete shareHealth record
            string deleteSharePersonalQuery = "DELETE FROM [sharePersonal] WHERE [patientID] = @PatientID"; // Delete sharePersonal record
            // Create a SqlConnection and SqlCommand to execute the queries within a transaction
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    // Delete records in the appointment table
                    using (SqlCommand deleteAppointmentCommand = new SqlCommand(deleteAppointmentQuery, connection, transaction))
                    {
                        deleteAppointmentCommand.Parameters.AddWithValue("@PatientID", patientID);
                        int RowsAffected = deleteAppointmentCommand.ExecuteNonQuery();
                    }

                    // Delete records in personal info table
                    using (SqlCommand deletePersonalInfoCommand = new SqlCommand(deletePersonalInfoQuery, connection, transaction))
                    {
                        deletePersonalInfoCommand.Parameters.AddWithValue("@PatientID", patientID);
                        int RowsAffected = deletePersonalInfoCommand.ExecuteNonQuery();
                    }

                    // Delete records in health info table
                    using (SqlCommand deleteHealthInfoCommand = new SqlCommand(deleteHealthInfoQuery, connection, transaction))
                    {
                        deleteHealthInfoCommand.Parameters.AddWithValue("@PatientID", patientID);
                        int RowsAffected = deleteHealthInfoCommand.ExecuteNonQuery();
                    }

                    // Delete records in share health table
                    using (SqlCommand deleteShareHealthCommand = new SqlCommand(deleteShareHealthQuery, connection, transaction))
                    {
                        deleteShareHealthCommand.Parameters.AddWithValue("@PatientID", patientID);
                        int RowsAffected = deleteShareHealthCommand.ExecuteNonQuery();
                    }

                    // Delete records in share personal table
                    using (SqlCommand deleteSharePersonalCommand = new SqlCommand(deleteSharePersonalQuery, connection, transaction))
                    {
                        deleteSharePersonalCommand.Parameters.AddWithValue("@PatientID", patientID);
                        int RowsAffected = deleteSharePersonalCommand.ExecuteNonQuery();
                    }

                    // Delete the patient
                    using (SqlCommand deletePatientCommand = new SqlCommand(deletePatientQuery, connection, transaction))
                    {
                        deletePatientCommand.Parameters.AddWithValue("@PatientID", patientID);
                        int deleteRowsAffected = deletePatientCommand.ExecuteNonQuery();

                        transaction.Commit(); // Commit the transaction if all queries succeed
                        return deleteRowsAffected > 0; // Returns true if deletion is successful
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                    transaction?.Rollback(); // Rollback the transaction if any error occurs
                    return false;
                }
                finally
                {
                    connection.Close(); // Close the connection
                }
            }
        }


        protected void GvTpManage_DeleteCommand(object sender, GridViewDeleteEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < gvTpManage.Rows.Count)
            {
                // Get the data key of the selected row
                string thirdID = gvTpManage.DataKeys[e.RowIndex]["thirdID"].ToString();
                // Call a method to delete the row in the database using thirdID
                DeleteTpFromDatabase(thirdID);
                gvTpManage.DataBind();
            }
        }

        protected bool DeleteTpFromDatabase(string thirdID)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='E:\PHR System\PHR System\PHR System\App_Data\PHR.mdf';Integrated Security=True;Connect Timeout=30";

            string deleteAppointmentQuery = "DELETE FROM [appointment] WHERE [thirdID] = @ThirdID"; //Delete appointment record
            string deleteThirdPartyQuery = "DELETE FROM [thirdParty] WHERE [thirdID] = @ThirdID"; // Delete third party record

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    // Delete records in the appointment table
                    using (SqlCommand deleteAppointmentCommand = new SqlCommand(deleteAppointmentQuery, connection, transaction))
                    {
                        deleteAppointmentCommand.Parameters.AddWithValue("@ThirdID", thirdID);
                        int RowsAffected = deleteAppointmentCommand.ExecuteNonQuery();
                    }

                    // Delete the third party
                    using (SqlCommand deleteCommand = new SqlCommand(deleteThirdPartyQuery, connection, transaction))
                    {
                        deleteCommand.Parameters.AddWithValue("@ThirdID", thirdID);
                        int deleteRowsAffected = deleteCommand.ExecuteNonQuery();

                        transaction.Commit(); // Commit the transaction if all queries succeed
                        return deleteRowsAffected > 0; // Returns true if deletion is successful
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                    transaction?.Rollback(); // Rollback the transaction if any error occurs
                    return false;
                }
                finally
                {
                    connection.Close(); // Close the connection
                }
            }
        }





        protected void GvContent_DeleteCommand(object sender, GridViewDeleteEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < gvContent.Rows.Count)
            {
                // Get the data key of the selected row
                string contentID = gvContent.DataKeys[e.RowIndex].Value.ToString();
                // Call a method to delete the row in the database using contentID

                DeleteContentFromDatabase(contentID);
                gvContent.DataBind();

            }
        }
      


        protected bool DeleteContentFromDatabase(string contentID)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\FYP\FYP\FYP\App_Data\adminPart.mdf;Integrated Security=True";

            // SQL query to delete content based on contentID
            string deleteQuery = "DELETE FROM [Content] WHERE [contentID] = @ContentID";

            // Create a SqlConnection and SqlCommand to execute the delete query
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    // Add contentID as a parameter to the SQL query
                    command.Parameters.AddWithValue("@ContentID", contentID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        
                        return rowsAffected > 0; // Returns true if deletion is successful
                    }
                    catch (SqlException ex)
                    {
                       
                        Console.WriteLine("SQL Error: " + ex.Message);
            
                        return false;
                    }
                    finally
                    {
                        connection.Close(); // Close the connection
                    }
                }
            }
        }

      


        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/addUser.aspx");
        }
        protected void btnCreateContent_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/createContent.aspx");
        }
        
        private string GetCurrentLogLevel()
        {
            try
            {
                // Retrieve the current log level from the app settings in Web.config
                string currentLogLevel = ConfigurationManager.AppSettings["LogLevel"];

                // Validate the retrieved log level (optional)
                // You may want to ensure that the retrieved value is a valid log level.
                // For simplicity, I'm assuming INFO as the default if the value is not present.
                if (string.IsNullOrEmpty(currentLogLevel))
                {
                    currentLogLevel = "INFO";
                }

                return currentLogLevel;
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during the retrieval
                log.Error("Error retrieving current log level", ex);
                return "INFO"; // Default value in case of an error
            }
        }
        protected void btnSaveLogConfig_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedLogLevel = logLevel.SelectedValue;

                // Save the selected log level to the app settings
                Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
                configuration.AppSettings.Settings["LogLevel"].Value = selectedLogLevel;
                configuration.Save(ConfigurationSaveMode.Modified);

                // Refresh the current log level label immediately after saving
                lblCurrentLogLevel.Text = selectedLogLevel;

                // Manually trigger the update of the systemConfigurationUpdatePanel
                systemConfigurationUpdatePanel.Update();

                btnSaveLogConfig.Focus();

                // Log the change
                log.Info($"Log level changed to: {selectedLogLevel}");
            }
            catch (Exception ex)
            {
                log.Error("Error saving log configuration", ex);
            }
        }
        protected void btnSaveBackupSettings_Click(object sender, EventArgs e)
        {
            // Retrieve backup settings entered by the user
            string backupType = ddlBackupType.SelectedValue;
            string backupSchedule = txtBackupSchedule.Text;

            // TODO: Implement logic to save these settings to a configuration file or database
            // For now, let's just display the chosen settings as an example
            string message = $"Backup Type: {backupType}, Schedule: {backupSchedule}";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{message}');", true);
        }
        protected void btnPerformRecovery_Click(object sender, EventArgs e)
        {
            string backupDirectory = @"YourBackupDirectory"; // Same directory where backups are stored

            // Get the latest backup file from the backup directory
            string[] backupFiles = Directory.GetFiles(Server.MapPath(backupDirectory), "*.bak");
            Array.Sort(backupFiles);
            string latestBackupFile = backupFiles.Length > 0 ? backupFiles[backupFiles.Length - 1] : null;

            if (!string.IsNullOrEmpty(latestBackupFile))
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='YourDatabasePath';Integrated Security=True;Connect Timeout=30";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string recoveryQuery = $"USE master RESTORE DATABASE YourDatabaseName FROM DISK = '{latestBackupFile}' WITH REPLACE";
                        SqlCommand command = new SqlCommand(recoveryQuery, connection);
                        command.ExecuteNonQuery();
                    }

                    string message = "Database recovery performed successfully.";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{message}');", true);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Error occurred during recovery: {ex.Message}";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{errorMessage}');", true);
                }
            }
            else
            {
                string message = "No backup file found for recovery.";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{message}');", true);
            }
        }
        protected void gvContent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlPatient.SelectedValue))
            {
                ddlThirdParty.Enabled = false;
                ddlAssignRole.SelectedValue = "Patient";
            }
            else
            {
                ddlThirdParty.Enabled = true;
            }
        }

        protected void ddlThirdParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlThirdParty.SelectedValue))
            {
                ddlPatient.Enabled = false;
                ddlAssignRole.SelectedValue = "Third-Party";
            }
            else
            {
                ddlPatient.Enabled = true;
            }
        }

        protected void btnAssignRole_Click(object sender, EventArgs e)
        {
            string selectedRole = ddlAssignRole.SelectedValue;
            string selectedPatientID = ddlPatient.SelectedValue;
            string selectedThirdPartyID = ddlThirdParty.SelectedValue;

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\FYP\FYP\FYP\App_Data\adminPart.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string updateQuery = "";

                if (!string.IsNullOrEmpty(selectedPatientID))
                {
                    // Update the role for the selected patient
                    updateQuery = "UPDATE [patient] SET [role] = @Role WHERE [patientID] = @PatientID";
                }
                else if (!string.IsNullOrEmpty(selectedThirdPartyID))
                {
                    // Update the role for the selected third-party
                    updateQuery = "UPDATE [thirdParty] SET [role] = @Role WHERE [thirdID] = @ThirdPartyID";
                }

                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Role", selectedRole);

                        if (!string.IsNullOrEmpty(selectedPatientID))
                        {
                            command.Parameters.AddWithValue("@PatientID", selectedPatientID);
                        }
                        else if (!string.IsNullOrEmpty(selectedThirdPartyID))
                        {
                            command.Parameters.AddWithValue("@ThirdPartyID", selectedThirdPartyID);
                        }

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // The role was assigned successfully
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Role assigned successfully');", true);
                        }
                        else
                        {
                            // Handle assignment failure
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to assign role');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
                }
            }
        }






        protected void SqlDataSource3_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        protected void gvPatientManage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string patientID = gvPatientManage.SelectedValue.ToString();

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='E:\PHR System\PHR System\PHR System\App_Data\PHR.mdf';Integrated Security=True;Connect Timeout=30";
            string selectAppointmentsQuery = "SELECT appointmentID, patientID, thirdID, date, time, status FROM [appointment] WHERE [patientID] = @PatientID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand selectAppointmentsCommand = new SqlCommand(selectAppointmentsQuery, connection))
                    {
                        selectAppointmentsCommand.Parameters.AddWithValue("@PatientID", patientID);

                        SqlDataAdapter adapter = new SqlDataAdapter(selectAppointmentsCommand);
                        DataTable dtAppointments = new DataTable();

                        adapter.Fill(dtAppointments);

                        gvPatientAppointment.Visible = true;
                        gvPatientAppointment.DataSource = dtAppointments;
                        gvPatientAppointment.DataBind();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                    // Handle exceptions accordingly
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        protected void gvTpManage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string thirdID = gvTpManage.SelectedValue.ToString();

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='E:\PHR System\PHR System\PHR System\App_Data\PHR.mdf';Integrated Security=True;Connect Timeout=30";
            string selectAppointmentsQuery = "SELECT appointmentID, patientID, thirdID, date, time, status FROM [appointment] WHERE [thirdID] = @ThirdID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand selectAppointmentsCommand = new SqlCommand(selectAppointmentsQuery, connection))
                    {
                        selectAppointmentsCommand.Parameters.AddWithValue("@ThirdID", thirdID);

                        SqlDataAdapter adapter = new SqlDataAdapter(selectAppointmentsCommand);
                        DataTable dtAppointments = new DataTable();

                        adapter.Fill(dtAppointments);

                        gvThirdPartyAppointment.Visible = true;
                        gvThirdPartyAppointment.DataSource = dtAppointments;
                        gvThirdPartyAppointment.DataBind();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                    // Handle exceptions accordingly
                }
                finally
                {
                    connection.Close();
                }
            }
        }

    }
}
   


