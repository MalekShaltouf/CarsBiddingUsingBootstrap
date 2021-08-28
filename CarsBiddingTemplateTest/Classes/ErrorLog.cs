using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CarsBiddingUsingBootstrap.Classes
{
    public class ErrorLog
    {
        public static void WriteInLog(string msg,string trackStack,string PageName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\ErrorLog\errorlog.txt";
            if (!File.Exists(path))
            {
                using (File.CreateText(path)) { }
                
            }
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine("PageName: " + PageName);
                sw.WriteLine("Message: " + msg);
                sw.WriteLine("ExceptionTrackStack: " + trackStack.Trim());
                sw.WriteLine("Date: " + DateTime.Now);
                sw.WriteLine("===========================================================================");
            }
        }
    }
}