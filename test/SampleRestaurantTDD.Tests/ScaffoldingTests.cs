using SampleRestaurantTDD.Domain;
using SampleRestaurantTDD.Infrastructure;

namespace SampleRestaurantTDD.Tests;

public sealed class ScaffoldingTests
{
    [Fact]
    public void Novo_pedido_deve_iniciar_vazio_e_em_rascunho()
    {
        var id = Guid.NewGuid();
        var order = new Order(id);
        Assert.Equal(id, order.Id);
        Assert.Equal(OrderStatus.Draft, order.Status);
        Assert.Empty(order.Items);
        Assert.Equal(0m, order.DiscountPercentage);
    }

    [Fact]
    public async Task Repositorio_deve_salvar_e_buscar_pedido()
    {
        var repository = new InMemoryOrderRepository();
        var order = new Order(Guid.NewGuid());
        Assert.Null(await repository.FindAsync(order.Id));
        await repository.SaveAsync(order);
        Assert.Same(order, await repository.FindAsync(order.Id));
    }

    [Fact]
    public async Task Cardapio_deve_permitir_consulta_por_identificador()
    {
        var menu = new InMemoryMenuCatalog();
        var items = await menu.ListAsync();
        Assert.Equal(4, items.Count);
        Assert.Contains(items, item => !item.IsAvailable);
        foreach (var item in items)
            Assert.Equal(item, await menu.FindAsync(item.Id));
        Assert.Null(await menu.FindAsync(Guid.NewGuid()));
    }
}
