# ADR-0001: Layered Architecture

[ADR index](README.md) · [Documentation index](../README.md) · [Magyar változat](../../hu/decisions/0001-layered-architecture.md)

## Status

Accepted

## Context

UserManager is both a WPF sample and a reusable architecture reference. The architecture must stay understandable, testable, and faithful to the current implementation.

## Decision

Use Domain, Application, Infrastructure, Presentation, and WPFUI with explicit dependency direction.

## Consequences

### Positive

This keeps WPF and persistence details away from business workflow, makes use cases testable, and gives the sample a reusable reference structure.

### Trade-offs

More projects and contracts exist than in a minimal WPF sample.
