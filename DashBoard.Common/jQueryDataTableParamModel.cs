using DashBoard.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    [Serializable]
    public class jQueryDataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
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
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }
        public string extra_search { get; set; }

        /// <summary>
        /// json数据：[{"name": "", "value": ""}]
        /// </summary>
        public string searchColumns { get; set; }
        public string columnIndex { get; set; }
    }
}
