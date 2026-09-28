# Roteiro de live-coding

Valores em reais usando `decimal`. Regras inválidas lançam `DomainException`; pedido ou produto não encontrado pelos casos de uso lança `KeyNotFoundException`. Falhas não devem alterar estado nem salvar pedidos. Pedidos começam em `Draft`, podem ir para `Confirmed` ou `Cancelled`, e não saem desses estados finais.

## 01 — Adicionar item (`Order.AddItem`)

- Aceitar produto disponível, com preço maior que zero, e quantidade inteira maior que zero.
- Copiar identificador, nome e preço para `OrderItem`.
- Adicionar o mesmo produto deve somar quantidades em uma única linha, preservando o preço e nome capturados na primeira inclusão.
- Rejeitar quantidade zero/negativa, preço zero/negativo e produto indisponível.
- Permitir somente em `Draft`. Adicione testes dos estados finais após os exercícios 05 e 06.

Primeiro teste fornecido: produto de R$ 20,00, quantidade 2, uma linha no pedido.

## 02 — Remover item (`Order.RemoveItem`)

- Remover a linha inteira, independentemente da quantidade.
- Rejeitar produto ausente com `DomainException` e preservar os demais itens.
- Permitir somente em `Draft`.

Depende de 01. Teste também pedido vazio e preservação de outra linha.

## 03 — Calcular total (`Order.CalculateTotal`)

- Somar `UnitPrice × Quantity`; pedido vazio vale zero.
- Aplicar `DiscountPercentage` sobre o subtotal.
- Arredondar somente o resultado final em duas casas, com `MidpointRounding.AwayFromZero`.
- Consulta permitida em qualquer estado, sem modificá-lo.

Depende de 01. Exemplo: dois hambúrgueres de R$ 20,00 + batata de R$ 10,00 = R$ 50,00. Após 04, testar R$ 10,05 com desconto de 10% = R$ 9,05.

## 04 — Aplicar desconto (`Order.ApplyDiscount`)

- Aceitar percentual de 0 a 30, inclusive; aceitar valores fracionários.
- Substituir desconto anterior, sem acumular; zero remove o desconto.
- Rejeitar valores fora do intervalo sem alterar desconto anterior.
- Permitir somente em `Draft`, inclusive em pedido vazio.

Depende de 03 para conferir o efeito no total. Teste limites 0 e 30, negativos, 30,01 e substituição de 10 por 20.

## 05 — Confirmar pedido (`Order.Confirm`)

- Exigir pelo menos uma linha e estado `Draft`.
- Alterar para `Confirmed`, preservando itens e desconto.
- Rejeitar pedido vazio, cancelado ou já confirmado.
- Após confirmar, rejeitar adição, remoção e aplicação de desconto.

Depende de 01. Retome os testes de mutação para fechar essas restrições.

## 06 — Cancelar pedido (`Order.Cancel`)

- Permitir cancelar um rascunho vazio ou com itens.
- Alterar para `Cancelled`, preservando itens para consulta.
- Rejeitar pedido confirmado ou já cancelado.
- Após cancelar, rejeitar adição, remoção, desconto e confirmação.

O primeiro teste é independente; testes adicionais usam 01 e 05.

## 07 — Abrir pedido (`OrderService.CreateAsync`)

- Gerar `Guid` não vazio, diferente a cada chamada.
- Criar pedido vazio em `Draft`, salvar uma vez e devolver o pedido.
- Encaminhar o `CancellationToken` recebido ao repositório.

Não depende dos métodos pendentes do domínio. Use o spy fornecido para observar persistência.

## 08 — Adicionar item pelo caso de uso (`OrderService.AddItemAsync`)

- Buscar pedido e produto pelas portas de saída.
- Pedido inexistente: lançar `KeyNotFoundException` antes de consultar produto.
- Produto inexistente: lançar `KeyNotFoundException`.
- Delegar a validação e inclusão a `Order.AddItem`, salvando uma vez em caso de sucesso.
- Não salvar quando qualquer validação falhar; encaminhar o token às portas.

Depende de 01. Adicione um spy de `IMenuCatalog` quando precisar observar consultas. Evite repetir regras de domínio no serviço.

## 09 — Finalizar pedido (`OrderService.CheckoutAsync`)

- Buscar pedido, chamar `Confirm`, salvar uma vez e retornar o pedido confirmado.
- Pedido inexistente: lançar `KeyNotFoundException`.
- Pedido vazio ou em estado final: propagar `DomainException`, sem salvar.
- Encaminhar o token ao repositório. Finalização aqui significa confirmação, sem cobrança.

Depende de 01 e 05. Verifique tanto o estado final quanto a chamada explícita de persistência.

## 10 — Consultar pedido (`OrderService.GetAsync`)

- Retornar o pedido existente, mantendo itens, desconto e estado.
- Pedido inexistente: lançar `KeyNotFoundException`.
- Nunca salvar nem modificar o pedido; encaminhar o token ao repositório.

Pode ser implementado antes de 08/09 se ajudar a demonstração HTTP. Após concluir, execute todos os testes e valide o fluxo criar → adicionar → consultar → finalizar na API.

## Sugestão de condução

Reserve blocos de aula: domínio (01–06) e aplicação (07–10). Para cada processo, discuta primeiro exemplos de entrada/saída e só depois escreva o teste. A turma deve ampliar os dez testes iniciais, e não apenas remover os dez `Skip`. A conclusão exige que os critérios acima estejam cobertos e que não restem `NotImplementedException` nos dez métodos.
