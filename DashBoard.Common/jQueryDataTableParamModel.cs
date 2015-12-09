using DashBoard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    /// <summary>
    /// 用户交易明细表格参数传递实体
    /// 刘静
    /// 2015-12-02
    /// </summary>
    //序列化
    [Serializable]
    public class jQueryDataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// 模糊搜索内容
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// 每页显示几条数据
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// 本页第一条记录是总数的第几条
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// 总列数
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// 被排序列
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

        /// <summary>
        /// 时间参数
        /// </summary>
        public string extra_search { get; set; }

        /// <summary>
        /// 搜索内容（每列下方）
        /// </summary>
        public string searchColumns { get; set; }

        /// <summary>
        /// 搜索列名
        /// </summary>
        public string columnIndex { get; set; }
    }
}
