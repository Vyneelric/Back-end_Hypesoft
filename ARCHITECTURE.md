# Documentação de Decisões Arquiteturais

## 📐 Visão Geral da Arquitetura

Este projeto utiliza **Clean Architecture** (Arquitetura Limpa) combinada com o padrão **CQRS** (Command Query Responsibility Segregation) para garantir separação de responsabilidades, testabilidade e manutenibilidade.

## 🏛️ Camadas da Aplicação

### 1. Hypersoft.Domain (Camada de Domínio)

**Responsabilidade**: Contém as regras de negócio e entidades do domínio.

**Componentes**:
- `Entities/`: Entidades de negócio (Product, Category)
- `Repositories/`: Interfaces dos repositórios (contratos)
- `Services/`: Interfaces de serviços de domínio
- `ValueObjects/`: Objetos de valor (preparado para expansão)

**Decisões**:
- ✅ Sem dependências externas (núcleo da aplicação)
- ✅ Entidades refletem a estrutura do MongoDB (snake_case)
- ✅ Interfaces de repositórios definidas aqui (Dependency Inversion)

### 2. Hypersoft.Application (Camada de Aplicação)

**Responsabilidade**: Orquestra o fluxo de dados e implementa casos de uso.

**Componentes**:
- `Commands/`: Operações de escrita (Create, Update, Delete)
- `Queries/`: Operações de leitura (Get, Search)
- `Handlers/`: Implementação dos casos de uso (MediatR)
- `DTOs/`: Objetos de transferência de dados (PascalCase)
- `Validators/`: Validações com FluentValidation
- `Mappings/`: Perfis do AutoMapper

**Decisões**:
- ✅ **CQRS**: Separação clara entre comandos e consultas
- ✅ **MediatR**: Desacoplamento entre Controllers e lógica de negócio
- ✅ **FluentValidation**: Validações declarativas e reutilizáveis
- ✅ **AutoMapper**: Mapeamento automático Entity ↔ DTO

### 3. Hypersoft.Infrastructure (Camada de Infraestrutura)

**Responsabilidade**: Implementa detalhes técnicos e acesso a dados.

**Componentes**:
- `Data/`: Contexto do MongoDB
- `Repositories/`: Implementação concreta dos repositórios
- `Services/`: Implementação de serviços externos (preparado)

**Decisões**:
- ✅ MongoDB como banco de dados NoSQL
- ✅ Repository Pattern para abstração do acesso a dados
- ✅ Configuração centralizada no MongoDbContext

### 4. Hypersoft.API (Camada de Apresentação)

**Responsabilidade**: Expõe a API REST e gerencia requisições HTTP.

**Componentes**:
- `Controllers/`: Endpoints da API
- `Middlewares/`: Tratamento de exceções e cross-cutting concerns
- `Program.cs`: Configuração da aplicação e injeção de dependências

**Decisões**:
- ✅ Controllers enxutos (apenas roteamento)
- ✅ Middleware de exceções centralizado
- ✅ Swagger para documentação automática
- ✅ Respostas padronizadas (success, status_code, message, data)

## 🎯 Padrões de Design Utilizados

### 1. CQRS (Command Query Responsibility Segregation)

**Por quê?**
- Separação clara entre operações de leitura e escrita
- Facilita otimizações específicas para cada tipo de operação
- Melhora a testabilidade

**Implementação**:
```
Commands (Escrita)     Queries (Leitura)
     ↓                      ↓
  Handlers              Handlers
     ↓                      ↓
  Repository            Repository
     ↓                      ↓
   MongoDB               MongoDB
```

### 2. Repository Pattern

**Por quê?**
- Abstrai o acesso a dados
- Facilita troca de tecnologia de persistência
- Melhora testabilidade (mock dos repositórios)

**Exemplo**:
```csharp
// Interface no Domain
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(string id);
}

// Implementação na Infrastructure
public class ProductRepository : IProductRepository
{
    // Implementação específica do MongoDB
}
```

### 3. Dependency Injection

**Por quê?**
- Inversão de controle
- Facilita testes unitários
- Reduz acoplamento

**Configuração** (Program.cs):
```csharp
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddMediatR(...);
```

### 4. Mediator Pattern (via MediatR)

**Por quê?**
- Desacopla Controllers dos Handlers
- Facilita adição de novos casos de uso
- Permite adicionar behaviors (logging, validação, etc.)

**Fluxo**:
```
Controller → MediatR → Handler → Repository → MongoDB
```

## 🗄️ Escolha do Banco de Dados

### MongoDB (NoSQL)

**Por quê MongoDB?**
- ✅ Flexibilidade de schema (produtos podem ter atributos variados)
- ✅ Performance em leituras
- ✅ Escalabilidade horizontal
- ✅ Documentos JSON nativos (fácil integração com APIs REST)
- ✅ Sem necessidade de migrations complexas

**Trade-offs**:
- ❌ Sem transações ACID complexas (não é problema neste domínio)
- ❌ Joins menos eficientes (resolvido com desnormalização)

## 🔒 Validação de Dados

### FluentValidation

**Por quê?**
- ✅ Validações declarativas e legíveis
- ✅ Reutilizáveis
- ✅ Separadas da lógica de negócio
- ✅ Mensagens de erro customizáveis

**Exemplo**:
```csharp
public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Preco).GreaterThan(0);
    }
}
```

## 🔄 Mapeamento de Objetos

### AutoMapper

**Por quê?**
- ✅ Elimina código repetitivo
- ✅ Centraliza regras de mapeamento
- ✅ Reduz erros de mapeamento manual

**Uso**:
- Entity (banco) → DTO (API)
- Nomes diferentes: `id` → `Id`, `categoria_id` → `CategoriaId`

## 🐳 Containerização

### Docker + Docker Compose

**Por quê?**
- ✅ Ambiente consistente (dev = prod)
- ✅ Fácil setup (um comando sobe tudo)
- ✅ Isolamento de dependências
- ✅ Facilita CI/CD

**Serviços**:
- `api`: Aplicação .NET
- `mongodb`: Banco de dados
- `mongo-express`: Interface gráfica do MongoDB
- `keycloak`: Autenticação (preparado)

## 📡 Padrão de Resposta da API

### Estrutura Padronizada

**Sucesso**:
```json
{
  "success": true,
  "status_code": 200,
  "data": { ... }
}
```

**Erro**:
```json
{
  "success": false,
  "status_code": 404,
  "message": "Produto não encontrado"
}
```

**Por quê?**
- ✅ Consistência nas respostas
- ✅ Facilita tratamento no frontend
- ✅ Informações claras sobre o resultado

## 🔐 Segurança (Preparado)

### Keycloak

**Por quê Keycloak?**
- ✅ Solução enterprise de autenticação
- ✅ Suporte a OAuth2, OpenID Connect, SAML
- ✅ Gerenciamento de usuários e roles
- ✅ Single Sign-On (SSO)
- ✅ Open source e amplamente utilizado

**Próximos passos**:
1. Configurar Realm e Client no Keycloak
2. Adicionar middleware de autenticação JWT na API
3. Proteger endpoints com `[Authorize]`

## 📊 Tratamento de Erros

### Middleware Centralizado

**Por quê?**
- ✅ Tratamento consistente de exceções
- ✅ Evita try-catch em todos os Controllers
- ✅ Logging centralizado
- ✅ Respostas de erro padronizadas

**Implementação**:
```csharp
app.UseMiddleware<ExceptionHandlerMiddleware>();
```

## 🚀 Decisões de Performance

1. **Async/Await**: Todas as operações de I/O são assíncronas
2. **Scoped Services**: Repositórios com ciclo de vida por requisição
3. **Singleton para MongoDB**: Contexto compartilhado (thread-safe)
4. **Índices no MongoDB**: Preparado para adicionar índices em campos frequentemente consultados

## 🧪 Testabilidade

A arquitetura facilita testes em todos os níveis:

- **Testes Unitários**: Handlers podem ser testados mockando repositórios
- **Testes de Integração**: Repositórios podem ser testados com MongoDB em memória
- **Testes de API**: Controllers podem ser testados com TestServer

## 📈 Escalabilidade

**Preparado para**:
- Múltiplas instâncias da API (stateless)
- Sharding do MongoDB
- Cache (Redis) para queries frequentes
- Message Queue (RabbitMQ) para operações assíncronas

## 🔮 Próximas Melhorias

1. ✅ Implementar autenticação com Keycloak
2. ✅ Adicionar Nginx como reverse proxy
3. ✅ Implementar cache com Redis
4. ✅ Adicionar logging estruturado (Serilog)
5. ✅ Implementar health checks
6. ✅ Adicionar rate limiting
7. ✅ Implementar testes automatizados
8. ✅ CI/CD pipeline

## 📚 Referências

- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [MongoDB Best Practices](https://www.mongodb.com/docs/manual/administration/production-notes/)
