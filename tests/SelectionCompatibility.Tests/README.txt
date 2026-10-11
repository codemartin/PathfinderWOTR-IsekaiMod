Run from the repository root:
dotnet build tests/SelectionCompatibility.Tests/SelectionCompatibility.Tests.csproj
dotnet tests/SelectionCompatibility.Tests/bin/Debug/net10.0/SelectionCompatibility.Tests.dll

These tests compile the real PrerequisiteAlternatives.cs against lightweight API stubs.
They verify prerequisite combination semantics and idempotency, not Unity/game integration.
The mod Release build separately checks compilation against game and TabletopTweaks assemblies.
