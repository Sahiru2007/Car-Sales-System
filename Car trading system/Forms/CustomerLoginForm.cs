using Car_trading_system.Authentication;
using Car_trading_system.Forms;
using Car_trading_system.Functions;
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
    public partial class CustomerLoginForm : Form
    {
        private readonly DatabaseHandler dbHandler;
        private readonly CustomerAuthenticator customerAuthenticator;
        private readonly CustomerDataHandler customerDataHandler;
        public CustomerLoginForm()
        {
            InitializeComponent();
            try
            {
                dbHandler = new DatabaseHandler();
                customerAuthenticator = new CustomerAuthenticator(dbHandler);
                // Initialize the customerDataHandler
                customerDataHandler = new CustomerDataHandler(new DatabaseHandler());
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred during initialization: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkboxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkboxShowPass.Checked) { 
          
            txtPassword.UseSystemPasswordChar = true;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = false ;
            }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate form fields
                if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    // Display an error message if required fields are empty
                    MessageBox.Show("Username and Password are required. Please enter both.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Authenticate customer
                if (customerAuthenticator.AuthenticateCustomer(txtUsername.Text, txtPassword.Text))
                {
                    // Successful login
                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // If login is successful, set properties of LoggedInUser
                    LoggedInUser.Instance.Username = txtUsername.Text;
                    LoggedInUser.Instance.ImageData = customerDataHandler.GetImageDataForUser(txtUsername.Text);
                    LoggedInUser.Instance.CustomerID = customerDataHandler.GetCustomerIDForUser(txtUsername.Text);

                    // Close the login form and open the home form
                    
                    CustomerHomeForm homeForm = new CustomerHomeForm();
                    this.Hide();
                    homeForm.Show();
                    homeForm.FormClosed += (s, args) => this.Close();
                }
                else
                {
                    // Login failed
                    MessageBox.Show("Invalid username or password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void ClearFields()
        {
            // Clear the form fields
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear the form fields
            ClearFields();
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            // Hide the current login form
            this.Hide();

            // Show the CustomerRegistrationForm
            var registrationForm = new CustomerRegistrationForm();
            registrationForm.FormClosed += (s, args) => this.Show(); 
            registrationForm.Show();
        }

        private void checkboxShowPass_CheckStateChanged(object sender, EventArgs e)
        {

        }
    }
}
