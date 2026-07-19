# ADR-0003: Module-Owned Dependency Registration

[ADR index](README.md) · [Dokumentációs index](../README.md) · [English version](../../en/decisions/0003-module-registration.md)

## Status

Accepted

## Context

A UserManager egyszerre WPF mintaalkalmazás és újrahasznosítható architekturális referencia. Az architektúrának érthetőnek, tesztelhetőnek és a jelenlegi implementációval összhangban lévőnek kell maradnia.

## Decision

Minden modul saját, szűken scoped `Bind...()` metódussal regisztrálja a saját típusait. A globális assembly scanning kerülendő.

## Consequences

### Positive

A registration discoverable, determinisztikus, és annak a modulnak az ownershipje alatt marad, amely az implementationt tartalmazza.

### Trade-offs

Alternatív implementation hozzáadása explicit composition módosítást igényel.
