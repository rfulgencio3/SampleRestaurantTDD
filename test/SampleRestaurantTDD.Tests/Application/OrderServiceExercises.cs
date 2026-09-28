using SampleRestaurantTDD.Application;
using SampleRestaurantTDD.Application.Ports;
using SampleRestaurantTDD.Domain;
using SampleRestaurantTDD.Infrastructure;

namespace SampleRestaurantTDD.Tests.Application;

public sealed class OrderServiceExercises
{
    private readonly RecordingRepository _orders = new();
    private readonly InMemoryMenuCatalog _menu = new();
    private OrderService Service => new(_orders, _menu);

    [Fact(Skip = "TDD 07: remova o Skip para começar")]
    public async Task Criar_pedido_deve_salvar_rascunho_com_identificador()
    {
        var order = await Service.CreateAsync();
        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(OrderStatus.Draft, order.Status);
        Assert.Empty(order.Items);
        Assert.Same(order, _orders.Saved);
        Assert.Equal(1, _orders.SaveCount);
    }

    [Fact(Skip = "TDD 08: depende do exercício 01")]
    public async Task Adicionar_item_deve_buscar_produto_e_salvar_pedido()
    {
        var order = new Order(Guid.NewGuid());
        _orders.Existing = order;
        var product = (await _menu.ListAsync())[0];
        await Service.AddItemAsync(order.Id, product.Id, 2);
        var item = Assert.Single(order.Items);
        Assert.Equal(product.Id, item.ProductId);
        Assert.Equal(2, item.Quantity);
        Assert.Same(order, _orders.Saved);
        Assert.Equal(1, _orders.SaveCount);
    }

    [Fact(Skip = "TDD 09: depende dos exercícios 01 e 05")]
    public async Task Finalizar_deve_confirmar_e_salvar_pedido()
    {
        var order = new Order(Guid.NewGuid());
        order.AddItem((await _menu.ListAsync())[0], 1);
        _orders.Existing = order;
        var result = await Service.CheckoutAsync(order.Id);
        Assert.Same(order, result);
        Assert.Equal(OrderStatus.Confirmed, result.Status);
        Assert.Same(order, _orders.Saved);
        Assert.Equal(1, _orders.SaveCount);
    }

    [Fact(Skip = "TDD 10: remova o Skip para começar")]
    public async Task Consultar_pedido_inexistente_deve_informar_ausencia()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() => Service.GetAsync(Guid.NewGuid()));
        Assert.Equal(0, _orders.SaveCount);
    }

    // Spy: comprova SaveAsync explicitamente, sem depender de referências em memória.
    private sealed class RecordingRepository : IOrderRepository
    {
        public Order? Existing { get; set; }
        public Order? Saved { get; private set; }
        public int SaveCount { get; private set; }

        public Task<Order?> FindAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(Existing?.Id == id ? Existing : null);

        public Task SaveAsync(Order order, CancellationToken cancellationToken = default)
        {
            Saved = order;
            SaveCount++;
            return Task.CompletedTask;
        }
    }
}
