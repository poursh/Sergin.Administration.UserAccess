using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Infrastructure.Data.Scopes.Converters;

internal sealed class ScopeIdConverter : ValueConverter<ScopeId, Guid>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public ScopeIdConverter() : this(null)
    {
    }

    public ScopeIdConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new ScopeId(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
