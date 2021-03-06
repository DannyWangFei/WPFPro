﻿using System;
using System.Web.Mvc;
using System.Web.Security;
using WPFPro.Models;
using WPFPro.Util;
using WPFPro.BLL;
using WPFPro.Common;
using WPFPro.Interface;
using System.Configuration;

namespace WPFPro.Controllers
{
    public class AccountController : Controller
    {
        private BLLAccount _service;
        public AccountController(BLLAccount service)
        {
            this._service = service;
        }
        //protected static LogHelper Logger = new LogHelper("account");

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                string vcode = Session["VerifyPwdCode"].ToString();
                if (model.veryCode.ToLower() == vcode.ToLower())
                {
                    bool isTrue = _service.checkAccount(model.UserName, model.Password);//验证账户密码是否正确
                    if (isTrue)
                    {
                        string accName = model.UserName;
                        string loginIP = RequestHelper.GetIP();
                        DateTime logintime = DateTime.Now;
                        bool isRemember = model.RememberMe;
                        _service.SetAuthCookie(accName, loginIP, logintime, isRemember);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        //账户或者密码错误
                        ModelState.AddModelError("", "账户或者密码错误");
                    }
                }
                else
                {
                    //验证码错误
                    ModelState.AddModelError("", "验证码错误");
                }
            }
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid && Session["VerifyPwdCode"] != null)
            {
                string vcode = Session["VerifyPwdCode"].ToString();
                if (model.veryCode.Trim().ToLower() == vcode.ToLower())
                {

                    bool status = _service.validateAccount(model.Account);//验证该邮箱是否被注册过
                    if (!status)
                    {
                        var user = new User();
                        user.UserId = Guid.NewGuid();
                        user.Account = model.Account;
                        user.Password = Md5Helper.MD5(model.Password);
                        user.CreatedDate = DateTime.Now;
                        user.IsActived = false;
                        bool result = _service.CreateAccount(user);
                        if (result)
                        {
                            ViewBag.Success = true;
                            ViewBag.pageTitle = "注册成功！";
                            ViewBag.Content = "这是您的一天，非常欢迎您的到来！";
                            #region 添加成功后发送邮件激活账户
                            SendEmail(model.Account);
                            #endregion
                            return View("RegisterResult");
                        }
                        else
                        {
                            ViewBag.Success = false;
                            ViewBag.pageTitle = "注册失败！";
                            ViewBag.Content = "对于您注册失败非常抱歉，请重新注册谢谢！";
                            return View("RegisterResult");
                        }
                    }
                }
            }
            return View(model);
        }

        /// <summary>
        /// sendEmail to activie account
        /// </summary>
        /// <param name="Email"></param>
        private void SendEmail(string Email)
        {
            string Subject = "激活邮件";
            //string linkUrl = ConfigurationManager.AppSettings["Domain"].ToString() + string.Format("ActiveAccount/em={0}", Email);
            string body = string.Format("<div>请您激活您的账号，我们将对会对您提供全天的产品服务，谢谢您的支持！<br/><a href=\"ActiveAccount?email = {0}\"target=\"_blank\"style=\"text-decoration-style:none\">请点击该链接激活您的账户</a><br/>如无法进行激活请联系我们，电话:0311-88505015，QQ：2523754112</div>", Email);
            //string.Format()
            //SMTPManager.MailSending(Email, Subject, body, "");
        }

        /// <summary>
        /// 验证该邮箱是否被注册过
        /// </summary>
        /// <param name="accName"></param>
        /// <returns></returns>
        public ActionResult checkAjaxAccount(string accName)
        {

            bool status = _service.validateAccount(accName);//验证该邮箱是否被注册过
            if (status)
            { return Json(new { IsSuccess = true }); }
            else
            { return Json(new { IsSuccess = false }); }

        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.OldPassword == model.NewPassword)
                {
                    ModelState.AddModelError("", "原密码与新密码相同,请重新填写");
                }
                else if (model.ConfirmPassword != model.NewPassword)
                {
                    ModelState.AddModelError("", "确认密码与原密码不匹配");
                }
                else
                {
                    var account = HttpContext.User.Identity.Name;
                    var usermodel = _service.QueryUserInfo(account);
                    usermodel.Password = Md5Helper.MD5(model.NewPassword);
                    _service.ChangePassword(usermodel);
                }
            }
            // If we got this far, something failed, redisplay form
            //ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************
        [HttpPost]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetPassword()
        {
            //var email = Request.QueryString["email"].ToString();
            return View();
        }

        /// <summary>
        /// 忘记密码发送激活邮件。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgetPasswordNext(string email)
        {
            var subject = "账号激活邮件";
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("email", "邮箱不可为空");
            }
            if (ModelState.IsValid)
            {
                var domain = ConfigurationManager.AppSettings["Domain"].ToString().Trim();
                string body = string.Format("<!doctype html><html lang=\"en\"><head><meta charset=\"UTF-8\"></head><body><div>请您激活您的账号，我们将对会对您提供全天的产品服务，谢谢您的支持！<br/><a style=\"text-decoration-style:none\" href=\"{0}Account/ActiveAccount?email={1}\" target=\"_blank\"> 请点击该链接激活您的账户</a><br/>如无法进行激活请联系我们，电话: 0311 - 88505015，QQ：2523754112</div></body></html> ", domain, email);
                SMTPManager.MailSending(email, subject, body, "");
                return View("WaitingPage");
            }
            else
            {
                return View("ForgetPassword");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult ActiveAccount()
        {
            var email = string.Empty;
            if (Request["email"] != null)
            {
                email = Request.QueryString["email"].ToString();
            }
            else
            {
                throw new Exception("params email null");
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = _service.QueryUserInfo(email);
                if (user != null)
                {
                    user.IsActived = true;
                    _service.UpdateAccount(user);
                }
            }
            return View("LogOn");
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        public FileResult CaptchaVerifyCode()
        {
            ValidateCode Vcode = new ValidateCode();
            string letter = Vcode.GetRandomString(4);
            byte[] bytes = Vcode.CreateImage(letter);
            this.Session["VerifyPwdCode"] = letter;
            return File(bytes, @"image/jpeg");
        }
    }
}
