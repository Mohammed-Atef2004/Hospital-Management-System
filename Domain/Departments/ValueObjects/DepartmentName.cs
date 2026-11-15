using Domain.Departments.Exceptions;

namespace Domain.Departments.ValueObjects;

public sealed record DepartmentName
{
    public string Value { get; }
    private DepartmentName(string value) => Value = value;

    public static DepartmentName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new DepartmentInvalidNameException();
        var normalized = value.Trim();
        if (normalized.Length > 100) throw new DepartmentInvalidNameException();
        return new DepartmentName(normalized);
    }
}
