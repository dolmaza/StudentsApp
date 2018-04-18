using Services.IServices;
using Services.Properties;
using Services.Services;
using System;
using System.Collections.Generic;

namespace Services.Validation
{
    public class CustomError
    {
        public int Code { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CustomErrors
    {
        public static CustomError FirstnameIsRequired => new CustomError { Code = 1, ErrorMessage = Resources.ValidationFieldIsRequired };
        public static CustomError LastnameIsRequired => new CustomError { Code = 2, ErrorMessage = Resources.ValidationFieldIsRequired };
        public static CustomError PersonalNumberIsRequired => new CustomError { Code = 3, ErrorMessage = Resources.ValidationFieldIsRequired };
        public static CustomError BirthdateIsRequired => new CustomError { Code = 4, ErrorMessage = Resources.ValidationFieldIsRequired };
        public static CustomError GenderIsRequired => new CustomError { Code = 5, ErrorMessage = Resources.ValidationFieldIsRequired };
        public static CustomError PersonalNumberMustBeUnique => new CustomError { Code = 6, ErrorMessage = Resources.ValidationPersonalNumberMustBeUnique };
        public static CustomError PersonalNumberMustBeElevenDigits => new CustomError { Code = 7, ErrorMessage = Resources.ValidationPersonalNumberMustBeElevenDigits };
        public static CustomError StudentMustBeMoreThenSixteenYearsOld => new CustomError { Code = 8, ErrorMessage = Resources.ValidationStudentMustBeMoreThenSixteenYearsOld };
    }

    public class ValidationBase
    {
        private readonly IStudentService _studentService;

        public ValidationBase()
        {
            _studentService = new StudentService();
        }

        protected CustomError ValidateFirstname(string firstname)
        {
            if (string.IsNullOrWhiteSpace(firstname))
            {
                return CustomErrors.FirstnameIsRequired;
            }
            return null;
        }

        protected CustomError ValidateLastname(string lastname)
        {
            if (string.IsNullOrWhiteSpace(lastname))
            {
                return CustomErrors.LastnameIsRequired;
            }
            return null;
        }

        protected CustomError ValidatePersonalNumber(string personalNumber, int? studentId = null)
        {
            if (string.IsNullOrWhiteSpace(personalNumber))
            {
                return CustomErrors.PersonalNumberIsRequired;
            }
            else if (_studentService.IsStudentPersonalNumberNotUnique(personalNumber, studentId))
            {
                return CustomErrors.PersonalNumberMustBeUnique;
            }
            else if (personalNumber.Length != 11)
            {
                return CustomErrors.PersonalNumberMustBeElevenDigits;
            }

            return null;
        }

        protected CustomError ValidateBirthdate(DateTime? birthdate)
        {
            if (birthdate == null)
            {
                return CustomErrors.BirthdateIsRequired;
            }
            else
            {
                var now = DateTime.Now;
                var age = now.Year - birthdate.Value.Year;
                if (birthdate > now.AddYears(-age))
                {
                    age--;
                }

                if (age <= 16)
                {
                    return CustomErrors.StudentMustBeMoreThenSixteenYearsOld;
                }
            }

            return null;
        }

        protected CustomError ValidateGender(int? gender)
        {
            if (gender == null)
            {
                return CustomErrors.GenderIsRequired;
            }
            return null;
        }
    }

    public class Validation : ValidationBase
    {
        public List<CustomError> ValidateStudentCreateForm(string firtname, string lastname, string personalNumber, DateTime? birthdate, int? gender)
        {
            var errors = new List<CustomError>
            {
                ValidateFirstname(firtname),
                ValidateLastname(lastname),
                ValidatePersonalNumber(personalNumber),
                ValidateBirthdate(birthdate),
                ValidateGender(gender)
            };

            errors.RemoveAll(e => e == null);

            return errors;
        }

        public List<CustomError> ValidateStudentUpdateForm(string firtname, string lastname, string personalNumber, DateTime? birthdate, int? gender, int? studentId = null)
        {
            var errors = new List<CustomError>
            {
                ValidateFirstname(firtname),
                ValidateLastname(lastname),
                ValidatePersonalNumber(personalNumber, studentId),
                ValidateBirthdate(birthdate),
                ValidateGender(gender)
            };

            errors.RemoveAll(e => e == null);

            return errors;
        }
    }
}