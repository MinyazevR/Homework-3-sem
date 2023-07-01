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
    public async Task ShouldExpectedThatMultipleClientsCanAccessTheFile()
    {
        var firstClient = new Client(IPAddress.Loopback, 10000);
        var secondClient = new Client(IPAddress.Loopback, 10000);
        var thirdClient = new Client(IPAddress.Loopback, 10000);
        var firstTask = firstClient.Get("..//..//..//Files//azaza.txt");
        var secondTask = secondClient.Get("..//..//..//Files//azaza.txt");
        var thirdTask = thirdClient.Get("..//..//..//Files//azaza.txt");
        var (firstSize, firstBytes) = await firstTask;
        var (secondSize, secondBytes) = await secondTask;
        var (thirdSize, thirdBytes) = await thirdTask;
        Assert.Multiple(() =>
        {
            Assert.That(firstSize, Is.EqualTo(secondSize));
            Assert.That(firstSize, Is.EqualTo(thirdSize));
            Assert.That(firstBytes, Is.EquivalentTo(secondBytes));
            Assert.That(firstBytes, Is.EquivalentTo(thirdBytes));
        });
    }
}
