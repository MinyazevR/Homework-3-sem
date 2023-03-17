namespace MyThreadPool;

using System.Collections.Concurrent;

public class MyTask<TResult> : IMyTask<TResult>
{
    private readonly Func<TResult> function;

    private readonly Object lockObject = new();

    private Exception? exception = null;

    private readonly ConcurrentQueue<Action> queueOfTasksToComplete = new();

    private volatile bool isCompleted;

    private TResult? result;

    private readonly MyThreadPool threadPool;

    public MyTask(Func<TResult> function, MyThreadPool threadPool)
    {
        this.function = function;
        this.threadPool = threadPool;
    }

    /// <inheritdoc/>
    public bool IsCompleted => isCompleted;

    public void Work()
    {
        if (threadPool.Source.Token.IsCancellationRequested)
        {
            lock (lockObject)
            {
                if (threadPool.Source.Token.IsCancellationRequested)
                {
                    Monitor.Pulse(lockObject);
                }
            }

            return;
        }

        try
        {
            result = function();
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        finally
        {
            isCompleted = true;
        }

        lock (lockObject)
        {
            // Сообщаем потоку, ждущему на Monitor.Wait, что задача вполнена
            Monitor.Pulse(lockObject);
            queueOfTasksToComplete.TryDequeue(out Action? result);
            if (result != null)
            {
                threadPool.QueueWorkItem(result!);
            }
        }
    }

    /// <inheritdoc/>
    public TResult Result
    {
        get
        {

            if (isCompleted)
            {
                // Если соответствующий задаче метод завершился с исключением, то бросаем AggregateException
                if (exception != null)
                {
                    throw new AggregateException(exception);
                }

                return result!;
            }

            threadPool.Source.Token.ThrowIfCancellationRequested();

            // Если результат еще не посчитан
            if (!isCompleted)
            {
                lock (lockObject)
                {
                    // Ждём когда рузльтат будет вычислен, блокируя при этом поток
                    while (!threadPool.Source.Token.IsCancellationRequested && !IsCompleted)
                    {
                        Monitor.Wait(lockObject);
                    }

                    threadPool.Source.Token.ThrowIfCancellationRequested();
                }
            }

            // Если соответствующий задаче метод завершился с исключением, то бросаем AggregateException
            if (exception != null)
            {
                throw new AggregateException(exception);
            }

            return result!;
        }

        private set { }
    }

    /// <inheritdoc/>
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
    {
        var task = new MyTask<TNewResult>(() => func(Result), threadPool);
        if (isCompleted)
        {
            threadPool.QueueForTaskItem(task);
            return task;
        }

        lock (lockObject)
        {
            if (isCompleted)
            {
                threadPool.QueueForTaskItem(task);
                return task;
            }
            else
            {
                // выгружаем в очередь
                queueOfTasksToComplete.Enqueue(task.Work);
            }
        }

        return task;
    }
}