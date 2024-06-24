using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_trading_system
{
    public partial class CustomerRegistrationForm : Form
    {
        private readonly DatabaseHandler dbHandler;
        private readonly CustomerDataHandler customerHandler;
        public CustomerRegistrationForm()
        {
            InitializeComponent();
            // Initialize the DatabaseHandler 
            dbHandler = new DatabaseHandler();

            // Initialize the CustomerDataHandler with the DatabaseHandler
            customerHandler = new CustomerDataHandler(dbHandler);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate form fields
                if (ValidateFields())
                {
                    // Validate password and confirm password
                    if (txtPassword.Text != txtConfirmPassword.Text)
                    {
                        MessageBox.Show("Password and Confirm Password do not match. Please enter matching passwords.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Check if the username is unique
                    string username = txtUsername.Text;
                    if (!customerHandler.IsUsernameUnique(username))
                    {
                        MessageBox.Show("Username is not unique. Please select another username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Collect data from the form
                    string fullName = txtFullName.Text;
                    string address = txtAddress.Text;
                    string contactNumber = txtContactNumber.Text;
                    string emailAddress = txtEmailAddress.Text;
                    string nic = txtNIC.Text;
                    DateTime dateOfBirth = dtpDateOfBirth.Value;
                    string gender = radFemale.Checked ? "Male" : "Female"; 
                    string password = txtPassword.Text;
                    byte[] imageData = ImageToByteArray(pictureBoxProfile.Image);

                    // Insert data into the database using the CustomerDataHandler
                    if (customerHandler.InsertCustomerData(fullName, address, contactNumber, emailAddress,nic, dateOfBirth, gender,username, password,imageData))
                    {
                        MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

  
                        ClearFields();
                    }
                    else
                    {

                        MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg); // Change the format based on your requirements
                return ms.ToArray();
            }
        }
        // Method to validate form fields
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtContactNumber.Text) ||
                string.IsNullOrWhiteSpace(txtEmailAddress.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show("All fields are required. Please fill out all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate email format
            if (!IsValidEmail(txtEmailAddress.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Check if the image is available
            if (pictureBoxProfile.Image == null)
            {
                MessageBox.Show("Please select an image for the car.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }



            return true;
        }
        // Method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Method to clear form fields
        private void ClearFields()
        {
            // Clear the form fields
            txtFullName.Clear();
            txtAddress.Clear();
            txtContactNumber.Clear();
            txtEmailAddress.Clear();
            dtpDateOfBirth.Value = DateTime.Now;
            radMale.Checked = false;
            radFemale.Checked = false;
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear the form fields
            ClearFields();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Hide the current registration form
            this.Hide();

            // Show the CustomerLoginForm
            var loginForm = new CustomerLoginForm();
            loginForm.FormClosed += (s, args) => this.Show(); // Show registration form when login form is closed
            loginForm.Show();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog to allow the user to select an image
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Get the selected file path
                        string imagePath = openFileDialog.FileName;

                        // Load the image into the PictureBox
                        pictureBoxProfile.Image = Image.FromFile(imagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
