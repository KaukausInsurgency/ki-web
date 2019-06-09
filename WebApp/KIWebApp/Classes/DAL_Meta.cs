using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIWebApp.Models;
using System.Data;
using MySql.Data.MySqlClient;

namespace KIWebApp.Classes
{
    public class DAL_Meta : IDAL_Meta
    {
        private const string SP_GET_VERSION_INFO = "websp_GetVersionInfo";

        public IAppSettings AppSettings { get; set; }

        public DAL_Meta()
        {
            AppSettings = new WebAppSettings();
        }

        // unused constructor
        public DAL_Meta(IAppSettings AppSettings)
        {
            this.AppSettings = AppSettings;
        }

        VersionInfoModel IDAL_Meta.GetVersionInfo()
        {
            IDbConnection conn = new MySqlConnection(AppSettings.MySqlConnectionString);
            try
            {
                conn.Open();
                return GetVersionInfo(ref conn);
            }
            finally
            {
                conn.Close();
            }
        }

        VersionInfoModel GetVersionInfo(ref IDbConnection dbconn)
        {
            if (dbconn.State == ConnectionState.Closed || dbconn.State == ConnectionState.Broken)
                dbconn.Open();

            IDbCommand cmd = SqlUtility.CreateCommand(dbconn, SP_GET_VERSION_INFO);
            DataTable dt = SqlUtility.Execute(cmd);

            if (dt == null)
                return null;

            VersionInfoModel model = null;
            foreach (DataRow dr in dt.Rows)
            {
                model = new VersionInfoModel()
                {
                    DCSClientGUID = dr.Field<string>("DCSClientGUID"),
                    DCSClientVersion = dr.Field<string>("DCSClientVersion"),
                    DCSModGUID = dr.Field<string>("DCSModGUID"),
                    DCSModVersion = dr.Field<string>("DCSModVersion")
                };
                break;
            }
            return model;
        }
    }
}