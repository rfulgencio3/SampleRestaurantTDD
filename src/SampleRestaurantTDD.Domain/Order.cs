namespace SampleRestaurantTDD.Domain;

public enum OrderStatus { Draft, Confirmed, Cancelled }

public sealed class Order
{
    private readonly List<OrderItem> _items = [];

    public Order(Guid id) => Id = id;

    public Guid Id { get; }
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public decimal DiscountPercentage { get; private set; }

    // TDD 01: adicionar produto disponível; agrupar itens do mesmo produto.
    public void AddItem(MenuItem product, int quantity) =>
        throw new NotImplementedException("TDD 01: adicionar item ao pedido.");

    // TDD 02: remover uma linha inteira do pedido.
    public void RemoveItem(Guid productId) =>
        throw new NotImplementedException("TDD 02: remover item do pedido.");

    // TDD 03: somar os itens, aplicar desconto e arredondar para centavos.
    public decimal CalculateTotal() =>
        throw new NotImplementedException("TDD 03: calcular total.");

    // TDD 04: definir desconto percentual, sem acumular descontos.
    public void ApplyDiscount(decimal percentage) =>
        throw new NotImplementedException("TDD 04: aplicar desconto.");

    // TDD 05: confirmar um pedido não vazio.
    public void Confirm() =>
        throw new NotImplementedException("TDD 05: confirmar pedido.");

    // TDD 06: cancelar somente um pedido em rascunho.
    public void Cancel() =>
        throw new NotImplementedException("TDD 06: cancelar pedido.");
}
