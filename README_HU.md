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

Rétegzett WPF mintaalkalmazás, amely modern MVVM architektúrát, Dependency Injection használatot, Repository patternt, Application Use Case-eket, valamint a Presentation, az üzleti logika és a persistence tiszta szétválasztását mutatja be.

A repository egyben újrahasznosítható technikai tudásbázis WPF, MVVM, layered architecture és Clean Architecture ihletésű tervezés témákhoz.

Az alkalmazás eredetileg egy WPF frontend állásinterjú próbafeladatként indult, majd refaktorálás után újrahasznosítható architektúra mintává alakult.

## Képernyőképek

<p align="center"><img src="Doc/images/screenshots/user-manager-login.jpg" alt="UserManager login screen"></p>
<p align="center"><img src="Doc/images/screenshots/user-manager-select.jpg" alt="UserManager user list screen"></p>
<p align="center"><img src="Doc/images/screenshots/user-manager-edit.jpg" alt="UserManager user editor screen"></p>

## Áttekintés

A UserManager egy kis WPF alkalmazás, amely user authentication, user management, XML persistence és JSON export funkciókat tartalmaz.

Az alkalmazás három képernyőből áll:

1. **Login** - a user hitelesítése az XML adatfájl alapján.
2. **User List** - felhasználók listázása, város szerinti szűrés, keresés, hozzáadás, szerkesztés, törlés és export.
3. **User Editor** - minden mező szerkesztése a `UserId` kivételével, input validation megjelenítése, valamint tranzakcionális Save / Cancel workflow.

## Főbb jellemzők

- Layered architecture.
- MVVM pattern.
- Dependency Injection.
- Repository pattern.
- Application Use Cases.
- Transactional Edit Session.
- XML persistence.
- Atomic JSON export.
- Presentation, Business Logic és Persistence szétválasztása.
- Unit testek Domain, Application, Presentation, Infrastructure és dependency registration viselkedésre.

## Architektúra összefoglaló

A projekt Clean Architecture és MVVM elvekből kiinduló layered architecture-t követ, miközben tudatosan kerüli a felesleges bonyolítást. Minden rétegnek egyértelmű felelőssége van, és jól definiált contractokon keresztül kommunikál.

<p align="center"><img src="Doc/images/architecture/dependencies.svg" alt="UserManager compile-time dependencies"></p>

Az egyszerűsített dependency szabályok:

- `Domain` nem függ más UserManager projekttől.
- `Application` a `Domain` projekttől függ, és use case-eket, illetve contractokat tartalmaz.
- `Presentation` az `Application`, `Domain` és a shared MVVM support rétegekre támaszkodik.
- `Infrastructure` az `Application` contractokat implementálja, és az `Application`, illetve `Domain` projektekre hivatkozik.
- `WPFUI` a Composition Root, amely összeköti a konkrét alkalmazást.
- `FW4di.Dotnet.Core` és `FW4di.Dotnet.MVVM` újrahasznosítható technikai framework modulok maradnak.

A részletes architektúra dokumentáció itt található: [Doc/architecture_HU.md](Doc/architecture_HU.md).

## Projektstruktúra

```text
UserManager
├── README.md
├── README_HU.md
├── Doc/
│   ├── images/
│   ├── architecture.md
│   ├── architecture_HU.md
│   ├── cookbook/
│   └── decisions/
└── Project/
    ├── App4di.Dotnet.UserManager.Domain/
    ├── App4di.Dotnet.UserManager.Application/
    ├── App4di.Dotnet.UserManager.Infrastructure/
    ├── App4di.Dotnet.UserManager.Presentation/
    ├── App4di.Dotnet.UserManager.WPFUI/
    ├── App4di.Dotnet.UserManager.Tests/
    └── FirstParty/
```

Minden architekturális modul és réteg részletes magyarázata itt található: [Doc/architecture_HU.md](Doc/architecture_HU.md).

## Clone

Klónozd a teljes projektet a submodule-okkal együtt:

```bash
git clone --recurse-submodules https://github.com/4dillusions/UserManager.git
```

Ha a projekt már klónozva van, de a submodule-ok kimaradtak:

```bash
git submodule update --init --recursive
```

Ha a submodule-ok frissültek, és le szeretnéd kérni a legújabb változásokat:

```bash
git submodule update --remote --merge
```

## Build és futtatás

Az alkalmazás `.NET 10.0` targetet használ, a WPF UI pedig `net10.0-windows` targetet.

A solution helye:

```text
Project/App4di.Dotnet.UserManager.Windows.slnx
```

Build parancssorból:

```bash
dotnet build Project/App4di.Dotnet.UserManager.Windows.slnx
```

A WPF alkalmazás Visual Studioból vagy más Windows/.NET desktop fejlesztőkörnyezetből futtatható az `App4di.Dotnet.UserManager.WPFUI` projekt kiválasztásával.

Az alkalmazás adatfájlja: `Project/Data/data.xml`.

## Tesztek

```bash
dotnet test Project/App4di.Dotnet.UserManager.Windows.slnx
```

A jelenlegi tesztek Domain, Application, Presentation, Infrastructure, dependency registration, repository behavior, export behavior, navigation, session és ViewModel viselkedést fednek le.

## Publikálás

Készíthető self-contained Windows build, amely a célgépen külön telepített .NET Runtime nélkül is fut. A build gépen továbbra is szükség van a .NET SDK-ra.

Single-file, 64 bites Windows executable publikálása:

```bash
dotnet publish Project/App4di.Dotnet.UserManager.WPFUI/App4di.Dotnet.UserManager.WPFUI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

A publikált file-ok ide kerülnek:

```text
Project/Bin/Release/
```

Ha single-file executable helyett self-contained mappát szeretnél publikálni, hagyd el a `PublishSingleFile` beállítást:

```bash
dotnet publish Project/App4di.Dotnet.UserManager.WPFUI/App4di.Dotnet.UserManager.WPFUI.csproj -c Release -r win-x64 --self-contained true
```

32 bites Windowshoz `win-x86`, Windows ARM64-hez `win-arm64` runtime identifiert használj.

## Dokumentáció

- [🇬🇧 Architecture handbook](Doc/architecture.md) / [🇭🇺 Architektúra kézikönyv](Doc/architecture_HU.md)
- [🇬🇧 Cookbook](Doc/cookbook/README.md) / [🇭🇺 Receptgyűjtemény](Doc/cookbook/README_HU.md)
- [🇬🇧 Architecture Decision Records](Doc/decisions/README.md) / [🇭🇺 Architekturális döntési napló](Doc/decisions/README_HU.md)

## License

A source file-ok szerint a projekt a GNU General Public License version 3 or later feltételei szerint érhető el.
