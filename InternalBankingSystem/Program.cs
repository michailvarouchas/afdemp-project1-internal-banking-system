using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace InternalBankingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            //culture info and encoding
            CultureInfo culture = CultureInfo.CreateSpecificCulture("el-GR");

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Console.OutputEncoding = System.Text.Encoding.Unicode;

            //login screen
            LogInScreen.WelcomeScreen();
            
            //check db connectivity
            DbAccess db = new DbAccess();
            if (db.IsDbAccessible())
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Database is up and running.\n\n");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Database connection failed.\n\n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
                return;

            }
            Console.BackgroundColor = ConsoleColor.Black;

            int tries = 3;
            while (tries > 0)
            {
                //User input
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = LogInScreen.ReadPassword();

                //check the validity against the db
                //DbAccess db = new DbAccess();
                if (db.IsOnDB(username, password, out UserLevel userLevel))
                {
                    Console.WriteLine("login successfull");

                    //make system wait for 2 seconds
                    System.Threading.Thread.Sleep(2000);
                    Console.Clear();

                    //initialize the logFile list
                    List<string> logFile = new List<string>();

                    //appear proper menu depending on user level
                    if (userLevel == UserLevel.Admin)
                    {
                        //shows the super admin menu & previews the results on a clean console screen
                        while (true)
                        {
                            int selection = Menus.DisplaySelectionMenu(userLevel, username) + 1;
                            switch (selection)
                            {
                                case 1:
                                    Console.Clear();
                                    logFile.Add($"{DateTime.Now} -> From: {username} -> view cooperative's account");
                                    Menus.DisplayUsername(username);
                                    db.ViewSingleAccountDetails(username, password);
                                    Console.WriteLine("Press any key to return to the menu");
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;
                                case 2:
                                    Console.Clear();
                                    Menus.DisplayUsername(username);
                                    logFile.Add($"{DateTime.Now} -> From: {username} -> view all accounts");
                                    db.ViewAllAccountDetails();
                                    Console.WriteLine("Press any key to return to the menu");
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;
                                case 3:
                                    Console.Clear();
                                    Menus.DisplayUsername(username);
                                    db.ViewAllAccountDetails();
                                    //ask for member username
                                    Console.Write("Member's username to depostit to: ");
                                    string depositUsername = Console.ReadLine();
                                    //ask for amount to be transered
                                    Console.Write("Amount to deposit: ");
                                    if (!LogInScreen.IsMoney(out decimal amount))
                                    {
                                        Console.WriteLine("Amount is not a number. Please try again.");
                                        Console.WriteLine("Press any key to return to the menu");
                                        Console.ReadKey();
                                        Console.Clear();
                                        continue;
                                    }
                                    //check if amount is available
                                    if (db.IsMoneyTransfered(username, depositUsername, amount, out string errMsg))
                                    {
                                        Console.WriteLine("Transaction completed successfully");
                                        logFile.Add($"{DateTime.Now} -> " +
                                        $"From: {username} -> " +
                                        $"deposit to: {depositUsername} -> " +
                                        $"amount: {amount}");
                                    }
                                    else
                                    {
                                        Console.WriteLine(errMsg);
                                        logFile.Add($"{DateTime.Now} -> " +
                                        $"From: {username} -> " +
                                        $"deposit to: {depositUsername} -> " +
                                        $"{errMsg}");
                                    }

                                    Console.WriteLine("Press any key to return to the menu");
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;
                                case 4:
                                    Console.Clear();
                                    Menus.DisplayUsername(username);
                                    db.ViewAllAccountDetails();
                                    //ask for member username
                                    Console.Write("Member's username to withdraw from: ");
                                    string withdrawUsername = Console.ReadLine();
                                    //ask for amount to be transered
                                    Console.Write("Amount to withdraw: ");
                                    if (!LogInScreen.IsMoney(out decimal withdrawAmount))
                                    {
                                        Console.WriteLine("Amount is not a number. Please try again.");
                                        Console.WriteLine("Press any key to return to the menu");
                                        Console.ReadKey();
                                        Console.Clear();
                                        continue;
                                    }
                                    //check if amount is available
                                    if (db.IsMoneyTransfered(withdrawUsername, username, withdrawAmount, out string errMsg2))
                                    {
                                        Console.WriteLine("Transaction completed successfully");
                                        logFile.Add($"{DateTime.Now} -> " +
                                        $"From: {username} -> " +
                                        $"withdraw from: {withdrawUsername} -> " +
                                        $"amount: {withdrawAmount}");
                                    }
                                    else
                                    {
                                        Console.WriteLine(errMsg2);
                                        logFile.Add($"{DateTime.Now} -> " +
                                        $"From: {username} -> " +
                                        $"withdraw from: {withdrawUsername} -> " +
                                        $"{errMsg2}");
                                    }

                                    Console.WriteLine("Press any key to return to the menu");
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;
                                case 5:
                                    Console.Clear();
                                    logFile.Add($"{DateTime.Now} -> From: {username} -> Send today's statement");
                                    FileAccess.SendTodayStatement(logFile, username);
                                    Console.Clear();
                                    Console.WriteLine("Statement sent.\n");
                                    LogInScreen.GoodbyeScreen();
                                    System.Threading.Thread.Sleep(3000);
                                    return;
                                case 6:
                                    Console.Clear();
                                    LogInScreen.GoodbyeScreen();
                                    System.Threading.Thread.Sleep(3000);
                                    return;
                            }
                        }
                    }
                    else
                    {
                        //shows the simple user menu & previews the results on a clean console screen
                        while (true)
                        {
                            int selection = Menus.DisplaySelectionMenu(userLevel, username) + 1;
                            switch (selection)
                            {
                                case 1:
                                    Console.Clear();
                                    Menus.DisplayUsername(username);
                                    logFile.Add($"{DateTime.Now} -> From: {username} -> view user account");
                                    db.ViewSingleAccountDetails(username, password);
                                    Console.WriteLine("Press any key to return to the menu");
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;
                                case 2:
                                    Console.Clear();
                                    Menus.DisplayUsername(username);
                                    //ask for amount to be transered
                                    Console.Write("Enter the amount to transfer to Cooperative's account: ");
                                    if (!LogInScreen.IsMoney(out decimal amount))
                                    {
                                        Console.WriteLine("Amount is not a number. Please try again.");
                                        Console.WriteLine("Press any key to return to the menu");
                                        Console.ReadKey();
                                        Console.Clear();
                                        continue;
                                    }
                                    //check if amount is available
                                    if (db.IsMoneyTransfered(username, "admin", amount, out string errMsg1))
                                    {
                                        Console.WriteLine("Transaction completed successfully");
                                        logFile.Add($"{DateTime.Now} -> " +
                                        $"From: {username} -> " +
                                        $"deposit to: admin -> " +
                                        $"amount: {amount}");
                                    }
                                    else
                                    {
                                        Console.WriteLine(errMsg1);
                                        logFile.Add($"{DateTime.Now} -> " +
                                        $"From: {username} -> " +
                                        $"deposit to: admin -> " +
                                        $"{errMsg1}");
                                    }

                                    Console.WriteLine("Press any key to return to the menu");
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;
                                case 3:
                                    Console.Clear();
                                    Menus.DisplayUsername(username);
                                    //ask for member username
                                    Console.Write("Member's username to depostit to: ");
                                    string depositUsername = Console.ReadLine();
                                    //ask for amount to be transered
                                    Console.Write("Amount to deposit: ");
                                    if (!LogInScreen.IsMoney(out decimal amountToMember))
                                    {
                                        Console.WriteLine("Amount is not a number. Please try again.");
                                        Console.WriteLine("Press any key to return to the menu");
                                        Console.ReadKey();
                                        Console.Clear();
                                        continue;
                                    }
                                    //check if amount is available
                                    if (db.IsMoneyTransfered(username, depositUsername, amountToMember, out string errMsg2))
                                    {
                                        Console.WriteLine("Transaction completed successfully");
                                        logFile.Add($"{DateTime.Now} -> " +
                                        $"From: {username} -> " +
                                        $"deposit to: {depositUsername} -> " +
                                        $"amount: {amountToMember}");
                                    }
                                    else
                                    {
                                        Console.WriteLine(errMsg2);
                                        logFile.Add($"{DateTime.Now} -> " +
                                        $"From: {username} -> " +
                                        $"deposit to: {depositUsername} -> " +
                                        $"{errMsg2}");
                                    }
                                    Console.WriteLine("Press any key to return to the menu");
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;
                                case 4:
                                    Console.Clear();
                                    logFile.Add($"{DateTime.Now} -> From: {username} -> Send today's statement");
                                    FileAccess.SendTodayStatement(logFile, username);
                                    Console.Clear();
                                    Console.WriteLine("Statement sent.\n");
                                    LogInScreen.GoodbyeScreen();
                                    System.Threading.Thread.Sleep(3000);
                                    return;
                                case 5:
                                    Console.Clear();
                                    LogInScreen.GoodbyeScreen();
                                    System.Threading.Thread.Sleep(3000);
                                    return;
                            }
                        }

                    }
                }
                else
                {
                    tries--;
                    Console.WriteLine("Incorrect username or password.");
                    Console.WriteLine($"{tries} tries remaining.");
                }
            }
        }
    }
}
