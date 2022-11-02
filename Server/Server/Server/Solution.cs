using System.Net;

Console.WriteLine("Формат ввода через командную строку");
Console.WriteLine("string: IPaddress");
Console.WriteLine("int: IPaddress");
Console.WriteLine($"get string :path to file");
Console.WriteLine($"list string :path to directory");
Console.WriteLine($"Например: {"127.0.0.1"} {"80"} {"list ./File"} {"get ./File/File.txt"}");

if (args.Length < 3)
{
    return;
}

if (!IPAddress.TryParse(args[0], out IPAddress? ip))
{
    Console.WriteLine("incorrect ip address input");
    return;
}

if (!int.TryParse(args[1], out int port))
{
    Console.WriteLine("incorrect port input");
    return;
}

var server = new Server.Server(ip!, port);
var cancelTokenSource = new CancellationTokenSource();
var serverTask = Task.Run(() => server.Start(cancelTokenSource), cancelTokenSource.Token);
var client = new Server.Client(ip!, port);

for (int i = 2; i < args.Length; i++)
{
    switch(args[i])
    {
        case "list":
        {
            var (size, names) = await Task.Run(() => client.List(args[i + 1]));
            Console.WriteLine($"size : {size}");
            for (int j = 0; j < names.Count; j++)
            {
                Console.Write(names[j]);
                Console.WriteLine();
            }
            i++;
            break;
        }
        case "get":
        {
            var(size, bytes) = await Task.Run(() => client.Get(args[i + 1]));
            Console.WriteLine($"size : {size}");
            for (int j = 0; j < bytes.Length; j++)
            {
                Console.Write(bytes[j]);
            }
            Console.WriteLine();
            i++;
            break;
        }
        default:
        {
            i++;
            continue;
        }
    }
}