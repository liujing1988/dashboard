using System.Web.Mvc;

namespace DashBoard.Web.Areas.ComprehensiveSearch
{
    public class ComprehensiveSearchAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ComprehensiveSearch";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ComprehensiveSearch_default",
                "ComprehensiveSearch/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
