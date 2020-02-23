using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTemplate.Entities;

namespace WebApiTemplate.Repositories
{
    public interface IWebApiTemplateRepository
    {
        IEnumerable<Student> GetStudents();
        Student GetStudent(int studentId);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(Student student);
        bool StudentExists(int studentId);
    }
}
