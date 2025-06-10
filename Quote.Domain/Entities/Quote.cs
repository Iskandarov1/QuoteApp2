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

    public Quote( Author author, Textt textt, Category category) : base()
    {
        Author = author;
        Textt = textt;
        Category = category;
    }
    
     public Author Author { get; private set; }
     public Textt Textt { get; private set; }
      public Category Category { get; private set; }

     public void Update(Author author, Textt textt, Category category)
     {
         Author = author;
         Textt = textt;
         Category = category;
         
         AddDomainEvent(new QuoteUpdatedDomainEvent(Id));
     }

     public static Quote Create(Author author, Textt textt, Category category) =>
         new(author, textt, category);
}

