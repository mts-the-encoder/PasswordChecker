
# PasswordChecker

Este repositório expõe uma pequena API Web que valida senhas de acordo com as regras descritas no desafio.

## Regras (o que torna uma senha válida)

- Nove ou mais caracteres
- Ao menos 1 dígito
- Ao menos 1 letra minúscula
- Ao menos 1 letra maiúscula
- Ao menos 1 caractere especial do conjunto: `!@#$%^&*()-+`
- Não possuir caracteres repetidos (cada caractere deve ser único)
- Não conter espaços em branco (espaços, tabs ou quebras de linha são inválidos)

Exemplos:

```c#
IsValid("") // false
IsValid("aa") // false
IsValid("ab") // false
IsValid("AAAbbbCc") // false
IsValid("AbTp9!foo") // false
IsValid("AbTp9!foA") // false
IsValid("AbTp9 fok") // false
IsValid("AbTp9!fok") // true
```

## Estrutura do projeto

- `Api/` - API Web ASP.NET com controllers e `Program.cs`
- `Application/` - Casos de uso, validadores, DTOs de request/response
- `Domain/` - Objetos de valor (por exemplo `Password`) quando aplicável
- `Tests/` - Testes unitários e de integração

## API

Endpoint: POST /validate

Request JSON:

```json
{
  "password": "AbTp9!fok"
}
```

Resposta (200) - senha válida

```json
{
  "isValid": true,
  "message": "valid password"
}
```

Resposta (400) - senha inválida

```json
{
  "isValid": false,
  "message": "password must contain at least 1 uppercase letter | password cannot contain repeated characters"
}
```

Observações:
- O controller da API está em `Api/Controllers/PasswordController.cs` e delega a validação para `Application.UseCases.ValidatePasswordUseCase`.
- O DTO de request é `Application/Communication/Requests/PasswordRequest.cs` e o DTO de response é `Application/Communication/Responses/PasswordResponse.cs`.

## Como executar (Windows / PowerShell)

Abra um prompt PowerShell na raiz do repositório (a pasta que contém `PasswordChecker.slnx`) e execute:

```powershell
dotnet restore
dotnet run --project .\Api\Api.csproj
```

Por padrão a aplicação irá ligar nas portas configuradas em `Api/Properties/launchSettings.json` (ou na porta padrão do Kestrel). Quando estiver rodando localmente você pode chamar o endpoint com `curl` ou outro cliente HTTP.

Exemplo usando `curl` (PowerShell):

```powershell
curl -Method POST -Uri http://localhost:5140/api/password/validate -Body (@{password='AbTp9!fok'} | ConvertTo-Json) -ContentType 'application/json'
```

Ou usando HTTPie (se instalado):

```powershell
http POST http://localhost:5140/api/password/validate password=AbTp9!fok
```

## Testes

Existem testes unitários e de integração no projeto `Tests/`. Para executar os testes:

```powershell
dotnet test
```

Os testes incluem:
- Testes do caso de uso para a lógica do validador 
- Testes de integração que exercitam os endpoints da API
- Além dos exemplos fixos exigidos no desafio, criei um gerador de dados dinâmico usando a biblioteca bogus. toda vez que a suíte de testes é executada, o código cria 100 senhas aleatórias na hora (50 válidas e 50 inválidas, forçando falhas em regras diferentes). isso garante que a validação é robusta de verdade e aguenta qualquer combinação que for enviada, provando que a api não foi feita apenas para passar nos testes estáticos

## Notas de design e justificativas

- Responsabilidade única: as regras de validação estão encapsuladas em `ValidatePasswordValidator`, que usa FluentValidation. O caso de uso `ValidatePasswordUseCase` orquestra a validação e constrói o `PasswordResponse`, bem como pode contar outras regras de negócio, se necessário.
- O controller não contém nenhum tipo de regra e apenas coordena a entrada e saída das requisições
- Os DTOs são simples e intencionalmente fáceis de serializar.
- Logging: falhas de validação são logadas com mensagens concatenadas para facilitar debugging e auditoria.

### Por que FluentValidation

FluentValidation fornece uma forma expressiva e legível de declarar regras de validação e entrega mensagens de erro detalhadas. Mantém a lógica de validação declarativa e fácil de estender, ou seja, consigo criar diversos tipos de validação e centralizar em apenas um lugar.

### Extensibilidade

- Para adicionar ou alterar uma regra, edite apenas `ValidatePasswordValidator`. O restante da aplicação (controller/use case) não precisa ser alterado.
- Caso seja necessário criar um novo caso de uso, nada do que já foi feito será alterado, cada UseCase é tratado de forma independente.

## Suposições

- A API espera JSON com uma única propriedade `password`.
- Caracteres em branco são explicitamente inválidos, o validador falha se qualquer whitespace estiver presente.
- A definição de caracteres especiais está limitada ao conjunto `!@#$%^&*()-+` conforme pedido.
- Caracteres repetidos são proibidos considerando distinção entre maiúsculas e minúsculas. Por exemplo, `a` e `A` são caracteres distintos.

## Próximos passos e melhorias

- Retornar erros de validação estruturados (array de mensagens) em vez de uma única string concatenada, para facilitar a exibição de mensagens pelos clientes.
