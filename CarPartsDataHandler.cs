using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_trading_system.Data_Handlers
{
    public class CarPartsDataHandler
    {

        private readonly DatabaseHandler dbHandler;
        private SqlConnection connection;

        // Constructor that takes a DatabaseHandler instance
        public CarPartsDataHandler(DatabaseHandler dbHandler)
        {
            // Initialize with the provided DatabaseHandler
            this.dbHandler = dbHandler;
        }
        // Method to insert car part data into the database
        public bool InsertCarPartData(string partName, string manufacturer, int year, string model, decimal price,
                                      string category, int stockCount, byte[] imageData)
        {
            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    // Generate PartID (incremental)
                    int partId = GetNextPartId(connection);

                    // Generate DateAdded and DateUpdated
                    DateTime currentDate = DateTime.Now;

                    // SQL command to insert car part data
                    string insertQuery = @"INSERT INTO CarPart
                                           (PartID, PartName, Manufacturer, Year, Model, Price, Category,
                                            StockCount, DateAdded, DateUpdated, ImageData)
                                           VALUES
                                           (@PartID, @PartName, @Manufacturer, @Year, @Model, @Price, @Category,
                                            @StockCount, @DateAdded, @DateUpdated, @ImageData)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@PartID", partId);
                        cmd.Parameters.AddWithValue("@PartName", partName);
                        cmd.Parameters.AddWithValue("@Manufacturer", manufacturer);
                        cmd.Parameters.AddWithValue("@Year", year);
                        cmd.Parameters.AddWithValue("@Model", model);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@StockCount", stockCount);
                        cmd.Parameters.AddWithValue("@DateAdded", currentDate);
                        cmd.Parameters.AddWithValue("@DateUpdated", currentDate);
                        cmd.Parameters.AddWithValue("@ImageData", imageData);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting car part data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (connection != null)
                {
                    dbHandler.CloseConnection(connection);
                }
            }
        }
        // Helper method to get the next available PartID
        private int GetNextPartId(SqlConnection connection)
        {
            try
            {
                // SQL command to get the maximum PartID from the database
                string selectQuery = "SELECT MAX(PartID) FROM CarPart";

                // Create a new SqlCommand with the select query and the open connection
                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    // Execute the query and get the result (maximum PartID)
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        // If the result is not DBNull, increment the maximum PartID
                        return Convert.ToInt32(result) + 1;
                    }
                    else
                    {
                        // If the table is empty, start with PartID = 1
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions and show an error message
                MessageBox.Show($"Error getting next PartID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        } // Helper method to get the next available PartID
          // Method to retrieve car part data from the database
        public DataTable GetCarPartData()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM CarPart;", connection))
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
                MessageBox.Show($"Error retrieving car part data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }

            return dataTable;
        }
        // Method to update car part data in the database
        public bool UpdateCarPartData(int partID, string columnName, string editedValue)
        {
            try
            {
                using (var connection = dbHandler.OpenConnection())
                {
                    // Check if the column is allowed to be updated
                    if (!IsColumnUpdateAllowed(columnName))
                    {
                        MessageBox.Show("Cannot update this column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Check if the edited value has the correct data type
                    if (!IsValidDataType(columnName, editedValue))
                    {
                        MessageBox.Show($"Incorrect data type for {columnName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Update the database with the edited value
                    using (var cmd = new SqlCommand($"UPDATE CarPart SET {columnName} = @EditedValue, DateUpdated = @DateUpdated WHERE PartID = @PartID;", connection))
                    {
                        cmd.Parameters.AddWithValue("@EditedValue", editedValue);
                        cmd.Parameters.AddWithValue("@PartID", partID);
                        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now); // Set DateUpdated to today's date

                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating car part data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (connection != null)
                {
                    dbHandler.CloseConnection(connection);
                }
            }
        }

        // Helper method to check if a column is allowed to be updated
        private bool IsColumnUpdateAllowed(string columnName)
        {
            // Define the list of columns that are not allowed to be updated
            string[] nonUpdateableColumns = { "PartID", "DateAdded", "DateUpdated" };

            // Check if the given column is in the non-updateable list
            return !Array.Exists(nonUpdateableColumns, col => col.Equals(columnName, StringComparison.OrdinalIgnoreCase));
        }
        // Helper method to check if a data type is valid for a given column
        private bool IsValidDataType(string columnName, string editedValue)
        {
            // Define the list of columns and their expected data types
            var columnDataTypes = new Dictionary<string, Type>
            {
                { "Year", typeof(int) },
                { "Price", typeof(decimal) },
                { "StockCount", typeof(int) },
                // Add other columns and their data types as needed
            };

            // Check if the given column is in the columnDataTypes dictionary
            if (columnDataTypes.TryGetValue(columnName, out Type expectedType))
            {
                // Attempt to convert the edited value to the expected data type
                try
                {
                    Convert.ChangeType(editedValue, expectedType);
                    return true;
                }
                catch (Exception)
                {
                    return false; // Conversion failed, incorrect data type
                }
            }

            // If the column is not found in the dictionary, assume any data type is valid
            return true;
        }
        public bool DeleteCarPart(int partID)
        {
            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    

                    using (SqlCommand cmd = new SqlCommand("DELETE FROM CarPart WHERE PartID = @PartID;", connection))
                    {
                        cmd.Parameters.AddWithValue("@PartID", partID);

                        // Execute the command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if the deletion was successful
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting car part with ID {partID}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }
        }

       

    }

}

