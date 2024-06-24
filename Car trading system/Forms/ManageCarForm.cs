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
    public partial class ManageCarForm : Form
    {
        private readonly DatabaseHandler dbHandler;
        private readonly CarDataHandler carHandler;
        private DataTable carsDataTable;
        public ManageCarForm()
        {
            InitializeComponent();
            // Initialize the DatabaseHandler with the default connection string
            dbHandler = new DatabaseHandler();

           
            carHandler = new CarDataHandler(dbHandler);
            carsDataTable = new DataTable();

            dataGridViewCars.AutoGenerateColumns = true;

            // Set up DataGridView columns
            SetupDataGridViewColumns();

            // Load data from the database
            LoadCarData();


            // Detach event handlers 
            dataGridViewCars.CellEndEdit -= dataGridViewCars_CellEndEdit;
            dataGridViewCars.CellContentClick -= dataGridViewCars_CellContentClick;

            // Hook up the CellEndEdit event
            dataGridViewCars.CellEndEdit += dataGridViewCars_CellEndEdit;

            // Hook up the CellContentClick event
            dataGridViewCars.CellContentClick += dataGridViewCars_CellContentClick;


        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            // Validate form fields
            if (ValidateFields())
            {
                string brand = txtBrand.Text;
                string model = txtModel.Text;
                int year = Convert.ToInt32(txtYear.Text);
                string color = txtColor.Text;
                string bodyType = txtBodyType.Text;
                string transmissionType = txtTransmissionType.SelectedItem.ToString();
                string fuelType = txtFuelType.SelectedItem.ToString();
                decimal engineCapacity = Convert.ToDecimal(txtEngineCapacity.Text);
                int mileage = Convert.ToInt32(txtMileage.Text);
                string condition = radNew.Checked ? "New" : "Used";
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int stockCount = Convert.ToInt32(txtStockCount.Text);
                byte[] imageData = ImageToByteArray(imgCar.Image);

                // Insert data into the database using the CarDataHandler
                if (carHandler.InsertCarData(brand, model, year, color, bodyType, transmissionType,
                                             fuelType, engineCapacity, mileage, condition, price, stockCount, imageData))
                {
                    // Successful 
                    MessageBox.Show("Car added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear form fields
                    ClearFields();
                    // Load updated car data into the DataGridView
                    LoadCarData();
                }
                else
                {
                    
                    MessageBox.Show("Failed to add car. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
        private bool ValidateFields()
        {
            // Check if at least one field is empty
            if (string.IsNullOrWhiteSpace(txtBrand.Text) ||
                string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtYear.Text) ||
                string.IsNullOrWhiteSpace(txtColor.Text) ||
                 string.IsNullOrWhiteSpace(txtBodyType.Text) ||
                txtTransmissionType.SelectedIndex == -1 ||
                txtFuelType.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtEngineCapacity.Text) ||
                string.IsNullOrWhiteSpace(txtMileage.Text) ||
                (!radNew.Checked && !radUsed.Checked) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtStockCount.Text))
            {
                MessageBox.Show("All fields are required. Please fill out all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }

        private void ClearFields()
        {
            // Clear all form fields
            txtBrand.Clear();
            txtModel.Clear();
            txtYear.Clear();
            txtColor.Clear();
            txtBodyType.Clear();
            txtTransmissionType.SelectedIndex = -1;
            txtFuelType.SelectedIndex = -1;
            txtEngineCapacity.Clear();
            txtMileage.Clear();
            radNew.Checked = false;
            radUsed.Checked = false;
            txtPrice.Clear();
            txtStockCount.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        
        private void SetupDataGridViewColumns()
        {
            // Add columns to the DataTable
            carsDataTable.Columns.Add("CarID", typeof(string));
            carsDataTable.Columns.Add("Brand", typeof(string));
            carsDataTable.Columns.Add("Model", typeof(string));
            carsDataTable.Columns.Add("Year", typeof(int));
            carsDataTable.Columns.Add("Color", typeof(string));
            carsDataTable.Columns.Add("BodyType", typeof(string));
            carsDataTable.Columns.Add("TransmissionType", typeof(string));
            carsDataTable.Columns.Add("FuelType", typeof(string));
            carsDataTable.Columns.Add("EngineCapacity", typeof(decimal));
            carsDataTable.Columns.Add("Mileage", typeof(int));
            carsDataTable.Columns.Add("Condition", typeof(string));
            carsDataTable.Columns.Add("Price", typeof(decimal));
            carsDataTable.Columns.Add("StockCount", typeof(int));
            carsDataTable.Columns.Add("DateAdded", typeof(DateTime));
            carsDataTable.Columns.Add("DateUpdated", typeof(DateTime));

            // Set the DataTable as the DataGridView's data source
            dataGridViewCars.DataSource = carsDataTable;

            dataGridViewCars.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

         
            foreach (DataGridViewColumn column in dataGridViewCars.Columns)
            {
                column.MinimumWidth = 100;
            }

        }




        private void LoadCarData()
        {
            try
            {
                // Call the GetCarData method from CarDataHandler
                DataTable newCarsDataTable = carHandler.GetCarData();

                // Clear the existing rows in the table
                carsDataTable.Rows.Clear();

                // Import the new data into the existing table
                carsDataTable.Merge(newCarsDataTable);

                // Refresh the DataGridView to reflect the changes
                dataGridViewCars.Refresh();
            }
            catch (Exception ex)
            {
               
                MessageBox.Show($"Error loading car data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            // Get the selected rows
            DataGridViewSelectedRowCollection selectedRows = dataGridViewCars.SelectedRows;

            if (selectedRows.Count > 0)
            {

                DialogResult result = MessageBox.Show("Are you sure you want to delete the selected car(s)?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in selectedRows)
                    {
                        // Get the CarID from the selected row as a string
                        string carID = row.Cells["CarID"].Value.ToString();

                        // Delete the car using CarDataHandler
                        if (carHandler.DeleteCar(carID))
                        {
                            // Remove the row from the DataTable
                            DataRow dataRow = ((DataRowView)row.DataBoundItem).Row;
                            carsDataTable.Rows.Remove(dataRow);
                        }
                        else
                        {
                            MessageBox.Show($"Failed to delete car with ID {carID}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // Refresh the DataGridView
                    dataGridViewCars.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select at least one row to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridViewCars_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell editedCell = dataGridViewCars.Rows[e.RowIndex].Cells[e.ColumnIndex];

            string editedValue = editedCell.Value?.ToString() ?? "";

            // Get the CarID 
            string carID = dataGridViewCars.Rows[e.RowIndex].Cells["CarID"].Value.ToString();

            // Get the column name
            string columnName = dataGridViewCars.Columns[e.ColumnIndex].Name;

            // Update the database 
            if (!carHandler.UpdateCarData(carID, columnName, editedValue))
            {
                MessageBox.Show("Failed to update car data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Car data updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void dataGridViewCars_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtSearch.Text) && !(cmbSearchBy.SelectedIndex == -1))
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
                MessageBox.Show("Please enter a search value and a category", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ApplyFilter(string columnName, string filterValue)
        {
            try
            {
                // Clear the existing filter
                carsDataTable.DefaultView.RowFilter = string.Empty;

                // Check if the search value is for CustomerID
                if (columnName == "CarID")
                {
                    
                    carsDataTable.DefaultView.RowFilter = $"{columnName} = '{filterValue}'";
                }
                else
                {
                    // For other columns, use 'LIKE' for partial matches
                    carsDataTable.DefaultView.RowFilter = $"{columnName} LIKE '%{filterValue}%'";
                }

                // Refresh the DataGridView to reflect the changes
                dataGridViewCars.Refresh();
            }
            catch (Exception ex)
            {
                // Handle any exceptions and show an error message
                MessageBox.Show($"Error applying filter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        imgCar.Image = Image.FromFile(imagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dataGridViewCars_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { 
            // To run the browse function when they presses on the image to update it
            if (e.ColumnIndex == dataGridViewCars.Columns["ImageData"].Index && e.RowIndex >= 0)
            {
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
                            Image selectedImage = Image.FromFile(imagePath);

                            // Convert the Image to a byte array
                            byte[] imageData = ImageToByteArray(selectedImage);

                            // Update the DataTable with the new image data
                            carsDataTable.Rows[e.RowIndex]["ImageData"] = imageData;

                            // Get the CarID of the selected row as a string
                            string carID = dataGridViewCars.Rows[e.RowIndex].Cells["CarID"].Value.ToString();

                            // Update the database with the new image data
                            if (carHandler.UpdateCarImageData(carID, imageData))
                            {
                                MessageBox.Show("Car image updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                dataGridViewCars.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update car image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void ManageCarForm_Load(object sender, EventArgs e)
        {
            dataGridViewCars.DataError += dataGridViewCars_DataError;
            dataGridViewCars.CellClick += dataGridViewCars_CellClick;
        }

        private void dataGridViewCars_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is FormatException)
            {
                // Display a custom error message
                MessageBox.Show("Invalid input. Please enter a valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
           
        }

        private void dataGridViewCars_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
               
                object imageDataObj = dataGridViewCars.Rows[e.RowIndex].Cells["ImageData"].Value;

               
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
            //adding the image to picturebox

            if (imageData != null && imageData.Length > 0)
            {

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

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            CarReport carReport = new CarReport();
            carReport.Show();
        }
    }
}

