using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIWebApp.Models;
using System.Data;

namespace KIWebApp.Classes
{
    public interface IDAL
    {
        List<ServerModel> GetServers();
        List<ServerModel> GetServers(ref IDbConnection conn);
        List<DepotModel> GetDepots(int serverID);
        List<DepotModel> GetDepots(int serverID, ref IDbConnection conn);
        List<CapturePointModel> GetCapturePoints(int serverID);
        List<CapturePointModel> GetCapturePoints(int serverID, ref IDbConnection conn);
        List<SideMissionModel> GetSideMissions(int serverID);
        List<SideMissionModel> GetSideMissions(int serverID, ref IDbConnection conn);
        List<MapLayerModel> GetMapLayers(int mapID);
        List<MapLayerModel> GetMapLayers(int mapID, ref IDbConnection conn);
        List<OnlinePlayerModel> GetOnlinePlayers(int serverID);
        List<OnlinePlayerModel> GetOnlinePlayers(int serverID, ref IDbConnection conn);
        GameMapModel GetGameMap(int serverID);
        GameMapModel GetGameMap(int serverID, ref IDbConnection conn);
        GameModel GetGame(int serverID);
        GameModel GetGame(int serverID, ref IDbConnection conn);
        MarkerViewModel GetMarkers(int serverID);
        MarkerViewModel GetMarkers(int serverID, ref IDbConnection conn);
        ServerViewModel GetServerInfo(int serverID);
        ServerViewModel GetServerInfo(int serverID, ref IDbConnection conn);
        SearchResultsModel GetSearchResults(string query);
        SearchResultsModel GetSearchResults(string query, ref IDbConnection conn);
        List<ServerModel> GetServerSearchResults(string query);
        List<ServerModel> GetServerSearchResults(string query, ref IDbConnection conn);
        List<PlayerModel> GetPlayerSearchResults(string query);
        List<PlayerModel> GetPlayerSearchResults(string query, ref IDbConnection conn);
    }
}