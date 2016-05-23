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
        /// 获取各模块使用交易量
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public List<Modules> GetStrategyType(GetDateTime dateTime)
        {
            return DataServiceHelper.GetStrategyType(dateTime);
        }

        /// <summary>
        /// 获取各模块开仓次数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public List<Modules> GetStrategyOpen(GetDateTime dateTime)
        {
            return DataServiceHelper.GetStrategyOpen(dateTime);
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

        /// <summary>
        /// 获取新增用户情况
        /// </summary>
        /// <param name="da">起止时间</param>
        /// <returns>新增机构用户数、非机构用户数、月份</returns>
        [HttpPost]
        public List<CustomerAmount> GetCreateCustomer(CustomerAmount da)
        {
            return DataServiceHelper.GetCreateCustomer(da);
        }

        /// <summary>
        /// 获取活跃用户情况
        /// </summary>
        /// <param name="da">起止时间</param>
        /// <returns>活跃机构用户数、非机构用户数、月份</returns>
        [HttpPost]
        public List<CustomerAmount> GetAliveCustomer(CustomerAmount da)
        {
            List<CustomerAmount> result = new List<CustomerAmount>();
            RecordResult<CustomerAmount> data = new RecordResult<CustomerAmount>();
            data = DataServiceHelper.GetAliveCustomer(da);
            result = data.List;
            result[0].TNumCustomer = data.TotalRecords;
            return result;
        }
    }
}
