namespace MyThreadPool;

/// <summary>
/// Class for implementing a thread pool
/// </summary>
public class MyThreadPool : IDisposable
{
    // Queue for storing tasks in a pool
    private readonly Queue<Action> queue = new();

    // object for sending a cancellation signal to the CancellationToken token
    private readonly CancellationTokenSource source = new();

    /// <summary>
    /// Get CancellationTokenSource
    /// </summary>
    public CancellationTokenSource Source => source;

    private bool disposed;

    // List for storing pool threads
    private readonly List<Thread> threads = new();

    public int CountOfThreads => threads.Count;

    // Method that puts the method in the execution queue
    public void QueueWorkItem(Action func)
    {
        lock (queue)
        {
            // Добавляем метод в очередь
            queue.Enqueue(func);

            // Сообщаем потоку, ждущему на Monitor.Wait, что нужно выполнить метод
            Monitor.Pulse(queue);
        }
    }

    // Method that puts the method in the execution queue
    public void QueueForTaskItem<TResult>(MyTask<TResult> task)
    {
        QueueWorkItem(task.Work);
    }

    /// <summary>
    /// Method for returning a task to be accepted for execution.
    /// </summary>
    /// <typeparam name="TResult">Type of return value</typeparam>
    /// <param name="func">Method</param>
    /// <returns>Task presented as an IMyTask<TResult> interface</returns>
    public IMyTask<TResult> Submit<TResult>(Func<TResult> func)
    {
        // Создаем объект класса MyTask
        var task = new MyTask<TResult>(func, this);

        // Ставим задачу на исполнение в пул потоков
        QueueWorkItem(task.Work);

        return task;
    }

    /// <summary>
    /// Constructor of the MyThreadPool class
    /// </summary>
    /// <param name="n">Number of threads in the pool</param>
    public MyThreadPool(int n)
    {
        for (int i = 0; i < n; i++)
        {
            threads.Add(new Thread(() =>
            {
                Action? result;
                // Пока операция не была отменена - выполняем работу
                while (!source.Token.IsCancellationRequested)
                {
                    lock (queue)
                    {
                        while (queue.Count == 0 && !source.Token.IsCancellationRequested)
                        {
                            // Ждем пока очередь станет непустой или операция будет отменена
                            Monitor.Wait(queue);
                        }

                        // Берем из очереди метод соответствующий задаче
                        queue.TryDequeue(out result);
                    }

                    if (result != null)
                    {
                        // Исполняем метод
                        result!();
                    }
                }
            }));
        }

        // При создании объекта в нем должно начать работу n потоков
        foreach(var thread in threads)
        {
            thread.Start();
        }
    }

    /// <summary>
    /// Method for shutting down threads
    /// </summary>
    public void ShutDown()
    {
        lock(queue)
        {
            // Должны вернуть управление когда остановятся все потоки
            // Хотим оставноить рабочий поток
            source.Cancel();

            // Сообщаем об этом потоку, который может ждать на Monitor.Wait отмены операции
            Monitor.PulseAll(queue);
        }

        
        // Блокируем поток до завершения потоков
        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            this.ShutDown();
        }

        disposed = true;
    }

    ~MyThreadPool()
    {
        Dispose(false);
    }
}
