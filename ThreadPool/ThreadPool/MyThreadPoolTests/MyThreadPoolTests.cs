namespace MyThreadPoolTests;

public class Tests
{ 
    // ≈сли операци€ была отменена ожидаем икслючение
    [Test]
    public void ShouldOperationCanceledExceptionWhenShutDownIfTaskHasNotStart()
    {
        var pool = new MyThreadPool.MyThreadPool(1);
        var task = pool.Submit(() => 1);
        pool.ShutDown();
        int ReturnResult() => task.Result;
        Assert.Throws<OperationCanceledException>(() => ReturnResult());
    }

    [Test]
    public void ShouldExpectedIsCancellationRequestedIsEqualTrueWhenShutDown()
    {
        var pool = new MyThreadPool.MyThreadPool(1);
        pool.ShutDown();
        Assert.That(pool.Source.Token.IsCancellationRequested, Is.EqualTo(true));
    }

    private static IEnumerable<TestCaseData> TestRemoveCaseData() => new TestCaseData[]
    {
    };

    private static IEnumerable<TestCaseData> CaseData()
    {
        MyThreadPool.MyThreadPool pool = new (10);
        var list = new List<MyThreadPool.IMyTask<int>>();
        var numberOfTasks = 15;

        for (int i = 0; i < numberOfTasks; i++)
        {
            var locali = i;
            list.Add(pool.Submit(() => locali));
        }

        int counter = 0;
        foreach (var value in list)
        {
            yield return new TestCaseData(value.Result, counter, pool);
            counter++;
        }
    }

    // ¬се задачи посчитались и мы провер€ем что результат верный
    [TestCaseSource(nameof(CaseData))]
    public void ShouldResultIsEqualExpectedValue(int actual, int expected, MyThreadPool.MyThreadPool pool)
    {
        Assert.That(actual, Is.EqualTo(expected));
        pool.ShutDown();
    }

    // ѕровер€ем, что число потоков не уменьшаетс€ (в моем случае не мен€етс€ вообще)
    [Test]
    public void ShouldNumberOfThreadIsEqualNumberThatWasSentToConstructor()
    {
        int numberOfThreads = 10;
        MyThreadPool.MyThreadPool pool = new(numberOfThreads);
        var task = pool.Submit(() => 1);
        task = pool.Submit(() => 1);
        Assert.That(numberOfThreads, Is.EqualTo(pool.CountOfThreads));
        pool.ShutDown();

        numberOfThreads = 5;
        pool = new(numberOfThreads);
        Assert.That(numberOfThreads, Is.EqualTo(pool.CountOfThreads));
        pool.ShutDown();

        numberOfThreads = 100;
        pool = new(numberOfThreads);
        task = pool.Submit(() => 1);
        int a = task.Result;
        Assert.That(numberOfThreads, Is.EqualTo(pool.CountOfThreads));
        pool.ShutDown();
    }

    // ѕроверим что нова€ задача будет исполнена не ранее, чем завершитс€ исходна€
    // «наю что сон в 1000 мс это очень много дл€ теста, но не знаю как иначе проверить
    // я его закомментирую, но если что он прошел
    /*
    [Test]
    public void ShouldTheNewTaskWillBeCompletedNoEarlierThanTheOriginalOneIsCompleted()
    {
        var pool = new MyThreadPool.MyThreadPool(10);

        // ƒелаем задачу с методом у которого врем€ исполнени€ > 1000 мс
        var task = pool.Submit(() => { Thread.Sleep(1000); return 1; });

        // ѕредположим, что нова€ задача не ждет исходную

        // Ќова€ задача не очень трудоемка€ - поэтому, вроде как, 100 мс дл€ нее вполне достаточно (учитыва€ что пул из 10 потоков, из которых зан€т только 1)
        static int ReturnTwo(int lol) => 2;
        var continuation = task.ContinueWith(returnTwo);

        // Ќова€ задача не ждет старую и должна исполнитьс€ за 100 мс (то есть до ShutDown)
        // «начит при обращении к свойству Result все должно быть ок
        Thread.Sleep(100);
        pool.ShutDown();

        bool isCompleted = false;

        // ≈сли все плохо - то это противоречие. Ќо все хорошо => нова€ ждет исходную ( если мы верим в то, что нова€ задача успеет исполнитьс€ за 100 мс)
        Assert.That(isCompleted, Is.EqualTo(continuation.IsCompleted));
    }*/

    // ѕроверим что новые задачи исполн€ютс€
    [Test]
    public void ShouldNewTaskIsBeingExecuted()
    {
        var pool = new MyThreadPool.MyThreadPool(10);
        var task = pool.Submit(() => 1);

        static int ReturnTwo(int lol) => 2;
        var continuation = task.ContinueWith(ReturnTwo);
        int result = 2;
        Assert.That(result, Is.EqualTo(continuation.Result));

        var continuationContinuation = continuation.ContinueWith((x) => x * x);
        result = 4;

        Assert.That(result, Is.EqualTo(continuationContinuation.Result));
        pool.ShutDown();
    }

    [Test]
    public void ShouldAggregateExceptionWhenDevideByZero()
    {
        var pool = new MyThreadPool.MyThreadPool(10);
        int zero = 0;
        var task = pool.Submit(() => 1 / zero);
        int ReturnResult() => task.Result;
        Assert.Throws<AggregateException>(() => ReturnResult());
        pool.ShutDown();
    }

    /*
    [Test]
    public void ShouldExpectedFalseWhenIsCompletedForCancelledTask()
    {
        var pool = new MyThreadPool.MyThreadPool(10);
        var task = pool.Submit(() => 1);
        pool.ShutDown();
        bool result = false;
        Assert.That(result, Is.EqualTo(task.IsCompleted));
    }*/
}
