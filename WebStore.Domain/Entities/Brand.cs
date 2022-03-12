using GB.ASPNET.WebStore.Domain.Entities.Base;
using GB.ASPNET.WebStore.Domain.Entities.Base.Interfaces;

namespace GB.ASPNET.WebStore.Domain.Entities;

public class Brand : NamedEntity, IOrderedEntity
{
    public int Order { get; set; }
}