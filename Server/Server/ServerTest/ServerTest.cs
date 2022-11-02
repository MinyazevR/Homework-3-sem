namespace ServerTest;

using System.Net;
using Server;

public class Tests
{
    Server? server;

    [SetUp]
    public void Setup()
    {
        server = new Server(IPAddress.Loopback, 10000);
        var cancelTokenSource = new CancellationTokenSource();
        var serverTask = Task.Run(() => server.Start(cancelTokenSource), cancelTokenSource.Token);
    }

    [Test]
    public void ShouldExpectedDirectoryNotFoundExceptionIfListForNonExistentDirectory()
    {
        var client = new Client(IPAddress.Loopback, 10000);
        Assert.ThrowsAsync<DirectoryNotFoundException>(() => client.List("./NonExistentDirectory"));
    }

    [Test]
    public void ShouldExpectedFileNotFoundExceptionIfGetForNonExistentFile()
    {
        var client = new Client(IPAddress.Loopback, 10000);
        Assert.ThrowsAsync<FileNotFoundException>(() => client.Get("NonExistentFile.txt"));
    }

    [Test]
    public async Task ShouldExpectedResultsOfGetAreTheSameForDifferentClients()
    {
        var client = new Client(IPAddress.Loopback, 10000);
        var newClient = new Client(IPAddress.Loopback, 10000);
        var(size, bytes) = await client.Get("..//..//..//Files//azaza.txt");
        var(newSize, newBytes) = await newClient.Get("..//..//..//Files//azaza.txt");
        Assert.Multiple(() =>
        {
            Assert.That(size, Is.EqualTo(newSize));
            Assert.That(bytes, Is.EqualTo(newBytes));
        });
    }
    
    [Test]
    public void ShouldExpectedThatMultipleClientsCanAccessTheFile()
    {
        var firstClient = new Client(IPAddress.Loopback, 10000);
        var secondClient = new Client(IPAddress.Loopback, 10000);
        var thirdClient = new Client(IPAddress.Loopback, 10000);
        var firstTask = Task.Run(() => firstClient.Get("..//..//..//Files//azaza.txt"));
        var secondTask = Task.Run(() => secondClient.Get("..//..//..//Files//azaza.txt"));
        var thirdTask = Task.Run(() => secondClient.Get("..//..//..//Files//azaza.txt"));
        var (firstSize, firstBytes) = firstTask.Result;
        var (secondSize, secondBytes) = secondTask.Result;
        var (thirdSize, thirdBytes) = thirdTask.Result;
        Assert.Multiple(() =>
        {
            Assert.That(firstSize, Is.EqualTo(secondSize));
            Assert.That(firstSize, Is.EqualTo(thirdSize));
            Assert.That(firstBytes, Is.EquivalentTo(secondBytes));
            Assert.That(firstBytes, Is.EquivalentTo(thirdBytes));
        });
    }
}
