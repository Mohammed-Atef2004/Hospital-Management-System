using Domain.Users;
using Domain.Users.Errors;
using Domain.Users.Events;
using Domain.Doctors;
using Domain.Nurses;
using DoctorEntity = Domain.Doctors.Doctor;

namespace Domain.Tests.Users;

public sealed class UserAuthenticationTests
{
    private static User CreateUser(UserRole role = UserRole.Patient)
    {
        var result = User.Create("identity-user-1", "patient@example.com", " Patient_1 ", "Aisha", "Hassan",
            isEmailTaken: false, isUsernameTaken: false, role);
        Assert.True(result.IsSuccess, result.Error.Message);
        return result.Value;
    }

    [Fact]
    public void Create_ShouldNormalizeEmailAndUsername_AndRaiseRegisteredEvent()
    {
        var result = User.Create("identity-1", " Patient@Example.com ", " Patient_1 ", "Aisha", "Hassan", false, false);

        Assert.True(result.IsSuccess);
        Assert.Equal("patient@example.com", result.Value.Email.Value);
        Assert.Equal("patient_1", result.Value.Username.Value);
        Assert.Contains(result.Value.DomainEvents, item => item is UserRegisteredDomainEvent);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Create_ShouldRejectMissingIdentityReference(string? identityId)
    {
        var result = User.Create(identityId!, "patient@example.com", "patient1", "Aisha", "Hassan", false, false);

        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.InvalidIdentityReference, result.Error);
    }

    [Fact]
    public void Create_ShouldRejectDuplicateEmailAndUsername()
    {
        var duplicateEmail = User.Create("identity-2", "other@example.com", "other1", "Aisha", "Hassan", true, false);
        var duplicateUsername = User.Create("identity-3", "other@example.com", "other1", "Aisha", "Hassan", false, true);

        Assert.Equal(RulesErrors.EmailMustBeUnique, duplicateEmail.Error);
        Assert.Equal(RulesErrors.UsernameMustBeUnique, duplicateUsername.Error);
    }

    [Fact]
    public void Create_ShouldRejectUndefinedRole()
    {
        var result = User.Create("identity-4", "other@example.com", "other1", "Aisha", "Hassan", false, false, (UserRole)99);

        Assert.Equal(UserErrors.InvalidRole, result.Error);
    }

    [Fact]
    public void ChangeName_ShouldRaiseEventAndTrimValues()
    {
        var user = CreateUser();
        user.ClearDomainEvents();

        var result = user.ChangeName("  Mariam ", " Ali ");

        Assert.True(result.IsSuccess);
        Assert.Equal("Mariam Ali", user.FullName.DisplayName);
        Assert.Contains(user.DomainEvents, item => item is UserNameChangedDomainEvent);
    }

    [Fact]
    public void SetAndRemovePhoneNumber_ShouldNormalizeAndRaiseEvents()
    {
        var user = CreateUser();
        Assert.True(user.SetPhoneNumber("+20 123-456-7890").IsSuccess);
        Assert.Equal("+201234567890", user.PhoneNumber!.Value);
        Assert.True(user.RemovePhoneNumber().IsSuccess);
        Assert.Null(user.PhoneNumber);
        Assert.Equal(2, user.DomainEvents.OfType<UserPhoneNumberChangedDomainEvent>().Count());
    }

    [Fact]
    public void RemovePhoneNumber_WhenAlreadyEmpty_ShouldNotRaiseEvent()
    {
        var user = CreateUser();
        user.ClearDomainEvents();

        Assert.True(user.RemovePhoneNumber().IsSuccess);
        Assert.Empty(user.DomainEvents);
    }

    [Fact]
    public void ChangeRole_ShouldRejectAdminAndUndefinedRoles()
    {
        var user = CreateUser();

        Assert.Equal(UserErrors.CannotAssignSuperAdminRole, user.ChangeRole(UserRole.Admin).Error);
        Assert.Equal(UserErrors.InvalidRole, user.ChangeRole((UserRole)55).Error);
        Assert.Equal(UserRole.Patient, user.Role);
    }

    [Fact]
    public void ChangeRole_WhenUnchanged_ShouldReturnRoleAlreadyAssigned()
    {
        var user = CreateUser();

        Assert.Equal(UserErrors.RoleAlreadyAssigned, user.ChangeRole(UserRole.Patient).Error);
    }

    [Fact]
    public void LinkStaffProfile_ShouldRequireMatchingRoleAndRaiseEvent()
    {
        var doctorUser = CreateUser(UserRole.Doctor);
        var doctor = DoctorEntity.Create("Mona", "Ali", "12 Main St", "Cairo", "Cardiology");
        doctorUser.ClearDomainEvents();

        var linked = doctorUser.LinkStaffProfile(doctor);

        Assert.True(linked.IsSuccess);
        Assert.Same(doctor, doctorUser.StaffProfile);
        Assert.Equal(doctor.Id, doctorUser.StaffId);
        Assert.Contains(doctorUser.DomainEvents, item => item is UserStaffProfileLinkedDomainEvent);
        Assert.Equal(UserErrors.StaffProfileAlreadyLinked, doctorUser.LinkStaffProfile(doctor).Error);

        var nurseUser = CreateUser(UserRole.Doctor);
        var nurse = Nurse.Register("Hana", "Said", "2 Clinic Rd", "Cairo", null, false);
        Assert.Equal(UserErrors.StaffRoleMismatch, nurseUser.LinkStaffProfile(nurse).Error);
    }

    [Fact]
    public void Deactivate_ShouldRequireReasonAndEmitEvent()
    {
        var user = CreateUser();

        Assert.Equal(UserErrors.InvalidReason, user.Deactivate(" ").Error);
        Assert.True(user.Deactivate("Policy violation").IsSuccess);
        Assert.False(user.IsActive);
        Assert.Contains(user.DomainEvents, item => item is UserDeactivatedDomainEvent);
    }

    [Fact]
    public void AdminCannotBeDeactivatedOrDeleted()
    {
        var admin = CreateUser(UserRole.Admin);

        Assert.Equal(UserErrors.CannotDeactivateSuperAdmin, admin.Deactivate("Reason").Error);
        Assert.Equal(UserErrors.CannotDeleteSuperAdmin, admin.Delete("Reason").Error);
        Assert.True(admin.IsActive);
        Assert.False(admin.IsDeleted);
    }

    [Fact]
    public void Reactivate_ShouldRestoreInactiveUser_ButNotDeletedUser()
    {
        var user = CreateUser();
        Assert.True(user.Deactivate("Reason").IsSuccess);
        Assert.True(user.Reactivate().IsSuccess);
        Assert.True(user.IsActive);
        Assert.Contains(user.DomainEvents, item => item is UserReactivatedDomainEvent);

        Assert.True(user.Delete("Reason").IsSuccess);
        Assert.Equal(UserErrors.AlreadyDeleted, user.Reactivate().Error);
    }

    [Fact]
    public void Delete_ShouldSetSharedEntitySoftDeleteState()
    {
        var user = CreateUser();

        var result = user.Delete("  Requested by user  ");

        Assert.True(result.IsSuccess);
        Assert.True(user.IsDeleted);
        Assert.False(user.IsActive);
        Assert.NotNull(user.DeletedAtUtc);
        Assert.NotNull(user.DeletedAt);
        Assert.Equal("Requested by user", user.DeleteReason);
    }

    [Fact]
    public void Delete_ShouldRequireAReason()
    {
        var user = CreateUser();

        Assert.Equal(UserErrors.InvalidReason, user.Delete("").Error);
        Assert.False(user.IsDeleted);
    }

    [Fact]
    public void RecordFailedLogin_ShouldLockAtFiveAndStopCountingWhileLocked()
    {
        var user = CreateUser();
        for (var attempt = 1; attempt < 5; attempt++)
            Assert.True(user.RecordFailedLogin("127.0.0.1").IsSuccess);

        Assert.True(user.RecordFailedLogin("127.0.0.1").IsSuccess);
        Assert.Equal(5, user.FailedLoginAttempts);
        Assert.NotNull(user.LockedUntil);
        Assert.Contains(user.DomainEvents, item => item is UserAccountLockedDomainEvent);

        Assert.Equal(UserErrors.SecurityErrors.AccountLocked, user.RecordFailedLogin("127.0.0.1").Error);
        Assert.Equal(5, user.FailedLoginAttempts);
    }

    [Fact]
    public void SuccessfulLogin_ShouldClearFailuresAndRecordTimestamp()
    {
        var user = CreateUser();
        user.RecordFailedLogin("127.0.0.1");
        user.ClearDomainEvents();

        var result = user.RecordSuccessfulLogin("127.0.0.1");

        Assert.True(result.IsSuccess);
        Assert.Equal(0, user.FailedLoginAttempts);
        Assert.Null(user.LockedUntil);
        Assert.NotNull(user.LastLoginAt);
        Assert.Contains(user.DomainEvents, item => item is UserLoggedInSuccessfullyDomainEvent);
    }

    [Fact]
    public void FailedLogin_ShouldNotCountForInactiveOrDeletedUsers()
    {
        var inactive = CreateUser();
        inactive.Deactivate("Reason");
        Assert.Equal(UserErrors.Deactivated, inactive.RecordFailedLogin("127.0.0.1").Error);
        Assert.Equal(0, inactive.FailedLoginAttempts);

        var deleted = CreateUser();
        deleted.Delete("Reason");
        Assert.Equal(UserErrors.Deactivated, deleted.RecordFailedLogin("127.0.0.1").Error);
        Assert.Equal(0, deleted.FailedLoginAttempts);
    }

    [Fact]
    public void UnlockAccount_ShouldRequireAnExistingLockout()
    {
        var user = CreateUser();
        Assert.Equal(UserErrors.SecurityErrors.AccountNotLocked, user.UnlockAccount().Error);
    }

    [Fact]
    public void UnlockAccount_ShouldClearLockoutAndEmitEvent()
    {
        var user = CreateUser();
        for (var attempt = 0; attempt < 5; attempt++) user.RecordFailedLogin("127.0.0.1");

        Assert.True(user.UnlockAccount().IsSuccess);
        Assert.Equal(0, user.FailedLoginAttempts);
        Assert.Null(user.LockedUntil);
        Assert.Contains(user.DomainEvents, item => item is UserAccountUnlockedDomainEvent);
    }

    [Fact]
    public void TwoFactor_ShouldNormalizeSecret_AndRequireEnabledStateToDisable()
    {
        var user = CreateUser();
        const string secret = "abcdefghijklmnopqrstuvwxyz234567";
        Assert.True(user.EnableTwoFactor(secret).IsSuccess);
        Assert.Equal(secret.ToUpperInvariant(), user.TwoFactorSecret);
        Assert.True(user.DisableTwoFactor().IsSuccess);
        Assert.Null(user.TwoFactorSecret);
        Assert.False(user.IsTwoFactorEnabled);
    }

    [Fact]
    public void TwoFactor_ShouldRejectInvalidSecretAndDuplicateEnable()
    {
        var user = CreateUser();
        Assert.Equal(UserErrors.SecurityErrors.InvalidTwoFactorSecret, user.EnableTwoFactor("bad secret").Error);
        Assert.True(user.EnableTwoFactor("abcdefghijklmnopqrstuvwxyz234567").IsSuccess);
        Assert.Equal(UserErrors.SecurityErrors.TwoFactorAlreadyEnabled,
            user.EnableTwoFactor("abcdefghijklmnopqrstuvwxyz234567").Error);
    }

    [Fact]
    public void DisableTwoFactor_WhenNotEnabled_ShouldFailWithoutEvent()
    {
        var user = CreateUser();
        user.ClearDomainEvents();

        Assert.Equal(UserErrors.SecurityErrors.TwoFactorNotEnabled, user.DisableTwoFactor().Error);
        Assert.Empty(user.DomainEvents);
    }

    [Fact]
    public void RecordPasswordChange_ShouldRejectEmptyHashesAndReusedPasswords()
    {
        var user = CreateUser();
        Assert.Equal(UserErrors.SecurityErrors.InvalidPasswordHash, user.RecordPasswordChange(" ", false).Error);
        Assert.Equal(UserErrors.SecurityErrors.PasswordAlreadyUsed, user.RecordPasswordChange("hash", true).Error);
        Assert.Empty(user.PasswordHashes);
    }

    [Fact]
    public void RecordPasswordChange_ShouldRetainOnlyConfiguredHistoryDepth()
    {
        var user = CreateUser();
        for (var index = 1; index <= 7; index++)
            Assert.True(user.RecordPasswordChange($"hash-{index}", false).IsSuccess);

        Assert.Equal(5, user.PasswordHashes.Count);
        Assert.Equal("hash-3", user.PasswordHashes[0]);
        Assert.Equal("hash-7", user.PasswordHashes[^1]);
    }
}
