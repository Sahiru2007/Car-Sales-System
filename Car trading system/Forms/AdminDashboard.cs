using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Car_trading_system.Data_Handlers;
using Bunifu.Charts.WinForms;
using Bunifu.Charts.WinForms.ChartTypes;


namespace Car_trading_system.Forms
{
    public partial class AdminDashboard : Form
    {
      
        private AdminDashboardDataHandler dashboardDataHandler;
        public AdminDashboard()
        {
            InitializeComponent();
            // Initialize the  DatabaseHandler
            dashboardDataHandler = new AdminDashboardDataHandler(new DatabaseHandler());

            // Load the dashboard data
            LoadDashboardData();
            LoadIncomeDataIntoChart();
            LoadIncomeDataIntoLineChart();
            LoadTopSellingProducts();
        }
        private void LoadDashboardData()
        {
            // Retrieve and display the number of customers
            NoCustomer.Text = dashboardDataHandler.GetNumberOfCustomers().ToString();

            // Retrieve and display the number of cars
            NoCars.Text = dashboardDataHandler.GetNumberOfCars().ToString();

            // Retrieve and display the number of car parts
            NoCarParts.Text = dashboardDataHandler.GetNumberOfCarParts().ToString();

            // Retrieve and display the total income for the current year
            decimal totalIncome =  dashboardDataHandler.GetTotalIncomeForCurrentYear();
            TotalIncome.Text = "RS. " + FormatIncome(totalIncome);
        }
        // Method to format income based on the given criteria
        
        private string FormatIncome(decimal income)
        {
            if (income >= 1000000) // If income is 1 million or more
            {
                return (income / 1000000).ToString("0.00") + "M";
            }
            else if (income >= 10000) // If income is 10,000 or more
            {
                return (income / 1000).ToString("0.00") + "k"; 
            }
            else // For income less than 10,000
            {
                return income.ToString("0.00"); 
            }
        }

        private void LoadTopSellingProducts()
        {
            // Retrieve top selling products
            var topSellingProducts = dashboardDataHandler.GetTopSellingProducts();

            // Display details of each top selling product in labels
            if (topSellingProducts.Count >= 1)
            {
                lblProductID1.Text = topSellingProducts[0].ProductID;
                lblProductName1.Text = topSellingProducts[0].ProductName;
                lblPricePerUnit1.Text = "Rs. " + FormatIncome(topSellingProducts[0].PricePerUnit);
                lblQuantitySold1.Text = topSellingProducts[0].QuantitySold.ToString();
            }

            if (topSellingProducts.Count >= 2)
            {
                lblProductID2.Text = topSellingProducts[1].ProductID;
                lblProductName2.Text = topSellingProducts[1].ProductName;
                lblPricePerUnit2.Text = "Rs. " + FormatIncome(topSellingProducts[1].PricePerUnit);
                lblQuantitySold2.Text = topSellingProducts[1].QuantitySold.ToString();
            }

            if (topSellingProducts.Count >= 3)
            {
                lblProductID3.Text = topSellingProducts[2].ProductID;
                lblProductName3.Text = topSellingProducts[2].ProductName;
                lblPricePerUnit3.Text = "Rs. " + FormatIncome(topSellingProducts[2].PricePerUnit);
                lblQuantitySold3.Text = topSellingProducts[2].QuantitySold.ToString();
            }
        }



        private void LoadIncomeDataIntoChart()
        {
            // Clear existing data points
            bunifuBarChart1.Data.Clear();

            var monthlyIncome = dashboardDataHandler.GetMonthlyIncome();

            // Lists to hold the data points and colors for each month
            List<double> dataPoints = new List<double>();
            List<Color> barColors = new List<Color>();

            
            int currentYear = DateTime.Now.Year;

            // Iterate through all months of the year, ensuring each month is represented
            for (int month = 1; month <= 12; month++) 
            {
                string monthYearKey = new DateTime(currentYear, month, 1).ToString("MMM yyyy");
                decimal income = monthlyIncome.ContainsKey(monthYearKey) ? monthlyIncome[monthYearKey] : 0;
                dataPoints.Add((double)income);

               
                if (income > 0)
                {
                    barColors.Add(Color.FromArgb(254, 138, 180)); 
                }
                else
                {
                    barColors.Add(Color.FromArgb(220, 220, 220)); 
                }
            }

            // Set the data points to the chart
            bunifuBarChart1.Data = dataPoints;
            bunifuBarChart1.Label = "Rs";

          
           
        }

        private void LoadIncomeDataIntoLineChart()
        {
            // Clear existing data points
            bunifuLineChart1.Data.Clear();

            var monthlyCarsIncome = dashboardDataHandler.GetMonthlyIncomeByCategory().CarsIncome;
            var monthlyCarsPartsIncome = dashboardDataHandler.GetMonthlyIncomeByCategory().CarPartsIncome;

            // Lists to hold the data points and colors for each month
            List<double> carsdataPoints = new List<double>();
            List<double> carPartsdataPoints = new List<double>();
            List<Color> barColors = new List<Color>();

      
            int currentYear = DateTime.Now.Year;

            // Iterate through all months of the yea
            for (int month = 1; month <= 12; month++) 
            {
                string monthYearKey = new DateTime(currentYear, month, 1).ToString("MMM yyyy");
                decimal carsincome = monthlyCarsIncome.ContainsKey(monthYearKey) ? monthlyCarsIncome[monthYearKey] : 0;
                decimal carPartsincome = monthlyCarsPartsIncome.ContainsKey(monthYearKey) ? monthlyCarsPartsIncome[monthYearKey] : 0;
                carsdataPoints.Add((double)carsincome);
                carPartsdataPoints.Add((double)carPartsincome);


                if (carPartsincome > 0 && carsincome > 0)
                {
                    barColors.Add(Color.FromArgb(254, 138, 180));
                }
                else
                {
                    barColors.Add(Color.FromArgb(220, 220, 220));
                }
            }

            // Set the data points to the chart
            bunifuLineChart1.Data = carsdataPoints;
            bunifuLineChart1.Label = "Rs";

            bunifuLineChart2.Data = carPartsdataPoints;
            bunifuLineChart2.Label = "Rs";



        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuChartCanvas2_Load(object sender, EventArgs e)
        {

        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {

    
        }
    }
}
