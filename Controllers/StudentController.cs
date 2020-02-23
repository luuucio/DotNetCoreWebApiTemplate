using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTemplate.Models;
using WebApiTemplate.Repositories;

namespace WebApiTemplate.Controllers
{
    [ApiController]
    [Route("/api/students")]
    public class StudentController : ControllerBase
    {
        private readonly IWebApiTemplateRepository _repository;
        private IMapper _mapper;

        public StudentController(IWebApiTemplateRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<StudentDto>> GetPersons()
        {
            var studentsFromRepo = _repository.GetStudents();
            return Ok(_mapper.Map<IEnumerable<StudentDto>>(studentsFromRepo));
        }

        [HttpGet]
        [Route("{studentId:int}")]
        public ActionResult<StudentDto> GetStudent(int studentId)
        {
            var personFromRepo = _repository.GetStudent(studentId);

            if (personFromRepo == null)
                return NotFound();

            return Ok(_mapper.Map<StudentDto>(personFromRepo));
        }

    }
}
