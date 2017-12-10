using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace InternalBankingSystem
{
    public enum UserLevel
    {
        Admin = 1,
        User = 2,
        NotRegistered = 3
    }

    class DbAccess
    {
        //connection credentials
        private readonly string _connectionCredentials = "Data Source=.\\SQLEXPRESS;" +
                                                          "Initial Catalog = afdemp_csharp_1;" +
                                                          "Integrated Security = true;";

        //view own bank account
        public void ViewSingleAccountDetails(string username, string password)
        {
            string querry = "SELECT username AS Username, transaction_date AS Last_Transaction, amount AS Amount " +
                "FROM users " +
                "JOIN accounts ON users.id = accounts.user_id " +
                "WHERE username = @username AND password = @password";

            using (SqlConnection connection = new SqlConnection(_connectionCredentials))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(querry, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        var accountDet = new InternalBankAccounts
                        {
                            Username = Convert.ToString(reader["Username"]),
                            TransactionDate = Convert.ToDateTime(reader["Last_Transaction"]),
                            Amount = Convert.ToDecimal(reader["Amount"])
                        };
                        string res = "";
                        if (username == "admin")
                        {
                            res += String.Format("{0, 45}\n\n", "COOPERATIVE'S BANK ACCOUNT");
                        }
                        else
                        {
                            res += String.Format("{0, 45}\n\n", "MY BANK ACCOUNT");
                        }

                        res += accountDet.AccountsHeader();
                        res += accountDet.ToString();
                        Console.WriteLine(res);
                    }
                }
            }
        }

        //view all bank accounts (super admin)
        public void ViewAllAccountDetails()
        {
            string querry = "SELECT username AS Username, transaction_date AS Last_Transaction, amount AS Amount " +
                "FROM users " +
                "JOIN accounts ON users.id = accounts.user_id ";

            using (SqlConnection connection = new SqlConnection(_connectionCredentials))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(querry, connection))
                {
                    List<InternalBankAccounts> list = new List<InternalBankAccounts>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new InternalBankAccounts
                            {
                                Username = Convert.ToString(reader["Username"]),
                                TransactionDate = Convert.ToDateTime(reader["Last_Transaction"]),
                                Amount = Convert.ToDecimal(reader["Amount"])
                            });
                        }
                        string res = String.Format("{0, 45}\n\n", "ALL BANK ACCOUNTS");
                        res += list[0].AccountsHeader();
                        foreach (var item in list)
                        {
                            res += item.ToString();
                        }
                        Console.WriteLine(res);
                    }
                }
            }
        }

        //make a trasaction
        public bool IsMoneyTransfered(string senderUsername, string receiverUsername, decimal amountToTranfer, out string errMsg)
        {
            string querry = "UPDATE accounts " +
                             "SET amount = amount - @amount, transaction_date = GETDATE() " +
                             "FROM accounts, users " +
                             "WHERE accounts.user_id = users.id AND username = @senderUsername " +
                             "AND @amount < (SELECT amount FROM users JOIN accounts ON accounts.user_id = users.id WHERE username = @senderUsername) ; ";

            string querry2 = "UPDATE accounts " +
                             "SET amount = amount + @amount, transaction_date = GETDATE() " +
                             "FROM accounts, users " +
                             "WHERE accounts.user_id = users.id AND username = @receiverUsername " +
                             "AND @amount < (SELECT amount FROM users JOIN accounts ON accounts.user_id = users.id WHERE username = @senderUsername) ; ";

            int querryLines1;
            int querryLines2;
            errMsg = "";

            using (SqlConnection connection = new SqlConnection(_connectionCredentials))
            {
                connection.Open();
                //Start a local transaction
                SqlTransaction transaction = connection.BeginTransaction();

                //make the first querry
                using (SqlCommand command = new SqlCommand(querry, connection, transaction))
                {
                    command.Parameters.AddWithValue("senderUsername", senderUsername);
                    command.Parameters.AddWithValue("amount", amountToTranfer);

                    querryLines1 = command.ExecuteNonQuery(); 
                    
                }
                //make the second querry
                using (SqlCommand command = new SqlCommand(querry2, connection, transaction))
                {
                    command.Parameters.AddWithValue("senderUsername", senderUsername);
                    command.Parameters.AddWithValue("receiverUsername", receiverUsername);
                    command.Parameters.AddWithValue("amount", amountToTranfer);

                    querryLines2 = command.ExecuteNonQuery();

                }
                //commit the transaction
                if(senderUsername == receiverUsername)
                {
                    errMsg = "Transaction cannot be performed to your own account.";
                    return false;
                }
                else if (!(this.IsOnDB(receiverUsername) && this.IsOnDB(senderUsername)))
                {
                    errMsg = "User does not exist on DB.";
                    return false;
                }
                else if (querryLines1 + querryLines2 == 2)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    errMsg = $"{senderUsername}'s balance is not enough.";
                    return false;
                }
                
            }
        }

        //check username password validity and user level
        public bool IsOnDB(string username, string password, out UserLevel userLevel)
        {
            userLevel = UserLevel.NotRegistered;

            string querry = "SELECT [username], [password] " +
                "FROM [users] " +
                "WHERE [username] = @username AND [password] =@password";

            using (SqlConnection connection = new SqlConnection(_connectionCredentials))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(querry, connection))
                {
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var cred = new
                            {
                                Username = Convert.ToString(reader["username"]),
                                Password = Convert.ToString(reader["password"])
                            };
                            if (cred.Username == "admin" && cred.Password == "admin")
                            {
                                userLevel = UserLevel.Admin;
                            }
                            else
                            {
                                userLevel = UserLevel.User;
                            }
                            return true;
                        }
                        return false;
                    }
                }
            }

        }

        public bool IsOnDB(string username)
        {
            string querry = "SELECT [username]" +
                "FROM [users] " +
                "WHERE [username] = @username";

            using (SqlConnection connection = new SqlConnection(_connectionCredentials))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(querry, connection))
                {
                    command.Parameters.AddWithValue("username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
        }
    }
}
