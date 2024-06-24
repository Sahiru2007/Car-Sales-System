using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Car_trading_system.Data_Handlers
{
    public class CarDataHandler
    {
        private readonly DatabaseHandler dbHandler;
        private SqlConnection connection;

        // Constructor that takes a DatabaseHandler instance
        public CarDataHandler(DatabaseHandler dbHandler)
        {
            // Initialize with the provided DatabaseHandler
            this.dbHandler = dbHandler;
        }

        // Method to insert car data into the database
        // Method to insert car data into the database with image
        public bool InsertCarData(string brand, string model, int year, string color, string bodyType,
                                  string transmissionType, string fuelType, decimal engineCapacity,
                                  int mileage, string condition, decimal price, int stockCount, byte[] imageData)
        {
            try
            {
                // Validate engine capacity
                if (!IsValidEngineCapacity(engineCapacity))
                {
                    MessageBox.Show("Invalid engine capacity. Please enter a valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                using (connection = dbHandler.OpenConnection())
                {
                    // Generate CarID (incremental)
                    string carId = GetNextCarId(connection);

                    // Generate DateAdded and DateUpdated
                    DateTime currentDate = DateTime.Now;

                    // SQL command to insert car data with image
                    string insertQuery = @"INSERT INTO Car
                                           (CarID, Brand, Model, Year, Color, BodyType, TransmissionType,
                                            FuelType, EngineCapacity, Mileage, Condition, Price, StockCount,
                                            DateAdded, DateUpdated, ImageData)
                                           VALUES
                                           (@CarID, @Brand, @Model, @Year, @Color, @BodyType, @TransmissionType,
                                            @FuelType, @EngineCapacity, @Mileage, @Condition, @Price, @StockCount,
                                            @DateAdded, @DateUpdated, @ImageData)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@CarID", carId);
                        cmd.Parameters.AddWithValue("@Brand", brand);
                        cmd.Parameters.AddWithValue("@Model", model);
                        cmd.Parameters.AddWithValue("@Year", year);
                        cmd.Parameters.AddWithValue("@Color", color);
                        cmd.Parameters.AddWithValue("@BodyType", bodyType);
                        cmd.Parameters.AddWithValue("@TransmissionType", transmissionType);
                        cmd.Parameters.AddWithValue("@FuelType", fuelType);
                        cmd.Parameters.AddWithValue("@EngineCapacity", engineCapacity);
                        cmd.Parameters.AddWithValue("@Mileage", mileage);
                        cmd.Parameters.AddWithValue("@Condition", condition);
                        cmd.Parameters.AddWithValue("@Price", price);
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
                MessageBox.Show($"Error inserting car data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        // Method to get the next available CarID
        private string GetNextCarId(SqlConnection connection)
        {
            try
            {
                string selectQuery = "SELECT MAX(CarID) FROM Car";
                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        string lastId = result.ToString();
                        // Assuming lastId is like "C3", extract the numeric part
                        int numericPart = int.Parse(lastId.Substring(1)); // Removes the first character and parses the rest
                        numericPart++; // Increment
                        return "C" + numericPart; // Create the new ID
                    }
                    else
                    {
                        // If the table is empty, start with "C1"
                        return "C1";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting next CarID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "C0"; // Return a default or error value
            }
        }

        private bool IsValidEngineCapacity(decimal engineCapacity)
        {
            // Convert the decimal to a string
            string engineCapacityStr = engineCapacity.ToString();

            // Split the string into integer and decimal parts
            string[] parts = engineCapacityStr.Split('.');

            // Check if the total length is within the specified limits
            return parts[0].Length <= 3 && (parts.Length == 1 || parts[1].Length <= 2);
        }
        public DataTable GetCarData()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Car;", connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Load the data into the DataTable
                            dataTable.Load(reader);
                            dbHandler.CloseConnection(connection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving car data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }

            return dataTable;
        }

        public bool DeleteCar(string carID)
        {
            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Car WHERE CarID = @CarID;", connection))
                    {
                        cmd.Parameters.AddWithValue("@CarID", carID);

                        // Execute the command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if the deletion was successful
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting car with ID {carID}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }
        }

        public bool UpdateCarData(string carID, string columnName, string editedValue)
        {
            try
            {
                using (var connection = dbHandler.OpenConnection())
                {
                    if (!IsColumnUpdateAllowed(columnName))
                    {
                        MessageBox.Show("Cannot update this column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (!IsValidDataType(columnName, editedValue))
                    {
                        MessageBox.Show($"Incorrect data type for {columnName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    string updateQuery = $"UPDATE Car SET {columnName} = @EditedValue, DateUpdated = @DateUpdated WHERE CarID = @CarID";
                    using (var cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@EditedValue", editedValue);
                        cmd.Parameters.AddWithValue("@CarID", carID);
                        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating car data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        private bool IsValidDataType(string columnName, string editedValue)
        {
            // Define the list of columns and their expected data types
            var columnDataTypes = new Dictionary<string, Type>
    {
        { "EngineCapacity", typeof(decimal) },
        { "Year", typeof(int) },
        { "Mileage", typeof(int) },
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

        // Add a new method to update the image data
        public bool UpdateCarImageData(string carID, byte[] imageData)
        {
            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    // SQL command to update the image data for a specific CarID
                    string updateQuery = "UPDATE Car SET ImageData = @ImageData WHERE CarID = @CarID";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        // Add parameters to the SQL command
                        cmd.Parameters.AddWithValue("@ImageData", imageData);
                        cmd.Parameters.AddWithValue("@CarID", carID);

                        // Execute the command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if the update was successful
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating car image data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }
        }


        // Helper method to check if a column is allowed to be updated
        private bool IsColumnUpdateAllowed(string columnName)
        {
            // Define the list of columns that are not allowed to be updated
            string[] nonUpdateableColumns = { "CarID", "DateCreated", "DateUpdated" };

            // Check if the given column is in the non-updateable list
            return !Array.Exists(nonUpdateableColumns, col => col.Equals(columnName, StringComparison.OrdinalIgnoreCase));
        }

    }

}
