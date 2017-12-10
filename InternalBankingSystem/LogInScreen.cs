using System;
using System.Collections.Generic;
using System.Text;

namespace InternalBankingSystem
{
    public static class LogInScreen
    {
        public static void WelcomeScreen()
        {
            string msg = "\n" +
                         "\t****************************************************************************************************\n" +
                         "\t*                                                                  Coding Bootcamp 3 -> project 1  *\n" +
                         "\t*                                                                               Michail Varouchas  *\n" +
                         "\t*                                                                                                  *\n" +
                         "\t*                                     Internal Banking System                                      *\n" +
                         "\t*                                             Welcome                                              *\n" +
                         "\t*                                                                                                  *\n" +
                         "\t*                                                                                                  *\n" +
                         "\t*                                                                                                  *\n" +
                         "\t****************************************************************************************************\n";
                Console.WriteLine(msg);            
        }

        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (Char.IsLetterOrDigit(info.KeyChar))
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove the last character from the password
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }
            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

        public static bool IsMoney(out decimal money)
        {
            bool isInputMoney = Decimal.TryParse(Console.ReadLine(), out money);
            if (isInputMoney)
            {
                return true;
            }
            return false;
        }

        public static void GoodbyeScreen()
        {
            string msg = "\n" +
                         "\t****************************************************************************************************\n" +
                         "\t*                                                                  Coding Bootcamp 3 -> project 1  *\n" +
                         "\t*                                                                               Michail Varouchas  *\n" +
                         "\t*                                                                                                  *\n" +
                         "\t*                                     Internal Banking System                                      *\n" +
                         "\t*                                             GOODBYE!                                              *\n" +
                         "\t*                                                                                                  *\n" +
                         "\t*                                                                                                  *\n" +
                         "\t*                                                                                                  *\n" +
                         "\t****************************************************************************************************\n";
            Console.WriteLine(msg);
        }
    }
}
