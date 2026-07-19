# Új képernyő hozzáadása

[English version](add-new-screen.md) · [Receptgyűjtemény](README_HU.md) · [Architektúra kézikönyv](../architecture_HU.md)

Az XAML View a WPFUI rétegbe kerüljön, a ViewModel a Presentation rétegbe, a workflow szükség esetén Application Use Case-be, a persistence pedig Application contractok mögé, Infrastructure implementációval. A flow maradjon `View -> ViewModel -> Application Use Case -> Domain / Infrastructure`. A navigation konfigurációja `ViewType`, `NavigationService` és `ViewTypeConverter` használatával történjen; regisztráld a ViewModelt, és teszteld a commandokat, validationt, mappinget és workflow viselkedést.

Kapcsolódó oldal: [Architektúra kézikönyv](../architecture_HU.md).
