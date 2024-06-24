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
        private readonly CarPartsDataHandler carPartsDataHandler;
        private DataTable carPartsDataTable;
        public ManageCarPartForm()
        {
            InitializeComponent();

            // Initialize the CarPartsDataHandler
            carPartsDataHandler = new CarPartsDataHandler(new DatabaseHandler());

            // Load car parts data into DataGridView
            LoadCarPartData();

        }
        private void LoadCarPartData()
        {
            // Retrieve car parts data from the database
            carPartsDataTable = carPartsDataHandler.GetCarPartData();

            // Bind data to DataGridView
            dataGridViewCarParts.DataSource = carPartsDataTable;

            // Resize columns for better visibility
            dataGridViewCarParts.AutoResizeColumns();
        }

      

        private void ManageCarPartForm_Load(object sender, EventArgs e)
        {
            dataGridViewCarParts.DataError += dataGridViewCarParts_DataError;
            dataGridViewCarParts.CellClick += dataGridViewCars_CellClick;// Load car part data into the DataGridView
            LoadCarPartData();
        }

        private void dataGridViewCars_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid row and column index are clicked
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the value of the ImageData column for the selected row
                object imageDataObj = dataGridViewCarParts.Rows[e.RowIndex].Cells["ImageData"].Value;

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
            // Check if imageData is not null
            if (imageData != null && imageData.Length > 0)
            {
                // Convert byte array to Image
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image image = Image.FromStream(ms);
                    // Display the image in the PictureBox
                    picCarPreview.Image = image;
                }
            }
            else
            {
                // Clear the PictureBox if no image data is available
                picCarPreview.Image = null;
            }
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            // Add logic for adding a new car part
            if (!ValidateCarPartFields())
            {
                return;
            }

            // Retrieve data from UI elements
            string partName = txtPartName.Text.Trim();
            string manufacturer = txtManufacturer.Text.Trim();
            int year = Convert.ToInt32(txtYear.Text);
            string model = txtModel.Text.Trim();
            decimal price = Convert.ToDecimal(txtPrice.Text);
            string category = txtCategory.Text.Trim();
            int stockCount = Convert.ToInt32(txtStockCount.Text);

            // Retrieve image data from PictureBox
            byte[] imageData = null;
            if (pictureBoxCarPart.Image != null)
            {
                imageData = imageToByteArray(pictureBoxCarPart.Image);
            }

            // Call the appropriate method in CarPartsDataHandler to add a new car part
            bool success = carPartsDataHandler.InsertCarPartData(partName, manufacturer, year, model, price, category, stockCount, imageData);

            // Check if the insertion was successful
            if (success)
            {
                MessageBox.Show("Car part added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearCarPartFields();
                LoadCarPartData();
            }
            else
            {
                MessageBox.Show("Error adding car part. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

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

        private byte[] imageToByteArray(Image imageIn)
        {
            // Convert Image to byte array
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Png); // You can change the image format as needed
                return ms.ToArray();
            }
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            // Convert byte array to Image
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(ms);
            }
        }
        private bool ValidateCarPartFields()
        {
            // Check if at least one field is empty
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

            // Add additional validation logic as needed

            return true;
        }

        private void btnDeleteCarPart_Click(object sender, EventArgs e)
        {

            // Add logic for deleting a car part
            // Check if a row is selected in the DataGridView
            if (dataGridViewCarParts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a car part to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirm deletion with the user
            DialogResult result = MessageBox.Show("Are you sure you want to delete this car part?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Retrieve the selected car part's ID
                int selectedRowIndex = dataGridViewCarParts.SelectedRows[0].Index;
                int carPartId = Convert.ToInt32(dataGridViewCarParts.Rows[selectedRowIndex].Cells["PartID"].Value);

                // Call the appropriate method in CarPartsDataHandler to delete the car part
                bool success = carPartsDataHandler.DeleteCarPart(carPartId);

                // Check if the deletion was successful
                if (success)
                {
                    MessageBox.Show("Car part deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearCarPartFields();
                    LoadCarPartData();
                }
                else
                {
                    MessageBox.Show("Error deleting car part. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewCarParts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is FormatException)
            {
                // Display a custom error message
                MessageBox.Show("Invalid input. Please enter a valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Optionally, you can set the error to be handled, preventing the default error dialog
                e.Cancel = true;
            }
            else
            {
                // Handle other types of errors or log them as needed
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
                            // Get the selected file path
                            string imagePath = openFileDialog.FileName;

                            // Load the image into the PictureBox
                            Image selectedImage = Image.FromFile(imagePath);

                            // Convert the Image to a byte array
                            byte[] imageData = ImageToByteArray(selectedImage);

                            // Update the DataTable with the new image data
                            carPartsDataTable.Rows[e.RowIndex]["ImageData"] = imageData;

                            // Update the database with the new image data
                            int carID = Convert.ToInt32(carPartsDataTable.Rows[e.RowIndex]["CarID"]);
                            //carPartsDataHandler.UpdateCarImageData(carID, imageData);
                            MessageBox.Show("Car updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Refresh the DataGridView to reflect the changes
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
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg); // Change the format based on your requirements
                return ms.ToArray();
            }
        }
    }
}
