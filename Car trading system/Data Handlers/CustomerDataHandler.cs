using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_trading_system
{

    public class CustomerDataHandler
    {
        private readonly DatabaseHandler dbHandler;
        private SqlConnection connection;
        // Constructor that takes a DatabaseHandler instance
        public CustomerDataHandler(DatabaseHandler dbHandler)
        {
            // Initialize with the provided DatabaseHandler
            this.dbHandler = dbHandler;
        }

        // Method to insert customer data into the database
        // Method to insert customer data into the database

        public bool IsUsernameUnique(string username)
        {
            try
            {
                // Open a new database connection using the DatabaseHandler
                using (var connection = dbHandler.OpenConnection())
                {
                    // Check if the username already exists in the database
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Customer WHERE Username = @Username;", connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        int count = (int)cmd.ExecuteScalar();

                        // If count is greater than 0, the username is not unique
                        return count == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions and show an error message
                MessageBox.Show($"Error checking username uniqueness: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                // Close the database connection using the DatabaseHandler
                if (connection != null)
                {
                    dbHandler.CloseConnection(connection);
                }
            }
        }
        public bool InsertCustomerData(string fullName, string address, string contactNumber, string emailAddress, string nic,
                                       DateTime dateOfBirth, string gender, string username, string password, byte[] imageData)
        {
            SqlConnection connection = null;

            try
            {
                // Open a new database connection using the DatabaseHandler
                connection = dbHandler.OpenConnection();

                // Generate a formatted six-digit ID (e.g., 000001, 000002, etc.)
                string formattedID = GenerateSixDigitID(connection);

                // Hash the password
                string hashedPassword = HashPassword(password);

                // Insert customer data into the database with the generated ID
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Customer (CustomerID, FullName, Address, ContactNumber, " +
                                                       "EmailAddress, DateOfBirth, Gender, NIC,Username, Password, ImageData,DateCreated) " +
                                                       "VALUES (@CustomerID, @FullName, @Address, @ContactNumber, @EmailAddress, " +
                                                       "@DateOfBirth, @Gender , @nic,@Username, @Password, @ImageData, @DateCreated);", connection))
                {
                    // Set parameters for the SQL command
                    cmd.Parameters.AddWithValue("@CustomerID", formattedID);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@ContactNumber", contactNumber);
                    cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    cmd.Parameters.AddWithValue("@nic", nic);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@ImageData", imageData);
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);

                    // Execute the command
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                // Handle any exceptions and show an error message
                MessageBox.Show("Error inserting customer data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                // Close the database connection using the DatabaseHandler
                if (connection != null)
                {
                    dbHandler.CloseConnection(connection);
                }
            }
        }

        // Method to generate a formatted six-digit ID
        private string GenerateSixDigitID(SqlConnection connection)
        {
            try
            {
                // Retrieve the maximum CustomerID from the database
                string maxIDQuery = "SELECT MAX(CAST(CustomerID AS INT)) FROM Customer;";
                using (SqlCommand maxIDCmd = new SqlCommand(maxIDQuery, connection))
                {
                    object result = maxIDCmd.ExecuteScalar();

                    // Check if there is an existing maximum ID
                    int currentMaxID = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                    // Increment the maximum ID and format it as a six-digit string
                    string formattedID = (currentMaxID + 1).ToString("D6");

                    return formattedID;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions and show an error message
                MessageBox.Show("Error generating customer ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null; // Return null in case of an error
            }
        }


        // Method to retrieve the current count of customers from the database
       
      
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public DataTable GetCustomerData()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CustomerID, FullName, Address, Gender, " +
                        "DateOfBirth,ContactNumber,EmailAddress,NIC,Username, ImageData, DateCreated FROM Customer;", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Load the data into the DataTable
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving customer data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }

            return dataTable;
        }
        public bool DeleteCustomer(string customerID)
        {
            try
            { 
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Customer WHERE CustomerID = @CustomerID;", connection))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);

                        // Execute the command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if the deletion was successful
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting car with ID {customerID}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }
        }
        public byte[] GetImageDataForUser(string username)
        {
            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT ImageData FROM Customer WHERE Username = @Username;", connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Check if the ImageData column is not null
                                if (!reader.IsDBNull(reader.GetOrdinal("ImageData")))
                                {
                                    // Retrieve the image data from the database
                                    return (byte[])reader["ImageData"];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving image data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }

            return null; // Return null in case of an error or if no image data is found
        }
        public string GetCustomerIDForUser(string username)
        {
            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CustomerID FROM Customer WHERE Username = @Username;", connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Check if the CustomerID column is not null
                                if (!reader.IsDBNull(reader.GetOrdinal("CustomerID")))
                                {
                                    // Retrieve the CustomerID from the database
                                    return (string)reader["CustomerID"];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving CustomerID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }
            return "";
            // Return -1 in case of an error or if no CustomerID is found
        }

    }

}
