using Core.Repositories;
using Data.Entities;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class StudentService : BaseService, IStudentService
    {
        private readonly IRepository<Student> _studentRepository;

        public StudentService()
        {
            _studentRepository = GetRepository<Student>();
        }

        public void Create(Student student)
        {
            student.CreateTime = DateTime.Now;
            _studentRepository.Add(student);
            Complete();
        }

        public void Delete(int id)
        {
            var student = _studentRepository.GetById(id);
            if (student == null)
            {
                IsError = true;
            }
            else
            {
                _studentRepository.Remove(student);
                Complete();
            }
        }

        public List<Student> GetAll()
        {
            return _studentRepository.GetAll(orderBy: ob => ob.OrderByDescending(s => s.Id)).ToList();
        }

        public List<Student> GetAll(string searchPhrase, DateTime? birthdateFrom = null, DateTime? birthdateTo = null)
        {
            return _studentRepository.Get(filter: s =>
                (
                    searchPhrase == null
                    ||
                    (
                        s.Firstname.Contains(searchPhrase)
                        || s.Lastname.Contains(searchPhrase)
                        || s.PersonalNumber.Contains(searchPhrase)
                    )
                ) &&
                (
                    (birthdateFrom.Value == null && birthdateTo.Value == null)
                    || (birthdateFrom.Value != null && birthdateTo.Value == null && s.Birthdate >= birthdateFrom.Value)
                    || (birthdateFrom.Value == null && birthdateTo.Value != null && s.Birthdate <= birthdateTo.Value)
                    || (birthdateFrom.Value != null && birthdateTo.Value != null && s.Birthdate >= birthdateFrom.Value && s.Birthdate <= birthdateTo.Value)
                )
            ).ToList();
        }

        public Student GetById(int id)
        {
            return _studentRepository.GetById(id);
        }

        public void Update(Student student)
        {
            _studentRepository.Update(student);
            Complete();
        }

        public bool IsStudentPersonalNumberNotUnique(string personalNumber, int? id = null)
        {
            return _studentRepository.Exists(u => (id == null && u.PersonalNumber == personalNumber) || (u.Id != id && u.PersonalNumber == personalNumber));
        }
    }
}
