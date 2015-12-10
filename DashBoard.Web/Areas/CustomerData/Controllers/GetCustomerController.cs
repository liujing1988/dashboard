using Dashboard.Common;
using Dashboard.Logic;
using DashBoard.Common;
using DashBoard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DashBoard.Web.Areas.CustomerData.Controllers
{
    public class GetCustomerController : ApiController
    {
        /// <summary>
        /// 获取各模块使用情况
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public List<StrategyTypes> GetStrategyType(GetDateTime dateTime)
        {
            return DataServiceHelper.GetStrategyType(dateTime);
        }

        /// <summary>
        /// 获取新增用户数
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CustomerAmount> GetCustomerAccount(CustomerAmount da)
        {
            return DataServiceHelper.GetCustomer(da);
        }

        /// <summary>
        /// 获取在线用户情况
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public CustomerOnline GetCustomerOnline()
        {
            return DataServiceHelper.GetCustomerOnline();
        }

        ///// <summary>
        ///// 获取撤单/委托比大于60%的客户id和比值
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public List<TradeDayVolume> GetRevoke(GetDateTime da)
        //{
        //    da.LimitMinOrder = HealthIndex.Models.IndexManagers.ReadConfig().LimitMinOrder;
        //    return DataServiceHelper.GetRevoke(da);
        //}
    }
}
