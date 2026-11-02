# CompleteSystem

Projeto de estudos em **ASP.NET Core MVC (.NET 10)** criado para entender, na prática, três assuntos que costumam ser ensinados de forma separada:

- **DTO** — como transportar dados entre as camadas sem vazar a entidade do banco;
- **Microsserviços** — como pensar em fronteiras, contratos e baixo acoplamento (aplicados aqui dentro de um monólito bem dividido);
- **Encapsulamento** — como esconder detalhes de implementação atrás de interfaces e classes com responsabilidade única.

A aplicação é um CRUD de usuários com views Razor, EF Core e SQLite. Parece simples — e é de propósito. O objetivo não é a tela, é a **arquitetura por trás dela**.

> ⚠️ **Importante:** este projeto é um **monólito MVC**, não um conjunto de microsserviços rodando em processos separados. O que ele faz é aplicar os princípios que tornam uma futura separação em microsserviços viável: contratos claros (DTOs), serviços encapsulados por interface e camadas que não se conhecem por dentro. A seção [Relação com microsserviços](#-relação-com-microsserviços) explica essa ponte.

---

## 📚 Documentação de estudo

O repositório tem material próprio de apoio:

| Arquivo | Conteúdo |
| --- | --- |
| [`fluxo.md`](fluxo.md) | Ordem de leitura do fluxo da aplicação |
| [`studyWhileCode.md`](studyWhileCode.md) | Anotações rápidas sobre DTO, DataAnnotations e mapeamento |
| [`guia/index.html`](guia/index.html) | Guia visual completo (HTML/CSS) sobre DTO, SOLID e SRP — abra no navegador |

---

## 🧠 Os três pilares do projeto

### 1. DTO (Data Transfer Object)

DTO é uma **classe de transporte**. Ela define o formato dos dados que entram e saem da aplicação, sem carregar comportamento de domínio nem acesso a banco.

Neste projeto existem três DTOs, um para cada intenção:

| DTO | Onde é usado | Campos | Por quê |
| --- | --- | --- | --- |
| `UsuarioCreateDTO` | Formulário e action `Create` | Nome, Email, Senha | A senha existe só na entrada; nunca volta |
| `UsuarioUpdateDTO` | Formulário e action `Edit` | Id, Nome, Email, Ativo | Não permite trocar senha nem `CriadoEm` |
| `UsuarioResponseDTO` | Listagem, detalhes e retorno do service | Id, Nome, Email, Ativo, CriadoEm | **Não contém `SenhaHash`** |

O que isso resolve:

- **Segurança:** a entidade `Usuario` tem `SenhaHash`; o DTO de resposta não. O hash nunca chega à View.
- **Proteção contra over-posting:** o formulário só consegue preencher os campos declarados no DTO.
- **Contrato estável:** mudar uma coluna do banco não quebra a tela automaticamente.
- **Validação no lugar certo:** `[Required]`, `[EmailAddress]` e `[StringLength]` ficam no DTO, validando **formato**. Regras de negócio (e-mail já cadastrado) ficam no service.

A conversão entidade ↔ DTO é feita por métodos de extensão em [`Models/DTOs/UsuarioMapper.cs`](Models/DTOs/UsuarioMapper.cs), mantendo o mapeamento explícito e em um único lugar.

### 2. Microsserviços (a ideia por trás das fronteiras)

Microsserviços não são sobre "muitos projetos", são sobre **fronteiras bem definidas**. Cada serviço:

- expõe um contrato explícito;
- esconde a implementação;
- não compartilha o banco nem as classes internas de outro serviço;
- pode ser trocado, testado e evoluído de forma independente.

Este projeto treina exatamente esses hábitos em escala pequena:

```csharp
// O controller conhece apenas o contrato, nunca a implementação.
public class UsuarioController : Controller
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }
}
```

`IUsuarioService` é a "fronteira" do serviço de usuários. O controller não sabe se por trás existe EF Core, SQLite, uma API HTTP ou um mock de teste. É essa mesma mentalidade que permite, no futuro, mover `UsuarioService` para outro processo sem reescrever o controller.

### 3. Encapsulamento

Encapsular não é só usar `private`. É **impedir que detalhes internos vazem** e garantir que cada classe seja usada apenas pelo que ela promete.

Onde o encapsulamento aparece aqui:

- **`UsuarioService`** esconde o `AppDbContext` e o `IPasswordHasher<Usuario>`. Ninguém de fora sabe como a senha vira hash ou como o e-mail é normalizado.
- **`IUsuarioService`** expõe só o necessário: cinco métodos assíncronos que retornam DTOs ou `bool`, nunca entidades ou `IActionResult`.
- **`Usuario`** é uma entidade burra de propósito: representa a tabela e não sabe validar formulário nem responder HTTP.
- **`RegraDeNegocioException`** traduz uma violação de regra em um erro que o controller consegue tratar, sem expor detalhes do banco.
- **`AppDbContext`** concentra o mapeamento (tamanho dos campos, índice único de e-mail), em vez de espalhar configuração pelo código.

Regra de ouro do projeto:

> **Controller não sabe SQL. Service não sabe HTTP. Entidade não sabe validação de formulário.**

---

## 🏗️ Arquitetura

```
Requisição HTTP
      │
      ▼
┌─────────────────┐   recebe DTO, valida ModelState, escolhe a resposta
│   Controller    │   (não tem DbContext, não faz hash, não conhece regra)
└────────┬────────┘
         │ depende de IUsuarioService (abstração)
         ▼
┌─────────────────┐   valida regras de negócio, normaliza dados, gera hash
│     Service     │   (não conhece View, IActionResult nem TempData)
└────────┬────────┘
         │ usa AppDbContext + IPasswordHasher
         ▼
┌─────────────────┐   consulta e grava com EF Core
│   DbContext     │
└────────┬────────┘
         ▼
      SQLite (app.db)
```

Na volta, o service devolve `UsuarioResponseDTO`. **A entidade nunca chega à View.**

### Estrutura de pastas

```
CompleteSystem/
├── Controllers/
│   ├── HomeController.cs            # páginas institucionais / erro
│   └── UsuarioController.cs         # só HTTP
├── Models/
│   ├── Usuario.cs                   # entidade = tabela
│   ├── ErrorViewModel.cs
│   └── DTOs/
│       ├── UsuarioCreateDTO.cs      # entrada do Create
│       ├── UsuarioUpdateDTO.cs      # entrada do Edit
│       ├── UsuarioResponseDTO.cs    # saída / resposta
│       └── UsuarioMapper.cs         # entidade ↔ DTO
├── Data/
│   ├── AppDbContext.cs              # EF Core + mapeamento
│   └── Migrations/                  # histórico do banco
├── Services/
│   ├── IUsuarioService.cs           # contrato (fronteira)
│   ├── UsuarioService.cs            # regras de negócio
│   └── RegraDeNegocioException.cs   # erro esperado de negócio
├── Views/
│   ├── Home/
│   ├── Shared/
│   └── Usuario/                     # Index, Create, Edit, Details, Delete
├── guia/                            # guia de estudos em HTML/CSS
├── Program.cs                       # injeção de dependência e pipeline
└── appsettings.json                 # connection string
```

---

## 🔄 Fluxo da aplicação

1. O usuário acessa `/Usuario`.
2. `UsuarioController.Index()` chama `_usuarioService.ListarAsync()`.
3. `UsuarioService` consulta o banco com `AsNoTracking()`, ordena por nome e mapeia cada entidade para `UsuarioResponseDTO`.
4. O controller entrega a lista de DTOs para a View.
5. A View `Index.cshtml` declara `@model IReadOnlyList<UsuarioResponseDTO>` — ela não conhece a entidade.

No cadastro (`Create`):

1. O formulário é vinculado a `UsuarioCreateDTO`.
2. O `ModelState` valida o formato com DataAnnotations.
3. O service verifica e-mail duplicado, normaliza nome/e-mail, gera o hash da senha e salva.
4. Se a regra de negócio falhar, `RegraDeNegocioException` é lançada e o controller a converte em erro de `ModelState` para o usuário.

---

## 🚀 Como executar

### Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- (Opcional) `dotnet-ef` para criar novas migrations

### Passos

```bash
# na raiz do projeto
dotnet restore

# (opcional) instalar a ferramenta de migrations
dotnet tool install --global dotnet-ef

# executar
dotnet run
```

A aplicação sobe em:

- HTTP: <http://localhost:5210>
- HTTPS: <https://localhost:7238>

O banco `app.db` é criado automaticamente na primeira execução, porque o `Program.cs` aplica as migrations na inicialização:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
```

### Criar uma nova migration

```bash
dotnet ef migrations add NomeDaMigration
dotnet ef database update   # opcional: o app aplica sozinho ao iniciar
```

---

## 🧩 Principais decisões de projeto

| Decisão | Motivo |
| --- | --- |
| Três DTOs separados (Create, Update, Response) | Cada operação tem um contrato próprio; nada de campos indevidos |
| Service atrás de interface (`IUsuarioService`) | Inversão de dependência, testabilidade e fronteira clara |
| Mapeamento manual no `UsuarioMapper` | Explícito e simples para um CRUD pequeno; AutoMapper seria exagero aqui |
| Senha só como hash (`SenhaHash`) | A senha em texto puro nunca é persistida |
| Índice único de e-mail no `AppDbContext` | Última linha de defesa contra duplicidade, mesmo com requisições concorrentes |
| `RegraDeNegocioException` | Separa erro de negócio de erro técnico e de erro de validação de formulário |
| `AsNoTracking()` nas leituras | Consultas de listagem/detalhe não precisam de rastreamento do EF |
| SQLite + `Database.Migrate()` no startup | Ambiente de estudo sem servidor de banco e sem passo manual |

---

## 🔗 Relação com microsserviços

| Conceito de microsserviços | Como aparece neste projeto |
| --- | --- |
| Contrato explícito entre serviços | DTOs + `IUsuarioService` |
| Implementação escondida | `UsuarioService` só é conhecido via interface |
| Serviço não conhece o cliente | Service não usa `IActionResult`, `View` ou `TempData` |
| Banco por serviço | Hoje um SQLite único; o `AppDbContext` já está isolado na camada `Data` |
| Baixo acoplamento | Controller depende de abstração; trocar a implementação não exige mudar a tela |
| Testabilidade independente | O service pode ser testado sem subir HTTP |

Para transformar este estudo em microsserviços de verdade, o caminho natural seria:

1. Extrair `UsuarioService` + `AppDbContext` para um projeto de API (`Usuarios.Api`).
2. Substituir a injeção local por um cliente HTTP (`IUsuarioService` implementado com `HttpClient`).
3. Manter os mesmos DTOs como contrato público (ou evoluir para OpenAPI/JSON Schema).
4. Dar ao serviço seu próprio banco e seu próprio ciclo de deploy.

O ponto central: **nada no controller mudaria** — só a implementação por trás da interface. Esse é o teste de que a fronteira está bem encapsulada.

---

## 🛠️ Stack

- .NET 10 / ASP.NET Core MVC
- C# com nullable reference types habilitado
- Entity Framework Core 10
- SQLite
- ASP.NET Core Identity (`PasswordHasher<Usuario>`) apenas para hash de senha
- Razor Views + Bootstrap

---

## 📌 Próximos passos sugeridos

- [ ] Testes unitários do `UsuarioService` com um `AppDbContext` in-memory e um `IPasswordHasher` fake
- [ ] Endpoint de login usando `VerifyHashedPassword`
- [ ] Paginação e busca na listagem
- [ ] Extrair o serviço de usuários para uma API separada
- [ ] Adicionar OpenAPI/Swagger no contrato da API
- [ ] Substituir SQLite por PostgreSQL em um ambiente de produção

---

## 👤 Autor

Projeto pessoal de estudos em .NET, com foco em **entender o porquê** de cada camada — e não apenas fazer o CRUD funcionar.
