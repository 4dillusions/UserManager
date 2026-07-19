# Architecture

[Documentation index](README.md) · [Magyar változat](../hu/architecture.md)

UserManager follows a layered architecture inspired by Clean Architecture and MVVM principles while intentionally avoiding unnecessary complexity. Each layer has a single responsibility and communicates only through well-defined contracts.

## Architectural Goals

- Clear separation of responsibilities.
- Business logic independent from WPF.
- Business logic independent from persistence.
- Dependency inversion through Application contracts.
- Thin ViewModels responsible only for UI orchestration.
- Infrastructure containing only technical implementations.
- High testability through dependency injection and focused services.

## Layered Architecture

The application is organized into `Domain`, `Application`, `Infrastructure`, `Presentation`, `WPFUI`, `Tests`, `FW4di.Dotnet.Core`, and `FW4di.Dotnet.MVVM`. The split is intentionally practical: enough structure to demonstrate clean boundaries without introducing a full enterprise framework.

<p align="center"><img src="../images/architecture/dependencies.svg" alt="UserManager compile-time dependencies"></p>

The arrows represent the main project dependencies. They describe which application layers may reference each other at compile time, not every shared framework dependency and not how requests flow through the application at runtime.

<p align="center"><img src="../images/architecture/architecture.jpg" alt="UserManager layered architecture"></p>

## Dependency Rules

- Domain must not depend on any other project.
- Application business code may depend only on Domain. Its dependency registration entry point additionally depends on `FW4di.Dotnet.Core`.
- Presentation may depend on Application, Domain, `FW4di.Dotnet.MVVM`, and `FW4di.Dotnet.Core` for registration.
- Infrastructure may depend on Application and Domain, plus `FW4di.Dotnet.Core` for technical helpers and dependency registration.
- WPFUI composes the application and may reference Application, Presentation, Infrastructure and `FW4di.Dotnet.Core`.

## Separation of Responsibilities

| Layer | Responsibility |
|---|---|
| `Domain` | Stable business concepts and rules. |
| `Application` | Use cases, workflow coordination, and contracts required by use cases. |
| `Infrastructure` | Concrete technical implementations such as XML repositories and JSON export. |
| `Presentation` | ViewModels, UI state, navigation, session state, and presentation mapping. |
| `WPFUI` | WPF views, XAML, WPF validation, converters, platform adapters, startup and composition. |
| `FW4di.Dotnet.Core` | Reusable Dependency Injection and general technical helpers. |
| `FW4di.Dotnet.MVVM` | Reusable MVVM support such as commands, notification, and messaging primitives. |

## Compile-Time Dependencies

The current project references are:

- `Domain` targets `net10.0` and has no project references.
- `Application` targets `net10.0` and references `Domain` and `FW4di.Dotnet.Core`.
- `Presentation` targets `net10.0` and references `Application`, `Domain`, `FW4di.Dotnet.Core`, and `FW4di.Dotnet.MVVM`.
- `Infrastructure` targets `net10.0` and references `Application`, `Domain`, and `FW4di.Dotnet.Core`.
- `WPFUI` targets `net10.0-windows` and references `Application`, `Presentation`, `Infrastructure`, and `FW4di.Dotnet.Core`.
- `Tests` targets `net10.0` and references the projects it verifies.

## Runtime Composition

At runtime, WPFUI acts as the Composition Root. It wires Presentation and Infrastructure through contracts defined by Application.

<p align="center"><img src="../images/architecture/runtime-flow.svg" alt="UserManager runtime flow"></p>

A typical operation follows `View -> ViewModel -> Application Use Case -> Domain / Infrastructure`, then returns through Application results, Presentation state, property change notification, and WPF binding.

## MVVM and Clean Architecture

MVVM separates the user interface, presentation state, and application behavior. Layered and Clean Architecture define responsibility boundaries and dependency direction across the whole system. Clean Code and SOLID guide the internal design of modules, classes, methods, and contracts.

| MVVM role | Current modules |
|---|---|
| Model | `Domain`, `Application`, and `Infrastructure` |
| ViewModel | `Presentation`, supported by `FW4di.Dotnet.MVVM` |
| View | `WPFUI` |
| Composition and startup | `WPFUI`, supported by `FW4di.Dotnet.Core` |
| Verification | `Tests` |

The modules do not each implement a separate MVVM pattern. They cooperate to implement the three MVVM roles.

Related: [Modules and Layers](modules-and-layers.md), [Dependency Registration](dependency-registration.md), [Runtime Flow](runtime-flow.md).
