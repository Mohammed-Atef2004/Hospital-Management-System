using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Authentication;

public interface ITwoFactorCodeService
{
    string GenerateSecret();
    string BuildProvisioningUri(string secret, string accountName, string issuer);
    bool VerifyCode(string secret, string? code, DateTimeOffset utcNow);
}

public sealed class TotpCodeService : ITwoFactorCodeService
{
    private const int CodeDigits = 6;
    private const int TimeStepSeconds = 30;
    private const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    public string GenerateSecret() => EncodeBase32(RandomNumberGenerator.GetBytes(20));

    public string BuildProvisioningUri(string secret, string accountName, string issuer)
    {
        var label = Uri.EscapeDataString($"{issuer}:{accountName}");
        var encodedIssuer = Uri.EscapeDataString(issuer);
        return $"otpauth://totp/{label}?secret={Uri.EscapeDataString(secret)}&issuer={encodedIssuer}&algorithm=SHA1&digits={CodeDigits}&period={TimeStepSeconds}";
    }

    public bool VerifyCode(string secret, string? code, DateTimeOffset utcNow)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != CodeDigits ||
            code.Any(character => character is < '0' or > '9') || !TryDecodeBase32(secret, out var key))
            return false;

        var currentStep = utcNow.ToUnixTimeSeconds() / TimeStepSeconds;
        var supplied = Encoding.ASCII.GetBytes(code);
        for (var offset = -1; offset <= 1; offset++)
        {
            var expected = Encoding.ASCII.GetBytes(ComputeCode(key, currentStep + offset));
            if (CryptographicOperations.FixedTimeEquals(supplied, expected))
                return true;
        }
        return false;
    }

    private static string ComputeCode(byte[] key, long counter)
    {
        Span<byte> counterBytes = stackalloc byte[8];
        for (var index = counterBytes.Length - 1; index >= 0; index--)
        {
            counterBytes[index] = (byte)(counter & 0xff);
            counter >>= 8;
        }
        var hash = HMACSHA1.HashData(key, counterBytes);
        var offset = hash[^1] & 0x0f;
        var binary = ((hash[offset] & 0x7f) << 24)
            | ((hash[offset + 1] & 0xff) << 16)
            | ((hash[offset + 2] & 0xff) << 8)
            | (hash[offset + 3] & 0xff);
        var otp = binary % (int)Math.Pow(10, CodeDigits);
        return otp.ToString(new string('0', CodeDigits), CultureInfo.InvariantCulture);
    }

    private static string EncodeBase32(byte[] bytes)
    {
        var result = new StringBuilder((bytes.Length * 8 + 4) / 5);
        var buffer = 0;
        var bits = 0;
        foreach (var value in bytes)
        {
            buffer = (buffer << 8) | value;
            bits += 8;
            while (bits >= 5)
            {
                result.Append(Base32Alphabet[(buffer >> (bits - 5)) & 31]);
                bits -= 5;
            }
        }
        if (bits > 0) result.Append(Base32Alphabet[(buffer << (5 - bits)) & 31]);
        return result.ToString();
    }

    private static bool TryDecodeBase32(string secret, out byte[] bytes)
    {
        bytes = Array.Empty<byte>();
        if (string.IsNullOrWhiteSpace(secret)) return false;
        var normalized = secret.Trim().TrimEnd('=').ToUpperInvariant();
        var output = new List<byte>(normalized.Length * 5 / 8);
        var buffer = 0;
        var bits = 0;
        foreach (var character in normalized)
        {
            var value = Base32Alphabet.IndexOf(character);
            if (value < 0) return false;
            buffer = (buffer << 5) | value;
            bits += 5;
            if (bits < 8) continue;
            output.Add((byte)(buffer >> (bits - 8)));
            bits -= 8;
        }
        bytes = output.ToArray();
        return bytes.Length > 0;
    }
}