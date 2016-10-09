using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RM.Common.DotNetEamil
{
    public class SMTPManager
    {
        public static string MailSending(string Dep_Email, string Mis_Name, string Mis_Describe, string File_Path)
        {
            string MailUser = ConfigurationManager.AppSettings["MailUser"].ToString();
            string MailName = ConfigurationManager.AppSettings["MailName"].ToString();
            string MailHost = ConfigurationManager.AppSettings["MailHost"].ToString();
            string MailPwd = ConfigurationManager.AppSettings["MailPwd"].ToString();
            MailAddress from = new MailAddress(MailUser, MailName);
            MailMessage mail = new MailMessage();
            mail.Subject = Mis_Name;
            mail.From = from;
            string[] mailNames = (Dep_Email + ";").Split(new char[]
			{
				';'
			});
            string[] array = mailNames;
            for (int i = 0; i < array.Length; i++)
            {
                string name = array[i];
                if (name != string.Empty)
                {
                    string displayName;
                    string address;
                    if (name.IndexOf('<') > 0)
                    {
                        displayName = name.Substring(0, name.IndexOf('<'));
                        address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                    }
                    else
                    {
                        displayName = string.Empty;
                        address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                    }
                    mail.To.Add(new MailAddress(address, displayName));//添加收件人
                }
            }
            mail.Body = Mis_Describe;
            mail.BodyEncoding = Encoding.UTF8;//编码格式
            mail.IsBodyHtml = true;//邮件html格式
            mail.Priority = MailPriority.Normal;//邮件优先级
            if (File_Path != "")
            {
                mail.Attachments.Add(new Attachment(File_Path));//发送邮件中添加附件
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;//获取发送通知成功状态
            }
            SmtpClient client = new SmtpClient();
            client.Host = MailHost;
            client.Port = 25;//端口
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(MailUser, MailPwd);//邮件账号密码
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;//传送是否失败
            client.Send(mail);//发送邮件
            return mail.ToString();
        }
    }
}