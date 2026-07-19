# Modules and Layers

[Documentation index](README.md) · [Magyar változat](../hu/modules-and-layers.md)

Although this sample application is intentionally small, every module has been designed to scale to larger applications.

The solution combines the **Model-View-ViewModel (MVVM)** presentation pattern with a layered architecture inspired by **Clean Architecture**, **Clean Code**, and the **SOLID** design principles. These concepts solve related but different problems:

- **MVVM** separates the user interface, presentation state, and application behavior.
- **Layered and Clean Architecture** define responsibility boundaries and dependency direction across the whole system.
- **Clean Code and SOLID** guide the internal design of modules, classes, methods, and contracts.

The modules do not each implement a separate MVVM pattern. Instead, they cooperate to implement the three MVVM roles. In a small demonstration project, the roles may look close to three logical parts. In a larger production system, each role is normally distributed across several projects, feature modules, services, and adapters.

| MVVM role | Current modules |
|---|---|
| **Model** | `Domain`, `Application`, and `Infrastructure` |
| **ViewModel** | `Presentation`, supported by `FW4di.Dotnet.MVVM` |
| **View** | `WPFUI` |
| **Composition and startup** | `WPFUI`, supported by `FW4di.Dotnet.Core` |
| **Verification** | `Tests` |

## MVVM in This Solution

MVVM divides presentation-oriented software into three cooperating roles: **Model**, **View**, and **ViewModel**.

The dependency direction is intentionally asymmetric: the View binds to and invokes the ViewModel; the ViewModel calls application use cases and exposes presentation state; the Model contains application and business behavior without depending on the View. The ViewModel must not depend on a concrete `Window`, `Page`, or `UserControl`. The Model must not know whether it is presented through WPF, another desktop toolkit, a web interface, or no user interface at all.

This separation allows important behavior to be tested without launching WPF and allows technical details such as XML persistence to be replaced without rewriting the presentation or business rules.

### Model

In MVVM, the Model is broader than a collection of DTOs or database records. It represents everything that belongs to the application but is not presentation-specific: business entities and Value Objects, business rules and invariants, Domain services and policies, Application use cases, commands, queries and handlers, validation and authorization rules, repository and service contracts, transaction boundaries, persistence implementations, external service adapters, and file, database or network integration.

In this solution the Model role is deliberately divided into Domain, Application, and Infrastructure.

`Domain` describes stable business concepts and rules. The current project contains the `UserData` model. In a larger system the Domain could also contain Entities, Value Objects, Aggregates, Domain Services, Specifications, Domain Events, policies, invariants, and Domain-specific exceptions or result types.

`Application` contains operations the application can perform. Current examples include authentication, user queries, user export contract, transactional save, user deletion, repository contracts, service contracts, and birth date rules. In a larger system it could also contain command/query handlers, workflow orchestration, transaction boundaries, permission checks, input validation, result models, DTO boundaries, ports, idempotency, and retry policies.

`Infrastructure` contains concrete technical implementations required by Application contracts. Current examples include XML repositories, JSON export, data managers, file path handling, and file system access. In a larger application Infrastructure could also contain database access, ORM implementations, HTTP clients, messaging, caching, logging, email, jobs, identity adapters, cloud storage, and OS integration.

### View

The View is the concrete user interface seen and operated by the user. In WPF this includes `Window`, `Page`, `UserControl`, XAML, controls, templates, styles, brushes, icons, resources, converters, behaviors, visual validation feedback, animations, accessibility and platform-specific interaction.

In this solution the View role is implemented by `WPFUI`. It renders ViewModel state, forwards user interaction through bindings and commands, displays validation and notification results, and performs strictly visual or WPF-specific behavior. It must not own business decisions, repository access, XML or JSON processing, application workflow orchestration, transaction handling, or direct construction of infrastructure services.

Code-behind is not automatically forbidden. It is acceptable for behavior that is truly visual, tightly coupled to a specific WPF control, and contains no reusable presentation or business rule. Saving a user, deciding permissions, loading a repository, or applying business validation does not belong in code-behind.

### ViewModel

The ViewModel is a presentation model and an abstraction of a View. It exposes state and actions in a form that data binding can consume without knowing the concrete visual controls.

A ViewModel may expose observable UI state, provide commands, process user intent, call Application use cases, track selection/loading/error/editing state, decide whether a UI action is currently available, map Application or Domain data to presentation-specific models, coordinate navigation and dialogs through abstractions, and translate technical or use-case results into user-facing state.

A ViewModel must not implement business rules, perform file or database access, select concrete repository implementations, manage infrastructure transactions directly, create XML or JSON documents, contain WPF controls, or directly manipulate a concrete View.

Current ViewModels are `MainViewModel`, `LoginViewModel`, `UserListViewModel`, and `UserViewModel`. They use `FW4di.Dotnet.MVVM` commands and notification support.

## Clean Code and SOLID Guidelines

Clean Code is not a single Microsoft standard. It is a collective name for widely adopted software-design practices popularized by authors and practitioners such as Robert C. Martin, Martin Fowler, Kent Beck, and many others. Microsoft architecture guidance applies many of the same ideas through separation of concerns, dependency injection, testability, SOLID principles, and layered or Clean Architecture.

The goal is not to maximize the number of classes, interfaces, or layers. The goal is to make responsibilities explicit, dependencies controlled, behavior testable, and future changes local rather than contagious.

### Single Responsibility Principle

A class or module should have one coherent responsibility and one primary reason to change. A ViewModel manages screen state and interaction. A use case coordinates one application operation. A repository handles persistence-oriented access. A mapper converts between model shapes. A WPF adapter displays a concrete platform dialog.

A ViewModel that handles navigation, file access, business validation, repository calls, mapping, export, and session management at the same time has become a presentation-level god object. Such responsibilities should be moved into focused Application use cases, Presentation services, or Infrastructure adapters.

### Separation of Concerns

Presentation, application workflow, business rules, and technical implementation are kept separate. Views do not execute business operations. ViewModels do not perform file operations. Application code does not depend on WPF. Domain code does not depend on storage technology. Infrastructure does not define business policy.

### Dependency Inversion Principle

High-level policy must not depend directly on low-level technical details. Application declares a repository contract required by a use case; Infrastructure provides the XML implementation; WPFUI selects and registers the implementation; the use case receives the contract through constructor injection.

### Explicit Dependencies

Required collaborators should be visible through constructors or method signatures. Avoid global service locators, hidden static services, resolving from DI inside business code, creating concrete repositories inside ViewModels or use cases, and passing the entire service provider to ordinary application classes.

### High Cohesion and Low Coupling

Closely related behavior should stay together. Unrelated behavior should not be forced into the same class or module. Feature-oriented organization, focused use cases, small contracts, and presentation models support these goals.

### Interface Segregation Principle

Consumers should depend only on the operations they actually require. Prefer focused contracts over large manager interfaces. This does not mean creating an interface for every class; abstractions are justified when they represent a real boundary, isolate volatile technology, support multiple implementations, or are required for isolated testing.

### Open/Closed Principle

Stable application policy should be extendable without repeatedly modifying central orchestration code, for example by adding another repository implementation behind an existing Application contract or another export format through a focused export abstraction. The principle should not be used to predict every hypothetical extension.

### Liskov Substitution Principle

An implementation must honor the behavioral expectations of its abstraction. An alternative repository implementation should preserve the semantics expected by Application use cases: identity handling, not-found behavior, error reporting, ordering guarantees where relevant, and save/delete consistency.

### Tell, Do Not Ask Excessively

Behavior should normally be placed near the data and responsibility that own it. MVVM presentation models expose state for binding, but business invariants should remain owned by Domain or Application behavior rather than reconstructed in XAML or ViewModels.

### Intention-Revealing Names

Prefer names such as `SaveUserUseCase`, `IUserRepository`, `UserEditSession`, `UserListViewModel`, and `IMessageService`. Avoid vague containers such as `Helper`, `CommonManager`, `MiscService`, `Utils2`, and `DataProcessor`.

### Small, Focused Methods

Methods should operate at a consistent level of abstraction and express one understandable step of behavior. A use-case method may coordinate validation, repository access, and result creation, but serialization details, UI message construction, and file-system retry loops should be delegated to the components that own those concerns.

### Avoid Duplication, But Preserve Clarity

Duplicated knowledge should be centralized when it represents the same rule. Superficially similar code should not be merged when the concepts can evolve independently. A single business age rule should have one authoritative implementation. Two visually similar ViewModels do not automatically require a generic base class if their workflows differ.

### Testability

Domain behavior can be covered by pure unit tests. Application use cases can be tested with fake or mocked contracts. ViewModels can be tested without creating a concrete View. Infrastructure can be covered by integration tests against files, databases, or external adapters. WPF-specific behavior can be reserved for focused UI or end-to-end tests.

### Avoid Unnecessary Abstractions

Clean Architecture does not require an interface in front of every class, a handler around every method, or a separate project for every concept. An abstraction is most useful when it crosses an architectural boundary, isolates volatile technology, has multiple implementations, must be replaced in tests, or hides a concrete implementation from higher-level policy.

### Keep Framework Code at the Edges

WPF, file APIs, serializers, database libraries, and DI containers are implementation tools. They should remain near the outer layers of the architecture. Domain and Application behavior should use framework-neutral language wherever practical.

## Current Project Responsibilities

- `FW4di.Dotnet.Core`: Dependency Injection support, common helper utilities, framework-independent technical components.
- `FW4di.Dotnet.MVVM`: commands, property change notification, binding support, messaging primitives. It contains no UserManager business logic.
- `Domain`: the `UserData` business model, independent from WPF, Presentation, Infrastructure, XML, and JSON.
- `Application`: `AuthenticationService`, `UserQueryService`, `SaveUserUseCase`, `DeleteUserUseCase`, repository contracts, `IUserExportService`, `BirthDateRules`, and service contracts.
- `Infrastructure`: `XmlUserRepository`, `XmlAddressCityRepository`, `JsonUserExportService`, `XmlDataManager<T>`, `JsonDataManager<T>`, `DataFilePaths`, and file system access.
- `Presentation`: ViewModels, bindable models, navigation, session state, edit session management, mapping, and exception message formatting.
- `WPFUI`: Views, XAML, WPF validation, converters, platform services, Composition Root, DI wiring, colors, brushes, and shared WPF styles.
- `Tests`: Domain, Application, Presentation, Infrastructure, dependency registration, authentication, queries, save, edit session, navigation, export, mapping, and repository behavior.

<p align="center"><img src="../images/architecture/flow.svg" alt="UserManager workflow diagram"></p>
