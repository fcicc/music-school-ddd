using Moq;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.External.SchoolManagement;
using MusicSchool.Finance.Domain.External.SchoolManagement.Models.Response;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Domain.Tests.Services;

public class InvoiceServiceTests
{
    private readonly Mock<ISchoolManagementClient> _schoolManagementClientMock;

    private readonly InvoiceService _sut;

    public InvoiceServiceTests()
    {
        _schoolManagementClientMock = new();

        _sut = new(_schoolManagementClientMock.Object);
    }

    [Fact]
    public async Task GenerateInvoicesForStudentAsync_WithNonExistingStudent_ThrowsDomainException()
    {
        Guid studentId = Guid.NewGuid();

        _schoolManagementClientMock
            .Setup(c => c.GetStudentAsync(studentId))
            .ReturnsAsync((Student?)null);

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.GenerateInvoicesForStudent(studentId)
        );

        Assert.Equal("Student not found.", exception.Message);
    }
}
