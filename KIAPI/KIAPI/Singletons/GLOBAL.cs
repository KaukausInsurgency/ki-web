using KIAPI.Asyncs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIAPI.Singletons
{
    public class GLOBAL
    {
        public static Dictionary<int, int> SignalRGameSessions = new Dictionary<int, int>();
        public static Dictionary<int, GameThreadWorker> BackgroundThreadWorkers = new Dictionary<int, GameThreadWorker>();
    }
    
}