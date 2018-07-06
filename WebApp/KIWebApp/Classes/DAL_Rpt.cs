using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIWebApp.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace KIWebApp.Classes
{
    public class DAL_Rpt : IDAL_Rpt
    {
        private const string SP_PLAYER_STATS_OVERALL = "rptsp_GetPlayerOverallStats";
        private const string SP_TOP_AIRFRAME_SERIES = "rptsp_GetTopAirframeSeries";
        private const string SP_LAST_SESSION_SERIES = "rptsp_GetLastSessionSeries";
        private const string SP_LAST_X_SESSIONS_SERIES = "rptsp_GetLast5SessionsBarGraph";
        private const string SP_PLAYER_ONLINE_ACTIVITY = "rptsp_GetOnlineActivity";
        private const string SP_SORTIES_OVER_TIME = "rptsp_GetSortiesOverTime";
        private const string SP_SCORE_OVER_TIME = "rptsp_GetScoreOverTime";
        private const string SP_BEST_SORTIE_STATS = "rptsp_GetPlayerBestSortieStats";

        private string _DBConnection;

        public DAL_Rpt()
        {
            _DBConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DBMySqlConnect"].ConnectionString;
        }

        public DAL_Rpt(string connection)
        {
            _DBConnection = connection;
        }

        List<RptAirframeOverallStatsModel> IDAL_Rpt.GetAirframeStats(string ucid)
        {
            MySqlConnection conn = new MySqlConnection(_DBConnection);
            try
            {
                conn.Open();
                return ((IDAL_Rpt)this).GetAirframeStats(ucid, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<RptAirframeOverallStatsModel> IDAL_Rpt.GetAirframeStats(string ucid, ref MySqlConnection conn)
        {
            throw new NotImplementedException();
        }

        RptPlayerOverallStatsModel IDAL_Rpt.GetOverallPlayerStats(string ucid)
        {
            MySqlConnection conn = new MySqlConnection(_DBConnection);
            try
            {
                conn.Open();
                return ((IDAL_Rpt)this).GetOverallPlayerStats(ucid, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        RptPlayerOverallStatsModel IDAL_Rpt.GetOverallPlayerStats(string ucid, ref MySqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            RptPlayerOverallStatsModel playerstats = new RptPlayerOverallStatsModel();
            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SP_PLAYER_STATS_OVERALL);
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter("UCID", ucid));
            MySqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);

            foreach (DataRow dr in dt.Rows)
            {
                TimeSpan totaltime;
                if (dr["TotalGameTime"] == DBNull.Value || dr["TotalGameTime"] == null)
                    totaltime = new TimeSpan(0, 0, 0);
                else
                    totaltime = new TimeSpan(TimeSpan.TicksPerSecond * dr.Field<long>("TotalGameTime"));

                decimal sortiesuccessratio = 0;
                if (dr["SortieSuccessRatio"] != DBNull.Value && dr["SortieSuccessRatio"] != null)
                    sortiesuccessratio = Math.Round(dr.Field<decimal>("SortieSuccessRatio"), 2) * 100;

                decimal slingloadsuccessratio = 0;
                if (dr["SlingLoadSuccessRatio"] != DBNull.Value && dr["SlingLoadSuccessRatio"] != null)
                    slingloadsuccessratio = Math.Round(dr.Field<decimal>("SlingLoadSuccessRatio"), 2) * 100;

                decimal killdeathejectratio = 0;
                if (dr["KillDeathEjectRatio"] != DBNull.Value && dr["KillDeathEjectRatio"] != null)
                    killdeathejectratio = Math.Round(dr.Field<decimal>("KillDeathEjectRatio"), 2);

                decimal transportratio = 0;
                if (dr["TransportSuccessRatio"] != DBNull.Value && dr["TransportSuccessRatio"] != null)
                    transportratio = Math.Round(dr.Field<decimal>("TransportSuccessRatio"), 2) * 100;

                playerstats.PlayerName = dr.Field<string>("PlayerName");
                playerstats.PlayerLives = dr.Field<int>("PlayerLives");
                playerstats.PlayerBanned = dr.Field<ulong>("PlayerBanned") == 1;  // for some reason MySql treats BIT(1) as ulong
                playerstats.TotalGameTime = (int)(totaltime.TotalHours) + ":" + totaltime.ToString(@"mm\:ss");
                playerstats.TakeOffs = dr.Field<int>("TakeOffs");
                playerstats.Landings = dr.Field<int>("Landings");
                playerstats.SlingLoadHooks = dr.Field<int>("SlingLoadHooks");
                playerstats.SlingLoadUnhooks = dr.Field<int>("SlingLoadUnhooks");
                playerstats.Kills = dr.Field<int>("Kills");
                playerstats.Deaths = dr.Field<int>("Deaths");
                playerstats.Ejects = dr.Field<int>("Ejects");
                playerstats.TransportMounts = dr.Field<int>("TransportMounts");
                playerstats.TransportDismounts = dr.Field<int>("TransportDismounts");
                playerstats.DepotResupplies = dr.Field<int>("DepotResupplies");
                playerstats.CargoUnpacked = dr.Field<int>("CargoUnpacked");

                // for some reason mysql likes to cast all SUM(x) as long even on an INT column :(
                playerstats.GroundKills = Convert.ToInt32(dr.Field<long>("GroundKills")); 
                playerstats.ShipKills = Convert.ToInt32(dr.Field<long>("ShipKills"));
                playerstats.HelicopterKills = Convert.ToInt32(dr.Field<long>("HelicopterKills"));
                playerstats.AirKills = Convert.ToInt32(dr.Field<long>("AirKills"));

                playerstats.SortieSuccessRatio = sortiesuccessratio;
                playerstats.SlingLoadSuccessRatio = slingloadsuccessratio;
                playerstats.KillDeathEjectRatio = killdeathejectratio;
                playerstats.TransportSuccessRatio = transportratio;      
                break;
            }

            playerstats.BestSortieStats = ((IDAL_Rpt)this).GetBestSortieStats(ucid, ref conn);
            playerstats.TopAirframesSeries = ((IDAL_Rpt)this).GetTopAirframeSeries(ucid, ref conn);
            playerstats.LastSessionSeries = ((IDAL_Rpt)this).GetLastSessionSeries(ucid, ref conn);
            playerstats.LastXSessionsEventsSeries = ((IDAL_Rpt)this).GetLastSetSessions(ucid, ref conn);
            playerstats.OnlineActivity = ((IDAL_Rpt)this).GetPlayerOnlineActivity(ucid, ref conn);
            playerstats.SortiesOverTime = ((IDAL_Rpt)this).GetSortiesOverTime(ucid, ref conn);
            playerstats.ScoreOverTime = ((IDAL_Rpt)this).GetScoreOverTime(ucid, ref conn);
            return playerstats;
        }

        List<RptTopAirframeSeriesModel> IDAL_Rpt.GetTopAirframeSeries(string ucid)
        {
            MySqlConnection conn = new MySqlConnection(_DBConnection);
            try
            {
                conn.Open();
                return ((IDAL_Rpt)this).GetTopAirframeSeries(ucid, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<RptTopAirframeSeriesModel> IDAL_Rpt.GetTopAirframeSeries(string ucid, ref MySqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            List<RptTopAirframeSeriesModel> airframe_series = new List<RptTopAirframeSeriesModel>();
            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SP_TOP_AIRFRAME_SERIES)
            {
                Connection = conn,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new MySqlParameter("UCID", ucid));
            cmd.Parameters.Add(new MySqlParameter("RowLimit", 5));
            MySqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);

            foreach (DataRow dr in dt.Rows)
            {
                RptTopAirframeSeriesModel data = new RptTopAirframeSeriesModel
                {
                    name = dr.Field<string>("Airframe"),
                    y = dr.Field<long>("TotalTime")
                };

                airframe_series.Add(data);
            }
            return airframe_series;
        }

        List<RptSessionSeriesModel> IDAL_Rpt.GetLastSessionSeries(string ucid)
        {
            MySqlConnection conn = new MySqlConnection(_DBConnection);
            try
            {
                conn.Open();
                return ((IDAL_Rpt)this).GetLastSessionSeries(ucid, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<RptSessionSeriesModel> IDAL_Rpt.GetLastSessionSeries(string ucid, ref MySql.Data.MySqlClient.MySqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            List<RptSessionSeriesModel> session_series = new List<RptSessionSeriesModel>();
            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SP_LAST_SESSION_SERIES)
            {
                Connection = conn,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new MySqlParameter("UCID", ucid));
            MySqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);

            foreach (DataRow dr in dt.Rows)
            {
                string evt = dr.Field<string>("Event");
                long t = dr.Field<long>("Time");
                RptSessionDataPlotModel plot = new RptSessionDataPlotModel();
                plot.x = t * 1000;  // highcharts treats the number as milliseconds - need to multiply by 1000 to convert seconds to milliseconds
                bool IsNew = false;
                RptSessionSeriesModel series = session_series.FirstOrDefault(x => x.name == evt);
                if (series == null)
                {
                    series = new RptSessionSeriesModel();
                    series.name = evt;
                    plot.y = 1;
                    IsNew = true;
                }
                else
                {
                    plot.y = series.data.Count + 1;
                }
                series.data.Add(plot);

                if (IsNew)
                    session_series.Add(series);
            }
            return session_series;
        }

        List<RptSessionEventsDataModel> IDAL_Rpt.GetLastSetSessions(string ucid)
        {
            MySqlConnection conn = new MySqlConnection(_DBConnection);
            try
            {
                conn.Open();
                return ((IDAL_Rpt)this).GetLastSetSessions(ucid, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<RptSessionEventsDataModel> IDAL_Rpt.GetLastSetSessions(string ucid, ref MySqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            List<RptSessionEventsDataModel> session_series = new List<RptSessionEventsDataModel>();
            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SP_LAST_X_SESSIONS_SERIES)
            {
                Connection = conn,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new MySqlParameter("UCID", ucid));
            MySqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);

            // hash of [SessionID].[EventName].[Count]
            Dictionary<long, Dictionary<string, int>> event_count_map = new Dictionary<long, Dictionary<string, int>>();
            foreach (DataRow dr in dt.Rows)
            {
                long sessionID = dr.Field<long>("SessionID");
                string evt = dr.Field<string>("Event");
                long c = dr.Field<long>("EventCount");  // for some reason MySQL typed this as a long when it should not be

                if (!event_count_map.ContainsKey(sessionID))
                {
                    event_count_map[sessionID] = new Dictionary<string, int>();

                    // hardcoding sucks, but unless the sproc can return the data definitions first, it's difficult for this code to know what data plots to zero out
                    event_count_map[sessionID]["KILL"] = 0;
                    event_count_map[sessionID]["TAKEOFF"] = 0;
                    event_count_map[sessionID]["LAND"] = 0;
                }
                      
                event_count_map[sessionID][evt] = Convert.ToInt32(c);
            }

            foreach (var session_pair in event_count_map)
            {
                foreach(var event_pair in session_pair.Value)
                {
                    RptSessionEventsDataModel series = session_series.FirstOrDefault(x => x.name == event_pair.Key);
                    if (series == null)
                    {
                        series = new RptSessionEventsDataModel();
                        series.name = event_pair.Key;
                        series.data.Add(event_pair.Value);
                        session_series.Add(series);
                    }
                    else
                    {
                        series.data.Add(event_pair.Value);
                    }
                    
                }

            }

            return session_series;
        }

        RptPlayerOnlineActivityModel IDAL_Rpt.GetPlayerOnlineActivity(string ucid)
        {
            MySqlConnection conn = new MySqlConnection(_DBConnection);
            try
            {
                conn.Open();
                return ((IDAL_Rpt)this).GetPlayerOnlineActivity(ucid, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        RptPlayerOnlineActivityModel IDAL_Rpt.GetPlayerOnlineActivity(string ucid, ref MySqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();
            RptPlayerOnlineActivityModel model = new RptPlayerOnlineActivityModel();
            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SP_PLAYER_ONLINE_ACTIVITY)
            {
                Connection = conn,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new MySqlParameter("UCID", ucid));
            MySqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);

            foreach (DataRow dr in dt.Rows)
            {
                object[] array = { DateTimeJavaScript.ToJavaScriptMilliseconds(dr.Field<DateTime>("Date")), dr.Field<long>("TotalTime") * 1000 };
                // highcharts treats the number as milliseconds - need to multiply by 1000 to convert seconds to milliseconds
                model.Series.Add(array);
            }

            return model;
        }

        List<RptSortiesOverTimeModel> IDAL_Rpt.GetSortiesOverTime(string ucid)
        {
            MySqlConnection conn = new MySqlConnection(_DBConnection);
            try
            {
                conn.Open();
                return ((IDAL_Rpt)this).GetSortiesOverTime(ucid, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<RptSortiesOverTimeModel> IDAL_Rpt.GetSortiesOverTime(string ucid, ref MySqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();

            List<RptSortiesOverTimeModel> model = new List<RptSortiesOverTimeModel>();
            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SP_SORTIES_OVER_TIME)
            {
                Connection = conn,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new MySqlParameter("UCID", ucid));
            MySqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);

            int CurrentIndex = -1;
            string CurrentAirframe = "";

            foreach (DataRow dr in dt.Rows)
            {
                string airframe = dr.Field<string>("Airframe");
                if (airframe != CurrentAirframe)
                {
                    model.Add(new RptSortiesOverTimeModel());
                    CurrentIndex++;
                    CurrentAirframe = airframe;
                    model[CurrentIndex].Airframe = airframe;

                    if (airframe == "TOTAL")
                        model[CurrentIndex].ChartType = "spline";
                }

                model[CurrentIndex].Data.Add(new RptSortiesOverTimePlotModel(dr));
            }

            return model;
        }

        List<RptScoreOverTimeModel> IDAL_Rpt.GetScoreOverTime(string ucid)
        {
            MySqlConnection conn = new MySqlConnection(_DBConnection);
            try
            {
                conn.Open();
                return ((IDAL_Rpt)this).GetScoreOverTime(ucid, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        List<RptScoreOverTimeModel> IDAL_Rpt.GetScoreOverTime(string ucid, ref MySqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();

            List<RptScoreOverTimeModel> model = new List<RptScoreOverTimeModel>();
            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SP_SCORE_OVER_TIME)
            {
                Connection = conn,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new MySqlParameter("UCID", ucid));
            MySqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);

            Dictionary<string, int> HashIndex = new Dictionary<string, int>();

            model.Add(new RptScoreOverTimeModel("Kills"));
            model.Add(new RptScoreOverTimeModel("Slingloads"));
            model.Add(new RptScoreOverTimeModel("Transport"));
            model.Add(new RptScoreOverTimeModel("Resupplies"));

            foreach (DataRow dr in dt.Rows)
            {
                model[0].Data.Add(new RptScoreOverTimePlotModel(dr, "Kills"));
                model[1].Data.Add(new RptScoreOverTimePlotModel(dr, "SlingLoads"));
                model[2].Data.Add(new RptScoreOverTimePlotModel(dr, "Transport"));
                model[3].Data.Add(new RptScoreOverTimePlotModel(dr, "Resupplies"));
            }

            return model;
        }

        RptPlayerBestSortieStatsModel IDAL_Rpt.GetBestSortieStats(string ucid)
        {
            MySqlConnection conn = new MySqlConnection(_DBConnection);
            try
            {
                conn.Open();
                return ((IDAL_Rpt)this).GetBestSortieStats(ucid, ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        RptPlayerBestSortieStatsModel IDAL_Rpt.GetBestSortieStats(string ucid, ref MySqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                conn.Open();

            RptPlayerBestSortieStatsModel model = null;

            MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(SP_BEST_SORTIE_STATS)
            {
                Connection = conn,
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add(new MySqlParameter("UCID", ucid));
            MySqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);

            foreach (DataRow dr in dt.Rows)
            {
                model = new RptPlayerBestSortieStatsModel(dr);
                break;
            }

            return model;
        }
    }
}