# ADR-0005: Dependency szabályok

[English version](0005-dependency-rules.md) · [ADR index](README_HU.md) · [Architektúra kézikönyv](../architecture_HU.md)

## Status

Accepted

## Context

A repository hosszú távú architektúra referencia. A dependency directionnek explicitnek kell lennie, hogy a minta később is tanulható és bővíthető maradjon.

## Decision

A dependencyk a stabilabb policy felé mutatnak. A Domain nem függ más UserManager projekttől. Az Application Domainre és contractokra épül. Infrastructure Application contractokat implementál. Presentation Application behaviort használ. WPFUI végzi a compositiont.

## Consequences

### Positive

A business behavior nem függ WPF-től, XML-től, JSON-től vagy konkrét repositoryktól. A use case-ek és ViewModelek izoláltabban tesztelhetők.

### Trade-offs

A határok fenntartása fegyelmet igényel, és néha több explicit contractot jelent.
