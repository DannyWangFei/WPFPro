using System;
using System.Web;
using WPFPro.DAL;
using WPFPro.Models;
using System.Web.Security;
using WPFPro.Interface;
using System.Data.SqlClient;

namespace WPFPro.BLL
{
    public class BLLAccount
    {
        public bool Status { get; set; }
        private IUserService _Iuser;
        public BLLAccount(IUserService Iuser)
        {
            this._Iuser = Iuser;
        }

        /// <summary>
        /// 验证注册账号是否有注册过
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool validateAccount(string account)
        {
            if (!string.IsNullOrEmpty(account.Trim()))
            {
                var model = new User();
                try
                {
                    model = _Iuser.GetUserByAccount(account.Trim());
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                if (model == null)
                    Status = false;
                else
                    Status = true;
                return Status;
            }
            else { return false; }
        }

        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CreateAccount(User model)
        {
            try
            {
                _Iuser.CreateUser(model);
                return true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证账号密码是否正确
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool checkAccount(string username, string password)
        {
            try
            {
                var model = _Iuser.GetUserByAccount(username, password);
                return true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存账号信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="loginIP"></param>
        /// <param name="logintime"></param>
        public void SetAuthCookie(string userName, string loginIP, DateTime logintime, bool isRemember)
        {
            DateTime expiration = new DateTime();
            var userdata = string.Empty;
            var userId = Guid.Empty;
            //string userNickName = "游客app";
            var usermodel = _Iuser.GetUserByAccount(userName);
            if (usermodel != null)
            {
                userId = usermodel.UserId;
            }
            userdata = string.Format("{0},{1},{2},{3}", userId, userName, logintime, loginIP);
            expiration = DateTime.Now + FormsAuthentication.Timeout;
            if (isRemember)
            {
                expiration = DateTime.Now.AddDays(7);
            }
            FormsAuthenticationTicket tickets = new FormsAuthenticationTicket(1, userName, logintime, expiration, isRemember, userdata, FormsAuthentication.FormsCookiePath);
            string strticket = FormsAuthentication.Encrypt(tickets);
            HttpCookie cookies = new HttpCookie(FormsAuthentication.FormsCookieName, strticket);
            cookies.Expires = tickets.Expiration;
            HttpContext.Current.Response.Cookies.Add(cookies);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="usermodel"></param>
        public void ChangePassword(User usermodel)
        {
            try
            {
                _Iuser.UpdateUser(usermodel);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据账户查询用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public User QueryUserInfo(string account)
        {
            try
            {
                return _Iuser.GetUserByAccount(account);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新账户信息
        /// </summary>
        /// <param name="user"></param>
        public void UpdateAccount(User user)
        {
            try
            {
                _Iuser.UpdateUser(user);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}