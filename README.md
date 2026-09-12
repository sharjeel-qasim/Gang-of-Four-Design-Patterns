# 🏛️ Gang of Four & Modern Enterprise Design Patterns in .NET 8 / C# 12

[![.NET 8](https://img.shields.io/badge/.NET-8.0%20LTS-purple.svg)](https://dotnet.microsoft.com/)
[![C# 12](https://img.shields.io/badge/C%23-12-blue.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![xUnit](https://img.shields.io/badge/Tests-29%20Passed-brightgreen.svg)](https://xunit.net/)
[![OpenAPI](https://img.shields.io/badge/Swagger-Interactive%20UI-green.svg)](https://swagger.io/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20%26%20Modular-orange.svg)]()

> A production-grade, interactive, and comprehensive reference showcasing all **23 classic Gang of Four (GoF) design patterns** plus **6 high-demand modern cloud and enterprise architecture patterns** built for Senior .NET Developers, Technical Leads, and Software Architects.

---

## ⚡ Quick Navigation

Jump directly to any pattern implementation, real-world scenario, and code:

| Category | Patterns (Click to Jump) |
| :--- | :--- |
| 🏭 **[Creational Patterns](#-creational-patterns)** | [Singleton](#1-singleton) &bull; [Factory Method](#2-factory-method) &bull; [Abstract Factory](#3-abstract-factory) &bull; [Builder](#4-builder) &bull; [Prototype](#5-prototype) |
| 🧩 **[Structural Patterns](#-structural-patterns)** | [Adapter](#6-adapter) &bull; [Bridge](#7-bridge) &bull; [Composite](#8-composite) &bull; [Decorator](#9-decorator) &bull; [Facade](#10-facade) &bull; [Flyweight](#11-flyweight) &bull; [Proxy](#12-proxy) |
| ⚡ **[Behavioral Patterns](#-behavioral-patterns)** | [Chain of Responsibility](#13-chain-of-responsibility) &bull; [Command](#14-command) &bull; [Interpreter](#15-interpreter) &bull; [Iterator](#16-iterator) &bull; [Mediator](#17-mediator) &bull; [Memento](#18-memento) &bull; [Observer](#19-observer) &bull; [State](#20-state) &bull; [Strategy](#21-strategy) &bull; [Template Method](#22-template-method) &bull; [Visitor](#23-visitor) |
| 🚀 **[Modern Enterprise](#-modern-enterprise--cloud-patterns)** | [CQRS](#24-cqrs) &bull; [Specification](#25-specification) &bull; [Result Pattern / ROP](#26-result-pattern-rop) &bull; [Transactional Outbox](#27-transactional-outbox) &bull; [Circuit Breaker](#28-circuit-breaker) &bull; [Options Pattern](#29-options-pattern) |
| 🎮 **[Interactive Interfaces](#-how-to-run-interactively)** | [Interactive Web Dashboard](#1-interactive-web-dashboard) &bull; [Swagger OpenAPI UI](#2-interactive-web-api-swagger-ui) &bull; [Interactive CLI Runner](#3-interactive-terminal-cli-runner) &bull; [Automated Unit Tests](#4-run-automated-unit-tests) |

---

## 🌟 What Makes This Repository Unique?

1. **Realistic Senior Scenarios**: No contrived `Animal/Dog` or `Shape/Circle` toys. Every implementation models realistic enterprise domains: Payment Gateways, Order Processing, Cloud Infrastructure Provisioners, Distributed Outboxes, Resilient Circuit Breakers, and ETL Pipelines.
2. **Modern C# 12 & .NET 8 LTS Idioms**: Leverages primary constructors, collection expressions, records, pattern matching, `System.Lazy<T>`, and `IAsyncEnumerable<T>`.
3. **Interactive Web Dashboard**: Launch the Web API and explore an interactive visual dashboard with sidebar navigation, search filters, and live "Run Pattern" buttons directly from your browser.
4. **Interactive Swagger / OpenAPI UI**: Execute every pattern endpoint via Swagger with sample payloads, live traces, and structured responses.
5. **Interactive Terminal CLI Runner**: Explore patterns interactively via a formatted console application with step-by-step colored traces.
6. **100% Automated Test Coverage**: Comprehensive xUnit test suite verifying design invariants, edge cases, and multi-threaded concurrency safety.

---

## 🚀 How to Run Interactively

### 1. Interactive Web Dashboard
Run the Web API and explore all 29 patterns visually in your browser:
```bash
dotnet run --project GoFDesignPatterns/GoFDesignPatterns.API/GoFDesignPatterns.API.csproj
```
Navigate to: **`http://localhost:5000`** (served at root with responsive sidebar navigation and instant live execution).

### 2. Interactive Web API (Swagger UI)
For full OpenAPI specifications and interactive HTTP documentation:
Navigate to: **`http://localhost:5000/swagger`**.

### 3. Interactive Terminal CLI Runner
Run the interactive console application to explore and simulate patterns in your terminal:
```bash
# Interactive menu:
dotnet run --project GoFDesignPatterns/GoFDesignPatterns.CLI/GoFDesignPatterns.CLI.csproj

# Run all 29 pattern demonstrations in batch:
dotnet run --project GoFDesignPatterns/GoFDesignPatterns.CLI/GoFDesignPatterns.CLI.csproj -- all
```

### 4. Run Automated Unit Tests
Execute the full xUnit test suite covering all 29 patterns:
```bash
dotnet test GoFDesignPatterns/GoFDesignPatterns.Tests/GoFDesignPatterns.Tests.csproj
```

---

## 🏭 Creational Patterns

### 1. Singleton
- **Intent**: Ensure a class has only one instance, while providing a global point of access to this instance.
- **Senior Real-World Scenario**: Centralized thread-safe configuration management, logging cache, or connection pool coordinator.
- **Modern C# Comparison**: In modern ASP.NET Core, prefer registering services with `services.AddSingleton<T>()` in the DI container. When a static singleton is required, always use `System.Lazy<T>` for guaranteed thread safety and publication control rather than naive double-check locking.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Creational/Singleton.cs](GoFDesignPatterns/GoFDesignPatterns.Creational/Singleton.cs)
- **API Endpoint**: `GET /api/creational/singleton`
- [⬆ Back to Navigation](#quick-navigation)

---

### 2. Factory Method
- **Intent**: Define an interface for creating an object, but let subclasses decide which class to instantiate.
- **Senior Real-World Scenario**: Multi-channel Notification Dispatcher routing notifications dynamically across Email, SMS, and Push gateways based on message urgency and user profile settings.
- **Modern C# Comparison**: Often paired with factory delegates `Func<string, INotificationSender>` or Keyed DI services introduced in .NET 8 (`services.AddKeyedScoped<T>`).
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Creational/FactoryMethod.cs](GoFDesignPatterns/GoFDesignPatterns.Creational/FactoryMethod.cs)
- **API Endpoint**: `POST /api/creational/factory-method`
- [⬆ Back to Navigation](#quick-navigation)

---

### 3. Abstract Factory
- **Intent**: Provide an interface for creating families of related or dependent objects without specifying their concrete classes.
- **Senior Real-World Scenario**: Multi-Cloud Infrastructure Provisioner abstracting AWS vs Azure vs GCP ecosystems (Blob Storage, Message Queues, Compute VMs) to ensure compatible component families.
- **Modern C# Comparison**: Enables cloud-agnostic microservices where switching from AWS S3/SQS to Azure Blob/ServiceBus requires changing only the factory registration.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Creational/AbstractFactory.cs](GoFDesignPatterns/GoFDesignPatterns.Creational/AbstractFactory.cs)
- **API Endpoint**: `POST /api/creational/abstract-factory`
- [⬆ Back to Navigation](#quick-navigation)

---

### 4. Builder
- **Intent**: Separate the construction of a complex object from its representation so that the same construction process can create different representations.
- **Senior Real-World Scenario**: Fluent Step-Builder for Financial Invoices enforcing sequential construction (`Customer -> Items -> Tax -> Build`) and preventing invalid object states at compile time.
- **Modern C# Comparison**: Step-builder interfaces prevent runtime validation exceptions by using the C# type system to guide the developer through required steps.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Creational/Builder.cs](GoFDesignPatterns/GoFDesignPatterns.Creational/Builder.cs)
- **API Endpoint**: `POST /api/creational/builder`
- [⬆ Back to Navigation](#quick-navigation)

---

### 5. Prototype
- **Intent**: Specify the kinds of objects to create using a prototypical instance, and create new objects by copying this prototype.
- **Senior Real-World Scenario**: Virtual machine and server configuration template cloning demonstrating Deep Copy vs Shallow Copy reference independence.
- **Modern C# Comparison**: Modern C# 12 records provide non-destructive mutation via the built-in `with` expression (`var clone = template with { Name = "Worker-02" };`), eliminating manual cloning code for shallow copies.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Creational/Prototype.cs](GoFDesignPatterns/GoFDesignPatterns.Creational/Prototype.cs)
- **API Endpoint**: `POST /api/creational/prototype`
- [⬆ Back to Navigation](#quick-navigation)

---

## 🧩 Structural Patterns

### 6. Adapter
- **Intent**: Convert the interface of a class into another interface clients expect. Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.
- **Senior Real-World Scenario**: Adapting legacy XML/SOAP mainframe banking systems to modern JSON REST `IPaymentGateway` in an Anti-Corruption Layer (ACL).
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Structural/Adapter.cs](GoFDesignPatterns/GoFDesignPatterns.Structural/Adapter.cs)
- **API Endpoint**: `POST /api/structural/adapter`
- [⬆ Back to Navigation](#quick-navigation)

---

### 7. Bridge
- **Intent**: Decouple an abstraction from its implementation so that the two can vary independently.
- **Senior Real-World Scenario**: Decoupling notification urgency abstractions (`StandardNotification`, `UrgentNotification`, `DigestNotification`) from delivery platform channels (`SlackChannel`, `EmailChannel`, `SmsChannel`).
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Structural/Bridge.cs](GoFDesignPatterns/GoFDesignPatterns.Structural/Bridge.cs)
- **API Endpoint**: `POST /api/structural/bridge`
- [⬆ Back to Navigation](#quick-navigation)

---

### 8. Composite
- **Intent**: Compose objects into tree structures to represent part-whole hierarchies. Composite lets clients treat individual objects and compositions of objects uniformly.
- **Senior Real-World Scenario**: E-Commerce nested product bundles and nested bill-of-materials with recursive discount and price aggregations.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Structural/Composite.cs](GoFDesignPatterns/GoFDesignPatterns.Structural/Composite.cs)
- **API Endpoint**: `GET /api/structural/composite`
- [⬆ Back to Navigation](#quick-navigation)

---

### 9. Decorator
- **Intent**: Attach additional responsibilities to an object dynamically. Decorators provide a flexible alternative to subclassing for extending functionality.
- **Senior Real-World Scenario**: Assembling transparent cross-cutting service pipelines decorating `IOrderService` with In-Memory Caching, Structured Logging, and Execution Timing.
- **Modern C# Comparison**: In modern ASP.NET Core, often registered via the `Scrutor` library (`services.Decorate<IOrderService, CachingOrderServiceDecorator>()`).
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Structural/Decorator.cs](GoFDesignPatterns/GoFDesignPatterns.Structural/Decorator.cs)
- **API Endpoint**: `GET /api/structural/decorator`
- [⬆ Back to Navigation](#quick-navigation)

---

### 10. Facade
- **Intent**: Provide a unified interface to a set of interfaces in a subsystem. Facade defines a higher-level interface that makes the subsystem easier to use.
- **Senior Real-World Scenario**: E-Commerce checkout orchestrator coordinating Inventory reservation, Payment charging, Shipping manifest creation, and Customer confirmation notifications into a single cohesive call.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Structural/Facade.cs](GoFDesignPatterns/GoFDesignPatterns.Structural/Facade.cs)
- **API Endpoint**: `POST /api/structural/facade`
- [⬆ Back to Navigation](#quick-navigation)

---

### 11. Flyweight
- **Intent**: Use sharing to support large numbers of fine-grained objects efficiently.
- **Senior Real-World Scenario**: High-density geospatial map rendering engine sharing immutable icon glyphs across thousands of markers, drastically reducing RAM footprint.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Structural/Flyweight.cs](GoFDesignPatterns/GoFDesignPatterns.Structural/Flyweight.cs)
- **API Endpoint**: `GET /api/structural/flyweight`
- [⬆ Back to Navigation](#quick-navigation)

---

### 12. Proxy
- **Intent**: Provide a surrogate or placeholder for another object to control access to it.
- **Senior Real-World Scenario**: `ProtectionProxy` verifying role-based security authorization combined with `VirtualProxy` lazy-loading expensive financial analytics engines.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Structural/Proxy.cs](GoFDesignPatterns/GoFDesignPatterns.Structural/Proxy.cs)
- **API Endpoint**: `GET /api/structural/proxy`
- [⬆ Back to Navigation](#quick-navigation)

---

## ⚡ Behavioral Patterns

### 13. Chain of Responsibility
- **Intent**: Avoid coupling the sender of a request to its receiver by giving more than one object a chance to handle the request. Chain the receiving objects and pass the request along the chain.
- **Senior Real-World Scenario**: Corporate purchase request and expense approval escalation hierarchy (`Team Lead ($1K) -> Director ($25K) -> VP ($100K) -> Board ($1M)`).
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/ChainOfResponsibility.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/ChainOfResponsibility.cs)
- **API Endpoint**: `POST /api/behavioral/chain-of-responsibility`
- [⬆ Back to Navigation](#quick-navigation)

---

### 14. Command
- **Intent**: Encapsulate a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations.
- **Senior Real-World Scenario**: Financial transaction journal supporting execution, complete rollback (Undo), and immutable audit history.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/Command.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/Command.cs)
- **API Endpoint**: `POST /api/behavioral/command`
- [⬆ Back to Navigation](#quick-navigation)

---

### 15. Interpreter
- **Intent**: Given a language, define a representation for its grammar along with an interpreter that uses the representation to interpret sentences in the language.
- **Senior Real-World Scenario**: Dynamic SQL-like rule evaluator parsing composite expressions (`Price <= 500 AND Category == 'Electronics' AND InStock == true`) against domain collections.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/Interpreter.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/Interpreter.cs)
- **API Endpoint**: `POST /api/behavioral/interpreter`
- [⬆ Back to Navigation](#quick-navigation)

---

### 16. Iterator
- **Intent**: Provide a way to access the elements of an aggregate object sequentially without exposing its underlying representation.
- **Senior Real-World Scenario**: Transparently streaming paginated 3rd-party REST API batches into an asynchronous pipeline via `IAsyncEnumerable<T>` and `yield return`.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/Iterator.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/Iterator.cs)
- **API Endpoint**: `GET /api/behavioral/iterator`
- [⬆ Back to Navigation](#quick-navigation)

---

### 17. Mediator
- **Intent**: Define an object that encapsulates how a set of objects interact. Mediator promotes loose coupling by keeping objects from referring to each other explicitly.
- **Senior Real-World Scenario**: In-process CQRS request/handler dispatcher (MediatR style) decoupling API controllers from domain handlers in Clean Architecture.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/Mediator.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/Mediator.cs)
- **API Endpoint**: `POST /api/behavioral/mediator`
- [⬆ Back to Navigation](#quick-navigation)

---

### 18. Memento
- **Intent**: Without violating encapsulation, capture and externalize an object's internal state so that the object can be restored to this state later.
- **Senior Real-World Scenario**: Shopping cart checkout state snapshotting and multi-step form rollback without exposing internal state fields.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/Memento.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/Memento.cs)
- **API Endpoint**: `POST /api/behavioral/memento`
- [⬆ Back to Navigation](#quick-navigation)

---

### 19. Observer
- **Intent**: Define a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.
- **Senior Real-World Scenario**: Real-time stock market price feeds broadcasting updates to automated algorithmic trading bots and compliance risk monitors using `System.IObservable<T>`.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/Observer.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/Observer.cs)
- **API Endpoint**: `POST /api/behavioral/observer`
- [⬆ Back to Navigation](#quick-navigation)

---

### 20. State
- **Intent**: Allow an object to alter its behavior when its internal state changes. The object will appear to change its class.
- **Senior Real-World Scenario**: E-Commerce order fulfillment state machine enforcing valid transitions (`Created -> Paid -> Shipped -> Delivered / Cancelled`) and rejecting invalid actions.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/State.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/State.cs)
- **API Endpoint**: `POST /api/behavioral/state`
- [⬆ Back to Navigation](#quick-navigation)

---

### 21. Strategy
- **Intent**: Define a family of algorithms, encapsulate each one, and make them interchangeable. Strategy lets the algorithm vary independently from clients that use it.
- **Senior Real-World Scenario**: Interchangeable checkout discount calculation strategies (Percentage, Fixed Voucher, VIP Tier, Progressive Cart Threshold) configured dynamically at runtime.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/Strategy.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/Strategy.cs)
- **API Endpoint**: `POST /api/behavioral/strategy`
- [⬆ Back to Navigation](#quick-navigation)

---

### 22. Template Method
- **Intent**: Define the skeleton of an algorithm in an operation, deferring some steps to subclasses. Template Method lets subclasses redefine certain steps of an algorithm without changing the algorithm's structure.
- **Senior Real-World Scenario**: ETL Data Ingestion pipeline skeleton (`Extract -> Validate -> Transform -> Load`) with format-specific hooks for CSV and JSON data streams.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/TemplateMethod.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/TemplateMethod.cs)
- **API Endpoint**: `POST /api/behavioral/template-method`
- [⬆ Back to Navigation](#quick-navigation)

---

### 23. Visitor
- **Intent**: Represent an operation to be performed on the elements of an object structure. Visitor lets you define a new operation without changing the classes of the elements on which it operates.
- **Senior Real-World Scenario**: Technical document AST exporter rendering document elements into Markdown and HTML formats without modifying the element class models.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.Behavioral/Visitor.cs](GoFDesignPatterns/GoFDesignPatterns.Behavioral/Visitor.cs)
- **API Endpoint**: `POST /api/behavioral/visitor`
- [⬆ Back to Navigation](#quick-navigation)

---

## 🚀 Modern Enterprise & Cloud Patterns

### 24. CQRS
- **Intent**: Segregate read and update operations for a data store. Commands mutate state, while queries return denormalized views without side effects.
- **Senior Real-World Scenario**: Separating high-scale transactional product writes from read-optimized cached projections in e-commerce microservices.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/CQRS.cs](GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/CQRS.cs)
- **API Endpoint**: `POST /api/modern/cqrs`
- [⬆ Back to Navigation](#quick-navigation)

---

### 25. Specification
- **Intent**: Encapsulate domain business rules into reusable objects that can be combined using boolean logic and applied to in-memory collections or translated to database SQL via Expressions.
- **Senior Real-World Scenario**: Composable enterprise order validation policies combining `PremiumCustomer`, `HighValueOrder`, and `FraudFree` specifications.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/Specification.cs](GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/Specification.cs)
- **API Endpoint**: `POST /api/modern/specification`
- [⬆ Back to Navigation](#quick-navigation)

---

### 26. Result Pattern (ROP)
- **Intent**: Model operations that can succeed or fail explicitly without throwing exceptions for control flow, enabling functional pipeline chaining via `Bind` and `Map`.
- **Senior Real-World Scenario**: Multi-stage financial loan underwriting pipeline chaining credit score, income verification, and debt-to-income calculations without exception overhead.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/ResultPattern.cs](GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/ResultPattern.cs)
- **API Endpoint**: `POST /api/modern/result-rop`
- [⬆ Back to Navigation](#quick-navigation)

---

### 27. Transactional Outbox
- **Intent**: Persist domain entities and outbound integration messages atomically in a single local database transaction to prevent dual-write inconsistencies.
- **Senior Real-World Scenario**: Reliable event-driven architectures publishing events to Kafka or RabbitMQ with zero message loss during database updates.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/OutboxPattern.cs](GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/OutboxPattern.cs)
- **API Endpoint**: `POST /api/modern/outbox`
- [⬆ Back to Navigation](#quick-navigation)

---

### 28. Circuit Breaker
- **Intent**: Prevent an application from repeatedly trying to execute an operation that's likely to fail, allowing it to continue without consuming resources while the fault is being fixed.
- **Senior Real-World Scenario**: Resilient cloud microservice communication transitioning across `Closed`, `Open`, and `Half-Open` states to protect against downstream service outages.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/CircuitBreaker.cs](GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/CircuitBreaker.cs)
- **API Endpoint**: `POST /api/modern/circuit-breaker`
- [⬆ Back to Navigation](#quick-navigation)

---

### 29. Options Pattern
- **Intent**: Provide strongly-typed access to related configuration settings groups with validation and support for real-time reload notifications.
- **Senior Real-World Scenario**: Validating cloud storage container names, concurrency limits, and allowed extensions on application startup using DataAnnotations.
- **Source Code**: [GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/OptionsPattern.cs](GoFDesignPatterns/GoFDesignPatterns.ModernEnterprise/OptionsPattern.cs)
- **API Endpoint**: `POST /api/modern/options`
- [⬆ Back to Navigation](#quick-navigation)

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
│   ├── Singleton.cs
│   ├── FactoryMethod.cs
│   ├── AbstractFactory.cs
│   ├── Builder.cs
│   └── Prototype.cs
│
├── GoFDesignPatterns.Structural/         # All 7 GoF Structural Patterns
│   ├── Adapter.cs
│   ├── Bridge.cs
│   ├── Composite.cs
│   ├── Decorator.cs
│   ├── Facade.cs
│   ├── Flyweight.cs
│   └── Proxy.cs
│
├── GoFDesignPatterns.Behavioral/         # All 11 GoF Behavioral Patterns
│   ├── ChainOfResponsibility.cs
│   ├── Command.cs
│   ├── Interpreter.cs
│   ├── Iterator.cs
│   ├── Mediator.cs
│   ├── Memento.cs
│   ├── Observer.cs
│   ├── State.cs
│   ├── Strategy.cs
│   ├── TemplateMethod.cs
│   └── Visitor.cs
│
├── GoFDesignPatterns.ModernEnterprise/   # High-Demand Senior .NET Architecture Patterns
│   ├── CQRS.cs
│   ├── Specification.cs
│   ├── ResultPattern.cs
│   ├── OutboxPattern.cs
│   ├── CircuitBreaker.cs
│   └── OptionsPattern.cs
│
├── GoFDesignPatterns.API/                # ASP.NET Core 8 Web API + Interactive Dashboard + Swagger UI
│   ├── Controllers/
│   ├── wwwroot/                          # Interactive visual explorer dashboard
│   └── Program.cs
│
├── GoFDesignPatterns.CLI/                # Interactive terminal runner
│   └── Program.cs
│
└── GoFDesignPatterns.Tests/              # 29 xUnit tests verifying all patterns
```

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to open a Pull Request or create an Issue.

---

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
