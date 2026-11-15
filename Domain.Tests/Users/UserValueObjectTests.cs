using Domain.Users.ValueObjects;

namespace Domain.Tests.Users;

public sealed class UserValueObjectTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not-an-email")]
    public void Email_ShouldRejectInvalidValues(string? value)
        => Assert.True(Email.Create(value).IsFailure);

    [Fact]
    public void Email_ShouldCompareCaseInsensitively()
    {
        var first = Email.Create("Person@example.com").Value;
        var second = Email.Create(" person@example.com ").Value;

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("name.with.dot")]
    [InlineData("user name")]
    public void Username_ShouldRejectInvalidValues(string value)
        => Assert.True(Username.Create(value).IsFailure);

    [Fact]
    public void Username_ShouldNormalizeCaseAndWhitespace()
        => Assert.Equal("doctor-1", Username.Create(" Doctor-1 ").Value.Value);

    [Fact]
    public void FullName_ShouldAcceptUnicodeLettersAndNormalizeWhitespace()
    {
        var result = FullName.Create("  محمد ", "  O'Neil-Smith ");

        Assert.True(result.IsSuccess);
        Assert.Equal("محمد O'Neil-Smith", result.Value.DisplayName);
    }

    [Fact]
    public void FullName_ShouldRejectNumbersAndSymbols()
        => Assert.True(FullName.Create("John3", "Smith!").IsFailure);

    [Theory]
    [InlineData("+20 123-456-7890", "+201234567890")]
    [InlineData("+14155552671", "+14155552671")]
    public void PhoneNumber_ShouldNormalizeE164(string input, string expected)
        => Assert.Equal(expected, PhoneNumber.Create(input).Value.Value);

    [Theory]
    [InlineData("123456789")]
    [InlineData("+01234567")]
    [InlineData("+123")]
    [InlineData("+12345678901234567")]
    public void PhoneNumber_ShouldRejectInvalidE164Values(string input)
        => Assert.True(PhoneNumber.Create(input).IsFailure);
}
