using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Car_trading_system.Data_Handlers
{
    // Handles data retrieval for the admin dashboard metrics
    public class AdminDashboardDataHandler
    {
        private readonly DatabaseHandler dbHandler;

        // Constructor: Initializes the data handler with a database handler
        public AdminDashboardDataHandler(DatabaseHandler dbHandler)
        {
            this.dbHandler = dbHandler;
        }

        // Gets the total number of customers from the database
        public int GetNumberOfCustomers()
        {
            try
            {
                using (var connection = dbHandler.OpenConnection())
                {
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Customer", connection))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting number of customers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; // Return 0 on error
            }
        }

        //get income data for the chart
        public decimal GetTotalIncomeForCurrentYear()
        {
            decimal totalIncome = 0;
            int currentYear = DateTime.Now.Year;

            string query = $@"
SELECT 
    SUM(Amount) AS TotalIncome
FROM [Order]
WHERE PaymentStatus = 'Paid' AND YEAR(OrderDate) = {currentYear};";

            try
            {
                using (var connection = dbHandler.OpenConnection())
                using (var cmd = new SqlCommand(query, connection))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalIncome = Convert.ToDecimal(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving total income for the current year: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return totalIncome;
        }

        public class TopSellingProduct
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
            public decimal PricePerUnit { get; set; }
            public int QuantitySold { get; set; }
        }
        //get names of top selling products
        public List<TopSellingProduct> GetTopSellingProducts(int topCount = 3)
        {
            List<TopSellingProduct> topSellingProducts = new List<TopSellingProduct>();

            string query = $@"
        SELECT TOP {topCount} 
            o.ProductID, 
            CASE 
                WHEN o.ProductID LIKE 'C%' THEN ISNULL(c.Brand + ' ' + c.Model, 'N/A')
                WHEN o.ProductID LIKE 'CP%' THEN ISNULL(cp.PartName, 'N/A')
            END AS ProductName,
            AVG(o.Amount / o.Quantity) AS PricePerUnit, 
            SUM(o.Quantity) AS TotalQuantity 
        FROM [Order] o
        LEFT JOIN Car c ON o.ProductID = c.CarID
        LEFT JOIN CarPart cp ON o.ProductID = cp.PartID
        GROUP BY o.ProductID, c.Brand, c.Model, cp.PartName
        ORDER BY TotalQuantity DESC;";

            try
            {
                using (var connection = dbHandler.OpenConnection())
                using (var cmd = new SqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string productID = reader.GetString(reader.GetOrdinal("ProductID"));
                        string productName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? "N/A" : reader.GetString(reader.GetOrdinal("ProductName"));
                        decimal pricePerUnit = reader.GetDecimal(reader.GetOrdinal("PricePerUnit"));
                        int totalQuantity = reader.GetInt32(reader.GetOrdinal("TotalQuantity"));

                        topSellingProducts.Add(new TopSellingProduct
                        {
                            ProductID = productID,
                            ProductName = productName,
                            PricePerUnit = pricePerUnit,
                            QuantitySold = totalQuantity
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving top selling products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return topSellingProducts;
        }




        // Gets the total number of cars from the database
        public int GetNumberOfCars()
        {
            try
            {
                using (var connection = dbHandler.OpenConnection())
                {
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Car", connection))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting number of cars: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; // Return 0 on error
            }
        }
        public Dictionary<string, decimal> GetMonthlyIncome()
        {
            var monthlyIncome = new Dictionary<string, decimal>();
            // Get the current year
            int currentYear = DateTime.Now.Year;

            string query = $@"
SELECT 
    MONTH(OrderDate) AS Month, 
    SUM(Amount) AS TotalIncome
FROM [Order]
WHERE PaymentStatus = 'Paid' AND YEAR(OrderDate) = {currentYear}
GROUP BY MONTH(OrderDate)
ORDER BY MONTH(OrderDate);";

            try
            {
                using (var connection = dbHandler.OpenConnection())
                using (var cmd = new SqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int month = reader.GetInt32(reader.GetOrdinal("Month"));
                            decimal totalIncome = reader.GetDecimal(reader.GetOrdinal("TotalIncome"));
                            string monthYear = new DateTime(currentYear, month, 1).ToString("MMM yyyy");

                            monthlyIncome[monthYear] = totalIncome;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving monthly income: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Adjusted to ensure all 12 months of the year are in the dictionary
            for (int month = 1; month <= 12; month++) // Now iterates through December
            {
                string monthYearKey = new DateTime(currentYear, month, 1).ToString("MMM yyyy");
                if (!monthlyIncome.ContainsKey(monthYearKey))
                {
                    monthlyIncome[monthYearKey] = 0; // Initialize to 0 if no income data for this month
                }
            }

            return monthlyIncome;
        }

        public (Dictionary<string, decimal> CarsIncome, Dictionary<string, decimal> CarPartsIncome) GetMonthlyIncomeByCategory()
        {
            var carsIncome = new Dictionary<string, decimal>();
            var carPartsIncome = new Dictionary<string, decimal>();
            int currentYear = DateTime.Now.Year;

            string query = $@"
SELECT 
    ProductID,
    MONTH(OrderDate) AS Month, 
    SUM(Amount) AS TotalIncome
FROM [Order]
WHERE PaymentStatus = 'Paid' AND YEAR(OrderDate) = {currentYear}
GROUP BY MONTH(OrderDate), ProductID
ORDER BY MONTH(OrderDate);";

            try
            {
                using (var connection = dbHandler.OpenConnection())
                using (var cmd = new SqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int month = reader.GetInt32(reader.GetOrdinal("Month"));
                            decimal totalIncome = reader.GetDecimal(reader.GetOrdinal("TotalIncome"));
                            string monthYear = new DateTime(currentYear, month, 1).ToString("MMM yyyy");
                            string productID = reader.GetString(reader.GetOrdinal("ProductID"));

                            if (productID.StartsWith("C") && !productID.StartsWith("CP"))
                            {
                                // This is income from Cars
                                if (!carsIncome.ContainsKey(monthYear))
                                {
                                    carsIncome[monthYear] = 0;
                                }
                                carsIncome[monthYear] += totalIncome;
                            }
                            else if (productID.StartsWith("CP"))
                            {
                                // This is income from Car Parts
                                if (!carPartsIncome.ContainsKey(monthYear))
                                {
                                    carPartsIncome[monthYear] = 0;
                                }
                                carPartsIncome[monthYear] += totalIncome;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving monthly income by category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            for (int month = 1; month <= 12; month++)
            {
                string monthYearKey = new DateTime(currentYear, month, 1).ToString("MMM yyyy");
                if (!carsIncome.ContainsKey(monthYearKey))
                {
                    carsIncome[monthYearKey] = 0; // Initialize to 0 if no income data for this month
                }
                if (!carPartsIncome.ContainsKey(monthYearKey))
                {
                    carPartsIncome[monthYearKey] = 0; // Initialize to 0 if no income data for this month
                }
            }


            return (carsIncome, carPartsIncome);
        }

        // Gets the total number of car parts from the database
        public int GetNumberOfCarParts()
        {
            try
            {
                using (var connection = dbHandler.OpenConnection())
                {
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM CarPart", connection))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting number of car parts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; // Return 0 on error
            }
        }

    }
}
