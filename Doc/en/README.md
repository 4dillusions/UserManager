# UserManager Documentation

[Magyar változat](../hu/README.md)

This documentation is the detailed technical reference for the UserManager project. It is organized as both project documentation and a reusable architecture knowledge base for future WPF/MVVM applications.

## Architecture

- [Architecture](architecture.md) - architectural overview, layered architecture, dependency direction, diagrams, and the relationship between MVVM and Clean Architecture.
- [Modules and Layers](modules-and-layers.md) - detailed explanation of MVVM, Clean Code, SOLID, and the responsibilities of every current project.
- [Dependency Registration](dependency-registration.md) - WPFUI as Composition Root, module-owned registration, registration order, and dependency rules.
- [Runtime Flow](runtime-flow.md) - startup, authentication, query, add/edit/save, delete, and export flows.

## Design

- [Design Guidelines](design-guidelines.md) - rules for extending the application without damaging the architecture.
- [Testing](testing.md) - current test structure, current coverage, boundaries, and recommended future test strategy.

## Cookbook

- [Cookbook Index](cookbook/README.md)
- [Add a New Use Case](cookbook/add-new-use-case.md)
- [Add a New ViewModel](cookbook/add-new-viewmodel.md)
- [Add a New Repository](cookbook/add-new-repository.md)
- [Add a New Module](cookbook/add-new-module.md)
- [Add a New Screen](cookbook/add-new-screen.md)

## Architecture Decision Records

- [ADR Index](decisions/README.md)
- [ADR-0001: Layered Architecture](decisions/0001-layered-architecture.md)
- [ADR-0002: WPFUI as Composition Root](decisions/0002-composition-root.md)
- [ADR-0003: Module-Owned Dependency Registration](decisions/0003-module-registration.md)
- [ADR-0004: Transactional Edit Session](decisions/0004-edit-session.md)
- [ADR-0005: Dependency Rules](decisions/0005-dependency-rules.md)

## Images

Documentation assets are stored under `Doc/images/branding`, `Doc/images/screenshots`, and `Doc/images/architecture`.
