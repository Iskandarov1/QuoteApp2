using System.ComponentModel.DataAnnotations.Schema;
using Quote.Domain.Core.Primitives;

namespace Quote.Domain.Entities;
public sealed class Quote : AggregateRoot
{
    private Quote() {}

    public Quote( string author, string textt, Guid category) : base()
    {
        this.Author = author;
        this.Textt = textt;
        this.CategoryId = category;
    }
    
    [Column("author")] public string Author { get; private set; }
    [Column("quote_text")] public string Textt { get; private set; }
    [Column("category_id")] public Guid CategoryId { get; private set; }
    
    public Category Category { get; set; }
     public Quote Update(string author, string textt, Guid category)
     {
         this.Author = author;
         this.Textt = textt;
         this.CategoryId = category;
         
         return this;
     }
     
}

