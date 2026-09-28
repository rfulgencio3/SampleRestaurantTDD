# SampleRestaurantTDD

Projeto didático em **.NET 8**, com arquitetura hexagonal, para praticar TDD em um restaurante fast-food. O cliente monta um pedido com hambúrguer, acompanhamento e bebida, aplica um desconto e confirma o pedido.

A estrutura está pronta; **exatamente 10 métodos de negócio estão pendentes**, lançando `NotImplementedException`. Cada exercício tem um teste inicial com `Skip`. Não são funcionalidades já concluídas: os alunos devem ativar o teste, observar a falha e construir a implementação durante a aula.

## Organização

```text
src/
  SampleRestaurantTDD.Domain/          Agregado Order, itens e regras de negócio
  SampleRestaurantTDD.Application/     Casos de uso e portas de entrada/saída
  SampleRestaurantTDD.Infrastructure/  Adaptadores de armazenamento e cardápio em memória
  SampleRestaurantTDD.Api/             Adaptador HTTP e composição de dependências
test/
  SampleRestaurantTDD.Tests/
    Domain/                           Exercícios 01–06
    Application/                      Exercícios 07–10
    ScaffoldingTests.cs                Testes ativos da estrutura fornecida
docs/
  LIVE-CODING.md                       Roteiro e critérios de aceite
```

As referências seguem `Api → Infrastructure → Application → Domain`; a API também usa os contratos da aplicação. O domínio não conhece HTTP, persistência ou frameworks. `IOrderService` é a porta de entrada; `IOrderRepository` e `IMenuCatalog` são portas de saída. A API conecta as interfaces aos adaptadores na injeção de dependência. Os testes dos casos de uso chamam a porta diretamente, sem servidor HTTP ou banco de dados.

## Executar

Requisitos: SDK .NET 8 ou superior com suporte a `net8.0`, runtime .NET/ASP.NET Core 8 e acesso ao NuGet no primeiro restore.

```powershell
dotnet restore SampleRestaurantTDD.sln
dotnet build SampleRestaurantTDD.sln --no-restore
dotnet test SampleRestaurantTDD.sln --no-build
dotnet run --project src/SampleRestaurantTDD.Api --urls http://localhost:5080
```

Estado inicial esperado: **3 testes passam e 10 estão ignorados**. `GET /health` e `GET /menu` funcionam. As rotas de pedidos retornam **501** até a implementação dos exercícios correspondentes. Violações de domínio retornam 400; recursos inexistentes, 404. Consulte `requests.http` para exemplos.

## Aula

Siga [o roteiro dos 10 exercícios](docs/LIVE-CODING.md). Remova o `Skip` de apenas um teste por vez e execute:

```powershell
dotnet test --filter FullyQualifiedName~Adicionar_item_deve_guardar_produto_e_quantidade
```

1. **Red:** leia o requisito, ative ou escreva um teste e confirme que falha pelo comportamento ausente.
2. **Green:** implemente o mínimo necessário para fazê-lo passar.
3. **Refactor:** melhore o código com os testes verdes.
4. Acrescente os casos de borda do roteiro, um por vez, e execute a suíte completa.

Os testes iniciais são exemplos de partida, não cobertura completa. Os critérios de aceite orientam novos testes escritos pelos alunos. Use `[Theory]` e `[InlineData]` para limites de quantidade e desconto. Não transforme a expectativa em `Throws<NotImplementedException>`: a exceção marca trabalho pendente.

## Limites intencionais

Persistência apenas em memória: reiniciar a API apaga os pedidos. O repositório guarda referências, sem transações nem isolamento para alterações concorrentes do mesmo pedido. O spy dos testes verifica se o caso de uso chamou `SaveAsync`. Este material não inclui pagamento, estoque, autenticação ou integração com redes reais de restaurantes. Desconto, remoção e cancelamento são exercícios do domínio; sua exposição HTTP pode ser feita depois da aula.
