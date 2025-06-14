using Quote.Application.Core.Abstractions.Common;

namespace Quote.Persistence;

public sealed class SystemDateTime : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}