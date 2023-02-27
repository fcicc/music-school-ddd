using Microsoft.AspNetCore.Mvc;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;

namespace MusicSchool.SchoolManagement.Api.Controllers;

[ApiController]
[Route("/students")]
public class StudentsController : ControllerBase
{
    private readonly IRepository<Student> _studentRepository;
    private readonly IStudentService _studentService;

    public StudentsController(
        IRepository<Student> studentRepository,
        IStudentService studentService)
    {
        _studentRepository = studentRepository;
        _studentService = studentService;
    }

    [HttpGet("")]
    public Task<List<Student>> GetStudentsAsync()
    {
        return _studentRepository.FindAsync();
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
        IStudentService.CreateRequest request)
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
