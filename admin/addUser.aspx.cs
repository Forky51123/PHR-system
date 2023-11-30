using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace FYP
{
    public partial class addUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='E:\PHR System\PHR System\PHR System\App_Data\PHR.mdf';Integrated Security=True;Connect Timeout=30";

            // Check the selected user type
            string userType = ddl_userType.SelectedValue;

            if (userType == "Patient")
            {
                AddPatient(connectionString);
            }
            else if (userType == "Third-Party")
            {
                AddThirdParty(connectionString);
            }
        }

        private string GenerateNextPatientID(string connectionString)
        {
            string query = "SELECT MAX(CAST(SUBSTRING(patientID, 2, LEN(patientID))) AS INT) FROM patient";
            string newPatientID = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();

                int latestPatientID = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                newPatientID = "P" + (latestPatientID + 1).ToString("D6"); // Assuming IDs like P000001 or T000001
            }

            return newPatientID;
        }

        private string GenerateNextThirdPartyID(string connectionString)
        {
            string query = "SELECT MAX(CAST(SUBSTRING(thirdID, 2, LEN(thirdID))) AS INT) FROM thirdParty";
            string newThirdPartyID = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();

                int latestThirdPartyID = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                newThirdPartyID = "T" + (latestThirdPartyID + 1).ToString("D6"); // Assuming IDs like P000001 or T000001
            }

            return newThirdPartyID;
        }

        private void AddPatient(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string insertPatientQuery = "INSERT INTO patient (patientID, patientIC, patientEmail, passwd, securityQues, securityAns, role) " +
                                            "VALUES (@PatientID, @PatientIC, @PatientEmail, @Passwd, @SecurityQuestion, @SecurityAnswer, @Role)";

                try
                {
                    connection.Open();

                    string newPatientID = GenerateNextPatientID(connectionString);

                    using (SqlCommand command = new SqlCommand(insertPatientQuery, connection))
                    {
                        command.Parameters.AddWithValue("@PatientID", newPatientID);
                        command.Parameters.AddWithValue("@PatientIC", DBNull.Value);
                        command.Parameters.AddWithValue("@PatientEmail", DBNull.Value);
                        command.Parameters.AddWithValue("@Passwd", DBNull.Value);
                        command.Parameters.AddWithValue("@SecurityQuestion", DBNull.Value);
                        command.Parameters.AddWithValue("@SecurityAnswer", DBNull.Value);
                        command.Parameters.AddWithValue("@Role", "patient");

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Patient added successfully
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Patient added successfully');", true);
                            // Redirect or further actions as needed
                        }
                        else
                        {
                            // Handle insertion failure
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to add patient');", true);
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

        private void AddThirdParty(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string insertThirdPartyQuery = "INSERT INTO thirdParty (thirdID, thirdName, thirdGender, address, thirdEmail, thirdPhoneNum, password, secQues, secAns, company, role) " +
                                               "VALUES (@ThirdID, @ThirdName, @ThirdGender, @Address, @ThirdEmail, @ThirdPhoneNum, @Password, @SecQues, @SecAns, @Company, @Role)";

                try
                {
                    connection.Open();

                    string newThirdID = GenerateNextThirdPartyID(connectionString);

                    using (SqlCommand command = new SqlCommand(insertThirdPartyQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ThirdID", newThirdID);
                        command.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                        command.Parameters.AddWithValue("@ThirdGender", DBNull.Value);
                        command.Parameters.AddWithValue("@Address", DBNull.Value);
                        command.Parameters.AddWithValue("@ThirdEmail", DBNull.Value);
                        command.Parameters.AddWithValue("@ThirdPhoneNum", DBNull.Value);
                        command.Parameters.AddWithValue("@Password", DBNull.Value);
                        command.Parameters.AddWithValue("@SecQues", DBNull.Value);
                        command.Parameters.AddWithValue("@SecAns", DBNull.Value);
                        command.Parameters.AddWithValue("@Company", DBNull.Value);
                        command.Parameters.AddWithValue("@Role", "third-party");

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Third-party added successfully
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Third-party added successfully');", true);
                            // Redirect or further actions as needed
                        }
                        else
                        {
                            // Handle insertion failure
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to add third-party');", true);
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





        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminDashboard.aspx?tab=userManage");
        }
    }
}
