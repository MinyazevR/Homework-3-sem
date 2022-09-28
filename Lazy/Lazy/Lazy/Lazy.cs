namespace Lazy;

/// <summary>
/// Abstract class, for subsequent implementation of the ILazy interface
/// </summary>
/// <typeparam name="T">Element type</typeparam>
public abstract class Lazy<T> : ILazy<T>
{
    protected Func<T> func;
    protected T? value;

    /// <summary>
    /// Сonstructor
    /// </summary>
    /// <param name="func">The object on the basis of which the calculation is performed</param>
    /// <exception cref="ArgumentNullException">If func is null</exception>
    public Lazy(Func<T> func)
    {
        this.func = func ?? throw new ArgumentNullException(nameof(Func<T>));
    }

    /// <summary>
    /// Function that calls a calculation and returns results
    /// </summary>
    /// <returns>Element type</returns>
    public abstract T Get();
}

