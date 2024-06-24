using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms.BunifuButton;

namespace Car_trading_system.Forms
{
    public partial class AdminHomeForm : Form
    {
        private BunifuButton2 currentActiveButton;
        public AdminHomeForm()
        {
            InitializeComponent();
            this.Size = new Size(1718, 697);
            this.WindowState = FormWindowState.Maximized; // Open the form in full screen
            loadform(new AdminDashboard());
            SetActiveButton(btnDashboard);

        }
        
        public void loadform(object Form)
        {
            if (this.pnlMain.Controls.Count > 0)
                this.pnlMain.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.pnlMain.Controls.Add(f);
            this.pnlMain.Tag = f;
            f.Show();
        }
        private void SetActiveButton(BunifuButton2 button)
        {
            if (currentActiveButton != null)
            {
                // Reset the background color of the previously active button
                currentActiveButton.IdleFillColor = Color.FromArgb(35, 20, 29);
            }

            // Highlight the current button
            button.IdleFillColor = Color.FromArgb(250, 171, 111);
            currentActiveButton = button;
        }


        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void btnCars_Click(object sender, EventArgs e)
        {
            loadform(new ManageCarForm());
            SetActiveButton((BunifuButton2)sender);  
            btnDashboard.IdleFillColor = Color.FromArgb(35, 20, 29);
        }

        private void btnParts_Click(object sender, EventArgs e)
        {
            loadform(new ManageCarPartForm());
            SetActiveButton((BunifuButton2)sender);  
            btnDashboard.IdleFillColor = Color.FromArgb(35, 20, 29);
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            loadform(new ManageCustomerForm());
            SetActiveButton((BunifuButton2)sender);  
            btnDashboard.IdleFillColor = Color.FromArgb(35, 20, 29);
        }

        private void AdminHomeForm_Load(object sender, EventArgs e)
        {

        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            loadform(new AdminOrder());
            SetActiveButton((BunifuButton2)sender);  
            btnDashboard.IdleFillColor = Color.FromArgb(35, 20, 29);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            loadform(new AdminDashboard());
            SetActiveButton((BunifuButton2)sender);  
        }

        bool sideBarExpand = true;
        private void sideBarTransition_Tick(object sender, EventArgs e)
        {
            if (sideBarExpand)
            {
                sidePanel.Width -= 10;
                if(sidePanel.Width <= 107)
                {
                    sideBarExpand = false;
                    SidebarTransition.Stop();
                    // Collapse all buttons
                    btnDashboard.Width = 45;
                    btnDashboard.Text = null;

                    btnCars.Width = 45;
                    btnCars.Text = null;

                    btnCarParts.Width = 45;
                    btnCarParts.Text = null;

                    btnCustomers.Width = 45;
                    btnCustomers.Text = null;

                    btnOrders.Width = 45;
                    btnOrders.Text = null;

                   
                }
            }
            else
            {
                sidePanel.Width += 10;
                if (sidePanel.Width >= 277)
                {
                    sideBarExpand = true;
                    SidebarTransition.Stop();
                    // Expand all buttons
                    btnDashboard.Width = 238;
                    btnDashboard.Text = "Dashboard";

                    btnCars.Width = 238;
                    btnCars.Text = "Cars";

                    btnCarParts.Width = 238;
                    btnCarParts.Text = "Car Parts";

                    btnCustomers.Width = 238;
                    btnCustomers.Text = "Customers";

                    btnOrders.Width = 238;
                    btnOrders.Text = "Orders";

                   
                }
            }
        }

        private void menubtn_Click(object sender, EventArgs e)
        {
            SidebarTransition.Start();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            loadform(new CarsReport());
            SetActiveButton((BunifuButton2)sender);  
        }

        private void bunifuButton21_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                HomePage homeForm = new HomePage();
                this.Hide();
                homeForm.Show();
            }
        }
    }
}
