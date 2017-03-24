using System;
using System.Data.SqlClient;
using System.Data;
using WPFPro.Models;
using WPFPro.Common;
using System.Data.Entity;
using WPFPro.Interface;

namespace WPFPro.DAL
{
    public class DALAccountContext : BaseContext
    {
        SQLHelper helper = new SQLHelper();
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("CAIDI_User");
        }

        public bool checkAccount(string username)
        {
            bool result = false;
            //try
            //{
            //    if (username != "" && !string.IsNullOrWhiteSpace(username))
            //    {
            //        SqlParameter[] para = new SqlParameter[]
            //    {
            //        new SqlParameter("@accName",SqlDbType.NVarChar),
            //        new SqlParameter("@result",SqlDbType.Bit)
            //    };
            //        para[0].Value = username;
            //        para[1].Direction = ParameterDirection.Output;
            //        helper.ExecuteNonQuery("wpf_registerAccount", CommandType.StoredProcedure, para);
            //        result = Convert.ToBoolean(para[1].Value);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string message = string.Format("执行方法{0},出错时间{1},error{2}", "checkAccount", DateTime.Now, ex.Message);
            //    //Logger.WriteLog(ex.Message);
            //}
            return result;
        }
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
                para[0].Value = model.Account;
                para[1].Value = Md5Helper.MD5(model.Password);
                para[2].Value = 1;
                para[3].Value = DateTime.Now;
                helper.ExecuteNonQuery(sql, para);
                result = true;
            }
            catch (Exception ex)
            {
                string message = string.Format("执行方法{0},出错时间{1},error{2}", "DalCreateAccount", DateTime.Now, ex.Message);
                //Logger.WriteLog(ex.Message);
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
                param[1].Value = Md5Helper.MD5(password);
                param[2].Direction = ParameterDirection.Output;
                helper.ExecuteNonQuery("wpf_CheckAccount", CommandType.StoredProcedure, param);
                result = Convert.ToBoolean(param[2].Value);
            }
            catch (Exception ex)
            {
                string message = string.Format("执行方法{0},出错时间{1},error{2}", "DalCheckAccount", DateTime.Now, ex.Message);
                //Logger.WriteLog(ex.Message);
            }
            return result;
        }
    }
}