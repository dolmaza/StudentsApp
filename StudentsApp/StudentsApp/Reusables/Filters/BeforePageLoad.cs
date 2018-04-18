using Services.Properties;
using StudentsApp.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StudentsApp.Reusables.Filters
{
    public class BeforePageLoad : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (BaseController)filterContext.Controller;
            var layoutViewModel = new LayoutViewModel();
            InitMenuItems(filterContext, ref layoutViewModel, ref controller);

            layoutViewModel.TextAbort = Resources.TextAbort;
            layoutViewModel.TextSuccess = Resources.TextSuccess;
            controller.ViewBag.LayoutViewModel = layoutViewModel;
        }

        private void InitMenuItems(ActionExecutingContext filterContext, ref LayoutViewModel model, ref BaseController controller)
        {
            var requestedUrl = filterContext.HttpContext.Request.RawUrl;
            model.MenuItems = new List<LayoutViewModel.MenuItem>
            {
                new LayoutViewModel.MenuItem
                {
                    Caption = "მთავარი",
                    Icon = "fa fa-dashboard",
                    Url = controller.Url.RouteUrl(ControllerActionRouteNames.Home.Dashboard),
                    IsActive = requestedUrl == controller.Url.RouteUrl(ControllerActionRouteNames.Home.Dashboard)
                },

                new LayoutViewModel.MenuItem
                {
                    Caption = "სტუდენტები",
                    Icon = "fa fa-users",
                    Url = controller.Url.RouteUrl(ControllerActionRouteNames.Student.Students),
                    IsActive = requestedUrl == controller.Url.RouteUrl(ControllerActionRouteNames.Student.Students)
                }
            };
        }
    }
}