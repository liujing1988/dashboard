using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoard.Web.Models
{
    /// <summary>
    /// 用户登录验证
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public SignService.ServiceResult Result { get; set; }

        /// <summary>
        /// 验证用户是否登录
        /// </summary>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //base.AuthorizeCore(httpContext);
            string sid = httpContext.Request["sessionid"];
            //WriteLog(sid);
            if (!string.IsNullOrEmpty(sid))
            {
                SignService.SignServiceSoapClient client = new SignService.SignServiceSoapClient();
                Result = client.CanAccessDashboard(sid);
                //WriteLog(Result.Code.ToString());
                if (Result.Code == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 未登录时事件处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //base.HandleUnauthorizedRequest(filterContext);
            string sid = filterContext.HttpContext.Request["sessionid"];
            string url = "http://" + HttpContext.Current.Request.Url.Host;
            string loginurl = url + ConfigurationManager.AppSettings.Get("LoginUrl");
            if (!string.IsNullOrEmpty(sid))
            {
                if (Result.Code == 2)
                {
                    filterContext.Result = new RedirectResult("~/Home/Error?msg=" + Result.Message);
                }
                else
                {
                    filterContext.Result = new RedirectResult(loginurl);
                }
            }
            else
            {
                filterContext.Result = new RedirectResult(loginurl);
            }
            
        }

        public void WriteLog(string s)
        {
            Console.WriteLine(s);
            FileStream aFile = new FileStream("D:\\temp\\lintest.dat", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(aFile);
            sw.Write(s);
            sw.Close();
        }
    }
}