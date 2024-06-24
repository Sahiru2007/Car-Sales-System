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
using Car_trading_system.CustomControls;

namespace Car_trading_system.Forms
{
    public partial class CarStoreForm : Form
    {
        private readonly DatabaseHandler dbHandler;
        private readonly OrderDataHandler orderDataHandler;
        
        private CarDataHandler carDataHandler;
       
        public CarStoreForm()
        {
            InitializeComponent();
            dbHandler = new DatabaseHandler();
            orderDataHandler = new OrderDataHandler(dbHandler);
            carDataHandler = new CarDataHandler(new DatabaseHandler());
           

            LoadCarStore();
        }
        private void LoadCarStore(string searchQuery = "", string selectedCategory = "")
        {
            flowLayoutPanelCars.Controls.Clear(); // Clear existing controls
            flowLayoutPanelCars.WrapContents = true;

            DataTable carDataTable = carDataHandler.GetCarData();

            // Apply filter if searchQuery is not empty
            if (!string.IsNullOrEmpty(searchQuery) && !string.IsNullOrEmpty(selectedCategory))
            {
                // Build filter string based on the selected category
                string filterExpression = "";
                switch (selectedCategory)
                {
                    case "CarID":
                    case "Brand":
                    case "Year":
                    case "Color":
                    case "BodyType":
                    case "TransmissionType":
                    case "FuelType":
                    case "Condition":
                        filterExpression = $"{selectedCategory} LIKE '%{searchQuery}%'";
                        break;
                    case "EngineCapacity":
                    case "Mileage":
                    case "Price":
                        // Parse searchQuery as a number and apply exact match
                        if (double.TryParse(searchQuery, out double numericValue))
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

                carDataTable.DefaultView.RowFilter = filterExpression;
                carDataTable = carDataTable.DefaultView.ToTable();
            }
            foreach (DataRow row in carDataTable.Rows)
            {
                ShopItem shopItem = new ShopItem(row);
                flowLayoutPanelCars.Controls.Add(shopItem);
            }
        }

           
        private void flowLayoutPanelCars_Paint(object sender, PaintEventArgs e)
        {

        }


        private void btnAddCar_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            string selectedCategory = comboSearchCategory.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                LoadCarStore(searchQuery, selectedCategory); // Reload the car store with the search query and selected category
            }
            else
            {
                MessageBox.Show("Please select a search category.", "Category not selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
