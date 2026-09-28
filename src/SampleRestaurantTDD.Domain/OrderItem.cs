namespace SampleRestaurantTDD.Domain;

// Snapshot do produto: alterações futuras no cardápio não alteram o pedido.
public sealed record OrderItem(Guid ProductId, string Name, decimal UnitPrice, int Quantity);
