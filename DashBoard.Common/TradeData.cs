using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    public class TradeData
    {
        public List<TradeMonthAmount> Obj { get; set; }
        public List<RealTimeData> Data { get; set; }
    }
}
