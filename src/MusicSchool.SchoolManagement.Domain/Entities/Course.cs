namespace MusicSchool.SchoolManagement.Domain.Entities;

public class Course : IAggregateRoot
{
    internal Course() { }

    public Guid Id { get; init; }

    public string Name { get; init; } = "";
}
