using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Interfaces;

namespace TelegramBot
{
    public static class Logger
    {
        public static void WriteLog(string message)
        {
            string logPath = @"C:\Repository\MyProjectTelegramBot\TelegramBot\logFile.txt";
            using(StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine($"{DateTime.Now} : {message}");  
            }
        }
    }
}
