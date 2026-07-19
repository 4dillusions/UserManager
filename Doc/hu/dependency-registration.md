# Dependency Registration

[Dokumentációs index](README.md) · [English version](../en/dependency-registration.md)

A WPFUI az alkalmazás Composition Rootja. Ez indítja a dependency registration folyamatot, kiválasztja a konkrét modulokat, beköti a WPF-specifikus adaptereket, és feloldja az induló ViewModelt és View-t.

## Jelenlegi registration entry pointok

- `BindApplication()` az Application projekt tulajdona; regisztrálja az `IAuthenticationService`, `IUserQueryService`, `ISaveUserUseCase` és `IDeleteUserUseCase` implementációkat.
- `BindPresentation()` a Presentation projekt tulajdona; regisztrálja a `UserAutoMapperManager`, `ISessionService`, `IUserEditSessionService`, `INavigationService` és a négy ViewModel típust.
- `BindInfrastructure()` az Infrastructure projekt tulajdona; regisztrálja az `XmlDataManager<UserData>`, `JsonDataManager<UserData>`, `IUserRepository`, `IAddressCityRepository` és `IUserExportService` implementációkat.
- `BindWpfUi()` a WPFUI tulajdona; WPF-specifikus adaptereket regisztrál: `IUserNotificationService` és `IApplicationService`.

## Startup sorrend

Az `App.xaml.cs` sorrendje: Application, Presentation, Infrastructure, WPFUI. A sorrend explicit, determinisztikus és a Composition Rootban látható.

## Miért nincs globális assembly scanning?

A registration kód nem szkenneli az összes betöltött assemblyt. Minden modul csak a saját típusait regisztrálja. Ez érthetőbb startupot, tiszta ownershipet és explicit alternatív implementationöket eredményez.

Alternatív implementation hozzáadásakor a Composition Root módosítása vagy egy szűken elnevezett module registration method javasolt, nem egy generikus globális binding hívás.

Kapcsolódó oldalak: [Architektúra](architecture.md), [ADR-0002](decisions/0002-composition-root.md), [ADR-0003](decisions/0003-module-registration.md).
