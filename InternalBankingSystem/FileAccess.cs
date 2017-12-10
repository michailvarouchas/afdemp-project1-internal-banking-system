using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace InternalBankingSystem
{
    static class FileAccess
    {
        public static void SendTodayStatement(List<string> list, string username)
        {
            string filename = "statement_" + username + "_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";

            using (StreamWriter sw = File.AppendText(filename))
            {
                foreach (var transaction in list)
                {
                    sw.WriteLine(transaction);
                }

            }
        }
    }
}
