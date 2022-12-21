namespace MyNUnit;

/// <summary>
/// Сlass representing information about each test
/// </summary>
public class TestInfo : IEquatable<TestInfo>
{
    public enum TestStatus {
        Passed,
        Failed,
        Skipped
    }

    public string Name { get; }
    public TestStatus Status { get; }
    public string? IgnoreReason { get; }
    public string? ErrorMessage { get; }
    public long Time { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name">Test name</param>
    /// <param name="status">Test status</param>
    /// <param name="ignoreReason">Ignore reason</param>
    /// <param name="errorMessage">Error message</param>
    public TestInfo(string name, TestStatus status, string? ignoreReason, string? errorMessage, long time)
    {
        Name = name;
        Status = status;
        IgnoreReason = ignoreReason;
        ErrorMessage = errorMessage;
        Time = time;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (obj is not TestInfo objAsTestInfo)
        {
            return false;
        }
        else
        {
            return Equals(objAsTestInfo);
        }
    }

    public bool Equals(TestInfo? other)
    {
        if (other == null)
        {
            return false;
        }

        // Все свойства равны (кроме времени, так как один и тот же тест не пройдет за одно и тоже время)
        return Name == other.Name && Status == other.Status && IgnoreReason == other.IgnoreReason && ErrorMessage == other.ErrorMessage;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

public class TestInfoComparer : EqualityComparer<TestInfo>
{
    public override bool Equals(TestInfo? x, TestInfo? y)
    {
        return x!.Equals(y);
    }

    public override int GetHashCode(TestInfo obj)
    {
        throw new NotImplementedException();
    }
}
