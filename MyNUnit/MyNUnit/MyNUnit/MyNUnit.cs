namespace MyNUnit;

using System.Diagnostics;
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
        var assemblyName = methodInfo.DeclaringType!.Assembly.GetName();
        var testAttribute = (TestAttribute)(methodInfo.GetCustomAttribute(typeof(TestAttribute))!);
        if (testAttribute.Ignore != null)
        {
            typeInfos.Add(new TestInfo(assemblyName.Name!, methodInfo.Name, TestInfo.TestStatus.Skipped, testAttribute.Ignore, null, 0));
            return;
        }

        Start<BeforeAttribute>(listOfMethodInfo);

        var typeObject = Activator.CreateInstance(methodInfo.DeclaringType!);
        var stopWatch = new Stopwatch();

        try
        {
            stopWatch.Start();
            methodInfo.Invoke(typeObject, null);
            stopWatch.Stop();
        }
        catch (Exception ex)
        {
            stopWatch.Stop();
            if (testAttribute.Expected == null)
            {
                typeInfos.Add(new TestInfo(assemblyName.Name!, methodInfo.Name, TestInfo.TestStatus.Failed, null, $"Expected: null, but was: {ex.InnerException!.GetType()}", stopWatch.ElapsedMilliseconds));
            }
            else if (testAttribute.Expected != ex.InnerException!.GetType())
            {
                typeInfos.Add(new TestInfo(assemblyName.Name!, methodInfo.Name, TestInfo.TestStatus.Failed, null, $"Expected: {testAttribute.Expected}, but was: {ex.InnerException!.GetType()}", stopWatch.ElapsedMilliseconds));
            }
            else
            {
                typeInfos.Add(new TestInfo(assemblyName.Name!, methodInfo.Name, TestInfo.TestStatus.Passed, null, null, stopWatch.ElapsedMilliseconds));
            }

            Start<AfterAttribute>(listOfMethodInfo);

            return;
        }

        if (testAttribute.Expected != null) {
            typeInfos.Add(new TestInfo(assemblyName.Name!, methodInfo.Name, TestInfo.TestStatus.Failed, null, $"Expected: {testAttribute.Expected}, but was: null", stopWatch.ElapsedMilliseconds));
        }
        else {
            typeInfos.Add(new TestInfo(assemblyName.Name!, methodInfo.Name, TestInfo.TestStatus.Passed, null, null, stopWatch.ElapsedMilliseconds));
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

    /// <summary>
    /// Method for running tests
    /// </summary>
    /// <param name="pathToDirectory">Path to directory containing .dll files</param>
    public static void RunForFiles(IEnumerable<string> files)
    {
        var types = files.Select(fileName => Assembly.LoadFrom(fileName)).SelectMany(assembly => assembly.ExportedTypes);
        Parallel.ForEach(types, classType => ParallelTestExecuteForEachType(classType));
    }

    public static void OutputInformationAboutTheTests()
    {
        foreach(var typeInfo in typeInfos) {
            Console.WriteLine();
            Console.WriteLine($"AsseblyName: {typeInfo.AssemblyName}");
            Console.WriteLine($"    Name: {typeInfo.Name}");
            Console.WriteLine($"        Status: {typeInfo.Status}");
            Console.WriteLine($"        Error message: {typeInfo.ErrorMessage}");
            Console.WriteLine($"        Ignore reason: {typeInfo.IgnoreReason}");
            Console.WriteLine($"        Time: {typeInfo.Time} ms");
        }
    }
}
