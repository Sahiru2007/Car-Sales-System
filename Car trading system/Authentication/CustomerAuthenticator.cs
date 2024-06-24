using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_trading_system.Authentication
{

    public class CustomerAuthenticator
    {
        private readonly DatabaseHandler dbHandler;

        public CustomerAuthenticator(DatabaseHandler dbHandler)
        {
            this.dbHandler = dbHandler;
        }

        // Authenticate the customer based on provided username and password
        // Authenticate the customer based on provided username and password
        public bool AuthenticateCustomer(string username, string password)
        {
            SqlConnection connection = null; // Declare SqlConnection at the method level

            try
            {
                // Open a database connection using the DatabaseHandler
                connection = dbHandler.OpenConnection();

                // Hash the entered password
                string hashedPassword = HashPassword(password);

                // Check the credentials in the database
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Customer WHERE Username = @Username AND Password = @PasswordHash;", connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    // If count is greater than 0, the credentials are valid
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions and show an error message
                MessageBox.Show($"Error authenticating customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                // Close the database connection using the DatabaseHandler
                dbHandler.CloseConnection(connection);
            }
        }

        // Hash the provided password using SHA-256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
