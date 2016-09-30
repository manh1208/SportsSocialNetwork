using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.PlaceOwner
{
    public class PlaceOwnerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PlaceOwner";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PlaceOwner_default",
                "PlaceOwner/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "SportsSocialNetwork.Areas.PlaceOwner.Controllers" }
            );
        }
    }
}