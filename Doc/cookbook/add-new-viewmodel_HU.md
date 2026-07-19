# Új ViewModel hozzáadása

[English version](add-new-viewmodel.md) · [Receptgyűjtemény](README_HU.md) · [Architektúra kézikönyv](../architecture_HU.md)

A ViewModelt a Presentation rétegbe tedd, és csak bindable state-et, commandokat, navigationt, user-facing hibákat és Application contract hívásokat tartalmazzon. Ne tegyél bele WPF controlokat, repository implementációkat, XML/JSON műveleteket vagy file access logikát. Használd a `FW4di.Dotnet.MVVM` command és notification primitiveket, regisztráld a ViewModelt a `BindPresentation()` metódusban, és teszteld WPF View példányosítása nélkül.

Kapcsolódó oldal: [Architektúra kézikönyv](../architecture_HU.md).
