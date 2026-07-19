# Új Use Case hozzáadása

[Cookbook](README.md) · [Dokumentációs index](../README.md) · [English version](../../en/cookbook/add-new-use-case.md)

Use Case contract és implementation az Application rétegbe kerüljön, ha az operation workflow-t, validationt vagy persistence-t koordinál contractokon keresztül. A Presentation ViewModelből hívja a contractot. Ha persistence kell, definiálj vagy használj meglévő Application repository/service contractot, és az implementationt Infrastructure-ben add hozzá. Regisztráció: `BindApplication()`. Testek: Application test stub contractokkal, Presentation test ViewModel behaviorra.

Kapcsolódó oldalak: [Dependency Registration](../dependency-registration.md), [Design Guidelines](../design-guidelines.md).
