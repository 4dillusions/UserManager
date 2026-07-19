# ADR-0001: Layered Architecture

[ADR index](README.md) · [Dokumentációs index](../README.md) · [English version](../../en/decisions/0001-layered-architecture.md)

## Status

Accepted

## Context

A UserManager egyszerre WPF mintaalkalmazás és újrahasznosítható architekturális referencia. Az architektúrának érthetőnek, tesztelhetőnek és a jelenlegi implementációval összhangban lévőnek kell maradnia.

## Decision

A rendszer `Domain`, `Application`, `Infrastructure`, `Presentation` és `WPFUI` rétegeket használ explicit dependency directionnel.

## Consequences

### Positive

Ez távol tartja a WPF és persistence részleteket az üzleti workflow-tól, tesztelhetővé teszi a use case-eket, és reusable reference struktúrát ad a mintaprojektnek.

### Trade-offs

Több projekt és contract létezik, mint egy minimális WPF mintában.
