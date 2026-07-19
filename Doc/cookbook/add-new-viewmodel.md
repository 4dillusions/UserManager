# Add a New ViewModel

[Magyar változat](add-new-viewmodel_HU.md) · [Cookbook](README.md) · [Architecture handbook](../architecture.md)

Place application-specific ViewModels in `App4di.Dotnet.UserManager.Presentation/ViewModels`. Expose bindable state through properties and user actions through `FW4di.Dotnet.MVVM` commands. Call Application contracts, use navigation/session abstractions, and keep WPF controls, repository implementations, XML, JSON, and file access out of the ViewModel. Register the ViewModel in `BindPresentation()` and add focused Presentation tests.

Related: [Architecture handbook](../architecture.md).
