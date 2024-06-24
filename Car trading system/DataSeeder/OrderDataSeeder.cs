using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
namespace Car_trading_system.DataSeeder
{
    class OrderDataSeeder
    {
        private readonly DatabaseHandler dbHandler;
        public OrderDataSeeder(DatabaseHandler dbHandler)
        {
            this.dbHandler = dbHandler;
        }
        public void Seed(int numberOfRecords)
        {
            Random random = new Random();
            DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime endOfYear = new DateTime(DateTime.Now.Year, 12, 31);

            using (var connection = dbHandler.OpenConnection())
            {
                for (int i = 0; i < numberOfRecords; i++)
                {
                    Order order = new Order
                    {
                        CustomerId = "C" + random.Next(1000, 9999).ToString("D4"),
                        ProductId = GenerateProductId(random),
                        Quantity = random.Next(1, 10),
                        Amount = random.Next(100, 10000),
                        OrderDate = RandomDate(startOfYear, endOfYear, random),
                        Status = "Completed",
                        PaymentStatus = "Paid"
                    };

                    InsertOrderData(connection, order);
                }
            } // The connection will be automatically closed here due to the 'using' statement
        }
        public class Order
        {
            public string CustomerId { get; set; }
            public string ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal Amount { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; }
            public string PaymentStatus { get; set; }
        }
        private static void InsertOrderData(SqlConnection connection, Order order)
        {
            string insertQuery = @"
                INSERT INTO [Order] (CustomerID, ProductID, Quantity, Amount, OrderDate, Status, PaymentStatus)
                VALUES (@CustomerID, @ProductID, @Quantity, @Amount, @OrderDate, @Status, @PaymentStatus);";

            using (var cmd = new SqlCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@CustomerID", order.CustomerId);
                cmd.Parameters.AddWithValue("@ProductID", order.ProductId);
                cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
                cmd.Parameters.AddWithValue("@Amount", order.Amount);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@Status", order.Status);
                cmd.Parameters.AddWithValue("@PaymentStatus", order.PaymentStatus);

                cmd.ExecuteNonQuery();
            }
        
    }

        private static DateTime RandomDate(DateTime start, DateTime end, Random random)
        {
            int range = (end - start).Days;
            return start.AddDays(random.Next(range));
        }

        private static string GenerateProductId(Random random)
        {
            // Assuming product IDs are like 'C1001' or 'CP1001'
            return (random.Next(2) == 0 ? "C" : "CP") + random.Next(1000, 9999).ToString("D4");
        }
    
}
}
