# ADR-0004: Transactional Edit Session

[ADR index](README.md) · [Dokumentációs index](../README.md) · [English version](../../en/decisions/0004-edit-session.md)

## Status

Accepted

## Context

A UserManager egyszerre WPF mintaalkalmazás és újrahasznosítható architekturális referencia. Az architektúrának érthetőnek, tesztelhetőnek és a jelenlegi implementációval összhangban lévőnek kell maradnia.

## Decision

Az edit workflow a `UserEditSessionService` szolgáltatást, cloned editable state-et, save snapshotot, `CompleteSave` és `Cancel` lépéseket használ.

## Consequences

### Positive

Az eredeti selected user izolált marad, amíg a persistence sikeresen le nem fut. A Cancel úgy törli a sessiont, hogy nem módosít persistent vagy selected state-et.

### Trade-offs

A session service explicitebb, mint a selected object közvetlen szerkesztése.
