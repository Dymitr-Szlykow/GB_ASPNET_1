using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.Domain.Entities.Base;
using GB.ASPNET.WebStore.Domain.Entities.Base.Interfaces;

namespace GB.ASPNET.WebStore.Domain.Entities;

[Index(nameof(Name), IsUnique = false)]
public class Section : NamedEntity, IOrderedEntity
{
    public int Order { get; set; }

    [ForeignKey(nameof(ParentId))]
    public Section? Parent { get; set; }
    public int? ParentId { get; set; }

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}