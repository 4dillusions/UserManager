# Testing

[Documentation index](README.md) · [Magyar változat](../hu/testing.md)

The current test project is `Project/App4di.Dotnet.UserManager.Tests` and targets `net10.0` with MSTest. It references Domain, Application, Infrastructure, Presentation, and `FW4di.Dotnet.Core`.

## Current Coverage

- Domain: `UserDataTests`.
- Application: authentication, user queries, birth date rules, save use case, delete use case.
- Presentation: ViewModels, navigation, session service, edit session service, exception message formatting, bindable models.
- Infrastructure: data managers, XML repositories, JSON export service.
- Configuration: dependency registration tests.

## Current Boundaries

Application tests use stubs for repository contracts. Presentation tests use stubs for application services, notifications, export, delete, and repositories where needed. Infrastructure tests touch file-based XML/JSON behavior and clean up test files.

## Future Strategy

Possible future additions include explicit architecture dependency tests, broader integration tests, WPF UI automation tests, and end-to-end tests. These should be documented as future extensions unless they are actually added.

## Test Command

```bash
dotnet test Project/App4di.Dotnet.UserManager.Windows.slnx
```

