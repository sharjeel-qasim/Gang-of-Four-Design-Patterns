# 🏛️ Gang of Four & Modern Enterprise Design Patterns in .NET 8 / C# 12

[![.NET 8](https://img.shields.io/badge/.NET-8.0%20LTS-purple.svg)](https://dotnet.microsoft.com/)
[![C# 12](https://img.shields.io/badge/C%23-12-blue.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![xUnit](https://img.shields.io/badge/Tests-29%20Passed-brightgreen.svg)](https://xunit.net/)
[![OpenAPI](https://img.shields.io/badge/Swagger-Interactive%20UI-green.svg)](https://swagger.io/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%26%20Modular-orange.svg)]()

> A production-grade, interactive, and comprehensive reference showcasing all **23 classic Gang of Four (GoF) design patterns** plus **6 high-demand modern cloud and enterprise architecture patterns** built for Senior .NET Developers, Technical Leads, and Software Architects.

---

## 🌟 What Makes This Repository Unique?

1. **Realistic Senior Scenarios**: No contrived `Animal/Dog` or `Shape/Circle` toys. Every implementation models realistic enterprise domains: Payment Gateways, Order Processing, Cloud Infrastructure Provisioners, Distributed Outboxes, Resilient Circuit Breakers, and ETL Pipelines.
2. **Modern C# 12 & .NET 8 LTS Idioms**: Leverages primary constructors, collection expressions, records, pattern matching, `System.Lazy<T>`, and `IAsyncEnumerable<T>`.
3. **Interactive Swagger / OpenAPI UI**: Launch the Web API and interactively execute every pattern with sample payloads, live traces, and structured responses directly from your browser.
4. **Interactive Terminal CLI Runner**: Explore patterns interactively via a formatted console application with step-by-step colored traces.
5. **100% Automated Test Coverage**: Comprehensive xUnit test suite verifying design invariants, edge cases, and multi-threaded concurrency safety.

---

## 🧭 Design Pattern Taxonomy & Quick Reference

### 🏭 Creational Patterns (5 Patterns)
| Pattern | Real-World Scenario | Modern C# / .NET Alternative | Code Link |
| :--- | :--- | :--- | :--- |
| **Singleton** | Centralized application configuration cache and connection management. | Prefer DI container `services.AddSingleton<T>()`; use `Lazy<T>` if static is strictly necessary. | [`Singleton.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Creational/Singleton.cs) |
| **Factory Method** | Multi-channel notification dispatcher routing to Email, SMS, or Push. | Factory delegates `Func<string, INotificationSender>` or Keyed DI services (.NET 8). | [`FactoryMethod.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Creational/FactoryMethod.cs) |
| **Abstract Factory** | Multi-cloud resource provisioner (AWS vs Azure Storage, Queues, Compute). | Cloud SDK factory abstractions or modular service provider configurations. | [`AbstractFactory.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Creational/AbstractFactory.cs) |
| **Builder** | Fluent Step-Builder for Financial Invoices enforcing valid states at compile time. | Fluent API builder or Init-only properties with validation. | [`Builder.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Creational/Builder.cs) |
| **Prototype** | Server infrastructure template cloning (Deep copy vs Shallow copy). | C# `record with { ... }` non-destructive mutation or custom copy constructors. | [`Prototype.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Creational/Prototype.cs) |

---

### 🧩 Structural Patterns (7 Patterns)
| Pattern | Real-World Scenario | Modern C# / .NET Alternative | Code Link |
| :--- | :--- | :--- | :--- |
| **Adapter** | Adapting legacy XML SOAP banking interfaces to modern REST `IPaymentGateway`. | Refit / AutoMapper or custom anti-corruption layer adapters. | [`Adapter.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Structural/Adapter.cs) |
| **Bridge** | Notification urgency abstractions (Standard, Urgent, Digest) bridged to channels (Slack, Email, SMS). | Composition over inheritance with decoupled strategy interfaces. | [`Bridge.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Structural/Bridge.cs) |
| **Composite** | E-Commerce nested product bundles and tree hierarchies with recursive discounts. | Recursive Linq aggregations over tree node structures. | [`Composite.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Structural/Composite.cs) |
| **Decorator** | Cross-cutting pipeline decorating `IOrderService` with Caching, Logging, and Timing. | Scrutor decorator registration or ASP.NET Core Middleware pipeline. | [`Decorator.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Structural/Decorator.cs) |
| **Facade** | E-Commerce checkout orchestrator coordinating Inventory, Payment, Shipping, and Notifications. | Application service orchestrators / Mediator handlers. | [`Facade.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Structural/Facade.cs) |
| **Flyweight** | High-density map markers sharing immutable icon graphics to conserve RAM. | In-memory object pooling (`Microsoft.Extensions.ObjectPool`) or string interning. | [`Flyweight.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Structural/Flyweight.cs) |
| **Proxy** | Role-based authorization protection proxy combined with lazy virtual report generation. | ASP.NET Core `[Authorize]` attributes or Castle DynamicProxy. | [`Proxy.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Structural/Proxy.cs) |

---

### ⚡ Behavioral Patterns (11 Patterns)
| Pattern | Real-World Scenario | Modern C# / .NET Alternative | Code Link |
| :--- | :--- | :--- | :--- |
| **Chain of Responsibility** | Purchase request approval escalation hierarchy (Lead -> Director -> VP -> Board). | ASP.NET Core Middleware or MediatR `IPipelineBehavior`. | [`ChainOfResponsibility.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/ChainOfResponsibility.cs) |
| **Command** | Bank account deposit/withdrawal transactions with full Undo rollback and audit history. | Command handlers or MediatR Commands with event journals. | [`Command.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/Command.cs) |
| **Interpreter** | Dynamic SQL-like filter rule engine evaluating boolean expressions on catalogs. | Linq Expression Trees (`System.Linq.Expressions`) or Dynamic LINQ. | [`Interpreter.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/Interpreter.cs) |
| **Iterator** | Streaming paginated REST API pages into unified asynchronous streams. | `IAsyncEnumerable<T>` with `await foreach` and `yield return`. | [`Iterator.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/Iterator.cs) |
| **Mediator** | In-process CQRS request/handler dispatcher decoupling commands from UI. | MediatR library or in-process message channels (`System.Threading.Channels`). | [`Mediator.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/Mediator.cs) |
| **Memento** | Shopping cart and document state snapshot manager supporting checkpoint rollback. | Immutable state records with time-travel checkpoints. | [`Memento.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/Memento.cs) |
| **Observer** | Real-time stock ticker price broadcasting to algorithmic traders and risk auditors. | `System.IObservable<T>` / `IObserver<T>`, Reactive Extensions (Rx), or SignalR. | [`Observer.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/Observer.cs) |
| **State** | Order fulfillment state machine (Created -> Paid -> Shipped -> Delivered / Cancelled). | Stateless library or switch expressions over state unions. | [`State.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/State.cs) |
| **Strategy** | Dynamic e-commerce discount calculators (Percentage, Fixed, VIP, Threshold). | Delegate functions `Func<Customer, decimal, decimal>` or keyed DI strategies. | [`Strategy.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/Strategy.cs) |
| **Template Method** | ETL Data Ingestion pipeline skeleton (Extract -> Validate -> Transform -> Load) with CSV/JSON hooks. | Strategy pattern or pipeline delegates passed to orchestrator. | [`TemplateMethod.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/TemplateMethod.cs) |
| **Visitor** | Technical document and AST exporter rendering into Markdown and HTML formats. | Modern C# 12 Pattern Matching (`switch` on type) or source generators. | [`Visitor.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.Behavioral/Visitor.cs) |

---

### 🚀 Modern Enterprise & Cloud Patterns (6 Patterns)
| Pattern | Real-World Scenario | Modern C# / .NET Alternative | Code Link |
| :--- | :--- | :--- | :--- |
| **CQRS** | Segregated Command (write model) and Query (read-optimized denormalized projection) pipelines. | MediatR / Marten / Dapper for queries + EF Core for commands. | [`CQRS.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/CQRS.cs) |
| **Specification** | Composable domain business rules (`And`, `Or`, `Not`) translating to Linq expressions. | Ardalis.Specification or custom generic specification base. | [`Specification.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/Specification.cs) |
| **Result Pattern (ROP)** | Railway-Oriented Programming chaining multi-step validation without throwing exceptions. | FluentResults / ErrorOr / LanguageExt / C# Result types. | [`ResultPattern.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/ResultPattern.cs) |
| **Transactional Outbox** | Atomically persisting database entities and outbound event messages to prevent dual-write loss. | MassTransit Outbox / Wolverine Outbox / EF Core Outbox. | [`OutboxPattern.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/OutboxPattern.cs) |
| **Circuit Breaker** | Resilient fault tolerance failing fast when downstream services degrade and probing for recovery. | Polly / `Microsoft.Extensions.Http.Resilience`. | [`CircuitBreaker.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/CircuitBreaker.cs) |
| **Options Pattern** | Strongly-typed, validated configuration access with reload support. | `IOptions<T>`, `IOptionsSnapshot<T>`, and `IOptionsMonitor<T>` with DataAnnotations. | [`OptionsPattern.cs`](file:///Volumes/SQ%20HD/Projects/Design%20Patterns/GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/OptionsPattern.cs) |

---

## 🚀 How to Run Interactively

### 1. Interactive Web API (Swagger UI)
Run the ASP.NET Core API to test endpoints live via the Swagger explorer:
```bash
dotnet run --project GoFDesignPatterns/GoFDesignPatterns.API/GoFDesignPatterns.API.csproj
```
Navigate to: **`http://localhost:5000`** or **`https://localhost:5001`** (Swagger UI is served at the root URL).

### 2. Interactive Terminal CLI Runner
Run the interactive console application to explore and simulate patterns right in your terminal:
```bash
# Interactive Menu
dotnet run --project GoFDesignPatterns/GoFDesignPatterns.CLI/GoFDesignPatterns.CLI.csproj

# Or run all 29 pattern demonstrations in batch:
dotnet run --project GoFDesignPatterns/GoFDesignPatterns.CLI/GoFDesignPatterns.CLI.csproj -- all
```

### 3. Run Automated Unit Tests
Run all 29 xUnit unit tests verifying pattern invariants:
```bash
dotnet test GoFDesignPatterns/GoFDesignPatterns.Tests/GoFDesignPatterns.Tests.csproj
```

---

## 🏗️ Solution Architecture

```
GoFDesignPatterns/
├── GoFDesignPatterns.sln
│
├── GoFDesignPatterns.Core/               # Shared abstractions, Result<T>, Error, Domain models
│   ├── Result.cs
│   ├── Models.cs
│   └── PatternExecutionReport.cs
│
├── GoFDesignPatterns.Creational/         # All 5 GoF Creational Patterns
│   ├── Singleton.cs                      # Naive, Lock-based, Lazy<T>, and DI Container comparison
│   ├── FactoryMethod.cs                  # Notification Dispatcher (Email, SMS, Push)
│   ├── AbstractFactory.cs                # Multi-Cloud Infrastructure (AWS vs Azure)
│   ├── Builder.cs                        # Step-Builder for Financial Invoices
│   └── Prototype.cs                      # Deep vs Shallow cloning & C# 12 records
│
├── GoFDesignPatterns.Structural/         # All 7 GoF Structural Patterns
│   ├── Adapter.cs                        # Legacy XML SOAP to modern IPaymentGateway
│   ├── Bridge.cs                         # Notification urgency bridged to delivery channels
│   ├── Composite.cs                      # Product Bundle catalog tree with recursive discounts
│   ├── Decorator.cs                      # Service pipeline (Caching, Logging, Metrics)
│   ├── Facade.cs                         # E-Commerce Checkout Orchestrator
│   ├── Flyweight.cs                      # High-density map marker icon cache
│   └── Proxy.cs                          # Protection & Virtual lazy-loading proxy
│
├── GoFDesignPatterns.Behavioral/         # All 11 GoF Behavioral Patterns
│   ├── ChainOfResponsibility.cs          # Corporate purchase approval hierarchy
│   ├── Command.cs                        # Bank account transactions with Undo & audit trail
│   ├── Interpreter.cs                    # AST rule engine evaluating catalog expressions
│   ├── Iterator.cs                       # IEnumerable and IAsyncEnumerable paginated streams
│   ├── Mediator.cs                       # In-process CQRS command dispatcher
│   ├── Memento.cs                        # Shopping cart state snapshot & rollback
│   ├── Observer.cs                       # Stock ticker broadcasting with IObservable<T>
│   ├── State.cs                          # Order fulfillment state machine
│   ├── Strategy.cs                       # Interchangeable discount calculation strategies
│   ├── TemplateMethod.cs                 # ETL pipeline (Extract -> Validate -> Transform -> Load)
│   └── Visitor.cs                        # Document exporter (Markdown and HTML)
│
├── GoFDesignPatterns.ModernEnterprise/   # High-Demand Senior .NET Architecture Patterns
│   ├── CQRS.cs                           # Write commands vs Read-optimized projections
│   ├── Specification.cs                  # Composable business rules (And, Or, Not)
│   ├── ResultPattern.cs                  # Railway-Oriented Programming (ROP)
│   ├── OutboxPattern.cs                  # Transactional Outbox for reliable event publishing
│   ├── CircuitBreaker.cs                 # Fault tolerance & resilience pipeline
│   └── OptionsPattern.cs                 # Strongly-typed validated configuration
│
├── GoFDesignPatterns.API/                # ASP.NET Core 8 Web API + Swagger OpenAPI UI
│   ├── Controllers/                      # Grouped interactive controllers
│   └── Program.cs
│
├── GoFDesignPatterns.CLI/                # Interactive terminal runner
│   └── Program.cs
│
└── GoFDesignPatterns.Tests/              # 29 xUnit tests verifying all patterns
```

---

## 💡 Key Architectural Insights for Senior Developers

### 1. Singleton vs DI Container
> **Rule of Thumb**: Avoid static singletons in modern ASP.NET Core apps. Static singletons introduce hidden dependencies, make unit testing difficult with mock frameworks, and can cause memory leaks. Instead, register dependencies as `services.AddSingleton<T>()`. If you genuinely need a static singleton, always use `System.Lazy<T>` for guaranteed thread safety and publication control.

### 2. Prototype vs C# 12 Records
> **Rule of Thumb**: Before modern C#, cloning complex objects required implementing `ICloneable` (which lacks type safety) or manual copy constructors. In modern C#, using `record` types with the built-in non-destructive mutation `with` expression (`var modified = original with { Status = "Active" };`) provides a cleaner, compiler-verified alternative for shallow copies.

### 3. Visitor vs C# 12 Pattern Matching
> **Rule of Thumb**: Classic Visitor requires double-dispatch (`element.Accept(visitor)` -> `visitor.Visit(this)`) and modifying element interfaces whenever a new element is added. In modern C#, pattern matching with switch expressions over record hierarchies often accomplishes the same goal with significantly less ceremony and better type safety.

### 4. Result Pattern vs Throwing Exceptions
> **Rule of Thumb**: Exceptions should be reserved for exceptional, unexpected circumstances (e.g., hardware faults, network dropouts). For predictable business logic outcomes (e.g., validation errors, insufficient balance, entity not found), use the `Result<T>` pattern. It provides explicit failure contracts, faster execution (no expensive stack trace allocations), and supports Railway-Oriented functional chaining via `Bind` and `Map`.

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to open a Pull Request or create an Issue.

---

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
