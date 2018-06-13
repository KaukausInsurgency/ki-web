using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIWebApp.Models;
using System.Data;
using StackExchange.Redis;

namespace KIWebApp.Classes
{
    public interface IDAL
    {
        List<ServerModel> GetServers();
        List<ServerModel> GetServers(ref IDbConnection conn);
        List<DepotModel> GetDepots(int serverID);
        List<DepotModel> GetDepots(int serverID, ref IConnectionMultiplexer conn);
        List<CapturePointModel> GetCapturePoints(int serverID);
        List<CapturePointModel> GetCapturePoints(int serverID, ref IConnectionMultiplexer conn);
        List<SideMissionModel> GetSideMissions(int serverID);
        List<SideMissionModel> GetSideMissions(int serverID, ref IConnectionMultiplexer conn);
        List<OnlinePlayerModel> GetOnlinePlayers(int serverID);
        List<OnlinePlayerModel> GetOnlinePlayers(int serverID, ref IConnectionMultiplexer conn);
        GameModel GetGame(int serverID);
        GameModel GetGame(int serverID, ref IDbConnection dbconn, ref IConnectionMultiplexer redisconn);
        MarkerViewModel GetMarkers(int serverID);
        MarkerViewModel GetMarkers(int serverID, ref IConnectionMultiplexer conn);
        ServerViewModel GetServerInfo(int serverID);
        ServerViewModel GetServerInfo(int serverID, ref IDbConnection conn);
        SearchResultsModel GetSearchResults(string query);
        SearchResultsModel GetSearchResults(string query, ref IDbConnection conn);
        List<ServerModel> GetServerSearchResults(string query);
        List<ServerModel> GetServerSearchResults(string query, ref IDbConnection conn);
        List<PlayerModel> GetPlayerSearchResults(string query);
        List<PlayerModel> GetPlayerSearchResults(string query, ref IDbConnection conn);
        List<CustomMenuItemModel> GetCustomMenuItems(int serverID);
        List<CustomMenuItemModel> GetCustomMenuItems(int serverID, ref IDbConnection conn);
    }
}