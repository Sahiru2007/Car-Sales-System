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
using Stripe;
namespace Car_trading_system.Forms
{
    public partial class PaymentGateway : Form
    {
        public PaymentGateway()
        {
            InitializeComponent();
       
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void btnPay_Click(object sender, EventArgs e)
        {
            var cardNumber = txtCardNumber.Text;
            var cvc = cvctxt.Text;
            var expiryYear = exYear.Text;
            var expiryMonth = exMonth.Text;

            try
                {
                    StripeConfiguration.ApiKey = "sk_test_51OaK8GIHxKiWcbbgdRni8thFOC87zrMpUvVWI3jO7NzRwZxTkXH8DoetlTkFtCjcLOV7kVPedhaVTCKeYpPHJDsn009abD70oQ";

                    var tokenOptions = new TokenCreateOptions
                    {
                        Card = new TokenCardOptions
                        {
                            Number = cardNumber,
                            ExpMonth = expiryMonth,
                            ExpYear = expiryYear,
                            Cvc = cvc
                        }
                    };

                    var tokenService = new TokenService();
                    Token stripeToken = await tokenService.CreateAsync(tokenOptions);

                    var options = new ChargeCreateOptions
                    {
                        Amount = 5000, // This is in the smallest unit of the currency (e.g., cents for USD)
                        Currency = "lkr",
                        Description = "Example charge",
                        Source = stripeToken.Id
                    };

                    var service = new ChargeService();
                    Charge charge = await service.CreateAsync(options);

                    MessageBox.Show($"Payment successful: {charge.Id}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Payment failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
        }
    }
}
