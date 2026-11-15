using Domain.Departments.Exceptions;

namespace Domain.Departments.ValueObjects;

public sealed record DepartmentDescription
{
    public string Value { get; }
    private DepartmentDescription(string value) => Value = value;

    public static DepartmentDescription Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new DepartmentInvalidDescriptionException();
        var normalized = value.Trim();
        if (normalized.Length > 500) throw new DepartmentInvalidDescriptionException();
        return new DepartmentDescription(normalized);
    }
}
