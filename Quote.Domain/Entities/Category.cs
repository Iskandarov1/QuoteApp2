using System.ComponentModel.DataAnnotations.Schema;
using Quote.Domain.Core.Primitives;

namespace Quote.Domain.Entities;

public sealed class Category : AggregateRoot
{
    private Category(){}

    public Category(string name)
    {
        this.Name = name;
    }
    [Column("category_name")]
    public string Name { get; set; }

    public Category Update(string name)
    {
        this.Name = name;

        return this;
    }
}