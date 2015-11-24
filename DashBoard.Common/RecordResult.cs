using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    public class RecordResult<T>
    {
        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalDisplayRecords { get; set; }

        public List<T> List { get; set; }
    }
}
