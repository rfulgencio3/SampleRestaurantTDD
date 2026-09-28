using SampleRestaurantTDD.Domain;

namespace SampleRestaurantTDD.Application.Ports;

public interface IOrderRepository
{
    Task<Order?> FindAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveAsync(Order order, CancellationToken cancellationToken = default);
}
