# ADR-0003: Module-Owned Dependency Registration

[Magyar változat](0003-module-registration_HU.md) · [ADR index](README.md) · [Architecture handbook](../architecture.md)

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
