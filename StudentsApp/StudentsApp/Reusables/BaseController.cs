using StudentsApp.Reusables.Filters;
using System.Web.Mvc;

namespace StudentsApp.Reusables
{
    [BeforePageLoad]
    public class BaseController : Controller
    {
        [Route("not-found", Name = ControllerActionRouteNames.Shared.NotFound)]
        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}