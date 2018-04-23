using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;

namespace TAWKI_TCPServer
{
    public class Utility
    {
        public static ProtocolRequest CreateRequest(ref dynamic j, string IPAddress)
        {
            ProtocolRequest request = new ProtocolRequest
            {
                Action = j["Action"],
                Destination = j["Destination"],
                IsBulkQuery = j["BulkQuery"],
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(j["Data"]), // serialize a new json string for just the data by itself that can be converted into a dictionary later
                Type = ((JToken)j["Data"]).Type,
                IPAddress = IPAddress,
            };

            return request;
        }

        public static string SanitizeHTML(string html, ref IConfigReader config)
        {
            string shtml = System.Web.HttpUtility.HtmlEncode(html);

            foreach (string x in config.SupportedHTML)
            {
                // Opening tags
                {
                    string encoded = "&lt;" + x + "&gt;";
                    string decoded = "<" + x + ">";
                    shtml = shtml.Replace(encoded, decoded);
                }

                // Closing tags
                {
                    string encoded = "&lt;/" + x + "&gt;";
                    string decoded = "</" + x + ">";
                    shtml = shtml.Replace(encoded, decoded);
                }
            }

            return shtml;
        }
    }
}
