using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using GB.ASPNET.WebStore.Domain.Entities.Base;
using GB.ASPNET.WebStore.Domain.Entities.Base.Interfaces;

namespace GB.ASPNET.WebStore.Domain.Entities;

[Index(nameof(Name), IsUnique = false)]
public class Product : NamedEntity, IOrderedEntity
{
    public int Order { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public string ImageUrl { get; set; } = null!;


    [ForeignKey(nameof(SectionId))]
    [Required]
    public Section Section { get; set; } = null!;
    public int SectionId { get; set; }


    [ForeignKey(nameof(BrandId))]
    public Brand? Brand { get; set; }
    public int? BrandId { get; set; }
}