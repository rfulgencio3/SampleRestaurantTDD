# SampleRestaurantTDD

Aula de TDD com .NET 8 e xUnit.

- `src/SampleRestaurantTDD.Domain`: pedido e item do pedido.
- `test/SampleRestaurantTDD.Tests`: testes do pedido.

Com o SDK .NET 8 instalado, execute na raiz:

```powershell
dotnet test
```

O comando restaura as dependências, compila e executa os testes. Estado inicial: **1 teste passando**.

Implemente durante a aula: `AddItem`, `CalculateTotal` e `Confirm`. Os três métodos estão pendentes; é possível parar após a primeira ou a segunda etapa.

[Passo a passo do live-coding](docs/LIVE-CODING.md).
