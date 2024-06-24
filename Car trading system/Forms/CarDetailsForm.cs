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
    public partial class CarDetailsForm : Form
    {
        public CarDetailsForm(DataRow carData)
        {
            InitializeComponent();
            // Use the carData DataRow to populate the detailed information
            CarID.Text = $"CarID: {carData["CarID"]}";
            Brand.Text = $"Brand: {carData["Brand"]}";
            Year.Text = $"Year: {carData["Year"]}";
            Color.Text = $"Color: {carData["Color"]}";
            BodyType.Text = $"BodyType: {carData["BodyType"]}";
            TransmissionType.Text = $"TransmissionType: {carData["TransmissionType"]}";
            FuelType.Text = $"FuelType: {carData["FuelType"]}";
            EngineCapacity.Text = $"EngineCapacity: {carData["EngineCapacity"]}";
            Mileage.Text = $"Mileage: {carData["Mileage"]}";
            Condition.Text = $"Condition: {carData["Condition"]}";
            Price.Text = $"Price: {carData["Price"]}";
            StockCount.Text = $"StockCount: {carData["StockCount"]}";

            // Set PictureBoxCar's image
            if (!(carData["ImageData"] is DBNull))
            {
                PictureBoxCar.Image = ImageFromByteArray((byte[])carData["ImageData"]);
            }
            else
            {
                
                PictureBoxCar.Image = Properties.Resources.DefaultCarImage; // Set a default image
            }
        }
        // Helper method to convert byte array to Image
        private Image ImageFromByteArray(byte[] byteArray)
        {
            using (var ms = new System.IO.MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
