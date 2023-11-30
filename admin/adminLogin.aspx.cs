using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP
{
    public partial class adminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\FYP\FYP\FYP\App_Data\adminPart.mdf;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string selectQuery = "SELECT adminPassword, loginAttempts, isLocked, lockExpiration FROM Admin WHERE adminUsername = @Username";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        string passwordFromDB = reader["adminPassword"].ToString();
                        int loginAttempts = Convert.ToInt32(reader["loginAttempts"]);
                        bool isLocked = Convert.ToBoolean(reader["isLocked"]);
                        DateTime? lockExpiration = reader["lockExpiration"] != DBNull.Value ? Convert.ToDateTime(reader["lockExpiration"]) : (DateTime?)null;

                        if (isLocked && lockExpiration != null && lockExpiration > DateTime.Now)
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text = "Account locked. Try again later.";
                            return;
                        }

                        if (password == passwordFromDB)
                        {
                            // Successful login, reset login attempts and unlock account
                            ResetLoginAttempts(username);
                            UnlockAccount(username);

                            // Redirect to dashboard or home page after successful login
                            Response.Redirect("~/admin/adminDashboard.aspx");
                          
                        }
                        else
                        {
                            if (loginAttempts > 2)
                            {
                                // Lock the account after 3 failed attempts
                                LockAccount(username);
                                lblMessage.Visible = true;
                                lblMessage.Text = "Account locked. Try again later.";
                            }
                            else
                            {
                                IncrementLoginAttempts(username, loginAttempts);
                                lblMessage.Visible = true;
                                lblMessage.Text = "Invalid username or password!";
                            }
                        }
                    }
                    else
                    {
                        // Username not found
                        lblMessage.Visible = true;
                        lblMessage.Text = "Invalid username or password!";
                    }
                }
            }
        }



        private void IncrementLoginAttempts(string username, int currentAttempts)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\FYP\FYP\FYP\App_Data\adminPart.mdf;Integrated Security=True";
            string updateQuery = "UPDATE Admin SET loginAttempts = @Attempts WHERE adminUsername = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Attempts", currentAttempts + 1);
                    command.Parameters.AddWithValue("@Username", username);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Log or display the error message
                        // Log.Error("Error in IncrementLoginAttempts", ex);
                        throw; // Re-throw the exception to propagate it further
                    }
                }
            }
        }



        private void ResetLoginAttempts(string username)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\FYP\FYP\FYP\App_Data\adminPart.mdf;Integrated Security=True";
            string updateQuery = "UPDATE Admin SET loginAttempts = 0, isLocked = 0, lockExpiration = NULL WHERE adminUsername = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void UnlockAccount(string username)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\FYP\FYP\FYP\App_Data\adminPart.mdf;Integrated Security=True";
            string updateQuery = "UPDATE Admin SET isLocked = 0, lockExpiration = NULL, loginAttempts = 0 WHERE adminUsername = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        private void LockAccount(string username)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\FYP\FYP\FYP\App_Data\adminPart.mdf;Integrated Security=True";
            string updateQuery = "UPDATE Admin SET isLocked = 1, loginAttempts = 0, lockExpiration = DATEADD(MINUTE, 30, GETDATE()) WHERE adminUsername = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


    }

}
