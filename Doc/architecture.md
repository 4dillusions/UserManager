# Architecture

[Magyar változat](architecture_HU.md) · [Cookbook](cookbook/README.md) · [Architecture Decision Records](decisions/README.md)

UserManager is not intended to be a product documentation site. It is primarily an MVVM and layered architecture reference implementation, and a long-term personal knowledge base that can be revisited before starting future WPF projects.

The English and Hungarian architecture documents are intended to stay synchronized in content. Neither language should be treated as a shortened version of the other.

This document is the central architecture handbook for the repository. It is meant to be read from top to bottom like a small technical book. A developer should be able to understand the architectural philosophy, the MVVM mapping, the Clean Architecture-inspired layering, the dependency rules, the runtime flow, the dependency injection setup, and the role of each module without opening several separate architecture pages.

## Introduction

UserManager is a small WPF application capable of user authentication, user management, XML persistence, and JSON export. The application contains three screens:

- Login: authenticates a user against the XML data file.
- User List: displays users, filters by city, searches user data, adds, edits, deletes, and exports users.
- User Editor: edits every field except `UserId`, shows input validation, and uses a transactional Save / Cancel workflow.

The sample is intentionally small, but the structure is designed to scale. It demonstrates modern MVVM architecture, dependency injection, repository pattern, application use cases, transactional editing, and clean separation between presentation, business behavior, and persistence.

The main projects are:

- `FW4di.Dotnet.Core`
- `FW4di.Dotnet.MVVM`
- `App4di.Dotnet.UserManager.Domain`
- `App4di.Dotnet.UserManager.Application`
- `App4di.Dotnet.UserManager.Infrastructure`
- `App4di.Dotnet.UserManager.Presentation`
- `App4di.Dotnet.UserManager.WPFUI`
- `App4di.Dotnet.UserManager.Tests`

The project follows a practical layered architecture inspired by MVVM, Clean Architecture, Clean Code, and SOLID principles while intentionally avoiding unnecessary enterprise complexity.

## Architectural Philosophy

The architecture is built around a few deliberate goals:

- Clear separation of responsibilities.
- Business logic independent from WPF.
- Business logic independent from persistence technology.
- Dependency inversion through Application contracts.
- Thin ViewModels responsible for presentation state and UI orchestration.
- Infrastructure containing technical implementations, not business policy.
- High testability through dependency injection and focused services.
- Reusable framework code separated from application-specific behavior.

The goal is not to maximize the number of projects, classes, interfaces, or patterns. The goal is to make responsibilities explicit, dependencies controlled, behavior testable, and future changes local rather than contagious.

This repository therefore uses enough structure to demonstrate architectural boundaries without turning the sample into a generic framework. Clean Architecture is used as guidance, not as a rigid ceremony. MVVM is used where it fits WPF naturally: XAML views bind to ViewModels, ViewModels expose state and commands, and application behavior stays outside the concrete UI.

Common industry guidance from Microsoft Learn, Martin Fowler's Presentation Model description, and Robert C. Martin's Clean Architecture writing points in the same direction: keep UI concerns separate from non-UI behavior, use binding and commands to decouple views from presentation logic, keep dependencies explicit, and make source dependencies point toward stable business policy rather than volatile technical details.

## MVVM

MVVM divides presentation-oriented software into three cooperating roles: Model, View, and ViewModel.

```text
User interaction
      |
      v
+-----------+     binding / commands     +-------------+     use cases / services     +-----------+
|   View    | --------------------------> |  ViewModel  | ---------------------------> |  Model    |
|  XAML UI  | <-------------------------- | UI state    | <--------------------------- | behavior  |
+-----------+     property changes        +-------------+     results / data           +-----------+
```

The View is the concrete user interface. In WPF this means XAML, visual layout, controls, styles, templates, resources, validation visuals, converters, and code-behind that is truly view-specific.

The ViewModel is a presentation model. It exposes state and actions in a form that WPF data binding can consume without knowing concrete controls. It coordinates user intent, commands, selection, loading state, validation messages, navigation, dialogs, and calls into application services or use cases.

The Model is broader than a DTO or database record. It represents the application behavior and data that are not presentation-specific. In this repository the Model role is divided into Domain, Application, and Infrastructure.

Microsoft's MVVM and WPF data binding guidance emphasizes declarative XAML, binding, commands, change notification, and separation between UI and non-UI code. Martin Fowler's Presentation Model pattern describes the same core idea from another angle: move presentation state and behavior out of GUI controls so it can be understood and tested independently.

## Clean Architecture

Clean Architecture organizes code so that stable business concepts are not controlled by volatile details such as UI frameworks, databases, serializers, file systems, or dependency injection containers.

The practical rule is:

```text
Source dependencies should point toward stable policy.
Outer technical details may depend on inner application rules.
Inner application rules must not depend on outer technical details.
```

In this repository:

- Domain is the most independent project.
- Application owns use cases and contracts required by use cases.
- Infrastructure implements Application contracts.
- Presentation uses Application behavior and exposes bindable UI state.
- WPFUI is the outer composition and platform layer.

This is not a textbook four-circle Clean Architecture implementation. It is a WPF-oriented layered architecture inspired by the same dependency rule. The important point is not the exact number of rings; the important point is that WPF, XML, JSON, file access, and concrete adapters remain outside the stable business concepts.

## Relationship Between MVVM and Clean Architecture

MVVM and Clean Architecture solve related but different problems.

MVVM explains how the user interface is separated from presentation state and application behavior. Clean Architecture explains how the whole system is organized so that business rules and use cases do not depend on technical details.

In a small application these ideas can look similar because the system may only have a few classes. In a larger application they are different dimensions:

- MVVM separates View, ViewModel, and Model roles.
- Clean Architecture separates Domain, Application, Infrastructure, Presentation, and platform concerns.
- SOLID and Clean Code guide the internal design of classes, methods, and contracts.

The modules do not each implement a separate MVVM pattern. They cooperate to implement the MVVM roles.

## Mapping MVVM to This Solution

| MVVM role | Current modules |
|---|---|
| Model | `Domain`, `Application`, and `Infrastructure` |
| ViewModel | `Presentation`, supported by `FW4di.Dotnet.MVVM` |
| View | `WPFUI` |
| Composition and startup | `WPFUI`, supported by `FW4di.Dotnet.Core` |
| Verification | `Tests` |

### Model / Domain Role

#### Purpose

The Model role contains the application's non-UI behavior and data. It is where business concepts, application operations, persistence contracts, and technical adapters cooperate without depending on the View.

In this repository the Model role is deliberately divided into three projects:

- `Domain` describes what the system is.
- `Application` describes what the system can do.
- `Infrastructure` describes how technical details are performed.

#### Current implementation

The current implementation keeps the Domain lightweight, puts use cases and service contracts in Application, and puts XML/JSON/file implementations in Infrastructure.

Current examples include:

- `UserData`
- `AuthenticationService`
- `UserQueryService`
- `SaveUserUseCase`
- `DeleteUserUseCase`
- `IUserRepository`
- `IAddressCityRepository`
- `IUserExportService`
- `XmlUserRepository`
- `XmlAddressCityRepository`
- `JsonUserExportService`

#### Typical responsibilities in larger applications

Typical Model-side responsibilities include:

- Business entities
- Value objects
- Business rules
- Domain services
- Application use cases
- Commands and queries
- Validation
- Repository interfaces
- Persistence implementations
- External service adapters

The Model should not depend on concrete WPF views, controls, windows, pages, or XAML resources.

### View

#### Purpose

The View is the concrete user interface seen and operated by the user. Its purpose is to render state, collect user input, and forward user intent to the ViewModel through binding and commands.

#### Current implementation

The View role is implemented by `App4di.Dotnet.UserManager.WPFUI`.

Current examples include:

- `LoginView`
- `UserListView`
- `UserView`
- `MainView`
- XAML layout
- Resource dictionaries
- Themes
- Styles
- WPF validation rules
- Converters
- Platform-specific services

#### Typical responsibilities in larger applications

Typical View responsibilities include:

- XAML
- Visual layout
- Controls
- Templates
- Styles
- Bindings
- Behaviors
- Animations
- Resource dictionaries
- Accessibility behavior
- Visual validation feedback

The View should not contain:

- Business logic
- Persistence logic
- Domain rules
- Repository access
- Application workflow orchestration
- XML or JSON processing

Code-behind is not automatically forbidden. It is acceptable for behavior that is truly visual, tightly coupled to a specific WPF control, and contains no reusable presentation or business rule. Saving a user, deciding permissions, loading repositories, or applying business validation does not belong in code-behind.

### ViewModel

#### Purpose

The ViewModel is a presentation model and an abstraction of a View. It exposes state and actions in a form that data binding can consume without knowing concrete visual controls.

It solves the problem of keeping UI behavior testable and independent from WPF controls. The ViewModel can be tested without creating a `Window`, `Page`, or `UserControl`.

#### Current implementation

The current ViewModels live in `App4di.Dotnet.UserManager.Presentation`.

Current ViewModels:

- `MainViewModel`
- `LoginViewModel`
- `UserListViewModel`
- `UserViewModel`

They use `FW4di.Dotnet.MVVM` commands and notification support. They call Application contracts such as `IAuthenticationService`, `IUserQueryService`, `ISaveUserUseCase`, `IDeleteUserUseCase`, and `IUserExportService`. They coordinate navigation through `INavigationService`, notifications through `IUserNotificationService`, and edit state through `IUserEditSessionService`.

#### Typical responsibilities in larger applications

Typical ViewModel responsibilities include:

- State management
- Observable properties
- Commands
- Validation state
- Interaction with use cases
- Navigation decisions
- Dialog coordination through abstractions
- Selection state
- Loading and error state
- Mapping to presentation models
- UI availability rules

ViewModels should not contain:

- WPF controls
- Concrete Views
- File access
- Database access
- XML or JSON serialization
- Concrete repository selection
- Infrastructure transactions
- Business invariants that belong in Domain or Application

## Overall Dependency Diagram

<p align="center"><img src="images/architecture/dependencies.svg" alt="UserManager compile-time dependencies"></p>

The arrows represent the main project dependencies. They describe which application layers may reference each other at compile time. They are not the same thing as runtime request flow.

An expanded textual view:

```text
FW4di.Dotnet.Core
  ^
  |
  +-- Application
  +-- Presentation
  +-- Infrastructure
  +-- WPFUI

FW4di.Dotnet.MVVM
  ^
  |
  +-- Presentation

Domain
  ^
  |
  +-- Application
        ^
        |
        +-- Presentation
        +-- Infrastructure
              ^
              |
              +-- WPFUI

WPFUI also references Application and Presentation so it can compose the executable application.
Tests reference the projects they verify.
```

The dependency graph is intentionally acyclic. Domain is not allowed to depend outward. Application business behavior depends on Domain and contracts. Infrastructure depends inward on Application contracts and Domain models. WPFUI sits at the edge and assembles the concrete application.

## Compile-Time Dependencies

The current project references are:

- `Domain` targets `net10.0` and has no project references.
- `Application` targets `net10.0` and references `Domain` and `FW4di.Dotnet.Core`.
- `Presentation` targets `net10.0` and references `Application`, `Domain`, `FW4di.Dotnet.Core`, and `FW4di.Dotnet.MVVM`.
- `Infrastructure` targets `net10.0` and references `Application`, `Domain`, and `FW4di.Dotnet.Core`.
- `WPFUI` targets `net10.0-windows` and references `Application`, `Presentation`, `Infrastructure`, and `FW4di.Dotnet.Core`.
- `Tests` targets `net10.0` and references the projects it verifies.

Dependency direction and runtime flow are not the same. A user action begins in WPFUI at runtime, but compile-time dependencies are still arranged so that inner business code does not know about WPFUI.

## Runtime Flow

At runtime, WPFUI acts as the Composition Root. It wires Presentation and Infrastructure through contracts defined by Application.

<p align="center"><img src="images/architecture/runtime-flow.svg" alt="UserManager runtime flow"></p>

A typical operation follows:

```text
View -> ViewModel -> Application Use Case -> Domain / Infrastructure
```

The result returns as:

```text
Application result -> Presentation state -> property change notification -> WPF binding -> updated View
```

### Startup Flow

1. WPF raises `Application_Startup` in `App.xaml.cs`.
2. `DIBindings.Init` invokes `BindApplication`, `BindPresentation`, `BindInfrastructure`, and `BindWpfUi`.
3. `ViewTypeConverter` is configured to create `LoginView`, `UserListView`, and `UserView` with their ViewModels.
4. Culture is fixed to `en-US` for consistent WPF parsing, validation, and formatting.
5. `MainView` is created with `MainViewModel` as `DataContext`.
6. `mainView.Show()` displays the shell.

### Typical Request Lifecycle

```text
User clicks Save
      |
      v
UserView.xaml binding invokes SaveCommand
      |
      v
UserViewModel validates presentation state and calls ISaveUserUseCase
      |
      v
SaveUserUseCase applies application rules and calls IUserRepository
      |
      v
XmlUserRepository uses XmlDataManager<UserData>
      |
      v
Result returns to UserViewModel
      |
      v
ViewModel updates observable state and navigation
      |
      v
WPF binding refreshes the View
```

### Authentication Flow

`LoginViewModel.LoginCommand` calls `IAuthenticationService.Authenticate`. `AuthenticationService` loads users through `IUserRepository` and compares login name and password. Success navigates to `ViewType.UserList`; failure shows a notification.

### Query Flow

`UserListViewModel` builds `UserQueryCriteria` from `UserFilter`, city selection, and search text. `UserQueryService` loads users through `IUserRepository`, loads cities through `IAddressCityRepository`, and returns filtered `UserData`. Presentation maps `UserData` to bindable `User` models.

### Add / Edit / Save Flow

Add starts `IUserEditSessionService.BeginAdd` with a fresh ID. Edit starts `BeginEdit` with a cloned selected user. `UserViewModel.SaveCommand` calls `ISaveUserUseCase.Execute`. `SaveUserUseCase` validates birth date, checks duplicate login names in the save snapshot, saves through `IUserRepository`, and then calls `CompleteSave`. The original UI list item is updated only after persistence succeeds.

### Cancel Flow

`UserViewModel.CancelCommand` calls `IUserEditSessionService.Cancel` and navigates back. The edit session is cleared without mutating persistent state or the original selected user.

### Delete Flow

`UserListViewModel.DeleteCommand` asks for confirmation through `IUserNotificationService`, calls `IDeleteUserUseCase.Execute`, prevents deleting the last user through the use case, saves the remaining list through `IUserRepository`, and refreshes the displayed list.

### Export Flow

`UserListViewModel.ExportCommand` maps displayed users to `UserData` and calls `IUserExportService.ExportUsers`. `JsonUserExportService` writes a temporary JSON file, moves it over the target file with overwrite, returns the full path, and performs best-effort temporary cleanup.

<p align="center"><img src="images/architecture/flow.svg" alt="UserManager workflow diagram"></p>

## Dependency Injection

Dependency Injection is used to keep required collaborators explicit and replaceable. Classes receive collaborators through constructors instead of creating concrete dependencies internally.

The .NET dependency injection guidance describes DI as a way to achieve inversion of control between classes and their dependencies: register abstractions and implementations at startup, then inject required services where they are used. This repository follows the same principle with the first-party `FW4di.Dotnet.Core` DI abstraction.

The important design choices are:

- Business code does not create concrete Infrastructure classes.
- ViewModels do not create repositories.
- Use cases depend on contracts.
- WPFUI selects concrete implementations at startup.
- Module-owned registration keeps ownership visible.

```text
WPFUI startup
   |
   v
DIBindings.Init
   |
   +--> Application registers use cases and application services
   +--> Presentation registers ViewModels and presentation services
   +--> Infrastructure registers XML/JSON implementations
   +--> WPFUI registers WPF-specific adapters
   |
   v
Resolve MainViewModel and show MainView
```

## Dependency Registration

WPFUI is the application's Composition Root. It starts dependency registration, selects the concrete modules used by the application, wires WPF-specific adapters, and resolves the startup ViewModel and View.

### Current Registration Entry Points

- `BindApplication()` is owned by Application and registers `IAuthenticationService`, `IUserQueryService`, `ISaveUserUseCase`, and `IDeleteUserUseCase`.
- `BindPresentation()` is owned by Presentation and registers `UserAutoMapperManager`, `ISessionService`, `IUserEditSessionService`, `INavigationService`, and the four ViewModels.
- `BindInfrastructure()` is owned by Infrastructure and registers `XmlDataManager<UserData>`, `JsonDataManager<UserData>`, `IUserRepository`, `IAddressCityRepository`, and `IUserExportService`.
- `BindWpfUi()` is owned by WPFUI and registers WPF-specific adapters: `IUserNotificationService` and `IApplicationService`.

### Startup Order

`App.xaml.cs` calls registration in this order:

1. Application
2. Presentation
3. Infrastructure
4. WPFUI

The order is explicit, deterministic, and visible in the Composition Root.

### Why No Global Assembly Scanning

The registration code does not scan all loaded assemblies. Each module registers only its own types. This keeps startup understandable, avoids accidental bindings, keeps module ownership clear, and makes alternative implementations explicit.

Adding an alternative implementation should be done by changing the Composition Root or adding a narrowly named module registration method instead of hiding the application setup behind a generic global binding call.

## Module Responsibilities

This section documents every major architectural module from three perspectives:

- Purpose: why the layer exists and what problem it solves.
- Current implementation: how this repository currently uses the layer.
- Typical responsibilities in larger applications: what usually belongs there in larger MVVM and Clean Architecture-inspired systems.

### FW4di.Dotnet.Core

#### Purpose

`FW4di.Dotnet.Core` is a reusable technical foundation library. It contains framework-independent infrastructure helpers that can be reused by more than one application.

Its purpose is to keep generic technical building blocks outside the UserManager application projects. This prevents the application from mixing reusable infrastructure code with feature-specific business behavior.

#### Current implementation

Current contents include:

- Dependency Injection abstractions and implementation helpers.
- `IDIManager`
- `DIManager`
- `NinjectDIManager`
- `DILifetimeScopes`
- Mapping helpers such as `AutoMapperManager`, `AutoMapperProfile`, and `MappingList`.
- XML helper functionality.
- Mocking helper functionality used by tests.

#### Typical responsibilities in larger applications

Typical reusable Core contents may include:

- Dependency Injection abstractions
- Generic mapping infrastructure
- Generic serialization helpers
- Generic IO helpers
- Cross-application utility primitives
- Test helper primitives
- Framework-neutral base abstractions

`FW4di.Dotnet.Core` should never contain:

- UserManager business rules
- User-specific use cases
- WPF views or ViewModels
- Repository implementations tied to one application feature
- Application-specific file paths
- UI wording
- Domain policies from a specific product

If a type would make no sense outside UserManager, it probably does not belong in `FW4di.Dotnet.Core`.

### FW4di.Dotnet.MVVM

#### Purpose

`FW4di.Dotnet.MVVM` is a reusable MVVM infrastructure library. It contains building blocks that support ViewModels without depending on UserManager business behavior.

It solves the repeated problem of implementing observable properties, commands, and messaging in WPF/MVVM-style applications.

#### Current implementation

Current contents include:

- `NotificationObject`
- `ICommand`
- `RelayCommand`
- `AsyncRelayCommand`
- `Messenger`
- `EventAggregator`

These components support `Presentation` ViewModels, but they contain no UserManager-specific workflows or business rules.

#### Typical responsibilities in larger applications

Reusable MVVM infrastructure may include:

- Observable base classes
- Command implementations
- Async command implementations
- Messenger or event aggregator primitives
- ViewModel lifecycle abstractions
- Validation notification primitives
- Weak event or subscription helpers

Application-specific MVVM code should remain inside the application, not the reusable library. The following should not be placed in `FW4di.Dotnet.MVVM`:

- `LoginViewModel`
- `UserListViewModel`
- `UserViewModel`
- User navigation workflows
- User edit session logic
- Application-specific dialogs
- Business-specific validation messages
- Repository calls

The reusable MVVM library should provide tools. The application should define behavior.

### Domain

#### Purpose

Domain contains the application's business model.

The Domain layer describes what the system is. It should be the most stable part of the application and should remain independent from user interface frameworks, persistence formats, and technical adapters.

#### Current implementation

This sample project intentionally keeps the Domain layer lightweight.

Currently this project contains:

- `UserData`
- Business model
- Domain abstractions

The Domain layer is independent from:

- WPF
- Presentation
- Infrastructure
- XML
- JSON
- Dependency Injection containers
- File paths

The current `UserData` type represents the core user data used by the application. Because the sample is small, many rules live in Application services instead of a rich Domain model.

#### Typical responsibilities in larger applications

In larger applications this layer would additionally contain:

- Business Entities
- Value Objects
- Domain Services
- Domain Events
- Specifications
- Business Rules
- Invariants
- Domain Policies
- Enumerations
- Aggregate Roots
- Factories where appropriate
- Domain-specific exceptions or result types

The Domain layer should not depend on:

- WPF
- UI
- Database libraries
- Persistence frameworks
- HTTP clients
- JSON/XML serializers
- Dependency Injection containers
- Application services
- Infrastructure adapters

### Application

#### Purpose

Application contains operations the application can perform. It expresses use cases, application workflow, orchestration, and contracts required by those use cases.

It solves the problem of keeping business workflows out of ViewModels and out of Infrastructure. A ViewModel can ask the Application layer to save, delete, authenticate, export, or query users without knowing how persistence works.

#### Current implementation

Current examples include:

- `AuthenticationService`
- `IAuthenticationService`
- `UserQueryService`
- `IUserQueryService`
- `UserQueryCriteria`
- `SaveUserUseCase`
- `ISaveUserUseCase`
- `DeleteUserUseCase`
- `IDeleteUserUseCase`
- `BirthDateRules`
- `IUserRepository`
- `IAddressCityRepository`
- `IUserSaveSession`
- `IUserExportService`
- `ApplicationDependencyRegistration`

The Application layer defines repository and export contracts because use cases require those capabilities. Infrastructure implements those contracts.

#### Typical responsibilities in larger applications

Typical Application contents include:

- Use Cases
- Application Services
- DTOs
- Commands
- Queries
- Command handlers
- Query handlers
- Validation
- Repository interfaces
- Unit of Work contracts
- Transaction boundaries
- Orchestration
- Mapping
- Authorization checks
- Result models
- Ports
- Idempotency policies
- Retry policies at the application boundary

Application should not depend on:

- WPF
- ViewModels
- Concrete repositories
- XML or JSON implementation details
- Entity Framework or Dapper implementations
- File paths selected by the UI
- Concrete dialogs

### Infrastructure

#### Purpose

Infrastructure contains concrete technical implementations required by Application contracts. It is the place for details: files, serializers, databases, external services, and other adapters.

It solves the problem of keeping technical volatility away from Domain and Application. XML storage can be replaced by a database-backed repository without rewriting ViewModels or use cases, as long as the Application contracts remain stable.

#### Current implementation

Current examples include:

- `XmlUserRepository`
- `XmlAddressCityRepository`
- `JsonUserExportService`
- `XmlDataManager<T>`
- `JsonDataManager<T>`
- `IDataManager<T>`
- `DataFilePaths`
- File system access
- `InfrastructureDependencyRegistration`

The current persistence implementation uses XML as the application data store and JSON for export. `JsonUserExportService` writes a temporary JSON file, moves it over the target file with overwrite, returns the full path, and performs best-effort temporary cleanup.

#### Typical responsibilities in larger applications

Typical Infrastructure contents include:

- Repository implementations
- XML persistence
- JSON persistence
- Database access
- Entity Framework
- Dapper
- HTTP clients
- Logging
- Configuration
- File system
- External services
- Email adapters
- Message brokers
- Caching
- Cloud storage
- Background jobs
- Identity provider adapters
- Operating system integration

Infrastructure should not define business policy. It may implement storage behavior and translate technical errors, but it should not decide business invariants such as whether a user is valid, whether deletion is allowed, or what an application workflow means.

### Presentation

#### Purpose

Presentation contains ViewModels, presentation models, navigation abstractions, session state, and UI-facing coordination that is independent from concrete WPF controls.

It solves the problem of keeping testable presentation behavior separate from XAML and code-behind. Presentation can be tested without launching WPF.

#### Current implementation

Current examples include:

- `MainViewModel`
- `LoginViewModel`
- `UserListViewModel`
- `UserViewModel`
- `User`
- `AddressCity`
- `UserFilter`
- `INavigationService`
- `NavigationService`
- `ViewType`
- `ISessionService`
- `SessionService`
- `IUserEditSessionService`
- `UserEditSessionService`
- `UserEditMode`
- `IUserNotificationService`
- `IApplicationService`
- `UserAutoMapperManager`
- `ExceptionMessageFormatter`
- `PresentationDependencyRegistration`

Presentation maps Domain/Application data to bindable presentation models and coordinates user workflows through Application contracts.

#### Typical responsibilities in larger applications

Typical Presentation contents include:

- ViewModels
- Navigation
- Dialog service abstractions
- UI coordination
- Presentation services
- Mapping
- Validation adapters
- Bindable presentation models
- Selection state
- Edit sessions
- Screen state machines
- Error formatting
- UI-specific result interpretation

Presentation should not contain:

- Concrete WPF controls
- XAML resources
- File persistence
- Database queries
- XML or JSON processing
- Concrete Infrastructure construction
- Domain rules that should be enforced independently from the UI

### WPFUI

#### Purpose

WPFUI is the concrete WPF application. It contains the executable startup, XAML views, WPF resources, platform adapters, and Composition Root.

It solves the problem of keeping WPF-specific details at the edge of the architecture. WPFUI is allowed to know about WPF because it is the WPF application. Inner layers should not know about WPFUI.

#### Current implementation

Current examples include:

- `App.xaml`
- `App.xaml.cs`
- `MainView`
- `LoginView`
- `UserListView`
- `UserView`
- View code-behind
- `DIBindings`
- `WpfApplicationService`
- `WpfUserNotificationService`
- WPF validation rules such as `BirthDateRule`, `PasswordRule`, and `DataLengthRule`
- `CommandAdapterConverter`
- `ViewTypeConverter`
- `ConverterHelper`
- `ConverterMarkupExtension`
- Resource dictionaries
- Themes
- Styles
- Brushes
- Colors
- Icons and images

WPFUI starts the application, registers modules, configures view creation, creates `MainView`, assigns `MainViewModel`, and shows the shell.

#### Typical responsibilities in larger applications

Typical WPFUI contents include:

- Views
- Resource Dictionaries
- Themes
- Styles
- Templates
- App.xaml
- Bootstrap
- Composition Root
- Platform adapters
- WPF converters
- WPF validation rules
- Window management
- Visual assets
- Control-specific code-behind
- Accessibility resources

WPFUI may reference outer and inner modules because it composes the final executable application. That does not give inner modules permission to depend back on WPFUI.

### Tests

#### Purpose

Tests verify behavior and protect architectural boundaries. They make the sample useful as a long-term reference because the architecture is not only documented; it is exercised.

#### Current implementation

The current test project is `Project/App4di.Dotnet.UserManager.Tests` and targets `net10.0` with MSTest. It references Domain, Application, Infrastructure, Presentation, and `FW4di.Dotnet.Core`.

Current coverage includes:

- Domain: `UserDataTests`.
- Application: authentication, user queries, birth date rules, save use case, delete use case.
- Presentation: ViewModels, navigation, session service, edit session service, exception message formatting, bindable models.
- Infrastructure: data managers, XML repositories, JSON export service.
- Configuration: dependency registration tests.

Application tests use stubs for repository contracts. Presentation tests use stubs for application services, notifications, export, delete, and repositories where needed. Infrastructure tests touch file-based XML/JSON behavior and clean up test files.

#### Typical responsibilities in larger applications

Typical test coverage may include:

- Domain unit tests
- Application use case tests
- Presentation ViewModel tests
- Infrastructure integration tests
- Dependency registration tests
- Architecture dependency tests
- Contract tests for adapters
- WPF UI automation tests
- End-to-end workflow tests

Possible future additions include explicit architecture dependency tests, broader integration tests, WPF UI automation tests, and end-to-end tests. These should be documented as future extensions unless they are actually added.

Run tests with:

```bash
dotnet test Project/App4di.Dotnet.UserManager.Windows.slnx
```

## Design Guidelines

Use these rules when extending the application.

### Preserve Layer Boundaries

- Do not place business logic inside ViewModels.
- Do not access repositories directly from Presentation.
- Do not perform XML, JSON, or file operations from ViewModels.
- Keep Infrastructure free from UI dependencies.
- Keep Domain independent from framework-specific code whenever practical.
- Prefer introducing new Application use cases instead of growing ViewModels.
- Add unit tests whenever business behavior changes.
- Preserve the dependency direction between layers.

### ViewModels

ViewModels may expose bindable state, commands, selected items, UI availability, user-facing errors, and navigation decisions. They may call Application contracts such as `IUserQueryService`, `ISaveUserUseCase`, `IDeleteUserUseCase`, `IUserExportService`, and `IAuthenticationService`.

ViewModels must not create XML, write JSON, choose `XmlUserRepository`, perform file access, contain WPF controls, or implement business invariants that belong in Application or Domain.

### Repositories and Persistence

Repository contracts belong in Application when use cases need persistence. Implementations belong in Infrastructure. Persistence-specific details such as `DataFilePaths`, `XmlDataManager<T>`, and `JsonDataManager<T>` must stay out of Presentation and Domain.

### Domain Independence

Domain types should remain stable business concepts. `UserData` must not learn about WPF, XML, JSON, ViewModels, DI containers, or file paths.

### When to Add a Use Case

Add a use case when an operation coordinates business validation, persistence, workflow order, or cross-layer contracts. Do not add a use case only to wrap one trivial property assignment inside a ViewModel.

### Good and Bad Examples

Good: `UserViewModel` calls `ISaveUserUseCase.Execute(userEditSessionService)` and handles navigation/error display.

Bad: `UserViewModel` creates `XmlUserRepository`, serializes XML, mutates the original user before persistence succeeds, and then catches file errors.

Good: add an `IUserRepository` implementation in Infrastructure and bind it at composition time.

Bad: make Application depend on `XmlUserRepository` because the current storage is XML.

## Dependency Rules

The dependency rules are the backbone of the repository.

- Domain must not depend on any other UserManager project.
- Application business code may depend only on Domain. Its dependency registration entry point additionally depends on `FW4di.Dotnet.Core`.
- Presentation may depend on Application, Domain, `FW4di.Dotnet.MVVM`, and `FW4di.Dotnet.Core` for registration.
- Infrastructure may depend on Application and Domain, plus `FW4di.Dotnet.Core` for technical helpers and dependency registration.
- WPFUI composes the application and may reference Application, Presentation, Infrastructure, and `FW4di.Dotnet.Core`.
- Tests may reference the projects they verify.

These rules support dependency inversion:

```text
Application owns IUserRepository
Infrastructure implements XmlUserRepository
WPFUI registers IUserRepository -> XmlUserRepository
Use cases receive IUserRepository through constructor injection
```

Application therefore uses persistence without depending on XML persistence.

## Extension Points

The preferred way to add behavior is to extend the appropriate architectural boundary instead of bypassing it.

### Add a New Use Case

Add a use case when an operation coordinates business validation, persistence, workflow order, or cross-layer contracts. Place the contract and implementation in Application. Register it in `BindApplication()`. Call it from a ViewModel through the contract.

### Add a New Repository Implementation

Place the contract in Application when use cases require the persistence capability. Place the implementation in Infrastructure. Register the selected implementation in `BindInfrastructure()` or through a narrowly named alternative registration method.

### Add a New Screen

Add the ViewModel and presentation behavior in Presentation. Add XAML and WPF-specific resources in WPFUI. Register the ViewModel in `BindPresentation()` and configure view creation in WPFUI.

### Add a New Export Format

Keep the export contract in Application if use cases or ViewModels depend on the capability. Add the concrete writer in Infrastructure. Register the selected implementation in composition.

### Add New Domain Behavior

Place stable business concepts in Domain when they describe what the system is and should remain independent from UI and persistence. Place application workflow in Application when the behavior describes what the system does.

Detailed step-by-step recipes remain separate in the [Cookbook](cookbook/README.md).

## Source Recommendations

The repository does not copy external architecture sources, but it follows widely accepted recommendations summarized from primary and long-standing references:

- Microsoft Learn: WPF data binding and MVVM guidance recommends using XAML binding, commands, and ViewModels to decouple UI and non-UI code.
- Microsoft Learn: .NET dependency injection guidance recommends registering services at startup and injecting required dependencies rather than hard-coding concrete collaborators.
- Martin Fowler: Presentation Model describes moving presentation state and behavior out of GUI widgets, which closely matches the ViewModel role in MVVM.
- Robert C. Martin: Clean Architecture emphasizes separation of concerns and the Dependency Rule, where source dependencies point inward toward business policy and away from frameworks and drivers.

References:

- [Data binding and MVVM - Microsoft Learn](https://learn.microsoft.com/en-us/windows/uwp/data-binding/data-binding-and-mvvm)
- [WPF data binding overview - Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/)
- [.NET dependency injection - Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/overview)
- [Presentation Model - Martin Fowler](https://martinfowler.com/eaaDev/PresentationModel.html)
- [The Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## Summary

UserManager is a working WPF sample and an architecture reference. Its most valuable asset is not the amount of product documentation around it, but the clarity of the architecture itself.

The central rules are:

- Keep Domain independent.
- Put application workflows in Application.
- Put technical implementations in Infrastructure.
- Put testable presentation behavior in Presentation.
- Put concrete WPF views and composition in WPFUI.
- Keep reusable technical primitives in `FW4di.Dotnet.Core`.
- Keep reusable MVVM primitives in `FW4di.Dotnet.MVVM`.
- Use dependency injection to connect contracts to implementations at the edge.
- Preserve the dependency direction when adding new features.

New features should fit into these boundaries instead of weakening them. If a change forces WPF, XML, JSON, or concrete repositories into Domain or Application policy, the design should be revisited before the implementation grows around the wrong dependency.
