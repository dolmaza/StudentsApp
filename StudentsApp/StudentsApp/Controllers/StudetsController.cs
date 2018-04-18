using StudentsApp.Models;
using StudentsApp.Reusables;
using System;
using System.Web.Mvc;

namespace StudentsApp.Controllers
{
    [RoutePrefix("students")]
    public class StudetsController : BaseController
    {
        private readonly StudentModel _studentModel;

        public StudetsController()
        {
            _studentModel = new StudentModel();
        }

        [Route("", Name = ControllerActionRouteNames.Student.Students)]
        public ActionResult Students()
        {
            var model = _studentModel.GetStudentsViewModel(Url);
            return View(model);
        }

        [HttpPost]
        [Route("get-filtered", Name = ControllerActionRouteNames.Student.StudentsGetFiltered)]
        public ActionResult StudentsGetFiltered(string searchPhrase, DateTime? birthdateFrom, DateTime? birthdateTo)
        {
            var ajaxResponse = _studentModel.StudentsGetFiltered(Url, searchPhrase, birthdateFrom, birthdateTo);
            return Json(ajaxResponse);
        }

        [Route("create", Name = ControllerActionRouteNames.Student.StudentsCreate)]
        public ActionResult StudentsCreate()
        {
            var model = _studentModel.GetStudentCreateViewModel(Url);
            return View(model);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult StudentsCreate(StudentCreateViewModel model)
        {
            var ajaxResponse = _studentModel.StudentCreate(model);
            return Json(ajaxResponse);
        }

        [Route("{studentId}/update", Name = ControllerActionRouteNames.Student.StudentsUpdate)]
        public ActionResult StudentsUpdate(int studentId)
        {
            var model = _studentModel.GetStudentUpdateViewModel(Url, studentId);
            if (model == null)
            {
                return NotFound();
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [Route("{studentId}/update")]
        public ActionResult StudentsUpdate(StudentUpdateViewModel model)
        {
            var ajaxResponse = _studentModel.StudentUpdate(model);
            return Json(ajaxResponse);
        }


        [HttpPost]
        [Route("{studentId}/delete", Name = ControllerActionRouteNames.Student.StudentsDelete)]
        public ActionResult StudentsDelete(int studentId)
        {
            var ajaxResponse = _studentModel.StudentDelete(studentId);
            return Json(ajaxResponse);
        }
    }
}