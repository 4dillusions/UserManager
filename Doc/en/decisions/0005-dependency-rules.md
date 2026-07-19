# ADR-0005: Dependency Rules

[ADR index](README.md) · [Documentation index](../README.md) · [Magyar változat](../../hu/decisions/0005-dependency-rules.md)

## Status

Accepted

## Context

UserManager is both a WPF sample and a reusable architecture reference. The architecture must stay understandable, testable, and faithful to the current implementation.

## Decision

Preserve the actual project reference rules: Domain depends on nothing; Application depends on Domain/Core; Presentation depends on Application, Domain, Core, and MVVM; Infrastructure depends on Application, Domain, Core; WPFUI composes Application, Presentation, Infrastructure, and Core.

## Consequences

### Positive

The rules keep policy independent from technical details and make dependency violations visible.

### Trade-offs

Shared framework modules must stay technical and reusable rather than becoming hidden business layers.
