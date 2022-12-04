namespace Lazy;

/// <summary>
/// Multithreaded implementation of Lazy
/// </summary>
/// <typeparam name="T">Element type</typeparam>
public class MultithreadedLazy<T> : Lazy<T>
{
    /// <summary>
    /// Boolean variable for detecting the first call
    /// </summary>
    private bool isAlreadyCounted;

    /// <summary>
    /// Lock
    /// </summary>
    private readonly Object lockObject = new();

    /// <inheritdoc/>
    public MultithreadedLazy(Func<T> func) : base(func) { }

    /// <inheritdoc/>
    public override T Get()
    {
        ///if the value has already been calculated, there should be no locks
        if (!Volatile.Read(ref isAlreadyCounted))
        {
            ///lock
            lock (lockObject)
            {
                /// if the value was not calculated
                if (!Volatile.Read(ref isAlreadyCounted))
                {
                    value = func();
                    isAlreadyCounted = true;
                    Volatile.Write(ref isAlreadyCounted, true);
                }
            }
        }

        return value!;
    }
}