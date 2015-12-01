using DashBoard.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;

namespace DashBoard.Web.Areas.HealthIndex.Models
{
    /// <summary>
    /// 平台健康指数阈值设置配置文件
    /// </summary>
    public class IndexManagers
    {
        /// <summary>
        /// 读取配置文件
        /// </summary>
        public static HealthManagers ReadConfig()
        {
            HealthManagers result = new HealthManagers();
            result.MaxDayOrder = ConfigurationManager.AppSettings.Get("MaxDay");
            result.MaxMinuteOrder = ConfigurationManager.AppSettings.Get("MaxMinute");
            result.MaxSecondOrder = ConfigurationManager.AppSettings.Get("MaxSecond");
            return result;
        }

        /// <summary>
        /// 修改，保存配置文件
        /// </summary>
        /// <param name="ConnenctionString"></param>
        public static void SaveConfig(string MaxDay, string MaxMinute, string MaxSecond)
        {
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            config.AppSettings.Settings["MaxDay"].Value = MaxDay;
            config.AppSettings.Settings["MaxMinute"].Value = MaxMinute;
            config.AppSettings.Settings["MaxSecond"].Value = MaxSecond;
            config.Save();
        }

    }

}