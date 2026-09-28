namespace SampleRestaurantTDD.Domain;

public sealed record OrderItem(string Name, decimal UnitPrice, int Quantity);
