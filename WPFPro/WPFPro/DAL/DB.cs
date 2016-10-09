using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WPFPro.DAL
{
    public class DB
    {
        public static readonly string ConnectionTemp = ConfigurationManager.ConnectionStrings["myConn"].ConnectionString;

        private SqlConnection CreateConnection()
        {
            SqlConnection dbconn = new SqlConnection();
            dbconn.ConnectionString = ConnectionTemp;
            return dbconn;
        }
        public SqlParameter CreateParameter(string name, SqlDbType dbType)
        {
            SqlParameter param = new SqlParameter(name, dbType);
            return param;
        }
        public SqlParameter CreateParameter(string name, DbType dbType)
        {
            SqlParameter param = new SqlParameter(name, dbType);
            return param;
        }

        public static bool Insert(object model,string procedureName)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(SQLHelper.ConnectionTemp);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 180;
            
            //foreach (object obj in model) 
            //{
                
            //}
            return true;
        }
    }
}