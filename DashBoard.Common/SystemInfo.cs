using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    /// <summary>
    /// 服务器使用情况实体
    /// 刘静
    /// 2015-12-02
    /// </summary>
    public class SystemInfo
    {
        /// <summary>
        /// cpu使用率
        /// </summary>
        public string CpuUse { get; set; }

        /// <summary>
        /// 内存使用率
        /// </summary>
        public string MeUse { get; set; }

        /// <summary>
        /// 硬盘使用率
        /// </summary>
        public string DiskUse { get; set; }
    }
}
