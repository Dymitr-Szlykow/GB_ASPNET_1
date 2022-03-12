using GB.ASPNET.WebStore.Domain.Entities.Base.Interfaces;

namespace GB.ASPNET.WebStore.Domain.Entities.Base;

public abstract class NamedEntity : Entity, INamedEntity
{
    public string Name { get; set; } = null!;
}