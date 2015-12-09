using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    /// <summary>
    /// 在线用户数
    /// 刘静
    /// 2015-12-02
    /// </summary>
    public class CustomerOnline
    {
        /// <summary>
        /// 在线总用户数
        /// </summary>
        public int NumCustomerOnline { get; set; }

        /// <summary>
        /// 信用用户数
        /// </summary>
        public int CreditCustomer { get; set; }

        /// <summary>
        /// 服务器时间
        /// </summary>
        public string ServerDate { get; set; }
    }
}
