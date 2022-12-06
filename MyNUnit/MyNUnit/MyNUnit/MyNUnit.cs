namespace MyNUnit;

using MyAttributes;
using System.Reflection;
using System.Collections.Concurrent;


/// <summary>
/// Сlass representing a system for running tests
/// </summary>
public class MyNUnit
{
    public static readonly BlockingCollection<TestInfo> typeInfos = new ();

    private static void TestAttributeExecute((MethodInfo, IEnumerable<MethodInfo>) methodInfoPair)
    {
        var (methodInfo, listOfMethodInfo) = methodInfoPair;

        var testAttribute = (TestAttribute)(methodInfo.GetCustomAttribute(typeof(TestAttribute))!);
        if (testAttribute.Ignore != null)
        {
            typeInfos.Add(new TestInfo(methodInfo.Name, TestInfo.TestStatus.Skipped, testAttribute.Ignore, null));
            return;
        }

        Start<BeforeAttribute>(listOfMethodInfo);

        var typeObject = Activator.CreateInstance(methodInfo.DeclaringType!);

        try
        {
            methodInfo.Invoke(typeObject, null);
        }
        catch (Exception ex)
        {
            if (testAttribute.Expected == null)
            {
                typeInfos.Add(new TestInfo(methodInfo.Name, TestInfo.TestStatus.Failed, null, $"Expected: null, but was: {ex.InnerException!.GetType()}"));
            }
            else if (testAttribute.Expected != ex.InnerException!.GetType())
            {
                typeInfos.Add(new TestInfo(methodInfo.Name, TestInfo.TestStatus.Failed, null, $"Expected: {testAttribute.Expected}, but was: {ex.InnerException!.GetType()}"));
            }
            else
            {
                typeInfos.Add(new TestInfo(methodInfo.Name, TestInfo.TestStatus.Passed, null, null));
            }

            Start<AfterAttribute>(listOfMethodInfo);
            return;
        }

        if (testAttribute.Expected != null) {
            typeInfos.Add(new TestInfo(methodInfo.Name, TestInfo.TestStatus.Failed, null, $"Expected: {testAttribute.Expected}, but was: null"));
        }
        else {
            typeInfos.Add(new TestInfo(methodInfo.Name, TestInfo.TestStatus.Passed, null, null));
        }
        Start<AfterAttribute>(listOfMethodInfo);
    }

    private static void AnotherAttributeExecute((MethodInfo, IEnumerable<MethodInfo>) methodInfoPair) {
        var (methodInfo, _) = methodInfoPair;
        var typeObject = Activator.CreateInstance(methodInfo.DeclaringType!);
        methodInfo.Invoke(typeObject, null);
    }

    private static void Start<AttributeType>(IEnumerable<MethodInfo> methodInfos)
    {
        var methodWithAttribute = methodInfos.Where(methodInfo => Attribute.IsDefined(methodInfo, typeof(AttributeType)));
        Action<(MethodInfo, IEnumerable<MethodInfo>)> actionDependentTheAttributeType = x => { };
        if (typeof(AttributeType) == typeof(TestAttribute))
        {
            actionDependentTheAttributeType = TestAttributeExecute;
        }
        else if (MyAttributesInfo.Exist(typeof(AttributeType)))
        {
            actionDependentTheAttributeType = AnotherAttributeExecute;
        }
        else
        {
            throw new ArgumentException("Attribute of this type not exist");
        }

        Parallel.ForEach(methodWithAttribute, method => actionDependentTheAttributeType((method, methodInfos)));
    }

    private static void ParallelTestExecuteForEachType(Type type)
    {
        Start<BeforeClassAttribute>(type.GetTypeInfo().DeclaredMethods);
        Start<TestAttribute>(type.GetTypeInfo().DeclaredMethods);
        Start<AfterClassAttribute>(type.GetTypeInfo().DeclaredMethods);
    }

    /// <summary>
    /// Method for running tests
    /// </summary>
    /// <param name="pathToDirectory">Path to directory containing .dll files</param>
    public static void Run(string pathToDirectory) {
        var types = Directory.EnumerateFiles(pathToDirectory, "*.dll").Select(fileName => Assembly.LoadFrom(fileName)).SelectMany(assembly => assembly.ExportedTypes);
        Parallel.ForEach(types, classType => ParallelTestExecuteForEachType(classType));
    }

    public static void OutputInformationAboutTheTests()
    {
        foreach(var typeInfo in typeInfos) {
            Console.WriteLine();
            Console.WriteLine($"Name: {typeInfo.Name}");
            Console.WriteLine($"     Status :{typeInfo.Status}");
            Console.WriteLine($"     Error message :{typeInfo.ErrorMessage}");
        }
    }
}
