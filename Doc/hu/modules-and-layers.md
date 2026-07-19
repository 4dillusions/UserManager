# Modulok és rétegek

[Dokumentációs index](README.md) · [English version](../en/modules-and-layers.md)

Bár ez a mintaalkalmazás szándékosan kicsi, a modulok úgy vannak kialakítva, hogy nagyobb alkalmazásokban is értelmezhető mintát adjanak.

A solution a **Model-View-ViewModel (MVVM)** presentation patternt kombinálja **Clean Architecture**, **Clean Code** és **SOLID** elvek által inspirált layered architecture-rel. Ezek kapcsolódó, de különböző problémákat oldanak meg:

- Az **MVVM** szétválasztja a user interface-t, a presentation state-et és az application behaviort.
- A **Layered / Clean Architecture** a teljes rendszer responsibility boundaryjeit és dependency directionjét definiálja.
- A **Clean Code és SOLID** a modulok, classok, metódusok és contractok belső designját vezeti.

A modulok nem önálló MVVM implementációk. Együttműködve valósítják meg a három MVVM role-t.

| MVVM role | Jelenlegi modulok |
|---|---|
| **Model** | `Domain`, `Application`, `Infrastructure` |
| **ViewModel** | `Presentation`, `FW4di.Dotnet.MVVM` támogatással |
| **View** | `WPFUI` |
| **Composition és startup** | `WPFUI`, `FW4di.Dotnet.Core` támogatással |
| **Verification** | `Tests` |

## MVVM ebben a solutionben

Az MVVM három együttműködő role-ra bontja a presentation-oriented szoftvert: **Model**, **View** és **ViewModel**. A View bindol a ViewModelhez és commandokat hív. A ViewModel Application use case-eket hív és presentation state-et tesz elérhetővé. A Model application és business behaviort tartalmaz anélkül, hogy a View-tól függene.

A ViewModel nem függhet konkrét `Window`, `Page` vagy `UserControl` típustól. A Modelnek nem kell tudnia, hogy WPF, más desktop toolkit, web UI vagy UI nélküli környezet használja.

### Model

MVVM-ben a Model tágabb fogalom, mint DTO-k vagy database rekordok gyűjteménye. Tartalmazhat business entityket, Value Objecteket, invariánsokat, Domain service-eket, Application use case-eket, commandokat, queryket, repository contractokat, transaction boundaryket, persistence implementationöket és external adaptereket.

Ebben a projektben a Model szerep három architekturális rétegre oszlik:

- `Domain`: stabil üzleti fogalmak és szabályok. Jelenleg a `UserData` modelt tartalmazza. Nagyobb rendszerben Entity, Value Object, Aggregate, Domain Service, Specification, Domain Event, Policy és invariant is ide kerülhet.
- `Application`: az alkalmazás műveletei és contractjai. Jelenleg authentication, user query, user export contract, transactional save, user delete, repository contractok, service contractok és `BirthDateRules` tartozik ide.
- `Infrastructure`: az Application contractok technikai implementációi. Jelenleg XML repositoryk, JSON export, data managerek, file path handling és file system access tartozik ide.

### View

A View a konkrét user interface. WPF-ben ide tartozik a `Window`, `Page`, `UserControl`, XAML, controls, templates, styles, resources, converters, behaviors, validation feedback és platform-specific interaction.

Ebben a solutionben a View role-t a `WPFUI` valósítja meg. A View rendereli a ViewModel state-et, bindingokon és commandokon keresztül továbbítja a user interactiont, valamint megjeleníti a validation és notification eredményeket. Nem tartalmazhat business decisiont, repository accesst, XML/JSON feldolgozást vagy application workflow orchestrationt.

Code-behind használható valóban vizuális, WPF controlhoz kötött behaviorra. User mentése, permission döntés, repository betöltés vagy business validation nem tartozik code-behindba.

### ViewModel

A ViewModel presentation model és a View abstractionje. Binding által fogyasztható state-et és actionöket tesz elérhetővé.

Egy ViewModel kezelhet observable UI state-et, commandokat, user intentet, selection/loading/error/editing state-et, navigationt, dialogokat, mappinget és Application use case hívásokat. Nem implementálhat business rule-t, nem végezhet file/database accesst, nem választhat konkrét repository implementationt, nem hozhat létre XML/JSON documentet, és nem manipulálhat konkrét View-t.

A jelenlegi ViewModelek: `MainViewModel`, `LoginViewModel`, `UserListViewModel`, `UserViewModel`.

## Clean Code és SOLID irányelvek

A Clean Code nem egyetlen Microsoft standard, hanem széles körben használt software-design gyakorlatok gyűjtőneve. A cél nem a classok, interface-ek vagy rétegek számának növelése, hanem az explicit responsibility, kontrollált dependency, testability és lokális változtathatóság.

### Single Responsibility Principle

Egy classnak vagy modulnak egy koherens felelőssége legyen. A ViewModel screen state-et és interactiont kezel; a use case egy application operationt koordinál; a repository persistence accessért felel; a mapper model alakzatokat alakít át; a WPF adapter platform dialogot jelenít meg.

### Separation of Concerns

A Presentation, application workflow, business rules és technical implementation külön van választva. A View nem hajt végre business operationt, a ViewModel nem végez file műveletet, az Application nem függ WPF-től, a Domain nem függ storage technologytól, az Infrastructure nem definiál business policyt.

### Dependency Inversion Principle

A high-level policy nem függhet közvetlenül low-level technikai részletektől. Az Application definiálja az `IUserRepository` contractot, az Infrastructure adja az `XmlUserRepository` implementationt, a WPFUI pedig Composition Rootként regisztrálja.

### Explicit dependencies

A collaboratorok legyenek láthatók constructorban vagy method signature-ben. Kerülendő a global service locator, hidden static service, DI container használata business code-ban, konkrét repository létrehozása ViewModelben vagy use case-ben, illetve teljes service provider átadása application classoknak.

### High cohesion és low coupling

A szorosan kapcsolódó behavior maradjon együtt, az unrelated behavior ne kerüljön ugyanabba a classba. A feature-oriented organization, fókuszált use case-ek, kisméretű contractok és presentation modelek támogatják ezt.

### Interface Segregation Principle

A consumerek csak a ténylegesen szükséges operationöktől függjenek. Fókuszált contract előnyösebb, mint egy nagy manager interface. Interface akkor indokolt, ha valós boundaryt képvisel, volatile technologyt izolál, több implementationt támogat vagy isolated testinghez kell.

### Open/Closed Principle

A stabil application policy legyen bővíthető anélkül, hogy központi orchestration kódot kellene folyamatosan módosítani. Példa: új repository implementation meglévő Application contract mögött vagy új export format fókuszált abstractionön keresztül.

### Liskov Substitution Principle

Egy implementationnek teljesítenie kell az abstraction behavior elvárásait. Alternatív repository esetén meg kell őrizni az identity handling, not-found behavior, error reporting és save/delete consistency szemantikáját.

### Tell, do not ask excessively

A behavior lehetőleg annak az adatnak és responsibilitynek a közelében legyen, amely birtokolja. Presentation state megfigyelhető a View számára, de business invariant ne XAML-ben vagy ViewModelben legyen újraimplementálva.

### Intention-revealing names

Jó nevek: `SaveUserUseCase`, `IUserRepository`, `UserEditSession`, `UserListViewModel`, `IMessageService`. Kerülendő: `Helper`, `CommonManager`, `MiscService`, `Utils2`, `DataProcessor`.

### Testability

A Domain pure unit testekkel, az Application fake vagy mocked contractokkal, a ViewModel konkrét View nélkül, az Infrastructure file/database integration testekkel ellenőrizhető. WPF-specifikus behavior UI vagy End-to-End Test szintre maradhat.

### Kerüld a szükségtelen abstractionöket

Clean Architecture nem követel interface-t minden class elé vagy handlert minden metódus köré. Abstraction akkor hasznos, ha boundaryt lép át, volatile technologyt izolál, több implementationt támogat, testben cserélni kell, vagy higher-level policy elől rejt konkrét implementationt.

## Jelenlegi projektfelelősségek

- `FW4di.Dotnet.Core`: Dependency Injection support, helper utilityk, framework-independent technikai komponensek.
- `FW4di.Dotnet.MVVM`: commandok, property change notification, binding support, messaging primitívek; nem tartalmaz UserManager business logicot.
- `Domain`: `UserData`, WPF-től, Presentationtől, Infrastructure-től, XML-től és JSON-től függetlenül.
- `Application`: `AuthenticationService`, `UserQueryService`, `SaveUserUseCase`, `DeleteUserUseCase`, repository contractok, `IUserExportService`, `BirthDateRules`.
- `Infrastructure`: `XmlUserRepository`, `XmlAddressCityRepository`, `JsonUserExportService`, `XmlDataManager<T>`, `JsonDataManager<T>`, `DataFilePaths`, file system access.
- `Presentation`: ViewModelek, bindable modellek, navigation, session state, edit session management, mapping és exception message formatting.
- `WPFUI`: Views, XAML, WPF validation, converters, platform services, Composition Root, DI wiring, colors, brushes és shared WPF styles.
- `Tests`: Domain, Application, Presentation, Infrastructure, dependency registration, authentication, query, save, edit session, navigation, export, mapping és repository behavior.

<p align="center"><img src="../images/architecture/flow.svg" alt="UserManager workflow diagram"></p>
