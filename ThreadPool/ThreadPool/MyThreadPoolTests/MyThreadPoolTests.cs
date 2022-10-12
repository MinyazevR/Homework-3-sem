namespace MyThreadPoolTests;

public class Tests
{ 
    // ���� �������� ���� �������� ������� ����������
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

    // ��� ������ ����������� � �� ��������� ��� ��������� ������
    [TestCaseSource(nameof(CaseData))]
    public void ShouldResultIsEqualExpectedValue(int actual, int expected, MyThreadPool.MyThreadPool pool)
    {
        Assert.That(actual, Is.EqualTo(expected));
        pool.ShutDown();
    }

    // ���������, ��� ����� ������� �� ����������� (� ���� ������ �� �������� ������)
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

    // �������� ��� ����� ������ ����� ��������� �� �����, ��� ���������� ��������
    // ���� ��� ��� � 1000 �� ��� ����� ����� ��� �����, �� �� ���� ��� ����� ���������
    // � ��� �������������, �� ���� ��� �� ������
    /*
    [Test]
    public void ShouldTheNewTaskWillBeCompletedNoEarlierThanTheOriginalOneIsCompleted()
    {
        var pool = new MyThreadPool.MyThreadPool(10);

        // ������ ������ � ������� � �������� ����� ���������� > 1000 ��
        var task = pool.Submit(() => { Thread.Sleep(1000); return 1; });

        // �����������, ��� ����� ������ �� ���� ��������

        // ����� ������ �� ����� ���������� - �������, ����� ���, 100 �� ��� ��� ������ ���������� (�������� ��� ��� �� 10 �������, �� ������� ����� ������ 1)
        static int ReturnTwo(int lol) => 2;
        var continuation = task.ContinueWith(returnTwo);

        // ����� ������ �� ���� ������ � ������ ����������� �� 100 �� (�� ���� �� ShutDown)
        // ������ ��� ��������� � �������� Result ��� ������ ���� ��
        Thread.Sleep(100);
        pool.ShutDown();

        bool isCompleted = false;

        // ���� ��� ����� - �� ��� ������������. �� ��� ������ => ����� ���� �������� ( ���� �� ����� � ��, ��� ����� ������ ������ ����������� �� 100 ��)
        Assert.That(isCompleted, Is.EqualTo(continuation.IsCompleted));
    }*/

    // �������� ��� ����� ������ �����������
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
