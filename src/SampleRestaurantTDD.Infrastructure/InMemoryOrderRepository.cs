using System.Collections.Concurrent;
using SampleRestaurantTDD.Application.Ports;
using SampleRestaurantTDD.Domain;

namespace SampleRestaurantTDD.Infrastructure;

// Adaptador didático: mantém referências dos agregados somente durante a execução.
public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Task<Order?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_orders.GetValueOrDefault(id));
    }

    public Task SaveAsync(Order order, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }
}
