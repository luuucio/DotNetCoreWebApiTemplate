using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTemplate.DbContexts;
using WebApiTemplate.Entities;

namespace WebApiTemplate.Repositories
{
    public class WebApiTemplateRepository : IWebApiTemplateRepository, IDisposable
    {
        private readonly WebApiTemplateContext _context;

        public WebApiTemplateRepository(WebApiTemplateContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public void AddStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public void DeleteStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudents()
        {
            return _context.Students.ToList();
        }

        public Student GetStudent(int studentId)
        {
            return _context.Students.FirstOrDefault(x => x.Id == studentId);
        }

        public bool StudentExists(int studentId)
        {
            throw new NotImplementedException();
        }

        public void UpdateStudent(Student student)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                // Add resources (if any) to dispose
            }
        }
    }
}
