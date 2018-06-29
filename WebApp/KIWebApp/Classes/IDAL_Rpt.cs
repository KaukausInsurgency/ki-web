using KIWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIWebApp.Classes
{
    public interface IDAL_Rpt
    {
        RptPlayerOverallStatsModel GetOverallPlayerStats(string ucid);
        RptPlayerOverallStatsModel GetOverallPlayerStats(string ucid, ref MySql.Data.MySqlClient.MySqlConnection conn);

        List<RptTopAirframeSeriesModel> GetTopAirframeSeries(string ucid);
        List<RptTopAirframeSeriesModel> GetTopAirframeSeries(string ucid, ref MySql.Data.MySqlClient.MySqlConnection conn);

        List<RptAirframeOverallStatsModel> GetAirframeStats(string ucid);
        List<RptAirframeOverallStatsModel> GetAirframeStats(string ucid, ref MySql.Data.MySqlClient.MySqlConnection conn);

        List<RptSessionSeriesModel> GetLastSessionSeries(string ucid);
        List<RptSessionSeriesModel> GetLastSessionSeries(string ucid, ref MySql.Data.MySqlClient.MySqlConnection conn);

        List<RptSessionEventsDataModel> GetLastSetSessions(string ucid);
        List<RptSessionEventsDataModel> GetLastSetSessions(string ucid, ref MySql.Data.MySqlClient.MySqlConnection conn);

        RptPlayerOnlineActivityModel GetPlayerOnlineActivity(string ucid);
        RptPlayerOnlineActivityModel GetPlayerOnlineActivity(string ucid, ref MySql.Data.MySqlClient.MySqlConnection conn);

        List<RptSortiesOverTimeModel> GetSortiesOverTime(string ucid);
        List<RptSortiesOverTimeModel> GetSortiesOverTime(string ucid, ref MySql.Data.MySqlClient.MySqlConnection conn);

        List<RptScoreOverTimeModel> GetScoreOverTime(string ucid);
        List<RptScoreOverTimeModel> GetScoreOverTime(string ucid, ref MySql.Data.MySqlClient.MySqlConnection conn);
    }
}
