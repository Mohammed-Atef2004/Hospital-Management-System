using Domain.SharedKernel;
using Domain.Users.Errors;
using Domain.Users.Events;
using Domain.Users.Rules;
using Domain.Users.ValueObjects;
using Domain.personnel;
using Domain.Doctors;
using Domain.Nurses;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Users;


public sealed class User : AggregateRoot<Guid>
{
    /// <summary>Reference to the Identity Provider record (e.g. ASP.NET Identity).</summary>
    public string IdentityId { get; private set; } = default!;

    public Email Email { get; private set; } = default!;
    public Username Username { get; private set; } = default!;
    public FullName FullName { get; private set; } = default!;
    public PhoneNumber? PhoneNumber { get; private set; }   // optional

    // ─── Authorization ─────────────────────────────────────────────────────────
    public UserRole Role { get; private set; }
    public int? StaffId { get; private set; }
    public Staff? StaffProfile { get; private set; }

    // ─── Lifecycle ─────────────────────────────────────────────────────────────
    public bool IsActive { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeleteReason { get; private set; }


    public DateTime? LastLoginAt { get; private set; }

    public int FailedLoginAttempts { get; private set; }

    public DateTime? LockedUntil { get; private set; }

    // ─── 2FA ───────────────────────────────────────────────────────────────────

    public bool IsTwoFactorEnabled { get; private set; }

    /// <summary>
    /// Base-32 TOTP secret.  Stored encrypted at-rest by the Infrastructure layer.
    /// Exposed here as a plain string so the domain stays persistence-ignorant.
    /// </summary>
    public string? TwoFactorSecret { get; private set; }
    public string? PendingTwoFactorSecret { get; private set; }

    // ─── Password History ──────────────────────────────────────────────────────

    /// <summary>
    /// Stores the last N ASP.NET Identity password hashes.
    /// The Application layer is responsible for hashing; the domain records history.
    /// </summary>
    private List<string> _passwordHashes = new();
    [NotMapped]
    public IReadOnlyList<string> PasswordHashes => _passwordHashes.AsReadOnly();
    public string PasswordHistoryJson
    {
        get => System.Text.Json.JsonSerializer.Serialize(_passwordHashes);
        private set => _passwordHashes = string.IsNullOrWhiteSpace(value)
            ? new List<string>()
            : System.Text.Json.JsonSerializer.Deserialize<List<string>>(value) ?? new List<string>();
    }

    /// <summary>UTC timestamp of the most recent password change.</summary>
    public DateTime? PasswordChangedAt { get; private set; }

    // ─── Constructor / Factory ─────────────────────────────────────────────────

    private User() { }

    private User(
        Guid id,
        string identityId,
        Email email,
        Username username,
        FullName fullName,
        UserRole role) : base(id)
    {
        IdentityId = identityId;
        Email = email;
        Username = username;
        FullName = fullName;
        Role = role;
        IsActive = true;
        RegisteredAt = DateTime.UtcNow;
        SetCreated(identityId);
    }

    /// <summary>
    /// Creates a new user, enforcing email + username uniqueness.
    /// </summary>
    public static Result<User> Create(
        string identityId,
        string email,
        string username,
        string firstName,
        string lastName,
        bool isEmailTaken,
        bool isUsernameTaken,
        UserRole role = UserRole.Patient)
    {
        // ── Validate identity reference ──
        if (string.IsNullOrWhiteSpace(identityId))
            return Result<User>.Failure(UserErrors.InvalidIdentityReference);

        if (!Enum.IsDefined(role))
            return Result<User>.Failure(UserErrors.InvalidRole);

        // ── Build Value Objects ──
        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return Result<User>.Failure(emailResult.Error);

        var usernameResult = Username.Create(username);
        if (usernameResult.IsFailure)
            return Result<User>.Failure(usernameResult.Error);

        var fullNameResult = FullName.Create(firstName, lastName);
        if (fullNameResult.IsFailure)
            return Result<User>.Failure(fullNameResult.Error);

        // ── Enforce business rules ──
        var user = new User(
            Guid.NewGuid(),
            identityId,
            emailResult.Value,
            usernameResult.Value,
            fullNameResult.Value,
            role);

        var emailUnique = user.CheckRule(new UserEmailMustBeUniqueRule(isEmailTaken));
        if (emailUnique.IsFailure)
            return Result<User>.Failure(emailUnique.Error);

        var usernameUnique = user.CheckRule(new UsernameMustBeUniqueRule(isUsernameTaken));
        if (usernameUnique.IsFailure)
            return Result<User>.Failure(usernameUnique.Error);

        // ── Raise domain event ──
        user.AddDomainEvent(new UserRegisteredDomainEvent(
            user.Id,
            user.Email.Value,
            user.FullName.DisplayName,
            user.Username.Value,
            user.Role));

        return Result<User>.Success(user);
    }

    // ─── Profile Operations ────────────────────────────────────────────────────

    public Result ChangeName(string firstName, string lastName)
    {
        if (!CanModify()) return Result.Failure(UserErrors.Deactivated);

        var result = FullName.Create(firstName, lastName);
        if (result.IsFailure) return Result.Failure(result.Error);

        var oldName = FullName.DisplayName;
        FullName = result.Value;
        SetUpdated("system");

        AddDomainEvent(new UserNameChangedDomainEvent(Id, oldName, FullName.DisplayName));
        return Result.Success();
    }

    public Result SetPhoneNumber(string phoneNumber)
    {
        if (!CanModify()) return Result.Failure(UserErrors.Deactivated);

        var result = PhoneNumber.Create(phoneNumber);
        if (result.IsFailure) return Result.Failure(result.Error);

        var oldPhone = PhoneNumber?.Value;
        PhoneNumber = result.Value;
        SetUpdated("system");

        AddDomainEvent(new UserPhoneNumberChangedDomainEvent(Id, oldPhone, PhoneNumber.Value));
        return Result.Success();
    }

    public Result RemovePhoneNumber()
    {
        if (!CanModify()) return Result.Failure(UserErrors.Deactivated);

        var oldPhone = PhoneNumber?.Value;
        if (oldPhone is null) return Result.Success();
        PhoneNumber = null;
        SetUpdated("system");

        AddDomainEvent(new UserPhoneNumberChangedDomainEvent(Id, oldPhone, null));
        return Result.Success();
    }

    // ─── Role Operations ───────────────────────────────────────────────────────

    public Result LinkStaffProfile(Staff staff)
    {
        ArgumentNullException.ThrowIfNull(staff);
        if (!CanModify()) return Result.Failure(UserErrors.Deactivated);
        if (StaffProfile is not null || StaffId.HasValue)
            return Result.Failure(UserErrors.StaffProfileAlreadyLinked);

        var roleMatches = (staff is Doctor && Role == UserRole.Doctor)
            || (staff is Nurse && Role == UserRole.Nurse);
        if (!roleMatches)
            return Result.Failure(UserErrors.StaffRoleMismatch);
        if (staff.IsDeleted)
            return Result.Failure(UserErrors.StaffProfileInactive);

        StaffProfile = staff;
        StaffId = staff.Id;
        SetUpdated("system");
        AddDomainEvent(new UserStaffProfileLinkedDomainEvent(Id, staff.Id, Role));
        return Result.Success();
    }

    public Result ChangeRole(UserRole newRole)
    {
        if (!CanModify()) return Result.Failure(UserErrors.Deactivated);
        if (Role == newRole) return Result.Failure(UserErrors.RoleAlreadyAssigned);
        if (!Enum.IsDefined(newRole)) return Result.Failure(UserErrors.InvalidRole);

        if (StaffProfile is not null &&
            !((StaffProfile is Doctor && newRole == UserRole.Doctor)
              || (StaffProfile is Nurse && newRole == UserRole.Nurse)))
            return Result.Failure(UserErrors.StaffRoleMismatch);

        // SuperAdmin is only seeded, never assigned through normal flow
        if (newRole == UserRole.Admin)
            return Result.Failure(UserErrors.CannotAssignSuperAdminRole);

        var oldRole = Role;
        Role = newRole;
        SetUpdated("system");

        AddDomainEvent(new UserRoleChangedDomainEvent(Id, oldRole, newRole));
        return Result.Success();
    }

    /// <summary>Convenience shortcut used by the platform.</summary>
    //public Result PromoteToInstructor() => ChangeRole(UserRole.Instructor);

    // ─── Lifecycle Operations ─────────────────────────────────────────────────

    public Result Deactivate(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            return Result.Failure(UserErrors.InvalidReason);
        if (Role == UserRole.Admin)
            return Result.Failure(UserErrors.CannotDeactivateSuperAdmin);

        if (!IsActive)
            return Result.Failure(UserErrors.Deactivated);

        IsActive = false;
        SetUpdated("system");

        AddDomainEvent(new UserDeactivatedDomainEvent(Id, reason));
        return Result.Success();
    }

    public Result Reactivate()
    {
        if (IsDeleted) return Result.Failure(UserErrors.AlreadyDeleted);
        if (IsActive) return Result.Failure(UserErrors.AlreadyActive);

        IsActive = true;
        SetUpdated("system");

        AddDomainEvent(new UserReactivatedDomainEvent(Id));
        return Result.Success();
    }

    /// <summary>Soft delete — data is retained, user cannot log in.</summary>
    public Result Delete(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            return Result.Failure(UserErrors.InvalidReason);
        if (Role == UserRole.Admin)
            return Result.Failure(UserErrors.CannotDeleteSuperAdmin);

        if (IsDeleted)
            return Result.Failure(UserErrors.AlreadyDeleted);

        base.Delete();
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        DeleteReason = reason.Trim();
        SetUpdated("system");

        AddDomainEvent(new UserDeletedDomainEvent(Id, Email.Value, DeleteReason));
        return Result.Success();
    }

    // ─── Security Operations ──────────────────────────────────────────────────

    /// <summary>
    /// Records a successful login.  Resets lockout counters.
    /// </summary>
    public Result RecordSuccessfulLogin(string ipAddress)
    {
        if (!CanModify())
            return Result.Failure(UserErrors.Deactivated);
        if (IsLockedOut())
            return Result.Failure(UserErrors.SecurityErrors.AccountLocked);

        FailedLoginAttempts = 0;
        LockedUntil = null;
        LastLoginAt = DateTime.UtcNow;
        SetUpdated("system");

        AddDomainEvent(new UserLoggedInSuccessfullyDomainEvent(Id, ipAddress, LastLoginAt.Value));
        return Result.Success();
    }

    /// <summary>
    /// Records a failed login attempt and locks the account when the threshold is exceeded.
    /// </summary>
    public Result RecordFailedLogin(string ipAddress)
    {
        if (!IsActive || IsDeleted)
            return Result.Failure(UserErrors.Deactivated);
        if (IsLockedOut())
            return Result.Failure(UserErrors.SecurityErrors.AccountLocked);

        // A new lockout period starts with a fresh attempt count.
        if (LockedUntil.HasValue)
        {
            FailedLoginAttempts = 0;
            LockedUntil = null;
        }

        FailedLoginAttempts++;

        AddDomainEvent(new UserLoginAttemptFailedDomainEvent(Id, FailedLoginAttempts, ipAddress));

        var lockoutRule = CheckRule(new AccountLockoutRule(FailedLoginAttempts));
        if (lockoutRule.IsFailure)
        {
            // Lock for 15 minutes
            LockedUntil = DateTime.UtcNow.AddMinutes(15);
            AddDomainEvent(new UserAccountLockedDomainEvent(Id, LockedUntil.Value));
        }

        SetUpdated("system");

        return Result.Success();
    }

    /// <summary>Manually unlocks an account (Admin action).</summary>
    public Result UnlockAccount()
    {
        if (IsDeleted) return Result.Failure(UserErrors.AlreadyDeleted);
        if (LockedUntil is null) return Result.Failure(UserErrors.SecurityErrors.AccountNotLocked);
        FailedLoginAttempts = 0;
        LockedUntil = null;
        SetUpdated("system");

        AddDomainEvent(new UserAccountUnlockedDomainEvent(Id));
        return Result.Success();
    }

    // ─── 2FA ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Enables TOTP-based 2FA.  The secret must be generated + verified
    /// by the Application layer before calling this method.
    /// </summary>
    public Result EnableTwoFactor(string secret)
    {
        if (!CanModify()) return Result.Failure(UserErrors.Deactivated);
        if (IsTwoFactorEnabled)
            return Result.Failure(UserErrors.SecurityErrors.TwoFactorAlreadyEnabled);

        if (!IsValidTwoFactorSecret(secret))
            return Result.Failure(UserErrors.SecurityErrors.InvalidTwoFactorSecret);

        TwoFactorSecret = secret.Trim().ToUpperInvariant();
        IsTwoFactorEnabled = true;
        SetUpdated("system");

        AddDomainEvent(new UserTwoFactorEnabledDomainEvent(Id));
        return Result.Success();
    }

    public Result BeginTwoFactorSetup(string secret)
    {
        if (!CanModify()) return Result.Failure(UserErrors.Deactivated);
        if (IsTwoFactorEnabled)
            return Result.Failure(UserErrors.SecurityErrors.TwoFactorAlreadyEnabled);
        if (!IsValidTwoFactorSecret(secret))
            return Result.Failure(UserErrors.SecurityErrors.InvalidTwoFactorSecret);

        PendingTwoFactorSecret = secret.Trim().ToUpperInvariant();
        SetUpdated("system");
        return Result.Success();
    }

    public Result CompleteTwoFactorSetup()
    {
        if (string.IsNullOrWhiteSpace(PendingTwoFactorSecret))
            return Result.Failure(UserErrors.SecurityErrors.TwoFactorSetupNotStarted);

        var result = EnableTwoFactor(PendingTwoFactorSecret);
        if (result.IsSuccess) PendingTwoFactorSecret = null;
        return result;
    }

    public Result DisableTwoFactor()
    {
        if (!CanModify()) return Result.Failure(UserErrors.Deactivated);
        if (!IsTwoFactorEnabled)
            return Result.Failure(UserErrors.SecurityErrors.TwoFactorNotEnabled);

        TwoFactorSecret = null;
        PendingTwoFactorSecret = null;
        IsTwoFactorEnabled = false;
        SetUpdated("system");

        AddDomainEvent(new UserTwoFactorDisabledDomainEvent(Id));
        return Result.Success();
    }

    // ─── Password History ─────────────────────────────────────────────────────

    /// <summary>
    /// Records a new password hash.
    /// The Application layer must check reuse BEFORE calling this
    /// (using <see cref="PasswordHashes"/> + a hash-compare service),
    /// then pass the <paramref name="isPasswordReused"/> result here.
    /// </summary>
    public Result RecordPasswordChange(string newPasswordHash, bool isPasswordReused)
    {
        if (!CanModify()) return Result.Failure(UserErrors.Deactivated);
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            return Result.Failure(UserErrors.SecurityErrors.InvalidPasswordHash);

        var reuseRule = CheckRule(new PasswordMustNotBeReusedRule(isPasswordReused));
        if (reuseRule.IsFailure) return Result.Failure(reuseRule.Error);

        _passwordHashes.Add(newPasswordHash.Trim());

        // Keep only the last N hashes (trim older ones)
        const int maxHistory = PasswordMustNotBeReusedRule.HistoryDepth;
        while (_passwordHashes.Count > maxHistory)
            _passwordHashes.RemoveAt(0);

        PasswordChangedAt = DateTime.UtcNow;
        SetUpdated("system");

        AddDomainEvent(new UserPasswordChangedDomainEvent(Id, PasswordChangedAt.Value));
        return Result.Success();
    }

    // ─── Private Helpers ──────────────────────────────────────────────────────

    /// <summary>True when the account is unlocked and not soft-deleted.</summary>
    private bool CanModify() => IsActive && !IsDeleted;

    /// <summary>True when a lockout is currently in effect.</summary>
    private bool IsLockedOut() =>
        LockedUntil.HasValue && LockedUntil.Value > DateTime.UtcNow;

    public bool IsAuthenticationLockedOut(DateTime utcNow)
        => LockedUntil.HasValue && LockedUntil.Value > utcNow;

    private static bool IsValidTwoFactorSecret(string? secret)
    {
        if (string.IsNullOrWhiteSpace(secret)) return false;
        var normalized = secret.Trim().ToUpperInvariant();
        return normalized.Length is >= 16 and <= 128
               && normalized.All(character => char.IsAsciiLetterUpper(character) || character is >= '2' and <= '7');
    }
}
