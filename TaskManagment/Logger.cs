using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagment
{
    class Logger
    {
        private static Logger instance;
        private readonly string logFilePath = "log.txt";
        private static readonly object lockObj = new object();

        private Logger() { }

        public static Logger GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                        instance = new Logger();
                }
            }
            return instance;
        }

        public void Log(string message)
        {
            string logMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + message;
            lock (lockObj)
            {
                File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
            }
            Console.WriteLine("[LOG] " + logMessage);
        }
    }
}


