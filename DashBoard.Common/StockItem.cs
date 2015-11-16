using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dashboard.Common
{
    [Serializable]
    public class StockItem
    {
        /// <summary>
        /// 市场代码
        /// </summary>
        public string Market { get; set; }

        /// <summary>
        /// 市场代码(数字)
        /// </summary>
        public string MarketID { get; set; }

        /// <summary>
        /// 证券代码
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 证券名称
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string SearchText { get; set; }
    }
}
