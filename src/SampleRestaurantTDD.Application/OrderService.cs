using SampleRestaurantTDD.Application.Ports;
using SampleRestaurantTDD.Domain;

namespace SampleRestaurantTDD.Application;

public sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _orders;
    private readonly IMenuCatalog _menu;

    public OrderService(IOrderRepository orders, IMenuCatalog menu)
    {
        _orders = orders;
        _menu = menu;
    }

    // TDD 07: gerar identificador e persistir pedido em rascunho.
    public Task<Order> CreateAsync(CancellationToken cancellationToken = default) =>
        throw new NotImplementedException("TDD 07: abrir pedido.");

    // TDD 08: buscar pedido/produto, delegar ao domínio e persistir.
    public Task AddItemAsync(Guid orderId, Guid productId, int quantity, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException("TDD 08: adicionar item pelo caso de uso.");

    // TDD 09: confirmar pelo domínio e persistir; pagamento fora deste exercício.
    public Task<Order> CheckoutAsync(Guid orderId, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException("TDD 09: finalizar pedido.");

    // TDD 10: consultar pedido ou lançar KeyNotFoundException.
    public Task<Order> GetAsync(Guid orderId, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException("TDD 10: consultar pedido.");
}
