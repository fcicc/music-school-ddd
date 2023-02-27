using Microsoft.AspNetCore.Mvc;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;

namespace MusicSchool.SchoolManagement.Api.Controllers;

[ApiController]
[Route("/courses")]
public class CoursesController : ControllerBase
{
    private readonly IRepository<Course> _courseRepository;
    private readonly ICourseService _courseService;

    public CoursesController(
        IRepository<Course> courseRepository,
        ICourseService courseService)
    {
        _courseRepository = courseRepository;
        _courseService = courseService;
    }

    [HttpGet("")]
    public Task<List<Course>> GetCoursesAsync()
    {
        return _courseRepository.FindAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Course>> GetCourseAsync(Guid id)
    {
        Course? course = await _courseRepository.FindOneAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        return course;
    }

    [HttpPost("")]
    public async Task<ActionResult<Course>> PostCourseAsync(
        ICourseService.CreateRequest request)
    {
        try
        {
            return await _courseService.CreateAsync(request);
        }
        catch (DomainException e)
        {
            // TODO: use standard format
            return BadRequest(new
            {
                Message = e.Message,
            });
        }
    }
}
