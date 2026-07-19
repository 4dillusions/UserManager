# Add a New Repository

[Magyar változat](add-new-repository_HU.md) · [Cookbook](README.md) · [Architecture handbook](../architecture.md)

Define repository contracts in Application when use cases need persistence. Implement concrete persistence in Infrastructure, using Domain models at the boundary. Register the implementation in `BindInfrastructure()` and select alternatives from the Composition Root. Test Application behavior with stubs and Infrastructure behavior against file/database details as appropriate.

Related: [Architecture handbook](../architecture.md).
