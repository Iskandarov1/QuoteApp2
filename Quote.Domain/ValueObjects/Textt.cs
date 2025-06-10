using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Domain.ValueObjects
{
    /// <summary>
    /// Represents the first name value object.
    /// </summary>
    public sealed class Textt : ValueObject
    {
        /// <summary>
        /// The first name maximum length.
        /// </summary>
        public const int MaxLength = 200;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstName"/> class.
        /// </summary>
        /// <param name="value">The first name value.</param>
        private Textt(string value) => Value = value;

        /// <summary>
        /// Gets the first name value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(Textt category) => category.Value;

        /// <summary>
        /// Creates a new <see cref="FirstName"/> instance based on the specified value.
        /// </summary>
        /// <param name="firstName">The first name value.</param>
        /// <returns>The result of the first name creation process containing the first name or an error.</returns>
        public static Result<Textt> Create(string firstName) =>
            Result.Create(firstName, DomainErrors.Quote.Textt.NullOrEmpty)
                .Ensure(f => !string.IsNullOrWhiteSpace(f), DomainErrors.Quote.Textt.NullOrEmpty)
                .Ensure(f => f.Length <= MaxLength, DomainErrors.Quote.Textt.LongerThanAllowed)
                .Map(f => new Textt(f));

        /// <inheritdoc />
        public override string ToString() => Value;

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}