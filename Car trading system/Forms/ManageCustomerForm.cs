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
    public partial class ManageCustomerForm : Form
    {
        private readonly DatabaseHandler dbHandler;
        private readonly CustomerDataHandler customerHandler;
        private DataTable customerDataTable;

        public ManageCustomerForm()
        {
            InitializeComponent();
            dbHandler = new DatabaseHandler();
            customerHandler = new CustomerDataHandler(dbHandler);

            // Initialize DataTable for storing customer data
            customerDataTable = new DataTable();

            // Set up DataGridView columns
            SetupDataGridViewColumns();

            // Load customer data from the database
            LoadCustomerData();
        }
        private void SetupDataGridViewColumns()
        {
            customerDataTable.Columns.Add("CustomerID", typeof(string));
            customerDataTable.Columns.Add("FullName", typeof(string));
            customerDataTable.Columns.Add("Address", typeof(string));
            customerDataTable.Columns.Add("Gender", typeof(string));
            customerDataTable.Columns.Add("DateOfBirth", typeof(DateTime));
            customerDataTable.Columns.Add("ContactNumber", typeof(string));
            customerDataTable.Columns.Add("EmailAddress", typeof(string));
            customerDataTable.Columns.Add("NIC", typeof(string));
            customerDataTable.Columns.Add("Username", typeof(string));          
            customerDataTable.Columns.Add("DateCreated", typeof(DateTime));
            customerDataTable.Columns.Add("ImageData", typeof(byte[]));


            // Set the DataTable as the DataGridView's data source
            dataGridViewCustomer.DataSource = customerDataTable;
        }
        private void LoadCustomerData()
        {
            try
            {
                // Call the GetCustomerData method from CustomerDataHandler
                DataTable newCustomerDataTable = customerHandler.GetCustomerData();

                // Clear the existing rows in the table
                customerDataTable.Rows.Clear();

                // Import the new data into the existing table
                customerDataTable.Merge(newCustomerDataTable);

                // Refresh the DataGridView to reflect the changes
                dataGridViewCustomer.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customer data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            // Get the selected rows
            DataGridViewSelectedRowCollection selectedRows = dataGridViewCustomer.SelectedRows;

            if (selectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete the selected customer(s)?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in selectedRows)
                    {
                        // Get the CarID from the selected row
                        string customerID = (string)row.Cells["CustomerID"].Value;


                        // Delete the car using CarDataHandler
                        if (customerHandler.DeleteCustomer(customerID))
                        {
                            DataRow dataRow = ((DataRowView)row.DataBoundItem).Row;
                            customerDataTable.Rows.Remove(dataRow);
                        }
                        else
                        {
                            MessageBox.Show($"Failed to delete car with ID {customerID}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    dataGridViewCustomer.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select at least one row to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && !(cmbSearchBy.SelectedIndex == -1))
            {
                // Get the selected search column
                string selectedColumn = cmbSearchBy.SelectedItem.ToString();

                // Get the search keyword
                string searchKeyword = txtSearch.Text;

                ApplyFilter(selectedColumn, searchKeyword);
            }
            else
            {
                MessageBox.Show("Please enter a search value and a category", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ApplyFilter(string columnName, string filterValue)
        {
            try
            {
                // Clear the existing filter
                customerDataTable.DefaultView.RowFilter = string.Empty;

                // Check if the search value is for CustomerID
                if (columnName == "CustomerID")
                {

                    customerDataTable.DefaultView.RowFilter = $"{columnName} = '{filterValue}'";
                }
                else
                {

                    customerDataTable.DefaultView.RowFilter = $"{columnName} LIKE '%{filterValue}%'";
                }

                dataGridViewCustomer.Refresh();
            }
            catch (Exception ex)
            {
            
                MessageBox.Show($"Error applying filter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid row and column index are clicked
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the value of the ImageData column for the selected row
                object imageDataObj = dataGridViewCustomer.Rows[e.RowIndex].Cells["ImageData"].Value;

                // Check if the value is DBNull
                if (imageDataObj != DBNull.Value)
                {
                    // Convert the value to byte[] and display the image
                    byte[] imageData = (byte[])imageDataObj;
                    DisplayImageInPictureBox(imageData);
                }

            }
        }
        private void DisplayImageInPictureBox(byte[] imageData)
        {
           
        }
    }
}
