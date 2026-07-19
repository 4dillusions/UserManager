# Design Guidelines

[Dokumentációs index](README.md) · [English version](../en/design-guidelines.md)

Ezeket a szabályokat kövesd az alkalmazás bővítésekor.

## Layer boundaryk megőrzése

- Ne kerüljön business logic ViewModelbe.
- A Presentation ne érjen el közvetlenül repositorykat.
- ViewModel ne végezzen XML, JSON vagy file műveletet.
- Az Infrastructure maradjon UI dependencyktől mentes.
- A Domain lehetőség szerint maradjon framework-specific kódtól független.
- ViewModel növelése helyett vezess be Application use case-t.
- Business behavior változásakor adj hozzá unit testet.
- Őrizd meg a rétegek dependency directionjét.

## ViewModelek

A ViewModel expose-olhat bindable state-et, commandokat, selected itemet, UI availabilityt, user-facing errort és navigation döntést. Hívhat Application contractokat, például `IUserQueryService`, `ISaveUserUseCase`, `IDeleteUserUseCase`, `IUserExportService`, `IAuthenticationService`.

A ViewModel nem hozhat létre XML-t, nem írhat JSON-t, nem választhat `XmlUserRepository` implementationt, nem végezhet file accesst, nem tartalmazhat WPF controlt, és nem implementálhat Application vagy Domain szintű business invariantot.

## Repository és persistence

Repository contract akkor tartozik Applicationbe, ha use case-nek persistence-re van szüksége. Az implementation Infrastructure-ben van. A `DataFilePaths`, `XmlDataManager<T>` és `JsonDataManager<T>` típusú persistence detail nem kerülhet Presentationbe vagy Domainbe.

## Domain independence

A Domain típusok stabil business conceptként maradjanak. A `UserData` ne tudjon WPF-ről, XML-ről, JSON-ről, ViewModelről, DI containerről vagy file pathról.

## Mikor kell Use Case?

Use Case akkor indokolt, ha egy operation business validationt, persistence-t, workflow sorrendet vagy cross-layer contractot koordinál. Egyetlen triviális ViewModel property assignment köré nem kell use case.

## Jó és rossz példák

Jó: a `UserViewModel` meghívja az `ISaveUserUseCase.Execute(userEditSessionService)` metódust, majd navigationt és error displayt kezel.

Rossz: a `UserViewModel` létrehozza az `XmlUserRepository` típust, XML-t serializál, persistence előtt módosítja az eredeti usert, majd file hibákat kezel.

Jó: új `IUserRepository` implementation kerül Infrastructure-be, és composition time történik a binding.

Rossz: az Application közvetlenül függ az `XmlUserRepository` típustól, mert jelenleg XML a storage.

Kapcsolódó oldalak: [Cookbook](cookbook/README.md), [Dependency Registration](dependency-registration.md).
