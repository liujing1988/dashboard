using System.Web.Mvc;

namespace DashBoard.Web.Areas.HealthIndex
{
    public class HealthIndexAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HealthIndex";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HealthIndex_default",
                "HealthIndex/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
