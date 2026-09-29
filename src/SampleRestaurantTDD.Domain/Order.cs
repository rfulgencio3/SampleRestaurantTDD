namespace SampleRestaurantTDD.Domain;

public sealed class Order
{
    private readonly List<OrderItem> _items = [];

    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public bool IsConfirmed { get; private set; }

    // Etapa 1: incluir um item com preço e quantidade positivos.
    public void AddItem(string product, decimal price, int quantity) =>
        throw new NotImplementedException();

    // Etapa 2: somar preço × quantidade de todas as linhas.
    public decimal CalculateTotal() =>
        throw new NotImplementedException();

    // Etapa 3: confirmar um pedido não vazio e impedir novas inclusões.
    public void Confirm() =>
        throw new NotImplementedException();
}
