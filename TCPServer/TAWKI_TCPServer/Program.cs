using Open.Nat;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAWKI_TCPServer
{
    class Program
    {
        static async Task<string> SetupPortMappings(int port)
        {
            string publicIP = "";
            try
            {
                var discoverer = new NatDiscoverer();

                // using SSDP protocol, it discovers NAT device.
                var NATDevice = await discoverer.DiscoverDeviceAsync();
                var ExternalIP = await NATDevice.GetExternalIPAsync();
                publicIP = ExternalIP.ToString();

                // create a new mapping in the router [external_ip:port -> host_machine:port]
                await NATDevice.CreatePortMapAsync(new Mapping(Protocol.Tcp, port, port, "KIService"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Error in UPnP - this service may not be available on your NAT device. Please check your router / firewall / network. UPnP is disabled. (Error : " + ex.Message + ")");
            }

            return publicIP;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Reading Config...");
            ConfigReader cr = new ConfigReader();
            if (!cr.ConfigReadSuccess)
            {
                Console.ReadKey();
                return;
            }
                
            KIDB.DBConnection = cr.MySQLDBConnect;
            KIDB.RedisDBConnection = cr.RedisDBConnect;

            Console.WriteLine("Attempting To Connect to MySQL database...");
            MySql.Data.MySqlClient.MySqlConnection test_connection = new MySql.Data.MySqlClient.MySqlConnection(cr.MySQLDBConnect);
            try
            {
                test_connection.Open();
                Console.WriteLine("Successful Connection to MySQL Database " + test_connection.Database);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed To Connect to MySQL Database - " + ex.Message);
            }
            finally
            {
                if (test_connection != null)
                    if (test_connection.State == System.Data.ConnectionState.Open 
                        || test_connection.State == System.Data.ConnectionState.Connecting)
                        test_connection.Close();

                test_connection = null;
            }

            Console.WriteLine("Attempting to Connect to Redis database...");

            try
            {
                KIDB.RedisConnection = ConnectionMultiplexer.Connect(cr.RedisDBConnect);
                Console.WriteLine("Successful Connection to Redis Database");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed To Connect to Redis Database - " + ex.Message);
                if (KIDB.RedisConnection != null)
                    if (KIDB.RedisConnection.IsConnected)
                        KIDB.RedisConnection.Close();

                Console.ReadKey();
                return;
            }

            string PublicIP = "";
            if (cr.UseUPnP)
            {
                Console.WriteLine("Using UPnP to forward ports...");
                PublicIP = SetupPortMappings(cr.PortNumber).GetAwaiter().GetResult();
            }

            if (cr.UseWhiteList)
                Console.WriteLine("Using whitelist...");

            KIDB.RedisActionKeyTable = cr.RedisActionKeys;

            SocketServer server = null;
            try
            {
                server = new SocketServer(cr.MaxConnections, cr.PortNumber, cr.WhiteList);
                server.Open();
                if (!String.IsNullOrWhiteSpace(PublicIP))
                {
                    Console.WriteLine("UPNP: Mapped Public {0} to Private {1} on Port {2}", PublicIP, server.Address(), cr.PortNumber);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in creating Socket: " + ex.Message);
                if (server != null)
                    server.Close();
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Server is now running on: " + server.Address() + " - use F2 to close server");
            while(true)
            {
                if (Console.ReadKey().Key == ConsoleKey.F2)
                {
                    server.Close();
                    Console.WriteLine("Server Terminated");
                    return;
                }
            }
        }
    }
}
