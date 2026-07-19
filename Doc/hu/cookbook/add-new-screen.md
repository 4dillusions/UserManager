# Új screen hozzáadása

[Cookbook](README.md) · [Dokumentációs index](../README.md) · [English version](../../en/cookbook/add-new-screen.md)

A XAML View a WPFUI-ba, a ViewModel a Presentationbe, a workflow Application Use Case-be, a persistence pedig Application contract mögötti Infrastructure implementationbe kerüljön. A flow maradjon `View -> ViewModel -> Application Use Case -> Domain / Infrastructure`. Navigation: `ViewType`, `NavigationService`, `ViewTypeConverter`. Regisztráld a ViewModelt, és teszteld a commandokat, validationt, mappinget és workflow behaviort.

Kapcsolódó oldalak: [Dependency Registration](../dependency-registration.md), [Design Guidelines](../design-guidelines.md).
