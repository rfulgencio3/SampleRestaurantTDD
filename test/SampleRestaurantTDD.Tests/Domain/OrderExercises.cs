using SampleRestaurantTDD.Domain;

namespace SampleRestaurantTDD.Tests.Domain;

public sealed class OrderExercises
{
    private static MenuItem Burger => new(Guid.NewGuid(), "Cheeseburger", 20m, MenuCategory.Burger);

    [Fact(Skip = "TDD 01: remova o Skip para começar")]
    public void Adicionar_item_deve_guardar_produto_e_quantidade()
    {
        var order = new Order(Guid.NewGuid());
        var product = Burger;
        order.AddItem(product, 2);
        var item = Assert.Single(order.Items);
        Assert.Equal(product.Id, item.ProductId);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(20m, item.UnitPrice);
    }

    [Fact(Skip = "TDD 02: depende do exercício 01")]
    public void Remover_item_deve_excluir_a_linha_inteira()
    {
        var order = new Order(Guid.NewGuid());
        var product = Burger;
        order.AddItem(product, 2);
        order.RemoveItem(product.Id);
        Assert.Empty(order.Items);
    }

    [Fact(Skip = "TDD 03: depende do exercício 01")]
    public void Total_deve_somar_preco_vezes_quantidade()
    {
        var order = new Order(Guid.NewGuid());
        order.AddItem(Burger, 2);
        order.AddItem(new MenuItem(Guid.NewGuid(), "Batata", 10m, MenuCategory.Side), 1);
        Assert.Equal(50m, order.CalculateTotal());
    }

    [Fact(Skip = "TDD 04: depende dos exercícios 01 e 03")]
    public void Desconto_de_dez_por_cento_deve_reduzir_total()
    {
        var order = new Order(Guid.NewGuid());
        order.AddItem(Burger, 2);
        order.ApplyDiscount(10m);
        Assert.Equal(36m, order.CalculateTotal());
    }

    [Fact(Skip = "TDD 05: depende do exercício 01")]
    public void Confirmar_pedido_com_item_deve_alterar_status()
    {
        var order = new Order(Guid.NewGuid());
        order.AddItem(Burger, 1);
        order.Confirm();
        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Fact(Skip = "TDD 06: remova o Skip para começar")]
    public void Cancelar_rascunho_deve_alterar_status()
    {
        var order = new Order(Guid.NewGuid());
        order.Cancel();
        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }
}
