# Új ViewModel hozzáadása

[Cookbook](README.md) · [Dokumentációs index](../README.md) · [English version](../../en/cookbook/add-new-viewmodel.md)

Application-specific ViewModel helye: `App4di.Dotnet.UserManager.Presentation/ViewModels`. Bindable state propertykkel, user action `FW4di.Dotnet.MVVM` commandokkal legyen expose-olva. A ViewModel Application contractokat hívjon, navigation/session abstractionöket használjon, és ne tartalmazzon WPF controlt, repository implementationt, XML-t, JSON-t vagy file accesst. Regisztráció: `BindPresentation()`. Adj hozzá fókuszált Presentation testeket.

Kapcsolódó oldalak: [Dependency Registration](../dependency-registration.md), [Design Guidelines](../design-guidelines.md).
