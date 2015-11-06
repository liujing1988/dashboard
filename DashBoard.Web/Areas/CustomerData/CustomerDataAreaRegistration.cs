using System.Web.Mvc;

namespace DashBoard.Web.Areas.CustomerData
{
    public class CustomerDataAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CustomerData";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CustomerData_default",
                "CustomerData/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
