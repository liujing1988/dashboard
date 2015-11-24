using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    public class TradeDayAmount
    {
        public string tradeDate { get; set; }
        public decimal tradeAmount { get; set; }
        public decimal creditTrade { get; set; }
        public int custId { get; set; }
    }
}
