using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAWKI_TCPServer
{
    public class RedisAction
    {
        public string Key { get; set; }
        public string Action { get; set; }
        public RedisAction(string k, string a)
        {
            Key = k;
            Action = a;
        }
    }
}
