# ADR-0004: Transactional Edit Session

[🇭🇺 Magyar változat](0004-edit-session_HU.md) · [🇬🇧 ADR index](README.md) · [🇬🇧 Architecture handbook](../architecture.md)

## Status

Accepted

## Context

UserManager is both a WPF sample and a reusable architecture reference. The architecture must stay understandable, testable, and faithful to the current implementation.

## Decision

Editing uses `UserEditSessionService`, cloned editable state, save snapshots, `CompleteSave`, and `Cancel`.

## Consequences

### Positive

The original selected user is isolated until persistence succeeds. Cancel clears the session without mutating persistent or selected state.

### Trade-offs

The session service is more explicit than editing the selected object directly.
