using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Infrastructure.Data.Roles.Converters;

internal sealed class RoleNameConverter : ValueConverter<RoleName, string>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public RoleNameConverter() : this(null)
    {
    }

    public RoleNameConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new RoleName(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
