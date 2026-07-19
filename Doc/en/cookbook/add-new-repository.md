# Add a New Repository

[Cookbook](README.md) · [Documentation index](../README.md) · [Magyar változat](../../hu/cookbook/add-new-repository.md)

Define repository contracts in Application when use cases need persistence. Implement concrete persistence in Infrastructure, using Domain models at the boundary. Register the implementation in `BindInfrastructure()` and select alternatives from the Composition Root. Test Application behavior with stubs and Infrastructure behavior against file/database details as appropriate.

Related: [Dependency Registration](../dependency-registration.md), [Design Guidelines](../design-guidelines.md).
