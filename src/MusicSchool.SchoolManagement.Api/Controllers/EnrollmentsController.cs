using Microsoft.AspNetCore.Mvc;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Domain.Specifications;

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
        List<ISpecification<Enrollment>> specifications = new();

        if (studentId != null)
        {
            specifications.Add(
                new EnrollmentForStudentSpecification(studentId.Value)
            );
        }

        return _enrollmentRepository.FindAsync(specifications.ToArray());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Enrollment>> GetEnrollmentAsync(Guid id)
    {
        Enrollment? enrollment = await _enrollmentRepository.FindOneAsync(id);
        if (enrollment == null)
        {
            return NotFound();
        }

        return enrollment;
    }

    [HttpPost("")]
    public async Task<ActionResult<Enrollment>> PostEnrollmentAsync(
        IEnrollmentService.EnrollRequest request)
    {
        try
        {
            return await _enrollmentService.EnrollAsync(request);
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
