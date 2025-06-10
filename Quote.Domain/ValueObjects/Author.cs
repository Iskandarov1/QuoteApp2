using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Core.Primitives.Result;

namespace Quote.Domain.ValueObjects
{
    /// <summary>
    /// Represents the first name value object.
    /// </summary>
    public sealed class Author : ValueObject
    {
        /// <summary>
        /// The first name maximum length.
        /// </summary>
        public const int MaxLength = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstName"/> class.
        /// </summary>
        /// <param name="value">The first name value.</param>
        private Author(string value) => Value = value;

        /// <summary>
        /// Gets the first name value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(Author author) => author.Value;

        /// <summary>
        /// Creates a new <see cref="FirstName"/> instance based on the specified value.
        /// </summary>
        /// <param name="firstName">The first name value.</param>
        /// <returns>The result of the first name creation process containing the first name or an error.</returns>
        public static Result<Author> Create(string author) =>
            Result.Create(author, DomainErrors.Quote.Author.NullOrEmpty)
                .Ensure(f => !string.IsNullOrWhiteSpace(f), DomainErrors.Quote.Author.NullOrEmpty)
                .Ensure(f => f.Length <= MaxLength, DomainErrors.Quote.Author.LongerThanAllowed)
                .Map(f => new Author(f));

        /// <inheritdoc />
        public override string ToString() => Value;

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}