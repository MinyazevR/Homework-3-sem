namespace LazyTest;

public class LazyTests
{
    private static IEnumerable<TestCaseData> CaseData()
    {
        var values = new List<int> { 0, -1, 1 };
        int number = 0;

        static int Increment(int number) => number + 1;

        yield return new TestCaseData(new Lazy.SingleThreadedLazy<int>(() => Increment(number)), 1);

        foreach (var value in values)
        {
            yield return new TestCaseData(new Lazy.SingleThreadedLazy<int>(() => value), value);
            yield return new TestCaseData(new Lazy.MultithreadedLazy<int>(() => value), value);
        }
    }

    [TestCaseSource(nameof(CaseData))]
    public void ShouldValueNotChangeForLazyInSinglethreadedMode(Lazy.ILazy<int> lazy, int expectedValue)
    {
        for (int i = 0; i < 10; i++)
        {
            Assert.That(lazy.Get(), Is.EqualTo(expectedValue));
        }
    }

    [Test]
    public void ShouldValueNotChangeForLazyInMultithreadedMode()
    {
        static int Increment(int number) => number + 1;
        Lazy.ILazy<int> lazy = new Lazy.MultithreadedLazy<int>(() => Increment(0));
        Thread[] threads = new Thread[8];

        for (int i = 0; i < 8; i++)
        {
            threads[i] = new Thread(() => {
                Assert.That(lazy.Get(), Is.EqualTo(1));
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }
}