namespace MusicSchool.SchoolManagement.Domain.Entities;

public class Teacher : IAggregateRoot
{
    public Guid Id { get; init; }

    public string Name { get; init; } = "";
}
