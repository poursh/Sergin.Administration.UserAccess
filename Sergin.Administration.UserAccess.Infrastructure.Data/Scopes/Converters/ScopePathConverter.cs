using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Infrastructure.Data.Scopes.Converters;

internal sealed class ScopePathConverter : ValueConverter<ScopePath, string>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public ScopePathConverter() : this(null)
    {
    }

    public ScopePathConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new ScopePath(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
