using Moq;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.External.SchoolManagement;
using MusicSchool.Finance.Domain.External.SchoolManagement.Models;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Tests.Services;

public class InvoiceServiceTests
{
    private readonly Mock<ISchoolManagementClient> _schoolManagementClientMock;
    private readonly Mock<IRepository<Invoice>> _invoiceRepositoryMock;

    private readonly InvoiceService _sut;

    public InvoiceServiceTests()
    {
        _schoolManagementClientMock = new();
        _invoiceRepositoryMock = new();

        _sut = new(
            _schoolManagementClientMock.Object,
            _invoiceRepositoryMock.Object
        );
    }

    [Fact]
    public async Task GenerateInvoicesForStudentAsync_WithOneEnrollment_GeneratesInvoices()
    {
        StudentResponse student = new()
        {
            Id = Guid.NewGuid(),
            Name = "Luiz Melodia",
        };

        CourseResponse course = new()
        {
            Id = Guid.NewGuid(),
            Name = "Técnica Vocal",
        };

        EnrollmentResponse enrollment = new()
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course.Id,
            StartMonth = new DateMonthOnly(2023, 1),
            EndMonth = new DateMonthOnly(2023, 6),
            MonthlyBillingValue = 200
        };

        _schoolManagementClientMock
            .Setup(c => c.GetStudentAsync(student.Id))
            .ReturnsAsync(student);

        _schoolManagementClientMock
            .Setup(c => c.GetCourseAsync(course.Id))
            .ReturnsAsync(course);

        _schoolManagementClientMock
            .Setup(c => c.GetEnrollmentsAsync(student.Id))
            .ReturnsAsync(new List<EnrollmentResponse> { enrollment });

        IInvoiceService.GenerateInvoicesForStudentRequest request = new()
        {
            StudentId = student.Id,
        };

        List<Invoice> invoices = await _sut.GenerateInvoicesForStudentAsync(request);

        Assert.Collection(invoices,
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 1), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment.Id, item.EnrollmentId);
                        Assert.Equal(course.Id, item.CourseId);
                        Assert.Equal(course.Name, item.CourseName);
                        Assert.Equal(enrollment.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            },
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 2), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment.Id, item.EnrollmentId);
                        Assert.Equal(course.Id, item.CourseId);
                        Assert.Equal(course.Name, item.CourseName);
                        Assert.Equal(enrollment.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            },
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 3), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment.Id, item.EnrollmentId);
                        Assert.Equal(course.Id, item.CourseId);
                        Assert.Equal(course.Name, item.CourseName);
                        Assert.Equal(enrollment.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            },
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 4), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment.Id, item.EnrollmentId);
                        Assert.Equal(course.Id, item.CourseId);
                        Assert.Equal(course.Name, item.CourseName);
                        Assert.Equal(enrollment.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            },
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 5), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment.Id, item.EnrollmentId);
                        Assert.Equal(course.Id, item.CourseId);
                        Assert.Equal(course.Name, item.CourseName);
                        Assert.Equal(enrollment.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            },
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 6), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment.Id, item.EnrollmentId);
                        Assert.Equal(course.Id, item.CourseId);
                        Assert.Equal(course.Name, item.CourseName);
                        Assert.Equal(enrollment.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            }
        );

        _invoiceRepositoryMock.Verify(r => r.AddRangeAsync(invoices.ToArray()), Times.Once);
    }

    [Fact]
    public async Task GenerateInvoicesForStudentAsync_WithTwoEnrollments_GeneratesInvoices()
    {
        StudentResponse student = new()
        {
            Id = Guid.NewGuid(),
            Name = "Luiz Melodia",
        };

        CourseResponse course1 = new()
        {
            Id = Guid.NewGuid(),
            Name = "Técnica Vocal",
        };

        CourseResponse course2 = new()
        {
            Id = Guid.NewGuid(),
            Name = "Guitarra",
        };

        EnrollmentResponse enrollment1 = new()
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course1.Id,
            StartMonth = new DateMonthOnly(2023, 1),
            EndMonth = new DateMonthOnly(2023, 4),
            MonthlyBillingValue = 200
        };

        EnrollmentResponse enrollment2 = new()
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course2.Id,
            StartMonth = new DateMonthOnly(2023, 3),
            EndMonth = new DateMonthOnly(2023, 6),
            MonthlyBillingValue = 150
        };

        _schoolManagementClientMock
            .Setup(c => c.GetStudentAsync(student.Id))
            .ReturnsAsync(student);

        _schoolManagementClientMock
            .Setup(c => c.GetCourseAsync(course1.Id))
            .ReturnsAsync(course1);

        _schoolManagementClientMock
            .Setup(c => c.GetCourseAsync(course2.Id))
            .ReturnsAsync(course2);

        _schoolManagementClientMock
            .Setup(c => c.GetEnrollmentsAsync(student.Id))
            .ReturnsAsync(new List<EnrollmentResponse> { enrollment1, enrollment2 });

        IInvoiceService.GenerateInvoicesForStudentRequest request = new()
        {
            StudentId = student.Id,
        };

        List<Invoice> invoices = await _sut.GenerateInvoicesForStudentAsync(request);

        Assert.Collection(invoices,
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 1), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment1.Id, item.EnrollmentId);
                        Assert.Equal(course1.Id, item.CourseId);
                        Assert.Equal(course1.Name, item.CourseName);
                        Assert.Equal(enrollment1.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            },
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 2), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment1.Id, item.EnrollmentId);
                        Assert.Equal(course1.Id, item.CourseId);
                        Assert.Equal(course1.Name, item.CourseName);
                        Assert.Equal(enrollment1.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            },
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 3), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment2.Id, item.EnrollmentId);
                        Assert.Equal(course2.Id, item.CourseId);
                        Assert.Equal(course2.Name, item.CourseName);
                        Assert.Equal(enrollment2.MonthlyBillingValue, item.Value);
                    },
                    item =>
                    {
                        Assert.Equal(enrollment1.Id, item.EnrollmentId);
                        Assert.Equal(course1.Id, item.CourseId);
                        Assert.Equal(course1.Name, item.CourseName);
                        Assert.Equal(enrollment1.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            },
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 4), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment2.Id, item.EnrollmentId);
                        Assert.Equal(course2.Id, item.CourseId);
                        Assert.Equal(course2.Name, item.CourseName);
                        Assert.Equal(enrollment2.MonthlyBillingValue, item.Value);
                    },
                    item =>
                    {
                        Assert.Equal(enrollment1.Id, item.EnrollmentId);
                        Assert.Equal(course1.Id, item.CourseId);
                        Assert.Equal(course1.Name, item.CourseName);
                        Assert.Equal(enrollment1.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            },
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 5), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment2.Id, item.EnrollmentId);
                        Assert.Equal(course2.Id, item.CourseId);
                        Assert.Equal(course2.Name, item.CourseName);
                        Assert.Equal(enrollment2.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            },
            invoice =>
            {
                Assert.NotEqual(Guid.Empty, invoice.Id);
                Assert.Equal(student.Id, invoice.StudentId);
                Assert.Equal(student.Name, invoice.StudentName);
                Assert.Equal(new DateMonthOnly(2023, 6), invoice.Month);
                Assert.Collection(invoice.Items,
                    item =>
                    {
                        Assert.Equal(enrollment2.Id, item.EnrollmentId);
                        Assert.Equal(course2.Id, item.CourseId);
                        Assert.Equal(course2.Name, item.CourseName);
                        Assert.Equal(enrollment2.MonthlyBillingValue, item.Value);
                    }
                );
                Assert.Equal(invoice.Items.Sum(i => i.Value), (decimal)invoice.TotalValue);
            }
        );

        _invoiceRepositoryMock.Verify(r => r.AddRangeAsync(invoices.ToArray()), Times.Once);
    }

    [Fact]
    public async Task GenerateInvoicesForStudentAsync_WithNoEnrollment_DoesNotGenerateInvoices()
    {
        StudentResponse student = new()
        {
            Id = Guid.NewGuid(),
            Name = "Luiz Melodia",
        };

        _schoolManagementClientMock
            .Setup(c => c.GetStudentAsync(student.Id))
            .ReturnsAsync(student);

        _schoolManagementClientMock
            .Setup(c => c.GetEnrollmentsAsync(student.Id))
            .ReturnsAsync(new List<EnrollmentResponse> { });

        IInvoiceService.GenerateInvoicesForStudentRequest request = new()
        {
            StudentId = student.Id,
        };

        List<Invoice> invoices = await _sut.GenerateInvoicesForStudentAsync(request);

        Assert.Empty(invoices);

        _invoiceRepositoryMock.Verify(r => r.AddRangeAsync(It.IsAny<Invoice[]>()), Times.Never);
    }

    [Fact]
    public async Task GenerateInvoicesForStudentAsync_WithNonExistingStudent_ThrowsDomainException()
    {
        IInvoiceService.GenerateInvoicesForStudentRequest request = new()
        {
            StudentId = Guid.NewGuid(),
        };

        _schoolManagementClientMock
            .Setup(c => c.GetStudentAsync(request.StudentId))
            .ReturnsAsync((StudentResponse?)null);

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.GenerateInvoicesForStudentAsync(request)
        );

        Assert.Equal("Student not found.", exception.Message);

        _invoiceRepositoryMock.Verify(r => r.AddRangeAsync(It.IsAny<Invoice[]>()), Times.Never);
    }

    [Fact]
    public async Task GenerateInvoicesForStudentAsync_WithInvalidReferenceToCourse_ThrowsInconsistentExternalStateException()
    {
        StudentResponse student = new()
        {
            Id = Guid.NewGuid(),
            Name = "Luiz Melodia",
        };

        EnrollmentResponse enrollment = new()
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = Guid.NewGuid(),
            StartMonth = new DateMonthOnly(2023, 1),
            EndMonth = new DateMonthOnly(2023, 6),
            MonthlyBillingValue = 200
        };

        _schoolManagementClientMock
            .Setup(c => c.GetStudentAsync(student.Id))
            .ReturnsAsync(student);

        _schoolManagementClientMock
            .Setup(c => c.GetEnrollmentsAsync(student.Id))
            .ReturnsAsync(new List<EnrollmentResponse> { enrollment });

        _schoolManagementClientMock
            .Setup(c => c.GetCourseAsync(enrollment.CourseId))
            .ReturnsAsync((CourseResponse?)null);

        IInvoiceService.GenerateInvoicesForStudentRequest request = new()
        {
            StudentId = student.Id,
        };

        InconsistentExternalStateException exception =
            await Assert.ThrowsAsync<InconsistentExternalStateException>(
                () => _sut.GenerateInvoicesForStudentAsync(request)
            );

        Assert.Equal("Unexpected situation: course not found for enrollment.", exception.Message);

        _invoiceRepositoryMock.Verify(r => r.AddRangeAsync(It.IsAny<Invoice[]>()), Times.Never);
    }
}
