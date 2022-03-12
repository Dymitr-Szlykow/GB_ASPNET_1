using GB.ASPNET.WebStore.Domain.Entities.Base.Interfaces;

namespace GB.ASPNET.WebStore.Domain.Entities.Base;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
}