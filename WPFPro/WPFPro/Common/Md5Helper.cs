using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace WPFPro.Common
{
    public class Md5Helper
    {
        public static string MD5(string text)
        {
            string hashstring = string.Empty;
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] inBytes = Encoding.GetEncoding("GB2312").GetBytes(text);
            byte[] outBytes = md5.ComputeHash(inBytes);
            foreach (var item in outBytes)
            {
                hashstring += item.ToString("x2");
            }
            return hashstring;
        }
    }
}