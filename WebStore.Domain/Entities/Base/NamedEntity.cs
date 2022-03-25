using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GB.ASPNET.WebStore.Domain.Entities.Base.Interfaces;

namespace GB.ASPNET.WebStore.Domain.Entities.Base;

public abstract class NamedEntity : Entity, INamedEntity
{
    [Required]
    public string Name { get; set; } = null!;
}