# ADR-0003: Modulonkénti Dependency Registration

[🇬🇧 English version](0003-module-registration.md) · [🇭🇺 ADR index](README_HU.md) · [🇭🇺 Architektúra kézikönyv](../architecture_HU.md)

## Status

Accepted

## Context

A Dependency Injection setupnak érthetőnek kell maradnia, és a modul ownershipnek látszania kell a codebase-ben.

## Decision

Minden modul saját, szűk registration extension methodot biztosít, például `BindApplication`, `BindPresentation` és `BindInfrastructure`. A WPFUI explicit sorrendben hívja ezeket.

## Consequences

### Positive

A registration ownership világos, nincs rejtett global assembly scanning, az alternatív implementációk explicit módosításokat igényelnek.

### Trade-offs

Egy új service hozzáadásakor frissíteni kell a megfelelő module registration metódust.
