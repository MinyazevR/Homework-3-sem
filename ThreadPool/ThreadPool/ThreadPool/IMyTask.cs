namespace MyThreadPool;

/// <summary>
/// Interface for tasks accepted for execution
/// </summary>
/// <typeparam name="TResult">Type of return value</typeparam>
public interface IMyTask<TResult>
{
    /// <summary>
    /// Returns true if the task is completed
    /// </summary>
    public bool IsCompleted { get; }

    /// <summary>
    /// Returns the result of the task execution
    /// </summary>
    public TResult Result { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNewResult"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
}
