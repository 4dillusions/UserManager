# Új Repository hozzáadása

[🇬🇧 English version](add-new-repository.md) · [🇭🇺 Receptgyűjtemény](README_HU.md) · [🇭🇺 Architektúra kézikönyv](../architecture_HU.md)

A repository contract az Application rétegbe kerüljön, amikor egy use case-nek persistence capabilityre van szüksége. A konkrét implementáció az Infrastructure rétegbe kerüljön. Az implementáció függhet XML-től, JSON-től, adatbázistól vagy file systemtől, de ezek a részletek nem szivároghatnak vissza Application, Presentation vagy Domain kódba. Regisztráld a kiválasztott implementációt a `BindInfrastructure()` metódusban, és adj hozzá Application teszteket fake contracttal, illetve Infrastructure teszteket a konkrét adapterhez.

Kapcsolódó oldal: [🇭🇺 Architektúra kézikönyv](../architecture_HU.md).
