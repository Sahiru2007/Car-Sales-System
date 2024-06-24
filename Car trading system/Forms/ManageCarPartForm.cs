using Car_trading_system.Data_Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace Car_trading_system.Forms
{
    public partial class ManageCarPartForm : Form
    {
        private readonly DatabaseHandler dbHandler;
        private readonly CarPartsDataHandler carPartsHandler;
        private DataTable carPartsDataTable;
        public ManageCarPartForm()
        {
            InitializeComponent();
            // Initialize the DatabaseHandler 
            dbHandler = new DatabaseHandler();
            carPartsHandler = new CarPartsDataHandler(dbHandler);
            // Initialize DataTable for storing car data
            carPartsDataTable = new DataTable();

            dataGridViewCarParts.AutoGenerateColumns = true;
            // Set up DataGridView columns
            SetupDataGridViewColumns();

            // Load data from the database
            LoadCarPartData();


        }
        private void ManageCarPartForm_Load(object sender, EventArgs e)
        {

        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearCarPartFields();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog to allow the user to select an image
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Get the selected file path
                        string imagePath = openFileDialog.FileName;

                        // Load the image into the PictureBox
                        pictureBoxCarPart.Image = Image.FromFile(imagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ClearCarPartFields()
        {
            // Clear textboxes and PictureBox
            txtPartName.Clear();
            txtManufacturer.Clear();
            txtYear.Clear();
            txtModel.Clear();
            txtPrice.Clear();
            txtCategory.Clear();
            txtStockCount.Clear();
            pictureBoxCarPart.Image = null;
        }

       
        private void btnAddCar_Click(object sender, EventArgs e)
        {
            // Validate fields
            if (ValidateFields())
            {
                string partName = txtPartName.Text.Trim();
                string manufacturer = txtManufacturer.Text.Trim();
                int year = Convert.ToInt32(txtYear.Text);
                string model = txtModel.Text.Trim();
                decimal price = Convert.ToDecimal(txtPrice.Text);
                string category = txtCategory.Text.Trim();
                int stockCount = Convert.ToInt32(txtStockCount.Text);

                // Convert image to byte array
                byte[] imageData = ImageToByteArray(pictureBoxCarPart.Image);

    
                if (carPartsHandler.InsertCarPartData(partName, manufacturer, year, model, price, category, stockCount, imageData))
                {
                    MessageBox.Show("Car part added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to add car part.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtPartName.Text) ||
                 string.IsNullOrWhiteSpace(txtManufacturer.Text) ||
                 string.IsNullOrWhiteSpace(txtYear.Text) ||
                 string.IsNullOrWhiteSpace(txtModel.Text) ||
                 string.IsNullOrWhiteSpace(txtPrice.Text) ||
                 string.IsNullOrWhiteSpace(txtCategory.Text) ||
                 string.IsNullOrWhiteSpace(txtStockCount.Text))
            {
                MessageBox.Show("All fields are required. Please fill out all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            int year;
            decimal price;
            int stockCount;

           
            if (!int.TryParse(txtYear.Text, out year) || year < 1900 || year > DateTime.Now.Year + 1)
            {
                MessageBox.Show("Invalid year. Please enter a valid year.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
            {
                MessageBox.Show("Invalid price. Please enter a valid price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            
            if (!int.TryParse(txtStockCount.Text, out stockCount) || stockCount < 0)
            {
                MessageBox.Show("Invalid stock count. Please enter a valid stock count.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }

        private void SetupDataGridViewColumns()
        {
            // Add columns to the DataTable
            carPartsDataTable.Columns.Add("PartID", typeof(string));
            carPartsDataTable.Columns.Add("PartName", typeof(string));
            carPartsDataTable.Columns.Add("Manufacturer", typeof(string));
            carPartsDataTable.Columns.Add("Year", typeof(int));
            carPartsDataTable.Columns.Add("Model", typeof(string));
            carPartsDataTable.Columns.Add("Price", typeof(decimal));
            carPartsDataTable.Columns.Add("Category", typeof(string));
            carPartsDataTable.Columns.Add("StockCount", typeof(int));
            carPartsDataTable.Columns.Add("DateAdded", typeof(DateTime));
            carPartsDataTable.Columns.Add("DateUpdated", typeof(DateTime));

            dataGridViewCarParts.DataSource = carPartsDataTable;
        }
        private void LoadCarPartData()
        {
            try
            {
                // Call the GetCarPartData method from CarPartsDataHandler
                DataTable newCarPartsDataTable = carPartsHandler.GetCarPartData();

                // Clear the existing rows in the table
                carPartsDataTable.Rows.Clear();

                // Import the new data into the existing table
                carPartsDataTable.Merge(newCarPartsDataTable);

                // Refresh the DataGridView to reflect the changes
                dataGridViewCarParts.Refresh();
            }
            catch (Exception ex)
            {
                // Handle any exceptions and show an error message
                MessageBox.Show($"Error loading car part data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private byte[] ImageToByteArray(Image image)
        {
            // Convert Image to byte array
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private void btnDeleteCarPart_Click(object sender, EventArgs e)
        {
            // Get the selected rows
            DataGridViewSelectedRowCollection selectedRows = dataGridViewCarParts.SelectedRows;

            if (selectedRows.Count > 0)
            {
   
                DialogResult result = MessageBox.Show("Are you sure you want to delete the selected car part(s)?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in selectedRows)
                    {
                     
                        string partID = row.Cells["PartID"].Value.ToString();

                        // Delete the car using CarDataHandler
                        if (carPartsHandler.DeleteCarPart(partID))
                        {
                            // Remove the row from the DataTable
                            DataRow dataRow = ((DataRowView)row.DataBoundItem).Row;
                            carPartsDataTable.Rows.Remove(dataRow);
                        }
                        else
                        {
                            MessageBox.Show($"Failed to delete the car part(s) with ID {partID}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // Refresh the DataGridView
                    dataGridViewCarParts.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select at least one row to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewCarParts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the "ImageData" column
            if (e.ColumnIndex == dataGridViewCarParts.Columns["ImageData"].Index && e.RowIndex >= 0)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                    openFileDialog.Title = "Select an Image";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                           
                            string imagePath = openFileDialog.FileName;

                            // Load the image into the PictureBox
                            Image selectedImage = Image.FromFile(imagePath);

                            // Convert the Image to a byte array
                            byte[] imageData = ImageToByteArray(selectedImage);

                            // Update the DataTable with the new image data
                            carPartsDataTable.Rows[e.RowIndex]["ImageData"] = imageData;

                            // Update the database with the new image data
                            string partID = dataGridViewCarParts.Rows[e.RowIndex].Cells["PartID"].Value.ToString();
                            carPartsHandler.UpdateCarImageData(partID, imageData);
                            MessageBox.Show("Car part updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dataGridViewCarParts.Refresh();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void dataGridViewCarParts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid row and column index are clicked
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
              
                object imageDataObj = dataGridViewCarParts.Rows[e.RowIndex].Cells["ImageData"].Value;
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
            // Check if imageData is not null
            if (imageData != null && imageData.Length > 0)
            {
                // Convert byte array to Image
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image image = Image.FromStream(ms);
                    picCarPreview.Image = image;
                }
            }
            else
            {
                picCarPreview.Image = null;
            }
        }

        private void dataGridViewCarParts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            // Get the edited cell
            DataGridViewCell editedCell = dataGridViewCarParts.Rows[e.RowIndex].Cells[e.ColumnIndex];

            // Get the edited value
            string editedValue = editedCell.Value.ToString();

            string partID = dataGridViewCarParts.Rows[e.RowIndex].Cells["PartID"].Value.ToString();
 
            string columnName = dataGridViewCarParts.Columns[e.ColumnIndex].Name;

            // Update the database with the edited value
            carPartsHandler.UpdateCarData(partID, columnName, editedValue);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text)  && !(cmbSearchBy.SelectedIndex == -1))
            {
                // Get the selected search column
                string selectedColumn = cmbSearchBy.SelectedItem.ToString();

                // Get the search keyword
                string searchKeyword = txtSearch.Text;

                // Apply the filter to the DataTable
                ApplyFilter(selectedColumn, searchKeyword);
            }
            else
            {
                // Handle any exceptions and show an error message
                MessageBox.Show("Please enter a search value and a category", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ApplyFilter(string columnName, string filterValue)
        {
            try
            {
                // Clear the existing filter
                carPartsDataTable.DefaultView.RowFilter = string.Empty;

                // Check if the search value is for CustomerID
                if (columnName == "PartID")
                {
                    carPartsDataTable.DefaultView.RowFilter = $"{columnName} = '{filterValue}'";
                }
                else
                {
                    carPartsDataTable.DefaultView.RowFilter = $"{columnName} LIKE '%{filterValue}%'";
                }

                // Refresh the DataGridView to reflect the changes
                dataGridViewCarParts.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error applying filter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            CarPartReport carReport = new CarPartReport();
            carReport.Show();
        }
    }
}
