# Add a New Use Case

[Cookbook](README.md) · [Documentation index](../README.md) · [Magyar változat](../../hu/cookbook/add-new-use-case.md)

Place use-case contracts and implementation in Application when the operation coordinates workflow, validation, or persistence through contracts. Presentation calls the contract from a ViewModel. If persistence is needed, define or reuse an Application repository/service contract and implement it in Infrastructure. Register the use case in `BindApplication()`. Add Application tests with stub contracts and Presentation tests for ViewModel behavior.

Related: [Dependency Registration](../dependency-registration.md), [Design Guidelines](../design-guidelines.md).
