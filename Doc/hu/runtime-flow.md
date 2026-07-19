# Runtime Flow

[Dokumentációs index](README.md) · [English version](../en/runtime-flow.md)

A runtime flow nem ugyanaz, mint a compile-time dependency direction. Egy user action a WPFUI-ban indul, a Presentation rétegen keresztül Application behaviorhöz jut, majd Domain vagy Infrastructure komponenseket csak a megengedett contractokon keresztül ér el.

<p align="center"><img src="../images/architecture/runtime-flow.svg" alt="UserManager runtime flow"></p>

## Startup flow

1. A WPF meghívja az `Application_Startup` metódust az `App.xaml.cs` fájlban.
2. A `DIBindings.Init` meghívja a `BindApplication`, `BindPresentation`, `BindInfrastructure` és `BindWpfUi` metódusokat.
3. A `ViewTypeConverter` beállítja a `LoginView`, `UserListView` és `UserView` létrehozását a megfelelő ViewModellel.
4. A culture `en-US`, hogy a WPF parsing, validation és formatting gépek között konzisztens legyen.
5. Létrejön a `MainView`, `MainViewModel` DataContexttel.
6. A `mainView.Show()` megjeleníti a shellt.

## Tipikus request flow

```text
View -> ViewModel -> Application Use Case -> Domain / Infrastructure
```

Az eredmény Application result, Presentation state, property change notification, WPF binding és frissített View formájában tér vissza.

## Authentication flow

A `LoginViewModel.LoginCommand` az `IAuthenticationService.Authenticate` metódust hívja. Az `AuthenticationService` az `IUserRepository` segítségével betölti a usereket, majd login name és password alapján ellenőriz. Siker esetén `ViewType.UserList` navigáció történik, hiba esetén notification jelenik meg.

## Query flow

A `UserListViewModel` a `UserFilter`, city selection és search text alapján `UserQueryCriteria` objektumot épít. A `UserQueryService` az `IUserRepository` és `IAddressCityRepository` contractokat használja, majd `UserData` listát ad vissza. A Presentation ezt bindable `User` modellekre mapeli.

## Add / Edit / Save flow

Add esetén az `IUserEditSessionService.BeginAdd` új ID-val indít sessiont. Edit esetén a `BeginEdit` a selected user klónján dolgozik. A `UserViewModel.SaveCommand` az `ISaveUserUseCase.Execute` metódust hívja. A `SaveUserUseCase` birth date validationt végez, duplicate login name-et ellenőriz a save snapshotban, ment az `IUserRepository` contracton keresztül, majd `CompleteSave` hívással zár. Az eredeti UI list item csak sikeres persistence után módosul.

## Cancel flow

A `UserViewModel.CancelCommand` az `IUserEditSessionService.Cancel` metódust hívja és visszanavigál. A session törlődik, persistent state és original selected user módosítása nélkül.

## Delete flow

A `UserListViewModel.DeleteCommand` confirmationt kér az `IUserNotificationService` segítségével, meghívja az `IDeleteUserUseCase.Execute` metódust, a use case tiltja az utolsó user törlését, majd az `IUserRepository` menti a fennmaradó listát.

## Export flow

A `UserListViewModel.ExportCommand` a megjelenített usereket `UserData` objektumokra mapeli, majd meghívja az `IUserExportService.ExportUsers` metódust. A `JsonUserExportService` temporary JSON file-ba ír, overwrite move-val cseréli a célfájlt, visszaadja a teljes pathot, és best-effort cleanupot végez.

<p align="center"><img src="../images/architecture/flow.svg" alt="UserManager workflow diagram"></p>
