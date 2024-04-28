namespace BlazorCms.BuildingBlocks.Abstractions;

public class ComponentConfiguration<TComponent>
{
    public IList<ComponentConfigurationItem<TComponent>>? Components { get; set; }
}

public class ComponentConfigurationItem<TComponent>
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool Disabled { get; set; }
    public TComponent? Component { get; set; }
}

public interface IComponentConfigurationProvider<TComponent>
{
    public Task<IList<ComponentConfigurationItem<TComponent>>?> FindByIdAsync(string id);
    public Task<IList<ComponentConfigurationItem<TComponent>>?> FindByNameAsync(string name);
}
