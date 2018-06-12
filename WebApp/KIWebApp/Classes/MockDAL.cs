using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIWebApp.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace KIWebApp.Classes
{
    public class MockDAL : IDAL
    {

        GameModel IDAL.GetGame(int serverID)
        {
            GameModel g = new GameModel()
            {
                ServerID = serverID,
                ServerName = "Dev Server",
                IPAddress = "127.0.0.1",
                OnlinePlayersCount = 5,
                RestartTime = new TimeSpan(3, 30, 0).ToString(),
                Status = "Online",
                Depots = ((IDAL)this).GetDepots(serverID),
                CapturePoints = ((IDAL)this).GetCapturePoints(serverID),
                OnlinePlayers = ((IDAL)this).GetOnlinePlayers(serverID)
            };
            return g;
        }

        List<CapturePointModel> IDAL.GetCapturePoints(int serverID)
        {
            List<CapturePointModel> cps = new List<CapturePointModel>
            {
                new CapturePointModel()
                {
                    ID = 1,
                    Name = "Beslan City",
                    LatLong = "43 11.707'N   44 33.693'E",
                    MGRS = "38T MN 64375 82575",
                    Status = "Red",
                    BlueUnits = 0,
                    RedUnits = 10,
                    Image = "Images/markers/flag-red-256x256.png",
                    MaxCapacity = 30
                },

                new CapturePointModel()
                {
                    ID = 2,
                    Name = "Beslan Airport",
                    LatLong = "43 12.216'N   44 36.394'E",
                    MGRS = "38T MN 68038 83498",
                    Status = "Blue",
                    BlueUnits = 9,
                    RedUnits = 0,
                    Image = "Images/markers/flag-blue-256x256.png",
                    MaxCapacity = 30
                },

                new CapturePointModel()
                {
                    ID = 3,
                    Name = "Kirovo City",
                    LatLong = "43 10.373'N   44 24.588'E",
                    MGRS = "38T MN 52028 80182",
                    Status = "Contested",
                    BlueUnits = 9,
                    RedUnits = 5,
                    Image = "Images/markers/flag-contested-256x256.png",
                    MaxCapacity = 30
                },

                new CapturePointModel()
                {
                    ID = 4,
                    Name = "Alagir City",
                    LatLong = "43 10.373'N   44 24.588'E",
                    MGRS = "38T MN 52028 80182",
                    Status = "Neutral",
                    BlueUnits = 0,
                    RedUnits = 0,
                    Image = "Images/markers/flag-neutral-256x256.png",
                    MaxCapacity = 30
                },

                new CapturePointModel()
                {
                    ID = 5,
                    Name = "Test 1",
                    LatLong = "43 10.373'N   44 24.588'E",
                    MGRS = "38T MN 52028 80182",
                    Status = "Neutral",
                    BlueUnits = 0,
                    RedUnits = 0,
                    Image = "Images/markers/flag-neutral-256x256.png",
                    MaxCapacity = 30
                },

                new CapturePointModel()
                {
                    ID = 6,
                    Name = "Test 2",
                    LatLong = "43 10.373'N   44 24.588'E",
                    MGRS = "38T MN 52028 80182",
                    Status = "Neutral",
                    BlueUnits = 0,
                    RedUnits = 0,
                    Image = "Images/markers/flag-neutral-256x256.png",
                    MaxCapacity = 30
                },

                new CapturePointModel()
                {
                    ID = 7,
                    Name = "Test 3",
                    LatLong = "43 10.373'N   44 24.588'E",
                    MGRS = "38T MN 52028 80182",
                    Status = "Neutral",
                    BlueUnits = 0,
                    RedUnits = 0,
                    Image = "Images/markers/flag-neutral-256x256.png",
                    MaxCapacity = 30
                },

                new CapturePointModel()
                {
                    ID = 8,
                    Name = "Test 4",
                    LatLong = "43 10.373'N   44 24.588'E",
                    MGRS = "38T MN 52028 80182",
                    Status = "Neutral",
                    BlueUnits = 0,
                    RedUnits = 0,
                    Image = "Images/markers/flag-neutral-256x256.png",
                    MaxCapacity = 30
                }
            };

            return cps;
        }

        List<DepotModel> IDAL.GetDepots(int serverID)
        {
            List<DepotModel> depots = new List<DepotModel>
            {
                new DepotModel()
                {
                    ID = 1,
                    Name = "Beslan Depot",
                    LatLong = "43 11.430'N   44 33.547'E",
                    MGRS = "38T MN 64175 82063",
                    Capacity = "79 / 150",
                    Status = "Online",
                    Resources = @"
Resource|Count
Infantry|40   
Watchtower Wood|4    
Fuel Tanks|8    
Outpost Wood|4    
Outpost Pipes|4    
Power Truck|4    
Fuel Truck|4    
Cargo Crates|8    
Watchtower Supplies|4    
Command Truck|4    
Tank|8    
Outpost Supplies|4    
Ammo Truck|4    
APC|8    
",
                    Image = "Images/markers/depot-red-256x256.png"
                },

                new DepotModel()
                {
                    ID = 2,
                    Name = "Kirovo Depot",
                    LatLong = "43 10.755'N   44 25.379'E",
                    MGRS = "38T MN 53104 80881",
                    Capacity = "148 / 150",
                    Status = "Online",
                    Resources = @"
Resource|Count
Infantry|40   
Watchtower Wood|4    
Fuel Tanks|8    
Outpost Wood|4    
Outpost Pipes|4    
Power Truck|4    
Fuel Truck|4    
Cargo Crates|8    
Watchtower Supplies|4    
Command Truck|4    
Tank|8    
Outpost Supplies|4    
Ammo Truck|4    
APC|8    
",
                    Image = "Images/markers/depot-blue-256x256.png"
                },

                new DepotModel()
                {
                    ID = 3,
                    Name = "Beslan Backup Depot",
                    LatLong = "43 11.537'N   44 34.866'E",
                    MGRS = "38T MN 65962 82253",
                    Capacity = "148 / 150",
                    Status = "Online",
                    Resources = @"
Resource|Count
Infantry|40   
Watchtower Wood|4    
Fuel Tanks|8    
Outpost Wood|4    
Outpost Pipes|4    
Power Truck|4    
Fuel Truck|4    
Cargo Crates|8    
Watchtower Supplies|4    
Command Truck|4    
Tank|8    
Outpost Supplies|4    
Ammo Truck|4    
APC|8    
",
                    Image = "Images/markers/depot-red-256x256.png"
                },

                new DepotModel()
                {
                    ID = 4,
                    Name = "Alagir Depot",
                    LatLong = "43 11.537'N   44 34.866'E",
                    MGRS = "38T MN 65962 82253",
                    Capacity = "148 / 150",
                    Status = "Online",
                    Resources = @"
Resource|Count
Infantry|40   
Watchtower Wood|4    
Fuel Tanks|8    
Outpost Wood|4    
Outpost Pipes|4    
Power Truck|4    
Fuel Truck|4    
Cargo Crates|8    
Watchtower Supplies|4    
Command Truck|4    
Tank|8    
Outpost Supplies|4    
Ammo Truck|4    
APC|8    
",
                    Image = "Images/markers/depot-red-256x256.png"
                },

                new DepotModel()
                {
                    ID = 5,
                    Name = "Vladikavkaz Depot",
                    LatLong = "43 11.537'N   44 34.866'E",
                    MGRS = "38T MN 65962 82253",
                    Capacity = "148 / 150",
                    Status = "Offline",
                    Resources = @"
Resource|Count
Infantry|40   
Watchtower Wood|4    
Fuel Tanks|8    
Outpost Wood|4    
Outpost Pipes|4    
Power Truck|4    
Fuel Truck|4    
Cargo Crates|8    
Watchtower Supplies|4    
Command Truck|4    
Tank|8    
Outpost Supplies|4    
Ammo Truck|4    
APC|8    
",
                    Image = "Images/markers/depot-contested-256x256.png"
                },

                new DepotModel()
                {
                    ID = 6,
                    Name = "Buron Depot",
                    LatLong = "43 11.537'N   44 34.866'E",
                    MGRS = "38T MN 65962 82253",
                    Capacity = "148 / 150",
                    Status = "Online",
                    Resources = @"
Resource|Count
Infantry|40   
Watchtower Wood|4    
Fuel Tanks|8    
Outpost Wood|4    
Outpost Pipes|4    
Power Truck|4    
Fuel Truck|4    
Cargo Crates|8    
Watchtower Supplies|4    
Command Truck|4    
Tank|8    
Outpost Supplies|4    
Ammo Truck|4    
APC|8    
",
                    Image = "Images/markers/depot-blue-256x256.png"
                }
            };

            return depots;
        }

        List<OnlinePlayerModel> IDAL.GetOnlinePlayers(int serverID)
        {
            List<OnlinePlayerModel> players = new List<OnlinePlayerModel>
            {
                new OnlinePlayerModel()
                {
                    UCID = "AAA",
                    Name = "Igneous01",
                    Role = "KA-50",
                    RoleImage = "Images/role/role-ka50-30x30.png",
                    Side = "Red",
                    Ping = "50ms"
                },
                new OnlinePlayerModel()
                {
                    UCID = "BBB",
                    Name = "HolyCrapBatman",
                    Role = "F-15C",
                    RoleImage = "Images/role/role-f15-30x30.png",
                    Side = "Red",
                    Ping = "121ms"
                },
                new OnlinePlayerModel()
                {
                    UCID = "CCC",
                    Name = "JakeTheSnake",
                    Role = "A10-C",
                    RoleImage = "Images/role/role-a10c-30x30.png",
                    Side = "Red",
                    Ping = "150ms"
                },
                new OnlinePlayerModel()
                {
                    UCID = "DDD",
                    Name = "MarvinStarvin",
                    Role = "MI-8",
                    RoleImage = "Images/role/role-mi8-30x30.png",
                    Side = "Red",
                    Ping = "132ms"
                },
                new OnlinePlayerModel()
                {
                    UCID = "EEE",
                    Name = "PardonMeLads",
                    Role = "A10-C",
                    RoleImage = "Images/role/role-a10c-30x30.png",
                    Side = "Red",
                    Ping = "84ms"
                },
                new OnlinePlayerModel()
                {
                    UCID = "FFF",
                    Name = "LoneStar",
                    Role = "GCI",
                    RoleImage = "Images/role/role-gci-30x30.png",
                    Side = "Red",
                    Ping = "94ms"
                },
                new OnlinePlayerModel()
                {
                    UCID = "GGG",
                    Name = "CarryMyPackBack",
                    Role = "SU-25T",
                    RoleImage = "Images/role/role-su25-30x30.png",
                    Side = "Red",
                    Ping = "92ms"
                },
                new OnlinePlayerModel()
                {
                    UCID = "HHH",
                    Name = "TotalBiscuitEaten",
                    Role = "UH-1H",
                    RoleImage = "Images/role/role-uh1h-30x30.png",
                    Side = "Red",
                    Ping = "35ms"
                }
            };
            return players;
        }

        List<ServerModel> IDAL.GetServers()
        {
            List<ServerModel> servers = new List<ServerModel>
            {
                new ServerModel
                {
                    ServerID = 1,
                    ServerName = "Demo Server",
                    IPAddress = "127.0.0.1",
                    OnlinePlayers = 5,
                    RestartTime = new TimeSpan(3, 30, 0),
                    Status = "Online"
                },
                new ServerModel
                {
                    ServerID = 2,
                    ServerName = "Development Server",
                    IPAddress = "192.105.24.87",
                    OnlinePlayers = 1,
                    RestartTime = new TimeSpan(4, 0, 0),
                    Status = "Offline"
                },
                new ServerModel
                {
                    ServerID = 3,
                    ServerName = "Clan KI Server",
                    IPAddress = "188.82.43.3",
                    OnlinePlayers = 32,
                    RestartTime = new TimeSpan(1, 32, 0),
                    Status = "Restarting"
                }
            };
            return servers;
        }

        MarkerViewModel IDAL.GetMarkers(int serverID)
        {
            MarkerViewModel mm = new MarkerViewModel()
            {
                Depots = ((IDAL)this).GetDepots(serverID),
                CapturePoints = ((IDAL)this).GetCapturePoints(serverID)
            };

            return mm;
        }

        List<ServerModel> IDAL.GetServers(ref IDbConnection conn)
        {
            return ((IDAL)this).GetServers();
        }

        List<DepotModel> IDAL.GetDepots(int serverID, ref IDbConnection conn)
        {
            return ((IDAL)this).GetDepots(serverID);
        }

        List<CapturePointModel> IDAL.GetCapturePoints(int serverID, ref IDbConnection conn)
        {
            return ((IDAL)this).GetCapturePoints(serverID);
        }

        List<OnlinePlayerModel> IDAL.GetOnlinePlayers(int serverID, ref IDbConnection conn)
        {
            return ((IDAL)this).GetOnlinePlayers(serverID);
        }

        GameModel IDAL.GetGame(int serverID, ref IDbConnection conn)
        {
            return ((IDAL)this).GetGame(serverID);
        }

        MarkerViewModel IDAL.GetMarkers(int serverID, ref IDbConnection conn)
        {
            return ((IDAL)this).GetMarkers(serverID);
        }

        List<SideMissionModel> IDAL.GetSideMissions(int serverID)
        {
            throw new NotImplementedException();
        }

        List<SideMissionModel> IDAL.GetSideMissions(int serverID, ref IDbConnection conn)
        {
            throw new NotImplementedException();
        }

        SearchResultsModel IDAL.GetSearchResults(string query)
        {
            throw new NotImplementedException();
        }

        SearchResultsModel IDAL.GetSearchResults(string query, ref IDbConnection conn)
        {
            throw new NotImplementedException();
        }

        ServerViewModel IDAL.GetServerInfo(int serverID)
        {
            throw new NotImplementedException();
        }

        ServerViewModel IDAL.GetServerInfo(int serverID, ref IDbConnection conn)
        {
            throw new NotImplementedException();
        }

        public List<ServerModel> GetServerSearchResults(string query)
        {
            throw new NotImplementedException();
        }

        public List<ServerModel> GetServerSearchResults(string query, ref IDbConnection conn)
        {
            throw new NotImplementedException();
        }

        public List<PlayerModel> GetPlayerSearchResults(string query)
        {
            throw new NotImplementedException();
        }

        public List<PlayerModel> GetPlayerSearchResults(string query, ref IDbConnection conn)
        {
            throw new NotImplementedException();
        }
    }
}