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
        [HttpPost]
        public List<StrategyTypes> GetStrategyType(GetDateTime dateTime)
        {
            return DataServiceHelper.GetStrategyType(dateTime);
        }


        [HttpPost]
        public List<CustomerAmount> GetCustomerAccount(CustomerAmount da)
        {
            return DataServiceHelper.GetCustomer(da);
        }

        [HttpPost]
        public CustomerOnline GetCustomerOnline()
        {
            return DataServiceHelper.GetCustomerOnline();
        }
    }
}
