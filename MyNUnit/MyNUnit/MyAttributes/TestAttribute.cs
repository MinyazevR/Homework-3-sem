namespace MyAttributes;

[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute
{
    public TestAttribute(Type? expected, string? ignore)
    {
        Expected = expected;
        Ignore = ignore;
    }

    public Type? Expected { get; set; }
    public string? Ignore { get; set; }
}
