using Dashboard.Common;
using Dashboard.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DashBoard.Web.Areas.TradeData.Controllers
{
    public class GetTradeDateController : ApiController
    {
        [HttpPost]
        public IEnumerable<TradeMonthAmount> GetMatchAmount(GetDateTime dateTime)
        {
            return DemoService.GetMatchAmount(dateTime);
        }

        [HttpPost]
        public IEnumerable<TradeMonthAmount> GetDateAmount(GetDateTime dateTime)
        {
            return DemoService.GetDateAmount(dateTime);
        }

        [HttpPost]
        public List<RealTimeData> GetRealTimeData()
        {
            return DemoService.GetRealTimeData();
        }
    }
}
