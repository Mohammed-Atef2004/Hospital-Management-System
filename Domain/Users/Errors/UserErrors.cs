using  Domain.SharedKernel;
using  Domain.Users.ValueObjects;

namespace  Domain.Users.Errors;

public static class UserErrors
{
    public static readonly Error NotFound =
        new("User.NotFound", "The requested user does not exist.");

    public static readonly Error AlreadyExists =
        new("User.AlreadyExists", "A user with this email address already exists.");

    public static readonly Error Deactivated =
        new("User.Deactivated", "This user account has been deactivated.");

    public static readonly Error AlreadyDeleted =
        new("User.AlreadyDeleted", "This user account has already been deleted.");

    public static readonly Error AlreadyActive =
        new("User.AlreadyActive", "This user account is already active.");

    public static readonly Error InvalidIdentityReference =
        new("User.InvalidIdentityReference", "The provided identity ID is required.");

    public static readonly Error InvalidRole =
        new("User.InvalidRole", "The requested user role is invalid.");

    public static readonly Error InvalidReason =
        new("User.InvalidReason", "A reason is required for this operation.");

    public static readonly Error CannotDeleteSuperAdmin =
        new("User.CannotDeleteSuperAdmin", "SuperAdmin accounts cannot be deleted.");

    public static readonly Error CannotDeactivateSuperAdmin =
        new("User.CannotDeactivateSuperAdmin", "SuperAdmin accounts cannot be deactivated.");

    public static readonly Error RoleAlreadyAssigned =
        new("User.RoleAlreadyAssigned", "The user already has this role.");

    public static readonly Error CannotAssignSuperAdminRole =
        new("User.CannotAssignSuperAdminRole", "SuperAdmin role cannot be assigned through normal operations.");

    public static readonly Error StaffProfileAlreadyLinked =
        new("User.StaffProfileAlreadyLinked", "This account is already linked to a staff profile.");

    public static readonly Error StaffRoleMismatch =
        new("User.StaffRoleMismatch", "The account role does not match the selected staff profile type.");

    public static readonly Error StaffProfileInactive =
        new("User.StaffProfileInactive", "An inactive staff profile cannot be linked to an account.");

    

   

    
    public static class SecurityErrors
    {
        public static readonly Error AccountLocked =
            new("User.Security.AccountLocked",
                "Account is temporarily locked due to too many failed login attempts.");

        public static readonly Error TwoFactorAlreadyEnabled =
            new("User.Security.TwoFactorAlreadyEnabled", "Two-factor authentication is already enabled.");

        public static readonly Error TwoFactorNotEnabled =
            new("User.Security.TwoFactorNotEnabled", "Two-factor authentication is not enabled.");

        public static readonly Error InvalidTwoFactorSecret =
            new("User.Security.InvalidTwoFactorSecret", "Two-factor secret must be a valid Base32 secret.");

        public static readonly Error AccountNotLocked =
            new("User.Security.AccountNotLocked", "The account is not locked.");

        public static readonly Error InvalidPasswordHash =
            new("User.Security.InvalidPasswordHash", "A password hash is required.");

        public static readonly Error PasswordAlreadyUsed =
            new("User.Security.PasswordAlreadyUsed",
                "This password was used recently. Please choose a different password.");
    }

}
