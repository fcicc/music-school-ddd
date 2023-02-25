using Microsoft.AspNetCore.Mvc;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Repositories;

namespace MusicSchool.SchoolManagement.Api.Controllers;

[ApiController]
[Route("/students")]
public class StudentsController : ControllerBase
{
    private readonly IRepository<Student> _studentRepository;

    public StudentsController(IRepository<Student> studentRepository)
    {
        _studentRepository = studentRepository;
    }

    [HttpGet("")]
    public Task<List<Student>> GetStudentsAsync()
    {
        return _studentRepository.FindAsync();
    }
}
