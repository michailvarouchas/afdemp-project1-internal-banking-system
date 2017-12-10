using System;
using System.Collections.Generic;
using System.Text;

namespace InternalBankingSystem
{
    public static class Menus
    {
        //public static void SuperAdminMenu()
        //{
        //    string menu = "\n" +
        //                  "\t\t***************** SUPER ADMIN MENU ***********************\n" +
        //                  "\t\t1. View Cooperative's (super admin) internal bank account\n" +
        //                  "\t\t2. View Members' bank accounts\n" +
        //                  "\t\t3. Deposit to Member's bank account\n" +
        //                  "\t\t4. Withdraw from Member's bank account\n" +
        //                  "\t\t5. Send all today's transactions\n" +
        //                  "\t\t6. Exit\n" +
        //                  "\t\t**********************************************************\n" +
        //                  "\n" +
        //                  "Please type the number of the desired operation: ";
        //    Console.Write(menu);
        //}

        //public static void SimpleUserMenu()
        //{
        //    string menu = "\n" +
        //                  "\t\t************** SIMPLE USER MENU ***************\n" +
        //                  "\t\t1. View bank account\n" +
        //                  "\t\t2. Deposit to Cooperative's (super admin) bank account\n" +
        //                  "\t\t3. Deposit to another Member's bank account\n" +
        //                  "\t\t4. Send today's transactions\n" +
        //                  "\t\t5. Exit\n" +
        //                  "\t\t***********************************************\n" +
        //                  "\n" +
        //                  "Please type the number of the desired operation: ";
        //    Console.Write(menu);
        //}

        private static readonly string _superAdminHeader = String.Format("{0, 10}{1}\n", "", "******************** SUPER ADMIN MENU ********************");
        private static readonly string[] _superAdminMenu = { "1. View Cooperative's (super admin) internal bank account",
                                    "2. View Members' bank accounts",
                                    "3. Deposit to Member's bank account",
                                    "4. Withdraw from Member's bank account",
                                    "5. Send all today's transactions",
                                    "6. Exit"
        };

        private static readonly string _simleUserHeader = String.Format("{0, 10}{1}\n", "", "******************** SIMPLE USER MENU ********************");
        private static string[] _simpleUserMenu = { "1. View own internal bank account",
                                    "2. Deposit to Cooperative's bank account",
                                    "3. Deposit to Member's bank account",
                                    "4. Send all today's transactions",
                                    "5. Exit"
        };

        public static short DisplaySelectionMenu(UserLevel userLevel, string username)
        {
            // A variable to keep track of the current Item, and a simple counter.
            short curItem = 0, c;

            // The object to read in a key
            ConsoleKeyInfo key;

            // Menu items (in order)
            string[] menuItems;
            string header;
            if (userLevel == UserLevel.Admin)
            {
                menuItems = _superAdminMenu;
                header = _superAdminHeader;
            }
            else
            {
                menuItems = _simpleUserMenu;
                header = _simleUserHeader;
            }

            do
            {
                // Clear the screen.
                Console.Clear();

                // Menu header
                DisplayUsername(username);
                Console.WriteLine(header);

                // The loop that goes through all of the menu items.
                for (c = 0; c < menuItems.Length; c++)
                {
                    // If the current item number is our variable c, tab out this option.
                    // You could easily change it to simply change the color of the text.
                    if (curItem == c)
                    {
                        Console.Write("{0, 10}", ">> ");
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("{0, 0}{1}", "", menuItems[c]);
                    }
                    // Just write the current option out if the current item is not our variable c.
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine("{0, 10}{1}", "", menuItems[c]);
                    }
                }

                // Waits until the user presses a key, and puts it into our object key.
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("\n{0, 10}{1}\n", "", "**********************************************************");
                Console.Write("{0, 10}{1}", "", "Select your choice with the arrow keys.");
                key = Console.ReadKey(true);

                // Simply put, if the key the user pressed is a "DownArrow", the current item will deacrease.
                // Likewise for "UpArrow", except in the opposite direction.
                // If curItem goes below 0 or above the maximum menu item, it just loops around to the other end.
                if (key.Key.ToString() == "DownArrow")
                {
                    curItem++;
                    if (curItem > menuItems.Length - 1) curItem = 0;
                }
                else if (key.Key.ToString() == "UpArrow")
                {
                    curItem--;
                    if (curItem < 0) curItem = Convert.ToInt16(menuItems.Length - 1);
                }
                // Loop around until the user presses the enter go.

            } while (key.KeyChar != 13);
            return curItem;
        }

        public static void DisplayUsername(string username)
        {
            Console.WriteLine($"You are logged in as: {username}.\n");
        }

    }
}
