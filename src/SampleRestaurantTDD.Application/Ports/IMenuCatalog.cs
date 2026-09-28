using SampleRestaurantTDD.Domain;

namespace SampleRestaurantTDD.Application.Ports;

public interface IMenuCatalog
{
    Task<MenuItem?> FindAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MenuItem>> ListAsync(CancellationToken cancellationToken = default);
}
