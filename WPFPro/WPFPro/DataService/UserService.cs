using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPFPro.Common;
using WPFPro.DAL;
using WPFPro.Interface;
using WPFPro.Models;

namespace WPFPro.DataService
{
    public class UserService : IUserService
    {
        public bool checkAccount(string userName, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void CreateUser(User model)
        {
            using (var db = new DALAccountContext())
            {
                db.User.Add(model);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 根据邮箱查询user
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public User GetUserByAccount(string account)
        {
            using (var db = new DALAccountContext())
            {
                var model = db.User.Where(x => x.Account == account).FirstOrDefault();
                return model;
            }
        }

        /// <summary>
        /// 根据账户密码查询用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User GetUserByAccount(string account, string password)
        {
            using (var db = new DALAccountContext())
            {
                password = Md5Helper.MD5(password);
                var model = db.User.Where(x => x.Account == account && x.Password == password).FirstOrDefault();
                return model;
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="usermodel"></param>
        public void UpdateUser(User usermodel)
        {
            using (var db = new DALAccountContext())
            {
                db.User.Add(usermodel);
                db.SaveChanges();
            }
        }
    }
}