namespace Test2Tests;

using Server;
using System.Net;

public class Tests2Tests
{
    Server? server;
    CancellationTokenSource cancelTokenSource = new();

    [SetUp]
    public void Setup()
    {
        var server = new Server(10000);
        server.Start();
    }

    [Test]
    public  void Test1()
    {
        
    }
}