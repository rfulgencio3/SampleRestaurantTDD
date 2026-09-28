namespace SampleRestaurantTDD.Domain;

public enum MenuCategory { Burger, Side, Drink, Dessert }

public sealed record MenuItem(Guid Id, string Name, decimal Price, MenuCategory Category, bool IsAvailable = true);
