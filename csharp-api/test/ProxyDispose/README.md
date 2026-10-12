# Dispose interface regression

Build the C# bridge, then run from the repository root on Windows x64:

```powershell
dotnet run --project csharp-api/test/ProxyDispose -c Release
```

Use `-p:BridgeDirectory=C:/path/to/bridge` when the built `REFramework.NET.dll`
and `Ijwhost.dll` are outside `build/bin`. No game instance is needed. With the
old bridge the first factory throws `TypeLoadException` because the generated
game `Dispose()` collides with the proxy base's destructor-generated final
method. The test also covers inherited interface declarations and all factories.

The proxy only holds a managed reference to its target; its empty destructor and
finalizer performed no cleanup. This change removes the base's implicit CLR
`IDisposable` implementation, allowing a game interface's `Dispose()` to be
forwarded like any other game method. It does not dispose or release the target
when the wrapper becomes unreachable.
