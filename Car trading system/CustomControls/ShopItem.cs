using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Car_trading_system.Data_Handlers;
using Car_trading_system.Forms;
using Car_trading_system.Functions;
using Stripe;
namespace Car_trading_system.CustomControls
{
    public partial class ShopItem : Bunifu.UI.WinForms.BunifuUserControl
    {
        private readonly DatabaseHandler dbHandler;
        private readonly OrderDataHandler orderDataHandler;

        // Assume stripeClient is initialized in a suitable place within your project
        private StripeClient stripeClient = new StripeClient("sk_test_51OaK8GIHxKiWcbbgdRni8thFOC87zrMpUvVWI3jO7NzRwZxTkXH8DoetlTkFtCjcLOV7kVPedhaVTCKeYpPHJDsn009abD70oQ");
        private DataRow carData;
        private int quantity = 1;

        public ShopItem(DataRow data)
        {
            InitializeComponent();
            carData = data;
            dbHandler = new DatabaseHandler();
            orderDataHandler = new OrderDataHandler(dbHandler);
            PopulateData();
        }
        private string FormatPrice(decimal price)
        {
            if (price >= 1000000)
                return $"{Math.Round(price / 1000000)}M";
            else if (price >= 1000)
                return $"{Math.Round(price / 1000)}k";
            else
                return $"{price}";
        }


        private void PopulateData()
        {
            // Populate labels and picture box with carData
            ProductName.Text = $"{carData["Brand"]} {carData["Model"]}";
            PriceLabel.Text = $"Rs. {FormatPrice(Convert.ToDecimal(carData["Price"]))}";
            Year.Text = $"{carData["Year"]} ";

            StockCount.Text = Convert.ToInt32(carData["StockCount"]) <= 0 ? "Out of Stock" : $"{carData["StockCount"]} left";
            ProductImage.Image = carData["ImageData"] is DBNull ? Properties.Resources.DefaultCarImage : ImageFromByteArray((byte[])carData["ImageData"]);
            quantitylbl.Text = quantity.ToString();

            minusBtn.Click += (s, e) => UpdateQuantity(-1);
            Plusbtn.Click += (s, e) => UpdateQuantity(1);
           
           
        }

        private void UpdateQuantity(int change)
        {
            int stockCount = Convert.ToInt32(carData["StockCount"]);
            quantity += change;
            if (quantity < 1) quantity = 1;
            else if (quantity > stockCount) quantity = stockCount;
            quantitylbl.Text = quantity.ToString();
        }

     
        private Image ImageFromByteArray(byte[] byteArray)
        {
            using (var ms = new System.IO.MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            ShowCarDetailsForm(carData);
        }
        private void ShowCarDetailsForm(DataRow carData)
        {
            // Create a new form for displaying detailed information
            CarDetailsForm detailsForm = new CarDetailsForm(carData);

            // Show the form
            detailsForm.ShowDialog();
        }

        private void purchaseBtn_Click(object sender, EventArgs e)
        {
            // Ensure that quantity is valid
            int selectedQuantity = Convert.ToInt32(quantitylbl.Text);
            int stockCount = Convert.ToInt32(carData["StockCount"]);
            if (selectedQuantity <= 0 || selectedQuantity > stockCount)
            {
                MessageBox.Show("Invalid quantity.", "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Assuming carData has columns for price, brand, and model, and that LoggedInUser contains the CustomerID
            decimal price = Convert.ToDecimal(carData["Price"]);
            string brand = Convert.ToString(carData["Brand"]);
            string model = Convert.ToString(carData["Model"]);
            string name = brand + model;
            string customerID = LoggedInUser.Instance.CustomerID; // or the appropriate way you have to get the customer ID
            string carID = Convert.ToString(carData["CarID"]); // or the appropriate identifier for the car

            // Open the OrderConfirmation form with the required data
            OrderConfirmation confirmationForm = new OrderConfirmation(carData, selectedQuantity, price, customerID, carID, "Car");
            confirmationForm.ShowDialog(); // Open as modal dialog
        }


        private void ShopItem_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click_1(object sender, EventArgs e)
        {
              // initialize the AdminHomeForm
                CarDetailsForm carDetailsForm = new CarDetailsForm(carData);

            carDetailsForm.Show();
        }
    }
}
