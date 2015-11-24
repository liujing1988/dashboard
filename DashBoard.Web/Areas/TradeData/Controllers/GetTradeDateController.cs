using Dashboard.Common;
using Dashboard.Logic;
using DashBoard.Common;
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
            return DataServiceHelper.GetMatchAmount(dateTime);
        }

        [HttpPost]
        public IEnumerable<TradeMonthAmount> GetDateAmount(GetDateTime dateTime)
        {
            return DataServiceHelper.GetDateAmount(dateTime);
        }

        [HttpPost]
        public List<RealTimeData> GetRealTimeData()
        {
            return DataServiceHelper.GetRealTimeData();
        }

        [HttpPost]
        public List<RealTimeData> GetRealData(RealTimeData da)
        {
            return DataServiceHelper.GetRealData(da);
        }
        [HttpPost]
        public List<TradeDayAmount> GetDayAmount()
        {
            return DataServiceHelper.GetDayAmount();
        }

        [HttpPost]
        public List<CreditTrade> GetCreditSalesAmount()
        {
            return DataServiceHelper.GetCreditSalesAmount();
        }

        [HttpPost]
        public List<CreditTrade> GetCreditBuyAmount()
        {
            return DataServiceHelper.GetCreditBuyAmount();
        }

        [HttpPost]
        public List<StrategyTypes> GetStrategyTradeAmt()
        {
            return DataServiceHelper.GetStrategyTradeAmt();
        }

        [HttpPost]
        public List<StrategyTypes> GetStrategyTradeAct()
        {
            return DataServiceHelper.GetStrategyTradeAct();
        }

        [HttpPost]
        public TradeDayVolume GetTradeDayVolume()
        {
            return DataServiceHelper.GetTradeDayVolume();
        }
    }
}
