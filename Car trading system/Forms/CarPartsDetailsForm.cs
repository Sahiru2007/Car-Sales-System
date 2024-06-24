using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Forms.VisualStyles;

namespace Car_trading_system.Forms
{
    public partial class CarPartsDetailsForm : Form
    {
        public CarPartsDetailsForm(DataRow carPartsData)
        {
            InitializeComponent();
            // Use the carPartsData DataRow to populate the detailed information
            PartID.Text = $"PartID: {carPartsData["PartID"]}";
            PartName.Text = $"PartName: {carPartsData["PartName"]}";
            Manufacturer.Text = $"Manufacturer: {carPartsData["Manufacturer"]}";
            Year.Text = $"Year: {carPartsData["Year"]}";
            Model.Text = $"Model: {carPartsData["Model"]}";
            Price.Text = $"Price: {carPartsData["Price"]}";
            Category.Text = $"Category: {carPartsData["Category"]}";
            StockCount.Text = $"StockCount: {carPartsData["StockCount"]}";

            // Set PictureBoxCar's image
            if (!(carPartsData["ImageData"] is DBNull))
            {
                PictureBoxCar.Image = ImageFromByteArray((byte[])carPartsData["ImageData"]);
            }
            else
            {
                // Handle the case where ImageData is DBNull (e.g., set a default image)
                PictureBoxCar.Image = Properties.Resources.DefaultCarImage; // Set a default image
            }
        }
        private Image ImageFromByteArray(byte[] byteArray)
        {
            using (var ms = new System.IO.MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
