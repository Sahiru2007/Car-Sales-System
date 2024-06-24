using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Car_trading_system
{
    public class DatabaseHandler
    {
        // Default constructor with an initial connection string
        private readonly string connectionString;
        public DatabaseHandler()
        {
            //The Connection String
            this.connectionString = "Data Source=DaphabDept1\\SQLEXPRESS;Initial Catalog=CarDealershipDB;Integrated Security=True;";
        }
        //Open a new database connection
        public SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        // Method to close an existing database connection
        public void CloseConnection(SqlConnection connection)
        {
            try
            {
                // Check if the provided connection is not null and is open
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions and show an error message
                MessageBox.Show($"Error closing database connection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public async Task<SqlConnection> OpenConnectionAsync()
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

    }
}
