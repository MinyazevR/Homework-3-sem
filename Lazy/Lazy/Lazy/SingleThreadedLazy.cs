namespace Lazy;

/// <summary>
/// Singlethreaded implementation of Lazy
/// </summary>
/// <typeparam name="T">Element type</typeparam>
public class SingleThreadedLazy<T> : Lazy<T>
{
    /// <inheritdoc/>
    public SingleThreadedLazy(Func<T> func) : base(func) { }

    private bool isAlreadyCounted;

    /// <inheritdoc/>
    public override T Get()
    {
        if (!isAlreadyCounted)
        {
            isAlreadyCounted = true;
            value = func();
        }

        return value!;
    }
}
