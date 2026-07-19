<p align="center">
  <img src="Doc/images/branding/logo.svg" alt="UserManager Logo" width="300"/>
</p>

# UserManager

## Nyelvek

- 🇬🇧 [English](README.md)
- 🇭🇺 Magyar

<p align="center">
  <img src="https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows badge" height="20"/>
  <img src="https://img.shields.io/badge/-.NET%2010.0-blueviolet" alt=".NET 10 badge"/>
  <a href="https://github.com/4dillusions/UserManager/actions/workflows/dotnet-desktop.yml">
    <img src="https://github.com/4dillusions/UserManager/actions/workflows/dotnet-desktop.yml/badge.svg" alt=".NET Desktop CI"/>
  </a>
</p>

Rétegzett WPF mintaalkalmazás, amely modern MVVM architektúrát, Dependency Injection használatot, Repository patternt, Application Use Case-eket, valamint a Presentation, az üzleti logika és a perzisztencia tiszta szétválasztását mutatja be.

A repository egyben újrahasznosítható technikai tudásbázis WPF, MVVM, layered architecture és Clean Architecture ihletésű tervezés témákhoz.

## Képernyőképek

<p align="center"><img src="Doc/images/screenshots/user-manager-login.jpg" alt="UserManager login screen"></p>
<p align="center"><img src="Doc/images/screenshots/user-manager-select.jpg" alt="UserManager user list screen"></p>
<p align="center"><img src="Doc/images/screenshots/user-manager-edit.jpg" alt="UserManager user editor screen"></p>

## Áttekintés

UserManager is a small WPF application capable of user authentication, user management, XML persistence, and JSON export.

Az alkalmazás három screenből áll:

1. **Login** - authenticates a user against the XML data file.
2. **User List** - displays users, filters by city, searches user data, adds, edits, deletes, and exports users.
3. **User Editor** - edits every field except `UserId`, shows input validation, and uses a transactional Save / Cancel workflow.

## Főbb jellemzők

- Layered architecture.
- MVVM pattern.
- Dependency Injection.
- Repository pattern.
- Application Use Cases.
- Transactional Edit Session.
- XML persistence.
- Atomic JSON export.
- Separation of Presentation, Business Logic and Persistence.
- Unit tests across Domain, Application, Presentation, Infrastructure, and dependency registration behavior.

## Architektúra összefoglaló

A projekt Clean Architecture és MVVM elvekből kiinduló layered architecture-t követ, miközben tudatosan kerüli a szükségtelen bonyolítást. Minden rétegnek egyértelmű felelőssége van, és jól definiált contractokon keresztül kommunikál.

<p align="center"><img src="Doc/images/architecture/dependencies.svg" alt="UserManager compile-time dependencies"></p>

Az egyszerűsített dependency szabályok:

- `Domain` depends on no other UserManager project.
- `Application` depends on `Domain` and owns use cases and contracts.
- `Presentation` depends on `Application`, `Domain`, and shared MVVM support.
- `Infrastructure` implements `Application` contracts and depends on `Application` and `Domain`.
- `WPFUI` is the Composition Root and wires the concrete application together.
- `FW4di.Dotnet.Core` and `FW4di.Dotnet.MVVM` remain reusable technical framework modules.

A részletes architektúra dokumentáció itt található: [Doc/hu/architecture.md](Doc/hu/architecture.md).

## Projektstruktúra

```text
UserManager
├── README.md
├── README_HU.md
├── Doc/
│   ├── images/
│   ├── en/
│   └── hu/
└── Project/
    ├── App4di.Dotnet.UserManager.Domain/
    ├── App4di.Dotnet.UserManager.Application/
    ├── App4di.Dotnet.UserManager.Infrastructure/
    ├── App4di.Dotnet.UserManager.Presentation/
    ├── App4di.Dotnet.UserManager.WPFUI/
    ├── App4di.Dotnet.UserManager.Tests/
    └── FirstParty/
```

For a detailed explanation of every module and layer, see [Doc/hu/modules-and-layers.md](Doc/hu/modules-and-layers.md).

## Clone

```bash
git clone --recurse-submodules https://github.com/4dillusions/UserManager.git
git submodule update --init --recursive
git submodule update --remote --merge
```

## Build és futtatás

Az alkalmazás `.NET 10.0` targetet használ, a WPF UI pedig `net10.0-windows` targetet.

Open the solution from:

```text
Project/App4di.Dotnet.UserManager.Windows.slnx
```

Build from the command line:

```bash
dotnet build Project/App4di.Dotnet.UserManager.Windows.slnx
```

Run the WPF application from Visual Studio or another Windows/.NET desktop capable IDE by selecting the `App4di.Dotnet.UserManager.WPFUI` project.

Az alkalmazás adatfájlja: `Project/Data/data.xml`.

## Tesztek

```bash
dotnet test Project/App4di.Dotnet.UserManager.Windows.slnx
```

A jelenlegi testek Domain, Application, Presentation, Infrastructure, dependency registration, repository behavior, export behavior, navigation, session és ViewModel viselkedést fednek le. Részletek: [Doc/hu/testing.md](Doc/hu/testing.md).

## Dokumentáció

- [Teljes dokumentációs index](Doc/hu/README.md)
- [Architecture](Doc/hu/architecture.md)
- [Modules and layers](Doc/hu/modules-and-layers.md)
- [Dependency registration](Doc/hu/dependency-registration.md)
- [Runtime flow](Doc/hu/runtime-flow.md)
- [Design guidelines](Doc/hu/design-guidelines.md)
- [Testing](Doc/hu/testing.md)
- [Cookbook](Doc/hu/cookbook/README.md)
- [Architecture Decision Records](Doc/hu/decisions/README.md)

## License

A source file-ok szerint a projekt a GNU General Public License version 3 or later feltételei szerint érhető el.
