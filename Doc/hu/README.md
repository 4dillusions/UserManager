# UserManager Dokumentáció

[English version](../en/README.md)

Ez a dokumentáció a UserManager projekt részletes technikai referenciája. Egyszerre szolgál projekt dokumentációként és újrahasznosítható WPF/MVVM architekturális tudásbázisként.

## Architektúra

- [Architektúra](architecture.md) - áttekintés, layered architecture, dependency direction, diagramok, valamint az MVVM és a Clean Architecture kapcsolata.
- [Modulok és rétegek](modules-and-layers.md) - MVVM, Clean Code, SOLID és az aktuális projektek felelősségei.
- [Dependency Registration](dependency-registration.md) - WPFUI mint Composition Root, modulonkénti registration, sorrend és dependency szabályok.
- [Runtime Flow](runtime-flow.md) - startup, authentication, query, add/edit/save, delete és export folyamatok.

## Design

- [Design Guidelines](design-guidelines.md) - szabályok az alkalmazás architektúrájának megőrzéséhez.
- [Testing](testing.md) - jelenlegi test struktúra, lefedettség, határok és javasolt későbbi test stratégia.

## Cookbook

- [Cookbook Index](cookbook/README.md)
- [Új Use Case hozzáadása](cookbook/add-new-use-case.md)
- [Új ViewModel hozzáadása](cookbook/add-new-viewmodel.md)
- [Új Repository hozzáadása](cookbook/add-new-repository.md)
- [Új modul hozzáadása](cookbook/add-new-module.md)
- [Új screen hozzáadása](cookbook/add-new-screen.md)

## Architecture Decision Records

- [ADR Index](decisions/README.md)
- [ADR-0001: Layered Architecture](decisions/0001-layered-architecture.md)
- [ADR-0002: WPFUI as Composition Root](decisions/0002-composition-root.md)
- [ADR-0003: Module-Owned Dependency Registration](decisions/0003-module-registration.md)
- [ADR-0004: Transactional Edit Session](decisions/0004-edit-session.md)
- [ADR-0005: Dependency Rules](decisions/0005-dependency-rules.md)

## Képek

A dokumentációs assetek a `Doc/images/branding`, `Doc/images/screenshots` és `Doc/images/architecture` könyvtárakban vannak.
