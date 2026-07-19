# ADR-0002: WPFUI mint Composition Root

[English version](0002-composition-root.md) · [ADR index](README_HU.md) · [Architektúra kézikönyv](../architecture_HU.md)

## Status

Accepted

## Context

A UserManager egyszerre WPF mintaalkalmazás és újrahasznosítható architektúra referencia. Az architektúrának érthetőnek, tesztelhetőnek és a jelenlegi implementációhoz hűnek kell maradnia.

## Decision

A WPFUI végzi a végső compositiont, és összeköti a module registrationöket, platform service-eket, startup ViewModelt és View-kat.

## Consequences

### Positive

Az alsóbb modulok nem építik fel a konkrét dependency graphot. A platform-specific adapterek a WPF szélen maradnak, a startup pedig explicit.

### Trade-offs

A WPFUI több projectet referál, mert ez az executable Composition Root.
