# Add a New Use Case

[🇭🇺 Magyar változat](add-new-use-case_HU.md) · [🇬🇧 Cookbook](README.md) · [🇬🇧 Architecture handbook](../architecture.md)

Place use-case contracts and implementation in Application when the operation coordinates workflow, validation, or persistence through contracts. Presentation calls the contract from a ViewModel. If persistence is needed, define or reuse an Application repository/service contract and implement it in Infrastructure. Register the use case in `BindApplication()`. Add Application tests with stub contracts and Presentation tests for ViewModel behavior.

Related: [🇬🇧 Architecture handbook](../architecture.md).
