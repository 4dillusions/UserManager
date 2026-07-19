# ADR-0002: WPFUI as Composition Root

[Magyar változat](0002-composition-root_HU.md) · [ADR index](README.md) · [Architecture handbook](../architecture.md)

## Status

Accepted

## Context

UserManager is both a WPF sample and a reusable architecture reference. The architecture must stay understandable, testable, and faithful to the current implementation.

## Decision

WPFUI performs final composition and wires module registrations, platform services, startup ViewModel, and Views.

## Consequences

### Positive

Lower-level modules do not create the concrete dependency graph. Platform-specific adapters remain at the WPF edge and startup stays explicit.

### Trade-offs

WPFUI references several projects because composition is intentionally centralized there.
