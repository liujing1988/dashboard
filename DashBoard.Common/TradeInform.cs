using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    public class TradeInform<T>
    {
        public int RefreshRate { get; set; }
        public List<T> Data { get; set; }
    }
}
