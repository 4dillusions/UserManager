# ADR-0002: WPFUI as Composition Root

[ADR index](README.md) · [Dokumentációs index](../README.md) · [English version](../../en/decisions/0002-composition-root.md)

## Status

Accepted

## Context

A UserManager egyszerre WPF mintaalkalmazás és újrahasznosítható architekturális referencia. Az architektúrának érthetőnek, tesztelhetőnek és a jelenlegi implementációval összhangban lévőnek kell maradnia.

## Decision

A WPFUI végzi a final compositiont: meghívja a module registration metódusokat, beköti a platform service-eket, beállítja a startup ViewModelt és a View-kat.

## Consequences

### Positive

Az alsóbb modulok nem hozzák létre a konkrét dependency graphot. A platform-specific adapterek a WPF szélen maradnak, a startup pedig explicit.

### Trade-offs

A WPFUI több projektet referál, mert a composition szándékosan központosított.
