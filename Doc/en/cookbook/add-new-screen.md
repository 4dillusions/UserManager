# Add a New Screen

[Cookbook](README.md) · [Documentation index](../README.md) · [Magyar változat](../../hu/cookbook/add-new-screen.md)

Add the XAML View in WPFUI, the ViewModel in Presentation, any workflow in an Application Use Case, and persistence behind Application contracts implemented by Infrastructure. The flow should remain `View -> ViewModel -> Application Use Case -> Domain / Infrastructure`. Configure navigation through `ViewType`, `NavigationService`, and `ViewTypeConverter`, register the ViewModel, and test commands, validation, mapping, and workflow behavior.

Related: [Dependency Registration](../dependency-registration.md), [Design Guidelines](../design-guidelines.md).
