namespace MyNUnitTests;

using MyNUnit;
using TestClass;

public class MyNUnitTest
{
    private readonly TestInfoComparer comparer = new ();
    [OneTimeSetUp]
    public void Setup()
    {
        MyNUnit.Run("../../../../TestClass");
    }

    [Test]
    public void ShouldTheAfterAndBeforeMethodsBeExecuted()
    {
        Assert.Multiple(() =>
        {
            Assert.That(ClassForTest.GetClassCounter, Is.EqualTo(0));
            Assert.That(ClassForTest.GetAfterMethodCounter, Is.EqualTo(2));
            Assert.That(ClassForTest.GetBeforeMethodCounter, Is.EqualTo(2));
        });
    }

    [Test]
    public void ShouldTestResultsCorrect()
    {
        Assert.Multiple(() =>
        {
            Assert.True(MyNUnit.typeInfos.Contains(new TestInfo("", "ExpectedDivideByZeroException", TestInfo.TestStatus.Passed, null, null, 0), comparer));
            Assert.True(MyNUnit.typeInfos.Contains(new TestInfo("", "NoExpectedDivideByZeroException", TestInfo.TestStatus.Failed, null, $"Expected: {typeof(DivideByZeroException)}, but was: {typeof(AppDomainUnloadedException)}", 0), comparer));
            Assert.True(MyNUnit.typeInfos.Contains(new TestInfo("", "IgnoreForBebraReasone", TestInfo.TestStatus.Skipped, "bebra", null, 0), comparer));
        });
    }
}