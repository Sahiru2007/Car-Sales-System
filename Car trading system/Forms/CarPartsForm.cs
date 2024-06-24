using Car_trading_system.CustomControls;
using Car_trading_system.Data_Handlers;
using Car_trading_system.Functions;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_trading_system.Forms
{
    public partial class CarPartsForm : Form
   {
        private readonly DatabaseHandler dbHandler;
        private readonly OrderDataHandler orderDataHandler;
        private DataRow carData;
        
        private CarPartsDataHandler carPartsDataHandler;
        public CarPartsForm()
        {
            InitializeComponent();
            dbHandler = new DatabaseHandler();
            orderDataHandler = new OrderDataHandler(dbHandler);
            carPartsDataHandler = new CarPartsDataHandler(new DatabaseHandler());
            LoadCarPartsStore();
        }


        private void LoadCarPartsStore(string searchQuery = "", string selectedCategory = "")
        {
            flowLayoutPanelCars.Controls.Clear(); // Clear existing controls
            flowLayoutPanelCars.WrapContents = true;

            DataTable carPartsDataTable = carPartsDataHandler.GetCarPartData();

            // Apply filter if searchQuery is not empty
            if (!string.IsNullOrEmpty(searchQuery) && !string.IsNullOrEmpty(selectedCategory))
            {
                // Build filter string based on the selected category
                string filterExpression = "";
                switch (selectedCategory)
                {
                    case "PartID":
                    case "PartName":
                    case "Manufacturer":
                    case "Model":
                    case "Category":
                        filterExpression = $"{selectedCategory} LIKE '%{searchQuery}%'";
                        break;
                    case "Year":
                    case "Price":
                        // Parse searchQuery as a number and apply exact match
                        if (int.TryParse(searchQuery, out int numericValue))
                        {
                            filterExpression = $"{selectedCategory} = {numericValue}";
                        }
                        else
                        {
                            MessageBox.Show($"Invalid input for {selectedCategory}. Please enter a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        break;
                    default:
                        MessageBox.Show("Invalid search category.", "Category not recognized", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }

                carPartsDataTable.DefaultView.RowFilter = filterExpression;
                carPartsDataTable = carPartsDataTable.DefaultView.ToTable();
            }

            foreach (DataRow row in carPartsDataTable.Rows)
            {
                PartsStoreItem shopItem = new PartsStoreItem(row);
                flowLayoutPanelCars.Controls.Add(shopItem);
            }
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Font = new Font("Poppins", 11, FontStyle.Regular),
            };
        }
        private void ShowCarPartsDetailsForm(DataRow carPartsData)
        {
            // Create a new form for displaying detailed information
            CarPartsDetailsForm detailsForm = new CarPartsDetailsForm(carPartsData);

            // Show the form
            detailsForm.ShowDialog();
        }


        private Image ImageFromByteArray(byte[] byteArray)
        {
            // Convert byte array to Image
            using (var ms = new System.IO.MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            string selectedCategory = comboSearchCategory.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedCategory))
            {
                MessageBox.Show("Please select a search category.", "Category not selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadCarPartsStore(searchQuery, selectedCategory); // Reload the car parts store with the search query and selected category
        }

    }
}
