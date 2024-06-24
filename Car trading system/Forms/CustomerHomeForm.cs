using Car_trading_system.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms.BunifuButton;

namespace Car_trading_system.Forms
{
    public partial class CustomerHomeForm : Form
    {
        private BunifuButton2 currentActiveButton;
        public CustomerHomeForm()
        {
            InitializeComponent();
            this.Size = new Size(1718, 697);
            this.WindowState = FormWindowState.Maximized; // Open the form in full screen
            SetActiveButton(btnStore);
            loadform(new CarStoreForm());
            btnCars.ForeColor = Color.FromArgb(250, 171, 111);
        }
        private void SetActiveButton(BunifuButton2 button)
        {
            if (currentActiveButton != null)
            {
                // Reset the background color of the previously active button
                currentActiveButton.IdleFillColor = Color.FromArgb(35, 20, 29); // Adjust as per your default or inactive background color
            }

            // Highlight the current button
            button.IdleFillColor = Color.FromArgb(250, 171, 111);
            currentActiveButton = button;
        }
        private void CustomerHomeForm_Load(object sender, EventArgs e)
        {
            // Set the profile picture
            if (LoggedInUser.Instance.ImageData != null)
            {
                pictureBoxProfile.Image = ByteArrayToImage(LoggedInUser.Instance.ImageData);
            }

            // Set the username label
            lblUsername.Text = LoggedInUser.Instance.Username;

        }
        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
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

        private void pnlSide_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCars_Click(object sender, EventArgs e)
        {
            loadform(new CarStoreForm());
            SetActiveButton(btnStore);  
            btnStore.IdleFillColor = Color.FromArgb(35, 20, 29);
            btnCars.ForeColor = Color.DarkGray;
            btnCarParts.ForeColor = Color.DarkGray;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadform(new CarPartsForm());
            SetActiveButton(btnStore);  
            btnStore.IdleFillColor = Color.FromArgb(35, 20, 29);
            btnCars.ForeColor = Color.DarkGray;
            btnCarParts.ForeColor = Color.DarkGray;
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            loadform(new CustomerOrderDetails());
            SetActiveButton((BunifuButton2)sender);  
            btnStore.IdleFillColor = Color.FromArgb(35, 20, 29);
            btnCars.ForeColor = Color.DarkGray;
            btnCarParts.ForeColor = Color.DarkGray;
        }

        bool sideBarExpand = true;

        private void sideBarTransition_Tick(object sender, EventArgs e)
        {
            if (sideBarExpand)
            {
                sidePanel.Width -= 10;
                if (sidePanel.Width <= 107)
                {
                    sideBarExpand = false;
                    SidebarTransition.Stop();
                    // Collapse all buttons
                    btnStore.Width = 45;
                    btnStore.Text = null;

                   
                    btnCars.Text = null;

                   
                    btnCarParts.Text = null;

                    MyAccount.Width = 45;
                    MyAccount.Text = null;

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
                    btnStore.Width = 238;
                    btnStore.Text = "Store";

      
                    btnCars.Text = "Cars";

                   
                    btnCarParts.Text = "Car Parts";

                    btnOrders.Width = 238;
                    btnOrders.Text = "My Orders";

                  
                    MyAccount.Width = 238;
                    MyAccount.Text = "Reports";
                }
            }
        }


        
        private void menubtn_Click(object sender, EventArgs e)
        {
            SidebarTransition.Start();
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
