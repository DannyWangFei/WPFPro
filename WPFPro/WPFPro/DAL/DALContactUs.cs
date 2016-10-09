using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using RM.Common.DotNetCode;
using WPFPro.Models;

namespace WPFPro.DAL
{
    public class DALContactUs
    {
        SQLHelper helper = new SQLHelper();
        protected static LogHelper Logger = new LogHelper("DALContactUs");

        public bool InsertContactus(Contactus contact)
        {
            bool result = false;
            try
            {
                string sql = "INSERT INTO ContactUs(contactName,contactEmail,mobilePhone,note,createtime)VALUES(@contactName,@contactEmail,@mobilePhone,@note,@createtime)";
                SqlParameter[] param = new SqlParameter[]
                {                    
                    new SqlParameter("@contactName",SqlDbType.NVarChar),
                    new SqlParameter("@contactEmail",SqlDbType.NVarChar),
                    new SqlParameter("@mobilePhone",SqlDbType.NVarChar),
                    new SqlParameter("@note",SqlDbType.NVarChar),
                    new SqlParameter("@createtime",SqlDbType.DateTime)
                };                
                param[0].Value = contact.contactName;
                param[1].Value = contact.contactEmail;
                param[2].Value = contact.mobilePhone;
                param[3].Value = contact.note;
                param[4].Value = contact.createtime;
                helper.ExecuteNonQuery(sql, param);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                string message = string.Format("执行方法{0},出错时间{1},error{2}", "InsertContactus", DateTime.Now, ex.Message);
                Logger.WriteLog(message);
            }
            return result;
        }
    }
}