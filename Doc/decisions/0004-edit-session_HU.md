# ADR-0004: Tranzakcionális Edit Session

[English version](0004-edit-session.md) · [ADR index](README_HU.md) · [Architektúra kézikönyv](../architecture_HU.md)

## Status

Accepted

## Context

A user edit workflow-nak támogatnia kell a Save / Cancel viselkedést anélkül, hogy a listában szereplő eredeti user vagy a persistent state idő előtt módosulna.

## Decision

A Presentation réteg edit session service-t használ. Add friss user példánnyal indul, Edit pedig clone-olt userrel. A save use case csak sikeres persistence után fejezi be a sessiont.

## Consequences

### Positive

A Cancel biztonságos, a UI state nem szennyezi a persistent state-et, és a save workflow tesztelhető marad.

### Trade-offs

A szerkesztéshez külön session state és mapping szükséges.
