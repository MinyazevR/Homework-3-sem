using MyAttributes;

namespace TestClass;

public class ClassForTest
{

    private static volatile int classCounter;
    private static volatile int afterMethodCounter;
    private static volatile int beforeMethodCounter;

    public static int GetClassCounter => classCounter;
    public static int GetAfterMethodCounter => afterMethodCounter;
    public static int GetBeforeMethodCounter => beforeMethodCounter;

    [AfterClass]
    public void Increment()
    {
        classCounter++;
    }

    [AfterClass]
    public void SecondIncrement()
    {
        classCounter++;
    }

    [BeforeClass]
    public void Decrement()
    {
        classCounter--;
    }

    [BeforeClass]
    public void SecondDecrement()
    {
        classCounter--;
    }

    [After]
    public void AfterTest()
    {
        afterMethodCounter++;
    }

    [Before]
    public void BeforeTest()
    {
        beforeMethodCounter++;
    }

    [Test(typeof(DivideByZeroException), null)]
    public void ExpectedDivideByZeroException()
    {
        throw new DivideByZeroException();
    }

    [Test(typeof(DivideByZeroException), null)]
    public void NoExpectedDivideByZeroException()
    {
        throw new AppDomainUnloadedException();
    }

    [Test(null, "bebra")]
    public void IgnoreForBebraReasone()
    {

    }
}
