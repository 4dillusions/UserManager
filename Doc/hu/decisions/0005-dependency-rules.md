# ADR-0005: Dependency Rules

[ADR index](README.md) · [Dokumentációs index](../README.md) · [English version](../../en/decisions/0005-dependency-rules.md)

## Status

Accepted

## Context

A UserManager egyszerre WPF mintaalkalmazás és újrahasznosítható architekturális referencia. Az architektúrának érthetőnek, tesztelhetőnek és a jelenlegi implementációval összhangban lévőnek kell maradnia.

## Decision

Meg kell őrizni az aktuális project reference szabályokat: Domain nem függ semmitől; Application Domain/Core; Presentation Application/Domain/Core/MVVM; Infrastructure Application/Domain/Core; WPFUI Application/Presentation/Infrastructure/Core.

## Consequences

### Positive

A szabályok függetlenítik a policyt a technikai részletektől, és láthatóvá teszik a dependency violationöket.

### Trade-offs

A shared framework moduloknak technikai és reusable moduloknak kell maradniuk, nem válhatnak rejtett business layeré.
