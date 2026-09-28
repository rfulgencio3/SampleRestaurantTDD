using System.Text.Json.Serialization;
using SampleRestaurantTDD.Application;
using SampleRestaurantTDD.Application.Ports;
using SampleRestaurantTDD.Domain;
using SampleRestaurantTDD.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<IMenuCatalog, InMemoryMenuCatalog>();
builder.Services.AddScoped<IOrderService, OrderService>();
var app = builder.Build();

app.Use(async (context, next) =>
{
    try { await next(context); }
    catch (Exception exception) when (exception is DomainException or KeyNotFoundException or NotImplementedException)
    {
        var status = exception switch
        {
            DomainException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status501NotImplemented
        };
        await Results.Problem(statusCode: status, title: exception.Message).ExecuteAsync(context);
    }
});

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapGet("/menu", (IMenuCatalog menu, CancellationToken ct) => menu.ListAsync(ct));
app.MapPost("/orders", async (IOrderService service, CancellationToken ct) =>
{
    var order = await service.CreateAsync(ct);
    return Results.Created($"/orders/{order.Id}", order);
});
app.MapGet("/orders/{id:guid}", (Guid id, IOrderService service, CancellationToken ct) => service.GetAsync(id, ct));
app.MapPost("/orders/{id:guid}/items", async (Guid id, AddItemRequest request, IOrderService service, CancellationToken ct) =>
{
    await service.AddItemAsync(id, request.ProductId, request.Quantity, ct);
    return Results.NoContent();
});
app.MapPost("/orders/{id:guid}/checkout", (Guid id, IOrderService service, CancellationToken ct) => service.CheckoutAsync(id, ct));
app.Run();

public sealed record AddItemRequest(Guid ProductId, int Quantity);
