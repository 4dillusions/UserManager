# Új Repository hozzáadása

[Cookbook](README.md) · [Dokumentációs index](../README.md) · [English version](../../en/cookbook/add-new-repository.md)

Repository contract Applicationbe kerüljön, amikor use case-nek persistence-re van szüksége. A konkrét persistence implementation Infrastructure-ben legyen, Domain modellek használatával a boundaryn. Regisztráció: `BindInfrastructure()`, alternatíva választása a Composition Rootban. Application behavior stubokkal, Infrastructure behavior file/database részletekkel tesztelendő.

Kapcsolódó oldalak: [Dependency Registration](../dependency-registration.md), [Design Guidelines](../design-guidelines.md).
