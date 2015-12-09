using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    /// <summary>
    /// 客户交易明细调取数据库返回参数实体
    /// 刘静
    /// 2015-12-02
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RecordResult<T>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总显示记录数
        /// </summary>
        public int TotalDisplayRecords { get; set; }

        /// <summary>
        /// 客户交易明细数据集
        /// </summary>
        public List<T> List { get; set; }
    }
}
