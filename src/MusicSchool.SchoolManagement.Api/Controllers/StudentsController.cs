using Microsoft.AspNetCore.Mvc;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Domain.Specifications;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Api.Controllers;

[ApiController]
[Route("/students")]
public class StudentsController : ControllerBase
{
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Enrollment> _enrollmentRepository;
    private readonly IStudentService _studentService;

    public StudentsController(
        IRepository<Student> studentRepository,
        IRepository<Enrollment> enrollmentRepository,
        IStudentService studentService)
    {
        _studentRepository = studentRepository;
        _enrollmentRepository = enrollmentRepository;
        _studentService = studentService;
    }

    [HttpGet("")]
    public Task<List<Student>> GetStudentsAsync([FromQuery] bool activeOnly = false)
    {
        List<ISpecification<Student>> specifications = new();

        if (activeOnly)
        {
            specifications.Add(
                new ActiveStudentSpecification(
                    new DateMonthOnly(DateTime.Now),
                    _enrollmentRepository.AsQueryable()
                )
            );
        }

        return _studentRepository.FindAsync(specifications.ToArray());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Student>> GetStudentAsync(Guid id)
    {
        Student? student = await _studentRepository.FindOneAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        return student;
    }

    [HttpPost("")]
    public async Task<ActionResult<Student>> PostStudentAsync(
        IStudentService.CreateStudentRequest request)
    {
        try
        {
            return await _studentService.CreateAsync(request);
        }
        catch (DomainException e)
        {
            return BadRequest(new
            {
                Message = e.Message,
            });
        }
    }
}
