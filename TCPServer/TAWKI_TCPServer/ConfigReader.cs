using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TAWKI_TCPServer
{
    class ConfigReader
    {
        private string _configPath; //test
        private string _MySQLDBConnect;
        private string _RedisDBConnect;
        private int _portNumber;
        private int _maxConnections;
        private bool _useUPnP;
        private bool _useWhiteList;
        private bool _configReadSuccess;
        private List<string> _whitelist;
        private List<string> _supportedHTML;
        private Dictionary<string, string> _redisActionKeyPair;

        public ConfigReader()
        {
            _configPath = Directory.GetCurrentDirectory() + "\\config.xml";
            XmlDocument xml = new XmlDocument();
            _redisActionKeyPair = new Dictionary<string, string>();
            _supportedHTML = new List<string>();
            try
            {
                xml.Load(_configPath);
                XmlNodeList dbxml = xml.GetElementsByTagName("DBConnect");
                XmlNodeList redisxml = xml.GetElementsByTagName("RedisDBConnect");
                XmlNodeList portxml = xml.GetElementsByTagName("Port");
                XmlNodeList maxConnxml = xml.GetElementsByTagName("MaxConnections");
                XmlNodeList whitelistxml = xml.GetElementsByTagName("WhiteList");
                XmlNodeList upnpxml = xml.GetElementsByTagName("UseUPnP");
                XmlNodeList actionkeysxml = xml.SelectNodes("/Config/RedisActionKeys/Pair");
                XmlNodeList supportedHTMLxml = xml.GetElementsByTagName("SupportedHTML");

                if (dbxml.Count == 0)
                    throw new Exception("Could not find <DBConnect> in config");
                if (portxml.Count == 0)
                    throw new Exception("Could not find <Port> in config");
                if (maxConnxml.Count == 0)
                    throw new Exception("Could not find <MaxConnections> in config");
                if (redisxml.Count == 0)
                    throw new Exception("Could not find <RedisDBConnect> in config");
                if (whitelistxml.Count == 0)
                    _useWhiteList = false;
                else
                    _useWhiteList = true;
                if (upnpxml.Count == 0)
                    _useUPnP = false;
         

                _MySQLDBConnect = dbxml[0].InnerText;
                _RedisDBConnect = redisxml[0].InnerText;
                _portNumber = int.Parse(portxml[0].InnerText);
                _maxConnections = int.Parse(maxConnxml[0].InnerText);

                if (_useWhiteList)
                    _whitelist = new List<String>(whitelistxml[0].InnerText.Split(';'));

                if (upnpxml.Count != 0 && (upnpxml[0].InnerText.ToUpper() == "YES" || upnpxml[0].InnerText.ToUpper() == "TRUE"))
                    _useUPnP = true;

                if (actionkeysxml.Count > 0)
                {
                    foreach (XmlNode x in actionkeysxml)
                    {
                        if (x.Attributes["Action"] != null && x.Attributes["RedisKey"] != null)
                        {
                            _redisActionKeyPair.Add(x.Attributes["Action"].Value, x.Attributes["RedisKey"].Value);
                        }
                        else
                        {
                            throw new Exception("<RedisActionKeys><Pair> - xml malformed (missing attribute 'Action' or 'RedisKey'");
                        }
                    }
                }

                if (supportedHTMLxml.Count > 0)
                {
                    _supportedHTML = supportedHTMLxml[0].InnerText.Split(',').ToList<string>();
                }

                _configReadSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - could not open or read config file (path: " + _configPath + ") - " + ex.Message);
                _configReadSuccess = false;
            }
        }

        public int PortNumber
        {
            get { return _portNumber; }
        }

        public int MaxConnections
        {
            get { return _maxConnections; }
        }

        public string MySQLDBConnect
        {
            get { return _MySQLDBConnect; }
        }

        public string RedisDBConnect
        {
            get { return _RedisDBConnect; }
        }

        public bool ConfigReadSuccess
        {
            get { return _configReadSuccess; }
        }

        public bool UseUPnP
        {
            get { return _useUPnP; }
        }

        public bool UseWhiteList
        {
            get { return _useWhiteList; }
        }

        public List<string> WhiteList
        {
            get { return _whitelist; }
        }

        public List<string> SupportedHTML
        {
            get { return _supportedHTML; }
        }

        public Dictionary<string, string> RedisActionKeys
        {
            get { return _redisActionKeyPair; }
        }
    }
}
