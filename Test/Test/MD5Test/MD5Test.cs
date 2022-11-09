namespace MD5Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ShouldReultRemainTheSameForComputeCheckSumForDirectory()
    {
        var firstCompute = await Test.MD5.SequentiallyComputeCheckSumForDirectory("..//..//..//Azaza");
        var secondCompute = await Test.MD5.SequentiallyComputeCheckSumForDirectory("..//..//..//Azaza");
        Assert.That(firstCompute, Is.EquivalentTo(secondCompute));
    }

    [Test]
    public async Task ShouldReultRemainTheSameForComputeCheckSumForFile()
    {
        var firstCompute = await Test.MD5.ComputeCheckSumForFile("..//..//..//text.txt");
        var secondCompute = await Test.MD5.ComputeCheckSumForFile("..//..//..//text.txt");
        Assert.That(firstCompute, Is.EquivalentTo(secondCompute));
    }

    [Test]
    public void ShouldDirectoryNotFoundException()
    {
        Assert.ThrowsAsync<DirectoryNotFoundException>(() => Test.MD5.SequentiallyComputeCheckSumForDirectory("..//..//..//Ahaha"));
    }

    [Test]
    public void ShouldFileNotFoundException()
    {
        Assert.ThrowsAsync<FileNotFoundException>(() => Test.MD5.ComputeCheckSumForFile("Ahaha.txt"));
    }
}
