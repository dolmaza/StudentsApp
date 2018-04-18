using Data.Entities;
using Services.IServices;
using Services.Properties;
using Services.Services;
using Services.Utilities;
using Services.Validation;
using StudentsApp.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StudentsApp.Models
{
    public class StudentModel
    {
        private readonly IStudentService _studentService;

        public StudentModel()
        {
            _studentService = new StudentService();
        }

        #region Methods
        public StudentsViewModel GetStudentsViewModel(UrlHelper url)
        {
            return new StudentsViewModel
            {
                StudentCreateUrl = url.RouteUrl(ControllerActionRouteNames.Student.StudentsCreate),
                StudentsGetFilteredUrl = url.RouteUrl(ControllerActionRouteNames.Student.StudentsGetFiltered),
                StudentGridItems = GetStudentGridItems(url, _studentService.GetAll())
            };
        }

        public AjaxResponse StudentsGetFiltered(UrlHelper url, string searchPhrase, DateTime? birthdateFrom = null, DateTime? birthdateTo = null)
        {
            var ajaxResponse = new AjaxResponse();
            var students = _studentService.GetAll(searchPhrase, birthdateFrom, birthdateTo);

            if (students != null)
            {
                ajaxResponse.IsSuccess = true;
                ajaxResponse.Data = new
                {
                    Students = GetStudentGridItems(url, students)
                };
            }

            return ajaxResponse;
        }

        List<StudentsViewModel.StudentGridItem> GetStudentGridItems(UrlHelper url, List<Student> students)
        {
            return students.Select(s => new StudentsViewModel.StudentGridItem
            {
                Id = s.Id,
                Firstname = s.Firstname,
                Lastname = s.Lastname,
                PersonalNumber = s.PersonalNumber,
                Birthdate = string.Format(Constants.Formats.DateEval, s.Birthdate),
                Gender = s.Gender == Gender.Male ? Resources.TextMale : Resources.TextFemale,

                StudentUpdateUrl = url.RouteUrl(ControllerActionRouteNames.Student.StudentsUpdate, new { studentId = s.Id }),
                StudentDeleteUrl = url.RouteUrl(ControllerActionRouteNames.Student.StudentsDelete, new { studentId = s.Id })
            }).ToList();
        }

        public AjaxResponse StudentCreate(StudentCreateViewModel model)
        {
            var ajaxResponse = new AjaxResponse();
            var validation = new Validation();
            var errors = validation.ValidateStudentCreateForm(model.Firstname, model.Lastname, model.PersonalNumber, model.Birthdate, model.Gender);
            if (errors.Count == 0)
            {
                _studentService.Create(new Student
                {
                    PersonalNumber = model.PersonalNumber,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Birthdate = model.Birthdate.Value,
                    Gender = (Gender)model.Gender
                });

                ajaxResponse.IsSuccess = !_studentService.IsError;
            }
            else
            {
                ajaxResponse.Data = new
                {
                    Errors = errors
                };
            }


            return ajaxResponse;
        }

        public AjaxResponse StudentUpdate(StudentUpdateViewModel model)
        {
            var ajaxResponse = new AjaxResponse();
            var validation = new Validation();
            var errors = validation.ValidateStudentUpdateForm(model.Firstname, model.Lastname, model.PersonalNumber, model.Birthdate, model.Gender, model.studentId);
            if (errors.Count == 0)
            {
                var student = _studentService.GetById(model.studentId);

                if (student != null)
                {
                    student.PersonalNumber = model.PersonalNumber;
                    student.Firstname = model.Firstname;
                    student.Lastname = model.Lastname;
                    student.Birthdate = model.Birthdate.Value;
                    student.Gender = (Gender)model.Gender;

                    _studentService.Update(student);

                    ajaxResponse.IsSuccess = !_studentService.IsError;
                }
            }
            else
            {
                ajaxResponse.Data = new
                {
                    Errors = errors
                };
            }

            return ajaxResponse;
        }

        public AjaxResponse StudentDelete(int id)
        {
            var ajaxResponse = new AjaxResponse();

            _studentService.Delete(id);

            ajaxResponse.IsSuccess = !_studentService.IsError;

            return ajaxResponse;
        }

        public StudentCreateViewModel GetStudentCreateViewModel(UrlHelper url)
        {
            return new StudentCreateViewModel
            {
                SaveUrl = url.RouteUrl(ControllerActionRouteNames.Student.StudentsCreate),
                StudentsUrl = url.RouteUrl(ControllerActionRouteNames.Student.Students),

                Genders = GetGenders()
            };
        }

        public StudentUpdateViewModel GetStudentUpdateViewModel(UrlHelper url, int studentId)
        {
            var student = _studentService.GetById(studentId);

            if (student == null)
            {
                return null;
            }
            else
            {
                return new StudentUpdateViewModel
                {
                    PersonalNumber = student.PersonalNumber,
                    Firstname = student.Firstname,
                    Lastname = student.Lastname,
                    BirthdateString = string.Format(Constants.Formats.DateEval, student.Birthdate),
                    Genders = GetGenders(student.Gender),
                    SaveUrl = url.RouteUrl(ControllerActionRouteNames.Student.StudentsUpdate, new { studentId = student.Id }),
                    StudentsUrl = url.RouteUrl(ControllerActionRouteNames.Student.Students),
                };
            }
        }

        List<SimpleKeyValue<int, string>> GetGenders(Gender? selectedValue = null)
        {
            return new List<SimpleKeyValue<int, string>>
            {
                new SimpleKeyValue<int, string>
                {
                    Key = (int)Gender.Male,
                    Value = Resources.TextMale,
                    IsSelected = Gender.Male == selectedValue
                },
                new SimpleKeyValue<int, string>
                {
                    Key = (int)Gender.Female,
                    Value = Resources.TextFemale,
                    IsSelected = Gender.Female == selectedValue
                }
            };
        }

        #endregion
    }

    public class StudentsViewModel
    {
        #region Properties
        public List<StudentGridItem> StudentGridItems { get; set; }
        public string StudentCreateUrl { get; set; }
        public string StudentsGetFilteredUrl { get; set; }
        #endregion

        #region Sub Classes
        public class StudentGridItem
        {
            #region Properties
            public int Id { get; set; }
            public string PersonalNumber { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Gender { get; set; }
            public string Birthdate { get; set; }

            public string StudentUpdateUrl { get; set; }
            public string StudentDeleteUrl { get; set; }
            #endregion
        }
        #endregion
    }

    public class StudentCreateViewModel
    {
        public string PersonalNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int? Gender { get; set; }
        public DateTime? Birthdate { get; set; }

        public string SaveUrl { get; set; }
        public string StudentsUrl { get; set; }

        public List<SimpleKeyValue<int, string>> Genders { get; set; }
    }

    public class StudentUpdateViewModel
    {
        public int studentId { get; set; }
        public string PersonalNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int? Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public string BirthdateString { get; set; }

        public string SaveUrl { get; set; }
        public string StudentsUrl { get; set; }


        public List<SimpleKeyValue<int, string>> Genders { get; set; }
    }
}