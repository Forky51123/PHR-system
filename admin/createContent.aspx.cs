using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP
{
    public partial class createContent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void calPublishDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date <= DateTime.Now.Date)
            {
                e.Day.IsSelectable = false;
                e.Cell.CssClass = "disabledDate";
            }
        }

        protected void btnCreateContent_Click(object sender, EventArgs e)
        {
            // Fetch other input values
            string contentTitle = txtTitle.Text;
            string contentDescription = txtDescription.Text;
            string targetUser = ddlTargetUser.SelectedValue;

            // Get the selected date from the calendar
            DateTime selectedDate = calPublishDate.SelectedDate;

            // Set the time part to 4 AM
            DateTime publishDateTime = selectedDate.Date.AddHours(4);

            // Save content to the database using contentTitle, contentDescription, targetUser, and publishDateTime
            SaveContentToDatabase(contentTitle, contentDescription, targetUser, publishDateTime);
        }

        // Method to save content to the database
        private void SaveContentToDatabase(string contentTitle, string contentDescription, string targetUser, DateTime publishDateTime)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\FYP\FYP\FYP\App_Data\adminPart.mdf;Integrated Security=True";

            string insertQuery = "INSERT INTO Content (contentID, contentTitle, contentDescription, contentPublishDate, targetUser) " +
                                 "VALUES (@ContentID, @Title, @Description, @PublishDate, @TargetUser)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand getMaxContentID = new SqlCommand("SELECT MAX(contentID) FROM Content", connection);

                SqlCommand command = new SqlCommand(insertQuery, connection);

                try
                {
                    connection.Open();
                    object result = getMaxContentID.ExecuteScalar();
                    int latestContentID = 0;

                    if (result != DBNull.Value)
                    {
                        latestContentID = Convert.ToInt32(result.ToString().Substring(1));
                    }

                    string newContentID = "C" + (latestContentID + 1).ToString("D3");

                    command.Parameters.AddWithValue("@ContentID", newContentID);
                    command.Parameters.AddWithValue("@Title", contentTitle);  
                    command.Parameters.AddWithValue("@Description", contentDescription);
                    command.Parameters.AddWithValue("@PublishDate", publishDateTime);
                    command.Parameters.AddWithValue("@TargetUser", targetUser);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Content saved successfully
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Content saved successfully');", true);
                    }
                    else
                    {
                        // Content not saved
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to save content');", true);
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception, log it, or provide feedback
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
                }
            }
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect back to adminDashboard.aspx with Content Management tab selected
            Response.Redirect("adminDashboard.aspx?tab=content");
        }
      
      
    }
}