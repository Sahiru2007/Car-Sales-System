using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_trading_system.Functions
{
    public class LoggedInUser
    {
        public string Username { get; set; }
        public string CustomerID { get; set; }
        public byte[] ImageData { get; set; }

        // Add other properties as needed

        private static LoggedInUser instance;

        private LoggedInUser() { }

        public static LoggedInUser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoggedInUser();
                }
                return instance;
            }
        }
    }

}
