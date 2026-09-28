using SampleRestaurantTDD.Application.Ports;
using SampleRestaurantTDD.Domain;

namespace SampleRestaurantTDD.Infrastructure;

public sealed class InMemoryMenuCatalog : IMenuCatalog
{
    private static readonly IReadOnlyList<MenuItem> Menu = Array.AsReadOnly(new[]
    {
        new MenuItem(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Cheeseburger", 20m, MenuCategory.Burger),
        new MenuItem(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Batata frita", 10m, MenuCategory.Side),
        new MenuItem(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Refrigerante", 8m, MenuCategory.Drink),
        new MenuItem(Guid.Parse("44444444-4444-4444-4444-444444444444"), "Sorvete", 6m, MenuCategory.Dessert, false)
    });

    public Task<MenuItem?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(Menu.FirstOrDefault(item => item.Id == id));
    }

    public Task<IReadOnlyList<MenuItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(Menu);
    }
}
