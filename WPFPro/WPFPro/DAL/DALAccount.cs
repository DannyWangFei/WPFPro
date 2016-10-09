using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using WPFPro.Models;
using WPFPro.Common;
using RM.Common.DotNetEncrypt;
using RM.Common.DotNetCode;

namespace WPFPro.DAL
{
    public class DALAccount
    {
        SQLHelper helper = new SQLHelper();
        protected static LogHelper Logger = new LogHelper("DALaccount");
        
        /// <summary>
        /// 验证用户是否被注册过
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool checkAccount(string username)
        {
            bool result = false;
            try
            {
                if (username != "" && !string.IsNullOrWhiteSpace(username))
                {
                    SqlParameter[] para = new SqlParameter[] 
                {
                    new SqlParameter("@accName",SqlDbType.NVarChar),
                    new SqlParameter("@result",SqlDbType.Bit)
                };
                    para[0].Value = username;
                    para[1].Direction = ParameterDirection.Output;
                    helper.ExecuteNonQuery("wpf_registerAccount", CommandType.StoredProcedure, para);
                    result = Convert.ToBoolean(para[1].Value);
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("执行方法{0},出错时间{1},error{2}", "checkAccount", DateTime.Now, ex.Message);
                Logger.WriteLog(ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 注册账户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DalCreateAccount(RegisterModel model)
        {
            bool result = false;
            try
            {
                string sql = @"INSERT INTO account (accName,password,status,createTime) VALUES (@accName,@password,@status,@createTime)";
                SqlParameter[] para = new SqlParameter[] 
                {
                    new SqlParameter("@accName",SqlDbType.NVarChar),
                    new SqlParameter("@password",SqlDbType.NVarChar),
                    new SqlParameter("@status",SqlDbType.NVarChar),
                    new SqlParameter("@createTime",SqlDbType.DateTime)
                };
                para[0].Value = model.UserName;
                para[1].Value = Md5Helper.MD5(model.Password, 32);
                para[2].Value = 1;
                para[3].Value = DateTime.Now;
                helper.ExecuteNonQuery(sql, para);
                result = true;
            }
            catch (Exception ex)
            {
                string message = string.Format("执行方法{0},出错时间{1},error{2}", "DalCreateAccount", DateTime.Now, ex.Message);
                Logger.WriteLog(ex.Message);
            }
            return result;
        }

        public bool DalCheckAccount(string username, string password)
        {
            bool result = false;
            try
            {
                SqlParameter[] param = new SqlParameter[] 
                {
                    new SqlParameter("@username",SqlDbType.NVarChar,50),
                    new SqlParameter("@password",SqlDbType.NVarChar,100),
                    new SqlParameter("@result",SqlDbType.Bit)
                };
                param[0].Value = username;
                param[1].Value = Md5Helper.MD5(password, 32);
                param[2].Direction = ParameterDirection.Output;
                helper.ExecuteNonQuery("wpf_CheckAccount", CommandType.StoredProcedure, param);
                result = Convert.ToBoolean(param[2].Value);
            }
            catch (Exception ex)
            {
                string message = string.Format("执行方法{0},出错时间{1},error{2}", "DalCheckAccount", DateTime.Now, ex.Message);
                Logger.WriteLog(ex.Message);
            }
            return result;
        }
    }
}