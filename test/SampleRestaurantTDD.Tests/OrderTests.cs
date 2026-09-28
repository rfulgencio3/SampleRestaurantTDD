using SampleRestaurantTDD.Domain;

namespace SampleRestaurantTDD.Tests;

public sealed class OrderTests
{
    [Fact]
    public void Novo_pedido_deve_estar_vazio_e_nao_confirmado()
    {
        var order = new Order();

        Assert.Empty(order.Items);
        Assert.False(order.IsConfirmed);
    }
}
