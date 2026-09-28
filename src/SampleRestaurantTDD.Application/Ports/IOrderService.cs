using SampleRestaurantTDD.Domain;

namespace SampleRestaurantTDD.Application.Ports;

// Porta de entrada: pode ser acionada por HTTP, CLI ou testes.
public interface IOrderService
{
    Task<Order> CreateAsync(CancellationToken cancellationToken = default);
    Task AddItemAsync(Guid orderId, Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task<Order> CheckoutAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<Order> GetAsync(Guid orderId, CancellationToken cancellationToken = default);
}
