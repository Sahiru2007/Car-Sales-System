using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_trading_system
{
    public partial class AdminLoginForm : Form
    {
        // Hard-coded username and password
        private const string CorrectUsername = "admin";
        private const string CorrectPassword = "12345678";

        public AdminLoginForm()
        {
            
            InitializeComponent();
           
            

        }
        private bool IsValidLogin(string enteredUsername, string enteredPassword)
        {
            // Compare entered credentials with hard-coded values
            return (enteredUsername == CorrectUsername) && (enteredPassword == CorrectPassword);
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string enteredUsername = txtUsername.Text;
            string enteredPassword = txtPassword.Text;

            if (IsValidLogin(enteredUsername, enteredPassword))
            {
                

                // initialize the AdminHomeForm
               Forms.AdminHomeForm adminHomeForm = new Forms.AdminHomeForm();
                // Close the login form
                this.Hide();

                adminHomeForm.Show();
               
                
                
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear textboxes and reset checkbox
            txtUsername.Clear();
            txtPassword.Clear();
            checkboxShowPass.Checked = false;
        }

        private void checkboxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle password visibility based on checkbox state
            txtPassword.UseSystemPasswordChar = !checkboxShowPass.Checked;
        
        }

        private void AdminLoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
