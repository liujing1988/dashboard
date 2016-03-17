﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DashBoard.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ModelDataContainer : DbContext
    {
        public ModelDataContainer()
            : base("name=ModelDataContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<strategyorder> strategyorder { get; set; }
        public virtual DbSet<strategytrade> strategytrade { get; set; }
        public virtual DbSet<customer> customer { get; set; }
        public virtual DbSet<strategyinfo> strategyinfo { get; set; }
        public virtual DbSet<custacctinfo> custacctinfo { get; set; }
        public virtual DbSet<positionbasicinfotable> positionbasicinfotable { get; set; }
        public virtual DbSet<strategykind> strategykind { get; set; }
        public virtual DbSet<generalparam> generalparam { get; set; }
        public virtual DbSet<menu> menu { get; set; }
    
        public virtual ObjectResult<GetCustomer> sp_GetCustomer(Nullable<int> begindate, Nullable<int> enddate)
        {
            var begindateParameter = begindate.HasValue ?
                new ObjectParameter("Begindate", begindate) :
                new ObjectParameter("Begindate", typeof(int));
    
            var enddateParameter = enddate.HasValue ?
                new ObjectParameter("Enddate", enddate) :
                new ObjectParameter("Enddate", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCustomer>("sp_GetCustomer", begindateParameter, enddateParameter);
        }
    
        public virtual ObjectResult<TradeAmt_Minute> sp_TradeAmt_Minute(Nullable<int> date)
        {
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<TradeAmt_Minute>("sp_TradeAmt_Minute", dateParameter);
        }
    
        public virtual ObjectResult<sp_GetRevokePercent_Result> sp_GetRevokePercent(Nullable<int> beginDate, Nullable<int> endDate, string searchColumns, Nullable<int> displayStart, Nullable<int> displayLength, string sortDirection, Nullable<int> currentPage, string orderField, Nullable<int> limitNumOrder, ObjectParameter pageCount, ObjectParameter totalRecords, ObjectParameter totalDisplayRecords)
        {
            var beginDateParameter = beginDate.HasValue ?
                new ObjectParameter("BeginDate", beginDate) :
                new ObjectParameter("BeginDate", typeof(int));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(int));
    
            var searchColumnsParameter = searchColumns != null ?
                new ObjectParameter("SearchColumns", searchColumns) :
                new ObjectParameter("SearchColumns", typeof(string));
    
            var displayStartParameter = displayStart.HasValue ?
                new ObjectParameter("DisplayStart", displayStart) :
                new ObjectParameter("DisplayStart", typeof(int));
    
            var displayLengthParameter = displayLength.HasValue ?
                new ObjectParameter("DisplayLength", displayLength) :
                new ObjectParameter("DisplayLength", typeof(int));
    
            var sortDirectionParameter = sortDirection != null ?
                new ObjectParameter("SortDirection", sortDirection) :
                new ObjectParameter("SortDirection", typeof(string));
    
            var currentPageParameter = currentPage.HasValue ?
                new ObjectParameter("CurrentPage", currentPage) :
                new ObjectParameter("CurrentPage", typeof(int));
    
            var orderFieldParameter = orderField != null ?
                new ObjectParameter("OrderField", orderField) :
                new ObjectParameter("OrderField", typeof(string));
    
            var limitNumOrderParameter = limitNumOrder.HasValue ?
                new ObjectParameter("LimitNumOrder", limitNumOrder) :
                new ObjectParameter("LimitNumOrder", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetRevokePercent_Result>("sp_GetRevokePercent", beginDateParameter, endDateParameter, searchColumnsParameter, displayStartParameter, displayLengthParameter, sortDirectionParameter, currentPageParameter, orderFieldParameter, limitNumOrderParameter, pageCount, totalRecords, totalDisplayRecords);
        }
    
        public virtual int sp_GetCustomerTradeDetail(Nullable<int> beginDate, Nullable<int> endDate, string searchColumns, Nullable<int> displayStart, Nullable<int> displayLength, string sortDirection, Nullable<int> currentPage, string orderField, ObjectParameter pageCount, ObjectParameter totalRecords, ObjectParameter totalDisplayRecords)
        {
            var beginDateParameter = beginDate.HasValue ?
                new ObjectParameter("BeginDate", beginDate) :
                new ObjectParameter("BeginDate", typeof(int));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(int));
    
            var searchColumnsParameter = searchColumns != null ?
                new ObjectParameter("SearchColumns", searchColumns) :
                new ObjectParameter("SearchColumns", typeof(string));
    
            var displayStartParameter = displayStart.HasValue ?
                new ObjectParameter("DisplayStart", displayStart) :
                new ObjectParameter("DisplayStart", typeof(int));
    
            var displayLengthParameter = displayLength.HasValue ?
                new ObjectParameter("DisplayLength", displayLength) :
                new ObjectParameter("DisplayLength", typeof(int));
    
            var sortDirectionParameter = sortDirection != null ?
                new ObjectParameter("SortDirection", sortDirection) :
                new ObjectParameter("SortDirection", typeof(string));
    
            var currentPageParameter = currentPage.HasValue ?
                new ObjectParameter("CurrentPage", currentPage) :
                new ObjectParameter("CurrentPage", typeof(int));
    
            var orderFieldParameter = orderField != null ?
                new ObjectParameter("OrderField", orderField) :
                new ObjectParameter("OrderField", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_GetCustomerTradeDetail", beginDateParameter, endDateParameter, searchColumnsParameter, displayStartParameter, displayLengthParameter, sortDirectionParameter, currentPageParameter, orderFieldParameter, pageCount, totalRecords, totalDisplayRecords);
        }
    
        public virtual ObjectResult<sp_GetStrategyMatchQty_Result> sp_GetStrategyMatchQty(Nullable<int> beginMonth, Nullable<int> endMonth, string strategyName, string kindName, string seriesNo)
        {
            var beginMonthParameter = beginMonth.HasValue ?
                new ObjectParameter("BeginMonth", beginMonth) :
                new ObjectParameter("BeginMonth", typeof(int));
    
            var endMonthParameter = endMonth.HasValue ?
                new ObjectParameter("EndMonth", endMonth) :
                new ObjectParameter("EndMonth", typeof(int));
    
            var strategyNameParameter = strategyName != null ?
                new ObjectParameter("StrategyName", strategyName) :
                new ObjectParameter("StrategyName", typeof(string));
    
            var kindNameParameter = kindName != null ?
                new ObjectParameter("KindName", kindName) :
                new ObjectParameter("KindName", typeof(string));
    
            var seriesNoParameter = seriesNo != null ?
                new ObjectParameter("SeriesNo", seriesNo) :
                new ObjectParameter("SeriesNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetStrategyMatchQty_Result>("sp_GetStrategyMatchQty", beginMonthParameter, endMonthParameter, strategyNameParameter, kindNameParameter, seriesNoParameter);
        }
    
        public virtual ObjectResult<sp_GetStrategyMatchAmt_Result> sp_GetStrategyMatchAmt(Nullable<int> beginMonth, Nullable<int> endMonth, string strategyName, string kindName, string seriesNo)
        {
            var beginMonthParameter = beginMonth.HasValue ?
                new ObjectParameter("BeginMonth", beginMonth) :
                new ObjectParameter("BeginMonth", typeof(int));
    
            var endMonthParameter = endMonth.HasValue ?
                new ObjectParameter("EndMonth", endMonth) :
                new ObjectParameter("EndMonth", typeof(int));
    
            var strategyNameParameter = strategyName != null ?
                new ObjectParameter("StrategyName", strategyName) :
                new ObjectParameter("StrategyName", typeof(string));
    
            var kindNameParameter = kindName != null ?
                new ObjectParameter("KindName", kindName) :
                new ObjectParameter("KindName", typeof(string));
    
            var seriesNoParameter = seriesNo != null ?
                new ObjectParameter("SeriesNo", seriesNo) :
                new ObjectParameter("SeriesNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetStrategyMatchAmt_Result>("sp_GetStrategyMatchAmt", beginMonthParameter, endMonthParameter, strategyNameParameter, kindNameParameter, seriesNoParameter);
        }
    }
}
