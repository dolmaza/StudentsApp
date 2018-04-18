using System;

namespace Data.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string PersonalNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
