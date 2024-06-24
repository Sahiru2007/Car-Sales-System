using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
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
    public partial class OrderConfirmation : Form
    {
        private readonly DatabaseHandler dbHandler;
        private readonly Data_Handlers.OrderDataHandler orderDataHandler;
        private DataRow carData;
        private DataRow carPartData;
        private int quantity;
        private decimal price;
        private string customerId;
        private string productId;
        private string type;


        public OrderConfirmation(DataRow carData, int quantity, decimal price, string customerId, string productId, string type)
        {
            dbHandler = new DatabaseHandler();
            orderDataHandler = new Data_Handlers.OrderDataHandler(dbHandler);
           
            InitializeComponent();
            this.carData = carData;
           
            this.quantity = quantity;
            this.price = price;
            this.customerId = customerId;
            this.productId = productId;
            this.type = type;
        }

        private void OrderConfirmation_Load(object sender, EventArgs e)
        {

            
        }

        private void txtBrand_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtYear_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                // Simulate a payment process
                ProcessPayment();

                // If fake payment succeeds, proceed to confirm order
                ConfirmOrder();
            }
        }
        private bool ValidateFields()
        {
            // Validate credit card number 
            if (txtCredit.Text.Length != 16)
            {
                MessageBox.Show("Invalid Credit Card Number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate CVC 
            if (txtCVC.Text.Length != 3 && txtCVC.Text.Length != 4)
            {
                MessageBox.Show("Invalid CVC", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Expiry Date 
            if (!DateTime.TryParseExact(txtExpire.Text, "MM/yy", null, System.Globalization.DateTimeStyles.None, out var expiryDate) || expiryDate < DateTime.UtcNow)
            {
                MessageBox.Show("Invalid Expiry Date", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void ProcessPayment()
        {
            // Simulate payment processing
            System.Threading.Thread.Sleep(2000); // Simulate some delay for payment processing

        }

        private void ConfirmOrder()
        {
            if (type == "Car")
            {
              
                MessageBox.Show($"Order confirmed for {quantity} units of {carData["Brand"]} {carData["Model"]}.", "Order Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Order confirmed for {quantity} units of {carData["partName"]} ", "Order Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


           
            int newOrderId = orderDataHandler.InsertOrderData(
         customerId: customerId,
         productId: productId,
         quantity: quantity,
         amount: price * quantity,
         orderDate: DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
         status: "Confirmed",
         paymentStatus: "Paid",
         type: type
     );



        }
    }

}

