using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace InternalBankingSystem
{
    class InternalBankAccounts
    {
        public string Username { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return String.Format("{0, 10} {1, 30:yyyy/MM/dd HH:mm:ss.FFF} {2, 20:C2}\n", Username, TransactionDate, Amount);

        }

        public string AccountsHeader()
        {
            string res = "";
            res += String.Format("{0, 10} {1, 30} {2, 20}\n\n", "Username", "Last Transaction Date", "Amount");
            return res;
        }

        
    }
}
