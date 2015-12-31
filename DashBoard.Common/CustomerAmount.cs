using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Common
{
    /// <summary>
    /// 用户增长情况实体
    /// 刘静
    /// 2015-12-02
    /// </summary>
    public class CustomerAmount
    {
        /// <summary>
        /// 活跃用户
        /// </summary>
        public int AliveCustomer { get; set; }

        /// <summary>
        /// 活跃机构用户
        /// </summary>
        public int AliveOrgCustomer { get; set; }

        /// <summary>
        /// 活跃个人用户
        /// </summary>
        public int AlivePersonCustomer { get; set; }

        /// <summary>
        /// 本月总用户数
        /// </summary>
        public string AllCustomerMonth { get; set; }

        /// <summary>
        /// 总用户数
        /// </summary>
        public int TNumCustomer { get; set; }

        /// <summary>
        /// 起始月份
        /// </summary>
        public string BeginMonth { get; set; }

        /// <summary>
        /// 截止月份
        /// </summary>
        public string EndMonth { get; set; }

        /// <summary>
        /// 新增用户数
        /// </summary>
        public int CreatCustomer { get; set; }

        /// <summary>
        /// 新增机构用户数
        /// </summary>
        public int CreatOrgCustomer { get; set; }

        /// <summary>
        /// 新增个人用户数
        /// </summary>
        public int CreatPersonCustomer { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }
    }
}
