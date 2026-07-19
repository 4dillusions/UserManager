# Dependency Registration

[Documentation index](README.md) · [Magyar változat](../hu/dependency-registration.md)

WPFUI is the application's Composition Root. It starts dependency registration, selects the concrete modules used by the application, wires WPF-specific adapters, and resolves the startup ViewModel and View.

## Current Registration Entry Points

- `BindApplication()` is owned by Application and registers `IAuthenticationService`, `IUserQueryService`, `ISaveUserUseCase`, and `IDeleteUserUseCase`.
- `BindPresentation()` is owned by Presentation and registers `UserAutoMapperManager`, `ISessionService`, `IUserEditSessionService`, `INavigationService`, and the four ViewModels.
- `BindInfrastructure()` is owned by Infrastructure and registers `XmlDataManager<UserData>`, `JsonDataManager<UserData>`, `IUserRepository`, `IAddressCityRepository`, and `IUserExportService`.
- `BindWpfUi()` is owned by WPFUI and registers WPF-specific adapters: `IUserNotificationService` and `IApplicationService`.

## Startup Order

`App.xaml.cs` calls registration in this order: Application, Presentation, Infrastructure, WPFUI. The order is explicit, deterministic, and visible in the Composition Root.

## Why No Global Assembly Scanning

The registration code does not scan all loaded assemblies. Each module registers only its own types. This keeps startup understandable, avoids accidental bindings, keeps module ownership clear, and makes alternative implementations explicit.

Adding an alternative implementation should be done by changing the Composition Root or adding a narrowly named module registration method instead of hiding the application setup behind a generic global binding call.

Related: [Architecture](architecture.md), [ADR-0002](decisions/0002-composition-root.md), [ADR-0003](decisions/0003-module-registration.md).
