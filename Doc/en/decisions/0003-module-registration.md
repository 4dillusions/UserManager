# ADR-0003: Module-Owned Dependency Registration

[ADR index](README.md) · [Documentation index](../README.md) · [Magyar változat](../../hu/decisions/0003-module-registration.md)

## Status

Accepted

## Context

UserManager is both a WPF sample and a reusable architecture reference. The architecture must stay understandable, testable, and faithful to the current implementation.

## Decision

Each module owns a narrowly scoped `Bind...()` method for its own types. Global assembly scanning is avoided.

## Consequences

### Positive

Registration is discoverable, deterministic, and owned by the module that contains the implementation.

### Trade-offs

Adding alternatives requires explicit composition changes.
