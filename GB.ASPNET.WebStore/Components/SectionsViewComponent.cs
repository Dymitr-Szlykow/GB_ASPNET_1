using Microsoft.AspNetCore.Mvc;
using GB.ASPNET.WebStore.Domain.Entities;
using GB.ASPNET.WebStore.Services.Interfaces;
using GB.ASPNET.WebStore.ViewModels;

namespace GB.ASPNET.WebStore.Components;

//[ViewComponent("SectionsVC")]
public class SectionsViewComponent : ViewComponent
{
    private readonly IProductData _data;

    public SectionsViewComponent(IProductData data) => _data = data;

    public IViewComponentResult Invoke()
    {
        IEnumerable<Section> sections = _data.GetSections();
        IEnumerable<Section> parentSections = sections.Where(sec => sec.ParentId is null);

        var parentSectionsVM = parentSections
            .Select(sec => new SectionViewModel
            {
                Id = sec.Id,
                Name = sec.Name,
                Order = sec.Order,
            })
            .ToList();

        foreach (SectionViewModel parentVM in parentSectionsVM)
        {
            IEnumerable<Section> children = sections.Where(sec => sec.ParentId == parentVM.Id);
            foreach (var child in children)
                parentVM.Children.Add(new()
                {
                    Id = child.Id,
                    Name = child.Name,
                    Order = child.Order,
                });

            parentVM.Children.Sort((a,b) => Comparer<int>.Default.Compare(a.Order, b.Order));
        }

        parentSectionsVM.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
        return View(parentSectionsVM);
    }

    //public Task<IViewComponentResult> InvokeAsync()
    //{
    //    //
    //}
}
