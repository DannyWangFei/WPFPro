using System;
using System.IO;
using System.Web.UI.WebControls;

namespace RM.Common.DotNetFile
{
    public class UploadHelper
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filleupload"></param>
        /// <returns></returns>
        public static string FileUpload(string path, FileUpload filleupload)
        {
            string result;
            try
            {
                bool fileOk = false;
                string fileExtension = Path.GetExtension(filleupload.FileName).ToLower();//得到文件后缀名
                string[] allowExtension = new string[]
				{
					".rar",
					".zip",
					".rar",
					".ios",
					".jpg",
					".png",
					"bmp",
					".gif",
					".txt"
				};
                if (filleupload.HasFile)
                {
                    for (int i = 0; i < allowExtension.Length; i++)
                    {
                        if (fileExtension == allowExtension[i])//后缀名格式验证
                        {
                            fileOk = true;
                            break;
                        }
                    }
                }
                if (fileOk)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (!FileHelper.IsExistFile(path + filleupload.FileName))
                    {
                        int Size = filleupload.PostedFile.ContentLength / 1024 / 1024;//上传文件大小
                        if (Size > 10)
                        {
                            result = "上传失败,文件过大";
                        }
                        else
                        {
                            filleupload.PostedFile.SaveAs(path + filleupload.FileName);//保存在相应的目录下
                            result = "上传成功";
                        }
                    }
                    else
                    {
                        result = "上传失败,文件已存在";
                    }
                }
                else
                {
                    result = "不支持【" + fileExtension + "】文件格式";
                }
            }
            catch (Exception)
            {
                result = "上传失败";
            }
            return result;
        }
    }
}