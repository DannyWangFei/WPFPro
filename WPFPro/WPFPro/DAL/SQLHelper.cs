using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.IO;

namespace WPFPro.DAL
{
    public class SQLHelper
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


        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());


        #region ExecuteNonQuery

        public virtual int ExecuteNonQuery(string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(SQLHelper.ConnectionTemp))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(cmdText, CommandType.Text, commandParameters);
        }

        public int ExecuteNonQuery(string cmdText)
        {
            return this.ExecuteNonQuery(cmdText, null);
        }

        #endregion

        #region ExecuteReader

        public virtual SqlDataReader ExecuteReader(string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(SQLHelper.ConnectionTemp);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] commandParameters)
        {
            return this.ExecuteReader(cmdText, CommandType.Text, commandParameters);
        }

        public SqlDataReader ExecuteReader(string cmdText)
        {
            return this.ExecuteReader(cmdText, null);
        }

        #endregion

        //**************************************

        #region ExecuteDataTable

        public virtual DataTable ExecuteDataTable(string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {

            using (SqlConnection connection = new SqlConnection(ConnectionTemp))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        throw ex;
                    }
                    return dt;
                }
            }
        }


        public DataTable ExecuteDataTable(string cmdText, params SqlParameter[] commandParameters)
        {
            return this.ExecuteDataTable(cmdText, CommandType.Text, commandParameters);
        }

        public DataTable ExecuteDataTable(string cmdText)
        {
            return this.ExecuteDataTable(cmdText, null);
        }
        public virtual DataSet ExecuteDataSet(string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {

            using (SqlConnection connection = new SqlConnection(ConnectionTemp))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet dt = new DataSet();
                    try
                    {
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        throw ex;
                    }
                    return dt;
                }
            }
        }


        #endregion

        //**************************************

        #region ExecuteScalar


        public virtual object ExecuteScalar(string cmdText, CommandType cmdType, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(ConnectionTemp))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }

        }
        public object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            return this.ExecuteScalar(cmdText, CommandType.Text, commandParameters);
        }
        public object ExecuteScalar(string cmdText)
        {
            return this.ExecuteScalar(cmdText, null);
        }


        #endregion

        //**************************************

        #region Parameters Cache


        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        protected void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;
            cmd.CommandTimeout = 180;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }



        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        public bool SqlBulkCopyData(DataTable dt, string tableName)
        {
            try
            {
                SqlBulkCopy bcp = new SqlBulkCopy(SQLHelper.ConnectionTemp);
                bcp.DestinationTableName = tableName;
                if (dt.Rows.Count > 0)
                {
                    bcp.WriteToServer(dt);
                }
                bcp.Close();
                return true;
            }
            catch (Exception ex)
            {
                //LogHelper.Tracke.Error("Insert to " + tableName, ex);
                //WriteErrorToXml(dt, tableName);
                return false;
            }
        }
        /// <summary>
        /// 基本表数据专用，里面tablename名称增加了前缀Sync,增加字段
        /// </summary>
        /// <param name="ds">数据集dataset</param>
        /// <param name="DataSetName">数据集名称</param>
        /// <returns></returns>
        public bool GetConfigDataBySqlBulkCopy(DataSet ds, out DateTime SyncDate)
        {
            SyncDate = DateTime.Now;
            using (SqlConnection conn = new SqlConnection(SQLHelper.ConnectionTemp))
            {
                conn.Open();
                using (SqlTransaction sqlTrans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlBulkCopy bcp = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, sqlTrans);
                        bcp.BatchSize = 1000;
                        foreach (DataTable dt in ds.Tables)
                        {
                            bcp.DestinationTableName = dt.TableName;
                            //LogHelper.Tracke.DebugFormat("芒果Pro整合Windows Service-基本表数据{0}写入中转表,共{1}行", bcp.DestinationTableName, dt.Rows.Count);
                            //if (dt.TableName.ToLower() == "app")
                            //{
                            //    dt.Columns.Remove("RTB_WinPrice");
                            //    dt.Columns.Remove("TableFloorPrice");
                            //}
                            if (dt.Rows.Count > 0)
                            {
                                DataColumn col = new DataColumn("SyncDate", typeof(DateTime));
                                col.DefaultValue = SyncDate;
                                dt.Columns.Add(col);
                                bcp.WriteToServer(dt);
                            }
                            //LogHelper.Tracke.DebugFormat("复制“" + bcp.DestinationTableName + "”结束");
                        }
                        sqlTrans.Commit();
                        bcp.Close();
                        conn.Close();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (sqlTrans != null && conn.State == ConnectionState.Open)
                        {
                            sqlTrans.Rollback();
                            conn.Close();
                        }
                        //LogHelper.Tracke.Error("芒果Pro整合Windows Service-基本表数据写入中转表失败，全部回滚。 错误描述：", ex);
                        ////发送邮件
                        //throw new Exception("芒果Pro整合Windows Service-基本表数据写入中转表失败，全部回滚。 错误描述：" + ex.ToString());
                        return false;

                    }
                }
            }
        }
        #endregion

        //**************************************
        #region SqlDataReader to IList<T>

        public static IList<T> SqlDataReaderToIList<T>(SqlDataReader sqldatareader)
        {
            try
            {
                IList<T> list = new List<T>();
                while (sqldatareader.Read())
                {
                    T t = System.Activator.CreateInstance<T>();
                    Type type = t.GetType();
                    for (int i = 0; i < sqldatareader.FieldCount; i++)
                    {
                        object TempValue = null;
                        if (sqldatareader.IsDBNull(i))
                        {
                            string typeFullName = type.GetProperty(sqldatareader.GetName(i)).PropertyType.FullName;
                            TempValue = GetDbNullValue(typeFullName);
                        }
                        else
                        {
                            TempValue = sqldatareader.GetValue(i);
                        }
                        type.GetProperty(sqldatareader.GetName(i)).SetValue(t, TempValue, null);
                    }
                    list.Add(t);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                sqldatareader.Close();
                sqldatareader.Dispose();
            }


        }

        private static object GetDbNullValue(string typeFullName)
        {
            typeFullName = typeFullName.ToLower();
            if (typeFullName == "string")
                return string.Empty;
            else if (typeFullName == "int32" || typeFullName == "int16" || typeFullName == "int64")
                return 0;
            else if (typeFullName == "datetime")
                return Convert.ToDateTime(DateTime.MinValue);
            else if (typeFullName == "boolean")
                return false;
            else if (typeFullName == "int")
                return 0;
            return null;
        }

        #endregion
    }
}