using System.Collections;
using System.Data;
using System.IO;

namespace RM.Common.DotNetData
{
    public class XMLHelper
    {
        /// <summary>
        /// xml文件转为哈希表类型
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static Hashtable XMLToHashtable(string xmlData)
        {
            DataTable dt = XMLHelper.XMLToDataTable(xmlData);
            return DataTableHelper.DataTableToHashtable(dt);
        }

        /// <summary>
        /// xml文件转为datatable类型
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataTable XMLToDataTable(string xmlData)
        {
            DataTable result;
            if (!string.IsNullOrEmpty(xmlData))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(new StringReader(xmlData));
                if (ds.Tables.Count > 0)
                {
                    result = ds.Tables[0];
                    return result;
                }
            }
            result = null;
            return result;
        }

        /// <summary>
        /// xml格式转为数据集类型
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataSet XMLToDataSet(string xmlData)
        {
            DataSet result;
            if (!string.IsNullOrEmpty(xmlData))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(new StringReader(xmlData));
                result = ds;
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
}