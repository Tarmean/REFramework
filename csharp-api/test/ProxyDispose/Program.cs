using System;
using REFrameworkNET;

// Creation does not need the game or a TDB. Before the fix, the Dispose
// forwarding method attempts to override the base's C++/CLI-generated final
// Dispose method, and DispatchProxy.Create throws TypeLoadException.
public interface IGameObject { void Dispose(); }
public interface IDerivedGameObject : IGameObject { new void Dispose(); }
public interface IControl { void Release(); }

internal static class Program
{
    private static void Main()
    {
        Assert(ManagedProxy<IGameObject>.Create(null!) is IGameObject);
        Assert(ManagedProxy<IDerivedGameObject>.Create(null!) is IDerivedGameObject);
        Assert(NativeProxy<IGameObject>.Create(null!) is IGameObject);
        Assert(AnyProxy<IGameObject>.Create(null!) is IGameObject);
        Assert(ManagedProxy<IControl>.Create(null!) is IControl);
        Assert(!typeof(IDisposable).IsAssignableFrom(typeof(Proxy<IGameObject, ManagedObject>)));
        Console.WriteLine("PASS: Dispose-bearing interfaces can be proxied without a game instance.");
    }

    private static void Assert(bool condition)
    {
        if (!condition) throw new InvalidOperationException("Proxy creation contract failed.");
    }
}
