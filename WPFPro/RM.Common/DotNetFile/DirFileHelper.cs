using System;
using System.IO;
using System.Text;
using System.Web;

namespace RM.Common.DotNetFile
{
    public static class DirFileHelper
    {
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 判断路径下文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 获取制定路径下的文件名称
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static string[] GetFileNames(string directoryPath)
        {
            if (!DirFileHelper.IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 获取目录中子目录的名称以字符串数组格式返回
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static string[] GetDirectories(string directoryPath)
        {
            string[] directories;
            try
            {
                directories = Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return directories;
        }

        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            if (!DirFileHelper.IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            string[] files;
            try
            {
                if (isSearchChild)
                {
                    files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return files;
        }

        /// <summary>
        /// 判断是否为空目录
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string directoryPath)
        {
            bool result;
            try
            {
                string[] fileNames = DirFileHelper.GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    result = false;
                }
                else
                {
                    string[] directoryNames = DirFileHelper.GetDirectories(directoryPath);
                    if (directoryNames.Length > 0)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            catch
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static bool Contains(string directoryPath, string searchPattern)
        {
            bool result;
            try
            {
                string[] fileNames = DirFileHelper.GetFileNames(directoryPath, searchPattern, false);
                if (fileNames.Length == 0)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            bool result;
            try
            {
                string[] fileNames = DirFileHelper.GetFileNames(directoryPath, searchPattern, true);
                if (fileNames.Length == 0)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void CreateDir(string dir)
        {
            if (dir.Length != 0)
            {
                if (!Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                {
                    Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
                }
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteDir(string dir)
        {
            if (dir.Length != 0)
            {
                if (Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                {
                    Directory.Delete(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file"></param>
        public static void DeleteFile(string file)
        {
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + file))
            {
                File.Delete(HttpContext.Current.Request.PhysicalApplicationPath + file);
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="pagestr"></param>
        public static void CreateFile(string dir, string pagestr)
        {
            dir = dir.Replace("/", "\\");
            if (dir.IndexOf("\\") > -1)
            {
                DirFileHelper.CreateDir(dir.Substring(0, dir.LastIndexOf("\\")));
            }
            StreamWriter sw = new StreamWriter(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir, false, Encoding.GetEncoding("GB2312"));
            sw.Write(pagestr);
            sw.Close();
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="dir1"></param>
        /// <param name="dir2"></param>
        public static void MoveFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
            {
                File.Move(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1, HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2);
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="dir1"></param>
        /// <param name="dir2"></param>
        public static void CopyFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
            {
                File.Copy(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1, HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2, true);
            }
        }

        /// <summary>
        /// 获取日期格式文件夹名称
        /// </summary>
        /// <returns></returns>
        public static string GetDateDir()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 时间文件名称
        /// </summary>
        /// <returns></returns>
        public static string GetDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }

        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);
            if (Directory.Exists(varFromDirectory))
            {
                string[] directories = Directory.GetDirectories(varFromDirectory);
                if (directories.Length > 0)
                {
                    string[] array = directories;
                    for (int i = 0; i < array.Length; i++)
                    {
                        string d = array[i];
                        DirFileHelper.CopyFolder(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                    }
                }
                string[] files = Directory.GetFiles(varFromDirectory);
                if (files.Length > 0)
                {
                    string[] array = files;
                    for (int i = 0; i < array.Length; i++)
                    {
                        string s = array[i];
                        File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                    }
                }
            }
        }

        /// <summary>
        /// 判断制定路径下是否存在不存在创建
        /// </summary>
        /// <param name="FilePath"></param>
        public static void ExistsFile(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                FileStream fs = File.Create(FilePath);
                fs.Close();
            }
        }

        /// <summary>
        /// 删除路径下的文件夹以及文件
        /// </summary>
        /// <param name="varFromDirectory"></param>
        /// <param name="varToDirectory"></param>
        public static void DeleteFolderFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);
            if (Directory.Exists(varFromDirectory))
            {
                string[] directories = Directory.GetDirectories(varFromDirectory);
                if (directories.Length > 0)
                {
                    string[] array = directories;
                    for (int i = 0; i < array.Length; i++)
                    {
                        string d = array[i];
                        DirFileHelper.DeleteFolderFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                    }
                }
                string[] files = Directory.GetFiles(varFromDirectory);
                if (files.Length > 0)
                {
                    string[] array = files;
                    for (int i = 0; i < array.Length; i++)
                    {
                        string s = array[i];
                        File.Delete(varToDirectory + s.Substring(s.LastIndexOf("\\")));
                    }
                }
            }
        }

        public static string GetFileName(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void CreateDirectory(string directoryPath)
        {
            if (!DirFileHelper.IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void CreateFile(string filePath)
        {
            try
            {
                if (!DirFileHelper.IsExistFile(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    FileStream fs = file.Create();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                if (!DirFileHelper.IsExistFile(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    FileStream fs = file.Create();
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetLineCount(string filePath)
        {
            string[] rows = File.ReadAllLines(filePath);
            return rows.Length;
        }

        public static int GetFileSize(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return (int)fi.Length;
        }

        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            string[] directories;
            try
            {
                if (isSearchChild)
                {
                    directories = Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    directories = Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return directories;
        }

        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            File.WriteAllText(filePath, text, encoding);
        }

        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }

        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }

        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            string sourceFileName = DirFileHelper.GetFileName(sourceFilePath);
            if (DirFileHelper.IsExistDirectory(descDirectoryPath))
            {
                if (DirFileHelper.IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DirFileHelper.DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }

        public static string GetFileNameNoExtension(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.Name.Split(new char[]
			{
				'.'
			})[0];
        }

        public static string GetExtension(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.Extension;
        }

        public static void ClearDirectory(string directoryPath)
        {
            if (DirFileHelper.IsExistDirectory(directoryPath))
            {
                string[] fileNames = DirFileHelper.GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DirFileHelper.DeleteFile(fileNames[i]);
                }
                string[] directoryNames = DirFileHelper.GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DirFileHelper.DeleteDirectory(directoryNames[i]);
                }
            }
        }

        /// <summary>
        /// 清空文件夹下的文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void ClearFile(string filePath)
        {
            File.Delete(filePath);
            DirFileHelper.CreateFile(filePath);
        }

        /// <summary>
        /// 删除指定目录
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (DirFileHelper.IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
    }
}