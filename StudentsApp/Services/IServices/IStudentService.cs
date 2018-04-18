using Data.Entities;
using System;
using System.Collections.Generic;

namespace Services.IServices
{
    public interface IStudentService : IBaseService
    {
        List<Student> GetAll();
        List<Student> GetAll(string searchPhrase, DateTime? birthdateFrom = null, DateTime? birthdateTo = null);
        Student GetById(int id);

        void Create(Student student);
        void Update(Student student);
        void Delete(int id);

        bool IsStudentPersonalNumberNotUnique(string personalNumber, int? id = null);
    }
}
