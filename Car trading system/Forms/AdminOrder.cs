using Car_trading_system.Data_Handlers;
using Car_trading_system.Functions;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_trading_system.Forms
{
    public partial class AdminOrder : Form
    {
        private OrderDataHandler orderDataHandler;
        private DataTable ordersDataTable;
        public AdminOrder()
        {
            InitializeComponent();
            orderDataHandler = new OrderDataHandler(new DatabaseHandler());
            ordersDataTable = new DataTable();
           
            orderDataGridView.AutoGenerateColumns = true;

            // Set up DataGridView columns
            SetupDataGridViewColumns();

            // Load data from the database
            LoadCarData();

            // Set DataGridView properties
            orderDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            orderDataGridView.Width = 1150; 
            orderDataGridView.RowTemplate.Height = 50;
            updateStatus.Click += new EventHandler(updateStatus_Click);


        }
        private void SetupDataGridViewColumns()
        {
            // Add columns to the DataTable
            ordersDataTable.Columns.Add("OrderID", typeof(int));
            ordersDataTable.Columns.Add("ProductID", typeof(string));
            ordersDataTable.Columns.Add("Quantity", typeof(int));
            ordersDataTable.Columns.Add("Amount", typeof(decimal));
            ordersDataTable.Columns.Add("Status", typeof(string));
            ordersDataTable.Columns.Add("PaymentStatus", typeof(string));

         
            orderDataGridView.DataSource = ordersDataTable;
            // Set AutoSizeColumnsMode to DisplayedCells 
            orderDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

          
            foreach (DataGridViewColumn column in orderDataGridView.Columns)
            {
                column.MinimumWidth = 100;
            }
        }

        private void AdminOrder_Load(object sender, EventArgs e)
        {
            
         
            try
            {
                // Populate the OrderGridView 
                orderDataHandler.PopulateOrderDataGrid(orderDataGridView);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadCarData()
        {
           
            try
            {
                // Call the GetOrderData method from OrderDataHandler
                DataTable newordersDataTable = orderDataHandler.GetOrderData();

                
                ordersDataTable.Rows.Clear();

                // Import the new data into the existing table
                ordersDataTable.Merge(newordersDataTable);

                // Refresh the DataGridView to reflect the changes
                orderDataGridView.Refresh();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"Error loading order data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void orderDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (orderDataGridView.SelectedRows.Count > 0)
            {
             
                string productId = orderDataGridView.SelectedRows[0].Cells["ProductID"].Value.ToString();

            
                byte[] imageData = orderDataHandler.GetProductImage(productId);

                if (imageData != null && imageData.Length > 0)
                {
                    // Convert the byte array into an Image object and display it
                    using (var ms = new MemoryStream(imageData))
                    {
                        productImage.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                  
                    productImage.Image = null; 
                }
            }
        }

        private void updateStatus_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (orderDataGridView.SelectedRows.Count > 0)
            {
                // Check if an order status has been selected
                if (orderStatus.SelectedItem != null)
                {
                    int selectedRowIndex = orderDataGridView.SelectedRows[0].Index;
                    if (selectedRowIndex >= 0) 
                    {
                        int orderId = Convert.ToInt32(orderDataGridView.Rows[selectedRowIndex].Cells["OrderID"].Value);
                        string newStatus = orderStatus.SelectedItem.ToString();

                        bool updateResult = orderDataHandler.UpdateOrderStatus(orderId, newStatus);
                        if (updateResult)
                        {
                            MessageBox.Show("Order status updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Refresh the DataGridView to show the updated status
                            LoadCarData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update order status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    // No order status selected
                    MessageBox.Show("Please select an order status to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // No row selected
                MessageBox.Show("Please select an order to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && cmbSearchBy.SelectedIndex != -1)
            {
                string selectedColumn = cmbSearchBy.SelectedItem.ToString();
                string searchKeyword = txtSearch.Text;

                ApplyOrderFilter(selectedColumn, searchKeyword);
                orderDataGridView.Refresh();
            }
            else
            {
                MessageBox.Show("Please enter a search value and select a category", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ApplyOrderFilter(string columnName, string filterValue)
        {
            try
            {
                ordersDataTable.DefaultView.RowFilter = string.Empty; // Clear any existing filter

                // Handle filtering for 'Quantity'
                if (columnName == "Quantity" || columnName == "Amount")
                {
                    ordersDataTable.DefaultView.RowFilter = $"{columnName} = '{filterValue}'";
                   
                }          
                else // For partial matches
                {
                    ordersDataTable.DefaultView.RowFilter = $"{columnName} LIKE '%{filterValue}%'";
                }
                // Refresh the DataGridView to reflect the changes
                orderDataGridView.DataSource = typeof(DataTable); 
                orderDataGridView.DataSource = ordersDataTable; 

                orderDataGridView.Refresh();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void orderDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            CarsReport carReport = new CarsReport();
            carReport.Show();
        }
    }
}
