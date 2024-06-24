using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Car_trading_system.Data_Handlers
{
    internal class OrderDataHandler
    {
        private readonly DatabaseHandler dbHandler;
        private SqlConnection connection;

        public OrderDataHandler(DatabaseHandler dbHandler)
        {
            this.dbHandler = dbHandler;
        }

        public int InsertOrderData(string customerId, string productId, int quantity, decimal amount, string orderDate, string status, string paymentStatus, string type)
        {
            int orderId = 0;

            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [Order] (CustomerID, ProductID, Quantity, Amount, OrderDate, Status, PaymentStatus) " +
                                                    "VALUES (@CustomerID, @ProductID, @Quantity, @Amount, @OrderDate, @Status, @PaymentStatus); SELECT SCOPE_IDENTITY();", connection))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerId);
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@Amount", amount);
                        cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            orderId = Convert.ToInt32(result); // Convert and store the result as OrderID
                        }

                        // Update stock count based on product type
                        if (type == "Car")
                        {
                            UpdateCarStockCount(productId, quantity);
                        }
                        else if (type == "CarPart")
                        {
                            UpdateCarPartStockCount(productId, quantity);
                        }

                        return orderId;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting order data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }
        }

        private void UpdateCarStockCount(string carId, int quantity)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Car SET StockCount = StockCount - @Quantity WHERE CarID = @CarID;", connection))
                {
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@CarID", carId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating car stock count: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCarPartStockCount(string partId, int quantity)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE CarPart SET StockCount = StockCount - @Quantity WHERE PartID = @PartID;", connection))
                {
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@PartID", partId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating car part stock count: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to populate the DataGridView with order data
        public void PopulateCustomerOrderDataGrid(DataGridView dataGridView, string customerID)
        {
            try
            {
                // Retrieve order data for the specified customer
                DataTable dataTable = GetCustomerOrderData(customerID);

                // Set the DataGridView data source to the retrieved DataTable
                dataGridView.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error populating order data grid: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable GetCustomerOrderData(string customerID)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    string query = "SELECT * FROM [Order] WHERE CustomerID = @CustomerID;";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }

                    // After fetching the basic order data, iterate over the rows to fetch images.
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string productId = row["ProductID"].ToString();
                        byte[] image = GetProductImage(productId); // Assuming you implement this method
                        if (image != null)
                        {
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving customer order data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }

            return dataTable;
        }

        public void PopulateOrderDataGrid(DataGridView dataGridView)
        {
            try
            {
                // Retrieve order data for the specified customer
                DataTable dataTable = GetOrderData();

                // Set the DataGridView data source to the retrieved DataTable
                dataGridView.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error populating order data grid: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public DataTable GetOrderData()
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    string query = "SELECT * FROM [Order]";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dataTable.Load(reader);
                            dataTable.Columns.Add("ProductDetails", typeof(string)); // Add a column for product details
                        }
                    }

                    // After fetching the basic order data, iterate over the rows to fetch and add product details
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string productId = row["ProductID"].ToString();
                        row["ProductDetails"] = GetProductDetails(productId); // Method to get and add product details
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving order data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }

            return dataTable;
        }

        public string GetProductDetails(string productId)
        {
            string productDetails = "";

            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;

                    if (productId.StartsWith("C") && !productId.StartsWith("CP")) // For Car
                    {
                        cmd.CommandText = "SELECT Brand, Model FROM Car WHERE CarID = @ID";
                    }
                    else if (productId.StartsWith("CP")) // For CarPart
                    {
                        cmd.CommandText = "SELECT PartName FROM CarPart WHERE PartID = @ID";
                    }
                    else
                    {
                        return "Invalid ProductID";
                    }

                    cmd.Parameters.AddWithValue("@ID", productId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (productId.StartsWith("C") && !productId.StartsWith("CP"))
                            {
                                productDetails = $"{reader["Brand"]} {reader["Model"]}";
                            }
                            else if (productId.StartsWith("CP"))
                            {
                                productDetails = reader["PartName"].ToString();
                            }
                        }
                        else
                        {
                            productDetails = "Product details not found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving product details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                productDetails = "Error fetching product details";
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }

            return productDetails;
        }


        // The GetProductImage method as previously described

        public byte[] GetProductImage(string productId)
        {
            byte[] imageData = null;

            // Only proceed if the ProductID has a valid format
            if (!productId.StartsWith("C"))
            {
                return null; // Invalid ProductID format, return null immediately.
            }

            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    SqlCommand cmd = null;

                    // Determine the source table and column based on the ProductID prefix
                    if (productId.StartsWith("C") && !productId.StartsWith("CP")) // Car
                    {
                        cmd = new SqlCommand("SELECT ImageData FROM Car WHERE CarID = @ID", connection);
                    }
                    else if (productId.StartsWith("CP")) // CarPart
                    {
                        cmd = new SqlCommand("SELECT ImageData FROM CarPart WHERE PartID = @ID", connection);
                    }

                    // If cmd is null, the ProductID format is not supported
                    if (cmd == null)
                    {
                        return null;
                    }

                    cmd.Parameters.AddWithValue("@ID", productId);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        imageData = (byte[])result;
                    }
                }
            }
            catch (Exception ex)
            {
                // Consider logging the error instead of showing a message box for a library method.
                // MessageBox.Show($"Error retrieving product image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Error retrieving product image: {ex.Message}"); // Logging the error
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }

            return imageData;
        }

        public bool UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                using (connection = dbHandler.OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE [Order] SET Status = @NewStatus WHERE OrderID = @OrderID", connection))
                    {
                        cmd.Parameters.AddWithValue("@NewStatus", newStatus);
                        cmd.Parameters.AddWithValue("@OrderID", orderId);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                dbHandler.CloseConnection(connection);
            }
        }

    }
}
    

