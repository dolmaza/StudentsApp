using StudentsApp.Reusables;
using System.Web.Mvc;

namespace StudentsApp.Controllers
{
    public class HomeController : BaseController
    {
        [Route("", Name = ControllerActionRouteNames.Home.Dashboard)]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}