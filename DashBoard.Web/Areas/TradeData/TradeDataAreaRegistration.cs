using System.Web.Mvc;

namespace DashBoard.Web.Areas.TradeData
{
    public class TradeDataAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TradeData";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TradeData_default",
                "TradeData/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
