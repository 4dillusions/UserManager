# Add a New ViewModel

[Cookbook](README.md) · [Documentation index](../README.md) · [Magyar változat](../../hu/cookbook/add-new-viewmodel.md)

Place application-specific ViewModels in `App4di.Dotnet.UserManager.Presentation/ViewModels`. Expose bindable state through properties and user actions through `FW4di.Dotnet.MVVM` commands. Call Application contracts, use navigation/session abstractions, and keep WPF controls, repository implementations, XML, JSON, and file access out of the ViewModel. Register the ViewModel in `BindPresentation()` and add focused Presentation tests.

Related: [Dependency Registration](../dependency-registration.md), [Design Guidelines](../design-guidelines.md).
