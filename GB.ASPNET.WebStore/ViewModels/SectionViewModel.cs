namespace GB.ASPNET.WebStore.ViewModels;

public class SectionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Order { get; set; }

    public List<SectionViewModel> Children { get; set; } = new List<SectionViewModel>();
}
