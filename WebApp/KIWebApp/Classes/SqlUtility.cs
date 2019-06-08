using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KIWebApp.Classes
{
    public class SqlUtility
    {
        public static IDbCommand CreateCommand(IDbConnection conn, string sp, Dictionary<string,object> parameters)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sp;
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (var pair in parameters)
            {
                IDbDataParameter param = cmd.CreateParameter();
                param.ParameterName = pair.Key;
                param.Value = pair.Value;
                cmd.Parameters.Add(param);
            }        

            return cmd;
        }

        public static DataTable Execute(IDbCommand cmd)
        {
            try
            {
                IDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ConvertTimeTicksToStringInt(ref DataRow dr, string field)
        {
            TimeSpan ts;
            
            if (dr[field] == DBNull.Value || dr[field] == null)
                ts = new TimeSpan(0, 0, 0);
            else
                ts = new TimeSpan(TimeSpan.TicksPerSecond * dr.Field<int>(field));

            return ((int)(ts.TotalHours)).ToString("D2") + ":" + ts.ToString(@"mm\:ss");
        }


        // In the future these will be condensed down to a single function once we get off MySql
        public static string ConvertTimeTicksToStringLong(ref DataRow dr, string field)
        {
            TimeSpan ts;

            if (dr[field] == DBNull.Value || dr[field] == null)
                ts = new TimeSpan(0, 0, 0);
            else
                ts = new TimeSpan(TimeSpan.TicksPerSecond * dr.Field<long>(field));

            return ((long)(ts.TotalHours)).ToString("D2") + ":" + ts.ToString(@"mm\:ss");
        }

        public static string ConvertTimeTicksToStringDouble(ref DataRow dr, string field)
        {
            TimeSpan ts;

            if (dr[field] == DBNull.Value || dr[field] == null)
                ts = new TimeSpan(0, 0, 0);
            else
                ts = new TimeSpan(TimeSpan.TicksPerSecond * Convert.ToInt32(dr.Field<double>(field)));

            return ((long)(ts.TotalHours)).ToString("D2") + ":" + ts.ToString(@"mm\:ss");
        }

        public static T GetValueOrDefault<T>(DataRow dr, string field, T defaultValue)
        {
            if (dr[field] == DBNull.Value || dr[field] == null)
                return defaultValue;
            else
                return dr.Field<T>(field);
        }
    }
}