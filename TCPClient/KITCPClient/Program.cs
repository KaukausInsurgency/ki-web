using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace KITCPClient
{
    class Program
    {
        private static byte[] FormatRequest(string data)
        {
            string nmsg = string.Format("{0:D6}", data.Length) + data;
            return System.Text.Encoding.UTF8.GetBytes(nmsg);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("ERROR: No Arguments Provided - expected ScriptPath IP Port");
                Console.ReadLine();
                return;
            }
            if (args.Length != 3)
            {
                Console.WriteLine("ERROR: Invalid Number of Arguments Provided - expected ScriptPath IP Port");
                Console.ReadLine();
                return;
            }

            string filepath = args[0];
            string IP = args[1];
            string port = args[2];

            if (!File.Exists(filepath))
            {
                Console.WriteLine("ERROR: File Path " + args[0] + " does not exist.");
                Console.ReadLine();
                return;
            }

            Socket clientsock = null;
            Console.WriteLine("CONNECTING TO TCP HOST");
            try
            {
                clientsock = new Socket(SocketType.Stream, ProtocolType.Tcp);
                clientsock.Connect(IP, Convert.ToInt32(port));
            }
            catch (Exception ex)
            {
                if (clientsock != null && clientsock.Connected)
                    clientsock.Disconnect(true);
                Console.WriteLine("ERROR: Could not connect to TCP host at " + IP + ":" + port + " - " + ex.Message);
                Console.ReadLine();
                return;
            }

            Console.WriteLine("CONNECTED TO " + IP + ":" + port + " SUCCESSFULLY");
            Console.WriteLine("RUNNING SCRIPT");
            int i = 1;
            foreach (var line in File.ReadAllLines(args[0]))
            {
                if (line.Substring(0,4).ToUpper() == "WAIT")
                {
                    int ms;
                    string val = line.Replace("WAIT", "").Replace(" ", "");
                    if (Int32.TryParse(val, out ms))
                    {
                        Thread.Sleep(ms);
                    }
                    else
                    {
                        Console.WriteLine("SCRIPT ERROR: WAIT has an invalid value (value: " + val + ", line: " + i + ")");
                    }           
                }
                else
                {
                    try
                    {
                        clientsock.Send(FormatRequest(line), SocketFlags.None);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("TCP SEND ERROR: (line: " + i + ") - " + ex.Message);
                    }
                }
                i++;
            }
            Console.WriteLine("SCRIPT FINISHED");
            if (clientsock != null && clientsock.Connected)
            {
                clientsock.Disconnect(true);
            }
            Console.ReadLine();
        }
    }
}
