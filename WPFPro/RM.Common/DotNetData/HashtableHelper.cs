using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RM.Common.DotNetData
{
    public class HashtableHelper
    {
        /// <summary>
        /// 哈希表数据生成xml文件
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static string HashtableToXml(Hashtable ht)
        {
            StringBuilder xml = new StringBuilder("<root>");
            xml.Append(HashtableHelper.HashtableToNode(ht));
            xml.Append("</root>");
            return xml.ToString();
        }

        /// <summary>
        /// 哈希表数据转为字符串类型类型输出<key>value</>
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        private static string HashtableToNode(Hashtable ht)
        {
            StringBuilder xml = new StringBuilder("");
            foreach (string key in ht.Keys)
            {
                object value = ht[key];
                xml.Append("<").Append(key).Append(">").Append(value).Append("</").Append(key).Append(">");
            }
            xml.Append("");
            return xml.ToString();
        }

        /// <summary>
        /// 泛型转换成string xml格式输出
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static string IListToXML(IList<Hashtable> datas)
        {
            StringBuilder xml = new StringBuilder("<root>");
            foreach (Hashtable ht in datas)
            {
                xml.Append(HashtableHelper.HashtableToNode(ht));
            }
            xml.Append("</root>");
            return xml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Hashtable GetModelToHashtable<T>(T model)
        {
            Hashtable ht = new Hashtable();
            PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] array = properties;
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo item = array[i];
                string key = item.Name;
                ht[key] = item.GetValue(model, null);
            }
            return ht;
        }
    }
}