using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Infrastructure.Data.Roles.Converters;

internal sealed class RoleIdConverter : ValueConverter<RoleId, Guid>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public RoleIdConverter() : this(null)
    {
    }

    public RoleIdConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new RoleId(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
