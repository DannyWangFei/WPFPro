using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPFPro.DAL;
using WPFPro.Models;
using System.Reflection;
using System.Data;
using System.Web.Security;
using RM.Common.DotNetCode;

namespace WPFPro.BLL
{
    public class BLLAccount
    {
        public bool Status { get; set; }

        DALAccount dalaccount = new DALAccount();
        protected static LogHelper Logger = new LogHelper("BLLAccount");

        /// <summary>
        /// 验证注册账号是否有注册过
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool validateAccount(string username)
        {
            if (!string.IsNullOrEmpty(username.Trim()))
            {
                bool result = dalaccount.checkAccount(username.Trim());
                Status = result;
                return Status;
            }
            else { return false; }
        }

        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CreateAccount(RegisterModel model)
        {
            bool result = dalaccount.DalCreateAccount(model);
            Status = result;
            return Status;
        }

        /// <summary>
        /// 验证账号密码是否正确
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool checkAccount(string username, string password)
        {
            bool result = dalaccount.DalCheckAccount(username, password);
            Status = result;
            return Status;
        }

        /// <summary>
        /// 保存账号信息
        /// </summary>
        /// <param name="accName"></param>
        /// <param name="loginIP"></param>
        /// <param name="logintime"></param>
        public void SetAuthCookie(string accName, string loginIP, DateTime logintime, bool isRemember)
        {
            DateTime expiration = new DateTime();
            string data = string.Format("{0},{1},{2}", accName, logintime, loginIP);
            expiration = DateTime.Now + FormsAuthentication.Timeout;
            if (isRemember)
            {
                expiration = DateTime.Now.AddDays(7);
            }
            FormsAuthenticationTicket tickets = new FormsAuthenticationTicket(1, accName, logintime, expiration, isRemember, data);
            string strticket = FormsAuthentication.Encrypt(tickets);
            HttpCookie cookies = new HttpCookie(FormsAuthentication.FormsCookieName, strticket);
            cookies.Expires = tickets.Expiration;
            HttpContext.Current.Response.Cookies.Add(cookies);
        }
    }
}