using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TAWKI_TCPServer.Interfaces;

namespace TAWKI_TCPServer.Implementations
{
    public class FileLogger : ILogger
    {
        private string _logpath;

        public FileLogger(string path)
        {
            _logpath = path;
        }

        void ILogger.Log(string data)
        {
            if (!File.Exists(_logpath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(_logpath))
                {
                    sw.WriteLine(DateTime.Now.ToString("{0:d/M/yyyy HH:mm:ss}") + " - Log File for client");
                    sw.WriteLine(DateTime.Now.ToString("{0:d/M/yyyy HH:mm:ss}") + data);
                    // sw.WriteLine(data);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(_logpath))
                {
                    sw.WriteLine(DateTime.Now.ToString("{0:d/M/yyyy HH:mm:ss}") + data);
                    //sw.WriteLine(data);
                }
            }
        }
    }
}
