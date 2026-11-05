using Sergin.SharedKernel.Domain.Localizations;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.UserAccess.Application.Configurations.Roles;

namespace Sergin.UserAccess.Application.Roles;

/// <summary>
/// BR-1, AC-4: a user may put a permission on a role only if they hold it themselves in the role's owner scope.
/// A validation error rather than a 403, so the caller learns which permission was refused.
/// </summary>
internal static class PermissionCeiling
{
    public static Error Exceeded(Permission permission) =>
        Error.Validation(
            "Permissions",
            $"You can only grant a permission you hold in the role's owner scope; you do not hold '{permission.Value}' there.",
            ErrorText.Of(
                RoleTexts.PermissionCeilingExceeded.Key.Value,
                new Dictionary<string, object?>(StringComparer.Ordinal) { ["Permission"] = permission.Value }));
}
