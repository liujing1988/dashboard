using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    public class TradeMonthAmount
    {
        public string Month { get; set; }

        public decimal? MatchAmount { get; set; }
        public string Date { get; set; }
        public decimal? DateAmount { get; set; }



    }
}
