using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        IQueryable<Student> queryable = _studentRepository.AsQueryable();

        if (activeOnly)
        {
            queryable = queryable.WithSpecification(new ActiveStudentSpecification(
                new DateMonthOnly(DateTime.Now),
                _enrollmentRepository.AsQueryable()
            ));
        }

        return queryable
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Student>> GetStudentAsync(Guid id)
    {
        Student? student = await _studentRepository
            .AsQueryable()
            .FirstOrDefaultAsync(s => s.Id == id);

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
