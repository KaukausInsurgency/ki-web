using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAWKI_TCPServer
{
    public class SqlUtility
    {
        public const Int64 LUANULL = -9999;        // THIS IS IMPORTANT - CHANGING THIS WILL BREAK IF THE LUA NIL PLACEHOLDER IS NOT THE SAME!!!

        public static IDbCommand CreateCommand(IDbConnection connection, string action, Dictionary<string, object> dataDictionary)
        {
            IDbCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = action;
            foreach (var kv in dataDictionary)
            {
                var parameter = cmd.CreateParameter();
                parameter.ParameterName = kv.Key;

                if (kv.Value.GetType() == typeof(Int64) && (Int64)kv.Value == LUANULL)
                    parameter.Value = null;
                else
                    parameter.Value = kv.Value;

                cmd.Parameters.Add(parameter);
            }

            return cmd;
        }

        public static List<object> InvokeCommand(IDbCommand cmd, out string error)
        {
            List<object> results = null;
            error = "";

            using (IDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    results = new List<object>();
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        results.Add(rdr[i]);
                    }
                }
                else
                {
                    error = "No Results Returned\n";
                }
            }

            return results;
        }


    }
}
