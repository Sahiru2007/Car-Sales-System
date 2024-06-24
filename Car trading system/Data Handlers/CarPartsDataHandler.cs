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

        public CarPartsDataHandler(DatabaseHandler dbHandler)
        {
            this.dbHandler = dbHandler;
        }

        public bool InsertCarPartData(string partName, string manufacturer, int year, string model, decimal price,
                                      string category, int stockCount, byte[] imageData)
        {
            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    string partId = GetNextPartId(connection);
                    DateTime currentDate = DateTime.Now;

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
                dbHandler.CloseConnection(connection);
            }
        }

        private string GetNextPartId(SqlConnection connection)
        {
            try
            {
                string selectQuery = "SELECT MAX(PartID) FROM CarPart";
                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        string lastId = result.ToString();
                        int numericPart = int.Parse(lastId.Substring(2)) + 1; // Assuming IDs start with "CP"
                        return "CP" + numericPart;
                    }
                    else
                    {
                        return "CP1";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting next PartID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "CP0"; // Error ID, adjust as needed
            }
        }

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
                            dataTable.Load(reader);
                            dbHandler.CloseConnection(connection);
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

        public bool DeleteCarPart(string partID)
        {
            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM CarPart WHERE PartID = @PartID;", connection))
                    {
                        cmd.Parameters.AddWithValue("@PartID", partID);
                        int rowsAffected = cmd.ExecuteNonQuery();
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

        public bool UpdateCarData(string partID, string columnName, string editedValue)
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

                    using (var cmd = new SqlCommand($"UPDATE CarPart SET {columnName} = @EditedValue, DateUpdated = @DateUpdated WHERE PartID = @PartID;", connection))
                    {
                        cmd.Parameters.AddWithValue("@EditedValue", editedValue);
                        cmd.Parameters.AddWithValue("@PartID", partID);
                        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Car part updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                dbHandler.CloseConnection(connection);
            }
        }

        private bool IsValidDataType(string columnName, string editedValue)
        {
            var columnDataTypes = new Dictionary<string, Type>
            {
                { "Year", typeof(int) },
                { "Price", typeof(decimal) },
                { "StockCount", typeof(int) },
            };

            if (columnDataTypes.TryGetValue(columnName, out Type expectedType))
            {
                try
                {
                    Convert.ChangeType(editedValue, expectedType);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsColumnUpdateAllowed(string columnName)
        {
            string[] nonUpdateableColumns = { "PartID", "DateAdded", "DateUpdated" };
            return !nonUpdateableColumns.Contains(columnName);
        }

        public bool UpdateCarImageData(string partID, byte[] imageData)
        {
            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE CarPart SET ImageData = @ImageData WHERE PartID = @PartID;", connection))
                    {
                        cmd.Parameters.AddWithValue("@ImageData", imageData);
                        cmd.Parameters.AddWithValue("@PartID", partID);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating car part image data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }
        }
    }
}
