using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFPro.Interface;
using WPFPro.Models;

namespace WPFPro.Interface
{
    public interface IUserService
    {
        User GetUserByAccount(string account);
        User GetUserByAccount(string username, string password);
        bool checkAccount(string userName, string password);
        void CreateUser(User model);
        void UpdateUser(User usermodel);
    }
}
