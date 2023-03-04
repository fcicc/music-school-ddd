using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;

namespace MusicSchool.SchoolManagement.Api.Controllers;

[ApiController]
[Route("/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly IRepository<Enrollment> _enrollmentRepository;
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(
        IRepository<Enrollment> enrollmentRepository,
        IEnrollmentService enrollmentService)
    {
        _enrollmentRepository = enrollmentRepository;
        _enrollmentService = enrollmentService;
    }

    [HttpGet("")]
    public Task<List<Enrollment>> GetEnrollmentsAsync(Guid? studentId = null)
    {
        IQueryable<Enrollment> queryable = _enrollmentRepository.AsQueryable();

        if (studentId != null)
        {
            queryable = queryable.Where(e => e.StudentId == studentId.Value);
        }

        return queryable
            .OrderBy(s => s.StartMonth).ThenBy(s => s.EndMonth)
            .ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Enrollment>> GetEnrollmentAsync(Guid id)
    {
        Enrollment? enrollment = await _enrollmentRepository
            .AsQueryable()
            .FirstOrDefaultAsync(e => e.Id == id);

        if (enrollment == null)
        {
            return NotFound();
        }

        return enrollment;
    }

    [HttpPost("")]
    public async Task<ActionResult<Enrollment>> PostEnrollmentAsync(
        IEnrollmentService.CreateEnrollmentRequest request)
    {
        try
        {
            return await _enrollmentService.CreateAsync(request);
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
