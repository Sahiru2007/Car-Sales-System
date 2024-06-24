using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Car_trading_system.Data_Handlers;
using System.Xml;


namespace Car_trading_system.CustomControls
{
    public partial class OrderDetails : Bunifu.UI.WinForms.BunifuUserControl
    {
        private OrderDataHandler orderDataHandler;
        private DataTable ordersDataTable;



        public OrderDetails()
        {
            InitializeComponent();
            orderDataHandler = new OrderDataHandler(new DatabaseHandler());
            ordersDataTable = new DataTable();


        }
        public void LoadOrderData(DataRow dataRow)
        {
            if (dataRow == null) return;

            byte[] imageData = orderDataHandler.GetProductImage(dataRow["ProductID"].ToString());
            if (imageData != null && imageData.Length > 0)
            {
                using (var ms = new MemoryStream(imageData))
                {
                    this.ProductImage.Image = Image.FromStream(ms);
                }
            }

            // Remove every single space in productstatus
            string productstatus = dataRow["Status"].ToString().Replace(" ", "");

            UpdateStatusUI(productstatus);
            decimal totalAmount = Convert.ToDecimal(dataRow["Amount"]);
            int quantity = Convert.ToInt32(dataRow["Quantity"]);
            decimal individualPrice = totalAmount / quantity;

            string productId = dataRow["ProductID"].ToString();
            string productDetails = orderDataHandler.GetProductDetails(productId);

            this.status.Text = productstatus; // Here you show the modified status without spaces
            this.labelOrderID.Text = $"# {dataRow["OrderID"]}";
            this.labelProductDetails.Text = productDetails;
            this.labelAmount.Text = $" Rs. {individualPrice} x {quantity}";
            this.labelTotalAmount.Text = $"Rs. {totalAmount}";
            this.labelOrderDate.Text = $"Order Date: {dataRow["OrderDate"]}";
        }

        private void UpdateStatusUI(string productstatus)
        {
            // The productstatus parameter here is already modified to have no spaces
            // Your if-else logic remains the same since it now operates on the modified status
            if (productstatus == "Processing")
            {
                processingBtn.ImageIndex = 0;
                line2.BackColor = Color.FromArgb(102, 207, 116); // #66CF74
            }
            else if (productstatus == "Shipped")
            {
                processingBtn.ImageIndex = 0;
                line2.BackColor = Color.FromArgb(102, 207, 116); // #66CF74
                shippedBtn.ImageIndex = 6;
                line3.BackColor = Color.FromArgb(102, 207, 116); // #66CF74
            }
            else if (productstatus == "Delivered")
            {
                processingBtn.ImageIndex = 0;
                line2.BackColor = Color.FromArgb(102, 207, 116); // #66CF74
                shippedBtn.ImageIndex = 6;
                line3.BackColor = Color.FromArgb(102, 207, 116); // #66CF74
                deliveredBtn.ImageIndex = 6;
                status.ForeColor = Color.FromArgb(102, 207, 116);
            }
            else if (productstatus == "Canceled")
            {
                line1.BackColor = Color.Red;
                line2.BackColor = Color.Red;
                line3.BackColor = Color.Red;
                status.ForeColor = Color.Red;
            }
        }

        private void OrderDetails_Load(object sender, EventArgs e)
        {

        }

        private void OrderDetails_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
