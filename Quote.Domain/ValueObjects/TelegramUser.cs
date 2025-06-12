using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Localizations;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Core.Primitives.Result;

public sealed class TelegramUser : ValueObject
{
    private TelegramUser(long? value) => Value = value;

    public long? Value { get; }

    // 1. keep the implicit long→value conversion
    public static implicit operator long?(TelegramUser telegramUser) => telegramUser.Value;

    // 2. optional explicit long→TelegramUser shown above

    // 3. validation factory (change parameter type to long or string-parse long)
    public static Result<TelegramUser> Create(long? value, string field, ISharedViewLocalizer sharedViewLocalizer) =>
        Result.Create(value, new Error(CaseConverter.PascalToSnakeCase(field),
                    sharedViewLocalizer[DomainErrors.Item.NullOrEmptyError]))
            .Map(v => new TelegramUser(v));

    // 4. ToString must return string, not long
    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}