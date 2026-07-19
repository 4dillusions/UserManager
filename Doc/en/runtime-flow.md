# Runtime Flow

[Documentation index](README.md) · [Magyar változat](../hu/runtime-flow.md)

Runtime flow is different from compile-time dependency direction. A user action begins in WPFUI, moves through Presentation, calls Application behavior, and reaches Domain or Infrastructure only through the allowed contracts.

<p align="center"><img src="../images/architecture/runtime-flow.svg" alt="UserManager runtime flow"></p>

## Startup Flow

1. WPF raises `Application_Startup` in `App.xaml.cs`.
2. `DIBindings.Init` invokes `BindApplication`, `BindPresentation`, `BindInfrastructure`, and `BindWpfUi`.
3. `ViewTypeConverter` is configured to create `LoginView`, `UserListView`, and `UserView` with their ViewModels.
4. Culture is fixed to `en-US` for consistent WPF parsing, validation, and formatting.
5. `MainView` is created with `MainViewModel` as `DataContext`.
6. `mainView.Show()` displays the shell.

## Typical Request Flow

```text
View -> ViewModel -> Application Use Case -> Domain / Infrastructure
```

The result returns as Application result, Presentation state, property change notification, WPF binding, and an updated View.

## Authentication Flow

`LoginViewModel.LoginCommand` calls `IAuthenticationService.Authenticate`. `AuthenticationService` loads users through `IUserRepository` and compares login name and password. Success navigates to `ViewType.UserList`; failure shows a notification.

## Query Flow

`UserListViewModel` builds `UserQueryCriteria` from `UserFilter`, city selection, and search text. `UserQueryService` loads users through `IUserRepository`, loads cities through `IAddressCityRepository`, and returns filtered `UserData`. Presentation maps `UserData` to bindable `User` models.

## Add / Edit / Save Flow

Add starts `IUserEditSessionService.BeginAdd` with a fresh ID. Edit starts `BeginEdit` with a cloned selected user. `UserViewModel.SaveCommand` calls `ISaveUserUseCase.Execute`. `SaveUserUseCase` validates birth date, checks duplicate login names in the save snapshot, saves through `IUserRepository`, and then calls `CompleteSave`. The original UI list item is updated only after persistence succeeds.

## Cancel Flow

`UserViewModel.CancelCommand` calls `IUserEditSessionService.Cancel` and navigates back. The edit session is cleared without mutating persistent state or the original selected user.

## Delete Flow

`UserListViewModel.DeleteCommand` asks for confirmation through `IUserNotificationService`, calls `IDeleteUserUseCase.Execute`, prevents deleting the last user through the use case, saves the remaining list through `IUserRepository`, and refreshes the displayed list.

## Export Flow

`UserListViewModel.ExportCommand` maps displayed users to `UserData` and calls `IUserExportService.ExportUsers`. `JsonUserExportService` writes a temporary JSON file, moves it over the target file with overwrite, returns the full path, and performs best-effort temporary cleanup.

<p align="center"><img src="../images/architecture/flow.svg" alt="UserManager workflow diagram"></p>

