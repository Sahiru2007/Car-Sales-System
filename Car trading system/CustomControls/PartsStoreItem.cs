using Car_trading_system.Data_Handlers;
using Car_trading_system.Forms;
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
using TheArtOfDev.HtmlRenderer.Core;

namespace Car_trading_system.CustomControls
{
    public partial class PartsStoreItem : Bunifu.UI.WinForms.BunifuUserControl
    {
        private readonly DatabaseHandler dbHandler;
        private readonly OrderDataHandler orderDataHandler;
        private StripeClient stripeClient = new StripeClient("sk_test_51OaK8GIHxKiWcbbgdRni8thFOC87zrMpUvVWI3jO7NzRwZxTkXH8DoetlTkFtCjcLOV7kVPedhaVTCKeYpPHJDsn009abD70oQ");
        private DataRow carPartData;
        private int quantity = 1;
        public PartsStoreItem(DataRow data)
        {
            InitializeComponent();
            carPartData = data;
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
        private void PartsStoreItem_Load(object sender, EventArgs e)
        {

        }
        private void PopulateData()
        {
            // Populate labels and picture box with carPartData
            ProductName.Text = $"{carPartData["PartName"]} ";
            PriceLabel.Text = $"Rs. {FormatPrice(Convert.ToDecimal(carPartData["Price"]))}";
            Year.Text = $"{carPartData["Year"]} ";

            StockCount.Text = Convert.ToInt32(carPartData["StockCount"]) <= 0 ? "Out of Stock" : $"{carPartData["StockCount"]} left";
            ProductImage.Image = carPartData["ImageData"] is DBNull ? Properties.Resources.DefaultCarImage : ImageFromByteArray((byte[])carPartData["ImageData"]);
            quantitylbl.Text = quantity.ToString();

            minusBtn.Click += (s, e) => UpdateQuantity(-1);
            Plusbtn.Click += (s, e) => UpdateQuantity(1);
           

        }

        private void UpdateQuantity(int change)
        {
            int stockCount = Convert.ToInt32(carPartData["StockCount"]);
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

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            ShowCarDetailsForm(carPartData);
        }
        private void ShowCarDetailsForm(DataRow carPartData)
        {
            // Create a new form for displaying detailed information
            CarDetailsForm detailsForm = new CarDetailsForm(carPartData);

            // Show the form
            detailsForm.ShowDialog();
        }

        private void purchaseBtn_Click(object sender, EventArgs e)
        {
            // Ensure that quantity is valid
            int selectedQuantity = Convert.ToInt32(quantitylbl.Text);
            int stockCount = Convert.ToInt32(carPartData["StockCount"]);
            if (selectedQuantity <= 0 || selectedQuantity > stockCount)
            {
                MessageBox.Show("Invalid quantity.", "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            string customerID = LoggedInUser.Instance.CustomerID; 
            string carID = Convert.ToString(carPartData["PartID"]); 
            decimal price = Convert.ToDecimal(carPartData["Price"]);
            OrderConfirmation confirmationForm = new OrderConfirmation(carPartData, selectedQuantity, price, customerID, carID, "CarPart");
            confirmationForm.ShowDialog(); 
        }

        private void bunifuButton1_Click_1(object sender, EventArgs e)
        {
            CarPartsDetailsForm carDetailsForm = new CarPartsDetailsForm(carPartData);

            carDetailsForm.Show();
        }
    }
}

