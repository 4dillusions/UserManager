# Design Guidelines

[Documentation index](README.md) · [Magyar változat](../hu/design-guidelines.md)

Use these rules when extending the application.

## Preserve Layer Boundaries

- Do not place business logic inside ViewModels.
- Do not access repositories directly from Presentation.
- Do not perform XML, JSON or file operations from ViewModels.
- Keep Infrastructure free from UI dependencies.
- Keep Domain independent from framework-specific code whenever practical.
- Prefer introducing new Application use cases instead of growing ViewModels.
- Add unit tests whenever business behavior changes.
- Preserve the dependency direction between layers.

## ViewModels

ViewModels may expose bindable state, commands, selected items, UI availability, user-facing errors, and navigation decisions. They may call Application contracts such as `IUserQueryService`, `ISaveUserUseCase`, `IDeleteUserUseCase`, `IUserExportService`, and `IAuthenticationService`.

ViewModels must not create XML, write JSON, choose `XmlUserRepository`, perform file access, contain WPF controls, or implement business invariants that belong in Application or Domain.

## Repositories and Persistence

Repository contracts belong in Application when use cases need persistence. Implementations belong in Infrastructure. Persistence-specific details such as `DataFilePaths`, `XmlDataManager<T>`, and `JsonDataManager<T>` must stay out of Presentation and Domain.

## Domain Independence

Domain types should remain stable business concepts. `UserData` must not learn about WPF, XML, JSON, ViewModels, DI containers, or file paths.

## When to Add a Use Case

Add a Use Case when an operation coordinates business validation, persistence, workflow order, or cross-layer contracts. Do not add a use case only to wrap one trivial property assignment inside a ViewModel.

## Good and Bad Examples

Good: `UserViewModel` calls `ISaveUserUseCase.Execute(userEditSessionService)` and handles navigation/error display.

Bad: `UserViewModel` creates `XmlUserRepository`, serializes XML, mutates the original user before persistence succeeds, and then catches file errors.

Good: add an `IUserRepository` implementation in Infrastructure and bind it at composition time.

Bad: make Application depend on `XmlUserRepository` because the current storage is XML.

Related: [Cookbook](cookbook/README.md), [Dependency Registration](dependency-registration.md).

