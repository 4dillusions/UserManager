# ADR-0001: Layered Architecture

[🇬🇧 English version](0001-layered-architecture.md) · [🇭🇺 ADR index](README_HU.md) · [🇭🇺 Architektúra kézikönyv](../architecture_HU.md)

## Status

Accepted

## Context

A UserManager egyszerre WPF mintaalkalmazás és újrahasznosítható architektúra referencia. Az architektúrának érthetőnek, tesztelhetőnek és a jelenlegi implementációhoz hűnek kell maradnia.

## Decision

A solution Domain, Application, Infrastructure, Presentation és WPFUI rétegeket használ explicit dependency directionnel.

## Consequences

### Positive

Ez távol tartja a WPF és persistence részleteket az üzleti workflow-tól, tesztelhetővé teszi a use case-eket, és újrahasznosítható referencia struktúrát ad a mintának.

### Trade-offs

Több project és contract létezik, mint egy minimális WPF mintában.
