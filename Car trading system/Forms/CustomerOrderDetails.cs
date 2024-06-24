using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Car_trading_system.Data_Handlers;
using Car_trading_system.Functions;


namespace Car_trading_system.Forms
{
    public partial class CustomerOrderDetails : Form
    {
        private OrderDataHandler orderDataHandler;
        private DataTable ordersDataTable;
        

        public CustomerOrderDetails()
        {
            InitializeComponent();
            orderDataHandler = new OrderDataHandler(new DatabaseHandler());
            ordersDataTable = new DataTable();

           
            LoadOrderData();
        }

       

        private void LoadOrderData()
        {
            string CustomerID = LoggedInUser.Instance.CustomerID;
            try
            {
                ordersDataTable = orderDataHandler.GetCustomerOrderData(CustomerID);

                foreach (DataRow row in ordersDataTable.Rows)
                {
                   CustomControls.OrderDetails orderDetails = new CustomControls.OrderDetails();
                    orderDetails.LoadOrderData(row);

                    // Assuming you have a FlowLayoutPanel or some other container in your form
                    this.flowLayoutPanelOrders.Controls.Add(orderDetails);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private Label CreateLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 10);
            label.ForeColor = Color.FromArgb(100, 100, 100);
            label.Padding = new Padding(5);
            label.Margin = new Padding(0, 5, 0, 5);
            label.Location = new Point(x, y);

            return label;
        }
        private Label CreateProductLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.AutoSize = true;
            label.Font = new Font("Poppins", 12, FontStyle.Bold);
            label.ForeColor = Color.Black;
            label.Padding = new Padding(5);
            label.Margin = new Padding(0, 5, 0, 5);
            label.Location = new Point(x, y);

            return label;
        }

         
        
    }
}
