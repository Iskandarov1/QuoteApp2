using System.ComponentModel.DataAnnotations.Schema;
using Quote.Domain.Core.Abstractions;
using Quote.Domain.Core.Errors;
using Quote.Domain.Core.Events;
using Quote.Domain.Core.Primitives;
using Quote.Domain.Events.Quotes;
using Quote.Domain.ValueObjects;


namespace Quote.Domain.Entities;


public sealed class Quote : AggregateRoot
{
    private Quote() {}

    public Quote(Guid id, Author author, Textt textt, Category category) : base(id)
    {
        Author = author;
        Textt = textt;
        Category = category;
    }
    
     [Column("author_id")] public Author Author { get; private set; }
     [Column("quote_text", TypeName = "nvarchar(400)")] public Textt Textt { get; private set; }
     [Column("category_id")] public Category Category { get; private set; }

     public void Update(Author author, Textt textt, Category category)
     {
         Author = author;
         Textt = textt;
         Category = category;
         
         AddDomainEvent(new QuoteUpdatedDomainEvent(Id));
     }

     public static Quote Create(Author author, Textt textt, Category category) =>
         new(Guid.NewGuid(), author, textt, category);
}

