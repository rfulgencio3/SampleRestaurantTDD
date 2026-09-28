# Live-coding: TDD com o mínimo de código

## Preparação

Abra `src/SampleRestaurantTDD.Domain/Order.cs` e `test/SampleRestaurantTDD.Tests/OrderTests.cs` lado a lado.

Na raiz do repositório:

```powershell
dotnet restore SampleRestaurantTDD.sln
dotnet test SampleRestaurantTDD.sln
```

Deve existir **um teste passando**, que verifica o estado inicial do pedido. Os três métodos de negócio ainda lançam `NotImplementedException`. Não há API ou banco para iniciar: os testes executam o domínio diretamente.

Escolha o tamanho da aula:

| Tempo sugerido | Conteúdo |
| --- | --- |
| 20–30 minutos | Etapa 1: adicionar item |
| 35–45 minutos | Etapas 1 e 2: adicionar e calcular |
| 50–60 minutos | As três etapas, incluindo confirmação |

As etapas são cumulativas. Os tempos são estimativas; priorize discutir a falha observada e a regra de negócio.

## Como conduzir cada ciclo

1. Discuta um exemplo concreto com a turma.
2. Adicione **somente o próximo teste** dentro da classe `OrderTests` existente.
3. Rode `dotnet test SampleRestaurantTDD.sln` e confira o motivo da falha (**Red**). Uma falha de compilação não substitui a verificação do comportamento.
4. Altere apenas o necessário no método correspondente para passar (**Green**).
5. Execute a suíte, melhore nomes ou elimine duplicação mantendo tudo verde (**Refactor**).
6. Só então siga para o próximo teste. Preserve os anteriores.

Os blocos abaixo devem ser escritos durante a aula. As implementações finais não estão prontas no projeto.

## Etapa 1 — Adicionar item

**Regra:** cada chamada adiciona uma linha com nome não vazio, preço positivo e quantidade positiva. O mesmo produto pode ocupar linhas diferentes; não há agrupamento nesta aula.

### Ciclo 1: caminho feliz

Adicione este teste:

```csharp
[Fact]
public void Adicionar_item_deve_guardar_nome_preco_e_quantidade()
{
    var order = new Order();

    order.AddItem("Hambúrguer", 20m, 2);

    var item = Assert.Single(order.Items);
    Assert.Equal("Hambúrguer", item.Name);
    Assert.Equal(20m, item.UnitPrice);
    Assert.Equal(2, item.Quantity);
}
```

**Red esperado:** `NotImplementedException` em `AddItem`.

**Green:** substitua o corpo pendente de `AddItem` por uma inclusão de `new OrderItem(name, unitPrice, quantity)` em `_items`. Não implemente os outros métodos.

### Ciclo 2: quantidade inválida

```csharp
[Theory]
[InlineData(0)]
[InlineData(-1)]
public void Adicionar_item_com_quantidade_invalida_deve_falhar(int quantity)
{
    var order = new Order();

    Assert.Throws<ArgumentOutOfRangeException>(() =>
        order.AddItem("Hambúrguer", 20m, quantity));
    Assert.Empty(order.Items);
}
```

**Red esperado:** nenhuma exceção foi lançada.

**Green:** valide `quantity <= 0` antes de modificar a lista e lance `ArgumentOutOfRangeException(nameof(quantity))`.

### Ciclos 3 e 4: preço e nome inválidos

Acrescente um bloco por vez, rodando os testes antes de implementar cada regra:

```csharp
[Theory]
[InlineData(0)]
[InlineData(-1)]
public void Adicionar_item_com_preco_invalido_deve_falhar(int price)
{
    var order = new Order();

    Assert.Throws<ArgumentOutOfRangeException>(() =>
        order.AddItem("Hambúrguer", price, 1));
    Assert.Empty(order.Items);
}
```

**Green:** valide `unitPrice <= 0` antes da inclusão e lance `ArgumentOutOfRangeException(nameof(unitPrice))`. O teste usa inteiros convertidos para `decimal`; os preços do domínio continuam sendo `decimal`.

```csharp
[Theory]
[InlineData("")]
[InlineData(" ")]
public void Adicionar_item_sem_nome_deve_falhar(string name)
{
    var order = new Order();

    Assert.Throws<ArgumentException>(() => order.AddItem(name, 20m, 1));
    Assert.Empty(order.Items);
}
```

**Green:** use `string.IsNullOrWhiteSpace(name)` e lance `ArgumentException` antes da inclusão.

**Refactor:** mantenha as validações no início do método e preserve a lista privada. Não crie interfaces ou serviços para uma regra que cabe nesta classe.

**Ponto de parada:** já é possível encerrar a aula com uma implementação guiada por TDD. Os outros dois métodos podem continuar pendentes.

## Etapa 2 — Calcular total

**Regra:** total é a soma de preço unitário × quantidade de todas as linhas. Pedido vazio vale zero. Use `decimal`; não há desconto nem regra adicional de arredondamento neste exercício.

### Ciclo 1: pedido vazio

```csharp
[Fact]
public void Pedido_vazio_deve_ter_total_zero()
{
    var order = new Order();

    Assert.Equal(0m, order.CalculateTotal());
}
```

**Red esperado:** `NotImplementedException` em `CalculateTotal`.

**Green:** retorne `0m`. Discuta por que isso atende apenas o exemplo atual.

### Ciclo 2: preço multiplicado pela quantidade

```csharp
[Fact]
public void Total_deve_multiplicar_preco_pela_quantidade()
{
    var order = new Order();
    order.AddItem("Hambúrguer", 20m, 2);

    Assert.Equal(40m, order.CalculateTotal());
}
```

**Red esperado:** esperado 40, obtido 0.

**Green:** calcule o total a partir dos itens. Um `foreach` acumulando `UnitPrice * Quantity` é suficiente e também atende pedido vazio.

### Ciclo 3: itens diferentes e centavos

```csharp
[Fact]
public void Total_deve_somar_todas_as_linhas_com_centavos()
{
    var order = new Order();
    order.AddItem("Hambúrguer", 19.90m, 2);
    order.AddItem("Batata", 9.50m, 1);

    Assert.Equal(49.30m, order.CalculateTotal());
}
```

Este teste pode passar de primeira com a implementação geral do ciclo anterior. Explique que ele amplia a cobertura; não altere código correto para forçar uma falha artificial.

**Refactor:** se melhorar a leitura, troque a acumulação por `_items.Sum(item => item.UnitPrice * item.Quantity)`. Rode os testes para demonstrar que o comportamento foi preservado.

## Etapa 3 — Confirmar pedido

**Regra:** somente um pedido com itens pode ser confirmado. Confirmar novamente é inválido. Após confirmar, o total continua consultável e novos itens são proibidos. Use `InvalidOperationException` para essas violações de estado.

### Ciclo 1: confirmação válida

```csharp
[Fact]
public void Confirmar_pedido_com_item_deve_marcar_como_confirmado()
{
    var order = new Order();
    order.AddItem("Hambúrguer", 20m, 1);

    order.Confirm();

    Assert.True(order.IsConfirmed);
    Assert.Single(order.Items);
    Assert.Equal(20m, order.CalculateTotal());
}
```

**Red esperado:** `NotImplementedException` em `Confirm`.

**Green:** substitua a exceção por `IsConfirmed = true`.

### Ciclo 2: pedido vazio

```csharp
[Fact]
public void Confirmar_pedido_vazio_deve_falhar()
{
    var order = new Order();

    Assert.Throws<InvalidOperationException>(() => order.Confirm());
    Assert.False(order.IsConfirmed);
}
```

**Green:** verifique se a lista está vazia antes de alterar `IsConfirmed`.

### Ciclo 3: confirmação repetida

```csharp
[Fact]
public void Confirmar_pedido_duas_vezes_deve_falhar()
{
    var order = new Order();
    order.AddItem("Hambúrguer", 20m, 1);
    order.Confirm();

    Assert.Throws<InvalidOperationException>(() => order.Confirm());
    Assert.True(order.IsConfirmed);
}
```

**Green:** em `Confirm`, rejeite também `IsConfirmed == true`.

### Ciclo 4: bloquear inclusão após confirmar

```csharp
[Fact]
public void Adicionar_item_apos_confirmar_deve_falhar_sem_alterar_pedido()
{
    var order = new Order();
    order.AddItem("Hambúrguer", 20m, 1);
    order.Confirm();

    Assert.Throws<InvalidOperationException>(() =>
        order.AddItem("Batata", 10m, 1));
    Assert.Single(order.Items);
    Assert.Equal(20m, order.CalculateTotal());
}
```

**Red esperado:** nenhuma exceção foi lançada.

**Green:** acrescente a proteção de `IsConfirmed` no início de `AddItem`. Este ciclo demonstra uma nova regra exigindo evolução de um método já implementado.

**Refactor:** revise a clareza das mensagens de exceção e a posição das validações. Extraia um método privado somente se facilitar a leitura.

## Encerramento

Execute:

```powershell
dotnet build SampleRestaurantTDD.sln
dotnet test SampleRestaurantTDD.sln
```

Com todos os exemplos do roteiro adicionados, a suíte terá **15 casos passando** (incluindo as linhas das teorias e o teste inicial). Se escolher somente a etapa 1, serão 8; até a etapa 2, serão 11. Nenhum `Skip` é necessário.

Peça aos alunos para explicar qual teste protege cada regra. Como extensão, eles podem testar preço fracionário na primeira etapa, inclusão de dois produtos com mesmo nome ou falha preservando itens já existentes. Uma API e um repositório podem ser introduzidos em outra aula, quando o foco passar de TDD de domínio para integração e arquitetura hexagonal.
