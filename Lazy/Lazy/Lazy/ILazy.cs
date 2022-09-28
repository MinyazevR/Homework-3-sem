namespace Lazy;

/// <summary>
/// Interface representing lazy computation
/// </summary>
/// <typeparam name="T">Element type</typeparam>
public interface ILazy<T>
{
    /// <summary>
    /// Function that calls the calculation and returns the result
    /// </summary>
    /// <returns>Returns the result of the called function</returns>
    T Get();
}
