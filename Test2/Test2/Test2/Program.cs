using System.Net;

if (args.Length > 2 || args.Length <= 0)
{
    return;
}

if (!int.TryParse(args[0], out int port))
{
    Console.WriteLine("incorrect port input");
    return;
}

if (port < 1024 || port > 65535)
{
    Console.WriteLine("incorrect port input");
    return;
}

if (args.Length == 1) {
    var server = new Server.Server(port);
    await Task.Run(() => server.Start());
}
else {

    if (!IPAddress.TryParse(args[1], out IPAddress? ip))
    {
        Console.WriteLine("incorrect ip address input");
        return;
    }

    var client = new Server.Client(ip!, port);
    await Task.Run(() => client.Start());
}