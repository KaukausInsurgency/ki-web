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
    }
}