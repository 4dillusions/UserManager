# Testing

[Dokumentációs index](README.md) · [English version](../en/testing.md)

A jelenlegi test projekt: `Project/App4di.Dotnet.UserManager.Tests`. `net10.0` targetet és MSTestet használ. Referálja a Domain, Application, Infrastructure, Presentation és `FW4di.Dotnet.Core` projekteket.

## Jelenlegi lefedettség

- Domain: `UserDataTests`.
- Application: authentication, user queryk, birth date rule-ok, save use case, delete use case.
- Presentation: ViewModelek, navigation, session service, edit session service, exception message formatting, bindable modellek.
- Infrastructure: data managerek, XML repositoryk, JSON export service.
- Configuration: dependency registration testek.

## Jelenlegi boundaryk

Az Application testek repository contract stubokat használnak. A Presentation testek Application service, notification, export, delete és repository stubokat használnak, ahol szükséges. Az Infrastructure testek file-based XML/JSON behaviort ellenőriznek, és takarítják a test file-okat.

## Future strategy

Lehetséges későbbi bővítés: explicit architecture dependency testek, szélesebb integration testek, WPF UI automation testek és End-to-End Testek. Ezeket csak akkor szabad jelenlegi képességként dokumentálni, ha ténylegesen bekerülnek.

## Test command

```bash
dotnet test Project/App4di.Dotnet.UserManager.Windows.slnx -p:EnableWindowsTargeting=true
```
