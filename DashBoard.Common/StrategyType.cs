using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    /// <summary>
    /// 模块使用情况
    /// 刘静
    /// 2015-12-02
    /// </summary>
    public class Modules
    {
        /// <summary>
        /// 模块使用次数
        /// </summary>
        public int? NumModules { get; set; }

        /// <summary>
        /// 模块类型
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块交易金额
        /// </summary>
        public decimal? ModuleTradeAmt { get; set; }
    }
}
